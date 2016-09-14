#!/bin/bash
# This bash file install iperf3 demonstraiton on centos
# Parameter 1 hostname 
usp_hostname=$1


environ=`env`
echo "Environment before installation: $environ"

echo "Installation script start : $(date)"
echo "Apache Installation: $(date)"
echo "#####  usp_hostname: $usp_hostname"
echo "Installation script start : $(date)"
# Apache installation 
yum clean all
yum -y install httpd
service firewalld start
# Apache configuration 
firewall-cmd --permanent --add-port=5201/tcp
firewall-cmd --permanent --add-port=5201/udp
firewall-cmd --permanent --add-port=80/tcp
firewall-cmd --permanent --add-port=443/tcp
firewall-cmd --reload
#
# install iperf3
#  
yum -y install libc.so.6
rpm -ivh https://iperf.fr/download/fedora/iperf3-3.1.3-1.fc24.x86_64.rpm

adduser iperf -s /sbin/nologin
cat <<EOF > /etc/systemd/system/iperf3.service
[Unit]
Description=iperf3 Service
After=network.target

[Service]
Type=simple
User=iperf
ExecStart=/usr/bin/iperf3 -s
Restart=on-abort


[Install]
WantedBy=multi-user.target
EOF
#
# Start Apache server
#
systemctl start httpd
systemctl enable httpd
systemctl status httpd

directory=/var/www/test-web
if [ ! -d $directory ]; then
mkdir $directory
fi

cat <<EOF > $directory/index.html 
<html>
  <head>
    <title>Sample "Hello from $usp_hostname" </title>
  </head>
  <body bgcolor=white>

    <table border="0" cellpadding="10">
      <tr>
        <td>
          <h1>Hello from $usp_hostname</h1>
        </td>
      </tr>
    </table>

    <p>This is the home page for the iperf3 test on Azure VM</p>
    <p>Launch the command line from your client: </p>
    <p>     iperf3 -c $usp_hostname -p 5201 --parallel 32  </p> 
    <ul>
      <li>To <a href="http://www.microsoft.com">Microsoft</a>
      <li>To <a href="https://portal.azure.com">Azure</a>
    </ul>
  </body>
</html>
EOF


echo "Configuring Web Site for Apache: $(date)"
cat <<EOF > /etc/httpd/conf.d/test-web.conf 
ServerName "$usp_hostname"
<VirtualHost *:80>
        ServerAdmin webmaster@localhost
        ServerName "$usp_hostname"

        DocumentRoot /var/www/test-web
        <Directory />
                Options FollowSymLinks
                AllowOverride None
        </Directory>

       # Add CORS headers for HTML5 players
        Header always set Access-Control-Allow-Headers "origin, range"
        Header always set Access-Control-Allow-Methods "GET, HEAD, OPTIONS"
        Header always set Access-Control-Allow-Origin "*"
        Header always set Access-Control-Expose-Headers "Server,range"

        # Possible values include: debug, info, notice, warn, error, crit,
        # alert, emerg.
        LogLevel warn
        ErrorLog /var/log/httpd/usp-evaluation-error.log
        CustomLog /var/log/httpd/usp-evaluation-access.log combined
</VirtualHost>
EOF
apachectl restart
systemctl enable iperf3
systemctl start iperf3
exit 0 

