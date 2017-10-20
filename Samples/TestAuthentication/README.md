# WPF and UWP Sample Applications used to test Azure Media Services Authentication

Currently, Media Services supports the Azure Access Control service authentication model. However, **Access Control authorization** will be **deprecated on June 1, 2018**. It's recommended that you migrate to the Azure AD authentication model as soon as possible.

When you use Azure AD authentication with Azure Media Services, you have two authentication options:

1. **User authentication**. Authenticate a person who is using the app to interact with Media Services resources. The interactive application should first prompt the user for the user's credentials. An example is a management console app used by authorized users to monitor encoding jobs or live streaming. </p> 
2. **Service principal authentication**. Authenticate a service. Applications that commonly use this authentication method are apps that run daemon services, middle-tier services, or scheduled jobs. Examples are web apps, function apps, logic apps, API, and microservices.</p>

These samples WPF and UWP applications allow you to test Azure Media Services Authentication.
1. The **WPF Application** is based on WindowsAzure.MediaServices nuget package (namespace: Microsoft.WindowsAzure.MediaServices.Client)</p>
2. The **UWP Application** is based on REST API calls for Legacy and Service Principal Authentications. The Interactive User Authentication is based on ADAL Library (Namespace: Microsoft.IdentityModel.Clients.ActiveDirectory) and REST API calls</p>

As Azure Media Services are not deployed in all regions, it's recommanded to use one of the following regions:
West US, West Europe,Southeast Asia,West Central US 
The Azure Media Services backend required to run this application can the installed using the Azure Resource Manager template below:
https://github.com/flecoqui/azure/tree/master/azure-quickstart-templates/101-media-search-cognitive  

Below further information about Azure Media Services API with Azure AD authentication:

https://docs.microsoft.com/en-us/azure/media-services/media-services-use-aad-auth-to-access-ams-api


## INSTALLING THE BACKEND SERVICES IN AZURE:

Before using the sample applications you need to install the Azure Media Services backend.
You can either use the Azure Portal to deploy manually all the Azure Services:

https://portal.azure.com
 
or you can use directly the Azure Resource Manager template available there to deploy Azure Media Services. Along with Azure Media Services this template deploys Azure Search and Azure Cognitive Services:

https://github.com/flecoqui/azure/tree/master/azure-quickstart-templates/101-media-search-cognitive

Using the two Azure CLI command lines below,you can deploy automatically all the Azure Services required for the application: 

    azure group create testamsseacog northeurope
	azure group deployment create testamsseacog depiperftest -f azuredeploy.json -e azuredeploy.parameters.json -vv

Once the backend services are installed,

## BUILD THE APPLICATIONS TestWPFAuthentication and TestUWPAuthentication :

**Prerequisite: Windows 10 + Visual Studio 2015 or 2017**

1. If you download the samples ZIP, be sure to unzip the entire archive, not just the folder with the sample you want to build. 
3. Start Microsoft Visual Studio 2015 or 2017 and select **File** \> **Open** \> **Project/Solution**.
3. Starting in the folder where you unzipped the samples, go to the Samples subfolder, then the subfolder for this specific sample. Double-click the Visual Studio 2015/2017 Solution (.sln) file.
4. Press Ctrl+Shift+B, or select **Build** \> **Build Solution**.

**Deploying and running the sample**
1.  To debug the sample and then run it, press F5 or select **Debug** \> **Start Debugging**. To run the sample without debugging, press Ctrl+F5 or select **Debug** \> **Start Without Debugging**.


## USING THE APPLICATION TestWPFAuthentication 
This sample application is a basic Windows Application with one single page:

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAuthentication/Docs/UI_WPF_0.png)

1. To test the **Legacy Authentication**, you need to enter the "Legacy Account Name" and the "Legacy Account Key" associated with your Azure Media Services Account. Then you click on button "Azure Media Services Legacy Authentication".
2. To test the **Azure AD User Authentication**, you need to enter the "Azure Region" and the "Azure AD Tenant Domain" associated with your Azure Media Services Account. Then you click on button "Azure Media Services User Authentication".
3. To test the **Azure AD Service Principal Authentication**, you need to enter the "Application ID" and the "Application Key" associated with the application created on Azure Portal and associated with your Azure Media Services Account. Then you click on button "Azure Media Services Service Principal Authentication".


## USING THE APPLICATION TestUWPAuthentication 
This sample application is a basic Windows Application with one single page:

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAuthentication/Docs/UI_UWP_0.png)


1. To test the **Legacy Authentication**, you need to enter the "Legacy Account Name" and the "Legacy Account Key" associated with your Azure Media Services Account. Then you click on button "Azure Media Services Legacy Authentication".
2. To test the **Azure AD User Authentication**, you need to enter the "Azure Region" and the "Azure AD Tenant Domain" associated with your Azure Media Services Account. Then you click on button "Azure Media Services User Authentication".
3. To test the **Azure AD Service Principal Authentication**, you need to enter the "Application ID" and the "Application Key" associated with the application created on Azure Portal and associated with your Azure Media Services Account. Then you click on button "Azure Media Services Service Principal Authentication".



## NEXT STEPS:
These sample applications could be improved to support the following features:</p>
1.  A full REST API implementation (not only HTTP GET request to retrieve the Assets, Channels, ...).</p>
2.  A full REST API implementation for **User Authentication**: so far Microsoft Libraries are required for Interactive User Authentication.</p>

