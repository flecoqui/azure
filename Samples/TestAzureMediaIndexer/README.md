# Create a Windows Application used to generate automatically subtitles files in several languages from an original video or audio file



This template allows you to deploy  a Web App, an Azure Media Services Account, an Azure Search Account and an Azure Text Translator service in the same region as the resource group.
As Azure Media Services, Search Service and Cognitive Services are not deployed in all regions, it's recommanded to use the following regions:
West US, West Europe,Southeast Asia,West Central US 
This template is with an application which is used to generate automatically video subtitles in different languages. Once generated the subtitles are stored in Azure Search to allow the users to find all the videos associated with a specific key word.
https://github.com/flecoqui/azure/tree/master/Samples/TestAzureMediaIndexer 


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture.png)



## INSTALL THE BACKEND SERVICES IN AZURE:

The Azure Resource Manager template is available there:
https://github.com/flecoqui/azure/tree/master/azure-quickstart-templates/101-media-search-cognitive 

azure group create "ResourceGroupName" "DataCenterName"

For instance:

    azure group create testamsseacog northeurope
	azure group deployment create testamsseacog depiperftest -f azuredeploy.json -e azuredeploy.parameters.json -vv




## BUILD THE APPLICATION TESTAZUREMEDIAINDEXER:
tobe completed

## USE THE APPLICATION TESTAZUREMEDIAINDEXER


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-0.png)


## CONNECT THE APPLICATION TO THE AZURE BACKEND

to be completed

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-1.png)


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-1.png)


## UPLOAD THE AUDIO AND VIDEO ASSETS

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

