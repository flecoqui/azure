using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace TestAMS
{
    public class Program
    {
        public static string GetRedirectedUrl(string s)
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
        public static string GetAttributeFromPos(string s, ref int p, string Name)
        {
            string Value = string.Empty;
            if (!string.IsNullOrEmpty(s))
            {
                int pos = s.IndexOf(Name,p);
                p = -1;                
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
                            p = end;
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
                int pos = s.IndexOf("\"name\":\"" + Name+ "\"");
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
        public static bool CreateAsset(string Token, string AssetName)
        {
            //POST https://media.windows.net/API/Assets HTTP/1.1  
            //Content - Type: application / json; odata = verbose
            //Accept: application / json; odata = verbose
            //DataServiceVersion: 3.0
            //MaxDataServiceVersion: 3.0
            //x - ms - version: 2.11
            //Authorization: Bearer http% 3a % 2f % 2fschemas.xmlsoap.org % 2fws % 2f2005 % 2f05 % 2fidentity % 2fclaims % 2fnameidentifier = 070500D0 - F35C - 4A5A - 9249 - 485BBF4EC70B & http % 3a % 2f % 2fschemas.microsoft.com % 2faccesscontrolservice % 2f2010 % 2f07 % 2fclaims % 2fidentityprovider = https % 3a % 2f % 2fwamsprodglobal001acs.accesscontrol.windows.net % 2f & Audience = urn % 3aWindowsAzureMediaServices & ExpiresOn = 1334275521 & Issuer = https % 3a % 2f % 2fwamsprodglobal001acs.accesscontrol.windows.net % 2f & HMACSHA256 = GxdBb % 2fmEyN7iHdNxbawawHRftLhPFFqxX1JZckuv3hY % 3d
            //Host: media.windows.net
            //Content - Length: 27
            //Expect: 100 -continue
            //{ "Name" : "NewJSONAsset" }
            bool response = false;
            string url = string.Empty;
            try
            {
                string ToEncode = "{ \"Name\" : \""+ AssetName + "\" }";
                HttpContent hContent = new StringContent(ToEncode);
                hContent.Headers.Remove("Content-Type");
                hContent.Headers.TryAddWithoutValidation("Content-Type", "application/json;odata=verbose");

                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Token);
                hc.DefaultRequestHeaders.TryAddWithoutValidation("x-ms-version", "2.11");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("DataServiceVersion", "3.0");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("MaxDataServiceVersion", "3.0");
                hc.DefaultRequestHeaders.Remove("Accept-Encoding");

                HttpResponseMessage rep = hc.PostAsync(new Uri("https://media.windows.net/API/Assets/"), hContent).Result;
                if ((rep != null) && (rep.StatusCode == System.Net.HttpStatusCode.MovedPermanently) && (rep.Content != null))
                {
                    string s = rep.Content.ReadAsStringAsync().Result;
                    url  = GetRedirectedUrl(s);
                    if (!string.IsNullOrEmpty(url))
                    {
                        rep = hc.PostAsync(new Uri(url), hContent).Result;
                    }
                }
                if ((rep != null) && (rep.StatusCode == System.Net.HttpStatusCode.OK) && (rep.Content != null))
                {
                    string s = rep.Content.ReadAsStringAsync().Result;
                    string newUrl = GetAttribute(s, "odata.metadata");
                    if (!string.IsNullOrEmpty(newUrl))
                    {
                        string AssetsSuffix = GetAttributeUrl(s, "Assets");
                        if (!string.IsNullOrEmpty(AssetsSuffix))
                        {
                            hContent = new StringContent(ToEncode);
                            hContent.Headers.Remove("Content-Type");
                            hContent.Headers.TryAddWithoutValidation("Content-Type", "application/json;odata=verbose");
                            string u = newUrl.Replace("$metadata", AssetsSuffix);
                            rep = hc.PostAsync(new Uri(u), hContent).Result;
                            if ((rep != null) && (rep.StatusCode == System.Net.HttpStatusCode.Created) )
                                response = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
            }
            return response;
        }

        public static List<string> GetAssetList(string Token)
        {
            //GET https://media.windows.net/API/Assets('nb:cid:UUID:fccb8cd9-7afa-4365-a36e-d5d68409bb64') HTTP/1.1  
            //Content - Type: application / json; odata = verbose
            //Accept: application / json; odata = verbose
            //DataServiceVersion: 3.0
            //MaxDataServiceVersion: 3.0
            //x - ms - version: 2.11
            //Authorization: Bearer http% 3a % 2f % 2fschemas.xmlsoap.org % 2fws % 2f2005 % 2f05 % 2fidentity % 2fclaims % 2fnameidentifier = 070500D0 - F35C - 4A5A - 9249 - 485BBF4EC70B & http % 3a % 2f % 2fschemas.microsoft.com % 2faccesscontrolservice % 2f2010 % 2f07 % 2fclaims % 2fidentityprovider = https % 3a % 2f % 2fwamsprodglobal001acs.accesscontrol.windows.net % 2f & Audience = urn % 3aWindowsAzureMediaServices & ExpiresOn = 1334275521 & Issuer = https % 3a % 2f % 2fwamsprodglobal001acs.accesscontrol.windows.net % 2f & HMACSHA256 = GxdBb % 2fmEyN7iHdNxbawawHRftLhPFFqxX1JZckuv3hY % 3d
            //Host: media.windows.net
            //Content - Length: 0

            List<string> list = new List<string>();
            string url = string.Empty;
            try
            {
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Token);
                hc.DefaultRequestHeaders.TryAddWithoutValidation("x-ms-version", "2.11");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("DataServiceVersion", "3.0");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("MaxDataServiceVersion", "3.0");
                hc.DefaultRequestHeaders.Remove("Accept-Encoding");

                HttpResponseMessage rep = hc.GetAsync(new Uri("https://media.windows.net/API/Assets/")).Result;
                if ((rep != null) && (rep.StatusCode == System.Net.HttpStatusCode.MovedPermanently) && (rep.Content != null))
                {
                    string s = rep.Content.ReadAsStringAsync().Result;
                    url = GetRedirectedUrl(s);
                    if (!string.IsNullOrEmpty(url))
                    {
                        rep = hc.GetAsync(new Uri(url)).Result;
                    }
                }
                if ((rep != null) && (rep.StatusCode == System.Net.HttpStatusCode.OK) && (rep.Content != null))
                {
                    string s = rep.Content.ReadAsStringAsync().Result;
                    string newUrl = GetAttribute(s, "odata.metadata");
                    if (!string.IsNullOrEmpty(newUrl))
                    {
                        string AssetsSuffix = GetAttributeUrl(s, "Assets");
                        if (!string.IsNullOrEmpty(AssetsSuffix))
                        {
                            string u = newUrl.Replace("$metadata", AssetsSuffix);
                            rep = hc.GetAsync(new Uri(u)).Result;
                            if ((rep != null) && (rep.StatusCode == System.Net.HttpStatusCode.OK) && (rep.Content != null))
                            {
                                s = rep.Content.ReadAsStringAsync().Result;
                                if (!string.IsNullOrEmpty(s))
                                {
                                    int pos = 0;
                                    while (pos >= 0)
                                    {
                                        string val = GetAttributeFromPos(s, ref pos, "\"Name\"");
                                        if(!string.IsNullOrEmpty(val))
                                        {
                                            list.Add(val);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
            }

            return list;
        }
        public static string GetToken(string AccountName, string AccountKey)
        {
            //POST https://wamsprodglobal001acs.accesscontrol.windows.net/v2/OAuth2-13 HTTP/1.1
            //Content - Type: application / x - www - form - urlencoded
            //Host: wamsprodglobal001acs.accesscontrol.windows.net
            //Content - Length: 120
            //Expect: 100 -continue
            //Connection: Keep - Alive
            //Accept: application / json

            string token = string.Empty;
            try
            {
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Expect", "100-continue");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "Keep-Alive");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                string AccountKeyUrlEncoded = System.Net.WebUtility.UrlEncode(AccountKey);
                string ToEncode = "grant_type=client_credentials&client_id=" + AccountName + "&client_secret=" + AccountKeyUrlEncoded + "&scope=urn%3aWindowsAzureMediaServices";
                HttpContent hContent = new StringContent(ToEncode);
                hContent.Headers.Remove("Content-Type");
                hc.DefaultRequestHeaders.Remove("Accept-Encoding");
                hContent.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");

                HttpResponseMessage rep = hc.PostAsync(new Uri("https://wamsprodglobal001acs.accesscontrol.windows.net/v2/OAuth2-13"), hContent).Result;
                if ((rep != null) && (rep.StatusCode == System.Net.HttpStatusCode.OK) && (rep.Content != null))
                {
                    string s = rep.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(s))
                    {
                        int pos = s.IndexOf("access_token");
                        if (pos > 0)
                        {
                            int start = s.IndexOf(":", pos);
                            int end = s.IndexOf(",", pos);
                            if ((start > 0) && (end > start))
                            {
                                token = s.Substring(start + 1, end - start - 1);
                                if (!string.IsNullOrEmpty(token))
                                {
                                    token = token.Replace('"', ' ');
                                    token = token.Trim();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
                token = string.Empty;
            }
            return token;
        }
        public static string GetAPIUrl(string Token)
        {
            //GET https://media.windows.net/ HTTP/1.1
            //Authorization: Bearer http% 3a % 2f % 2fschemas.xmlsoap.org % 2fws % 2f2005 % 2f05 % 2fidentity % 2fclaims % 2fnameidentifier = amstestaccount001 & urn % 3aSubscriptionId = z7f19258 - 6753 - 4ca2 - b1ae - 193798e2c9d8 & http % 3a % 2f % 2fschemas.microsoft.com % 2faccesscontrolservice % 2f2010 % 2f07 % 2fclaims % 2fidentityprovider = https % 3a % 2f % 2fwamsprodglobal001acs.accesscontrol.windows.net % 2f & Audience = urn % 3aWindowsAzureMediaServices & ExpiresOn = 1421500579 & Issuer = https % 3a % 2f % 2fwamsprodglobal001acs.accesscontrol.windows.net % 2f & HMACSHA256 = ElVWXOnMVggFQl % 2ft9vhdcv1qH1n % 2fE8l3hRef4zPmrzg % 3d
            //x - ms - version: 2.11
            //Accept: application / json
            //Host: media.windows.net

            string url = string.Empty;
            try
            {
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + Token);
                hc.DefaultRequestHeaders.TryAddWithoutValidation("x-ms-version", "2.11");
                hc.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
                hc.DefaultRequestHeaders.Remove("Accept-Encoding");

                HttpResponseMessage rep = hc.GetAsync(new Uri("https://media.windows.net/")).Result;
                if ((rep != null) && (rep.StatusCode == System.Net.HttpStatusCode.MovedPermanently) && (rep.Content != null))
                {
                    string s = rep.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(s))
                    {
                        int pos = s.IndexOf("href=");
                        if (pos > 0)
                        {
                            int start = s.IndexOf("\"", pos);
                            int end = s.IndexOf(">", pos);
                            if ((start > 0) && (end > start))
                            {
                                url = s.Substring(start + 1, end - start - 1);
                                if (!string.IsNullOrEmpty(url))
                                {
                                    url = url.Replace('"', ' ');
                                    url = url.Trim();
                                    Console.WriteLine("Redirected Url: " + url);
                                    rep = hc.GetAsync(new Uri(url)).Result;

                                }
                            }
                        }
                    }
                }
                if ((rep != null) && (rep.StatusCode == System.Net.HttpStatusCode.OK) && (rep.Content != null))
                {
                    string s = rep.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(s))
                    {
                        int pos = s.IndexOf("odata.metadata");
                        if (pos > 0)
                        {
                            int start = s.IndexOf(":", pos);
                            int end = s.IndexOf(",", pos);
                            if ((start > 0) && (end > start))
                            {
                                url = s.Substring(start + 1, end - start - 1);
                                if (!string.IsNullOrEmpty(url))
                                {
                                    url = url.Replace('"', ' ');
                                    url = url.Trim();
                                    Console.WriteLine("Url: " + url);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
                url = string.Empty;
            }
            return url;
        }
        public static void Main(string[] args)
        {
            string AccountName = "testamsindexer";
            string AccountKey = "7osPnuOAoPP3dY48IFLuJxY++V8nq4ICI0rCDy12Hsk=";
            string Token = GetToken(AccountName, AccountKey);
            if(!string.IsNullOrEmpty(Token))
            {
                Console.WriteLine("Token: " + Token);
                string url = GetAPIUrl(Token);
                if (!string.IsNullOrEmpty(url))
                {
                    Console.WriteLine("Url: " + url);
                    bool res = CreateAsset(Token, "TestAsset");
                    if(res==true)
                        Console.WriteLine("Asset Created" );
                    else
                        Console.WriteLine("Failed to create an asset");

                    List<string> l = GetAssetList(Token);
                    if((l!=null)&&(l.Count>=0))
                    {
                        Console.WriteLine("List - items: " + l.Count.ToString() );
                        foreach(var i in l)
                            Console.WriteLine("List: " + i);

                    }
                }

            }
        }
    }
}
