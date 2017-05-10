


// Functions -------------------------------------------------------------------------------------//
function Trim(s) {
    return s.replace(/^\s+|\s+$/g, "");
}
//  this disables and grays out the demo portions except for loading a video
function DisableDemoExceptLoadVideo() {
    $(".grayNoVideo a, .grayNoVideo button, .grayNoVideo input, .grayNoVideo textarea").prop("disabled", true);
    $(".grayNoVideo, .grayNoVideo a").css("color", "rgb(153,153,153)");
}

//  this enables the demo after a successful video load
function EnableDemoAfterLoadVideo() {
    $(".grayNoVideo, .grayNoVideo a").removeAttr("style");
    $(".grayNoVideo a, .grayNoVideo button, .grayNoVideo input, .grayNoVideo textarea").prop("disabled", false);
    //$("#pauseButton, #saveCaptionAndPlay, #justSaveCaption").prop("disabled", true); // these are still disabled
    $("#textCaptionEntry").prop("readonly", true);
}
//  this resets our state on a video load
function ResetState() {
    captionsArray.length = 0;
    autoPauseAtTime = -1;
    var tag = GetMediaTag();
    if (tag != undefined) {
        tag.each(function () {
            $(this).get(0).pause();
            $(this).attr("src", "");
        });
    }
    $("#display div").remove();
    $("textarea, #ttURL, #ttFile").val("");
    $("#captionFormatNone").prop("checked", true);
    $("#textCaptionEntry").val("").prop("readonly", true).removeClass("playing");
    $("#captionTitle").html("&nbsp;");
    DisableDemoExceptLoadVideo();
}

