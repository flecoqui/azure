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
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
namespace TestAzureMediaIndexer
{
    
    public partial class MainForm : Form
    {


        public bool SearchIndexExists()
        {
            bool result = false;
            if (_searchContext != null)
            {
                try
                {
                    var response = _searchContext.Indexes.ExistsWithHttpMessagesAsync("media");
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
                catch (Exception ex)
                {
                    TextBoxLogWriteLine("Exception while creating Azure Search Index: " + ex.Message + " " + ex.InnerException.Message, true);
                }
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
                                            keyId = mediaAssetID + "_" + language + "_" + item.startTime.Replace(':', '-').Replace('.', '-'),
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
                catch (Exception ex)
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

        private void buttonPlaySearch_Click(object sender, EventArgs e)
        {
            string url = GetSearchSelectedUrl();
            if (!string.IsNullOrEmpty(url))
            {
                System.Diagnostics.Process.Start(url);

            }
        }

    }
}
