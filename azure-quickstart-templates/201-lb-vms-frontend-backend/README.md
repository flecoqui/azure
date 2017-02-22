# Deployment of <N> VMs running Apache (port 80) behind a Load Balancer and <M> Debian VM in a backend subnetwork.

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F201-lb-vms-frontend-backend%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2F%2Fmaster%2Fazure-quickstart-templates%2F201-lb-vms-frontend-backend%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


This template allows you to deploy the following configuration:
An Azure Load Balancer which routes the inbound traffic towards an availability set of VMs running a Web Site in server mode in the frontend subnet.
Moreover, VMs are also deployed in the backend subnet .
</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/201-lb-vms-frontend-backend/Docs/architecture.png)
</p>
###The main parameters are:</p>
- The VM provider, VM Offer, VM Sku and VM Version for the VM in the frontend</p>
- The VM size for the VM in the frontend subnet</p>
- The number of VM in the frontend subnet</p>
- The VM provider, VM Offer, VM Sku and VM Version for the VM in the backend</p>
- The VM size for the VM in the backend subnet</p>
- The number of VM in the backend subnet</p>
</p>


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

VM Publisher, Offer, Sku and Version:

    "frontendImagePublisher": {
      "type": "string",
      "metadata": {
        "description": "frontendImagePublisher"
      },
      "defaultValue": "credativ"
    },
    "frontendImageOffer": {
      "type": "string",
      "metadata": {
        "description": "frontendImageOffer"
      },
      "defaultValue": "Debian"
    },
    "frontendImageSku": {
      "type": "string",
      "metadata": {
        "description": "frontendImageSku"
      },
      "defaultValue": "8"
    },
    "frontendImageVersion": {
      "type": "string",
      "metadata": {
        "description": "frontendImageVersion"
      },
      "defaultValue": "latest"
    },


The Load Balancer is configured to forward the following ports:
###SSH TCP 22 (command line: "ssh -p 5000X VMAdmin@[PublicIPAddress]")
Load Balancer Public IP Address : port tcp 50000   ->   VM0 port tcp 22</p>
Load Balancer Public IP Address : port tcp 50001   ->   VM1 port tcp 22</p>

With Azure CLI you can deploy these VMs and the Load Balancer with 2 following command lines:

##CREATE RESOURCE GROUP:
azure group create "ResourceGroupName" "DataCenterName"

For instance:

    azure group create lbfrbagrp eastus2

##DEPLOY THE VMs:
azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploy.json -e azuredeploy.parameters.json

For instance:

    azure group deployment create lbfrbagrp deprraslbtest -f azuredeploy.json -e azuredeploy.parameters.json -vv

##DELETE THE RESOURCE GROUP:
azure group delete "ResourceGroupName" "DataCenterName"

For instance:

    azure group delete lbfrbagrp 
