(function (mediaPlayer) {
    Math.clamp = function (value, min, max) {
        return Math.min(max, Math.max(min, value));
    };

    function distanceToSegmentSquared(p, v, w) {
        function sqr(x) { return x * x }
        function dist2(v, w) { return sqr(v.x - w.x) + sqr(v.y - w.y) }

        var l2 = dist2(v, w);
        if (l2 == 0)
            return dist2(p, v);

        var t = ((p.x - v.x) * (w.x - v.x) + (p.y - v.y) * (w.y - v.y)) / l2;

        if (t < 0)
            return dist2(p, v);

        if (t > 1)
            return dist2(p, w);

        return dist2(p, { x: v.x + t * (w.x - v.x), y: v.y + t * (w.y - v.y) });
    };

    var AreaShape = function (area, paper) {
        this.area = area;
        this.paper = paper;

        this.points = [];

        // track last dx and dy values
        this.odx = 0;
        this.ody = 0;

        // attach handlers
        area.drag(this.onDragMove.bind(this), this.onDragStart.bind(this), null);
        area.click(function (e) {
            this.focus();

            if (e.altKey) {
                this.createHandleAtPoint(e.offsetX, e.offsetY);
            }
        }.bind(this));
    };

    AreaShape.prototype.createHandle = function (point) {
        this.points.push(point);

        new ShapeHandle(point, this);
    };

    AreaShape.prototype.onDragStart = function () {
        this.odx = 0;
        this.ody = 0;
    };

    AreaShape.prototype.onDragMove = function (dx, dy) {
        var self = this,
            tx = dx - this.odx,
            ty = dy - this.ody,
            paper = this.area.paper,
            translateX = true,
            translateY = true;

        // Check if all points will end up within canvas boundaries
        this.points.forEach(function (point) {
            var ncx = point.attr("cx") + tx,
                ncy = point.attr("cy") + ty;

            translateX &= ncx > 0 && ncx < self.paper.width;
            translateY &= ncy > 0 && ncy < self.paper.height;
        });

        // Apply translation to all points
        if (translateX || translateY) {
            this.points.forEach(function (point) {
                translateX && point.attr("cx", point.attr("cx") + tx);
                translateY && point.attr("cy", point.attr("cy") + ty);
            });

            if (translateX) {
                this.odx = dx;
            }

            if (translateY) {
                this.ody = dy;
            }

            this.redraw(this.area, this.points);
        }
    };

    AreaShape.prototype.redraw = function () {
        var pathValue = this.points.map(function (point) {
            return point.attr("cx") + "," + point.attr("cy");
        }).join(" L ");

        this.area.attr("path", "M " + pathValue + " Z");
    };

    AreaShape.prototype.focus = function () {
        this.area.toFront();

        this.points.forEach(function (point) {
            point.toFront();
        });
    };

    AreaShape.prototype.remove = function () {
        this.area.remove();

        this.points.forEach(function (point) {
            point.remove();
        });
    };

    AreaShape.prototype.createHandleAtPoint = function (x, y) {
        // Get list of vectors that form the shape        
        var lastPoint = null;
        var shapeVectors = this.points.map(function (point) {
            var last = lastPoint;
            lastPoint = point;

            if (last) {
                return { a: last, b: point };
            }
        });

        shapeVectors.push({ a: lastPoint, b: this.points[0] });

        // Calculate lowest divergence of edges with added node
        var lowestDistance = 0,
            lowestDistanceVector = null;

        shapeVectors.forEach(function (vector) {
            if (!vector) {
                return;
            }

            var distance = distanceToSegmentSquared({ x: x, y: y }, { x: vector.a.attr("cx"), y: vector.a.attr("cy") }, { x: vector.b.attr("cx"), y: vector.b.attr("cy") });

            if (!lowestDistanceVector || lowestDistance > distance) {
                lowestDistance = distance;
                lowestDistanceVector = vector;
            }
        });

        // Add point to collection and redraw shape
        var newPoint = this.paper.circle(x, y, 10),
            newPointIndex = this.points.indexOf(lowestDistanceVector.b);

        newPoint.attr("fill", this.points[0].attr("fill"));
        newPoint.attr("stroke", 0);

        this.points.splice(newPointIndex, 0, newPoint);
        this.redraw();

        //new ShapeHandle(newPoint, this);
    };


    var ShapeHandle = function (point, shape) {
        this.point = point;
        this.shape = shape;

        point.drag(this.onDragMove.bind(this), this.onDragStart.bind(this), null);
        point.click(function (e) {
            if (e.ctrlKey && this.shape.points.length > 3) {
                this.shape.points.splice(this.shape.points.indexOf(this.point), 1);
                this.point.remove();
                this.shape.redraw();
            }
        }.bind(this));
    };

    ShapeHandle.prototype.onDragStart = function () {
        this.ocx = this.point.attr('cx');
        this.ocy = this.point.attr('cy');
    };

    ShapeHandle.prototype.onDragMove = function (dx, dy) {
        this.point.attr({
            cx: Math.clamp(this.ocx + dx, 0, this.shape.paper.width),
            cy: Math.clamp(this.ocy + dy, 0, this.shape.paper.height)
        });

        this.shape.redraw();
    };

    mediaPlayer.plugin('areas', function (options) {
        var player = this,
            playerContainer = player.el();

        // Create drawing area
        var paper = null;

        player.addArea = function (rgb, x, y, dx, dy) {
            // Add area path and circle handles
            var path = paper.path();
            path.attr("fill", "rgba(" + rgb.join(",") + ",.1)");
            path.attr("stroke", "rgba(" + rgb.join(",") + ",.8)");
            path.attr("stroke-width", 3);

            var shape = new AreaShape(path, paper)
            paper.add(path);

            var w = playerContainer.clientWidth;
            var h = playerContainer.clientHeight;

            var location = [[w * x, h * y], [w * x + w * dx, h * y], [w * x + w * dx, h * y + h * dy], [w * x, h * y + h * dy]];


            for (var i = 0; i < 4; i++) {
                var point = paper.circle(
                    location[i][0],
                    location[i][1],
                    1
                    //playerContainer.clientWidth / 2 + ((i % 2) * 2 - 1) * (Math.floor(i / 2) * -2 + 1) * 20, 
                    //playerContainer.clientHeight / 2 + (Math.floor(i / 2) * 2 - 1) * 20,
                    //1


                  );

                point.attr("fill", "rgb(" + rgb.join(",") + ")");
                point.attr("stroke", 0);

                shape.createHandle(point);
                paper.add(point);
            }

            shape.redraw();

            return shape;
        };

        // Main function
        player.ready(function () {
            paper = Raphael(playerContainer, playerContainer.clientWidth, playerContainer.clientHeight);
            player.addEventListener("fullscreenchange", function () {
                //paper = Raphael(playerContainer, playerContainer.clientWidth, playerContainer.clientHeight);
                paper.setSize(playerContainer.clientWidth, playerContainer.clientHeight);
            });

        });
    });
}(window.amp));