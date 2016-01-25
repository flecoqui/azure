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


    class Program
    {

        private const string SourceFileName = "C:\\Hyper-V\\WS\\ws2012.zip";
        private const string ContentType = "application/zip";
        // private const string SourceFileName = "C:\\Hyper-V\\WS\\datafile.txt";
        //private const string ContentType = "application/txt";
        private const string ContainerName = "testcsharp";
          // private const string BlobName = "newnewtest1.txt";
        private const string BlobName = "newtest.zip";
        private const string StorageAccountName = "dashtestarm0";
        private const string StorageAccountKey = "lpPI8ciFdZ6mvu24bP34dE8i9+RjXWDkNn6LfSVo1AwtiUGpefzVldpIqjzq8z/BxmH0O7u/8qD0U1pOyXwwTg==";
        public static DateTime StartTime;


        static bool UploadFileWithNuggetDLL(string filesource, string contentType, string storageAccountName, string storageAccountKey, string containerName, string blobName)
        {
            bool bResult = false;
            try
            { 
                System.Net.ServicePointManager.Expect100Continue = false;
                System.Net.ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount * 8;
//                Microsoft.WindowsAzure.Storage.DataMovement.TransferManager.Configurations.ParallelOperations = 64;


                Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(
                    new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(storageAccountName, storageAccountKey), true);

                Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);

                // Create the container if it doesn't already exist.
                container.CreateIfNotExists();
                Microsoft.WindowsAzure.Storage.Blob.CloudBlob cloudBlob = container.GetBlockBlobReference(blobName);

                Microsoft.WindowsAzure.Storage.DataMovement.UploadOptions options = new Microsoft.WindowsAzure.Storage.DataMovement.UploadOptions();
                options.ContentType = contentType;

                Microsoft.WindowsAzure.Storage.DataMovement.TransferContext context = new Microsoft.WindowsAzure.Storage.DataMovement.TransferContext();
                context.ProgressHandler = new Progress<Microsoft.WindowsAzure.Storage.DataMovement.TransferProgress>((progress) =>
                {
                    Console.WriteLine("Bytes Uploaded: {0}", progress.BytesTransferred);
                });
                StartTime = DateTime.Now;
                Console.WriteLine(string.Format("Starting upload at {0:d/M/yyyy HH:mm:ss.fff}", StartTime));
                // Start the upload
                Task t = Microsoft.WindowsAzure.Storage.DataMovement.TransferManager.UploadAsync(filesource, cloudBlob, options, context);
                t.Wait();
                bResult = true;
                Console.WriteLine(string.Format("Upload finished at {0:d/M/yyyy HH:mm:ss.fff}", DateTime.Now));
                Console.WriteLine(string.Format("Time Elapsed in seconds: " + (DateTime.Now - StartTime).TotalSeconds.ToString()));

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                if(e.InnerException!= null)
                    Console.WriteLine("Exception: " + e.InnerException.Message);
            }
            return bResult;
        }
        static void Main(string[] args)
        {
            UploadFileWithNuggetDLL(SourceFileName, ContentType, StorageAccountName, StorageAccountKey, ContainerName, BlobName);
        }


    }
}
