#!/bin/bash
# This bash file install apache on debian
# Parameter 1 hostname 
wm_hostname=$1
wm_ipaddr=`ip addr show  eth0 | grep global`

environ=`env`
echo "Environment before installation: $environ"
echo "Installation script start : $(date)"
echo "Apache Installation: $(date)"
echo "#####  wm_hostname: $wm_hostname"
echo "Installation script start : $(date)"
ARCH=$(uname -m | sed 's/x86_//;s/i[3-6]86/32/')

#############################################################################
VERSION="3.0.0"
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
elif [ -f /etc/redhat-release ]; then
    # TODO add code for Red Hat and CentOS here
    ...
else
    OS=$(uname -s)
    VER=$(uname -r)
fi
echo "OS=$OS version $VER $ARCH"

# Apache installation 
apt-get -y update
apt-get -y install apache2

# Redis installation & configuration 
install_redis()
configure_redis()


# firewall configuration 
iptables -A INPUT -p tcp --dport 80 -j ACCEPT
iptables -A INPUT -p tcp --dport 443 -j ACCEPT
iptables -A INPUT -p tcp --dport 22 -j ACCEPT

# redis test 
wm_redis=`redis-cli -h 10.0.0.80 ping`

directory=/var/www/html
if [ ! -d $directory ]; then
mkdir $directory
fi

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
    <p>Redis Cache response to ping:</p>
    <p>   $wm_redis</p> 
    <ul>
      <li>To the VM Scale Set: <a href="http://$wm_hostname/html/index.html">http://$wm_hostname/html/index.html</a>
    </ul>
    <ul>
      <li>To <a href="http://www.microsoft.com">Microsoft</a>
      <li>To <a href="https://portal.azure.com">Azure</a>
    </ul>
  </body>
</html>
EOF
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


        # Possible values include: debug, info, notice, warn, error, crit,
        # alert, emerg.
        LogLevel warn
        ErrorLog /var/log/apache2/evaluation-error.log
        CustomLog /var/log/apache2/evaluation-access.log combined
</VirtualHost>
EOF

apachectl restart
exit 0 