function LoadVideoSource(url, subarray,time) {
    var subtitleUrl = undefined;
    if (subarray.length > 0)
        subtitleUrl = subarray[0].src;
    if ((subtitleUrl != undefined) && (subtitleUrl != ""))
        LoadSubtitle(subtitleUrl);


    mimeType = "application/vnd.ms-sstr+xml";
    if ((url.trim().toLowerCase().match('.ism/manifest')) || (url.trim().toLowerCase().match('.isml/manifest'))) {
    } else if (url.toLowerCase().match('.mpd$')) {
        mimeType = "application/dash+xml";
    } else if (url.toLowerCase().match('.flv$')) {
        mimeType = "video/x-flv";
    } else if (url.toLowerCase().match('.ogv$')) {
        mimeType = "video/ogg";
    } else if (url.toLowerCase().match('.webm$')) {
        mimeType = "video/webm";
    } else if (url.toLowerCase().match('.3gp$')) {
        mimeType = "video/3gp";
    } else if (url.toLowerCase().match('.mp4')) {
        mimeType = "video/mp4";
    } else if (url.toLowerCase().match('.mp3')) {
        mimeType = "audio/mp3";
    }
    myPlayer.src(
    [
        { src: url, type: mimeType }
    ],
    subarray
    );
    myPlayer.play();
    if (time != 0) {
        control = GetMediaControl()
        if(control != undefined)
            control.currentTime = time;
    }
}
function httpGet(theUrl) {
    if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp = new XMLHttpRequest();
    }
    else {// code for IE6, IE5
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    }
    xmlhttp.onreadystatechange = function () {
        if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
            return xmlhttp.responseText;
        }
    }
    xmlhttp.open("GET", theUrl, false);
    xmlhttp.send();
}
function httpGetAsync(theUrl, callback) {
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.onreadystatechange = function () {
        if (xmlHttp.readyState == 4 && xmlHttp.status == 200)
            callback(xmlHttp.responseText);
    }
    xmlHttp.open("GET", theUrl, true); // true for asynchronous 
    xmlHttp.send(null);
}
function LoadSubtitle(url) {

   
    $.get(url, function (data) {
        var s = url.toLowerCase();
        if (s.indexOf("vtt") > 0)
            ParseAndLoadWebVTT(data);
        else if (s.indexOf("ttml") > 0) {
            var xmlString = (new XMLSerializer()).serializeToString(data);
            ParseAndLoadTTML(xmlString);
        }
    });
    
    /*
    var s = document.createElement("script");
    s.setAttribute("type", "text/javascript");
    s.setAttribute("src", "http://www.baccano.com/proxyvtt.ashx?vtt=" + url);
    document.body.appendChild(s);
    */
}
//  sets the video source to the URL s
function LoadAudioSource(url, subarray,time) {


    if (!errorHandlerBound) {
        $("#audioElm").bind("error", function (event) {
            var vh = $(this).height();

            // error handling straight from the HTML5 video spec (http://dev.w3.org/html5/spec-author-view/video.html)
            switch (event.originalEvent.target.error.code) {
                case event.originalEvent.target.error.MEDIA_ERR_ABORTED:
                    $("#videoError").text("You aborted the video playback.");
                    break;
                case event.originalEvent.target.error.MEDIA_ERR_NETWORK:
                    $("#videoError").text("A network error caused the video download to fail part-way.");
                    break;
                case event.originalEvent.target.error.MEDIA_ERR_DECODE:
                    $("#videoError").text("The video playback was aborted due to a corruption problem or because the video used features your browser did not support.");
                    break;
                case event.originalEvent.target.error.MEDIA_ERR_SRC_NOT_SUPPORTED:
                    $("#videoError").text("The video could not be loaded, either because the server or network failed or because the format is not supported.");
                    break;
                default:
                    $("#videoError").text("An unknown error occurred.");
                    break;
            }

            $("#videoError").height(vh).css("display", "block");
            $(this).css("display", "none");
        });

        errorHandlerBound = true;
    }
    url = Trim(url);
    if (url != "") {
        $("#videoError").css("display", "none");
        var ctrl = $("#audioElm").css("display", "block").attr("src", url).prop("controls", true).get(0);
        if (ctrl !== undefined) {
            ctrl.load();
            ctrl.play();
            if (subarray.length > 0)
                LoadSubtitle(subarray[0].src);
            if (time != 0)
                ctrl.currentTime = time;
        }

    }
}
function StopAllMediaSources() {
    if($('video').get(0) !== undefined)
        $('video').get(0).pause();
    if ($('audio').get(0) !== undefined)
        $('audio').get(0).pause();
}
function StopMediaSource() {
    var control = GetMediaControl();
    if (control != undefined) {
        control.pause();
    }
}
// Retrieve paramters from the url
function getParamFromUrl(url, parm) {
    var re = new RegExp(".*[?&]" + parm + "=([^&]+)(&|$)");
    var match = url.match(re);
    return (match ? match[1] : undefined);
}
function AdjustHeight() {
    // Get height of original column you want to match
    var boxheight = $('#div1').outerHeight();
    // Output same height on particular element or element(s)
    $('#subtitleplayer').height(boxheight);
    $('#display').height(boxheight - 40);
}
// Retrieve paramters from the url
function LaunchPlayer(video, audio, subtitleurlarray, time) {
    //  delete any captions we've got
    ResetState();


    if ((audio != undefined) && (audio != "")) {
        if ($("#videoContainer") != undefined)
            $("#videoContainer").hide();
        if ($("#audioplayer") != undefined)
            $("#audioplayer").show();
        StopAllMediaSources();
        LoadAudioSource(audio, subtitleurlarray,time);
    }
    else if ((video != undefined) && (video != "")) {
        if ($("#videoContainer") != undefined)
            $("#videoContainer").show();
        if ($("#audioplayer") != undefined)
            $("#audioplayer").hide();
        StopAllMediaSources();
        LoadVideoSource(video, subtitleurlarray,time);
    }
    $("#captionFormatTTML").prop("checked", false);
    
    $("#captionFormatVTT").prop("checked", true);
}
$("#loadButton").click(function () {
    v = $("#inputVideoUrl").val();
    a = $("#inputAudioUrl").val();
    s = $("#inputSubtitleUrl").val();
    l = $("#inputSubtitleLanguage").val();
    d = $("#inputSubtitleLabel").val();

    array = [];
    if ((s != undefined) && (s != "")) {
        if ((l === undefined) || (l == ""))
            l = "unk";
        if ((d === undefined) || (d == ""))
            d = "unknown";
        array = [
         { src: s, srclang: l, kind: "subtitles", label: d }
        ];
    }
    LaunchPlayer(v, a, array,0);
});


