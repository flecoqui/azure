pwd
pws// TestAzureStorageCpp.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#include "was/storage_account.h"
#include "was/blob.h"
#include "cpprest/filestream.h"
#include "cpprest/containerstream.h"
using namespace std;

namespace myapplication {
			// TODO: Put your account name and account key here
			utility::string_t storage_connection_string(U("DefaultEndpointsProtocol=https;AccountName=myaccountname;AccountKey=myaccountkey"));
			void DumpMessage(utility::string_t Message)
			{

				ucout << Message.c_str();
				ucout << TEXT("\r\n");
			}
			void AppendString(utility::string_t& s, const char* p)
			{
				if (p != nullptr)
				{
					size_t newsize = strlen(p) + 1;
					wchar_t * wcstring = new wchar_t[newsize];
					if (wcstring != nullptr) {
						size_t convertedChars = 0;
						mbstowcs_s(&convertedChars, wcstring, newsize, p, _TRUNCATE);
						s += wcstring;
					}
					delete wcstring;
				}
			}
			utility::string_t to_string(const std::vector<uint8_t>& data)
			{
				return utility::string_t(data.cbegin(), data.cend());
			}
			LONGLONG getfilesize(utility::string_t FileName)
			{
				LONGLONG filesize = -1;
				try
				{
					HANDLE m_file_handle = CreateFile(FileName.c_str(),           // open MYFILE.TXT 
						GENERIC_READ,              // open for reading 
						FILE_SHARE_READ,           // share for reading 
						NULL,                      // no security 
						OPEN_EXISTING,             // existing file only 
						FILE_ATTRIBUTE_NORMAL,     // normal file 
						NULL);                     // no attr. template 

					if (m_file_handle != INVALID_HANDLE_VALUE)
					{
						LARGE_INTEGER l;
						if (::GetFileSizeEx(m_file_handle, &l))
							filesize =l.QuadPart;
						CloseHandle(m_file_handle);
					}
				}
				catch (exception e)
				{

				}
				return filesize;
			}
			int UploadFile(utility::string_t FileName, utility::string_t ContentType, utility::string_t StorageAccountName, utility::string_t StorageAccountKey, utility::string_t ContainerName, utility::string_t BlobName, utility::string_t& errormsg)
			{
				try
				{
					errormsg = TEXT("");
					// Initialize storage account
					azure::storage::storage_credentials* pcredentials = new azure::storage::storage_credentials(StorageAccountName, StorageAccountKey);
					if (pcredentials != nullptr)
					{
						azure::storage::cloud_storage_account* pstorage_account = new azure::storage::cloud_storage_account(*pcredentials, true);
						if (pstorage_account != nullptr)
						{
							//azure::storage::cloud_storage_account::parse(storage_connection_string);

						// Create a blob container
							azure::storage::cloud_blob_client blob_client = pstorage_account->create_cloud_blob_client();
							azure::storage::cloud_blob_container container = blob_client.get_container_reference(ContainerName);

							// Return value is true if the container did not exist and was successfully created.
							container.create_if_not_exists();

							// Make the blob container publicly accessible
							azure::storage::blob_container_permissions permissions;
							permissions.set_public_access(azure::storage::blob_container_public_access_type::blob);
							container.upload_permissions(permissions);

							concurrency::streams::istream input_stream = concurrency::streams::file_stream<uint8_t>::open_istream(FileName).get();
							utility::size64_t filesize = getfilesize(FileName);
							azure::storage::cloud_block_blob blob1 = container.get_block_blob_reference(BlobName);
							azure::storage::access_condition condition;
							azure::storage::blob_request_options options;
							azure::storage::operation_context context;
							context.set_default_log_level(azure::storage::log_level_verbose);
							options.set_parallelism_factor(8);
							options.set_store_blob_content_md5(true);
							//	::DumpMessage(TEXT("Blob size:"));
	//							::DumpMessage(to_string(blob1.properties().size));
								//::DumpMessage(TEXT("Blob content_type:"));
								//::DumpMessage(to_string(blob1.properties().content_type()));
							utility::size64_t counter = 0;
							context.set_sending_request([&counter, &filesize](web::http::http_request& request, azure::storage::operation_context)
							{
								utility::string_t res;
								if (!request.headers().match(web::http::header_names::content_length, res))
								{
									res.clear();
								}
								//input_stream.streambuf.m_rdpos;
								utility::string_t s = TEXT("Number of bytes being transmitted:");
								s += res;
								s += TEXT(" bytes");
								counter += atol(res);
								DumpMessage(s);

								//res = TEXT("res");
						//		for (int i = 0; i < h.size; i++)
							//	{
									
							//	}
							});
							
							Concurrency::task<void> t = blob1.upload_from_stream_async(input_stream, condition, options, context);
							while (t.is_done() != true)
							{
								::Sleep(1000);

								int64_t c = blob1.copy_state().bytes_copied();
								int64_t t = blob1.copy_state().total_bytes();
								int64_t sz = blob1.properties().size();
								//input_stream.streambuf.m_rdpos;
								ostringstream oss;
								oss << c;
								oss << "/";
								oss << t;
								
								utility::string_t s = TEXT("");
								AppendString(s, oss.str().c_str());
								DumpMessage(s);

							}
							input_stream.close().wait();

							return 1;
						}
					}
				}
				catch (const azure::storage::storage_exception& e)
				{
					AppendString(errormsg, e.what());

					azure::storage::request_result result = e.result();
					azure::storage::storage_extended_error extended_error = result.extended_error();
					if (!extended_error.message().empty())
					{
						errormsg += TEXT(" ");
						errormsg += extended_error.message();
					}
				}
				catch (const std::exception& e)
				{
					AppendString(errormsg, e.what());
				}
				return 0;
			}
			int DownloadFile(utility::string_t FileName, utility::string_t ContentType, utility::string_t StorageAccountName, utility::string_t StorageAccountKey, utility::string_t ContainerName, utility::string_t BlobName, utility::string_t& errormsg)
			{
				try
				{
					errormsg = TEXT("");
					// Initialize storage account
					azure::storage::storage_credentials* pcredentials = new azure::storage::storage_credentials(StorageAccountName, StorageAccountKey);
					if (pcredentials != nullptr)
					{
						azure::storage::cloud_storage_account* pstorage_account = new azure::storage::cloud_storage_account(*pcredentials, true);
						if (pstorage_account != nullptr)
						{

							// Create a blob container
							azure::storage::cloud_blob_client blob_client = pstorage_account->create_cloud_blob_client();
							azure::storage::cloud_blob_container container = blob_client.get_container_reference(ContainerName);

							// Return value is true if the container did not exist and was successfully created.
							if (container.exists())
							{
								azure::storage::cloud_block_blob blob1 = container.get_block_blob_reference(BlobName);
								
								if (blob1.exists())
								{
									concurrency::streams::ostream output_stream = concurrency::streams::file_stream<uint8_t>::open_ostream(FileName).get();
									//::DumpMessage(TEXT("Blob size:"));
//									::DumpMessage(to_string(blob1.properties().size));
									//::DumpMessage(TEXT("Blob content_type:"));
									//::DumpMessage(to_string(blob1.properties().content_type));
									azure::storage::access_condition condition;
									azure::storage::blob_request_options options;
									azure::storage::operation_context context;
									options.set_parallelism_factor(8);
									options.set_store_blob_content_md5(true);
									blob1.download_to_stream(output_stream, condition, options, context);
									size_t r = context.request_results().size();
									output_stream.close().wait();
									return 1;
								}
								else
									errormsg += TEXT("Error Blob doesn't exist");
							}
							else
								errormsg += TEXT("Error Container doesn't exist");

						}

					}
				}
				catch (const azure::storage::storage_exception& e)
				{
					AppendString(errormsg, e.what());

					azure::storage::request_result result = e.result();
					azure::storage::storage_extended_error extended_error = result.extended_error();
					if (!extended_error.message().empty())
					{
						errormsg += TEXT(" ");
						errormsg += extended_error.message();
					}
				}
				catch (const std::exception& e)
				{
					AppendString(errormsg, e.what());
				}
				return 0;
			}

} // namespace azure::storage::samples
enum Action
{
	None = 0,
	Upload,
	Download,
	Copy
};

