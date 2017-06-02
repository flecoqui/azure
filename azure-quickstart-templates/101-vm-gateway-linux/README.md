# Deployment of a simple VM (Linux or Windows) with Web Server (Apache or IIS) and iperf3 components behind a Virtual Machine acting as NAT gateaway and with port forwarding

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-vm-bridge-linux%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-vm-bridge-linux%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


This template allows you to deploy a simple VM running: </p>
#### Debian: Apache and Iperf3 ,
#### Ubuntu: Apache and Iperf3, 
#### Centos: Apache and Iperf3, 
#### Red Hat: Apache and Iperf3,
#### Windows Server 2016: IIS and Iperf3,
#### Nano Server 2016: IIS and Iperf3
This template will deploy in the region associated with Resource Group a simple VM (Linux or Windows) with Web Server (Apache or IIS) and iperf3 components behind a Virtual Machine acting as NAT gateaway and with port forwarding.
THe objecitve of this template is to show how to capture the traffic between the simple VM (Linux or Windows) running a Web Server (Apache or IIS) and iperf3 with this Virtual Machine acting as NAT gateaway and with port forwarding.

Actually, it's like a migration from this architecture:

![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-bridge-linux/Docs/1-architecture.png)

to this architecture:

![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-vm-bridge-linux/Docs/2-architecture.png)


With Azure CLI you can deploy those VMs with 2 command lines:

## CREATE RESOURCE GROUP:
azure group create "ResourceGroupName" "DataCenterName"

For instance:

    azure group create gateawaygrp eastus2

## DEPLOY THE VM:
azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploy.json -e azuredeploy.parameters.json

For instance:

    azure group deployment create gateawaygrp -f azuredeploy.json -e azuredeploy.parameters.json -vv

Beyond login/password for the 2 virtual machine, the input parameters are :</p>
configurationSize (Small: F1 and 128 GB data disk, Medium: F2 and 256 GB data disk, Large: F4 and 512 GB data disk, XLarge: F4 and 1024 GB data disk) : 

    "configurationSize": {
      "type": "string",
      "defaultValue": "Small",
      "allowedValues": [
        "Small",
        "Medium",
        "Large",
        "XLarge"
      ],
      "metadata": {
        "description": "Configuration Size: VM Size + Disk Size"
      }
    }

configurationOS (debian, ubuntu, centos, redhat, nano server 2016, windows server 2016): 

    "configurationOS": {
      "type": "string",
      "defaultValue": "debian",
      "allowedValues": [
        "debian",
        "ubuntu",
        "centos",
        "redhat",
        "nanoserver2016",
        "windowsserver2016"
      ],
      "metadata": {
        "description": "The Operating System to be installed on the VM. Default value debian. Allowed values: debian,ubuntu,centos,redhat,nanoserver2016,windowsserver2016"
      }
    },

## FORWARDING PORTS:
The Virtual Machine running the NAT Gateway has been configured to forward the following ports to the Virtual Machine in the backend:
- port 80:		http for the Web Server 
- port 443:		https for the Web Server 
- port 5201:	iperf3 server
- port 2222:	ssh for the Linux Virtual Machine in the backend
- port 3389:	rdp for the Windows Server Virtual Machine in the backend
- port 5985:	rdp for the Nano Server Virtual Machine in the backend
- port 5986:	rdp for the Nano Server Virtual Machine in the backend


Below the bash commands to confgure the NAT gateway to forward the ports:

iptables -t nat -A PREROUTING -p tcp  -d 10.0.1.5/32 --dport 80 -j DNAT --to-destination 10.0.0.4:80
iptables -t nat -A POSTROUTING  -d 10.0.0.4 -p tcp --dport 80 -j MASQUERADE
iptables -t nat -A PREROUTING -p tcp  -d 10.0.1.5/32 --dport 443 -j DNAT --to-destination 10.0.0.4:443
iptables -t nat -A POSTROUTING  -d 10.0.0.4 -p tcp --dport 443 -j MASQUERADE# Forwarding port 5201
iptables -t nat -A PREROUTING -p tcp  -d 10.0.1.5/32 --dport 5201 -j DNAT --to-destination 10.0.0.4:5201
iptables -t nat -A POSTROUTING  -d 10.0.0.4 -p tcp --dport 5201 -j MASQUERADE
iptables -t nat -A PREROUTING -p udp  -d 10.0.1.5/32 --dport 5201 -j DNAT --to-destination 10.0.0.4:5201
iptables -t nat -A POSTROUTING  -d 10.0.0.4 -p udp --dport 5201 -j MASQUERADE# Forwarding port 3389
iptables -t nat -A PREROUTING -p tcp  -d 10.0.1.5/32 --dport 3389 -j DNAT --to-destination 10.0.0.4:3389
iptables -t nat -A POSTROUTING  -d 10.0.0.4 -p tcp --dport 3389 -j MASQUERADE
iptables -t nat -A PREROUTING -p tcp  -d 10.0.1.5/32 --dport 5985 -j DNAT --to-destination 10.0.0.4:5985
iptables -t nat -A POSTROUTING  -d 10.0.0.4 -p tcp --dport 5985 -j MASQUERADE
iptables -t nat -A PREROUTING -p tcp  -d 10.0.1.5/32 --dport 5986 -j DNAT --to-destination 10.0.0.4:5986
iptables -t nat -A POSTROUTING  -d 10.0.0.4 -p tcp --dport 5986 -j MASQUERADE
iptables -t nat -A PREROUTING -p tcp  -d 10.0.1.5/32 --dport 2222 -j DNAT --to-destination 10.0.0.4:22
iptables -t nat -A POSTROUTING  -d 10.0.0.4 -p tcp --dport 22 -j MASQUERADE



## TESTING THE DEPLOYMENT:
Once the VM has been deployed, you can open the Web page hosted on the VM.
For instance for Linux VM:

     http://vmubus001.eastus2.cloudapp.azure.com/index.php 

for Windows VM:

     http://vmnanos001.eastus2.cloudapp.azure.com/index.html 

</p>
You can also use Iperf3 to test the ingress/egress between the VM and an Iperf3 client.
For instance for Linux VM:

     iperf3 -c vmubus001.eastus2.cloudapp.azure.com -p 5201

</p>
Finally, you can open a remote session with the Virtual Machines.

For instance for Linux VM running the NAT Gateway:

     ssh VMAdmin@vmubus001.eastus2.cloudapp.azure.com

For instance for Linux VM:

     ssh -p 2222 VMAdmin@vmubus001.eastus2.cloudapp.azure.com

For Windows Server VM:

     mstsc /admin /v:vmwins001.eastus2.cloudapp.azure.com

For Nano Server VM:

     Set-Item WSMan:\\localhost\\Client\\TrustedHosts vmnanos001.eastus2.cloudapp.azure.com </p>
     Enter-PSSession -ComputerName vmnanos001.eastus2.cloudapp.azure.com </p>


## DELETE THE RESOURCE GROUP:
azure group delete "ResourceGroupName" "DataCenterName"

For instance:

    azure group delete gateawaygrp eastus2
