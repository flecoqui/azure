# Create a simple VM (Windows or Linux) from an existing VHD file in a storage account in the same region

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-vm-createfromimage%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-vm-createfromimage%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


This template allows you to deploy create a simple VM (Windows) from an existing custom image VHD file in a storage account in the same region.
This template is useful if you need to create a VM in Azure from an existing image stored in the same storage account in the same region.
It's actually a 5 steps installation:
1. You create first the resource group (Azure CLI command)
2. Then the Storage Account associated with the resource group (Azure CLI command to deploy the resource)
3. Create a container for the image file in the new storage account (Azure CLI command)
4. Copy the image VHD file in the new container (Azure CLI command)
5. Deploy the VM and the network (Azure CLI command to deploy the resources)

## USING THIS TEMPLATE:

First you need to create the Windows Image from an existing Azure Imagerunning in Azure.

1. You run sysprep on the existing VM 
2. Stop the existing VM
3. Run the following powershell command to initialize the variable

     $rgName = "testvm2012grp"
     $location = "EastUS2"
     $snapshotName = "vm2012iisSnapshot"
     $imageName = "vm2012iisImage"


4. Run the following powershell command to generalize the image 

    Set-AzureRmVm -ResourceGroupName testvm2012grp -Name testvm2012vm -Generalized


5. Run the following powershell commands to check the generalization of  the image 

    $vm = Get-AzureRmVM -ResourceGroupName testvm2012grp -Name testvm2012vm -Status
    $vm.Statuses

6. Run the following powershell commands to create the image in the same storage account as the one used by the existing VM 

    Save-AzureRmVMImage -ResourceGroupName testvm2012grp -Name testvm2012vm  -DestinationContainerName image -VHDNamePrefix vm2012iis    -Path C:\temp\vm2012iisimage.json

Once the image is created, you are ready to deploy the service associated with these ARM templates.


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


With Azure CLI you can create this VM with the following 5 command lines:

## CREATE RESOURCE GROUP:
azure group create "ResourceGroupName" "DataCenterName"

For instance:

    azure group create imagegrp eastus2

## DEPLOY THE STORAGE ACCOUNT:
azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploystorage.json -e azuredeploystorage.parameters.json

For instance:

    azure group deployment create imagegrp -f azuredeploystorage.json -e azuredeploystorage.parameters.json -vv


## DEPLOY THE VM:
azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploy.json -e azuredeploy.parameters.json

For instance:

    azure group deployment create attachgrp -f azuredeploy.json -e azuredeploy.parameters.json -vv

## DELETE THE RESOURCE GROUP:
azure group delete "ResourceGroupName" "DataCenterName"

For instance:

    azure group delete attachgrp