function FindCaptionIndexLinearSearch(seconds) {
    if (captionsArray.length < 1)
        return -1;

    //	linear search isn't optimal but it's safe
    for (var i = 0; i < captionsArray.length; ++i) {
        if (captionsArray[i].start <= seconds && seconds < captionsArray[i].end)
            return i;
    }

    return -1;
}

function FindCaptionIndex(seconds) {
    var below = -1;
    var above = captionsArray.length;
    var i = Math.floor((below + above) / 2);

    while (below < i && i < above) {

        if (captionsArray[i].start <= seconds && seconds < captionsArray[i].end)
            return i;

        if (seconds < captionsArray[i].start) {
            above = i;
        } else {
            below = i;
        }

        i = Math.floor((below + above) / 2);
    }

    return -1;
}
//	parses webvtt time string format into floating point seconds
function ParseTime(sTime) {

    //  parse time formatted as hours:mm:ss.sss where hours are optional
    if (sTime) {
        var m = sTime.match(/^\s*(\d+)?:?(\d+):([\d\.]+)\s*$/);
        if (m != null) {
            return (m[1] ? parseFloat(m[1]) : 0) * 3600 + parseFloat(m[2]) * 60 + parseFloat(m[3]);
        } else {
            m = sTime.match(/^\s*(\d{2}):(\d{2}):(\d{2}):(\d{2})\s*$/);
            if (m != null) {
                var seconds = parseFloat(m[1]) * 3600 + parseFloat(m[2]) * 60 + parseFloat(m[3]) + parseFloat(m[4]) / 30;
                return seconds;
            }
        }
    }

    return 0;
}

//	formats floating point seconds into the webvtt time string format
function FormatTime(seconds) {
    var hh = Math.floor(seconds / (60 * 60));
    var mm = Math.floor(seconds / 60) % 60;
    var ss = seconds % 60;

    return (hh == 0 ? "" : (hh < 10 ? "0" : "") + hh.toString() + ":") + (mm < 10 ? "0" : "") + mm.toString() + ":" + (ss < 10 ? "0" : "") + ss.toFixed(3);
}

function DisplayExistingCaption(seconds) {
    var ci = FindCaptionIndex(seconds);
    if (ci != captionBeingDisplayed) {
        captionBeingDisplayed = ci;
        if (ci != -1) {
            var theCaption = captionsArray[ci];
            $("#captionTitle").text("Caption for segment from " + FormatTime(theCaption.start) + " to " + FormatTime(theCaption.end) + ":");
            $("#textCaptionEntry").val(theCaption.caption);
        } else {
            $("#captionTitle").html("&nbsp;");
            $("#textCaptionEntry").val("");
        }
    }
}

function existingCaptionsEndTime() {
    return captionsArray.length > 0 ? captionsArray[captionsArray.length - 1].end : 0;
}

function mediaPlayEventHandler() {
    captionBeingDisplayed = -1;

    //  give Opera a beat before doing this
    window.setTimeout(function () {
        $("#textCaptionEntry").val("").prop("readonly", true).addClass("playing");
        $("#pauseButton").prop("disabled", false);
        $("#playButton, #justSaveCaption, #saveCaptionAndPlay").prop("disabled", true);
    }, 16);
}
function mediaPauseEventHandler() {
    $("#playButton, #justSaveCaption, #saveCaptionAndPlay").prop("disabled", false);
    $("#textCaptionEntry").removeClass("playing").prop("readonly", false);
    $("#pauseButton").prop("disabled", true);
    var tag = GetMediaTag();
    if (tag !== undefined) {
        var playTime = tag.prop("currentTime");
        var captionsEndTime = existingCaptionsEndTime();
        if (playTime - 1 < captionsEndTime) {
            var ci = FindCaptionIndex(playTime - 1);
            if (ci != -1) {
                var theCaption = captionsArray[ci];
                $("#captionTitle").text("Edit caption for segment from " + FormatTime(theCaption.start) + " to " + FormatTime(theCaption.end) + ":");
                $("#textCaptionEntry").val(theCaption.caption);
                captionBeingDisplayed = ci;
            } else {
                $("#captionTitle").text("No caption at this time code.");
                $("#textCaptionEntry").val("");
                captionBeingDisplayed = -1;
            }
        } else {
            $("#captionTitle").text("Enter caption for segment from " + FormatTime(existingCaptionsEndTime()) + " to " + FormatTime(playTime) + ":");
            $("#textCaptionEntry").val("");
            captionBeingDisplayed = -1;
        }
    }
    $("#textCaptionEntry").focus().get(0).setSelectionRange(1000, 1000); // set focus and selection point to end
}


