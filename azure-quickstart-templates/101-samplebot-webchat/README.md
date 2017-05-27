# Deploy a Web App hosting a sample Bot with Web Chat and Skype channels and a Virtual Machine running Linux (debian, ubuntu, centos, redhat) and an Apache/PHP server with Web Chat control and a link to Skype

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-samplebot-webchat%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-samplebot-webchat%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


This template allows you to deploy  Deploy a Web App hosting a sample Bot with Web Chat and Skype channels and a Virtual Machine running Linux (debian, ubuntu, centos, redhat) and an Apache/PHP server with Web Chat control and a link to Skype. All those resources will be deployed in the same region as the resource group.

![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/1-architecture.png)

This template allows you to deploy a simple VM running: </p>
- Debian (version 8): Apache and php with Web chat control and link to Skype,
- Ubuntu (version 16.04): Apache and php with Web chat control and link to Skype, 
- Centos (version 7.2): Apache and php with Web chat control and link to Skype,
- Red Hat (version 7.2): Apache and php with Web chat control and link to Skype.

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
6. In the "Configuration" section, click on "Create Microsoft App ID and password" button to register your App ID and password.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/4-install.png)
7. On the page below, enter your "App name" and click on the "Generate an App  password to continue" button.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/5-install.png)
8. On the subsequent page, copy the new password, you'll need this password to deploy your Azure template.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/6-install.png)
9. Copy your App ID as well, you'll need this ID to deploy your Azure template.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/7-install.png)
10. Check the "Terms of use" box and click on the "Register" button to register your bot.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/8-install.png)
11. During this registration phase, we leave the field "Messaging endpoint" empty, we will fill this field when the bot will be deployed.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/9-install.png)
12. Once the box is created the message below is displayed.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/10-install.png)
13. By default 2 channels are created, the Skype channel and the Web Chat channel. Click on  the "Edit" link associated with the Web Chat channel</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/11-install.png)
14. The page "Configure Web Chat" is displayed, click on the "Add new site" link.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/13-install.png)
15. Fill the field "Site name" and click on the "Done" button .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/14-install.png)
16. On the subsequent page, copy the "Secret keys" clicking on the "Show" link. You'll need this "Secret key" to deploy your Azure template.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/15-install.png)
17. The registration of your bot is now done, you know the "App ID" and the "Password" of your bot, and the "Secret Key" of the Web Chat channel. You are now ready to deploy the Azure template.

## CREATE RESOURCE GROUP:

Using Azure CLI you can run the following command to create the resource group associated with your deployment:

azure group create "ResourceGroupName" "RegionName"

For instance:

    azure group create testbotgrp northeurope

## DEPLOY THE SERVICES:

Once the resource group is created, you can launch the deployment using Azure CLI:

azure group deployment create "ResourceGroupName" "DeploymentName"  -f azuredeploy.json -e azuredeploy.parameters.json

For instance:

    azure group deployment create testbotgrp depiperftest -f azuredeploy.json -e azuredeploy.parameters.json -vv
 
You can also launch the deployment with the Azure portal clicking on the button below: 

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-samplebot-webchat%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>

Through the portal or Azure CLI you'll need to define the following parameters before launching the deployment. If you use Azure CLI you'll need to fill the file azuredeploy.parameters.json, if you use the Azure Portal, you'll fill directly the fields on the portal:

The Bot Name prefix which will be used to deploy your bot in a Web App. </p>
The url of the Web App will be : https://[Bot Name Prefix]bot.azurewebsites.net,</p>
the messaging endpoint url will be : https://[Bot Name Prefix]bot.azurewebsites.net/api/messages :

    "namePrefix": {
      "defaultValue": "Bot Name prefix",
      "type": "string",
      "metadata": {
        "description": "Bot Name prefix"
      }
    }

The Bot sku :

    "sku": {
      "type": "string",
      "allowedValues": [
        "Free",
        "Shared",
        "Basic",
        "Standard"
      ],
      "defaultValue": "Free",
      "metadata": {
        "description": "Bot Sku: Free, Shared, Basic, Standard"
      }
    }

The Bot worker size:

    "workerSize": {
      "type": "string",
      "allowedValues": [
        "0",
        "1",
        "2"
      ],
      "defaultValue": "0",
      "metadata": {
        "description": "Bot Worker Size: 0, 1, 2"
      }
    }


The Microsoft App ID associated with your Bot, you got this parameter after the registration phase:


    "MICROSOFT_APP_ID": {
      "type": "string",
      "metadata": {
        "description": "Bot Application ID"
      }
    }


The Microsoft App Password associated with your Bot, you got this parameter after the registration phase:


    "MICROSOFT_APP_PASSWORD": {
      "type": "string",
      "metadata": {
        "description": "Bot Application Password"
      }
    }


