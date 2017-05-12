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
using System.Windows.Forms;
using Microsoft.WindowsAzure.MediaServices.Client;
namespace TestAzureMediaIndexer
{
    
    public partial class MainForm : Form
    {
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

                    if (MessageBox.Show("You are about to translate subtitles associated with asset: " + inputAssetName + " from language '" + inputLanguage + "' to language '" + outputLanguage + "'", "Cognitive Services Text Translator", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
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
                                bool result = await UploadAsset(localPath, assetFileName, assetName, true);
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

    }
}