function mediaTimeUpdateEventHandler() {
    var tag = GetMediaTag();
    if (tag !== undefined) {
        var playTime = tag.prop("currentTime");

        if (autoPauseAtTime >= 0 && playTime >= autoPauseAtTime) {
            autoPauseAtTime = -1;
            tag.get(0).pause();
            return;
        }

        var captionsEndTime = existingCaptionsEndTime();
        if (playTime < captionsEndTime) {
            DisplayExistingCaption(playTime);
        } else {
            $("#captionTitle").text("Pause to enter caption for segment from " + FormatTime(captionsEndTime) + " to " + FormatTime(playTime) + ":");
            if (captionBeingDisplayed != -1) {
                $("#textCaptionEntry").val("");
                captionBeingDisplayed = -1;
            }
        }
    }
}

function SaveCurrentCaption() {
    var tag = GetMediaTag();
    if (tag !== undefined) {
        var playTime = tag.prop("currentTime");
        var captionsEndTime = existingCaptionsEndTime();
        if (playTime - 1 < captionsEndTime) {
            var ci = FindCaptionIndex(playTime - 1);
            if (ci != -1) {
                UpdateCaption(ci, $("#textCaptionEntry").val());
            }
        } else {
            AddCaption(captionsEndTime, playTime, $("#textCaptionEntry").val());
        }
    }
}

function IsPlayingVideo() {
    if ($("#videoContainer").is(':visible')) {
        return true;
    }
    return false;
}
function GetMediaControl() {
    if ($("#videoContainer").is(':visible')) {
        return $('video').get(0);
    }
    else {
        return  $('audio').get(0);
    }
}
function GetMediaTag() {
    if ($("#videoContainer").is(':visible')) {
        return $('video');
    }
    else {
        return $('audio');
    }
}

function PlayCaptionFromList(listRowId) {
    var captionsArrayIndex = parseInt(listRowId.match(/ci(\d+)/)[1]);
    var control = GetMediaControl();
    if (control != undefined) {
        var tag = GetMediaTag();
        if(tag != undefined){
            tag.bind('loadedmetadata', function () {
                control.currentTime = captionsArray[captionsArrayIndex].start;
                autoPauseAtTime = captionsArray[captionsArrayIndex].end;
            });
        }
        myPlayer.currentTime(captionsArray[captionsArrayIndex].start);
        control.currentTime = captionsArray[captionsArrayIndex].start;
        autoPauseAtTime = captionsArray[captionsArrayIndex].end;
      //  control.play();
    }
    /*
    if ($("#videoContainer").is(':visible')) {
        $('video').bind('oncanplay', function () {
            $('video').currentTime = captionsArray[captionsArrayIndex].start;
        });
    }
    else {
        $('#audioElm').bind('oncanplay', function () {
            $('#audioElm').currentTime = captionsArray[captionsArrayIndex].start;
        });
    }
    vid.play();
    */
}

function DisplayVTTSource() {
    var s = "WEBVTT\r\n\r\n";

    for (var i = 0; i < captionsArray.length; ++i) {
        if (captionsArray[i].caption != "") {
            s += (FormatTime(captionsArray[i].start) + " --> " + FormatTime(captionsArray[i].end) + "\r\n");
            s += captionsArray[i].caption + "\r\n\r\n";
        }
    }

    $("#captionFile").val(s);
    $("#captionFileKind").text(".vtt");
}

