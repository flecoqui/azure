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
#### Debian (version 8): Apache and php with Web chat control and link to Skype,
#### Ubuntu (version 16.04): Apache and php with Web chat control and link to Skype, 
#### Centos (version 7.2): Apache and php with Web chat control and link to Skype,
#### Red Hat (version 7.2): Apache and php with Web chat control and link to Skype.

## REGISTERING YOUR BOT:

Before deploying this Azure Resource Manager template you need to register your bot:
 
1. With your Browser open the url: https://dev.botframework.com/  </p>
2. Click on the link "Sign In" and use your Microsoft Account. If you don't have a Microsoft Account you can sign up there: https://signup.live.com/ </p>
3. Once connected to the botframework web site you can see the menu bar below::</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/1-install.png)
4. Click on the menu "My bots", you can see the page below. </p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/2-install.png)
5. Click on the button "Register", the page is displayed and fill the different fields like Display Name, Bot handle, Long description.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/3-install.png)
6. In the "Configuration" section, click on "Create Microsoft App ID and password" to register your App ID and password.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/4-install.png)
7. On the page below, enter your "App name" and click on the "Generate an App  password to continue" button.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/5-install.png)
8. On the subsequent page, copy the new password, you'll need this password to deploy your Azure template.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/6-install.png)
9. Copy your App ID as well, you'll need this ID to deploy your Azure template..</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/7-install.png)
10. Check the term of use box and click on the "Register" button to register your bot.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/8-install.png)
11. During this registration phase, we leave the field "Messaging endpoint" empty, we will fill this field when the bot will be deployed..</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/9-install.png)
12. Once the box is created the message below is displayed.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/10-install.png)
13. By default 2 channels are created, the Skype channel and the Web Chat channel. Click on  the "Edit" link associated with the Web Chat connector</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/11-install.png)
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/12-install.png)
14. The page Configure "Web Chat" is displayed, click on the "Add new site" link.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/13-install.png)
15. Fill the filed "Site name" and click on the "Done" button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/14-install.png)
16. On the subsequent page, copy the "Secret keys" clicking on the "Show" link. You'll need this "Secret key" to deploy your Azure template..</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/15-install.png)
17. The registration of your bot is now done, you know the "App ID" and the "Password" of your bot, and the "Secret Key" of the Web Chat channel. You are now ready to deploy the Azure template.

## CREATE RESOURCE GROUP:

Using Azure CLI you can run the following command to create the resource group associated with your deployment.

azure group create "ResourceGroupName" "DataCenterName"

For instance:

    azure group create testbotgrp northeurope

## DEPLOY THE SERVICES:

Once the resource group is created, you can launch the deployment using Azure CLI.

azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploy.json -e azuredeploy.parameters.json

For instance:

    azure group deployment create testbotgrp depiperftest -f azuredeploy.json -e azuredeploy.parameters.json -vv
 
You can also launch the deployment with the Azure portal clicking on the button below: 

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-samplebot-webchat%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>

Through the portal or Azure CLI you'll need to define the following parameter before launching the deployment:

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

