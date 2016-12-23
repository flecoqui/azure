using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestAMSWF
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
            textBoxAccountName.Text = "testamsindexer";
            textBoxAccountKey.Text = "7osPnuOAoPP3dY48IFLuJxY++V8nq4ICI0rCDy12Hsk=";
        }
        Microsoft.WindowsAzure.MediaServices.Client.CloudMediaContext _context;
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxAccountName.Text))
            {
                MessageBox.Show("The account name cannot be empty.");
                return;
            }
            try
            {
                _context = new Microsoft.WindowsAzure.MediaServices.Client.CloudMediaContext(textBoxAccountName.Text, textBoxAccountKey.Text);
                if (_context != null)
                {

                    _context.Credentials.RefreshToken();
                    PopulateAssets();
                }
            }
            catch(Exception ex)

            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }
        void PopulateAssets()
        {
            listAssets.Items.Clear();
            foreach (var asset in _context.Assets)
            {
                listAssets.Items.Add("ASSET: " + asset.Name + " ASSET-ID " + asset.Id);
            }
            if (listAssets.Items.Count > 0)
                listAssets.SelectedItem = 0;

        }
        void PopulateFiles(string id)
        {
            listFiles.Items.Clear();
            foreach (var file in _context.Files)
            {
                if(id == file.Asset.Id)
                    listFiles.Items.Add("FILE: " + file.Name + " ASSET-ID " + file.Asset.Id);
            }
            if (listFiles.Items.Count > 0)
                listFiles.SelectedItem = 0;

        }
        private void listAssets_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateFiles();
        }
        void PopulateFiles()
        {
            string id = "";
            string s = listAssets.SelectedItem as string;
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf(" ASSET-ID ");
                if (pos > 0)
                    id = s.Substring(pos + 10);
            }
            PopulateFiles(id);
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
                PopulateAssets();
                PopulateFiles();
            }
        }
        void UploadProgress(object sender, Microsoft.WindowsAzure.MediaServices.Client.UploadProgressChangedEventArgs args)
        {

        }
        public static async Task DeleteAssetAsync(Microsoft.WindowsAzure.MediaServices.Client.CloudMediaContext mediaContext, Microsoft.WindowsAzure.MediaServices.Client.IAsset asset)
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

            return;

        }
        private async void buttonRemoveAsset_Click(object sender, EventArgs e)
        {
            string id = "";
            string s = listAssets.SelectedItem as string;
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf(" ASSET-ID ");
                if (pos > 0)
                    id = s.Substring(pos + 10);
                var a = _context.Assets.Where(j => j.Id == id).FirstOrDefault();
                if(a != null)
                {
                    await DeleteAssetAsync(_context, a);
                    PopulateAssets();
                    PopulateFiles();
                }
            }
        }
    }
}
