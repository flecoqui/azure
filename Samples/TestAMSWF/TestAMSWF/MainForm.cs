using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAzure.MediaServices.Client;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace TestAMSWF
{

    public partial class MainForm : Form
    {
        public readonly List<Item> LanguagesIndexV2 = new List<Item> {
            new Item("English", "EnUs"),
            new Item("Spanish", "EsEs"),
            new Item("Chinese", "ZhCn"),
            new Item("French", "FrFr"),
            new Item("German", "DeDe"),
            new Item("Italian", "ItIt"),
            new Item("Portuguese", "PtBr"),
            new Item("Arabic (Egyptian)", "ArEg"),
            new Item("Japanese", "JaJp")
        };
        public MainForm()
        {
            InitializeComponent();
            textBoxAccountName.Text = "testamsindexer";
            textBoxAccountKey.Text = "7osPnuOAoPP3dY48IFLuJxY++V8nq4ICI0rCDy12Hsk=";
            textBoxSearchAccountName.Text = "testamsindexsearch";
            textBoxSearchAccountKey.Text = "50373C7F6D3D38FEA3A672392E059AEA";
            comboBoxLanguages.Items.AddRange(LanguagesIndexV2.ToArray());
            comboBoxLanguages.SelectedIndex = 0;
            _context = null;
            UpdateControls();

        }
        Microsoft.WindowsAzure.MediaServices.Client.CloudMediaContext _context = null;
        Microsoft.Azure.Search.SearchServiceClient _searchContext = null;
        Microsoft.Azure.Search.ISearchIndexClient _indexClient = null;

        void PopulateJobList()
        {
            if(_context!=null)
                foreach( var j in _context.Jobs)
                {
                    TextBoxLogWriteLine(string.Format("Job: {0} is {1} ", j.Name, j.State.ToString()));
                }
        }
        bool IsConnected()
        {
            if ((_context != null)&&(_searchContext != null))
                return true;
            return false;
        }
        void UpdateControls()
        {
            if(IsConnected())
            {
                textBoxAccountKey.Enabled = false;
                textBoxAccountName.Enabled = false;
                textBoxSearchAccountKey.Enabled = false;
                textBoxSearchAccountName.Enabled = false;
                buttonLogin.Enabled = false;

                listInputAssets.Enabled = true;
                listInputFiles.Enabled = true;
                buttonAddAsset.Enabled = true;
                buttonRemoveAsset.Enabled = true;
                listOutputFiles.Enabled = true;
                listOutputAssets.Enabled = true;
                buttonGenerateSubtitle.Enabled = true;
                buttonDonwloadSubtitle.Enabled = true;
                richTextBoxLog.Enabled = true;
                comboBoxLanguages.Enabled = true;
                buttonOpenSubtitle.Enabled = true;
                buttonDisplayJobs.Enabled = true;

                buttonCreateIndex.Enabled = true;
                buttonDeleteIndex.Enabled = true;
                buttonPopulateIndex.Enabled = true;
                buttonSearch.Enabled = true;
                textBoxSearch.Enabled = true;
            }
            else
            {
                textBoxAccountKey.Enabled = true;
                textBoxAccountName.Enabled = true;
                textBoxSearchAccountKey.Enabled = true;
                textBoxSearchAccountName.Enabled = true;
                buttonLogin.Enabled = true;

                listInputAssets.Enabled = false;
                listInputFiles.Enabled = false;
                buttonAddAsset.Enabled = false;
                buttonRemoveAsset.Enabled = false;
                listOutputFiles.Enabled = false;
                listOutputAssets.Enabled = false;
                buttonGenerateSubtitle.Enabled = false;
                buttonDonwloadSubtitle.Enabled = false;
                richTextBoxLog.Enabled = false;
                comboBoxLanguages.Enabled = false;
                buttonOpenSubtitle.Enabled = false;
                buttonDisplayJobs.Enabled = false;
                buttonCreateIndex.Enabled = false;
                buttonDeleteIndex.Enabled = false;
                buttonPopulateIndex.Enabled = false;
                buttonSearch.Enabled = false;
                textBoxSearch.Enabled = false;

            }
        }
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxAccountName.Text))
            {
                MessageBox.Show("The account name cannot be empty.");
                return;
            }
            using (new CursorHandler())
            {
                try
                {
                    _context = new Microsoft.WindowsAzure.MediaServices.Client.CloudMediaContext(textBoxAccountName.Text, textBoxAccountKey.Text);
                    if (_context != null)
                    {
                        _searchContext =  new Microsoft.Azure.Search.SearchServiceClient(textBoxSearchAccountName.Text, new Microsoft.Azure.Search.SearchCredentials(textBoxSearchAccountKey.Text));
                        if (_searchContext != null)
                        {
                            _indexClient = _searchContext.Indexes.GetClient("media");


                            _context.Credentials.RefreshToken();
                            UpdateControls();
                            PopulateInputAssets();
                            PopulateInputFiles();
                            PopulateOutputAssets();
                            PopulateOutputFiles();
                        }
                    }
                }
                catch (Exception ex)

                {
                    MessageBox.Show("Exception: " + ex.Message);
                }
            }
        }
        void PopulateInputAssets()
        {
            listInputAssets.Items.Clear();
            foreach (var asset in _context.Assets)
            {
                if (!asset.Name.EndsWith("Indexed"))
                    listInputAssets.Items.Add("ASSET: " + asset.Name + " ASSET-ID " + asset.Id);
            }
            if (listInputAssets.Items.Count > 0)
                listInputAssets.SelectedItem = 0;

        }
        void PopulateInputFiles(string id)
        {
            listInputFiles.Items.Clear();
            foreach (var file in _context.Files)
            {
                if (id == file.Asset.Id)
                    listInputFiles.Items.Add("FILE: " + file.Name + " ASSET-ID " + file.Asset.Id);
            }
            if (listInputFiles.Items.Count > 0)
                listInputFiles.SelectedItem = 0;

        }
        void PopulateOutputAssets(string id)
        {
            listOutputAssets.Items.Clear();
            string AssetName = string.Empty;
            foreach (var asset in _context.Assets)
            {
                if (id == asset.Id)
                {
                    AssetName = asset.Name;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(AssetName))
            {
                foreach (var asset in _context.Assets)
                {
                    if (asset.Name.StartsWith(AssetName) && asset.Name.EndsWith("ndexed"))
                    {
                        listOutputAssets.Items.Add("ASSET: " + asset.Name + " ASSET-ID " + asset.Id);
                        break;
                    }
                }
                if (listOutputAssets.Items.Count > 0)
                    listOutputAssets.SelectedItem = 0;
            }
        }
        void PopulateOutputFiles(string id)
        {
            listOutputFiles.Items.Clear();
            foreach (var file in _context.Files)
            {
                if (id == file.Asset.Id)
                    listOutputFiles.Items.Add("FILE: " + file.Name + " ASSET-ID " + file.Asset.Id);
            }
            if (listOutputFiles.Items.Count > 0)
                listOutputFiles.SelectedItem = 0;

        }
        private void listAssets_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (new CursorHandler())
            {
                PopulateInputFiles();
                PopulateOutputAssets();
                PopulateOutputFiles();
            }
        }
        void PopulateInputFiles()
        {
            string id = "";
            string s = listInputAssets.SelectedItem as string;
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf(" ASSET-ID ");
                if (pos > 0)
                    id = s.Substring(pos + 10);
            }
            PopulateInputFiles(id);
        }
        void PopulateOutputFiles()
        {
            string id = "";
            string s = listOutputAssets.SelectedItem as string;
            if (string.IsNullOrEmpty(s))
            {
                if (listOutputAssets.Items.Count == 1)
                    s = listOutputAssets.Items[0] as string;
            }
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf(" ASSET-ID ");
                if (pos > 0)
                    id = s.Substring(pos + 10);
            }
            PopulateOutputFiles(id);
        }
        void PopulateOutputAssets()
        {
            string id = "";
            string s = listInputAssets.SelectedItem as string;
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf(" ASSET-ID ");
                if (pos > 0)
                    id = s.Substring(pos + 10);
            }
            PopulateOutputAssets(id);
        }
        private void MyUploadProgressChanged(object sender, Microsoft.WindowsAzure.MediaServices.Client.UploadProgressChangedEventArgs e)
        {
        }
        private void ProcessUploadFileToAsset(string SafeFileName, string FileName, Microsoft.WindowsAzure.MediaServices.Client.IAsset MyAsset)
        {
            try
            {
                Microsoft.WindowsAzure.MediaServices.Client.IAssetFile UploadedAssetFile = MyAsset.AssetFiles.Create(SafeFileName);
                UploadedAssetFile.UploadProgressChanged += MyUploadProgressChanged;
                UploadedAssetFile.Upload(FileName as string);
            }
            catch
            {
                MessageBox.Show("Error when uploading the file");
            }
        }
        private static readonly List<string> InvalidFileNamePrefixList = new List<string>
                {
                    "CON",
                    "PRN",
                    "AUX",
                    "NUL",
                    "COM1",
                    "COM2",
                    "COM3",
                    "COM4",
                    "COM5",
                    "COM6",
                    "COM7",
                    "COM8",
                    "COM9",
                    "LPT1",
                    "LPT2",
                    "LPT3",
                    "LPT4",
                    "LPT5",
                    "LPT6",
                    "LPT7",
                    "LPT8",
                    "LPT9"
                };
        public static bool AssetFileNameIsOk(string filename)
        {
            // check if the filename is compatible with AMS
            // Validates if the asset file name conforms to the following requirements
            // AssetFile name must be a valid blob name.
            // AssetFile name must be a valid NTFS file name.
            // AssetFile name length must be limited to 248 characters. 
            // AssetFileName should not contain the following characters: + % and #
            // AssetFileName should not contain only space(s)
            // AssetFileName should not start with certain prefixes restricted by NTFS such as CON1, PRN ... 
            // An AssetFileName constructed using the above mentioned criteria shall be encoded, streamed and played back successfully.

            if (string.IsNullOrWhiteSpace(filename))
            {
                return false;
            }

            // let's make sure we exract the file name (without the path)
            filename = System.IO.Path.GetFileName(filename);

            // white space
            if (string.IsNullOrWhiteSpace(filename))
            {
                return false;
            }

            if (filename.Length > 248)
            {
                return false;
            }

            if (filename.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) > 0 || System.Text.RegularExpressions.Regex.IsMatch(filename, @"[+%#]+"))
            {
                return false;
            }

            //// Invalid NTFS Filename prefix checks
            if (InvalidFileNamePrefixList.Any(x => filename.StartsWith(x + ".", StringComparison.OrdinalIgnoreCase)) ||
                InvalidFileNamePrefixList.Any(x => filename.Equals(x, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            return true;
        }
        public string storageaccount;
        private async void buttonAddAsset_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();
            Dialog.Multiselect = false;
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                using (new CursorHandler())
                {
                    foreach (string file in Dialog.FileNames)
                    {
                        string filename = System.IO.Path.GetFileName(file);
                        if (!AssetFileNameIsOk(filename))
                        {
                            MessageBox.Show("Filename incorrect to create an asset from", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (string.IsNullOrEmpty(storageaccount)) storageaccount = _context.DefaultStorageAccount.Name; // no storage account or null, then let's take the default one

                        System.Threading.CancellationTokenSource tokenSource = new System.Threading.CancellationTokenSource();
                        Microsoft.WindowsAzure.MediaServices.Client.IAsset asset = await _context.Assets.CreateAsync(filename,
                                                              storageaccount,
                                                              Microsoft.WindowsAzure.MediaServices.Client.AssetCreationOptions.None,
                                                              tokenSource.Token);

                        Microsoft.WindowsAzure.MediaServices.Client.IAssetFile UploadedAssetFile = await asset.AssetFiles.CreateAsync(filename, tokenSource.Token);
                        if (tokenSource.Token.IsCancellationRequested) return;
                        UploadedAssetFile.UploadProgressChanged += UploadProgress;
                        UploadedAssetFile.Upload(file);
                        UploadedAssetFile.IsPrimary = true;
                        UploadedAssetFile.Update();
                    }
                    // Refresh the asset.
                    PopulateInputAssets();
                    PopulateInputFiles();
                    PopulateOutputAssets();
                    PopulateOutputFiles();
                }
            }
        }
        void UploadProgress(object sender, Microsoft.WindowsAzure.MediaServices.Client.UploadProgressChangedEventArgs args)
        {

        }
        public static async Task DeleteAssetAsync(Microsoft.WindowsAzure.MediaServices.Client.CloudMediaContext mediaContext, Microsoft.WindowsAzure.MediaServices.Client.IAsset asset)
        {

            using (new CursorHandler())
            {
                foreach (var locator in asset.Locators.ToArray())
                {
                    await locator.DeleteAsync();
                }
                foreach (var policy in asset.DeliveryPolicies.ToArray())
                {
                    asset.DeliveryPolicies.Remove(policy);
                    await policy.DeleteAsync();
                }
                //foreach (var key in asset.ContentKeys.ToArray())
                //{
                //    CleanupKey(mediaContext, key);
                //    try // because we have an error for FairPlay key
                //    {
                //        asset.ContentKeys.Remove(key);
                //    }
                //    catch
                //    {

                //    }
                //}
                await asset.DeleteAsync();
            }
            return;

        }
        private async void buttonRemoveAsset_Click(object sender, EventArgs e)
        {
            string id = "";
            string s = listInputAssets.SelectedItem as string;
            if (!string.IsNullOrEmpty(s))
            {
                using (new CursorHandler())
                {
                    int pos = s.IndexOf(" ASSET-ID ");
                    if (pos > 0)
                        id = s.Substring(pos + 10);
                    var a = _context.Assets.Where(j => j.Id == id).FirstOrDefault();
                    if (a != null)
                    {
                        foreach (var asset in _context.Assets)
                        {
                            if (asset.Name.EndsWith("Indexed") && asset.Name.StartsWith(a.Name))
                            {
                                if (MessageBox.Show("Do you want to delete the output Asset as well?", "Deleting Output Asset", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                                    await DeleteAssetAsync(_context, asset);
                                break;
                            }
                        }

                        await DeleteAssetAsync(_context, a);
                        PopulateInputAssets();
                        PopulateInputFiles();
                        PopulateOutputAssets();
                        PopulateOutputFiles();
                    }
                }
            }
        }

        private void listOutputAssets_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (new CursorHandler())
            {
                PopulateOutputFiles();
            }
        }
        List<Microsoft.WindowsAzure.MediaServices.Client.IAsset> ReturnSelectedAssets()
        {
            List<Microsoft.WindowsAzure.MediaServices.Client.IAsset> list = new List<Microsoft.WindowsAzure.MediaServices.Client.IAsset>();
            string id = "";
            string s = listInputAssets.SelectedItem as string;
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf(" ASSET-ID ");
                if (pos > 0)
                    id = s.Substring(pos + 10);
            }
            foreach (var asset in _context.Assets)
            {
                if (id == asset.Id)
                {
                    list.Add(asset);
                    break;
                }
            }
            return list;
        }
        private static bool CheckPrimaryFileExtension(List<Microsoft.WindowsAzure.MediaServices.Client.IAsset> SelectedAssets, string[] mediaFileExtensions)
        {
            // if one asset selected
            if (SelectedAssets.Count == 1 && SelectedAssets.FirstOrDefault() != null)
            {
                Microsoft.WindowsAzure.MediaServices.Client.IAsset asset = SelectedAssets.FirstOrDefault();
                Microsoft.WindowsAzure.MediaServices.Client.IAssetFile primary = asset.AssetFiles.Where(f => f.IsPrimary).FirstOrDefault();
                var selectableFiles = asset.AssetFiles.ToList().Where(f => mediaFileExtensions.Contains(System.IO.Path.GetExtension(f.Name).ToUpperInvariant())).ToList();


                // if primary file is not a video file supported but there are video files in asset
                if (primary != null
                    && mediaFileExtensions.Contains(System.IO.Path.GetExtension(primary.Name).ToUpperInvariant())
                    && selectableFiles.Count > 0)
                    return true;
            }
            MessageBox.Show("Source asset must be a single " + string.Join(", ", mediaFileExtensions) + " file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        public Microsoft.WindowsAzure.MediaServices.Client.IMediaProcessor GetLatestMediaProcessorByName(string mediaProcessorName)
        {
            // The possible strings that can be passed into the 
            // method for the mediaProcessor parameter:
            //   Windows Azure Media Encoder
            //   Windows Azure Media Packager
            //   Windows Azure Media Encryptor
            //   Storage Decryption

            var processor = _context.MediaProcessors.Where(p => p.Name == mediaProcessorName).
                ToList().OrderBy(p => new Version(p.Version)).LastOrDefault();

            return processor;
        }
        public void TextBoxLogWriteLine(string message, object o1, bool Error = false)
        {
            TextBoxLogWriteLine(string.Format(message, o1), Error);
        }

        public void TextBoxLogWriteLine(string message, object o1, object o2, bool Error = false)
        {
            TextBoxLogWriteLine(string.Format(message, o1, o2), Error);
        }

        public void TextBoxLogWriteLine(string message, object o1, object o2, object o3, bool Error = false)
        {
            TextBoxLogWriteLine(string.Format(message, o1, o2, o3), Error);
        }

        public void TextBoxLogWriteLine(string message, object o1, object o2, object o3, object o4, bool Error = false)
        {
            TextBoxLogWriteLine(string.Format(message, o1, o2, o3, o4), Error);
        }
        public void TextBoxLogWriteLine()
        {
            TextBoxLogWriteLine(string.Empty);
        }
        public void TextBoxLogWriteLine(Exception e)
        {
            TextBoxLogWriteLine(e.Message, true);
        }
        public void TextBoxLogWriteLine(string text, bool Error = false)
        {
            bool stringEmpty = string.IsNullOrEmpty(text);
            text += Environment.NewLine;
            string date = string.Format("[{0}] ", String.Format("{0:G}", DateTime.Now));

            if (richTextBoxLog.InvokeRequired)
            {
                richTextBoxLog.BeginInvoke(new Action(() =>
                {
                    if (!stringEmpty)
                    {
                        richTextBoxLog.SelectionStart = richTextBoxLog.TextLength;
                        richTextBoxLog.SelectionLength = 0;

                        richTextBoxLog.SelectionColor = Color.Gray;
                        richTextBoxLog.AppendText(date);

                        richTextBoxLog.SelectionStart = richTextBoxLog.TextLength;
                        richTextBoxLog.SelectionLength = 0;

                        richTextBoxLog.SelectionColor = Error ? Color.Red : Color.Black;
                    }
                    richTextBoxLog.AppendText(text);
                    if (!stringEmpty)
                    {
                        richTextBoxLog.SelectionColor = richTextBoxLog.ForeColor;
                    }
                }));
            }
            else
            {
                if (!stringEmpty)
                {
                    richTextBoxLog.SelectionStart = richTextBoxLog.TextLength;
                    richTextBoxLog.SelectionLength = 0;

                    richTextBoxLog.SelectionColor = Color.Gray;
                    richTextBoxLog.AppendText(date);

                    richTextBoxLog.SelectionStart = richTextBoxLog.TextLength;
                    richTextBoxLog.SelectionLength = 0;

                    richTextBoxLog.SelectionColor = Error ? Color.Red : Color.Black;
                }
                richTextBoxLog.AppendText(text);
                if (!stringEmpty)
                {
                    richTextBoxLog.SelectionColor = richTextBoxLog.ForeColor;
                }
            }
        }
        public void LaunchJobs_OneJobPerInputAssetWithSpecificConfig(Microsoft.WindowsAzure.MediaServices.Client.IMediaProcessor processor, List<Microsoft.WindowsAzure.MediaServices.Client.IAsset> selectedassets, string jobname, int jobpriority, string taskname, string outputassetname, List<string> configuration, Microsoft.WindowsAzure.MediaServices.Client.AssetCreationOptions myAssetCreationOptions, Microsoft.WindowsAzure.MediaServices.Client.TaskOptions myTaskOptions, string storageaccountname = "", bool copySubtitlesToInput = false)
        {
            // a job per asset, one task per job, but each task has a specific config
            Task.Factory.StartNew(() =>
            {
                int index = -1;

                foreach (Microsoft.WindowsAzure.MediaServices.Client.IAsset asset in selectedassets)
                {
                    index++;
                    //                    string jobnameloc = jobname.Replace(Constants.NameconvInputasset, asset.Name);
                    string jobnameloc = jobname;
                    Microsoft.WindowsAzure.MediaServices.Client.IJob myJob = _context.Jobs.Create(jobnameloc, jobpriority);

                    string config = configuration[index];

                    //                    string tasknameloc = taskname.Replace(Constants.NameconvInputasset, asset.Name).Replace(Constants.NameconvAMEpreset, config);
                    string tasknameloc = taskname;
                    Microsoft.WindowsAzure.MediaServices.Client.ITask myTask = myJob.Tasks.AddNew(
                           tasknameloc,
                          processor,
                          config,
                          myTaskOptions);

                    myTask.InputAssets.Add(asset);

                    // Add an output asset to contain the results of the task
                    //                    string outputassetnameloc = outputassetname.Replace(Constants.NameconvInputasset, asset.Name).Replace(Constants.NameconvAMEpreset, config);
                    string outputassetnameloc = outputassetname;
                    if (storageaccountname == "")
                    {
                        myTask.OutputAssets.AddNew(outputassetnameloc, asset.StorageAccountName, myAssetCreationOptions); // let's use the same storage account than the input asset
                    }
                    else
                    {
                        myTask.OutputAssets.AddNew(outputassetnameloc, storageaccountname, myAssetCreationOptions);
                    }

                    // Submit the job and wait until it is completed. 
                    bool Error = false;
                    try
                    {
                        TextBoxLogWriteLine("Job '{0}' : submitting...", jobnameloc);
                        myJob.Submit();
                    }

                    catch (Exception ex)
                    {
                        // Add useful information to the exception
                        TextBoxLogWriteLine("Job '{0}' : problem", jobnameloc, true);
                        TextBoxLogWriteLine(ex);
                        Error = true;
                    }
                    if (!Error)
                    {
                        TextBoxLogWriteLine("Job '{0}' : submitted.", jobnameloc);
                        Task.Factory.StartNew(() =>
                        {
                            bool bStop = false;
                            while(bStop == false)
                            {
                                var t = System.Threading.Tasks.Task.Delay(10000);
                                t.Wait();
                                foreach(var j in _context.Jobs)
                                {
                                    if (j.Id == myJob.Id)
                                    {
                                        TextBoxLogWriteLine(string.Format("Job '{0}' : {1}",jobnameloc,j.State.ToString()));
                                        if ((j.State == JobState.Canceled) ||
                                            (j.State == JobState.Error) ||
                                            (j.State == JobState.Finished))
                                        {
                                            bStop = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        );
                    }
                 //   TextBoxLogWriteLine();
                }

                //                DotabControlMainSwitch(Constants.TabJobs);
                //                DoRefreshGridJobV(false);
            }

                );
        }
        public const string WindowsAzureMediaEncoder = "Windows Azure Media Encoder";
        public const string AzureMediaEncoder = "Azure Media Encoder";
        public const string AzureMediaEncoderStandard = "Media Encoder Standard";
        public const string AzureMediaEncoderPremiumWorkflow = "Media Encoder Premium Workflow";
        public const string AzureMediaIndexer = "Azure Media Indexer";
        public const string AzureMediaIndexer2Preview = "Azure Media Indexer 2 Preview";
        public const string AzureMediaHyperlapse = "Azure Media Hyperlapse";
        public const string AzureMediaFaceDetector = "Azure Media Face Detector";
        public const string AzureMediaRedactor = "Azure Media Redactor";
        public const string AzureMediaMotionDetector = "Azure Media Motion Detector";
        public const string AzureMediaStabilizer = "Azure Media Stabilizer";
        public const string AzureMediaVideoThumbnails = "Azure Media Video Thumbnails";
        public const string AzureMediaVideoOCR = "Azure Media OCR";
        public const string AzureMediaContentModerator = "Azure Media Content Moderator";
        public string JsonConfig(string Language, bool bSAMI, bool bWebVTT, bool bTTML)
        {
            // Example of config :
            /*
              	{
                      "Features": [{
                                      "Options": {
                                                      "Formats": ["WebVtt", "Sami", “TTML”],
                                                      "Language": "EnUs",
                                                      "Type": "RecoOptions"
                                      },
                                      "Type": "SpReco"
                      }],
                      "Version": 1.0
      }
             */

            dynamic obj = new Newtonsoft.Json.Linq.JObject();
            obj.Version = "1.0";
            obj.Features = new Newtonsoft.Json.Linq.JArray();
            dynamic Feature = new Newtonsoft.Json.Linq.JObject();
            obj.Features.Add(Feature);
            dynamic Option = new Newtonsoft.Json.Linq.JObject();
            Feature.Options = Option;
            dynamic Format = new Newtonsoft.Json.Linq.JArray();

            if (bSAMI) Format.Add("Sami");
            if (bWebVTT) Format.Add("WebVtt");
            if (bTTML) Format.Add("TTML");

            Option.Formats = Format;
            Option.Language = Language;
            Option.Type = "RecoOptions";
            Feature.Type = "SpReco";

            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        private void buttonGenerateSubtitle_Click(object sender, EventArgs e)
        {
            List<Microsoft.WindowsAzure.MediaServices.Client.IAsset> SelectedAssets = ReturnSelectedAssets();

            if (SelectedAssets.Count == 0)
            {
                MessageBox.Show("No asset was selected");
                return;
            }
            if (SelectedAssets.Count != 1)
            {
                MessageBox.Show("More than one asset selected");
                return;
            }

            if (SelectedAssets.FirstOrDefault() == null) return;


            //var l = SelectedAssets.FirstOrDefault().GetSmoothStreamingUri();

            // Removed as not supported by Indexer v2 Preview
            //var proposedfiles = CheckSingleFileIndexerSupportedExtensions(SelectedAssets);
            if (CheckPrimaryFileExtension(SelectedAssets, new[] { ".MP4", ".WMV", ".MP3", ".M4A", ".WMA", ".AAC", ".WAV" }) == false)
                return;

            // Get the SDK extension method to  get a reference to the Azure Media Indexer.
            Microsoft.WindowsAzure.MediaServices.Client.IMediaProcessor processor = GetLatestMediaProcessorByName(AzureMediaIndexer2Preview);

            string IndexerJobName = "Media Indexing v2 of " + SelectedAssets.FirstOrDefault().Name;
            string IndexerOutputAssetName = SelectedAssets.FirstOrDefault().Name + " - Indexed";
            string IndexerInputAssetName = SelectedAssets.FirstOrDefault().Name;
            string taskname = "Media Indexing v2 of " + SelectedAssets.FirstOrDefault().Name;

            {
                var ListConfig = new List<string>();
                foreach (var asset in SelectedAssets)
                {
                    //new Item("English", "EnUs"),
                    //new Item("Spanish", "EsEs"),
                    //new Item("Chinese", "ZhCn"),
                    //new Item("French", "FrFr"),
                    //new Item("German", "DeDe"),
                    //new Item("Italian", "ItIt"),
                    //new Item("Portuguese", "PtBr"),
                    //new Item("Arabic (Egyptian)", "ArEg"),
                    //new Item("Japanese", "JaJp")
                    string language = "FrFr";
                    Item item = comboBoxLanguages.SelectedItem as Item;
                    if (item != null)
                        language = item.Value;
                    if (string.IsNullOrEmpty(language))
                        language = "FrFr";
                    ListConfig.Add(JsonConfig(language, false, false, true));
                }
                LaunchJobs_OneJobPerInputAssetWithSpecificConfig(
                            processor,
                            SelectedAssets,
                            IndexerJobName,
                            //Priority
                            10,
                            taskname,
                            IndexerOutputAssetName,
                            ListConfig,
                            // OutputAssetsCreationOptions ? AssetCreationOptions.StorageEncrypted : AssetCreationOptions.None
                            Microsoft.WindowsAzure.MediaServices.Client.AssetCreationOptions.None,
                            //TasksOptionsSetting =
                            //    (checkBoxUseProtectedConfig.Checked ? TaskOptions.ProtectedConfiguration : TaskOptions.None) |
                            //    (checkBoxDoNotCancelOnJobFailure.Checked ? TaskOptions.DoNotCancelOnJobFailure : TaskOptions.None) |
                            //    (checkBoxDoNotDeleteOutputAssetOnFailure.Checked ? TaskOptions.DoNotDeleteOutputAssetOnFailure : TaskOptions.None),
                            Microsoft.WindowsAzure.MediaServices.Client.TaskOptions.None,
                            //form.JobOptions.StorageSelected,
                            _context.DefaultStorageAccount.Name,
                            //CopySubtitlesFilesToInputAsset
                            false
                                );
            }
        }
        private List<IAssetFile> ReturnSelectedAssetFiles()
        {
            List<IAssetFile> Selection = new List<IAssetFile>();
            string assetID = string.Empty;
            string fileName = string.Empty;

            string s = listOutputFiles.SelectedItem as string;
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf("FILE: ");
                if (pos >= 0)
                {
                    int end = s.IndexOf(" ASSET-ID ");
                    if (end > 0)
                    {
                        fileName = s.Substring(pos + 6, end - pos - 6);
                        fileName.Trim();
                        assetID = s.Substring(end + 10, s.Length - end - 10);
                        assetID.Trim();
                        foreach (var file in _context.Files)
                        {
                            if ((assetID == file.Asset.Id) && (fileName == file.Name))
                            {
                                Selection.Add(file);
                                break;
                            }
                        }
                    }
                }
            }
            return Selection;
        }
        private IAsset ReturnAsset(IAssetFile file)
        {
            foreach (var asset in _context.Assets)
            {
                if (file.Asset.Id == asset.Id)
                {
                    return asset;
                }
            }
            return null;
        }
        private void buttonPlaySubtitle_Click(object sender, EventArgs e)
        {
            var SelectedAssetFiles = ReturnSelectedAssetFiles();
            if (SelectedAssetFiles.Count > 0)
            {
                FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();
                if (openFolderDialog.ShowDialog(this) == DialogResult.OK)
                {
                    var listfiles = SelectedAssetFiles.ToList().Where(f => System.IO.File.Exists(openFolderDialog.SelectedPath + @"\\" + f.Name)).Select(f => openFolderDialog.SelectedPath + @"\\" + f.Name).ToList();
                    if (listfiles.Count > 0)
                    {
                        string text;
                        if (listfiles.Count > 1)
                        {
                            text = string.Format(
                                                "The following files are already in the folder(s)\n\n{0}\n\nOverwite the files ?",
                                                string.Join("\n", listfiles.Select(f => System.IO.Path.GetFileName(f)).ToArray())
                                                );
                        }
                        else
                        {
                            text = string.Format(
                                                 "The following file is already in the folder\n\n{0}\n\nOverwite the file ?",
                                                 string.Join("\n", listfiles.Select(f => System.IO.Path.GetFileName(f)).ToArray())
                                                 );
                        }

                        if (MessageBox.Show(text, "File(s) overwrite", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                        {
                            return;
                        }
                        try
                        {
                            listfiles.ForEach(f => System.IO.File.Delete(f));
                        }
                        catch
                        {
                            MessageBox.Show("Error when deleting files", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    try
                    {
                        foreach (var assetfile in SelectedAssetFiles)
                        {
                            Guid Id = Guid.NewGuid();
                            // to cancel task if needed
                            System.Threading.CancellationTokenSource tokenSource = new System.Threading.CancellationTokenSource();
                            System.Threading.CancellationToken tokenloc = tokenSource.Token;

                            var response = new TransferEntryResponse() { Id = Id, token = tokenloc };

                            // Start a worker thread that does downloading.
                            DoDownloadFileFromAsset(ReturnAsset(assetfile), assetfile, openFolderDialog.SelectedPath, response);
                        }
                        MessageBox.Show("Download process has been initiated. See the Transfers tab to check the progress.");

                    }
                    catch
                    {
                        MessageBox.Show("Error when downloading file(s)");
                    }
                }
            }
        }

        public void DoDownloadFileFromAsset(Microsoft.WindowsAzure.MediaServices.Client.IAsset asset, Microsoft.WindowsAzure.MediaServices.Client.IAssetFile File, object folder, TransferEntryResponse response)
        {
            // If download is in the queue, let's wait our turn
            //DoGridTransferWaitIfNeeded(response.Id);
            //if (response.token.IsCancellationRequested)
            //{
            //    DoGridTransferDeclareCancelled(response.Id);
            //    return;
            //}

            string labeldb = string.Format("Starting download of '{0}' of asset '{1}' to {2}", File.Name, asset.Name, folder as string);
            Microsoft.WindowsAzure.MediaServices.Client.ILocator sasLocator = null;
            var locatorTask = Task.Factory.StartNew(() =>
            {
                sasLocator = _context.Locators.Create(Microsoft.WindowsAzure.MediaServices.Client.LocatorType.Sas, asset, Microsoft.WindowsAzure.MediaServices.Client.AccessPermissions.Read, TimeSpan.FromHours(24));
            });
            locatorTask.Wait();

            TextBoxLogWriteLine(labeldb);

            BlobTransferClient blobTransferClient = new BlobTransferClient
            {
                NumberOfConcurrentTransfers = _context.NumberOfConcurrentTransfers,
                ParallelTransferThreadCount = _context.ParallelTransferThreadCount
            };
            
            var myTask = Task.Factory.StartNew(async () =>
            {
                bool Error = false;
                try
                {
                    await File.DownloadAsync(System.IO.Path.Combine(folder as string, File.Name), blobTransferClient, sasLocator, response.token);
                    sasLocator.Delete();
                }
                catch (Exception e)
                {
                    Error = true;
                    TextBoxLogWriteLine(string.Format("Download of file '{0}' failed !", File.Name), true);
                    TextBoxLogWriteLine(e);
                    //                    DoGridTransferDeclareError(response.Id, e);
                }
                if (!Error)
                {
                    if (!response.token.IsCancellationRequested)
                    {
                        //                      DoGridTransferDeclareCompleted(response.Id, folder.ToString());
                    }
                    else
                    {
                        //                    DoGridTransferDeclareCancelled(response.Id);
                    }
                }
            }, response.token);
        }
        public static string GetTempFilePathWithExtension(string extension)
        {
            var path = System.IO.Path.GetTempPath();
            var fileName = Guid.NewGuid().ToString() + extension;
            return System.IO.Path.Combine(path, fileName);
        }
        public string GetAssetFileContent(  Microsoft.WindowsAzure.MediaServices.Client.IAsset asset, 
                                            Microsoft.WindowsAzure.MediaServices.Client.IAssetFile File)
        {
            // If download is in the queue, let's wait our turn
            //DoGridTransferWaitIfNeeded(response.Id);
            //if (response.token.IsCancellationRequested)
            //{
            //    DoGridTransferDeclareCancelled(response.Id);
            //    return;
            //}
            string content = string.Empty;
            string localfile = GetTempFilePathWithExtension(".ttml");

            Microsoft.WindowsAzure.MediaServices.Client.ILocator sasLocator = null;
            var locatorTask = Task.Factory.StartNew(() =>
            {
                sasLocator = _context.Locators.Create(Microsoft.WindowsAzure.MediaServices.Client.LocatorType.Sas, asset, Microsoft.WindowsAzure.MediaServices.Client.AccessPermissions.Read, TimeSpan.FromHours(24));
            });
            locatorTask.Wait();
            File.Download(localfile);

            var myTask = Task.Factory.StartNew(() =>
            {
                bool Error = false;
                try
                {
                    File.Download(localfile);
                    sasLocator.Delete();
                }
                catch (Exception e)
                {
                    Error = true;
                    TextBoxLogWriteLine(string.Format("Download of file '{0}' failed !", File.Name), true);
                    TextBoxLogWriteLine(e);
                }
                if (!Error)
                {
                }
            });
            myTask.Wait();

            using (System.IO.StreamReader sr = new System.IO.StreamReader(localfile))
            {
                // Read the stream to a string, and write the string to the console.
                content = sr.ReadToEnd();
            }
            return content;

        }

        private async void buttonRemoveOutputAsset_Click(object sender, EventArgs e)
        {
            string id = "";
            string s = listOutputAssets.SelectedItem as string;
            if (!string.IsNullOrEmpty(s))
            {
                using (new CursorHandler())
                {
                    int pos = s.IndexOf(" ASSET-ID ");
                    if (pos > 0)
                        id = s.Substring(pos + 10);
                    var a = _context.Assets.Where(j => j.Id == id).FirstOrDefault();
                    if (a != null)
                    {
                        await DeleteAssetAsync(_context, a);
                        PopulateOutputAssets();
                        PopulateOutputFiles();
                    }
                }
            }
        }
        public ILocator tempLocator = null;
        public IAsset tempAsset = null;
        private ILocator GetTemporaryLocator(IAsset asset)
        {
            if((tempAsset!=null) &&(tempAsset.Id != asset.Id)&& (tempLocator != null))
//            if (tempLocator != null)
            {
                try
                {
                    var locatorTask = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            tempLocator.Delete();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show("Exception when creating the temporary SAS locator." + ex.Message);
                        }
                    });
                    locatorTask.Wait();
                    locatorTask.Wait(1000);
                    tempLocator = null;
                }
                catch
                {

                }
            }
            if (tempLocator == null) // no temp locator, let's create it
            {
                try
                {
                    var locatorTask = Task.Factory.StartNew(() =>
                    {
                        tempLocator = _context.Locators.Create(LocatorType.Sas, asset, AccessPermissions.Read, TimeSpan.FromHours(1));
                        tempAsset = asset;
                    });
                    locatorTask.Wait();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when creating the temporary SAS locator." + ex.Message);
                }
            }
            return tempLocator;
        }
        private void buttonOpenSubtitle_Click(object sender, EventArgs e)
        {
            var SelectedAssetFiles = ReturnSelectedAssetFiles();
            if (SelectedAssetFiles.Count > 0)
            {
                var af = SelectedAssetFiles.FirstOrDefault();
                ILocator locator = GetTemporaryLocator(af.Asset);
                if (locator != null)
                {
                    try
                    {
                        foreach (var assetfile in SelectedAssetFiles)
                        {

                            System.Diagnostics.Process.Start(assetfile.GetSasUri(locator).ToString());
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Error when accessing temporary SAS locator");
                    }
                }
            }
        }

        private void buttonDisplayJobs_Click(object sender, EventArgs e)
        {
            PopulateJobList();
        }

        public async Task<bool> DeleteSearchIndex()
        {
            bool result = false;
            if (_searchContext != null)
            {
                var response = _searchContext.Indexes.ExistsWithHttpMessagesAsync("media");
                if ((response != null) && (response.Status != TaskStatus.Created))
                {
                    if (response.Result.Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await _searchContext.Indexes.DeleteWithHttpMessagesAsync("media");
                        _indexClient = null;
                        result = true;
                    }
                }
            }
            return result;
        }
        public  bool CreateSearchIndex()
        {
           
            bool result = false;
            if (_searchContext != null)
            {
                var definition = new Microsoft.Azure.Search.Models.Index()
                {
                    Name = "media",
                    Fields = new[]
                    {
                        new Microsoft.Azure.Search.Models.Field("mediaId", Microsoft.Azure.Search.Models.DataType.String)                       { IsKey = true },
                        new Microsoft.Azure.Search.Models.Field("mediaName", Microsoft.Azure.Search.Models.DataType.String)                     { IsSearchable = true, IsFilterable = true },
                        new Microsoft.Azure.Search.Models.Field("mediaUrl", Microsoft.Azure.Search.Models.DataType.String) { IsSearchable = true, IsFilterable = true },
                        new Microsoft.Azure.Search.Models.Field("mediaContent", Microsoft.Azure.Search.Models.DataType.String)                     { IsSearchable = true, IsFilterable = true }
                    }
                };

                var response = _searchContext.Indexes.CreateWithHttpMessagesAsync(definition);
                if ((response != null) && (response.Status != TaskStatus.Created))
                {
                    if (response.Result.Response.StatusCode == System.Net.HttpStatusCode.Created)
                    {
                        _indexClient = _searchContext.Indexes.GetClient("media");
                        result = true;
                    }
                }
            }
            return result;
        }
        public bool UploadTestIndex()
        {

            bool result = false;
            if (_searchContext != null)
            {
                var documents = new Media[]
                {
                    new Media()
                    {
                      mediaId = "1111",
                      mediaName = "Name1",
                      mediaUrl = "http://media",
                      mediaContent = "ti too  a a a toot"
                    },
                    new Media()
                    {
                      mediaId = "1112",
                      mediaName = "Name2",
                      mediaUrl = "http://medi2",
                      mediaContent = "ti tiiti toot2"
                    }
                };
                try
                {
                    var batch = Microsoft.Azure.Search.Models.IndexBatch.Upload(documents);
                    if(_indexClient!=null)
                        _indexClient.Documents.Index(batch);
                    result = true;
                }
                catch (IndexBatchException e)
                {
                    // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                    // the batch. Depending on your application, you can take compensating actions like delaying and
                    // retrying. For this simple demo, we just log the failed document keys and continue.
                    Console.WriteLine(
                        "Failed to index some of the documents: {0}",
                        String.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
                }

                // Wait a while for indexing to complete.
                System.Threading.Tasks.Task.Delay(2000);
                    result = true;
            }
            return result;
        }
        public bool UploadIndex()
        {
            bool result = false;
            if ((_context != null)&&(_searchContext != null))
            {
                List<Media> documents = new List<Media>();
                foreach (var asset in _context.Assets)
                {
                    if (!asset.Name.EndsWith("Indexed"))
                    {
                        foreach (var a in _context.Assets)
                        {
                            if (a.Name.StartsWith(asset.Name) && a.Name.EndsWith("Indexed"))
                            {

                                foreach (var file in _context.Files)
                                {
                                    if ((a.Id == file.Asset.Id)&&(file.Name.EndsWith(".ttml",StringComparison.OrdinalIgnoreCase)))
                                    {
                                        string content = GetAssetFileContent(a, file);
                                        byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(asset.Id);
                                        Media media = new Media()
                                        {
                                            mediaId = System.Convert.ToBase64String(toEncodeAsBytes),
                                            mediaName = asset.Name,
                                            mediaUrl = asset.Id,
                                            mediaContent = content
                                        };
                                        documents.Add(media);
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                try
                {
                    var batch = Microsoft.Azure.Search.Models.IndexBatch.Upload(documents);
                    if (_indexClient != null)
                        _indexClient.Documents.Index(batch);
                    result = true;
                }
                catch (IndexBatchException e)
                {
                    // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                    // the batch. Depending on your application, you can take compensating actions like delaying and
                    // retrying. For this simple demo, we just log the failed document keys and continue.
                    Console.WriteLine(
                        "Failed to index some of the documents: {0}",
                        String.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
                }

                // Wait a while for indexing to complete.
                System.Threading.Tasks.Task.Delay(2000);
                result = true;
            }
            return result;
        }

        private void buttonCreateIndex_Click(object sender, EventArgs e)
        {
            bool res = CreateSearchIndex();
            if(res == true)
                TextBoxLogWriteLine("Azure Search Index Created");
            else
                TextBoxLogWriteLine("Azure Search Index not Created");

        }

        private async void buttonDeleteIndex_Click(object sender, EventArgs e)
        {
            bool res = await DeleteSearchIndex();
            if (res == true)
                TextBoxLogWriteLine("Azure Search Index Deleted");
            else
                TextBoxLogWriteLine("Azure Search Index not Deleted");
        }

        private void buttonPopulateIndex_Click(object sender, EventArgs e)
        {
            bool res = UploadIndex();
            if (res == true)
                TextBoxLogWriteLine("Azure Search Index populated");
            else
                TextBoxLogWriteLine("Azure Search Index not populated");
        }
        private bool Search(string text)
        {
            bool result = false;
            if (_indexClient != null)
            {
                DocumentSearchResult<Media> response = null;
                try
                {
                    response = _indexClient.Documents.Search<Media>(text);
                }
                catch(Exception e)
                {
                    TextBoxLogWriteLine("Exception while searching media found for : " + textBoxSearch.Text + " exception: " + e.Message);
                }
                if ((response != null) && (response.Results.Count > 0))
                {
                    TextBoxLogWriteLine("Search result for : " + textBoxSearch.Text);
                    foreach (SearchResult<Media> res in response.Results)
                    {
                        TextBoxLogWriteLine(res.Document.ToString());
                    }
                    result = true;
                }
                else
                {
                    TextBoxLogWriteLine("No media found for : " + textBoxSearch.Text);
                }
            }
            return result;
        }
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            Search(textBoxSearch.Text);
        }
    }

    public class Media
    {
        public string mediaId { set; get; } 
        public string mediaName { set; get; }
        public string mediaUrl { set; get; }
        public string mediaContent { set; get; }
        public override string ToString()
        {
            return String.Format(
                "ID: {0}\tName: {1}\tUrl: {2}",
                mediaId,
                mediaName,
                mediaUrl);
        }
    }
    public class TransferEntryResponse
    {
        public Guid Id;
        public System.Threading.CancellationToken token;
    }
    public class CursorHandler
        : IDisposable
    {
        public CursorHandler(Cursor cursor = null)
        {
            _saved = Cursor.Current;
            Cursor.Current = cursor ?? Cursors.WaitCursor;
        }

        public void Dispose()
        {
            if (_saved != null)
            {
                Cursor.Current = _saved;
                _saved = null;
            }
        }

        private Cursor _saved;
    }
    public class Item
    {
        public string Name;
        public string Value;
        public Item(string name, string value)
        {
            Name = name; Value = value;
        }
        public override string ToString()
        {
            // Generates the text shown in the combo box
            return Name;
        }
    }
}