function DisplayTTMLSource() {
    var s = '<?xml version="1.0" encoding="UTF-8"?>\r\n<tt xmlns="http://www.w3.org/ns/ttml" xml:lang="en" >\r\n  <body>\r\n    <div>\r\n';

    for (var i = 0; i < captionsArray.length; ++i) {
        if (captionsArray[i].caption != "") {
            s += '      <p begin="' + captionsArray[i].start.toFixed(3) + 's" end="' + captionsArray[i].end.toFixed(3) + 's">' + XMLEncode(captionsArray[i].caption).replace(/\r\n|\r|\n/g, "<br />") + "</p>\r\n";
        }
    }

    s += "    </div>\r\n  </body>\r\n</tt>\r\n";

    $("#captionFile").val(s);
    $("#captionFileKind").text(".ttml");
}
function UpdateCaptionFileSource() {
    captionFileSourceUpdateTimer = null;
    if ($("#captionFormatVTT").prop("checked"))
        DisplayVTTSource();
    else if ($("#captionFormatTTML").prop("checked"))
        DisplayTTMLSource();
}
function download(data, filename, type) {
    var file = new Blob([data], { type: type });
    if (window.navigator.msSaveOrOpenBlob) // IE10+
        window.navigator.msSaveOrOpenBlob(file, filename);
    else { // Others
        var a = document.createElement("a"),
                url = URL.createObjectURL(file);
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        setTimeout(function () {
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
        }, 0);
    }
}
function CaptionFileExtension() {
    return $("#captionFormatVTT").prop("checked") ? "vtt" : $("#captionFormatTTML").prop("checked") ? "ttml" : "";
}
function GetFileName(url) {
    if (url) {
        var m = url.toString().match(/.*\/(.+?)\./);
        if (m && m.length > 1) {
            return m[1];
        }
    }
    return "";
}
function GetUrlName() {
    var videoName = undefined;
    if (IsPlayingVideo())
        videoName = $("#inputVideoUrl").val();
    else
        videoName = $("#inputAudioUrl").val();
    var fileName = GetFileName(videoName);
    if (fileName != "")
        return GetFileName(videoName) + "." + CaptionFileExtension();
    else
        return "";
}
function RefreshCaptionFileDisplay() {
    if ($("#captionFileAndMarkup").css("display") != "none") {
        if (captionFileSourceUpdateTimer === null)
            captionFileSourceUpdateTimer = window.setTimeout(UpdateCaptionFileSource, 16);
    }
}

function XMLEncode(s) {
    return s.replace(/\&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');   //.replace(/'/g, '&apos;').replace(/"/g, '&quot;');
}

function XMLDecode(s) {
    return s.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&apos;/g, "'").replace(/&quot;/g, '"').replace(/&amp;/g, '&');
}

function UpdateCaption(ci, captionText) {
    captionsArray[ci].caption = captionText;
    $("#ci" + ci.toString() + " span:last-child").html(XMLEncode(captionText).replace(/\r\n|\r|\n/g, "<br/>"));

    RefreshCaptionFileDisplay();
}

function AddCaptionListRow(ci) {
    var theId = "ci" + ci.toString();
    $("#display").append("<div id=\"" + theId + "\"><span>" + FormatTime(captionsArray[ci].start) + "</span><span>" + FormatTime(captionsArray[ci].end) + "</span><span>" + XMLEncode(captionsArray[ci].caption).replace(/\r\n|\r|\n/g, "<br/>") + "</span></div>");
    $("#" + theId).click(function () {
        PlayCaptionFromList($(this).attr("id"));
    });
}

function AddCaption(captionStart, captionEnd, captionText) {
    captionsArray.push({ start: captionStart, end: captionEnd, caption: Trim(captionText) });
    AddCaptionListRow(captionsArray.length - 1);
    RefreshCaptionFileDisplay();
}

function SortAndDisplayCaptionList() {
    captionsArray.sort(function (a, b) {
        return a.start - b.start;
    });

    $("#display div").remove();
    for (var ci = 0; ci < captionsArray.length; ++ci) {
        AddCaptionListRow(ci);
    }

    RefreshCaptionFileDisplay();
    AdjustHeight();
}


//-----------------------------------------------------------------------------------------------------------------------------------------
//	Partial parser for WebVTT files based on the spec at http://dev.w3.org/html5/webvtt/
//-----------------------------------------------------------------------------------------------------------------------------------------

