# Deployment of N Windows Server 2012/2016 VMs running IIS (port 80) and iperf3 (port 5201) behind a Load Balancer

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F201-lb-vms-windows2016-iperf%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2F%2Fmaster%2Fazure-quickstart-templates%2F201-lb-vms-windows2016-iperf%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


This template allows you to deploy up-to 4 Windows Server 2012/2016 VMs running IIS and iPerf3 behind a load balancer, using the latest patched version. This will deploy the VMs and the Load Balancer in the region associated with Resource Group.</p>
The parameters are:</p>
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
Load Balancer Public IP Address : port tcp 50002   ->   VM2 port tcp 3389</p>
Load Balancer Public IP Address : port tcp 50003   ->   VM3 port tcp 3389</p>

###iPerf TCP (command line: "iperf3 -c [PublicIPAddress] -p [5200X]")
Load Balancer Public IP Address : port tcp 52000   ->   VM0 port tcp 5201</p>
Load Balancer Public IP Address : port tcp 52001   ->   VM1 port tcp 5201</p>
Load Balancer Public IP Address : port tcp 52002   ->   VM2 port tcp 5201</p>
Load Balancer Public IP Address : port tcp 52003   ->   VM3 port tcp 5201</p>

###iPerf UDP (command line: "iperf3 -c [PublicIPAddress] -u -p [5200X]")
Load Balancer Public IP Address : port tcp 52000   ->   VM0 port udp 5201</p>
Load Balancer Public IP Address : port tcp 52001   ->   VM1 port udp 5201</p>
Load Balancer Public IP Address : port tcp 52002   ->   VM2 port udp 5201</p>
Load Balancer Public IP Address : port tcp 52003   ->   VM3 port udp 5201</p>

With Azure CLI you can deploy these VMs and the Load Balancer with 2 following command lines:

##CREATE RESOURCE GROUP:
azure group create "ResourceGroupName" "DataCenterName"

For instance:

    azure group create iperflbgrp eastus2

##DEPLOY THE VM:
azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploy.json -e azuredeploy.parameters.json

For instance:

    azure group deployment create iperflbgrp depiperftest -f azuredeploy.json -e azuredeploy.parameters.json -vv

##DELETE THE RESOURCE GROUP:
azure group delete "ResourceGroupName" "DataCenterName"

For instance:

    azure group delete iperflbgrp eastus2
