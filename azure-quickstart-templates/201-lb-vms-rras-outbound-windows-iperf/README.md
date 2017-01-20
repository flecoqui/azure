# Deployment of N VMs running IIS (port 80) and iperf3 (port 5201) behind a Load Balancer and N VM in a backend network using the same IP address for the outbound traffic.

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F201-lb-vms-rras-outbound-windows-iperf%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2F%2Fmaster%2Fazure-quickstart-templates%2F201-lb-vms-rras-outbound-windows-iperf%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


This template allows you to deploy the following configuration:
An Azure Load Balancer which routes the inbound traffic towards an availability set of VMs running a Web Site and iperf3 in server mode in the frontend subnet.
Moreover, all the VMs in the backend subnet must use the same outbount IP address as the service they need to reach is protected with a whitelist of IP addresses.
A route table associated with the backend subnet will route all the outbound traffic towards a VM running RRAS between two NIC cards: one connected to the backend subnet and the second one connected a a public IP address.
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-rras-outbound-windows-iperf/architecture.png)
</p>
###The main parameters are:</p>
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

osVersion (Windows 2008 R2, 2012, 2012 R2, 2016):

    "osVersion": {
      "type": "string",
      "metadata": {
        "description": "OS Version - Image SKU"
      },
      "defaultValue": "2016-Datacenter",
      "allowedValues": [
        "2008-R2-SP1",
        "2012-Datacenter",
        "2012-R2-Datacenter",
        "2016-Datacenter"
      ]
    },

The Load Balancer is configured to forward the following ports:
###RDP TCP 3389 (command line: "mstsc /admin /v:[PublicIPAddress:5000X]")
Load Balancer Public IP Address : port tcp 50000   ->   VM0 port tcp 3389</p>
Load Balancer Public IP Address : port tcp 50001   ->   VM1 port tcp 3389</p>

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

##DEPLOY THE VM:
azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploy.json -e azuredeploy.parameters.json

For instance:

    azure group deployment create rraslbgrp deprraslbtest -f azuredeploy.json -e azuredeploy.parameters.json -vv

In order to complete the installation you need to configure manually the RRAS on the foward VM:


##DELETE THE RESOURCE GROUP:
azure group delete "ResourceGroupName" "DataCenterName"

For instance:

    azure group delete rraslbgrp eastus2
