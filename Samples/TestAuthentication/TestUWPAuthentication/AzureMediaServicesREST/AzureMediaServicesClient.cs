using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace TestUWPAuthentication.AzureMediaServicesREST
{
    class AzureMediaServicesClient
    {
        private static string GetRedirectedUrl(string s)
        {
            string Value = string.Empty;
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf("href=");
                if (pos > 0)
                {
                    int start = s.IndexOf("\"", pos);
                    int end = s.IndexOf(">", pos);
                    if ((start > 0) && (end > start))
                    {
                        string value = s.Substring(start + 1, end - start - 1);
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = value.Replace('"', ' ');
                            Value = value.Trim();
                        }
                    }
                }
            }
            return Value;
        }
        public static T ReadToObject<T>(string json)
        {
            T deserializedObject;
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            deserializedObject = (T)ser.ReadObject(ms);
            return deserializedObject;
        }
        public static string GetAttribute(string s, string Name)
        {
            string Value = string.Empty;
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf(Name);
                if (pos > 0)
                {
                    int start = s.IndexOf(":", pos);
                    int end = s.IndexOf(",", pos);
                    if ((start > 0) && (end > start))
                    {
                        string value = s.Substring(start + 1, end - start - 1);
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = value.Replace('"', ' ');
                            Value = value.Trim();
                        }
                    }
                }
            }
            return Value;
        }
        public static string GetAttributeUrl(string s, string Name)
        {
            string Value = string.Empty;
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf("\"name\":\"" + Name + "\"");
                if (pos > 0)
                {
                    int start = s.IndexOf("\"url\":", pos);
                    int end = s.IndexOf("}", pos);
                    if ((start > 0) && (end > start))
                    {
                        string value = s.Substring(start + 6, end - start - 6);
                        if (!string.IsNullOrEmpty(value))
                        {
                            value = value.Replace('"', ' ');
                            Value = value.Trim();
                        }
                    }
                }
            }
            return Value;
        }
        public static async System.Threading.Tasks.Task<List<T>> GetMediaObjects<T>(string Token, string accountName, string azureRegion, string forcedUri)
        {
            List<T> Result = null;
            Uri restAPIUri = null;
            if (string.IsNullOrEmpty(forcedUri))
            {
                if (!string.IsNullOrEmpty(accountName) && !string.IsNullOrEmpty(azureRegion))
                    restAPIUri = new Uri("https://" + accountName + ".restv2." + azureRegion + ".media.azure.net/API/" + typeof(T).Name + "s/");
                else
                    restAPIUri = new Uri("https://media.windows.net/API/" + typeof(T).Name + "s/");
            }
            else
                restAPIUri = new Uri(forcedUri);
            string url = string.Empty;
            try
            {
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.TryAppendWithoutValidation("Authorization", "Bearer " + Token);
                hc.DefaultRequestHeaders.TryAppendWithoutValidation("x-ms-version", "2.11");
                hc.DefaultRequestHeaders.TryAppendWithoutValidation("Accept", "application/json");
                hc.DefaultRequestHeaders.TryAppendWithoutValidation("DataServiceVersion", "3.0");
                hc.DefaultRequestHeaders.TryAppendWithoutValidation("MaxDataServiceVersion", "3.0");
                hc.DefaultRequestHeaders.Remove("Accept-Encoding");

                HttpResponseMessage rep = await hc.GetAsync(restAPIUri);
                if ((rep != null) && (rep.StatusCode == HttpStatusCode.MovedPermanently) && (rep.Content != null))
                {
                    string s = rep.Content.ReadAsStringAsync().GetResults();
                    url = GetRedirectedUrl(s);
                    if (!string.IsNullOrEmpty(url))
                    {
                        Result = await GetMediaObjects<T>(Token, accountName, azureRegion, url);
                    }
                }
                else if ((rep != null) && (rep.StatusCode == HttpStatusCode.Ok) && (rep.Content != null))
                {
                    string s = rep.Content.ReadAsStringAsync().GetResults();
                    // If Legacy REST API 
                    // a subsequent request is required
                    if ((string.IsNullOrEmpty(accountName)) && (string.IsNullOrEmpty(azureRegion)))
                    {
                        string newUrl = GetAttribute(s, "odata.metadata");
                        if (!string.IsNullOrEmpty(newUrl))
                        {
                            string MediaObjectSuffix = GetAttributeUrl(s, typeof(T).Name + "s");
                            if (!string.IsNullOrEmpty(MediaObjectSuffix))
                            {
                                string u = newUrl.Replace("$metadata", MediaObjectSuffix);
                                rep = await hc.GetAsync(new Uri(u));
                                if ((rep != null) && (rep.StatusCode == HttpStatusCode.Ok) && (rep.Content != null))
                                {
                                    s = await rep.Content.ReadAsStringAsync();
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(s))
                    {
                        ResultObject<T> result = ReadToObject<ResultObject<T>>(s);
                        if (result != null)
                        {
                            if (result.value != null)
                            {
                                Result = result.value;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + e.Message);
            }

            return Result;
        }

    }
}