The Web Chat channel secret key associated with your Bot, you got this parameter after the registration phase:


    "WEBCHAT_SECRET": {
      "type": "string",
      "metadata": {
        "description": "Bot WebChat Secret"
      }
    }


The administrator user name of the Virtual Machine running Linux and Apache/PHP:


    "adminUsername": {
      "type": "string",
      "metadata": {
        "description": "Administrator User name for the Virtual Machine."
      }
    }


The administrator password of the Virtual Machine running Linux and Apache/PHP:


    "adminPassword": {
      "type": "securestring",
      "metadata": {
        "description": "Password for the Virtual Machine."
      }
    }


The DNS name prefix of your Virtual Machine running Linux and Apache/PHP, the public DNS entry of your Virtual Machine will be [dnsLabelPrefix].[Region].cloudapp.azure.com:


    "dnsLabelPrefix": {
      "type": "string",
      "metadata": {
        "description": "Unique DNS Name for the Public IP used to access the Virtual Machine. DNS name: <dnsLabelPrefix>.<Region>.cloudapp.azure.com"
      }
    }

The Linux distribution of your Virtual Machine  (debian, ubuntu, centos, redhat): 

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




The configuration size of your virtual machine (Small: F1 and 128 GB data disk, Medium: F2 and 256 GB data disk, Large: F4 and 512 GB data disk, XLarge: F4 and 1024 GB data disk) : 

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


## COMPLETING THE BOT REGISTRATION

Now the Web App running your Bot has been deployed, you now need to associate this Web App with your Bot registration on the Bot Framework Web Site https://dev.botframework.com/ :

1. To complete the configuration of your Bot, you need to define the "Messaging Endpoint" of your bot, if you did the deployment with Azure CLI, this information has been displayed at the end of the Azure CLI. Otherwise, the syntax of this url is  https://[Bot Name Prefix]bot.azurewebsites.net/api/messages .</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/0-configure.png)
2. With your Browser open the url https://dev.botframework.com/ , on the new page select your bot registered during the first step of this deployment. Click on the "Settings" menu.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/1-configure.png)
3. In the "Configuration" section, enter the "messaging endpoint" of your Web App:</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/2-configure.png)
4. Click on the "Save changes" button. Your Bot is now fully configured</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/3-configure.png)


## TESTING THE BOT

1. With your Browser open the url https://dev.botframework.com/ , on the new page select your bot registered during the first step of this deployment. Click on the "Test" button on the menu bar.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/1-test.png)
2. In the Test page, enter a message in the "Type your message" field. Check that you get a response from the bot.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/2-test.png)
3. Now, let's test the Apache Server running in the Virtual Machine. With your Browser of the url  http://[dnsLabelPrefix].[Region].cloudapp.azure.com/index.php. If you did the deployment with Azure CLI, , this url has been displayed at the end of the Azure CLI. The page will display the public IP address of the virtual machine, the Web Chat control and the link to Skype to add the bot to your contacts.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/3-test.png)
4. In the Web Chat control enter a message, check that you get a response from the bot.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/4-test.png)
5. Click on the "Add to Skype" button to add your Bot to your Skype's contacts</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/5-test.png)
6. Launch Skype on your machine, you can see your bot in the list of your contacts.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/6-test.png)
7. Enter a message, check that you get a response from the bot.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-samplebot-webchat/Docs/7-test.png)


## UNDER THE HOOD:
The [script](https://github.com/flecoqui/azure/blob/master/azure-quickstart-templates/101-samplebot-webchat/install-software.sh) used to install and configure the Virtual Machine running Apache/PHP server is called from the Azure Resource Manager Template.
This script is called with 3 parameters:
- the first parameter is the machine hostname
- the second parameter is the Web Chat secret key associated with the bot.
- the third parameter is the App ID associated with the bot.

```
    # Parameter 2 Bot WebChat Secret 
    webchat_secret=$2
    webchat_url=https://webchat.botframework.com/embed/mynewsamplebot?s=$webchat_secret
    # Parameter 3 Bot Application ID 
    skype_appid=$3
    skype_url=https://join.skype.com/bot/$skype_appid
```

The second parameter the Web Chat secret key is used to embed the Web Chat control in the PHP page.
The third parameter the App ID is used to embed the link to the Skype page to add the bot to your Skype contats.


```
    <p>This is the home page of a VM running on Azure</p>
    <p>Below the WebChat page for the Bot: </p>
	<iframe src="$webchat_url"></iframe>
    <p></p>
    <p></p>
    <p></p>
    <p>Below the link to add the Bot to your Skype contacts: </p>
	<a href="$skype_url">
	<img src="https://dev.botframework.com/Client/Images/Add-To-Skype-Buttons.png"/>
	</a>
```



## DELETE THE RESOURCE GROUP:
When you don't need to test your bot anymore, you can remove all those resources from Azure using Azure CLI.
You can run the command below:

azure group delete "ResourceGroupName" "RegionName"

For instance:

    azure group delete testbotgrp northeurope

