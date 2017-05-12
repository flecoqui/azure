# Sample Application to generate automatically subtitles files in several languages from an original video or audio file using Azure Services


This sample application allows you to generate automatically subtitles files in several languages from an original video or audio file using Azure Services like 
1. a Web App to play and check the audio, video and subtitles, 
2. an Azure Media Services Account to generate subtitles from the original video or audio file), 
3. an Azure Cognitive Services Text Translator service to translate the original subtitles into several languages
4. an Azure Search Account to index all the subtitles associated with the audio or video files.

As Azure Media Services, Search Service and Cognitive Services are not deployed in all regions, it's recommanded to use one of the following regions:
West US, West Europe,Southeast Asia,West Central US 
The Azure backend required to run this application can the installed using the Azure Resource Manager template below:
https://github.com/flecoqui/azure/tree/master/azure-quickstart-templates/101-media-search-cognitive  

Below the architecture of this Azure deployment:

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture.png)


## INSTALLING THE BACKEND SERVICES IN AZURE:

Before using the sample application you need to install the Azure backend with all the services associated.
You can either use the Azure Portal The Azure Resource Manager template is available there:
![Azure Portal](https://portal.azure.com) 

![Azure ARM Template](https://github.com/flecoqui/azure/tree/master/azure-quickstart-templates/101-media-search-cognitive) 


azure group create "ResourceGroupName" "DataCenterName"

For instance:

    azure group create testamsseacog northeurope
	azure group deployment create testamsseacog depiperftest -f azuredeploy.json -e azuredeploy.parameters.json -vv

## BUILD THE APPLICATION TESTAZUREMEDIAINDEXER:

**Prerequisite: Visual Studio 2015 or 2017**

1. If you download the samples ZIP, be sure to unzip the entire archive, not just the folder with the sample you want to build. 
3. Start Microsoft Visual Studio 2015 or 2017 and select **File** \> **Open** \> **Project/Solution**.
3. Starting in the folder where you unzipped the samples, go to the Samples subfolder, then the subfolder for this specific sample. Double-click the Visual Studio 2015/2017 Solution (.sln) file.
4. Press Ctrl+Shift+B, or select **Build** \> **Build Solution**.

**Deploying and running the sample**
1.  To debug the sample and then run it, press F5 or select **Debug** \> **Start Debugging**. To run the sample without debugging, press Ctrl+F5 or select **Debug** \> **Start Without Debugging**.

**Downloading the binary**
The binary associted with the application is available there:
![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Releases/Latestrelease.zip)

1. You can download the zip file.
2. Unzip the LatestRelease.zip file
3. Run locally TestAzureMediaIndexer.exe

## USING THE APPLICATION TESTAZUREMEDIAINDEXER
This sample application is a basic Windows Application with one single page:

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-0.png)


## CONNECT THE APPLICATION TO THE AZURE BACKEND
In order to use the application you need to provide the following paramters to establish a connection with your backend in Azure:
1. The Azure Media Serivces Account name
2. The Azure Media Serivces Account key
3. The Azure Cognitive Services Text Translator key
4. The Azure Search Serivces Account name
5. The Azure Search Serivces Account key
6. The url of the Web Player application hosted on the Web site of your backend, the url should be close to this format: http://YourWebAppName.azurewebsites.net/player.html

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-1.png)

You can retrieve all the parameters below from the ![Azure Portal:](https://portal.azure.com) 
1. The Azure Media Serivces Account name
2. The Azure Media Serivces Account key
3. The Azure Cognitive Services Text Translator key
4. The Azure Search Serivces Account name
5. The Azure Search Serivces Account key
6. The url of the Web Player application hosted on the Web site 

Once all the parameters are ready click on the button "Connect" to establish the connection with your backend:
![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-1.png)


## UPLOAD THE AUDIO AND VIDEO ASSETS
Once you are connected, the first step consists in uploading video or audio assets on your backend in Azure.

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-2.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-2.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-2-1.png)


## GENERATE THE SUBTITLES FROM THE AUDIO AND VIDEO ASSETS

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-3.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-3.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-3-1.png)


## UPDATE THE GENERATED SUBTITLES WITH THE WEB APPLICATION

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-4.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-4.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-4-1.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-4-2.png)



![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-4-3.png)



![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-4-4.png)





## TRANSLATE THE GENERATED SUBTITLES WITH AZURE COGNITIVE SERVICES TEXT TRANSLATOR 

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-5.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-5.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-5-1.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-5-2.png)



![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-5-3.png)



## USE AZURE SEARCH TO INDEX THE SUBTITLES FILES ASSOCIATED WITH YOUR AUDIO AND VIDEO CONTENT

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-6.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-6.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-6-1.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-6-2.png)



![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-6-3.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-6-4.png)


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

Azure SEarch SKU:

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

Azure Cognitive Services SKU:

    "cognitiveSku": {
      "type": "string",
      "defaultValue": "S1",
      "allowedValues": [
        "F0",
        "P0",
        "P1",
        "P2",
        "S0",
        "S1",
        "S2",
        "S3",
        "S4",
        "S5",
        "S6"
      ],
      "metadata": {
        "description": "The SKU of the search service you want to create. E.g. free or standard"
      }
    },

Azure Web App SKU:

    "webSku": {
      "type": "string",
      "defaultValue": "F1",
      "allowedValues": [
        "F1",
        "D1",
        "B1",
        "B2",
        "B3",
        "S1",
        "S2",
        "S3",
        "P1",
        "P2",
        "P3",
        "P4"
      ],
      "metadata": {
        "description": "The SKU of the Web service you want to create. E.g. free or standard"
      }
    }


With Azure CLI you can create this VM with 2 command lines:



## DELETE THE RESOURCE GROUP:
azure group delete "ResourceGroupName" "DataCenterName"

For instance:

    azure group delete testamsseacog northeurope

