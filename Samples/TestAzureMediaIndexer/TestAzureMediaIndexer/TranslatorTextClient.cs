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

namespace TestAzureMediaIndexer
{
    /// <summary>
    /// class TranslatorTextClient: TranslatorText UWP Client
    /// </summary>
    /// <info>
    /// Event data that describes how this page was reached.
    /// This parameter is typically used to configure the page.
    /// </info>
    public class TranslatorTextClient
    {
        private string SubscriptionKey;
        private string Token;
        private const string AuthUrl = "https://api.cognitive.microsoft.com/sts/v1.0/issueToken";
        private const string BaseUrl = "http://api.microsofttranslator.com/v2/Http.svc/";
        private const string LanguagesUri = "GetLanguagesForTranslate";
        private const string TranslateUri = "Translate?text={0}&to={1}&contentType=text/plain";
        private const string TranslateWithFromUri = "Translate?text={0}&from={1}&to={2}&contentType=text/plain";
        private const string DetectUri = "Detect?text={0}";
        private const string ArrayNamespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays";


        /// <summary>
        /// class TranslatorTextClient constructor
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        public TranslatorTextClient()
        {
            SubscriptionKey = string.Empty;
            Token = string.Empty;
        }

        /// <summary>
        /// GetToken method
        /// </summary>
        /// <param name="subscriptionKey">SubscriptionKey associated with the TranslatorText 
        /// Cognitive Service subscription.
        /// </param>
        /// <return>Token which is used for all calls to the TranslatorText REST API.
        /// </return>
        public async System.Threading.Tasks.Task<string> GetToken(string subscriptionKey)
        {
            if (string.IsNullOrEmpty(subscriptionKey))
                return string.Empty;
            SubscriptionKey = subscriptionKey;
            try
            {
                Token = string.Empty;
                System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
                hc.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
                System.Net.Http.StringContent content = new System.Net.Http.StringContent(string.Empty);
                System.Net.Http.HttpResponseMessage hrm = await hc.PostAsync(new Uri(AuthUrl), content);
                if (hrm != null)
                {
                    
                    switch (hrm.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            var b = await hrm.Content.ReadAsByteArrayAsync();
                            string result = System.Text.UTF8Encoding.UTF8.GetString(b);
                            if (!string.IsNullOrEmpty(result))
                            {
                                Token = "Bearer " + result;
                                return Token;
                            }
                            break;

                        default:
                            System.Diagnostics.Debug.WriteLine("Http Response Error:" + hrm.StatusCode.ToString() + " reason: " + hrm.ReasonPhrase.ToString());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception while getting the token: " + ex.Message);
            }
            return string.Empty;
        }
        /// <summary>
        /// RenewToken method
        /// </summary>
        /// <param>
        /// </param>
        /// <return>Token which is used to all the calls to the TranslatorText REST API.
        /// </return>
        public async System.Threading.Tasks.Task<string> RenewToken()
        {
            if (string.IsNullOrEmpty(SubscriptionKey))
                return string.Empty;
            try
            {
                Token = string.Empty;
                System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
                hc.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);
                System.Net.Http.StringContent content = new System.Net.Http.StringContent(String.Empty);
                System.Net.Http.HttpResponseMessage hrm = await hc.PostAsync(new Uri(AuthUrl), content);
                if (hrm != null)
                {
                    switch (hrm.StatusCode)
                    {
                        case System.Net.HttpStatusCode.OK:
                            var b = await hrm.Content.ReadAsByteArrayAsync();
                            string result = System.Text.UTF8Encoding.UTF8.GetString(b.ToArray());
                            if (!string.IsNullOrEmpty(result))
                            {
                                Token = "Bearer  " + result;
                                return Token;
                            }
                            break;

                        default:
                            System.Diagnostics.Debug.WriteLine("Http Response Error:" + hrm.StatusCode.ToString() + " reason: " + hrm.ReasonPhrase.ToString());
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception while getting the token: " + ex.Message);
            }
            return string.Empty;
        }
        /// <summary>
        /// HasToken method
        /// </summary>
        /// <param>Check if a Token has been acquired
        /// </param>
        /// <return>true if a Token has been acquired to use the TranslatorText REST API.
        /// </return>
        public bool HasToken()
        {
            if (string.IsNullOrEmpty(Token))
                return false;
            return true;
        }
        /// <summary>
        /// GetLanguages method
        /// Return the list of supported languages.
        /// </summary>
        /// <return>list of supported languages
        /// </return>
        public async System.Threading.Tasks.Task<System.Collections.Generic.Dictionary<string, string>> GetLanguages()
        {
            System.Collections.Generic.Dictionary<string, string> r = new System.Collections.Generic.Dictionary<string, string>();
            int loop = 1;

            while (loop-- > 0)
            {
                try
                {
                    string languageUrl = BaseUrl + LanguagesUri;
                    System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
                    System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();
                    hc.DefaultRequestHeaders.Add("Authorization", Token);
                    System.Net.Http.HttpResponseMessage hrm = null;
                    System.Diagnostics.Debug.WriteLine("REST API Get");
                    hrm = await hc.GetAsync(new Uri(languageUrl));
                    if (hrm != null)
                    {
                        switch (hrm.StatusCode)
                        {
                            case System.Net.HttpStatusCode.OK:
                                var b = await hrm.Content.ReadAsByteArrayAsync();
                                    string result = System.Text.UTF8Encoding.UTF8.GetString(b);
                                if (!string.IsNullOrEmpty(result))
                                {
                                    System.Xml.Linq.XNamespace ns = ArrayNamespace;
                                    var doc = System.Xml.Linq.XDocument.Parse(result);

                                    doc.Root.Elements(ns + "string").Select(s => s.Value).ToList().ForEach(lang =>
                                    {
                                        try
                                        {
                                            var culture = new System.Globalization.CultureInfo(lang);
                                            if (!culture.EnglishName.StartsWith("Unknown"))
                                            {
                                                r.Add(lang, culture.EnglishName);
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    });
                                }
                                break;
                            case System.Net.HttpStatusCode.Forbidden:
                                string token = await RenewToken();
                                if (string.IsNullOrEmpty(token))
                                {
                                    loop++;
                                }
                                break;

                            default:
                                int code = (int)hrm.StatusCode;
                                string HttpError = "Http Response Error: " + code.ToString() + " reason: " + hrm.ReasonPhrase.ToString();
                                System.Diagnostics.Debug.WriteLine(HttpError);
                                break;
                        }
                    }
                }
                catch (System.Threading.Tasks.TaskCanceledException)
                {
                    System.Diagnostics.Debug.WriteLine("http POST canceled");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("http POST exception: " + ex.Message);
                }
                finally
                {
                    System.Diagnostics.Debug.WriteLine("http POST done");
                }
            }
            return r;

        }

        /// <summary>
        /// Translate method
        /// Translate input text from one language to another language.
        /// </summary>
        /// <param name="Text">
        /// Input Text 
        /// <param name="FromLang">
        /// Input Language
        /// <param name="ToLang">
        /// Output Language
        /// </param>
        /// <return>return output text (translated)
        /// </return>
        public async System.Threading.Tasks.Task<string> Translate(string Text, string FromLang, string ToLang)
        {
            string r = string.Empty;
            int loop = 1;

            while (loop-- > 0)
            {
                try
                {
                    string uriString = null;
                    if (string.IsNullOrWhiteSpace(FromLang))
                    {
                        uriString = string.Format(TranslateUri, Uri.EscapeDataString(Text), ToLang);
                    }
                    else
                    {
                        uriString = string.Format(TranslateWithFromUri, Uri.EscapeDataString(Text), FromLang, ToLang);
                    }
                    System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
                    System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();
                    hc.DefaultRequestHeaders.Add("Authorization", Token);
                    System.Net.Http.HttpResponseMessage hrm = null;
                    System.Diagnostics.Debug.WriteLine("REST API Get");

                    hrm = await hc.GetAsync(new Uri(BaseUrl + uriString));
                    if (hrm != null)
                    {
                        switch (hrm.StatusCode)
                        {
                            case System.Net.HttpStatusCode.OK:
                                var b = await hrm.Content.ReadAsByteArrayAsync();
                                string result = System.Text.UTF8Encoding.UTF8.GetString(b);
                                if (!string.IsNullOrEmpty(result))
                                {
                                    System.Xml.Linq.XNamespace ns = ArrayNamespace;
                                    var doc = System.Xml.Linq.XDocument.Parse(result);
                                    r = doc.Root.Value;
                                }
                                break;
                            case System.Net.HttpStatusCode.Forbidden:
                                string token = await RenewToken();
                                if (string.IsNullOrEmpty(token))
                                {
                                    loop++;
                                }
                                break;

                            default:
                                int code = (int)hrm.StatusCode;
                                string HttpError = "Http Response Error: " + code.ToString() + " reason: " + hrm.ReasonPhrase.ToString();
                                System.Diagnostics.Debug.WriteLine(HttpError);
                                break;
                        }
                    }
                }
                catch (System.Threading.Tasks.TaskCanceledException)
                {
                    System.Diagnostics.Debug.WriteLine("http POST canceled");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("http POST exception: " + ex.Message);
                }
                finally
                {
                    System.Diagnostics.Debug.WriteLine("http POST done");
                }
            }
            return r;
        }

        /// <summary>
        /// DetectLanguage method
        /// Detect Language from input text.
        /// </summary>
        /// <param name="Text">
        /// Input Text 
        /// <return>return language(string)
        /// </return>
        public async System.Threading.Tasks.Task<string> DetectLanguage(string Text)
        {
            string r = string.Empty;
            int loop = 1;

            while (loop-- > 0)
            {
                try
                {
                    string uriString = null;
                    uriString = string.Format(DetectUri, Uri.EscapeDataString(Text));
                    System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
                    System.Threading.CancellationTokenSource cts = new System.Threading.CancellationTokenSource();
                    hc.DefaultRequestHeaders.Add("Authorization", Token);
                    System.Net.Http.HttpResponseMessage hrm = null;
                    System.Diagnostics.Debug.WriteLine("REST API Get");

                    hrm = await hc.GetAsync(new Uri(BaseUrl + uriString));
                    if (hrm != null)
                    {
                        switch (hrm.StatusCode)
                        {
                            case System.Net.HttpStatusCode.OK:
                                var b = await hrm.Content.ReadAsByteArrayAsync();
                                string result = System.Text.UTF8Encoding.UTF8.GetString(b);
                                if (!string.IsNullOrEmpty(result))
                                {
                                    System.Xml.Linq.XNamespace ns = ArrayNamespace;
                                    var doc = System.Xml.Linq.XDocument.Parse(result);
                                    r = doc.Root.Value;
                                }
                                break;
                            case System.Net.HttpStatusCode.Forbidden:
                                string token = await RenewToken();
                                if (string.IsNullOrEmpty(token))
                                {
                                    loop++;
                                }
                                break;

                            default:
                                int code = (int)hrm.StatusCode;
                                string HttpError = "Http Response Error: " + code.ToString() + " reason: " + hrm.ReasonPhrase.ToString();
                                System.Diagnostics.Debug.WriteLine(HttpError);
                                break;
                        }
                    }
                }
                catch (System.Threading.Tasks.TaskCanceledException)
                {
                    System.Diagnostics.Debug.WriteLine("http POST canceled");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("http POST exception: " + ex.Message);
                }
                finally
                {
                    System.Diagnostics.Debug.WriteLine("http POST done");
                }
            }
            return r;
        }



    }
}
