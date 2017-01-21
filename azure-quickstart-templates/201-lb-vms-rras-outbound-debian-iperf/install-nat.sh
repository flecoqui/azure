#!/bin/bash
# This bash file install NAT on Debian
# Parameter 1 hostname 
usp_hostname=$1


environ=`env`
echo "Environment before installation: $environ"

echo "Installation script start : $(date)"
echo "#####  hostname: $usp_hostname"
echo "Installation script start : $(date)"

# firewall configuration 
sudo iptables -A INPUT -p tcp --dport 80 -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 443 -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 5201 -j ACCEPT
sudo iptables -A INPUT -p udp --dport 5201 -j ACCEPT


# Configure the general settings
# Uncomment the next line to enable packet forwarding for IPv4
sed -i "s/#net.ipv4.ip_forward=1/net.ipv4.ip_forward=1/g" /etc/sysctl.conf
sysctl -p

#enable NAT
sudo iptables -t nat -A POSTROUTING -o eth0 -j MASQUERADE
iptables-save > /etc/iptables_rules.save
cat /etc/network/interfaces

exit 0 

