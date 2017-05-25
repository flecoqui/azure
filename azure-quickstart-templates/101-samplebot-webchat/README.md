# Deploy a Web App, an Azure Media Services Account, an Azure Search Account and an Azure Text Translator service in the same region

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-media-search-cognitive%2Fazuredeploy.json" target="_blank">
    <img src="http://azuredeploy.net/deploybutton.png"/>
</a>
<a href="http://armviz.io/#/?load=https%3A%2F%2Fraw.githubusercontent.com%2Fflecoqui%2Fazure%2Fmaster%2Fazure-quickstart-templates%2F101-media-search-cognitive%2Fazuredeploy.json" target="_blank">
    <img src="http://armviz.io/visualizebutton.png"/>
</a>


This template allows you to deploy  a Web App, an Azure Media Services Account, an Azure Search Account and an Azure Text Translator service in the same region as the resource group.
As Azure Media Services, Search Service and Cognitive Services are not deployed in all regions, it's recommanded to use the following regions:
West US, West Europe,Southeast Asia,West Central US 
This template is associated with Windows Application which is used to generate automatically video subtitles in different languages fron an orginal video or audio file. Once generated the subtitles are stored in Azure Search to allow the users to find all the videos associated with a specific key word.
This Application called TestAzureMediaIndexer is available there:
https://github.com/flecoqui/azure/tree/master/Samples/TestAzureMediaIndexer 


![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-media-search-cognitive/Docs/1-architecture.png)


Once the backend services are installed, the storage account needs to be manually configured to support CORS (Cross-Origin Resource Sharing) using the Azure portal:

1. Select the Storage Account of your new resource group:</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-media-search-cognitive/Docs/cors-0.png)
2. Select CORS on the page of your Storage Account</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-media-search-cognitive/Docs/cors-1.png)
3. Click on the button Add to Add a CORS rule. Enter  * for **Allowed origins**, GET,POST and PUT for **Allowed verbs**, * for **Allowed headers**, * for **Exposed headers** and 5 for **Maximum age**.
Click on the Add button to Create the new rule.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-media-search-cognitive/Docs/cors-2.png)
4. Click on the Add button to Create the new rule. Once the rule is created the Web Player will be able to play audio/video files and subtitles files stored on the Storage Account.</p>
![](https://raw.githubusercontent.com/flecoqui/azure/master/azure-quickstart-templates/101-media-search-cognitive/Docs/cors-3.png)


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

