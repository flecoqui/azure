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
sudo iptables -A INPUT -p tcp --dport 5985 -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 5986 -j ACCEPT
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

iptables -t nat -A PREROUTING -j DNAT -d 10.0.1.5 -p tcp --dport 80 --to 10.0.0.4
iptables -t nat -A PREROUTING -j DNAT -d 10.0.1.5 -p tcp --dport 443 --to 10.0.0.4
iptables -t nat -A PREROUTING -j DNAT -d 10.0.1.5 -p tcp --dport 5985 --to 10.0.0.4
iptables -t nat -A PREROUTING -j DNAT -d 10.0.1.5 -p tcp --dport 5986 --to 10.0.0.4
iptables -t nat -A PREROUTING -j DNAT -d 10.0.1.5 -p tcp --dport 5201 --to 10.0.0.4
iptables -t nat -A PREROUTING -j DNAT -d 10.0.1.5 -p udp --dport 5201 --to 10.0.0.4


exit 0 

