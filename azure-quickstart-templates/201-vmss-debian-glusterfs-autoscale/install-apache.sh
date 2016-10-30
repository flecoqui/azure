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
# GlusterFS client  installation 
if [ $VER -eq 7 ];
then
# install a version compliant with Debian 7 
wget -O - http://download.gluster.org/pub/gluster/glusterfs/3.7/LATEST/rsa.pub | apt-key add -
echo deb http://download.gluster.org/pub/gluster/glusterfs/3.7/LATEST/Debian/wheezy/apt wheezy main > /etc/apt/sources.list.d/gluster.list 
fi
if [ $VER -eq 8 ];
then
wget -O - http://download.gluster.org/pub/gluster/glusterfs/LATEST/rsa.pub | apt-key add -
echo deb http://download.gluster.org/pub/gluster/glusterfs/LATEST/Debian/jessie/apt jessie main > /etc/apt/sources.list.d/gluster.list
fi

apt-get update -y
apt-get install glusterfs-client -y

# firewall configuration 
iptables -A INPUT -p tcp --dport 80 -j ACCEPT
iptables -A INPUT -p tcp --dport 443 -j ACCEPT
iptables -A INPUT -p tcp --dport 22 -j ACCEPT

# glusterfs mount
mkdir /shareddata
mount -t glusterfs gfs1vm0:gfs1vol /shareddata
wm_nfs=`df -h /shareddata`

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
    <p>NFS partition:</p>
    <p>   $wm_nfs</p> 
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