function ParseAndLoadWebVTT(vtt) {

    var rxSignatureLine = /^WEBVTT(?:\s.*)?$/;

    var vttLines = vtt.split(/\r\n|\r|\n/); // create an array of lines from our file

    if (!rxSignatureLine.test(vttLines[0])) { // must start with a signature line
        alert("Not a valid time track file.");
        return;
    }

    var rxTimeLine = /^([\d\.:]+)\s+-->\s+([\d\.:]+)(?:\s.*)?$/;
    var rxCaptionLine = /^(?:<v\s+([^>]+)>)?([^\r\n]+)$/;
    var rxBlankLine = /^\s*$/;
    var rxMarkup = /<[^>]>/g;

    var cueStart = null, cueEnd = null, cueText = null;

    function appendCurrentCaption() {
        if (cueStart && cueEnd && cueText) {
            captionsArray.push({ start: cueStart, end: cueEnd, caption: Trim(cueText) });
        }

        cueStart = cueEnd = cueText = null;
    }

    for (var i = 1; i < vttLines.length; i++) {

        if (rxBlankLine.test(vttLines[i])) {
            appendCurrentCaption();
            continue;
        }

        if (!cueStart && !cueEnd && !cueText && vttLines[i].indexOf("-->") == -1) {
            //	this is a cue identifier we're ignoring
            continue;
        }

        var timeMatch = rxTimeLine.exec(vttLines[i]);
        if (timeMatch) {
            appendCurrentCaption();
            cueStart = ParseTime(timeMatch[1]);
            cueEnd = ParseTime(timeMatch[2]);
            continue;
        }

        var captionMatch = rxCaptionLine.exec(vttLines[i]);
        if (captionMatch && cueStart && cueEnd) {
            //	captionMatch[1] is the optional voice (speaker) we're ignoring
            var capLine = captionMatch[2].replace(rxMarkup, "");
            if (cueText)
                cueText += " " + capLine;
            else {
                cueText = capLine;
            }
        }
    }

    appendCurrentCaption();

    SortAndDisplayCaptionList();
}

//-----------------------------------------------------------------------------------------------------------------------------------------
//	A very partial parser for TTML files based on the spec at http://www.w3.org/TR/ttaf1-dfxp/
//  see samples at \\iefs\users\franko\ttml
//-----------------------------------------------------------------------------------------------------------------------------------------

function ParseAndLoadTTML(ttml) {

    var rxBr = /<br\s*\/>/g;
    var rxMarkup = /<[^>]+>/g;
    var rxP = /<p\s+([^>]+)>\s*((?:\s|.)*?)\s*<\/p>/g;
    var rxTime = /(begin|end|dur)\s*=\s*"([\d.:]+)(h|m|s|ms|t)?"/g;

    var tickRateMatch = ttml.match(/<tt\s[^>]*ttp:tickRate\s*=\s*"(\d+)"[^>]*>/i);
    var tickRate = (tickRateMatch != null) ? parseInt(tickRateMatch[1], 10) : 1;
    if (tickRate == 0)
        tickRate = 1;

    var pMatch;
    while ((pMatch = rxP.exec(ttml)) != null) {
        var cues = {};
        var timeMatch;
        rxTime.lastIndex = 0;
        var attrs = pMatch[1];
        while ((timeMatch = rxTime.exec(attrs)) != null) {
            var seconds;
            var metric = timeMatch[3];
            if (metric) {
                seconds = parseFloat(timeMatch[2]);
                if (metric == "h")
                    seconds *= (60 * 60);
                else if (metric == "m")
                    seconds *= 60;
                else if (metric == "ms")
                    seconds /= 1000;
                else if (metric == "t")
                    seconds /= tickRate;
            }
            else {
                seconds = ParseTime(timeMatch[2]);
            }

            cues[timeMatch[1]] = seconds;
        }

        if ("begin" in cues && ("end" in cues || "dur" in cues)) {
            var cueEnd = "end" in cues ? cues.end : (cues.begin + cues.dur);
            var cueText = Trim(XMLDecode(pMatch[2].replace(/[\r\n]+/g, "").replace(rxBr, "\n").replace(rxMarkup, "").replace(/ {2,}/g, " ")));
            captionsArray.push({ start: cues.begin, end: cueEnd, caption: cueText });
        }
    }

    SortAndDisplayCaptionList();
}

