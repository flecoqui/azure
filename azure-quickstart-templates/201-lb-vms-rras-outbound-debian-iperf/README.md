# Deployment of N VMs running Apache (port 80) and iperf3 (port 5201) behind a Load Balancer and N VM in a backend network using the same IP address for the outbound traffic.

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F201-lb-vms-rras-outbound-debian-iperf%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2F%2Fmaster%2Fazure-quickstart-templates%2F201-lb-vms-rras-outbound-debian-iperf%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


This template allows you to deploy the following configuration:
An Azure Load Balancer which routes the inbound traffic towards an availability set of VMs running a Web Site and iperf3 in server mode in the frontend subnet.
Moreover, all the VMs in the backend subnet must use the same outbount IP address as the service they need to reach is protected with a whitelist of IP addresses.
A route table associated with the backend subnet will route all the outbound traffic towards a VM running RRAS between two NIC cards: one connected to the backend subnet and the second one connected a a public IP address.
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-debian-iperf/Docs/architecture.png)
</p>
###The main parameters are:</p>
- The VM oS Debian Version (7, 8)</p>
- The VM size for the VM in the frontend subnet</p>
- The number of VM in the frontend subnet</p>
- The VM size for the VM in the backend subnet</p>
- The number of VM in the backend subnet</p>
- The VM size for the forward VM. As this VM needs to support 2 NIC card, you need to select at least D2 VMs.</p>


vmSize (Standard_AX, Standard_DX, Standard_DX_v2, Standard_FX, ...) : 

    "vmSize": {
      "type": "string",
      "metadata": {
        "description": "VM Size"
      },
      "defaultValue": "Standard_A1"
    },

vmCount (from 1 to 4): 

    "vmCount": {
      "type": "int",
      "metadata": {
        "description": "Number of VMs"
      },
      "defaultValue": 2,
      "allowedValues": [
        1,
        2,
        3,
        4
      ]
    },

osVersion (7, 8):

    "osVersion": {
      "type": "string",
      "defaultValue": "8",
      "allowedValues": [
        "7",
        "8"
      ],
      "metadata": {
        "description": "The Debian version for the VM. "
      }
    },

The Load Balancer is configured to forward the following ports:
###SSH TCP 22 (command line: "ssh -p 5000X VMAdmin@[PublicIPAddress]")
Load Balancer Public IP Address : port tcp 50000   ->   VM0 port tcp 22</p>
Load Balancer Public IP Address : port tcp 50001   ->   VM1 port tcp 22</p>

###iPerf TCP (command line: "iperf3 -c [PublicIPAddress] -p [5200X]")
Load Balancer Public IP Address : port tcp 52000   ->   VM0 port tcp 5201</p>
Load Balancer Public IP Address : port tcp 52001   ->   VM1 port tcp 5201</p>

###iPerf UDP (command line: "iperf3 -c [PublicIPAddress] -u -p [5200X]")
Load Balancer Public IP Address : port udp 52000   ->   VM0 port udp 5201</p>
Load Balancer Public IP Address : port udp 52001   ->   VM1 port udp 5201</p>

With Azure CLI you can deploy these VMs and the Load Balancer with 2 following command lines:

##CREATE RESOURCE GROUP:
azure group create "ResourceGroupName" "DataCenterName"

For instance:

    azure group create rraslbgrp eastus2

##DEPLOY THE VMs:
azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploy.json -e azuredeploy.parameters.json

For instance:

    azure group deployment create rraslbgrp deprraslbtest -f azuredeploy.json -e azuredeploy.parameters.json -vv


##CHECK THE OUTBOUND IP ADDRESS:

In order to valid the installation, you need to check the outbound IP Address.
</p>
1. Once the deployment is done in the output section you can see the ssh command line to establish a SSH session with the VMs in the frontend subnet.  
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-debian-iperf/Docs/1-ssh.png)
</p>
For instance: </p>

	 ssh -p 50000 VMAdmin@frontendvm.eastus2.cloudapp.azure.com
</p>
2. From the VMs in the frontend subnet, you can establish a SSH session with the VMs in the backend subnet.
</p>
For instance: </p>

	 ssh -p 22 VMAdmin@10.0.1.10
</p>
3. Once connected with the VM in the backend subnet, you need to install curl on your VM.</p>
Launch the following commands: </p>

</p>
	 sudo -i

</p>
	 apt-get -y install curl

</p>
4. Once curl is installed, run the following command to get the outbound IP address of the VM in the backend subnet
</p>
	 curl -s checkip.dyndns.org | sed -e 's/.*Current IP Address: //' -e 's/<.*$//'
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-debian-iperf/Docs/1-myip.png)
</p>
5. On this Azure portal check the Public IP address of the forward VM is the same:
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-debian-iperf/Docs/2-myip.png)
</p>


##DELETE THE RESOURCE GROUP:
azure group delete "ResourceGroupName" "DataCenterName"

For instance:

    azure group delete rraslbgrp eastus2
