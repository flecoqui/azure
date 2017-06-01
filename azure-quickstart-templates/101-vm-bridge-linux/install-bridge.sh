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
iptables -A INPUT -p tcp --dport 80 -j ACCEPT
iptables -A INPUT -p tcp --dport 443 -j ACCEPT
iptables -A INPUT -p tcp --dport 5985 -j ACCEPT
iptables -A INPUT -p tcp --dport 5986 -j ACCEPT
iptables -A INPUT -p tcp --dport 5201 -j ACCEPT
iptables -A INPUT -p udp --dport 5201 -j ACCEPT


# Configure the general settings
echo 1 > /proc/sys/net/ipv4/ip_forward
# Uncomment the next line to enable packet forwarding for IPv4
sed -i "s/#net.ipv4.ip_forward=1/net.ipv4.ip_forward=1/g" /etc/sysctl.conf
sysctl -p

#enable NAT
iptables -t nat -A POSTROUTING -o eth0 -j MASQUERADE
iptables -A FORWARD -i eth0 -o eth1 -m state --state RELATED,ESTABLISHED -j ACCEPT
iptables -A FORWARD -i eth1 -o eth0 -j ACCEPT
# Forwarding port 80
iptables -t nat -A PREROUTING -p tcp  -d 10.0.1.5/32 --dport 80 -j DNAT --to-destination 10.0.0.4:80
iptables -t nat -A POSTROUTING  -d 10.0.0.4 -p tcp --dport 80 -j MASQUERADE
# Forwarding port 443
iptables -t nat -A PREROUTING -p tcp  -d 10.0.1.5/32 --dport 443 -j DNAT --to-destination 10.0.0.4:443
iptables -t nat -A POSTROUTING  -d 10.0.0.4 -p tcp --dport 443 -j MASQUERADE
# Forwarding port 5201
iptables -t nat -A PREROUTING -p tcp  -d 10.0.1.5/32 --dport 5201 -j DNAT --to-destination 10.0.0.4:5201
iptables -t nat -A POSTROUTING  -d 10.0.0.4 -p tcp --dport 5201 -j MASQUERADE
# Forwarding port 5201 udp
iptables -t nat -A PREROUTING -p udp  -d 10.0.1.5/32 --dport 5201 -j DNAT --to-destination 10.0.0.4:5201
iptables -t nat -A POSTROUTING  -d 10.0.0.4 -p udp --dport 5201 -j MASQUERADE

iptables-save > /etc/iptables_rules.save
cat /etc/network/interfaces


exit 0 