//  invoked by script insertion of proxyvtt.ashx
function ProcessProxyVttResponse(obj) {
    if (obj.status == "error")
        alert("Error loading caption file: " + obj.message);
    else if (obj.status == "success") {
        //  delete any captions we've got
        captionsArray.length = 0;

        if (obj.response.indexOf("<tt") != -1) {
            ParseAndLoadTTML(obj.response);
        } else if (obj.response.indexOf("WEBVTT") == 0) {
            ParseAndLoadWebVTT(obj.response);
        } else {
            alert("Unrecognised caption file format.");
        }
    }
}



// Functions End-------------------------------------------------------------------------------------//

//	Caption Array
var captionsArray = [];
// Subtitle Url Array 
var subtitlesArray = [];
// Video Url 
var videourl = undefined;
// Audio Url 
var audiourl = undefined;
// Start Time in seconds
var startTime = 0;
// Player Options
var myOptions = {
   // "nativeControlsForTouch": false,
    "autoplay": true,
    "controls": true,
 //   "poster": "",
    "muted": false,
    "language": "en",
    logo: {
        "enabled": false
    },
    "heuristicProfile": "Hybrid",
    "techOrder": ["azureHtml5JS", "flashSS", "silverlightSS", "html5"]

};
var autoPauseAtTime = -1;
//  keep track of whether we've attached an error handler to the video element
//  we don't do this during initialization because it immediately throws an error because we have no source
var errorHandlerBound = false;

//    
var captionFileSourceUpdateTimer = null;


//	copying to the clipboard requires clipboardData
//if (typeof window.clipboardData == 'undefined') {
    $("#copyToClipboard").css("display", "none");
//}

DisableDemoExceptLoadVideo();

// Create the player
var myPlayer = amp("azuremediaplayer", myOptions);

// Retrieve audioUrl from the url
var audioencodedurl = getParamFromUrl(window.location.href, "audiourl");
if (audioencodedurl !== undefined) {
    audiourl = decodeURIComponent(audioencodedurl);
    $("#inputAudioUrl").val(audiourl);
}

// Retrieve videoUrl from the url
var videoencodedurl = getParamFromUrl(window.location.href, "url");
if (videoencodedurl !== undefined) {
    videourl = decodeURIComponent(videoencodedurl);
    $("#inputVideoUrl").val(videourl);
}
// Retrieve subtitlesUrls from the url
var subtitlesurl = undefined;
var subtitlesencodedurl = getParamFromUrl(window.location.href, "subtitles");
if (subtitlesencodedurl !== undefined) {
    // URI sample below:
    // http://ampdemo.azureedge.net/player.html?url=http%3A%2F%2Ftestamsindexer.streaming.mediaservices.windows.net%2Faa8b3a1d-516b-4fd6-8c9a-5edc4a07bfd8%2FforestApril2017.ism%2Fmanifest&subtitles=FRE,fr,http%3A%2F%2Fams-samplescdn.streaming.mediaservices.windows.net%2F11196e3d-2f40-4835-9a4d-fc52751b0323%2FTOS-fr.vtt;ENG,en,http%3A%2F%2Fams-samplescdn.streaming.mediaservices.windows.net%2F11196e3d-2f40-4835-9a4d-fc52751b0323%2FTOS-en.vtt
    //
    subtitlesurl = decodeURIComponent(subtitlesencodedurl);
    var elmt = subtitlesurl.split(";");
    if (elmt !== undefined)
    {
        for(var i = 0; i < elmt.length; i++)
        {
            var subArray = elmt[i].split(",");
            if((subArray !== undefined)&&(subArray.length == 3)) 
            {
                subtitlesArray.push({ src: subArray[2], srclang: subArray[1], kind: "subtitles", label: subArray[0] });
            }
        }
    }
}

// Retrieve start time from the url
var StartTime = getParamFromUrl(window.location.href, "time");
if (StartTime !== undefined) {
    startTime = parseInt(StartTime);
}

