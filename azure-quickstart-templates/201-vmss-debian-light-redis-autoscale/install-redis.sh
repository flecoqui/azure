#!/bin/bash

# The MIT License (MIT)
#
# Copyright (c) 2015 Microsoft Azure
#
# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documentation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of the Software, and to permit persons to whom the Software is
# furnished to do so, subject to the following conditions:
# 
# The above copyright notice and this permission notice shall be included in all
# copies or substantial portions of the Software.
# 
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
# SOFTWARE.

# Script parameters and their defaults
VERSION="3.0.7"
CLUSTER_NAME="redis-cluster"
IS_LAST_NODE=0
IS_CLUSTER_AWARE=1
INSTANCE_COUNT=2
MASTER_NODE_COUNT=1
SLAVE_NODE_COUNT=1
NODE_INDEX=0
IP_PREFIX="10.0.0.0"
LOGGING_KEY="[account-key]"
REDIS_PORT=6379
CURRENT_DIRECTORY=$(pwd)

########################################################
# This script will install Redis from sources
########################################################
help()
{
	echo "This script installs Redis Cache on the Ubuntu virtual machine image"
	echo "Available parameters:"
	echo "-n Cluster name"
	echo "-c Number of instances"
	echo "-i Sequential node index (starting from 0)"
	echo "-p Private IP address prefix"
	echo "-h Help"
}

#############################################################################
log()
{
	# If you want to enable this logging, uncomment the line below and specify your logging key 
	#curl -X POST -H "content-type:text/plain" --data-binary "$(date) | ${HOSTNAME} | $1" https://logs-01.loggly.com/inputs/${LOGGING_KEY}/tag/redis-extension,${HOSTNAME}
	echo "$1"
}

log "Begin execution of Redis installation script extension on ${HOSTNAME}"

if [ "${UID}" -ne 0 ];
then
    log "Script executed without root permissions"
    echo "You must be root to run this program." >&2
    exit 0
#    exit 3
fi

# Parse script parameters
while getopts :n:c:i:p:h optname; do
  log "Option $optname set with value ${OPTARG}"
  
  case $optname in
    n)  # Cluster name
		CLUSTER_NAME=${OPTARG}
		;;
	c) # Number of instances
		INSTANCE_COUNT=${OPTARG}
		;;
	i) # Sequential node index
		NODE_INDEX=${OPTARG}
		;;				
	p) # Private IP address prefix
		IP_PREFIX=${OPTARG}
		;;			
    h)  # Helpful hints
		help
		exit 0
#		exit 2
		;;
    \?) #unrecognized option - show help
		echo -e \\n"Option -${BOLD}$OPTARG${NORM} not allowed."
		help
		exit 0
#		exit 2
		;;
  esac
done

if [ "$INSTANCE_COUNT" -eq "$(($NODE_INDEX+1))" ];
then
  	log "Last node flag set"
	IS_LAST_NODE=1
	else
  	log "Last node flag not set"
	IS_LAST_NODE=0
fi

let MASTER_NODE_COUNT=$INSTANCE_COUNT%2
let SLAVE_NODE_COUNT=$INSTANCE_COUNT%2

#############################################################################
tune_system()
{
	log "Tuning the system configuration"
	
	# Ensure the source list is up-to-date
	apt-get -y update

	# Add local machine name to the hosts file to facilitate IP address resolution
	if grep -q "${HOSTNAME}" /etc/hosts
	then
	  echo "${HOSTNAME} was found in /etc/hosts"
	else
	  echo "${HOSTNAME} was not found in and will be added to /etc/hosts"
	  # Append it to the hsots file if not there
	  echo "127.0.0.1 $(hostname)" >> /etc/hosts
	  log "Hostname ${HOSTNAME} added to /etc/hosts"
	fi	
}

#############################################################################
tune_memory()
{
	log "Tuning the memory configuration"
	
	# Get the supporting utilities
	apt-get -y install hugepages

	# Resolve a "Background save may fail under low memory condition." warning
	sysctl vm.overcommit_memory=1

	# Disable the Transparent Huge Pages (THP) support in the kernel
	sudo hugeadm --thp-never
}

