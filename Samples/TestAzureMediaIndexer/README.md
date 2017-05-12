# Sample Application to generate automatically subtitles files in several languages from an original video or audio file using Azure Services


This sample application allows you to generate automatically subtitles files in several languages from an original video or audio file using Azure Services like 
1. a Web App to play and check the audio, video and subtitles, </p>
2. an Azure Media Services Account to generate subtitles from the original video or audio file), </p>
3. an Azure Cognitive Services Text Translator service to translate the original subtitles into several languages</p>
4. an Azure Search Account to index all the subtitles associated with the audio or video files.</p>

As Azure Media Services, Search Service and Cognitive Services are not deployed in all regions, it's recommanded to use one of the following regions:
West US, West Europe,Southeast Asia,West Central US 
The Azure backend required to run this application can the installed using the Azure Resource Manager template below:
https://github.com/flecoqui/azure/tree/master/azure-quickstart-templates/101-media-search-cognitive  

Below the architecture of this Azure deployment:

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture.png)


## INSTALLING THE BACKEND SERVICES IN AZURE:

Before using the sample application you need to install the Azure backend with all the services associated.
You can either use the Azure Portal to deploy manually all the Azure Services:

https://portal.azure.com
 
or you can use directly the Azure Resource Manager template available there:

https://github.com/flecoqui/azure/tree/master/azure-quickstart-templates/101-media-search-cognitive

Using the two Azure CLI command lines below,you can deploy automatically all the Azure Services required for the application: 

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
[ZIP file with the Application](https://github.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Releases/Latestrelease.zip)

1. You can download the zip file.</p>
2. Unzip the LatestRelease.zip file</p>
3. Run locally TestAzureMediaIndexer.exe</p>

## USING THE APPLICATION TESTAZUREMEDIAINDEXER
This sample application is a basic Windows Application with one single page:

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-0.png)


## CONNECT THE APPLICATION TO THE AZURE BACKEND
In order to use the application you need to provide the following paramters to establish a connection with your backend in Azure:
1. The Azure Media Serivces Account name</p>
2. The Azure Media Serivces Account key</p>
3. The Azure Cognitive Services Text Translator key</p>
4. The Azure Search Serivces Account name</p>
5. The Azure Search Serivces Account key</p>
6. The url of the Web Player application hosted on the Web site of your backend, the url should be close to this format: http://YourWebAppName.azurewebsites.net/player.html

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-1.png)

You can retrieve all the parameters below from the [Azure Portal:](https://portal.azure.com) </p>
1. The Azure Media Serivces Account name</p>
2. The Azure Media Serivces Account key</p>
3. The Azure Cognitive Services Text Translator key</p>
4. The Azure Search Serivces Account name</p>
5. The Azure Search Serivces Account key</p>
6. The url of the Web Player application hosted on the Web site </p>

Once all the parameters are ready click on the button "Connect" to establish the connection with your backend:
![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-1.png)


## UPLOAD THE AUDIO AND VIDEO ASSETS
Once the applicaiton is connected, the first step consists in uploading video or audio assets on your backend in Azure.


![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-2.png)

1. Click on the **Add Asset** button</p>
2. Select the audio file or video file you want to upload</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-2.png)

3. Once the file is uploaded, a new asset and a new file is displayed in the lists below:</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-2-1.png)


## GENERATE THE SUBTITLES FROM THE AUDIO AND VIDEO ASSETS
Once the video or audio file is uploaded, you can generate the subtitles with Azure Media Services Indexer 

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-3.png)

1. Select the spoken language of your audio or video file</p>
2. Click on the button **Generate subtitle** </p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-3.png)

3. A Job to generate the subtitles is launched in Azure after few minutes, the new subtitle file is available.</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-3-1.png)


## UPDATE THE GENERATED SUBTITLES WITH THE WEB APPLICATION
Once the subtile file is available, it's possible to update the subtitle file. The native format is WEBVTT, but it's still possible to convert a WEBVTT subtitle file into a TTML subtitle file.

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-4.png)

1. Click on the button **Play Video/Subtile** or **Play Audio/Subtile** </p> 
The Windows Application launches the Web Player Application 
![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-4.png)

2. Your default browser on your computer running Windows is displaying a page playing the video or audio file along with the subtitles.</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-4-1.png)

3. As sometimes the generated subtiles file needs to be updated, you can update each subtitle. Click on the **Pause** button.</p>
4. Update the subtile, click on the button **Save**.</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-4-2.png)

5. When all the subtitles are updated, you can save the subtile file on your machine in WEBVTT or TTML format.</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-4-3.png)

6. Once the subtile file is stored locally on your machine, you can update the subtitle file on Azure Storage when clicking on button **Update Subtitle**.</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-4-4.png)


## TRANSLATE THE GENERATED SUBTITLES WITH AZURE COGNITIVE SERVICES TEXT TRANSLATOR 
Once the first subtile file associted with your audio or video file are correct, you can generate more subtitle files in different languages.
 
![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-5.png)

1. Select the source subtile in the list box **List of Subtitle Assets**.</p>
2. Select the language of your new subtitle file.</p>
3. Click on the button **Translate subtitle** </p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-5.png)

4. After few seconds the new subtile file is displayed in the list box **List of Subtitle Assets**</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-5-1.png)

5. If you click on the button **Play Video/Subtile** or **Play Audio/Subtile** , you can playback the new subtitles.</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-5-2.png)

6. Finally, you can download or display the new subtitle file.</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-5-3.png)


## USE AZURE SEARCH TO INDEX THE SUBTITLES FILES ASSOCIATED WITH YOUR AUDIO AND VIDEO CONTENT
Once all the subtitles associated with your video or audio files are generated, you can store the subtitles in the Azure Search service.</p>
 
![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/1-architecture-step-6.png)

1. First you need to create the Index associated with the subtiles. Click on the button **Create Index** to create the Index. This step is only required once. If you want to clear the Azure Search database you can click on **Delete Index** button.</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-6.png)

2. Once the Index is created, select the subtile file you want to import and click on the button **Populate the Index with subtiles**. You can repeat this step for each subtitle file.</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-6-1.png)

3. Now you can test the Azure Search database, enter a word in the search edit box and click on the button **Search**.</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-6-2.png)

4. If this word is present in any subtitle, the search list box is populated with all the subtile where this word has been pronounced.</p>
5. Select the subtitle in the list box and click on the button **Play Search**.</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-6-3.png)

6. The Web Player will play the audio or video files along with the subtitles when the word has been pronounced.</p>

![](https://raw.githubusercontent.com/flecoqui/azure/master/Samples/TestAzureMediaIndexer/Docs/ui-6-4.png)



## NEXT STEP:
This sampple application could be improved to support the following features:</p>
1.  A full Web (HTML5) frontend to replace the current Windows Application.</p>
2.  Use other Azure Media Services Indexer services to enhanced the current content.</p>

