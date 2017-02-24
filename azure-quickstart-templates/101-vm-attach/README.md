# Create a simple VM (Windows or Linux) from an existing VHD file in a storage account in the same region

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-vm-attach%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-vm-attach%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


This template allows you to deploy create a simple VM (Windows or Linux) from an existing VHD file in a storage account in the same region.
This template is useful if you need to create a VM in Azure from an existing VHD stored in a storage account in the same region.

##USING THIS TEMPLATE:

For instance, if you have an existing VirtualBox VM, you need first to convert the image of this VM into a VHD file with fixed size.
 
1. Convert the existing VM image file into a RAW file , command below:</p>

 VBoxManage.exe clonemedium [path-to-existing-VM-image] [path-to-the-raw-file] --format RAW 

2. Convert the RAW file into a VHD file with fixed size, command below:</p>
 
 VBoxManage.exe convertfromraw  [path-to-the-raw-file] [path-to-the-vhd-file] --format VHD --variant Fixed 

3. With tools like Azure Storage Explorer [here](http://storageexplorer.com/) upload the VHD file with fixed size on your Storage Account in your Region. The VHD file uploaded will be defined with:</p>
	- the Storage Account Name</p>
	- the Container Name</p>
	- the VHD File Name</p>
4. Create a resource group in the same region with Azure CLI, command below:</p>

azure group create attachgrp [region]

6. Use the current ARM template (attach option) to create your VM from the existing VHD file, command below:
azure group deployment create attachgrp attachdep -f azuredeploy.json -e azuredeploy.parameters.json  -vv 
Using the following parameters:

Storage Account Name where the VHD file is stored:

    "userImageStorageAccountName": {
      "type": "string",
      "metadata": {
        "description": "This is the name of the your storage account"
      }
    },

Container Name where the VHD file is stored:

    "userImageStorageContainerName": {
      "type": "string",
      "metadata": {
        "description": "This is the name of the container in your storage account"
      }
    },

VHD file Name:

    "userImageVhdName": {
      "type": "string",
      "metadata": {
        "description": "This is the name of the your customized VHD"
      }
    },

DNS prefix which will be associated with the VM:

    "dnsNameForPublicIP": {
      "type": "string",
      "metadata": {
        "description": "Unique DNS Name for the Public IP used to access the Virtual Machine."
      }
    },

OS Type (Windows or linux):

    "osType": {
      "type": "string",
      "allowedValues": [
        "windows",
        "linux"
      ],
      "metadata": {
        "description": "This is the OS that your VM will be running"
      }


With Azure CLI you can create this VM with 2 command lines:

##CREATE RESOURCE GROUP:
azure group create "ResourceGroupName" "DataCenterName"

For instance:

    azure group create attachgrp eastus2

##DEPLOY THE VM:
azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploy.json -e azuredeploy.parameters.json

For instance:

    azure group deployment create attachgrp -f azuredeploy.json -e azuredeploy.parameters.json -vv

##DELETE THE RESOURCE GROUP:
azure group delete "ResourceGroupName" "DataCenterName"

For instance:

    azure group delete attachgrp

