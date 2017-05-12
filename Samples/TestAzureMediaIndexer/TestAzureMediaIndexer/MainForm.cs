﻿using System;
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
using TestAzureMediaIndexer;

namespace TestAzureMediaIndexer
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
        public readonly List<Item> TranslateLanguagesIndex = new List<Item> { };

        void LoadSettings()
        {
            string a = string.Empty;
            if (TestAzureMediaIndexer.Properties.Settings.Default.TryGetValue("AzureMediaAccountName", out a))
                textBoxMediaAccountName.Text = a;
            else
                textBoxMediaAccountName.Text = a;
            a = string.Empty;
            if (TestAzureMediaIndexer.Properties.Settings.Default.TryGetValue("AzureMediaAccountKey", out a))
                textBoxMediaAccountKey.Text = a;
            else
                textBoxMediaAccountKey.Text = a;

            /*
            a = string.Empty;
            if (TestAzureMediaIndexer.Properties.Settings.Default.TryGetValue("AzureStorageAccountName", out a))
                textBoxStorageAccountName.Text = a;
            else
                textBoxStorageAccountName.Text = a;
            a = string.Empty;
            if (TestAzureMediaIndexer.Properties.Settings.Default.TryGetValue("AzureStorageAccountKey", out a))
                textBoxStorageAccountKey.Text = a;
            else
                textBoxStorageAccountKey.Text = a;
            a = string.Empty;
            if (TestAzureMediaIndexer.Properties.Settings.Default.TryGetValue("AzureStorageContainer", out a))
                textBoxStorageContainerName.Text = a;
            else
                textBoxStorageContainerName.Text = a;
            */

            a = string.Empty;
            if (TestAzureMediaIndexer.Properties.Settings.Default.TryGetValue("AzureSearchAccountName", out a))
                textBoxSearchAccountName.Text = a;
            else
                textBoxSearchAccountName.Text = a;
            a = string.Empty;
            if (TestAzureMediaIndexer.Properties.Settings.Default.TryGetValue("AzureSearchAccountKey", out a))
                textBoxSearchAccountKey.Text = a;
            else
                textBoxSearchAccountKey.Text = a;
            a = "myapp";
            if (TestAzureMediaIndexer.Properties.Settings.Default.TryGetValue("AssetPrefix", out a))
            {
                if (string.IsNullOrEmpty(a))
                    a = "myapp";
                textBoxAssetPrefix.Text = a;
            }
            else
                textBoxAssetPrefix.Text = a;
            a = "http://ampdemo.azureedge.net/azuremediaplayer.html";
            if (TestAzureMediaIndexer.Properties.Settings.Default.TryGetValue("PlayerUri", out a))
            {
                if (string.IsNullOrEmpty(a))
                    a = "http://ampdemo.azureedge.net/azuremediaplayer.html";
                textBoxPlayerUri.Text = a;
            }
            else
                textBoxPlayerUri.Text = a;
            a = string.Empty;
            if (TestAzureMediaIndexer.Properties.Settings.Default.TryGetValue("TranslatorAPIKey", out a))
                textBoxTranslatorAPIKey.Text = a;
            else
                textBoxTranslatorAPIKey.Text = a;
            /*
            textBoxMediaAccountName.Text = "testamsindexer";
            textBoxMediaAccountKey.Text = "7osPnuOAoPP3dY48IFLuJxY++V8nq4ICI0rCDy12Hsk=";
            textBoxSearchAccountName.Text = "testamsindexsearch";
            textBoxSearchAccountKey.Text = "50373C7F6D3D38FEA3A672392E059AEA";
            textBoxStorageAccountName.Text = "testamsindexersto";
            textBoxStorageAccountKey.Text = "06KUYkR81KFZDpmSw8NvArrXwW/qW3gW9J5Yt6hSEeadkt9+vhAZw3rirYVcfR84tTAGs4yWUGnZ/lNLkg2tEQ==";
            textBoxStorageContainerName.Text = "test";
            */
            comboBoxLanguages.Items.AddRange(LanguagesIndexV2.ToArray());
            comboBoxLanguages.SelectedIndex = 0;
            comboBoxTranslateLanguages.Items.AddRange(LanguagesIndexV2.ToArray());
            comboBoxTranslateLanguages.SelectedIndex = 0;
        }
        void SaveSettings()
        {
            string a = string.Empty;
            TestAzureMediaIndexer.Properties.Settings.Default.SetValue("AzureMediaAccountName", textBoxMediaAccountName.Text);
            TestAzureMediaIndexer.Properties.Settings.Default.SetValue("AzureMediaAccountKey", textBoxMediaAccountKey.Text);
            /*
            TestAzureMediaIndexer.Properties.Settings.Default.SetValue("AzureStorageAccountName", textBoxStorageAccountName.Text);
            TestAzureMediaIndexer.Properties.Settings.Default.SetValue("AzureStorageAccountKey", textBoxStorageAccountKey.Text);
            TestAzureMediaIndexer.Properties.Settings.Default.SetValue("AzureStorageContainer", textBoxStorageContainerName.Text);
            */
            TestAzureMediaIndexer.Properties.Settings.Default.SetValue("AzureSearchAccountName", textBoxSearchAccountName.Text);
            TestAzureMediaIndexer.Properties.Settings.Default.SetValue("AzureSearchAccountKey", textBoxSearchAccountKey.Text);
            TestAzureMediaIndexer.Properties.Settings.Default.SetValue("AssetPrefix", textBoxAssetPrefix.Text);
            TestAzureMediaIndexer.Properties.Settings.Default.SetValue("PlayerUri", textBoxPlayerUri.Text);
            TestAzureMediaIndexer.Properties.Settings.Default.SetValue("TranslatorAPIKey", textBoxTranslatorAPIKey.Text);
            TestAzureMediaIndexer.Properties.Settings.Default.Save();
        }
        public MainForm()
        {

            InitializeComponent();
            this.Icon = Properties.Resources.AppIcon;
            _context = null;
            LoadSettings();

            UpdateControls();

        }
        Microsoft.WindowsAzure.MediaServices.Client.CloudMediaContext _context = null;
        Microsoft.Azure.Search.SearchServiceClient _searchContext = null;
        Microsoft.Azure.Search.ISearchIndexClient _indexClient = null;
        TranslatorTextClient _ttc = null;

        void PopulateJobList()
        {
            if (_context != null)
                foreach (var j in _context.Jobs)
                {
                    TextBoxLogWriteLine(string.Format("Job: {0} is {1} ", j.Name, j.State.ToString()));
                }
        }
        bool IsConnected()
        {
            if ((_context != null) && (_searchContext != null) && (_ttc != null))
                return true;
            return false;
        }
        bool IsAudioInputAssetSelected()
        {
            string name = "";
            string s = (listInputAssets.Items.Count > 0 ? listInputAssets.SelectedItem as string : string.Empty);
            if (!string.IsNullOrEmpty(s))
            {
                s = s.Replace("ASSET: ", "");
                int pos = s.IndexOf(" ASSET-ID ");
                if (pos > 0)
                {
                    name = s.Substring(0, pos);
                    if (name.StartsWith(GetInputAssetPrefix()))
                        name = name.Replace(GetInputAssetPrefix(), "");
                    if (name.StartsWith(GetAssetAudioMidfix()))
                        return true;
                }
            }
            return false;
        }
        string GetSearchSelectedUrl()
        {
            string url = string.Empty;
            string s = (listSearchResult.Items.Count > 0 ? listSearchResult.SelectedItem as string : string.Empty);
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf(" URL: ");
                if (pos > 0)
                {
                    url = s.Substring(pos + 6);
                }
            }
            return url;
        }
        bool IsSearchSelectedUrlAudio()
        {
            string s = (listSearchResult.Items.Count > 0 ? listInputAssets.SelectedItem as string : string.Empty);
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf("AUDIO: ");
                if (pos >= 0)
                {
                    return true;
                }
            }
            return false;
        }
        bool IsSearchResultSelected()
        {
            string url = GetSearchSelectedUrl();
            if (string.IsNullOrEmpty(url))
                return false;
            return true;
        }
        void UpdateControls()
        {
            richTextBoxLog.Enabled = true;
            if (IsConnected())
            {
                textBoxMediaAccountKey.Enabled = false;
                textBoxMediaAccountName.Enabled = false;
                textBoxSearchAccountKey.Enabled = false;
                textBoxSearchAccountName.Enabled = false;
                /*
                textBoxStorageAccountKey.Enabled = false;
                textBoxStorageAccountName.Enabled = false;
                textBoxStorageContainerName.Enabled = false;
                */
                textBoxTranslatorAPIKey.Enabled = false;
                textBoxAssetPrefix.Enabled = false;
                textBoxPlayerUri.Enabled = false;
                buttonLogin.Enabled = false;

                listInputAssets.Enabled = true;
                listInputFiles.Enabled = true;
                listOutputFiles.Enabled = true;
                listOutputAssets.Enabled = true;


                buttonDisplayJobs.Enabled = true;

                bool bIndexExists = SearchIndexExists();

                buttonCreateIndex.Enabled = ((bIndexExists == true) ? false : true);
                buttonDeleteIndex.Enabled = ((bIndexExists == true) ? true : false);
                buttonSearch.Enabled = ((bIndexExists == true) ? true : false);
                textBoxSearch.Enabled = ((bIndexExists == true) ? true : false);
                listSearchResult.Enabled = ((bIndexExists == true) ? true : false);
                buttonPlaySearch.Enabled = (((bIndexExists == true)&&(IsSearchResultSelected())) ? true : false);

                //buttonSearch.Enabled = (((_indexClient != null) && (_indexClient.Documents.Count() > 0)) ? true : false);
                //textBoxSearch.Enabled = (((_indexClient != null) && (_indexClient.Documents.Count() > 0)) ? true : false);


                if ((bUploadingAsset != true) && (bProcessingAsset != true))
                {
                    buttonAddAsset.Enabled = true;

                    if (listInputAssets.SelectedIndex >= 0)
                    {
                        buttonRemoveAsset.Enabled = true;
                        buttonGenerateSubtitle.Enabled = true;
                        if (listOutputAssets.SelectedIndex >= 0)
                        {
                            buttonPlayAudioSubtitle.Enabled = true;
                            buttonTranslateSubtitile.Enabled = true;
                            buttonUpdateSubtitle.Enabled = true;
                            buttonOpenSubtitle.Enabled = true;
                            buttonDonwloadSubtitle.Enabled = true;
                            comboBoxTranslateLanguages.Enabled = true;
                            buttonPopulateIndex.Enabled = ((bIndexExists == true) ? true : false);
                            if (IsAudioInputAssetSelected() == true)
                                buttonPlayVideoSubtitle.Enabled = false;
                            else
                                buttonPlayVideoSubtitle.Enabled = true;
                        }
                        else
                        {
                            buttonPopulateIndex.Enabled = false;
                            buttonPlayAudioSubtitle.Enabled = false;
                            buttonTranslateSubtitile.Enabled = false;
                            buttonUpdateSubtitle.Enabled = false;
                            comboBoxTranslateLanguages.Enabled = false;
                            buttonPlayVideoSubtitle.Enabled = false;
                            buttonOpenSubtitle.Enabled = false;
                            buttonDonwloadSubtitle.Enabled = false;
                        }
                    }
                    else
                    {
                        buttonPopulateIndex.Enabled = false;
                        buttonRemoveAsset.Enabled = false;
                        buttonGenerateSubtitle.Enabled = false;
                        buttonPlayAudioSubtitle.Enabled = false;
                        buttonPlayVideoSubtitle.Enabled = false;
                        buttonTranslateSubtitile.Enabled = false;
                        buttonUpdateSubtitle.Enabled = false;
                        comboBoxTranslateLanguages.Enabled = false;
                    }
                    comboBoxLanguages.Enabled = true;
                    

                }
                else
                {
                    buttonAddAsset.Enabled = false;
                    buttonRemoveAsset.Enabled = false;
                    buttonGenerateSubtitle.Enabled = false;
                    buttonDonwloadSubtitle.Enabled = false;
                    comboBoxLanguages.Enabled = false;
                    buttonOpenSubtitle.Enabled = false;
                    buttonPlayAudioSubtitle.Enabled = false;
                    buttonPlayVideoSubtitle.Enabled = false;
                    buttonTranslateSubtitile.Enabled = false;
                    buttonUpdateSubtitle.Enabled = false;
                    comboBoxTranslateLanguages.Enabled = false;
                }

            }
            else
            {
                textBoxMediaAccountKey.Enabled = true;
                textBoxMediaAccountName.Enabled = true;
                textBoxSearchAccountKey.Enabled = true;
                textBoxSearchAccountName.Enabled = true;
                /*
                textBoxStorageAccountKey.Enabled = true;
                textBoxStorageAccountName.Enabled = true;
                textBoxStorageContainerName.Enabled = true;
                */
                textBoxTranslatorAPIKey.Enabled = true;
                textBoxAssetPrefix.Enabled = true;
                textBoxPlayerUri.Enabled = true;
                buttonLogin.Enabled = true;

                listInputAssets.Enabled = false;
                listInputFiles.Enabled = false;
                buttonAddAsset.Enabled = false;
                buttonRemoveAsset.Enabled = false;
                listOutputFiles.Enabled = false;
                listOutputAssets.Enabled = false;
                buttonGenerateSubtitle.Enabled = false;
                buttonDonwloadSubtitle.Enabled = false;
                comboBoxLanguages.Enabled = false;
                comboBoxTranslateLanguages.Enabled = false;
                buttonOpenSubtitle.Enabled = false;
                buttonDisplayJobs.Enabled = false;
                buttonPlayAudioSubtitle.Enabled = false;
                buttonPlayVideoSubtitle.Enabled = false;
                buttonCreateIndex.Enabled = false;
                buttonDeleteIndex.Enabled = false;
                buttonPopulateIndex.Enabled = false;
                buttonSearch.Enabled = false;
                listSearchResult.Enabled = false;
                buttonPlaySearch.Enabled = false;
                textBoxSearch.Enabled = false;
                buttonTranslateSubtitile.Enabled = false;
                buttonUpdateSubtitle.Enabled = false;

            }
        }
        bool IsVideoFile(string filename)
        {
            string s = filename.ToLower();
            if ((s.EndsWith(".wmv")) || (s.EndsWith(".mp4")) || (s.EndsWith(".mkv")))
                return true;
            return false;
        }
        string assetPrefix = string.Empty;
        string GetInputAssetPrefix()
        {
            return assetPrefix + "_input_";
        }
        string GetOutputAssetPrefix()
        {
            return assetPrefix + "_output_";
        }
        string GetAssetVideoMidfix()
        {
            return "video_";
        }
        string GetAssetAudioMidfix()
        {
            return "audio_";
        }
        string GetAssetSubtitleMidfix()
        {
            return "subtitle_";
        }
        string GetAssetLanguageMidfix(string language)
        {
            return language.ToLower() + "_";
        }
        bool AreAssetsLinked(string InputAsset, string OutputAsset)
        {
            if (InputAsset.StartsWith(GetInputAssetPrefix()))
            {
                if (OutputAsset.StartsWith(GetOutputAssetPrefix()))
                {
                    string a = GetAssetNameSuffix(InputAsset);
                    string b = GetAssetNameSuffix(OutputAsset);
                    if (b.Equals(a))
                        return true;
                }

            }
            return false;
        }
        string[] langArray = new[] { "enus", "eses", "zhcn", "frfr", "dede", "itit", "ptbr", "areg", "jajp" };
        string GetAssetNameSuffix(string InputAsset)
        {
            string result = string.Empty;
            if (InputAsset.StartsWith(GetInputAssetPrefix()))
                result = InputAsset.Replace(GetInputAssetPrefix(), "");
            else if (InputAsset.StartsWith(GetOutputAssetPrefix()))
                result = InputAsset.Replace(GetOutputAssetPrefix(), "");
            if (result.StartsWith(GetAssetVideoMidfix()))
                result = result.Replace(GetAssetVideoMidfix(), "");
            if (result.StartsWith(GetAssetAudioMidfix()))
                result = result.Replace(GetAssetAudioMidfix(), "");
            if (result.StartsWith(GetAssetSubtitleMidfix()))
                result = result.Replace(GetAssetSubtitleMidfix(), "");
            if (result.IndexOf("_") == 4)
            {
                string lang = result.Substring(0, 4);
                if (langArray.Contains(lang))
                    result = result.Substring(5);
            }
            return result;
        }
        string GetAssetNameLanguage(string InputAsset)
        {
            string lang = string.Empty;
            string result = string.Empty;
            if (InputAsset.StartsWith(GetInputAssetPrefix()))
                result = InputAsset.Replace(GetInputAssetPrefix(), "");
            else if (InputAsset.StartsWith(GetOutputAssetPrefix()))
                result = InputAsset.Replace(GetOutputAssetPrefix(), "");
            if (result.StartsWith(GetAssetVideoMidfix()))
                result = result.Replace(GetAssetVideoMidfix(), "");
            if (result.StartsWith(GetAssetAudioMidfix()))
                result = result.Replace(GetAssetAudioMidfix(), "");
            if (result.StartsWith(GetAssetSubtitleMidfix()))
                result = result.Replace(GetAssetSubtitleMidfix(), "");
            if (result.IndexOf("_") == 4)
            {
                lang = result.Substring(0, 2);
            }
            lang.ToLower();
            return lang;
        }
        void DeleteLocators()
        {
            foreach (var loc in _context.Locators)
            {
                try
                {
                    loc.Delete();
                }
                catch (Exception e)
                {
                    TextBoxLogWriteLine("Exception while deleting locators: " + e.Message);
                }

            }

        }
        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxMediaAccountName.Text)|| string.IsNullOrEmpty(textBoxMediaAccountKey.Text))
            {
                MessageBox.Show("The Azure Media Services Account name and key cannot be empty.");
                return;
            }
            if (string.IsNullOrEmpty(textBoxSearchAccountName.Text) || string.IsNullOrEmpty(textBoxSearchAccountKey.Text))
            {
                MessageBox.Show("The Azure Search Account name and key cannot be empty.");
                return;
            }
            if (string.IsNullOrEmpty(textBoxTranslatorAPIKey.Text))
            {
                MessageBox.Show("The Azure Cognitive Services Text Translator API key cannot be empty.");
                return;
            }
            if (string.IsNullOrEmpty(textBoxAssetPrefix.Text))
            {
                MessageBox.Show("The application prefix cannot be empty.");
                return;
            }

            bool bResult = false;
            using (new CursorHandler())
            {
                try
                {

                    _context = new Microsoft.WindowsAzure.MediaServices.Client.CloudMediaContext(textBoxMediaAccountName.Text, textBoxMediaAccountKey.Text);
                    if (_context != null)
                    {
                        _searchContext = new Microsoft.Azure.Search.SearchServiceClient(textBoxSearchAccountName.Text, new Microsoft.Azure.Search.SearchCredentials(textBoxSearchAccountKey.Text));
                        if (_searchContext != null)
                        {
                            // Get Index client
                           _indexClient = _searchContext.Indexes.GetClient("media");
                           // if (_indexClient != null)
                            {

                                if (!string.IsNullOrEmpty(textBoxTranslatorAPIKey.Text))
                                {
                                    _ttc = new TranslatorTextClient();
                                    if (_ttc != null)
                                    {
                                        string token = await _ttc.GetToken(textBoxTranslatorAPIKey.Text);
                                        if (!string.IsNullOrEmpty(token))
                                        {
                                            var list = await _ttc.GetLanguages();
                                            TranslateLanguagesIndex.Clear();
                                            List<string> listLanguage = new List<string>();
                                            foreach (var value in list)
                                            {
                                                TranslateLanguagesIndex.Add(new Item(value.Value, value.Key));
                                                string l= value.Key.ToLower();
                                                if (l.Length < 4)
                                                    l += l;
                                                l = l.Substring(0, 4);
                                                listLanguage.Add(l);
                                                
                                            }
                                            listLanguage.AddRange(langArray.ToArray());
                                            langArray = listLanguage.ToArray();
                                            comboBoxTranslateLanguages.Items.Clear();
                                            comboBoxTranslateLanguages.Items.AddRange(TranslateLanguagesIndex.ToArray());
                                            comboBoxTranslateLanguages.SelectedIndex = 0;

                                            SaveSettings();
                                            assetPrefix = textBoxAssetPrefix.Text;
                                            _context.Credentials.RefreshToken();
                                            PopulateInputAssets();
                                            //PopulateInputFiles();
                                            //PopulateOutputAssets();
                                            //PopulateOutputFiles();
                                            UpdateControls();
                                            bResult = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception: " + ex.Message);
                }
            }
            if(bResult==false)
                MessageBox.Show("The connection to Azure Media Services, Azure Search Services and Cognitive Services Text Translator failed check if your accont name and account key are still valid.");

        }
        void PopulateInputAssets(string AssetName = null, bool bSelect = true)
        {
            int IndexToSelect = 0;
            if (!string.IsNullOrEmpty(AssetName))
            {
                if (listInputAssets.Items.Count > 0)
                    IndexToSelect = listInputAssets.SelectedIndex;
            }
            listInputAssets.Items.Clear();
            int Count = 0;
            foreach (var asset in _context.Assets)
            {
                if (asset.Name.StartsWith(GetInputAssetPrefix()))
                {
                    listInputAssets.Items.Add("ASSET: " + asset.Name + " ASSET-ID " + asset.Id);
                    if (!string.IsNullOrEmpty(AssetName))
                    {
                        if (string.Equals(asset.Name, AssetName))
                            IndexToSelect = Count;
                    }
                    Count++;
                }
            }
            if (bSelect == true)
            {
                if ((listInputAssets.Items.Count > 0) && (listInputAssets.Items.Count > IndexToSelect))
                    listInputAssets.SelectedIndex = IndexToSelect;
            }
        }
        void PopulateInputFiles(string id)
        {
            listInputFiles.Items.Clear();
            foreach (var file in _context.Files)
            {
                if (id == file.Asset.Id)
                    listInputFiles.Items.Add("FILE: " + file.Name + " SIZE: " + file.ContentFileSize.ToString()+ " ASSET-ID " + file.Asset.Id );
            }
            if (listInputFiles.Items.Count > 0)
                listInputFiles.SelectedIndex = 0;

        }
        void PopulateOutputAssets(string id, bool bSelect = true)
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
                    if (asset.Name.StartsWith(GetOutputAssetPrefix()) && AreAssetsLinked(AssetName, asset.Name))
                    {
                        listOutputAssets.Items.Add("ASSET: " + asset.Name + " ASSET-ID " + asset.Id);
                    }
                }
                if (bSelect == true)
                {
                    if (listOutputAssets.Items.Count > 0)
                        listOutputAssets.SelectedIndex = 0;
                }
            }
        }
        void PopulateOutputFiles(string id)
        {
            listOutputFiles.Items.Clear();
            foreach (var file in _context.Files)
            {
                if (id == file.Asset.Id)
                    listOutputFiles.Items.Add("FILE: " + file.Name + " SIZE: " + file.ContentFileSize.ToString() + " ASSET-ID " + file.Asset.Id );
            }
            if (listOutputFiles.Items.Count > 0)
                listOutputFiles.SelectedIndex = 0;

        }
        private void listAssets_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (new CursorHandler())
            {
                PopulateInputFiles();
                PopulateOutputAssets();
                PopulateOutputFiles();
                if (listOutputAssets.Items.Count > 0) listOutputAssets.SelectedIndex = 0;
                if (listOutputFiles.Items.Count > 0) listOutputFiles.SelectedIndex = 0;
                if (listInputFiles.Items.Count > 0) listInputFiles.SelectedIndex = 0;
                UpdateControls();
            }
        }
        private void listSearchResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (new CursorHandler())
            {
                UpdateControls();
            }
        }

        void PopulateInputFiles()
        {
            string id = "";
            string s = (listInputAssets.Items.Count > 0 ? listInputAssets.SelectedItem as string : string.Empty);
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
            string s = (listOutputAssets.Items.Count > 0 ? listOutputAssets.SelectedItem as string : string.Empty);
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
            string s = (listInputAssets.Items.Count > 0 ? listInputAssets.SelectedItem as string : string.Empty);
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
        string GetAssetUniqueName(string filename)
        {
            bool bAlreadyExist = true;
            string name = filename;
            int count = 0;
            while (bAlreadyExist)
            {
                var a = _context.Assets.Where(j => j.Name == name).FirstOrDefault();
                if (a != null)
                    name = filename + "_" + count.ToString();
                else
                    bAlreadyExist = false;
                count++;
            }
            return name;
        }
        bool IsAssetNameUsed(string assetName)
        {
            var a = _context.Assets.Where(j => j.Name == assetName).FirstOrDefault();
            if (a != null)
                return true;
            return false;
        }
        private void ProcessUploadFileToAsset(string file, string filename, IAsset asset)
        {
            try
            {
                Microsoft.WindowsAzure.MediaServices.Client.IAssetFile UploadedAssetFile = asset.AssetFiles.Create(filename);
                UploadedAssetFile.IsPrimary = true;
                UploadedAssetFile.UploadProgressChanged += UploadProgress;
                UploadedAssetFile.Upload(file);
            }
            catch (Exception ex)
            {
                TextBoxLogWriteLine("Exception while uploading the Asset " + ex.Message, true);
            }
        }
        bool bUploadingAsset = false;
        bool bProcessingAsset = false;
        async Task<bool> DeleteAsset(string assetName)
        {
            bool bResult = false;
            var a = _context.Assets.Where(j => j.Name == assetName).FirstOrDefault();
            if (a != null)
                bResult = await DeleteAssetAsync(_context, a);
            if (bResult == false)
                TextBoxLogWriteLine("Error while deleting Asset " + assetName, true);
            return bResult;
        }
        private async Task<bool> UploadAsset(string localPath, string AssetFileName, string AssetName, bool Overwrite = false)
        {
            bool bResult = false;

            try
            {
                if (string.IsNullOrEmpty(storageaccount))
                    storageaccount = _context.DefaultStorageAccount.Name; // no storage account or null, then let's take the default one
                if (IsAssetNameUsed(AssetName))
                {
                    if (Overwrite == false)
                        AssetName = GetAssetUniqueName(AssetName);
                    else
                    {
                        await DeleteAsset(AssetName);
                    }
                }

                if (!string.IsNullOrEmpty(AssetName))
                {
                    TextBoxLogWriteLine("Uploading the Asset file " + AssetFileName, false);

                    System.Threading.CancellationTokenSource tokenSource = new System.Threading.CancellationTokenSource();
                    Microsoft.WindowsAzure.MediaServices.Client.IAsset asset = await _context.Assets.CreateAsync(AssetName,
                                                            storageaccount,
                                                            Microsoft.WindowsAzure.MediaServices.Client.AssetCreationOptions.None,
                                                            tokenSource.Token);
                    if (asset != null)
                    {
                        await Task.Factory.StartNew(() => ProcessUploadFileToAsset(localPath, AssetFileName, asset));
                    }
                    bResult = true;
                }

            }
            catch (Exception ex)
            {
                TextBoxLogWriteLine("Exception while uploading the Asset" + ex.Message, true);
            }
            return bResult;
        }
        private async void buttonAddAsset_Click(object sender, EventArgs e)
        {
            OpenFileDialog Dialog = new OpenFileDialog();
            Dialog.Multiselect = false;
            if (Dialog.ShowDialog() == DialogResult.OK)
            {
                using (new CursorHandler())
                {
                    string filename = null;
                    string assetfilename = null;
                    bUploadingAsset = true;
                    UpdateControls();
                    foreach (string file in Dialog.FileNames)
                    {
                        filename = System.IO.Path.GetFileName(file);
                        if (IsVideoFile(filename))
                            assetfilename = GetInputAssetPrefix() + GetAssetVideoMidfix() + filename;
                        else
                            assetfilename = GetInputAssetPrefix() + GetAssetAudioMidfix() + filename;
                        if (!AssetFileNameIsOk(assetfilename))
                        {
                            MessageBox.Show("Filename incorrect to create an asset from", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        try
                        {
                            using (new CursorHandler())
                            {
                                bool result = await UploadAsset(file, filename, assetfilename);
                                if (result == false)
                                    TextBoxLogWriteLine("Error while uploading the Asset: " + assetfilename, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            TextBoxLogWriteLine("Exception while uploading the Asset" + ex.Message, true);
                        }
                    }
                    bUploadingAsset = false;
                    // Refresh the asset.
                    PopulateInputAssets(assetfilename);
                    //PopulateInputFiles();
                    //PopulateOutputAssets();
                    //PopulateOutputFiles();
                    UpdateControls();
                }
            }
        }
        void UploadProgress(object sender, Microsoft.WindowsAzure.MediaServices.Client.UploadProgressChangedEventArgs args)
        {
            TextBoxLogWriteLine("Uploading the Asset " + args.BytesUploaded.ToString() + "/" + args.TotalBytes.ToString(), false);
            if (args.BytesUploaded == args.TotalBytes)
                TextBoxLogWriteLine("Uploading the Asset done...");
        }
        public static async Task<bool> DeleteAssetAsync(Microsoft.WindowsAzure.MediaServices.Client.CloudMediaContext mediaContext, Microsoft.WindowsAzure.MediaServices.Client.IAsset asset)
        {
            bool bResult = false;
            try
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
                foreach (var file in mediaContext.Files)
                {
                    if (asset.Id == file.Asset.Id)
                        file.Delete();
                }
                await asset.DeleteAsync();
                bResult = true;
            }
            catch (Exception)
            {
            }
            return bResult;

        }
        private async void buttonRemoveAsset_Click(object sender, EventArgs e)
        {
            string id = "";
            string s = (listInputAssets.Items.Count > 0 ? listInputAssets.SelectedItem as string : string.Empty);
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
                        try
                        {
                            foreach (var asset in _context.Assets)
                            {
                                if (asset.Name.StartsWith(GetOutputAssetPrefix()) && AreAssetsLinked(a.Name, asset.Name))
                                {
                                    if (MessageBox.Show("Do you want to delete the output Asset as well?", "Deleting Output Asset", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                                        await DeleteAssetAsync(_context, asset);
                                    break;
                                }
                            }

                            await DeleteAssetAsync(_context, a);
                        }
                        catch (Exception ex)
                        {
                            TextBoxLogWriteLine("Exception while Deleting Asset" + ex.Message, true);
                        }
                        PopulateInputAssets();
                        PopulateInputFiles();
                        PopulateOutputAssets();
                        PopulateOutputFiles();
                        UpdateControls();
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
            string s = (listInputAssets.Items.Count > 0 ? listInputAssets.SelectedItem as string : string.Empty);
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
        List<Microsoft.WindowsAzure.MediaServices.Client.IAsset> ReturnSelectedOutputAssets()
        {
            List<Microsoft.WindowsAzure.MediaServices.Client.IAsset> list = new List<Microsoft.WindowsAzure.MediaServices.Client.IAsset>();
            string id = "";
            string s = (listOutputAssets.Items.Count > 0 ? listOutputAssets.SelectedItem as string : string.Empty);
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
            bProcessingAsset = true;
            UpdateControls();
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
                            try
                            {
                                bool bStop = false;
                                while (bStop == false)
                                {
                                    var t = System.Threading.Tasks.Task.Delay(10000);
                                    t.Wait();
                                    foreach (var j in _context.Jobs)
                                    {
                                        if (j.Id == myJob.Id)
                                        {
                                            TextBoxLogWriteLine(string.Format("Job '{0}' : {1}", jobnameloc, j.State.ToString()));
                                            if ((j.State == JobState.Canceled) ||
                                                (j.State == JobState.Error) ||
                                                (j.State == JobState.Finished))
                                            {
                                                bStop = true;
                                                bProcessingAsset = false;
                                                this.Invoke((MethodInvoker)delegate
                                                {

                                                    PopulateOutputAssets();
                                                    PopulateOutputFiles();
                                                    UpdateControls();
                                                });
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {

                            }
                            this.Invoke((MethodInvoker)delegate
                            {
                                UpdateControls();
                                PopulateOutputAssets();
                                PopulateOutputFiles();
                            });
                            return;
                        }
                        );
                    }
                }

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
            if (SelectedAssets.FirstOrDefault() == null)
            {
                MessageBox.Show("No files selected");
                return;
            }


            if (CheckPrimaryFileExtension(SelectedAssets, new[] { ".MP4", ".WMV", ".MP3", ".M4A", ".WMA", ".AAC", ".WAV" }) == false)
            {
                MessageBox.Show("The file selected seems not a video (MP4, WMV) nor audio (MP3, M4A, WMA, AAC, WAV) file ");
                return;
            }

            // Get the SDK extension method to  get a reference to the Azure Media Indexer.
            Microsoft.WindowsAzure.MediaServices.Client.IMediaProcessor processor = GetLatestMediaProcessorByName(AzureMediaIndexer2Preview);

            {
                var ListConfig = new List<string>();
                string language = "EnUs";
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
                    Item item = (comboBoxLanguages.Items.Count > 0 ? comboBoxLanguages.SelectedItem as Item : null);
                    if (item != null)
                        language = item.Value;
                    if (string.IsNullOrEmpty(language))
                        language = "EnUs";
                    ListConfig.Add(JsonConfig(language, false, true, false));
                }
                string IndexerInputAssetName = SelectedAssets.FirstOrDefault().Name;
                string IndexerJobName = "Media Indexing v2 of " + IndexerInputAssetName;
                string IndexerOutputAssetName = GetOutputAssetPrefix() + GetAssetSubtitleMidfix() + GetAssetLanguageMidfix(language) + GetAssetNameSuffix(IndexerInputAssetName);
                string taskname = "Media Indexing v2 of " + IndexerInputAssetName;
                LaunchJobs_OneJobPerInputAssetWithSpecificConfig(
                            processor,
                            SelectedAssets,
                            IndexerJobName,
                            //Priority
                            10,
                            taskname,
                            IndexerOutputAssetName,
                            ListConfig,
                            Microsoft.WindowsAzure.MediaServices.Client.AssetCreationOptions.None,
                            Microsoft.WindowsAzure.MediaServices.Client.TaskOptions.None,
                            _context.DefaultStorageAccount.Name,
                            false
                           );
            }
        }
        private List<IAssetFile> ReturnSelectedSubtitleAssetFiles()
        {
            List<IAssetFile> Selection = new List<IAssetFile>();
            string assetID = string.Empty;
            string fileName = string.Empty;

            string s = (listOutputFiles.Items.Count > 0 ? listOutputFiles.SelectedItem as string : string.Empty);
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf("FILE: ");
                if (pos >= 0)
                {
                    int endsize = s.IndexOf(" SIZE: ");
                    if (endsize > 0)
                    {
                        int end = s.IndexOf(" ASSET-ID ");
                        if (end > 0)
                        {
                            fileName = s.Substring(pos + 6, endsize - pos - 6);
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
            }
            return Selection;
        }
        private List<IAssetFile> ReturnSelectedMediaAssetFiles()
        {
            List<IAssetFile> Selection = new List<IAssetFile>();
            string assetID = string.Empty;
            string fileName = string.Empty;

            string s = (listInputFiles.Items.Count > 0 ? listInputFiles.SelectedItem as string : string.Empty);
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf("FILE: ");
                if (pos >= 0)
                {
                    int endsize = s.IndexOf(" SIZE: ");
                    if (endsize > 0)
                    {
                        int end = s.IndexOf(" ASSET-ID ");
                        if (end > 0)
                        {
                            fileName = s.Substring(pos + 6, endsize - pos - 6);
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
        private void buttonDownloadSubtitle_Click(object sender, EventArgs e)
        {
            var SelectedAssetFiles = ReturnSelectedSubtitleAssetFiles();
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
                    TextBoxLogWriteLine("Downloading from SASLocator: " + File.GetSasUri(sasLocator).ToString());
                    await File.DownloadAsync(System.IO.Path.Combine(folder as string, File.Name), blobTransferClient, sasLocator, response.token);
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
                    if (!response.token.IsCancellationRequested)
                    {
                        TextBoxLogWriteLine(string.Format("Download of file '{0}' sucessful", File.Name), false);
                    }
                    else
                    {
                        TextBoxLogWriteLine(string.Format("Download of file '{0}' cancelled", File.Name), true);
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
        public string GetAssetFileContent(Microsoft.WindowsAzure.MediaServices.Client.IAsset asset,
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
            string localfile = GetTempFilePathWithExtension(".vtt");

            //Microsoft.WindowsAzure.MediaServices.Client.ILocator sasLocator = null;
            //var locatorTask = Task.Factory.StartNew(() =>
            //{
            //    sasLocator = _context.Locators.Create(Microsoft.WindowsAzure.MediaServices.Client.LocatorType.Sas, asset, Microsoft.WindowsAzure.MediaServices.Client.AccessPermissions.Read, TimeSpan.FromHours(24));
            //});
            //locatorTask.Wait();
            // File.Download(localfile);
            bool Error = false;
            //var myTask = Task.Factory.StartNew(() =>
            //{
            //    Error = false;
            try
            {
                File.Download(localfile);
            }
            catch (Exception e)
            {
                Error = true;
                TextBoxLogWriteLine(string.Format("Download of file '{0}' failed !", File.Name), true);
                TextBoxLogWriteLine(e);
            }
            //});
            //myTask.Wait();
            //if(sasLocator!= null)
            //    sasLocator.Delete();

            if (Error == false)
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(localfile))
                {
                    // Read the stream to a string, and write the string to the console.
                    content = sr.ReadToEnd();
                }
            }
            return content;

        }

        private async void buttonRemoveOutputAsset_Click(object sender, EventArgs e)
        {
            string id = "";
            string s = (listOutputAssets.Items.Count > 0 ? listOutputAssets.SelectedItem as string : string.Empty);
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
        //        public ILocator tempLocator = null;
        //        public IAsset tempAsset = null;
        private ILocator GetTemporaryOnDemandLocator(IAsset asset, int Duration = 24)
        {
            ILocator tempLocator = null;
            IAsset tempAsset = null;
            TextBoxLogWriteLine(string.Format("Get OnDemand Locator for asset Id: '{0}' ", asset.Id), true);
            foreach (var loc in _context.Locators)
            {
                if (loc.Type == LocatorType.OnDemandOrigin)
                {
                    if (loc.AssetId == asset.Id)
                    {
                        if (loc.ExpirationDateTime > DateTime.Now)
                            return loc;
                        else
                        {
                            tempLocator = loc;
                            tempAsset = loc.Asset;
                            break;
                        }
                    }
                }
            }

            if ((tempAsset != null) && (tempLocator != null))
            {
                try
                {
                    var locatorTask = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            tempLocator.Delete();
                        }
                        catch (Exception ex)
                        {
                            TextBoxLogWriteLine(string.Format("Exception when deleting an OnDemand Locator for asset Id: '{0}'  Exception: {1} ", asset.Id, ex.Message), true);
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
                var locatorTask = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        tempLocator = _context.Locators.Create(LocatorType.OnDemandOrigin, asset, AccessPermissions.Read, TimeSpan.FromHours(Duration));
                        tempAsset = asset;
                    }
                    catch (Exception ex)
                    {
                        TextBoxLogWriteLine(string.Format("Exception when creating an OnDemand Locator for asset Id: '{0}' Exception: {1} ", asset.Id, ex.Message), true);
                        tempAsset = null;
                    }
                });
                locatorTask.Wait();
            }
            return tempLocator;
        }
        private ILocator GetTemporarySASLocator(IAsset asset, int DurationinHours = 24)
        {
            ILocator tempLocator = null;
            IAsset tempAsset = null;
            TextBoxLogWriteLine(string.Format("Get SAS Locator for asset Id: '{0}' ", asset.Id), false);
            foreach (var loc in _context.Locators)
            {
                if (loc.Type == LocatorType.Sas)
                {
                    if (loc.AssetId == asset.Id)
                    {
                        if (loc.ExpirationDateTime > DateTime.Now + TimeSpan.FromHours(DurationinHours))
                            return loc;
                        else
                        {
                            tempLocator = loc;
                            tempAsset = loc.Asset;
                            break;
                        }
                    }
                }
            }

            if ((tempAsset != null) && (tempLocator != null))
            {
                try
                {
                    var locatorTask = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            tempLocator.Delete();
                        }
                        catch (Exception ex)
                        {
                            TextBoxLogWriteLine(string.Format("Exception when deleting a SAS Locator for asset Id: '{0}'  Exception: {1} ", asset.Id, ex.Message), true);
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
                var locatorTask = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        tempLocator = _context.Locators.Create(LocatorType.Sas, asset, AccessPermissions.Read, TimeSpan.FromHours(DurationinHours));
                        tempAsset = asset;
                    }
                    catch (Exception ex)
                    {
                        TextBoxLogWriteLine(string.Format("Exception when creating a SAS Locator for asset Id: '{0}' Exception: {1} ", asset.Id, ex.Message), true);
                        tempAsset = null;
                    }
                });
                locatorTask.Wait();
            }
            return tempLocator;
        }
        public ILocator tempInputLocator = null;
        public IAsset tempInputAsset = null;
        private ILocator GetTemporaryInputLocator(IAsset asset, int Duration = 24)
        {
            foreach (var loc in _context.Locators)
            {
                if (loc.AssetId == asset.Id)
                {
                    if (loc.ExpirationDateTime > DateTime.Now)
                        return loc;
                    else
                    {
                        tempInputLocator = loc;
                        tempInputAsset = loc.Asset;
                        break;
                    }
                }
            }
            if ((tempInputAsset != null) && (tempInputLocator != null))
            {
                try
                {
                    var locatorTask = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            tempInputLocator.Delete();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception when creating the temporary SAS locator." + ex.Message);
                        }
                    });
                    locatorTask.Wait();
                    locatorTask.Wait(1000);
                    tempInputLocator = null;
                }
                catch
                {

                }
            }
            if (tempInputLocator == null) // no temp locator, let's create it
            {
                var locatorTask = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        tempInputLocator = _context.Locators.Create(LocatorType.OnDemandOrigin, asset, AccessPermissions.Read, TimeSpan.FromHours(Duration));
                        tempInputAsset = asset;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error when creating the temporary OnDemandOrigin locator." + ex.Message);
                        tempInputAsset = null;
                    }
                });
                locatorTask.Wait();
            }
            return tempInputLocator;
        }
        private void buttonOpenSubtitle_Click(object sender, EventArgs e)
        {
            using (new CursorHandler())
            {
                var SelectedAssetFiles = ReturnSelectedSubtitleAssetFiles();
                if (SelectedAssetFiles.Count > 0)
                {
                    var af = SelectedAssetFiles.FirstOrDefault();
                    ILocator locator = GetTemporarySASLocator(af.Asset);
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
        }

        private void buttonDisplayJobs_Click(object sender, EventArgs e)
        {
            PopulateJobList();
        }
        public bool SearchIndexExists()
        {
            bool result = false;
            if (_searchContext != null)
            {
                try
                {
                    var response =  _searchContext.Indexes.ExistsWithHttpMessagesAsync("media");
                    if (response != null)
                    {
                        response.Wait();
                        if ((response.Result != null) && (response.Result.Response != null) && (response.Result.Response.StatusCode == System.Net.HttpStatusCode.OK))
                        {
                            result = true;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            return result;
        }
        public async Task<bool> DeleteSearchIndex()
        {
            bool result = false;
            if (_searchContext != null)
            {
                try
                {
                    var response = await _searchContext.Indexes.ExistsWithHttpMessagesAsync("media");
                    if ((response != null) && (response.Response.StatusCode == System.Net.HttpStatusCode.OK))
                    {
                        await _searchContext.Indexes.DeleteWithHttpMessagesAsync("media");
                            _indexClient = null;
                            result = true;
                    }
                    else
                    {
                        TextBoxLogWriteLine("Deleting Azure Search Index: media index didn't exist ");
                        _indexClient = null;
                    }
                }
                catch (Exception ex)
                {
                    TextBoxLogWriteLine("Exception while deleting Azure Search Index: " + ex.Message + " " + ex.InnerException.Message, true);
                }
            }
            return result;
        }
        public bool CreateSearchIndex()
        {

            bool result = false;
            if (_searchContext != null)
            {
                var definition = new Microsoft.Azure.Search.Models.Index()
                {
                    Name = "media",
                    Fields = new[]
                    {
                        new Microsoft.Azure.Search.Models.Field("keyId", Microsoft.Azure.Search.Models.DataType.String)                       {   IsKey = true  },
                        new Microsoft.Azure.Search.Models.Field("mediaId", Microsoft.Azure.Search.Models.DataType.String)                       {   IsFilterable = true,  IsSortable = true  },
                        new Microsoft.Azure.Search.Models.Field("mediaName", Microsoft.Azure.Search.Models.DataType.String)                     { IsFilterable = true,  IsSortable = true },
                        new Microsoft.Azure.Search.Models.Field("mediaUrl", Microsoft.Azure.Search.Models.DataType.String)                       { IsFilterable = true},
                        new Microsoft.Azure.Search.Models.Field("isAudio", Microsoft.Azure.Search.Models.DataType.Boolean)                       {IsFilterable = true},
                        new Microsoft.Azure.Search.Models.Field("subtitleUrl", Microsoft.Azure.Search.Models.DataType.String)                       { IsFilterable = true},
                        new Microsoft.Azure.Search.Models.Field("subtitleLanguage", Microsoft.Azure.Search.Models.DataType.String)                   { IsFilterable = true},
                        new Microsoft.Azure.Search.Models.Field("subtitleStartTime", Microsoft.Azure.Search.Models.DataType.String)                   { IsSortable = true},
                        new Microsoft.Azure.Search.Models.Field("subtitleEndTime", Microsoft.Azure.Search.Models.DataType.String)                   { IsSortable = true},
                        new Microsoft.Azure.Search.Models.Field("subtitleContent", Microsoft.Azure.Search.Models.DataType.String)                   { IsSearchable = true}
                    }
                };

                try
                {


                        var response = _searchContext.Indexes.CreateWithHttpMessagesAsync(definition);
                        if (response != null)
                        {
                            response.Wait();
                            if ((response.Result != null) && (response.Result.Response != null))
                            {
                                if (response.Result.Response.StatusCode == System.Net.HttpStatusCode.Created)
                                {
                                    _indexClient = _searchContext.Indexes.GetClient("media");
                                    result = true;
                                }
                            }
                        }


                }
                catch(Exception ex)
                {
                    TextBoxLogWriteLine("Exception while creating Azure Search Index: " + ex.Message + " " + ex.InnerException.Message,true);
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
                      keyId = "1111_fr_00_00_00",
                      mediaId = "1111",
                      mediaName = "Name1",
                      mediaUrl = "http://media",
                      isAudio = true,
                      subtitleLanguage = "fr",
                      subtitleStartTime ="00:00:00",
                      subtitleEndTime = "00:00:00",
                      subtitleUrl = "http://subtitle",
                      subtitleContent = "ti too  a a a toot",
                    },
                    new Media()
                    {
                      keyId = "1112_fr_00_00_00",
                      mediaId = "1112",
                      mediaName = "Name2",
                      mediaUrl = "http://medi2",
                      isAudio = false,
                      subtitleLanguage = "fr",
                      subtitleStartTime ="00:00:00",
                      subtitleEndTime = "00:00:00",
                      subtitleUrl = "http://subtitle",
                      subtitleContent = "ti too  a a a toot",
                    }
                };
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
                catch (Exception ex)
                {
                    TextBoxLogWriteLine("Exception while uploading Azure Search Index: " + ex.Message, true);
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
            if ((_context != null) && (_searchContext != null))
            {
                List<Media> documents = new List<Media>();
                foreach (var asset in _context.Assets)
                {
                    if (asset.Name.StartsWith(GetInputAssetPrefix()))
                    {
                        foreach (var a in _context.Assets)
                        {
                            if (a.Name.StartsWith(asset.Name) && a.Name.StartsWith(GetOutputAssetPrefix()))
                            {
                                bool bAudio = IsAudioInputAssetSelected();
                                foreach (var file in _context.Files)
                                {
                                    if ((a.Id == file.Asset.Id) && (file.Name.EndsWith(".vtt", StringComparison.OrdinalIgnoreCase)))
                                    {
                                        
                                        string content = GetAssetFileContent(a, file);
                                        if (!string.IsNullOrEmpty(content))
                                        {
                                            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(asset.Id);
                                            Media media = new Media()
                                            {
                                                mediaId = System.Convert.ToBase64String(toEncodeAsBytes),
                                                mediaName = asset.Name,
                                                mediaUrl = asset.Id,
                                                isAudio = bAudio,
                                                subtitleLanguage = "fr",
                                                subtitleStartTime = "00:00:00",
                                                subtitleEndTime = "00:00:00",
                                                subtitleUrl = "http://subtitle",
                                                subtitleContent = "ti too  a a a toot"

                                            };
                                            documents.Add(media);
                                        }
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
                catch (Exception ex)
                {
                    TextBoxLogWriteLine("Exception while uploading Azure Search Index: " + ex.Message, true);
                }
                // Wait a while for indexing to complete.
                System.Threading.Tasks.Task.Delay(2000);
                result = true;
            }
            return result;
        }

        private void buttonCreateIndex_Click(object sender, EventArgs e)
        {
            TextBoxLogWriteLine("Creating Azure Search Index...");
            bool res = CreateSearchIndex();
            if (res == true)
                TextBoxLogWriteLine("Azure Search Index Created");
            else
                TextBoxLogWriteLine("Azure Search Index not Created");

            UpdateControls();
        }

        private async void buttonDeleteIndex_Click(object sender, EventArgs e)
        {
            TextBoxLogWriteLine("Deleting Azure Search Index...");
            bool res = await DeleteSearchIndex();
            if (res == true)
                TextBoxLogWriteLine("Azure Search Index Deleted");
            else
                TextBoxLogWriteLine("Azure Search Index not Deleted");
            UpdateControls();
        }

        private async void buttonPopulateIndex_Click(object sender, EventArgs e)
        {
            using (new CursorHandler())
            {
                try
                { 
                    var MediaAssetFiles = ReturnSelectedMediaAssetFiles();
                    var SubtitleAssetFiles = ReturnSelectedSubtitleAssetFiles();
                    if ((SubtitleAssetFiles.Count > 0) && (MediaAssetFiles.Count > 0))
                    {
                        var mediaAssetFile = MediaAssetFiles.FirstOrDefault();
                        var subtitleAssetFile = SubtitleAssetFiles.FirstOrDefault();
                        ILocator subtitleLocator = GetTemporarySASLocator(subtitleAssetFile.Asset, 10000);
                        ILocator mediaLocator = GetTemporarySASLocator(mediaAssetFile.Asset, 10000);
                        string mediaUri = mediaAssetFile.GetSasUri(mediaLocator).ToString();
                        string subtitleUri = subtitleAssetFile.GetSasUri(subtitleLocator).ToString();
                        string mediaAssetName = mediaAssetFile.Asset.Name;
                        string mediaAssetID = mediaAssetFile.Asset.Id.Replace(':', '-');
                        string language = GetSelectedOutputAssetLanguage();
                        TextBoxLogWriteLine("Populating the Index for asset: " + mediaAssetName);

                        if (!string.IsNullOrEmpty(mediaAssetName) &&
                            !string.IsNullOrEmpty(language) &&
                            !string.IsNullOrEmpty(subtitleUri) &&
                            !string.IsNullOrEmpty(mediaUri) &&
                            !string.IsNullOrEmpty(language))
                        {
                            string content = await GetContent(subtitleUri);
                            if (!string.IsNullOrEmpty(content))
                            {
                                bool bAudio = IsAudioInputAssetSelected();
                                List<SubtitileItem> SubtitleList = ParseWEBVTT(content);
                                if (SubtitleList.Count > 0)
                                {
                                    List<Media> documents = new List<Media>();
                                    foreach (SubtitileItem item in SubtitleList)
                                    {
                                        Media media = new Media()
                                        {
                                            keyId = mediaAssetID + "_" + language + "_" + item.startTime.Replace(':', '-').Replace('.','-'),
                                            mediaId = mediaAssetID,
                                            mediaName = mediaAssetName,
                                            mediaUrl = mediaUri,
                                            isAudio = bAudio,
                                            subtitleLanguage = language,
                                            subtitleStartTime = item.startTime,
                                            subtitleEndTime = item.endTime,
                                            subtitleUrl = subtitleUri,
                                            subtitleContent = item.subtitle

                                        };
                                        documents.Add(media);
                                    }
                                    var batch = Microsoft.Azure.Search.Models.IndexBatch.Upload(documents);
                                    if (_indexClient != null)
                                        _indexClient.Documents.Index(batch);
                                }
                            }
                        }
                        TextBoxLogWriteLine("Populating the Index done for asset: " + mediaAssetName);
                    }
                }
                catch(Exception ex)
                {
                    TextBoxLogWriteLine("Exception while populating the Index: " + ex.Message);
                }
                UpdateControls();
            }
        }
        private bool Search(string text)
        {
            bool result = false;
            if (_indexClient != null)
            {
                DocumentSearchResult<Media> response = null;
                listSearchResult.Items.Clear();
                SearchParameters parameters = new SearchParameters
                {
                    Top = 3
                };      

                try
                {
                    if (string.IsNullOrEmpty(text))
                        text = "*";
                    response = _indexClient.Documents.Search<Media>(text, parameters);
                }
                catch (Exception e)
                {
                    TextBoxLogWriteLine("Exception while searching media found for : " + textBoxSearch.Text + " exception: " + e.Message);
                }
                if ((response != null) && (response.Results.Count > 0))
                {
                    TextBoxLogWriteLine("Search result for : " + textBoxSearch.Text);
                    foreach (SearchResult<Media> res in response.Results)
                    {
                        TextBoxLogWriteLine(res.Document.ToString());
                        int seconds = GetWEBVTTTimeInSeconds(res.Document.subtitleStartTime);
                        string uri = string.Empty;
                        string subtitleEncoded = UriEncode(res.Document.subtitleUrl);
                        string mediaEncoded = UriEncode(res.Document.mediaUrl);
                        string lang = res.Document.subtitleLanguage.Substring(0, 2);

                        if (res.Document.isAudio == true)
                            uri = textBoxPlayerUri.Text + "?audiourl=" + mediaEncoded + "&subtitles=" + lang + "," + lang + "," + subtitleEncoded + "&time=" + seconds.ToString();
                        else
                            uri = textBoxPlayerUri.Text + "?url=" + mediaEncoded + "&subtitles=" + lang + "," + lang + "," + subtitleEncoded + "&time=" + seconds.ToString();

                        TextBoxLogWriteLine("Player Uri: " + uri);
                        string item = ((res.Document.isAudio == true) ? "AUDIO " : "VIDEO ") + res.Document.mediaName + "TIME: " + res.Document.subtitleStartTime + " CONTENT: '" + res.Document.subtitleContent + "' URL: " + uri;

                        listSearchResult.Items.Add(item);
                    }
                    if (listSearchResult.Items.Count > 0)
                        listSearchResult.SelectedIndex = 0;
                    UpdateControls();
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
        public static string UriEncode(string plainText)
        {
            return Uri.EscapeDataString(plainText);
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        /*
        private string GetDuplicateUri(IAssetFile SelectedAssetFile)
        {
            string res = string.Empty;
            if (SelectedAssetFile != null)
            {
                try
                {
                    Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount;
                    storageAccount = new Microsoft.WindowsAzure.Storage.CloudStorageAccount(new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(textBoxStorageAccountName.Text, textBoxStorageAccountKey.Text), true);
                    var cloudBlobClient = storageAccount.CreateCloudBlobClient();
                    string containerName = "asset-" + SelectedAssetFile.Asset.Id.Replace("nb:cid:UUID:", "");
                    Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer sourceContainer = cloudBlobClient.GetContainerReference(containerName);
                    Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer targetContainer = cloudBlobClient.GetContainerReference(textBoxStorageContainerName.Text);
                    string blobName = SelectedAssetFile.Name;
                    Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob sourceBlob = sourceContainer.GetBlockBlobReference(blobName);
                    Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob targetBlob = targetContainer.GetBlockBlobReference(blobName);
                    targetBlob.DeleteIfExists();
                    targetBlob.StartCopyFromBlob(sourceBlob);

                    while (targetBlob.CopyState.Status == Microsoft.WindowsAzure.Storage.Blob.CopyStatus.Pending)
                    {
                        Task.Delay(TimeSpan.FromSeconds(1d)).Wait();
                        targetBlob.FetchAttributes();
                    }
                    res = targetBlob.SnapshotQualifiedUri.ToString().Replace("https://", "http://");
                }
                catch
                {
                    MessageBox.Show("Error when duplicating this file");
                }
            }
            return res;
        }
        */
        private void PlaySubtitle(bool bAudio)
        {
            using (new CursorHandler())
            {
                var SelectedAssetFiles = ReturnSelectedSubtitleAssetFiles();
                var SelectedInputAssetFiles = ReturnSelectedMediaAssetFiles();
                if ((SelectedAssetFiles.Count > 0) && (SelectedInputAssetFiles.Count > 0))
                {
                    var subtitleAssetFile = SelectedAssetFiles.FirstOrDefault();
                    var mediaAssetFile = SelectedInputAssetFiles.FirstOrDefault();
                    ILocator subtitleLocator = GetTemporarySASLocator(subtitleAssetFile.Asset);
                    ILocator mediaLocator = GetTemporarySASLocator(mediaAssetFile.Asset);
                    //ILocator locator = GetTemporaryOnDemandLocator(subtitleAssetFile.Asset);
                    //ILocator Inputlocator = GetTemporaryOnDemandLocator(mediaAssetFile.Asset);
                    if ((subtitleLocator != null) && (mediaLocator != null))
                    {
                        try
                        {
                            foreach (var assetfile in SelectedAssetFiles)
                            {
                                //Duplicate the subtitle file in a container
                                //string subtitleUri = GetDuplicateUri(subtitleAssetFile);
                                //string mediaUri = GetDuplicateUri(mediaAssetFile);
                                string subtitleUri = subtitleAssetFile.GetSasUri(subtitleLocator).ToString();
                                string mediaUri = mediaAssetFile.GetSasUri(mediaLocator).ToString();

                                TextBoxLogWriteLine("Url for Subtitle file: " + subtitleUri);
                                string subtitleEncoded = UriEncode(subtitleUri);
                                TextBoxLogWriteLine("Url for Media file: " + mediaUri);
                                string mediaEncoded = UriEncode(mediaUri);
                                string uri = string.Empty;
                                string lang = GetAssetNameLanguage(subtitleAssetFile.Asset.Name);
                                if (string.IsNullOrEmpty(lang))
                                    lang = "en";
                                if (bAudio == true)
                                    uri = textBoxPlayerUri.Text + "?audiourl=" + mediaEncoded + "&subtitles=" + lang + "," + lang + "," + subtitleEncoded;
                                else
                                    uri = textBoxPlayerUri.Text + "?url=" + mediaEncoded + "&subtitles=" + lang + "," + lang + "," + subtitleEncoded;
                                TextBoxLogWriteLine("Url for player: " + uri);
                                System.Diagnostics.Process.Start(uri);
                            }
                        }
                        catch (Exception ex)
                        {
                            TextBoxLogWriteLine("Error when accessing temporary SAS locator: " + ex.Message);
                        }
                    }
                }
            }

        }
        private void buttonPlayAudioSubtitle_Click(object sender, EventArgs e)
        {
            PlaySubtitle(true);
        }

        private void buttonPlayVideoSubtitle_Click(object sender, EventArgs e)
        {
            PlaySubtitle(false);
        }

        private async void buttonUpdateSubtitle_Click(object sender, EventArgs e)
        {
            using (new CursorHandler())
            {
                string inputAssetName = GetSelectedInputAssetName();
                string outputAssetName = GetSelectedOutputAssetName();
                string outputLanguage = GetSelectedOutputAssetLanguage();

                if (!string.IsNullOrEmpty(inputAssetName) && !string.IsNullOrEmpty(outputLanguage))
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Multiselect = false;
                    openFileDialog.Filter = "WEBVTT Files (VTT)|*.VTT";
                    openFileDialog.Title = "Select the WEBVTT file to updload";
                    if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        if (MessageBox.Show("You are about to update subtitles associated with asset: " + outputAssetName + " and language '" + outputLanguage + "' with the content in file: " + openFileDialog.FileName, "Cognitive Services Text Translator", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                        {
                            return;
                        }

                        bUploadingAsset = true;
                        try
                        {
                            string localPath = openFileDialog.FileName;
                            string assetFileName = System.IO.Path.GetFileName(localPath);

                            UpdateControls();

                            if (!AssetFileNameIsOk(outputAssetName))
                            {
                                TextBoxLogWriteLine("Asset Name incorrect to update an asset :" + outputAssetName, true);
                            }

                            using (new CursorHandler())
                            {
                                bool result = await UploadAsset(localPath, assetFileName, outputAssetName, true);
                                if (result == false)
                                    TextBoxLogWriteLine("Error while uploading the Asset: " + outputAssetName, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            TextBoxLogWriteLine("Exception while uploading the Asset" + ex.Message, true);
                        }
                        bUploadingAsset = false;
                        PopulateOutputAssets();
                        PopulateOutputFiles();
                        UpdateControls();
                    }
                }
            }
        }

        string GetSelectedOutputAssetName()
        {
            string result = string.Empty;
            var SelectedAssets = ReturnSelectedOutputAssets();
            if (SelectedAssets.Count > 0)
            {
                var a = SelectedAssets.FirstOrDefault();
                if (a != null)
                    result = a.Name;
            }
            return result;
        }
        string GetSelectedInputAssetName()
        {
            string result = string.Empty;
            var SelectedAssets = ReturnSelectedAssets();
            if (SelectedAssets.Count > 0)
            {
                var a = SelectedAssets.FirstOrDefault();
                if (a != null)
                    result = a.Name;
            }
            return result;
        }
        string GetSelectedOutputAssetSASUri(int duration = 24)
        {
            string result = string.Empty;
            var SelectedAssetFiles = ReturnSelectedSubtitleAssetFiles();
            if (SelectedAssetFiles.Count > 0)
            {
                var af = SelectedAssetFiles.FirstOrDefault();
                ILocator locator = GetTemporarySASLocator(af.Asset,duration);
                if (locator != null)
                {
                    try
                    {
                        foreach (var assetfile in SelectedAssetFiles)
                        {

                            result = assetfile.GetSasUri(locator).ToString();
                            break;
                        }
                    }
                    catch
                    {
                        TextBoxLogWriteLine("Error when accessing temporary SAS locator");
                    }
                }
            }
            return result;
        }
        string GetSelectedOutputAssetLanguage()
        {
            string result = string.Empty;
            var SelectedAssetFiles = ReturnSelectedSubtitleAssetFiles();
            if (SelectedAssetFiles.Count > 0)
            {
                var af = SelectedAssetFiles.FirstOrDefault();
                result = GetAssetNameLanguage(af.Asset.Name);
            }
            return result;
        }
        public async Task<string> GetContent(string uri)
        {
            string result = string.Empty;
            try
            {
                System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
                System.Net.Http.HttpResponseMessage hrm = null;
                hrm = await hc.GetAsync(new Uri(uri));
                if (hrm != null)
                {
                    switch (hrm.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            var b = await hrm.Content.ReadAsByteArrayAsync();
                            result = System.Text.UTF8Encoding.UTF8.GetString(b);
                            break;
                        default:
                            int code = (int)hrm.StatusCode;
                            string HttpError = "Http Response Error: " + code.ToString() + " reason: " + hrm.ReasonPhrase.ToString();
                            TextBoxLogWriteLine(HttpError);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                TextBoxLogWriteLine("http GET exception: " + ex.Message);
            }
            finally
            {
                //TextBoxLogWriteLine("http GET done");
            }
            return result;
        }
        string ParseWEBVTTTime(string content)
        {
            string result = string.Empty;
            result = content.Trim();

            return result;
        }
        int GetWEBVTTTimeInSeconds(string content)
        {
            int result = 0;
            content = content.Trim();
            try
            {
                TimeSpan t = TimeSpan.Parse(content);
                result = (int)t.TotalSeconds;
            }
            catch (Exception)
            {
            }
            return result;
        }
        List<SubtitileItem> ParseWEBVTT(string content)
        {
            List<SubtitileItem> result = new List<SubtitileItem>();

            // Remove the first WCHAR FEFF from the string
            int n = 0;
            while ((n < content.Length) && (content[n] != 'W')) n++;
            content = content.Substring(n);

            if (!string.IsNullOrEmpty(content) && (result != null))
            {
                string separator = "\r\n\r\n";
                string separator1 = "\r\n";
                string separator2 = "-->";
                string[] tabLineSeparator = { separator };
                string[] tabItemSeparator = { separator1, separator2 };
                string[] lines = content.Split(tabLineSeparator, StringSplitOptions.RemoveEmptyEntries);
                if (lines.Count() > 0)
                {
                    if (lines[0] == "WEBVTT")
                    {
                        for (int i = 1; i < lines.Count(); i++)
                        {
                            string[] items = lines[i].Split(tabItemSeparator, StringSplitOptions.RemoveEmptyEntries);
                            if (items.Count() == 3)
                            {
                                string startTime = ParseWEBVTTTime(items[0]);
                                string endTime = ParseWEBVTTTime(items[1]);
                                string subtitle = items[2].Trim();
                                if (!string.IsNullOrEmpty(startTime) &&
                                    !string.IsNullOrEmpty(endTime) &&
                                    !string.IsNullOrEmpty(subtitle))
                                {
                                    SubtitileItem item = new SubtitileItem(startTime, endTime, subtitle);
                                    if (item != null)
                                    {
                                        result.Add(item);
                                    }
                                }
                            }
                        }
                    }
                }
            };
            return result;
        }
        async Task<string> GetTranslatedWEBVTT(string uri, string inputLanguage, string outputLanguage)
        {
            string translatedContent = string.Empty;
            if (!string.IsNullOrEmpty(uri) && !string.IsNullOrEmpty(inputLanguage) && !string.IsNullOrEmpty(outputLanguage))
            {
                string content = await GetContent(uri);
                if (!string.IsNullOrEmpty(content))
                {
                    TextBoxLogWriteLine("Original Subtitles downloaded");
                    List<SubtitileItem> SubtitleList = ParseWEBVTT(content);
                    if (SubtitleList.Count > 0)
                    { 
                        TextBoxLogWriteLine("Original Subtitles parsed");
                        translatedContent += "\xFEFF";
                        translatedContent += "WEBVTT\r\n";
                        SubtitileItem newItem = new SubtitileItem("", "", "");
                        bool bError = false;
                        foreach (SubtitileItem item in SubtitleList)
                        {
                            newItem.startTime = item.startTime;
                            newItem.endTime = item.endTime;
                            if (!string.IsNullOrEmpty(item.subtitle))
                            {
                                newItem.subtitle = await _ttc.Translate(item.subtitle, inputLanguage, outputLanguage);
                                if (!string.IsNullOrEmpty(newItem.subtitle))
                                {
                                    translatedContent += newItem.ToString();
                                }
                                else
                                {
                                    bError = true;
                                    break;
                                }
                            }
                        }
                        if (bError == true)
                            TextBoxLogWriteLine("Error while translating subtitles at " + newItem.startTime);
                        else
                            TextBoxLogWriteLine("Translating subtitles done:" + translatedContent);
                    }
                }
                else
                    TextBoxLogWriteLine("Error while downloading subtitles: subtitle string empty");
            }
            return translatedContent;
        }
        private async void buttonTranslateSubtitile_Click(object sender, EventArgs e)
        {
            using (new CursorHandler())
            {
                string inputAssetName = GetSelectedInputAssetName();
                string uri = GetSelectedOutputAssetSASUri();
                string inputLanguage = GetSelectedOutputAssetLanguage();
                string outputLanguage = "enen";
                Item comboitem = (comboBoxLanguages.Items.Count > 0 ? comboBoxTranslateLanguages.SelectedItem as Item : null);
                if (comboitem != null)
                    outputLanguage = comboitem.Value;


                if (!string.IsNullOrEmpty(uri) && !string.IsNullOrEmpty(inputLanguage) && !string.IsNullOrEmpty(outputLanguage))
                {

                    if (MessageBox.Show( "You are about to translate subtitles associated with asset: " + inputAssetName + " from language '" + inputLanguage + "' to language '" + outputLanguage + "'", "Cognitive Services Text Translator", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    {
                        return;
                    }
                    string content = await GetTranslatedWEBVTT(uri, inputLanguage, outputLanguage);
                    if (!string.IsNullOrEmpty(content))
                    {
                        bUploadingAsset = true;
                        try
                        {
                            string assetFileName = "webvtt_" + outputLanguage + ".vtt";
                            string localPath = System.IO.Path.GetTempPath() + assetFileName;
                            System.IO.File.WriteAllBytes(localPath, Encoding.UTF8.GetBytes(content));
                            UpdateControls();
                            string fileName = System.IO.Path.GetFileName(localPath);
                            if (outputLanguage.Length == 2)
                                outputLanguage += outputLanguage;
                            outputLanguage = outputLanguage.Substring(0, 4);
                            string assetName = GetOutputAssetPrefix() + GetAssetSubtitleMidfix() + GetAssetLanguageMidfix(outputLanguage) + GetAssetNameSuffix(inputAssetName);

                            if (!AssetFileNameIsOk(assetName))
                            {
                                TextBoxLogWriteLine("Asset Name incorrect to create an asset :" + assetName, true);
                            }

                            using (new CursorHandler())
                            {
                                bool result = await UploadAsset(localPath, assetFileName, assetName,true);
                                if (result == false)
                                    TextBoxLogWriteLine("Error while uploading the Asset: " + assetName, true);
                            }
                        }
                        catch (Exception ex)
                        {
                            TextBoxLogWriteLine("Exception while uploading the Asset" + ex.Message, true);
                        }
                        bUploadingAsset = false;
                        PopulateOutputAssets();
                        PopulateOutputFiles();
                        UpdateControls();
                    }
                    else
                        TextBoxLogWriteLine("The translated text is empty, check if the innput text is correct", true);

                }
            }
        }

        private void buttonPlaySearch_Click(object sender, EventArgs e)
        {
            string url = GetSearchSelectedUrl(); 
            if(!string.IsNullOrEmpty(url))
            {
                System.Diagnostics.Process.Start(url);
            }
        }
    }

    public class Media
    {
        public string keyId { set; get; }
        public string mediaId { set; get; }
        public string mediaName { set; get; }
        public string mediaUrl { set; get; }
        public  bool isAudio { set; get; }
        public string subtitleLanguage { set; get; }
        public string subtitleUrl { set; get; }
        public string subtitleStartTime { set; get; }
        public string subtitleEndTime { set; get; }
        public string subtitleContent { set; get; }
        public override string ToString()
        {
            return String.Format(
                "ID: {0}\tName: {1}\tUrl: {2}\tLanguage: {3}\tUrl: {4}\tStartTime: {5}\tEndTime: {6}\tSubtitle: {7}\tIsAudio: {8}",
                mediaId,
                mediaName,
                mediaUrl,
                subtitleLanguage,
                subtitleUrl,
                subtitleStartTime,
                subtitleEndTime,
                subtitleContent,
                isAudio);
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
    public class SubtitileItem
    {
        public string startTime;
        public string endTime;
        public string subtitle;
        public SubtitileItem(string start, string end, string sub)
        {
            startTime = start;
            endTime = end;
            subtitle = sub;
        }
        public override string ToString()
        {
            // Generates the text shown in the combo box
            return startTime + " --> " + endTime + "\r\n" + subtitle + "\r\n\r\n";
        }
    }
}