if (subtitlesArray.length > 0) {
    $("#inputSubtitleUrl").val(subtitlesArray[0].src);
    $("#inputSubtitleLanguage").val(subtitlesArray[0].srclang);
    $("#inputSubtitleLabel").val(subtitlesArray[0].label);

}


if((videourl === undefined)&&(audiourl === undefined))
{
    // Use default url
    videourl = "http://ams-samplescdn.streaming.mediaservices.windows.net/11196e3d-2f40-4835-9a4d-fc52751b0323/TearsOfSteel_WAMEH264SmoothStreaming720p.ism/manifest";
    subtitlesArray = [
        { src: "http://ams-samplescdn.streaming.mediaservices.windows.net/11196e3d-2f40-4835-9a4d-fc52751b0323/TOS-en.vtt", srclang: "en", kind: "subtitles", label: "english" },
        { src: "http://ams-samplescdn.streaming.mediaservices.windows.net/11196e3d-2f40-4835-9a4d-fc52751b0323/TOS-es.vtt", srclang: "es", kind: "subtitles", label: "spanish" },
        { src: "http://ams-samplescdn.streaming.mediaservices.windows.net/11196e3d-2f40-4835-9a4d-fc52751b0323/TOS-fr.vtt", srclang: "fr", kind: "subtitles", label: "french" },
        { src: "http://ams-samplescdn.streaming.mediaservices.windows.net/11196e3d-2f40-4835-9a4d-fc52751b0323/TOS-it.vtt", srclang: "it", kind: "subtitles", label: "italian" }];

    $("#inputVideoUrl").val(videourl);
    $("#inputSubtitleUrl").val(subtitlesArray[0].src);
    $("#inputSubtitleLanguage").val(subtitlesArray[0].srclang);
    $("#inputSubtitleLabel").val(subtitlesArray[0].label);
}
//  index into captionsArray of the caption being displayed. -1 if none.
var captionBeingDisplayed = -1;





$("#playButton").click(function () {
    var control = GetMediaControl();
    if (control !== undefined) {
        control.play();
    }
});

$("#pauseButton").click(function () {
    var control = GetMediaControl();
    if (control !== undefined) {
        control.pause();
    }
});
$("#captionFormatTTML").click(function () {
    
    RefreshCaptionFileDisplay();
    DisplayTTMLSource();
});

$("#captionFormatVTT").click(function () {
    
    RefreshCaptionFileDisplay();
    DisplayVTTSource();
});

// button is not enabled if window.MSBlobBuilder is undefined
$("#blobBuilderSave").click(function () {
    UpdateCaptionFileSource();
    download($("#captionFile").val(),GetUrlName(),"text/plain");
});

// button is not enabled if window.clipboardData is undefined
//$("#copyToClipboard").click(function () {
//    UpdateCaptionFileSource();
//    window.clipboardData.clearData("Text");
//    window.clipboardData.setData("Text", $("#captionFile").val().replace(/\r\n|\r|\n/g, "\r\n")); // we normalize the line breaks to cr-lf for Notepad
//});

$("#justSaveCaption").click(function () {
    SaveCurrentCaption();
});

$("#saveCaptionAndPlay").click(function () {
    SaveCurrentCaption();
    var control = GetMediaControl();
    if (control != undefined) {
        control.play();
    }
});
LaunchPlayer(videourl, audiourl, subtitlesArray,startTime);


//  the audio element's event handlers
$("audio").bind({
    play: mediaPlayEventHandler,
    timeupdate: mediaTimeUpdateEventHandler,
    pause: mediaPauseEventHandler,
    canplay: EnableDemoAfterLoadVideo,
    loadeddata: EnableDemoAfterLoadVideo    // opera doesn't appear to fire canplay but does fire loadeddata
});
//  the video element's event handlers
$("video").bind({
    play: mediaPlayEventHandler,
    timeupdate: mediaTimeUpdateEventHandler,
    pause: mediaPauseEventHandler,
    canplay: EnableDemoAfterLoadVideo,
    loadeddata: EnableDemoAfterLoadVideo    // opera doesn't appear to fire canplay but does fire loadeddata
});


