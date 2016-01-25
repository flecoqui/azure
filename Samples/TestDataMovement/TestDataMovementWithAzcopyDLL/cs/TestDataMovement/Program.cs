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
        private const string ContainerName = "testcsharp";
        //   private const string BlobName = "newnewtest.txt";
        private const string BlobName = "newtest.zip";
        private const string StorageAccountName = "dashtestarm0";
        private const string StorageAccountKey = "lpPI8ciFdZ6mvu24bP34dE8i9+RjXWDkNn6LfSVo1AwtiUGpefzVldpIqjzq8z/BxmH0O7u/8qD0U1pOyXwwTg==";
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
                Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);

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
 
        static void Main(string[] args)
        {
            UploadFileWithAzcopyDLL(SourceFileName, ContentType, StorageAccountName, StorageAccountKey, ContainerName, BlobName);
        }


    }
}
