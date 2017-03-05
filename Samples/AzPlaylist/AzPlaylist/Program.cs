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
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzPlaylist
{
    class Program
    {
        const string header = "{ \"Groups\": [{\"UniqueId\": \"audio_video_picture\",\"Title\": \"Windows 10 Audio Video Tests\",\"Category\": \"Windows 10 Audio Video Tests\",\"ImagePath\": \"ms-appx:///Assets/AudioVideo.png\",\"Description\": \"Windows 10 Audio Video Tests\",\"Items\": [";
        const string songItem = " \"UniqueId\": \"{0}\", \"Comment\": \"\", \"Title\": \"{1}\", \"ImagePath\": \"ms-appx:///Assets/MP4.png\",\"Description\": \"\", \"Content\": \"{2}\", \"PosterContent\": \"{3}\",\"Start\": \"0\",\"Duration\": \"0\",\"PlayReadyUrl\": \"null\",\"PlayReadyCustomData\": \"null\",\"BackgroundAudio\": true";
        //        const string songItem = "{0}{1}{2}{3}";
        const string footer = "\r\n]}]}";
        static string GetExtension(string uri)
        {
            string result = string.Empty;
            int pos = 0;
            if ((pos = uri.LastIndexOf("."))>0 )
            {
                if(pos + 1 < uri.Length)
                    result = uri.Substring(pos + 1);
            }
            return result;
        }
        static string GetUriFileName(string uri)
        {
            string result = string.Empty;
            int pos = 0;
            if ((pos = uri.LastIndexOf("/")) > 0)
            {
                if (pos + 1 < uri.Length)
                    result = uri.Substring(pos + 1);
            }
            return result;
        }
        static string GetFileName(string uri)
        {
            string result = string.Empty;
            int pos = 0;
            if ((pos = uri.LastIndexOf("\\")) > 0)
            {
                if (pos + 1 < uri.Length)
                    result = uri.Substring(pos + 1);
            }
            return result;
        }
        static string GetPosterUri(string unescapeuri,string extensions)
        {
            string result = "ms-appx:///Assets/MP4.png";
            char[] sep = { '/' };
            string[] res = unescapeuri.Split(sep);
            if ((res != null) && (res.Length > 6))
            {
                string artist = res[4];
                string album = res[5];
                string title = res[6];
                string ext = GetExtension(title);
                if ((!string.IsNullOrEmpty(ext)) && (extensions.Contains(ext)))
                {
                    string uri = Uri.EscapeUriString(unescapeuri);
                    uri = uri.Replace("\'", "%27");
                    int pos = uri.LastIndexOf('/');
                    if (pos > 0)
                        result = uri.Substring(0, pos + 1) + "artwork.jpg";
                }
            }
            return result;
        }
        static string GetPosterFile(string path, string extensions)
        {
            string result = "ms-appx:///Assets/MP4.png";
            char[] sep = { '\\' };
            string[] res = path.Split(sep);
            if ((res != null) && (res.Length > 3))
            {
                string artist = res[res.Length - 3];
                string album = res[res.Length - 2];
                string title = res[res.Length-1];
                string ext = GetExtension(title);
                if ((!string.IsNullOrEmpty(ext)) && (extensions.Contains(ext)))
                {
                    int pos = path.LastIndexOf('\\');
                    if (pos > 0)
                        result = path.Substring(0, pos + 1) + "artwork.jpg";
                }
            }
            return result;
        }
        static bool CreatePlayList(string AccountName, string AccountKey, string Container, string extensions, string outputFile, out string errorMessage)
        {
            ulong counter = 0;
            List<string> blobs = new List<string>();
            errorMessage = string.Empty;

            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    "DefaultEndpointsProtocol=https;AccountName=" + AccountName + ";AccountKey=" + AccountKey);

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(Container);

                System.IO.StreamWriter writer = null;
                string ext = string.Empty;
                if (!string.IsNullOrEmpty(outputFile))
                    writer = new System.IO.StreamWriter(outputFile, false, Encoding.UTF8);
                if ((writer != null)|| string.IsNullOrEmpty(outputFile))
                {
                    // Loop over items within the container and output the length and URI.
                    foreach (IListBlobItem item in container.ListBlobs(null, true))
                    {
                        if (item.GetType() == typeof(CloudBlockBlob))
                        {
                            CloudBlockBlob blob = (CloudBlockBlob)item;

                            string unescapeuri = blob.Uri.ToString();
                            ext = GetExtension(unescapeuri);
                            if ((!string.IsNullOrEmpty(ext))&&(extensions.Contains(ext)))
                            {
                                string artist = string.Empty ;
                                string album = string.Empty;
                                string posteruri = GetPosterUri(unescapeuri,extensions);
                                string title = GetUriFileName(unescapeuri);
                                if(!string.IsNullOrEmpty(title))
                                { 
                                    string uri = Uri.EscapeUriString(unescapeuri);
                                    uri = uri.Replace("\'", "%27");
                                    if (counter == 0)
                                    {
                                        if (!string.IsNullOrEmpty(outputFile))
                                        {
                                            writer.WriteLine(header);
                                            writer.Write("{");
                                            writer.Write(string.Format(songItem, counter.ToString(), title, uri, posteruri));
                                            writer.Write("}");
                                        }
                                        else
                                            Console.Write(uri + "\n");
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(outputFile))
                                        {
                                            writer.WriteLine(",\r\n");
                                            writer.Write("{");
                                            writer.Write(string.Format(songItem, counter.ToString(), title, uri, posteruri));
                                            writer.Write("}");
                                        }
                                        else
                                            Console.Write(uri + "\n");
                                    }
                                    counter++;
                                }
                            }
                        }
                        else if (item.GetType() == typeof(CloudPageBlob))
                        {
                            CloudPageBlob pageBlob = (CloudPageBlob)item;
                        }
                        else if (item.GetType() == typeof(CloudBlobDirectory))
                        {
                            CloudBlobDirectory directory = (CloudBlobDirectory)item;
                        }
                    }
                    if (!string.IsNullOrEmpty(outputFile))
                    {
                        writer.WriteLine(footer);
                        writer.Close();
                    }
                    else
                        Console.Write(counter.ToString() + " files discovered on Azure storage\n");
                    return true;
                }
                else
                    errorMessage = "Can't create file : " + outputFile;

            }
            catch (Exception e)
            {
                if(string.IsNullOrEmpty(outputFile))
                    errorMessage = "Exception while dumping playlist: " + e.Message;
                else
                    errorMessage = "Exception while creating playlist: " + e.Message;
            }
            return false;
        }
        // Process all files in the directory passed in, recurse on any directories 
        // that are found, and process the files they contain.
        public static void ProcessDirectory(ref ulong counter, string extensions, System.IO.StreamWriter writer, string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = System.IO.Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(ref counter, extensions, writer,fileName);

            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = System.IO.Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(ref counter, extensions, writer,subdirectory);
        }

        // Insert logic for processing found files here.
        public static void ProcessFile(ref ulong counter, string extensions, System.IO.StreamWriter writer, string path)
        {
            Console.WriteLine("Processed file '{0}'.", path);
            string ext = GetExtension(path);
            if ((!string.IsNullOrEmpty(ext)) && (extensions.Contains(ext)))
            {
                string artist = string.Empty;
                string album = string.Empty;
                string posteruri = GetPosterFile(path, extensions);
                string title = GetFileName(path);
                if (!string.IsNullOrEmpty(title))
                {
                    string uri = path;
                    if (counter == 0)
                    {
                        if(writer!=null)
                        {
                            writer.WriteLine(header);
                            writer.Write("{");
                            uri = "file://" + uri.Replace("\\", "\\\\"); 
                            posteruri = "file://" + posteruri.Replace("\\","\\\\");
                            writer.Write(string.Format(songItem, counter.ToString(), title, uri, posteruri));
                            writer.Write("}");
                        }
                        else
                            Console.Write(uri + "\n");
                    }
                    else
                    {
                        if (writer != null)
                        {
                            writer.WriteLine(",\r\n");
                            writer.Write("{");
                            uri = "file://" + uri.Replace("\\", "\\\\");
                            posteruri = "file://" + posteruri.Replace("\\", "\\\\");
                            writer.Write(string.Format(songItem, counter.ToString(), title, uri, posteruri));
                            writer.Write("}");
                        }
                        else
                            Console.Write(uri + "\n");
                    }
                    counter++;
                }
            }
        }
        static bool CreateLocalPlayList(string FolderName, string extensions, string outputFile, out string errorMessage)
        {
            ulong counter = 0;
            List<string> blobs = new List<string>();
            errorMessage = string.Empty;

            try
            {
                string ext = string.Empty;
                System.IO.StreamWriter writer = null;
                if (!string.IsNullOrEmpty(outputFile))
                    writer = new System.IO.StreamWriter(outputFile, false, Encoding.UTF8);
                if ((writer != null) || string.IsNullOrEmpty(outputFile))
                {

                    if (System.IO.File.Exists(FolderName))
                    {
                        // This path is a file
                        ProcessFile(ref counter, extensions, writer, FolderName);
                    }
                    else if (System.IO.Directory.Exists(FolderName))
                    {
                        // This path is a directory
                        ProcessDirectory(ref counter, extensions, writer, FolderName);
                    }
                    else
                    {
                        Console.WriteLine("{0} is not a valid file or directory.", FolderName);
                    }
                    if (!string.IsNullOrEmpty(outputFile))
                    {
                        writer.WriteLine(footer);
                        writer.Close();
                    }
                    else
                        Console.Write(counter.ToString() + " files discovered on Azure storage\n");
                    return true;
                }
                else
                    errorMessage = "Can't create file : " + outputFile;

            }
            catch (Exception e)
            {
                if (string.IsNullOrEmpty(outputFile))
                    errorMessage = "Exception while dumping playlist: " + e.Message;
                else
                    errorMessage = "Exception while creating playlist: " + e.Message;
            }
            return false;
        }
        enum Action
        {
            None = 0,
            Create,
            Dump,
            LocalCreate,
            LocalDump
        }

        struct ArgsParsed
        {
            public Action action;
            public string ErrorMessage;

            public string StorageAccountName;
            public string StorageAccountKey;
            public string ContainerName;
            public string Folder;
            public string extensions;
            public string outputFileName;
        }
        static void DumpSyntax(string ErrorMessage)
        {
            Console.WriteLine("AzPlaylist Application version 1.0.1");
            if (!string.IsNullOrEmpty(ErrorMessage)) Console.WriteLine("Error: " + ErrorMessage);
            Console.WriteLine("Syntax:");
            Console.WriteLine("AzPlaylist -action <Action> ");
            Console.WriteLine("                 -storageaccountname <StorageAccountName> -storageaccountkey <StorageAccountKey> ");
            Console.WriteLine("                 -containername <ContainerName> -extensions <extensions> ");
            Console.WriteLine("                 [ -outputfilename <outputFileName> ]  ");
            Console.WriteLine("Where <Action>: 'create' or 'dump' or 'localcreate' or 'localdump' ");
            Console.WriteLine("For instance: ");
            Console.WriteLine("AzPlaylist -action create  ");
            Console.WriteLine("                 -storageaccountname <StorageAccountName> -storageaccountkey <StorageAccountKey> -containername ");
            Console.WriteLine("                 <ContainerName> -extensions mp3;m4a;aac;flac -outputfilename <outputFileName> ");
            Console.WriteLine("AzPlaylist -action dump  ");
            Console.WriteLine("                 -storageaccountname <StorageAccountName> -storageaccountkey <StorageAccountKey> -containername ");
            Console.WriteLine("                 <ContainerName> -extensions mp3;m4a;aac;flac  ");
            Console.WriteLine("AzPlaylist -action localcreate  ");
            Console.WriteLine("                 -folder <FolderName> -extensions mp3;m4a;aac;flac -outputfilename <outputFileName> ");
            Console.WriteLine("AzPlaylist -action localdump  ");
            Console.WriteLine("                 -folder <FolderName> -extensions mp3;m4a;aac;flac ");
        }
        static void CheckArgs(ArgsParsed arg)
        {
            arg.ErrorMessage = string.Empty;

            if (arg.action == Action.Create)
            {
                if (string.IsNullOrEmpty(arg.outputFileName) &&
                    string.IsNullOrEmpty(arg.StorageAccountName) &&
                    string.IsNullOrEmpty(arg.StorageAccountKey) &&
                    string.IsNullOrEmpty(arg.extensions) &&
                    string.IsNullOrEmpty(arg.ContainerName))
                    arg.ErrorMessage = string.Empty;
                else
                    arg.ErrorMessage = "Missing argument for Create Playlist scenario";

            }
            else if (arg.action == Action.Dump)
            {
                if (string.IsNullOrEmpty(arg.StorageAccountName) &&
                    string.IsNullOrEmpty(arg.StorageAccountKey) &&
                    string.IsNullOrEmpty(arg.extensions) &&
                    string.IsNullOrEmpty(arg.ContainerName))
                    arg.ErrorMessage = string.Empty;
                else
                    arg.ErrorMessage = "Missing argument for Dump Playlist scenario";
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
                            if (args[i] == "create")
                                result.action = Action.Create;
                            else if (args[i] == "dump")
                                result.action = Action.Dump;
                            else if (args[i] == "localcreate")
                                result.action = Action.LocalCreate;
                            else if (args[i] == "localdump")
                                result.action = Action.LocalDump;
                            else
                                result.action = Action.None;
                        }
                        break;
                    case "-outputfilename":
                        if (++i < args.Length)
                        {
                            result.outputFileName = args[i];
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
                    case "-folder":
                        if (++i < args.Length)
                        {
                            result.Folder = args[i];
                        }
                        break;
                    case "-extensions":
                        if (++i < args.Length)
                        {
                            result.extensions = args[i];
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
                if (arg.action == Action.Create)
                {
                    if (CreatePlayList(arg.StorageAccountName, arg.StorageAccountKey, arg.ContainerName, arg.extensions, arg.outputFileName, out arg.ErrorMessage) != true)
                        DumpSyntax(arg.ErrorMessage);
                }
                else if (arg.action == Action.Dump)
                {
                    if (CreatePlayList(arg.StorageAccountName, arg.StorageAccountKey, arg.ContainerName, arg.extensions, null, out arg.ErrorMessage) != true)
                        DumpSyntax(arg.ErrorMessage);
                }
                else if (arg.action == Action.LocalCreate)
                {
                    if (CreateLocalPlayList(arg.Folder, arg.extensions, arg.outputFileName, out arg.ErrorMessage) != true)
                        DumpSyntax(arg.ErrorMessage);
                }
                else if (arg.action == Action.LocalDump)
                {
                    if (CreateLocalPlayList(arg.Folder, arg.extensions, null, out arg.ErrorMessage) != true)
                        DumpSyntax(arg.ErrorMessage);
                }
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
