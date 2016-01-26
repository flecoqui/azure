//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDataMovement
{
    enum Action
    {
        None = 0,
        Upload,
        Download,
        Copy
    }

    struct ArgsParsed
    {
        public Action action;
        public string ErrorMessage;

        public string FileName;
        public string ContentType;

        public string StorageAccountName;
        public string StorageAccountKey;
        public string ContainerName;
        public string BlobName;

        public string SourceStorageAccountName;
        public string SourceStorageAccountKey;
        public string SourceContainerName;
        public string SourceBlobName;
    }



    class Program
    {

        public static DateTime StartTime;

        private static void Job_Finished(object sender, Microsoft.WindowsAzure.Storage.DataMovement.TransferFinishEventArgs e)
        {
            Console.WriteLine(string.Format("Upload finished at {0:d/M/yyyy HH:mm:ss.fff}", DateTime.Now));
            Console.WriteLine(string.Format("Time Elapsed in seconds: " + (DateTime.Now - StartTime).TotalSeconds.ToString()));
        }
        private static void Job_ProgressUpdated(object sender, Microsoft.WindowsAzure.Storage.DataMovement.TransferProgressEventArgs e)
        {
            Console.WriteLine("Progress speed: " + e.Speed + " progress: " + e.Progress);
        }
        private static void Job_Starting(object sender, Microsoft.WindowsAzure.Storage.DataMovement.TransferStartEventArgs e)
        {
            StartTime = DateTime.Now;
            Console.WriteLine(string.Format("Starting upload at {0:d/M/yyyy HH:mm:ss.fff}", StartTime));
        }
        static bool UploadFileWithAzcopyDLL(string filesource, string contentType, string storageAccountName, string storageAccountKey, string containerName, string blobName )
        {
            bool bResult = false;
            try
            {

                System.Net.ServicePointManager.Expect100Continue = false;
                System.Net.ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 8;


                Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(
                    new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(storageAccountName, storageAccountKey), true);

                Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();
                Microsoft.WindowsAzure.Storage.Blob.CloudBlob cloudBlob = container.GetBlockBlobReference(blobName);

                Microsoft.WindowsAzure.Storage.DataMovement.TransferOptions option = new Microsoft.WindowsAzure.Storage.DataMovement.TransferOptions();

                //option.ParallelOperations = 64;
                //option.MaximumCacheSize = 500000000;

                Microsoft.WindowsAzure.Storage.DataMovement.TransferManager manager = new Microsoft.WindowsAzure.Storage.DataMovement.TransferManager(option);

                var fileStream = System.IO.File.OpenRead(filesource);
                Microsoft.WindowsAzure.Storage.DataMovement.TransferLocation source = new Microsoft.WindowsAzure.Storage.DataMovement.TransferLocation(fileStream);
                Microsoft.WindowsAzure.Storage.DataMovement.TransferLocation destination = new Microsoft.WindowsAzure.Storage.DataMovement.TransferLocation(cloudBlob);

                //source.SourceUri = new Uri("file://" + sourceFileName);
                Microsoft.WindowsAzure.Storage.DataMovement.TransferJob job = new Microsoft.WindowsAzure.Storage.DataMovement.TransferJob(source, destination, Microsoft.WindowsAzure.Storage.DataMovement.TransferMethod.SyncCopy);
                System.Threading.CancellationToken token = new System.Threading.CancellationToken();


                //Microsoft.WindowsAzure.Storage.DataMovement.TransferManager.UseV1MD5 = false;

                job.ContentType = contentType;
                job.Starting += Job_Starting;
                job.ProgressUpdated += Job_ProgressUpdated;
                job.Finished += Job_Finished;

                Task t = manager.ExecuteJobAsync(job, token);
                t.Wait();
                if (job.IsCompleted == true)
                    bResult = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                if(e.InnerException!= null)
                    Console.WriteLine("Exception: " + e.InnerException.Message);
            }
            return bResult;
        }
        static void DumpSyntax(string ErrorMessage)
        {
            Console.WriteLine("TestDataMovement Application version 1.0");
            if (!string.IsNullOrEmpty(ErrorMessage)) Console.WriteLine("Error: " + ErrorMessage);
            Console.WriteLine("Syntax:");
            Console.WriteLine("TestDataMovement -action <Action> [-filename <FileName>] [-contenttype <ContentType>] ");
            Console.WriteLine("                 [-storageaccountname <StorageAccountName> -storageaccountkey <StorageAccountKey> ");
            Console.WriteLine("                 -containername <ContainerName> -blobname <BlobName>] ");
            Console.WriteLine("                 [-sourcestorageaccountname <StorageAccountName> -sourcestorageaccountkey <StorageAccountKey> ");
            Console.WriteLine("                 -sourcecontainername <ContainerName> -sourceblobname <BlobName>] ");
            Console.WriteLine("Where <Action>: 'upload' or 'download' or 'copy'");
            Console.WriteLine("For instance: ");
            Console.WriteLine("TestDataMovement -action upload -filename <FileName> -contenttype <ContentType> ");
            Console.WriteLine("                 -storageaccountname <StorageAccountName> -storageaccountkey <StorageAccountKey> -containername ");
            Console.WriteLine("                 <ContainerName> -blobname <BlobName>");
            Console.WriteLine("TestDataMovement -action download -filename <FileName> -contenttype <ContentType> ");
            Console.WriteLine("                 -sourcestorageaccountname <StorageAccountName> -sourcestorageaccountkey <StorageAccountKey> ");
            Console.WriteLine("                 -sourcecontainername <ContainerName> -sourceblobname <BlobName>");
            Console.WriteLine("TestDataMovement -action copy -storageaccountname <StorageAccountName> -storageaccountkey <StorageAccountKey>");
            Console.WriteLine("                 -containername <ContainerName> -blobname <BlobName> ");
            Console.WriteLine("                 -sourcestorageaccountname <StorageAccountName> -sourcestorageaccountkey <StorageAccountKey> ");
            Console.WriteLine("                 -sourcecontainername <ContainerName> -sourceblobname <BlobName>");
        }
        static void CheckArgs(ArgsParsed arg)
        {
            arg.ErrorMessage = string.Empty;

            if (arg.action == Action.Upload)
            {
                if (string.IsNullOrEmpty(arg.FileName) &&
                    string.IsNullOrEmpty(arg.StorageAccountName) &&
                    string.IsNullOrEmpty(arg.StorageAccountKey) &&
                    string.IsNullOrEmpty(arg.BlobName) &&
                    string.IsNullOrEmpty(arg.ContainerName))
                    arg.ErrorMessage = string.Empty;
                else
                    arg.ErrorMessage = "Missing argument for uplaod scenario";

            }
            else if (arg.action == Action.Download)
            {
                if (string.IsNullOrEmpty(arg.FileName) &&
                    string.IsNullOrEmpty(arg.SourceStorageAccountName) &&
                    string.IsNullOrEmpty(arg.SourceStorageAccountKey) &&
                    string.IsNullOrEmpty(arg.SourceBlobName) &&
                    string.IsNullOrEmpty(arg.SourceContainerName))
                    arg.ErrorMessage = string.Empty;
                else
                    arg.ErrorMessage = "Missing argument for downlaod scenario";
            }
            else if (arg.action == Action.Copy)
            {
                if (string.IsNullOrEmpty(arg.SourceStorageAccountName) &&
                    string.IsNullOrEmpty(arg.SourceStorageAccountKey) &&
                    string.IsNullOrEmpty(arg.SourceBlobName) &&
                    string.IsNullOrEmpty(arg.SourceContainerName) &&
                    string.IsNullOrEmpty(arg.StorageAccountName) &&
                    string.IsNullOrEmpty(arg.StorageAccountKey) &&
                    string.IsNullOrEmpty(arg.BlobName) &&
                    string.IsNullOrEmpty(arg.ContainerName))
                    arg.ErrorMessage = string.Empty;
                else
                    arg.ErrorMessage = "Missing argument for copy scenario";

            }
            else
                arg.ErrorMessage = "Unexpected action";

        }
        static ArgsParsed ParseArgs(string[] args)
        {
            ArgsParsed result = new ArgsParsed();
            result.ErrorMessage = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-action":
                        if (++i < args.Length)
                        {
                            if (args[i] == "upload")
                                result.action = Action.Upload;
                            else if (args[i] == "download")
                                result.action = Action.Download;
                            else if (args[i] == "copy")
                                result.action = Action.Copy;
                            else
                                result.action = Action.None;
                        }
                        break;
                    case "-filename":
                        if (++i < args.Length)
                        {
                            result.FileName = args[i];
                        }
                        break;
                    case "-contenttype":
                        if (++i < args.Length)
                        {
                            result.ContentType = args[i];
                        }
                        break;
                    case "-storageaccountname":
                        if (++i < args.Length)
                        {
                            result.StorageAccountName = args[i];
                        }
                        break;
                    case "-storageaccountkey":
                        if (++i < args.Length)
                        {
                            result.StorageAccountKey = args[i];
                        }
                        break;
                    case "-containername":
                        if (++i < args.Length)
                        {
                            result.ContainerName = args[i];
                        }
                        break;
                    case "-blobname":
                        if (++i < args.Length)
                        {
                            result.BlobName = args[i];
                        }
                        break;
                    case "-sourcestorageaccountname":
                        if (++i < args.Length)
                        {
                            result.SourceStorageAccountName = args[i];
                        }
                        break;
                    case "-sourcestorageaccountkey":
                        if (++i < args.Length)
                        {
                            result.SourceStorageAccountKey = args[i];
                        }
                        break;
                    case "-sourcecontainername":
                        if (++i < args.Length)
                        {
                            result.SourceContainerName = args[i];
                        }
                        break;
                    case "-sourceblobname":
                        if (++i < args.Length)
                        {
                            result.SourceBlobName = args[i];
                        }
                        break;
                }

            }
            CheckArgs(result);
            return result;
        }
        static void Main(string[] args)
        {
            ArgsParsed arg = ParseArgs(args);
            if (string.IsNullOrEmpty(arg.ErrorMessage))
            {
                if (arg.action == Action.Upload)
                    UploadFileWithAzcopyDLL(arg.FileName, arg.ContentType, arg.StorageAccountName, arg.StorageAccountKey, arg.ContainerName, arg.BlobName);
                else
                    DumpSyntax("Not implemented");
            }
            else
            {
                DumpSyntax(arg.ErrorMessage);
            }

        }


    }
}