#############################################################################
tune_network()
{
	log "Tuning the network configuration"
	
>/etc/sysctl.conf cat << EOF 

	# Disable syncookies (syncookies are not RFC compliant and can use too muche resources)
	net.ipv4.tcp_syncookies = 0

	# Basic TCP tuning
	net.ipv4.tcp_keepalive_time = 600
	net.ipv4.tcp_synack_retries = 3
	net.ipv4.tcp_syn_retries = 3

	# RFC1337
	net.ipv4.tcp_rfc1337 = 1

	# Defines the local port range that is used by TCP and UDP to choose the local port
	net.ipv4.ip_local_port_range = 1024 65535

	# Log packets with impossible addresses to kernel log
	net.ipv4.conf.all.log_martians = 1

	# Disable Explicit Congestion Notification in TCP
	net.ipv4.tcp_ecn = 0

	# Enable window scaling as defined in RFC1323
	net.ipv4.tcp_window_scaling = 1

	# Enable timestamps (RFC1323)
	net.ipv4.tcp_timestamps = 1

	# Enable select acknowledgments
	net.ipv4.tcp_sack = 1

	# Enable FACK congestion avoidance and fast restransmission
	net.ipv4.tcp_fack = 1

	# Allows TCP to send "duplicate" SACKs
	net.ipv4.tcp_dsack = 1

	# Controls IP packet forwarding
	net.ipv4.ip_forward = 0

	# No controls source route verification (RFC1812)
	net.ipv4.conf.default.rp_filter = 0

	# Enable fast recycling TIME-WAIT sockets
	net.ipv4.tcp_tw_recycle = 1
	net.ipv4.tcp_max_syn_backlog = 20000

	# How may times to retry before killing TCP connection, closed by our side
	net.ipv4.tcp_orphan_retries = 1

	# How long to keep sockets in the state FIN-WAIT-2 if we were the one closing the socket
	net.ipv4.tcp_fin_timeout = 20

	# Don't cache ssthresh from previous connection
	net.ipv4.tcp_no_metrics_save = 1
	net.ipv4.tcp_moderate_rcvbuf = 1

	# Increase Linux autotuning TCP buffer limits
	net.ipv4.tcp_rmem = 4096 87380 16777216
	net.ipv4.tcp_wmem = 4096 65536 16777216

	# increase TCP max buffer size
	net.core.rmem_max = 16777216
	net.core.wmem_max = 16777216
	net.core.netdev_max_backlog = 2500

	# Increase number of incoming connections
	net.core.somaxconn = 65000
EOF

	# Reload the networking settings
	/sbin/sysctl -p /etc/sysctl.conf
}

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
	cp utils/redis_init_script /etc/init.d/redis-server
	

	# Clean up temporary files
	cd ..
	rm redis-$VERSION -R
	rm redis-$VERSION.tar.gz

	log "Redis configuration was applied successfully"
	
	# Create service user and configure for permissions
	useradd -r -s /bin/false redis
	chown redis:redis /var/run/redis.pid
	chmod 755 /etc/init.d/redis-server

	# Start the script automatically at boot time
	update-rc.d redis-server defaults
	
	log "Redis service was created successfully"	
}

#############################################################################
configure_redis_cluster()
{
	# Enable the AOF persistence
	sed -i "s/^appendonly no$/appendonly yes/g" /etc/redis/redis.conf

	# Tune the RDB persistence
	sed -i "s/^save.*$/# save/g" /etc/redis/redis.conf
	echo "save 3600 1" >> /etc/redis/redis.conf

	# Add cluster configuration (expected to be commented out in the default configuration file)
	echo "cluster-enabled yes" >> /etc/redis/redis.conf
	echo "cluster-node-timeout 5000" >> /etc/redis/redis.conf
	echo "cluster-config-file ${CLUSTER_NAME}.conf" >> /etc/redis/redis.conf
}
#############################################################################
# Expand a list of successive IP range defined by a starting address prefix (e.g. 10.0.0.1) and the number of machines in the range
# 10.0.0.1-3 would be converted to "10.0.0.10 10.0.0.11 10.0.0.12"
expand_ip_range() {
    IFS='-' read -a HOST_IPS <<< "$1"

    declare -a EXPAND_STATICIP_RANGE_RESULTS=()
	
    for (( n=0 ; n<("${HOST_IPS[1]}"+0) ; n++))
    do
        HOST="${HOST_IPS[0]}${n}:${REDIS_PORT}"
		EXPAND_STATICIP_RANGE_RESULTS+=($HOST)
    done
	
    echo "${EXPAND_STATICIP_RANGE_RESULTS[@]}"
}