struct ArgsParsed
{
public:Action action;
public:utility::string_t ErrorMessage;

public:utility::string_t FileName;
public:utility::string_t ContentType;

public:utility::string_t  StorageAccountName;
public:utility::string_t StorageAccountKey;
public:utility::string_t ContainerName;
public:utility::string_t BlobName;

public:utility::string_t SourceStorageAccountName;
public:utility::string_t SourceStorageAccountKey;
public:utility::string_t SourceContainerName;
public:utility::string_t SourceBlobName;
};

/*
	
public static DateTime StartTime;

private static void Job_Finished(object sender, Microsoft.WindowsAzure.Storage.DataMovement.TransferFinishEventArgs e)
{
	Console.WriteLine(string.Format("Upload finished at {0:d/M/yyyy HH:mm:ss.fff}", DateTime.Now));
	Console.WriteLine(string.Format("Time Elapsed in seconds: " + (DateTime.Now - StartTime).TotalSeconds.ToString()));
}
private static void Job_ProgressUpdated(object sender, Microsoft.WindowsAzure.Storage.DataMovement.TransferProgressEventArgs e)
{
	ucout << TEXT("Progress speed: " + e.Speed + " progress: " + e.Progress);
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
	ucout << TEXT("Exception: " + e.Message);
	if(e.InnerException!= null)
	ucout << TEXT("Exception: " + e.InnerException.Message);
	}
	return bResult;
}
*/
static void DumpMessage(utility::string_t Message)
{
	
	ucout << Message.c_str();
	ucout << TEXT("\r\n");
}
static void DumpSyntax(utility::string_t ErrorMessage)
{
	
	ucout << TEXT("TestDataMovement Application version 1.0 \r\n");
	if (ErrorMessage.length() > 0) {
		ucout << TEXT("Error: ");
		ucout << ErrorMessage.c_str();
	}
	ucout << TEXT("Syntax:\r\n");
	ucout << TEXT("TestAzureStorageCpp -action <Action> [-filename <FileName>] [-contenttype <ContentType>] \r\n");
	ucout << TEXT("                 [-storageaccountname <StorageAccountName> -storageaccountkey <StorageAccountKey> \r\n");
	ucout << TEXT("                 -containername <ContainerName> -blobname <BlobName>] \r\n");
	ucout << TEXT("                 [-sourcestorageaccountname <StorageAccountName> -sourcestorageaccountkey <StorageAccountKey> \r\n");
	ucout << TEXT("                 -sourcecontainername <ContainerName> -sourceblobname <BlobName>] \r\n");
	ucout << TEXT("Where <Action>: 'upload' or 'download' or 'copy'\r\n");
	ucout << TEXT("For instance: \r\n");
	ucout << TEXT("TestAzureStorageCpp -action upload -filename <FileName> -contenttype <ContentType> \r\n");
	ucout << TEXT("                 -storageaccountname <StorageAccountName> -storageaccountkey <StorageAccountKey> -containername \r\n");
	ucout << TEXT("                 <ContainerName> -blobname <BlobName>\r\n");
	ucout << TEXT("TestAzureStorageCpp -action download -filename <FileName> -contenttype <ContentType> \r\n");
	ucout << TEXT("                 -sourcestorageaccountname <StorageAccountName> -sourcestorageaccountkey <StorageAccountKey> \r\n");
	ucout << TEXT("                 -sourcecontainername <ContainerName> -sourceblobname <BlobName>\r\n");
	ucout << TEXT("TestAzureStorageCpp -action copy -storageaccountname <StorageAccountName> -storageaccountkey <StorageAccountKey>\r\n");
	ucout << TEXT("                 -containername <ContainerName> -blobname <BlobName> \r\n");
	ucout << TEXT("                 -sourcestorageaccountname <StorageAccountName> -sourcestorageaccountkey <StorageAccountKey> \r\n");
	ucout << TEXT("                 -sourcecontainername <ContainerName> -sourceblobname <BlobName>\r\n");
}

	static void CheckArgs(ArgsParsed* arg)
	{
		arg->ErrorMessage = TEXT("");

		if (arg->action == Upload)
		{
			if (arg->FileName.length()!=0 &&
			arg->StorageAccountName.length() != 0 &&
			arg->StorageAccountKey.length() != 0 &&
			arg->BlobName.length() != 0 &&
			arg->ContainerName.length() != 0)
				arg->ErrorMessage = TEXT("");
			else
				arg->ErrorMessage = TEXT("Missing argument for uplaod scenario");
		}
		else if (arg->action == Download)
		{
			if (arg->FileName.length() != 0 &&
			arg->SourceStorageAccountName.length() != 0 &&
			arg->SourceStorageAccountKey.length() != 0 &&
			arg->SourceBlobName.length() != 0 &&
			arg->SourceContainerName.length() != 0)
				arg->ErrorMessage = TEXT("");
			else
				arg->ErrorMessage = TEXT("Missing argument for downlaod scenario");
		}
		else if (arg->action == Copy)
		{
			if (arg->SourceStorageAccountName.length() != 0 &&
			arg->SourceStorageAccountKey.length() != 0 &&
			arg->SourceBlobName.length() != 0 &&
			arg->SourceContainerName.length() != 0 &&
			arg->StorageAccountName.length() != 0 &&
			arg->StorageAccountKey.length() != 0 &&
			arg->BlobName.length() != 0 &&
			arg->ContainerName.length() != 0)
				arg->ErrorMessage = TEXT("");
			else
				arg->ErrorMessage = TEXT("Missing argument for copy scenario");
		}
		else
			arg->ErrorMessage = TEXT("Unexpected action");
	};
	static ArgsParsed ParseArgs(int argc, _TCHAR* argv[])
	{

		ArgsParsed* presult = new ArgsParsed();
		if (presult != NULL)
			presult->ErrorMessage = TEXT("");
	
	
		for (int i = 0; i < argc; i++)
		{
			utility::string_t option = TEXT("");
			++argv;
			if(*argv != nullptr)
				option = *argv;
			if(option == TEXT("-action"))
			{
				if (++i < argc)
				{
					utility::string_t action = *++argv;
				if (action == TEXT("upload"))
					presult->action = Upload;
				else if (action == TEXT("download"))
					presult->action = Download;
				else if (action  == TEXT("copy"))
					presult->action = Copy;
				else
					presult->action = None;
				}
			}
			else if(option == TEXT("-filename"))
			{
				if (++i < argc)
					presult->FileName = *++argv;
			}
			else if (option == TEXT("-contenttype"))
			{
				if (++i < argc)
					presult->ContentType = *++argv;
			}
			else if (option == TEXT("-storageaccountname"))
			{
				if (++i < argc)
					presult->StorageAccountName = *++argv;
			}
			else if (option == TEXT("-storageaccountkey"))
			{
				if (++i < argc)
					presult->StorageAccountKey = *++argv;
			}
			else if (option == TEXT("-containername"))
			{
				if (++i < argc)
					presult->ContainerName = *++argv;
			}
			else if (option == TEXT("-blobname"))
			{
				if (++i < argc)
					presult->BlobName = *++argv;
			}
			else if(option == TEXT("-sourcestorageaccountname"))
			{
				if (++i < argc)
					presult->SourceStorageAccountName = *++argv;
			}
			else if(option == TEXT("-sourcestorageaccountkey"))
			{
				if (++i < argc)
					presult->SourceStorageAccountKey = *++argv;
			}
			else if (option == TEXT("-sourcecontainername"))
			{
				if (++i < argc)
					presult->SourceContainerName = *++argv;
			}
			else if (option == TEXT("-sourceblobname"))
			{
				if (++i < argc)
					presult->SourceBlobName = *++argv;
			}
	
		}
		CheckArgs(presult);
		return *presult;	
	}

