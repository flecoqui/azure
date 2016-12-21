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
apt-get -y update
apt-get -y install apache2
# firewall configuration 
sudo iptables -A INPUT -p tcp --dport 80 -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 443 -j ACCEPT
#
# install .Net Core
#  
apt-get -y install curl
apt-get -y install  gettext
apt-get -y install libunwind8
curl -sSL -o dotnet.tar.gz https://go.microsoft.com/fwlink/?LinkID=835021
mkdir -p /opt/dotnet && tar zxf dotnet.tar.gz -C /opt/dotnet
ln -s /opt/dotnet/dotnet /usr/local/bin

# Create new app
#mkdir hwapp
#cd hwapp
#dotnet new
#dotnet restore
#dotnet run

#
# Start Apache server
#
systemctl start apache2
systemctl enable apache2
systemctl status apache2

directory=/var/www/html
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

    <p>This is the home page for .Net Core test on Azure VM</p>
    <ul>
      <li>To <a href="http://www.microsoft.com">Microsoft</a>
      <li>To <a href="https://portal.azure.com">Azure</a>
    </ul>
  </body>
</html>
EOF


echo "Configuring Web Site for Apache: $(date)"
cat <<EOF > /etc/apache2/conf-available/test-web.conf 
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
exit 0 

