# Deploy a Web App hosting a sample Bot with Web Chat and Skype connector and a Virtual Machine running Linux (debian, ubuntu, centos, redhat) and an Apache/PHP server with Web Chat control and a link to Skype

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-samplebot-webchat%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-samplebot-webchat%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


This template allows you to deploy  Deploy a Web App hosting a sample Bot with Web Chat and Skype connector and a Virtual Machine running Linux (debian, ubuntu, centos, redhat) and an Apache/PHP server with Web Chat control and a link to Skype in the same region as the resource group.

![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/1-architecture.png)

This template allows you to deploy a simple VM running: </p>
#### Debian: Apache and php with Web chat control and link to Skype,
#### Ubuntu: Apache and php with Web chat control and link to Skype, 
#### Centos: Apache and php with Web chat control and link to Skype,
#### Red Hat: Apache and php with Web chat control and link to Skype.

Before deploying this Azure Resource Manager template you need to register your bot:
 
1. Select the Storage Account of your new resource group:</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/1-install.png)
2. Select CORS on the page of your Storage Account</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/2-install.png)
3. Click on the button Add to Add a CORS rule. Enter  * for **Allowed origins**, GET,POST and PUT for **Allowed verbs**, * for **Allowed headers**, * for **Exposed headers** and 5 for **Maximum age**.
Click on the Add button to Create the new rule.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/3-install.png)
4. Click on the Add button to Create the new rule. Once the rule is created the Web Player will be able to play audio/video files and subtitles files stored on the Storage Account.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/4-install.png)
4. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/5-install.png)
4. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/6-install.png)
4. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/7-install.png)
4. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/8-install.png)
4. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/9-install.png)
4. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/10-install.png)
4. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/11-install.png)
4. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/12-install.png)
4. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/13-install.png)
4. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/14-install.png)
4. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/15-install.png)


Moreover, the media files will be streamed from SAS locators which returns "Accept-Ranges: bytes" in the http headers. This http header is mandatory to play MP4 video files or MP3 audio files over http with Chrome browser.


## CREATE RESOURCE GROUP:
azure group create "ResourceGroupName" "DataCenterName"

For instance:

    azure group create testamsseacog northeurope

## DEPLOY THE SERVICES:
azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploy.json -e azuredeploy.parameters.json

For instance:

    azure group deployment create testamsseacog depiperftest -f azuredeploy.json -e azuredeploy.parameters.json -vv
 

Using the following parameters:

Name prefix which will be used to create Azure Media Serivces Account, Azure Storage Account,  Azure Search Account, Azure Cognitive Services Text Translation Account:

    "namePrefix": {
      "type": "string",
      "minLength": 2,
      "maxLength": 50,
      "metadata": {
        "description": "Service name prefix must only contain lowercase letters, digits or dashes, cannot use dash as the first two or last one characters, cannot contain consecutive dashes, and is limited between 2 and 50 characters in length."
      }

Azure Storage SKU associated with Azure Media Services, used to store video and audio files:

    "mediaStorageSku": {
      "type": "string",
      "defaultValue": "Standard_LRS",
      "allowedValues": [
        "Standard_LRS",
        "Standard_GRS",
        "Standard_RAGRS",
        "Premium_LRS"
      ],
      "metadata": {
        "description": "This is  Storage Account SKU associated with Azure Media Services"
      }
    },

Azure Search SKU:

    "searchSku": {
      "type": "string",
      "defaultValue": "free",
      "allowedValues": [
        "free",
        "basic",
        "standard",
        "standard2",
        "standard3"
      ],
      "metadata": {
        "description": "The SKU of the search service you want to create. E.g. free or standard"
      }
    },

Beyond login/password, the input parameters are :</p>
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
        "redhat"
      ],
      "metadata": {
        "description": "The Operating System to be installed on the VM. Default value debian. Allowed values: debian,ubuntu,centos,redhat,nanoserver2016,windowsserver2016"
      }
    },



With Azure CLI you can create this VM with 2 command lines:

1. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/0-configure.png)
2. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/1-configure.png)
3. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/2-configure.png)
4. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/3-configure.png)



1. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/1-test.png)
2. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/2-test.png)
3. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/3-test.png)
1. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/4-test.png)
2. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/5-test.png)
3. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/6-test.png)
3. Click on the Add button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/7-test.png)


## DELETE THE RESOURCE GROUP:
azure group delete "ResourceGroupName" "DataCenterName"

For instance:

    azure group delete testamsseacog northeurope

