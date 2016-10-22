#!/bin/bash
# This bash file install iperf3 demonstration on centos
# Parameter 1 hostname 
wm_hostname=$1
wm_ipaddr=`ip addr show  eth0 | grep global`

environ=`env`
echo "Environment before installation: $environ"

echo "Installation script start : $(date)"
echo "Apache Installation: $(date)"
echo "#####  wm_hostname: $wm_hostname"
echo "Installation script start : $(date)"
# Apache installation 
apt-get -y update
apt-get -y install apache2
# firewall configuration 
sudo iptables -A INPUT -p tcp --dport 80 -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 443 -j ACCEPT
# sudo iptables -A INPUT -p tcp --dport 5201 -j ACCEPT
# sudo iptables -A INPUT -p udp --dport 5201 -j ACCEPT
#
# install iperf3
#  
# apt-get remove iperf3 libiperf0
# wget https://iperf.fr/download/ubuntu/libiperf0_3.1.3-1_amd64.deb
# wget https://iperf.fr/download/ubuntu/iperf3_3.1.3-1_amd64.deb
# dpkg -i libiperf0_3.1.3-1_amd64.deb iperf3_3.1.3-1_amd64.deb
# rm libiperf0_3.1.3-1_amd64.deb iperf3_3.1.3-1_amd64.deb

# adduser iperf --disabled-login
# cat <<EOF > /etc/systemd/system/iperf3.service
# [Unit]
# Description=iperf3 Service
# After=network.target

# [Service]
# Type=simple
# User=iperf
# ExecStart=/usr/bin/iperf3 -s
# Restart=on-abort


# [Install]
# WantedBy=multi-user.target
# EOF
#
# Start Apache server
#

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
    <ul>
      <li>To <a href="http://www.microsoft.com">Microsoft</a>
      <li>To <a href="https://portal.azure.com">Azure</a>
    </ul>
  </body>
</html>
EOF


# echo "Configuring Web Site for Apache: $(date)"
# cat <<EOF > /etc/apache2/conf-available/test-web.conf 
#ServerName "$wm_hostname"
#<VirtualHost *:80>
#        ServerAdmin webmaster@localhost
#        ServerName "$wm_hostname"

#        DocumentRoot /var/www/test-web
#        <Directory />
#                Options FollowSymLinks
#                AllowOverride None
#        </Directory>

#       # Add CORS headers for HTML5 players
#        Header always set Access-Control-Allow-Headers "origin, range"
#        Header always set Access-Control-Allow-Methods "GET, HEAD, OPTIONS"
#        Header always set Access-Control-Allow-Origin "*"
#        Header always set Access-Control-Expose-Headers "Server,range"

#        # Possible values include: debug, info, notice, warn, error, crit,
#        # alert, emerg.
#        LogLevel warn
#        ErrorLog /var/log/httpd/usp-evaluation-error.log
#        CustomLog /var/log/httpd/usp-evaluation-access.log combined
#</VirtualHost>
#EOF
apachectl restart
#systemctl enable iperf3
#systemctl start iperf3
exit 0 

