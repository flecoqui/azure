#!/bin/bash
# This bash file install apache
# Parameter 1 hostname 
azure_hostname=$1


environ=`env`
echo "Environment before installation: $environ"

echo "Installation script start : $(date)"
echo "Apache Installation: $(date)"
echo "#####  azure_hostname: $azure_hostname"
echo "Installation script start : $(date)"
# Apache installation 
apt-get -y update
apt-get -y install apache2
apt-get -y install php5-common libapache2-mod-php5 php5-cli
apt-get -y install curl
# firewall configuration 
sudo iptables -A INPUT -p tcp --dport 80 -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 443 -j ACCEPT

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

cat <<EOF > $directory/index.php 
<html>
  <head>
    <title>Sample "Hello from $azure_hostname" </title>
  </head>
  <body bgcolor=white>

    <table border="0" cellpadding="10">
      <tr>
        <td>
          <h1>Hello from $azure_hostname</h1>
			<p>Local IP Address: </p>
			<?php echo shell_exec("ifconfig eth0 |  grep 'inet ' | awk '{print $2}' | sed 's/addr://'"); ?>
			<p>Public IP Address: </p>
			<?php echo shell_exec("curl -s checkip.dyndns.org | sed -e 's/.*Current IP Address: //' -e 's/<.*$//'"); ?>
        </td>
      </tr>
    </table>

    <p>This is the home page for the Apache test on Azure VM</p>
    <ul>
      <li>To <a href="http://www.microsoft.com">Microsoft</a>
      <li>To <a href="https://portal.azure.com">Azure</a>
    </ul>
  </body>
</html>
EOF


echo "Configuring Web Site for Apache: $(date)"
cat <<EOF > /etc/apache2/sites-available/html.conf 
ServerName "$azure_hostname"
<VirtualHost *:80>
        ServerAdmin webmaster@localhost
        ServerName "$azure_hostname"

        DocumentRoot /var/www/html
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
        ErrorLog /var/log/apache2/azure-evaluation-error.log
        CustomLog /var/log/apache2/azure-evaluation-access.log combined
</VirtualHost>
EOF
apachectl restart