#############################################################################
initialize_redis_cluster()
{
	# Cluster setup must run on the last node (a nasty workaround until ARM can recognize multiple custom script extensions)
	if [ "$IS_LAST_NODE" -eq 1 ]; then
		let REPLICA_COUNT=$SLAVE_NODE_COUNT/$MASTER_NODE_COUNT
		SLAVE_COUNT=$REPLICA_COUNT

#		sudo bash redis-cluster-setup.sh -c $INSTANCE_COUNT -s $REPLICA_COUNT -p $IP_PREFIX
		log "Configuring Redis cluster on ${INSTANCE_COUNT} nodes with ${SLAVE_COUNT} slave(s) for every master node"

		# Install the Ruby runtime that the cluster configuration script uses
		apt-get -y install ruby-full

		# Install the Redis client gem (a pre-requisite for redis-trib.rb)
		gem install redis

		# Create a cluster based upon the specified host list and replica count
		echo "yes" | /usr/local/bin/redis-trib.rb create --replicas ${SLAVE_COUNT} $(expand_ip_range "${IP_PREFIX}-${INSTANCE_COUNT}")

		log "Redis cluster was configured successfully"
	fi
}

#############################################################################
configure_redis_replication()
{
	log "Configuring master-slave replication"
	
	if [ "$NODE_INDEX" -lt "$MASTER_NODE_COUNT" ]; then
		log "Redis node ${HOSTNAME} is considered a MASTER, no further configuration changes are required"
	else
		log "Redis node ${HOSTNAME} is considered a SLAVE, additional configuration changes will be made"
		
		let MASTER_NODE_INDEX=$NODE_INDEX%$MASTER_NODE_COUNT
		MASTER_NODE_IP="${IP_PREFIX}${MASTER_NODE_INDEX}"
		
		echo "slaveof ${MASTER_NODE_IP} ${REDIS_PORT}" >> /etc/redis/redis.conf
		log "Redis node ${HOSTNAME} is configured as a SLAVE of ${MASTER_NODE_IP}:${REDIS_PORT}"
	fi
}

#############################################################################
configure_sentinel()
{
	let MASTER_NODE_INDEX=$NODE_INDEX%$MASTER_NODE_COUNT
	MASTER_NODE_IP="${IP_PREFIX}${MASTER_NODE_INDEX}"

	# Patch the sentinel configuration file with a new master
	sed -i "s/^sentinel monitor.*$/sentinel monitor mymaster ${MASTER_NODE_IP} ${REDIS_PORT} ${MASTER_NODE_COUNT}/g" /etc/redis/sentinel.conf

	# Make a writable log file
	touch /var/log/redis-sentinel.log
	chown redis:redis /var/log/redis-sentinel.log
	chmod u+w /var/log/redis-sentinel.log
	
	# Change owner for /etc/redis/ to allow sentinel change the configuration files
	chown -R redis.redis /etc/redis/

	# Start the script automatically at boot time
	update-rc.d redis-sentinel defaults
}

#############################################################################
start_redis()
{
	# Start the Redis daemon
	/etc/init.d/redis-server start
	log "Redis daemon was started successfully"
}



# Step1
tune_system
tune_memory
tune_network

# Step 2
install_redis

# Step 3
configure_redis

# Step 4
configure_redis_cluster
configure_redis_replication()


# Step 5
start_redis

