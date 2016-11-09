#!/bin/bash
# This bash file install apache on debian
# Parameter 1 hostname 
wm_hostname=$1
wm_gfsvm=$2
wm_gfsvol=$3
wm_mysqlvm=$4
wm_redisvm=$5
wm_ipaddr=`ip addr show  eth0 | grep global`
#############################################################################
log()
{
	# If you want to enable this logging, uncomment the line below and specify your logging key 
	#curl -X POST -H "content-type:text/plain" --data-binary "$(date) | ${HOSTNAME} | $1" https://logs-01.loggly.com/inputs/${LOGGING_KEY}/tag/redis-extension,${HOSTNAME}
	echo "$1"
}

environ=`env`
log "Environment before installation: $environ"

log "Installation script start : $(date)"
log "Apache Installation: $(date)"
log "#####  wm_hostname: $wm_hostname"
log "Installation script start : $(date)"
log "NFS: $wm_gfsvm:$wm_gfsvol"
log "MYSQL: $wm_mysqlvm"
log "REDIS: $wm_redisvm"


ARCH=$(uname -m | sed 's/x86_//;s/i[3-6]86/32/')
#############################################################################
VERSION="3.0.7"
REDIS_PORT=6379
#############################################################################
install_redis()
{
	log "Installing Redis v${VERSION}"

	# Installing build essentials (if missing) and other required tools
	apt-get -y install build-essential

	wget http://download.redis.io/releases/redis-$VERSION.tar.gz
	tar xzf redis-$VERSION.tar.gz
	cd redis-$VERSION
	make
	make install prefix=/usr/local/bin/

	log "Redis package v${VERSION} was downloaded and built successfully"
}

#############################################################################
configure_redis()
{
	# Configure the general settings
	sed -i "s/^port.*$/port ${REDIS_PORT}/g" redis.conf
	sed -i "s/^daemonize no$/daemonize yes/g" redis.conf
	sed -i 's/^logfile ""/logfile \/var\/log\/redis.log/g' redis.conf
	sed -i "s/^loglevel verbose$/loglevel notice/g" redis.conf
	sed -i "s/^dir \.\//dir \/var\/redis\//g" redis.conf 
	sed -i "s/\${REDISPORT}.conf/redis.conf/g" utils/redis_init_script 
	sed -i "s/_\${REDISPORT}.pid/.pid/g" utils/redis_init_script 
	
	# Create all essentials directories and copy files to the correct locations
	mkdir /etc/redis
	mkdir /var/redis
	
	cp redis.conf /etc/redis/redis.conf
	
	# Clean up temporary files
	cd ..
	rm redis-$VERSION -R
	rm redis-$VERSION.tar.gz

	log "Redis configuration was applied successfully"
}

if [ -f /etc/lsb-release ]; then
    . /etc/lsb-release
    OS=$DISTRIB_ID
    VER=$DISTRIB_RELEASE
elif [ -f /etc/debian_version ]; then
    OS=Debian  # XXX or Ubuntu??
    VER=$(cat /etc/debian_version)
else
    OS=$(uname -s)
    VER=$(uname -r)
fi
log "OS=$OS version $VER $ARCH"

# Apache installation 
log "Installing Apache"
apt-get -y update
apt-get -y install apache2
# GlusterFS client  installation 
log "Installing Gluster FS"
NUM=`echo $VER |  sed 's/\(\.\)[0-9]*//g'`
if [ $NUM -eq 7 ];
then
# install a version compliant with Debian 7 
wget -O - http://download.gluster.org/pub/gluster/glusterfs/3.7/LATEST/rsa.pub | apt-key add -
echo deb http://download.gluster.org/pub/gluster/glusterfs/3.7/LATEST/Debian/wheezy/apt wheezy main > /etc/apt/sources.list.d/gluster.list 
fi
# install a version compliant with Debian 8 
if [ $NUM -eq 8 ];
then
wget -O - http://download.gluster.org/pub/gluster/glusterfs/LATEST/rsa.pub | apt-key add -
echo deb http://download.gluster.org/pub/gluster/glusterfs/LATEST/Debian/jessie/apt jessie main > /etc/apt/sources.list.d/gluster.list
fi
apt-get update -y
apt-get install glusterfs-client -y

#install mysql client
apt-get install mysql-client -y
wm_mysql=`mysql --user=admin --password=VMP@ssw0rd -h $wm_mysqlvm -e "show databases;"`