int _tmain(int argc, _TCHAR* argv[])
{
	ArgsParsed arg = ParseArgs(argc, argv);
	
	if (arg.ErrorMessage.length() == 0)
	{

		if (arg.action == Upload) {
			DumpMessage(TEXT("Uploading file..."));
			if (myapplication::UploadFile(arg.FileName,
				arg.ContentType,
				arg.StorageAccountName,
				arg.StorageAccountKey,
				arg.ContainerName,
				arg.BlobName,
				arg.ErrorMessage) > 0)
				DumpMessage(TEXT("Upload Sucessful"));
			else
			{
				DumpMessage(TEXT("Error while uploading file"));
				DumpMessage(arg.ErrorMessage);
			}
		}
		else if (arg.action == Download) {
			DumpMessage(TEXT("Downloading file..."));
			if (myapplication::DownloadFile(arg.FileName,
				arg.ContentType,
				arg.StorageAccountName,
				arg.StorageAccountKey,
				arg.ContainerName,
				arg.BlobName,
				arg.ErrorMessage) > 0)
				DumpMessage(TEXT("Download Sucessful"));
			else
			{
				DumpMessage(TEXT("Error while downloading file"));
				DumpMessage(arg.ErrorMessage);
			}
		}else
			DumpSyntax(TEXT("Not implemented"));
	}
	else
	{
		DumpSyntax(arg.ErrorMessage);
	}

	return 0;
}