# Redis installation & configuration 
log "Installing redis Cache"
install_redis
configure_redis

# firewall configuration 
log "Configuring firewall "
iptables -A INPUT -p tcp --dport 80 -j ACCEPT
iptables -A INPUT -p tcp --dport 443 -j ACCEPT

# glusterfs mount
log "Installing Gluster FS Client"
mkdir /shareddata
mount -t glusterfs $wm_gfsvm:$wm_gfsvol /shareddata
wm_nfs=`df -h /shareddata`

# redis test 
log "Testing redis Cache Client"
wm_redis=`redis-cli -h $wm_redisvm ping`

directory=/var/www/html
if [ ! -d $directory ]; then
mkdir $directory
fi
directorybin=/var/www/html/cgi-bin
if [ ! -d $directorybin ]; then
mkdir $directorybin
fi

log "Creating Apache home page"
cat <<EOF > $directory/index.html 
<html>
  <head>
    <title>Sample "Hello from Azure Debian VM scaleset $wm_hostname" </title>
  </head>
  <body bgcolor=white>

    <table border="0" cellpadding="10">
      <tr>
        <td>
          <h1>Hello from Azure Debian VM scaleset $wm_hostname</h1>
        </td>
      </tr>
    </table>

    <p>This is the home page for the Apache test on Azure VM</p>
    <p>Local IP address:</p>
    <p>   $wm_ipaddr</p> 
    <p>MYSQL databses:</p>
    <p>   $wm_mysql</p>
	<p>NFS partition:</p>
    <p>   $wm_nfs</p> 
    <p>Redis Cache response to ping:</p>
    <p>   $wm_redis</p> 
	<ul>
      <li>To the VM Scale Set: <a href="http://$wm_hostname/html/index.html">http://$wm_hostname/html/index.html</a>
    </ul>
    <ul>
      <li>To <a href="cgi-bin/info.cgi">Services information</a>
      <li>To <a href="http://www.microsoft.com">Microsoft</a>
      <li>To <a href="https://portal.azure.com">Azure</a>
    </ul>
  </body>
</html>
EOF

log "Creating cgi file"
cat <<EOF > $directorybin/info.cgi 
#!/bin/bash
echo "Content-type: text/html"
echo ""
echo "<html><head><title>Bash as CGI"
echo "</title></head><body>"

echo "<h1>General system information for host $(hostname -s)</h1>"
echo ""

echo "<h1>Memory Info</h1>"
echo "<pre> \$(free -m) </pre>"

echo "<h1>Disk Info:</h1>"
echo "<pre> \$(df -h) </pre>"
echo "<pre> \$(mount -t glusterfs $wm_gfsvm:$wm_gfsvol /shareddata) </pre>"

echo "<h1>NFS partition content: /shareddata</h1>"
echo "<pre> \$(ls -l /shareddata) </pre>"

echo "<h1>MYSQL databases:</h1>"
echo "<pre> \$(mysql --user=admin --password=VMP@ssw0rd -h $wm_mysqlvm -e "show databases;")</pre>"

echo "<h1>REDIS Cache response on \$(date):</h1>"
echo "<pre> \$(redis-cli -h $wm_redisvm ping) </pre>"

echo "<h1>Logged in user</h1>"
echo "<pre> \$(w) </pre>"

echo "<center>Information generated on \$(date)</center>"
echo "</body></html>"
EOF

chmod +x $directorybin/info.cgi 

rm -f /etc/apache2/sites-enabled/*.conf
echo "Configuring Web Site for Apache: $(date)"
cat <<EOF > /etc/apache2/sites-enabled/html.conf 
ServerName "$wm_hostname"
<VirtualHost *:80>
        ServerAdmin webmaster@localhost
        ServerName "$wm_hostname"

        DocumentRoot /var/www
        <Directory  />
                Options FollowSymLinks
                AllowOverride None
        </Directory>
		<Directory "/var/www/html/cgi-bin" >
				Options ExecCGI
				SetHandler cgi-script 
		</Directory>
		AddHandler cgi-script .cgi .pl .sh

        # Possible values include: debug, info, notice, warn, error, crit,
        # alert, emerg.
        LogLevel warn
        ErrorLog /var/log/apache2/html-error.log
        CustomLog /var/log/apache2/html-access.log combined
</VirtualHost>
EOF
a2enmod cgi
apachectl restart
exit 0 

