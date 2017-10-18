using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestUWPAuthentication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
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
                int pos = s.IndexOf(Name, p);
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
        public static async System.Threading.Tasks.Task<List<string>> GetAssetList(string Token, string accountName, string azureRegion)
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
            Uri restAPIUri = null;
            if (!string.IsNullOrEmpty(accountName) && !string.IsNullOrEmpty(azureRegion))
                restAPIUri = new Uri("https://" + accountName + ".restv2." + azureRegion + ".media.azure.net/API/Assets/");
            else
                restAPIUri = new Uri("https://media.windows.net/API/Assets/");
            List<string> list = new List<string>();
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
                        rep = await hc.GetAsync(new Uri(url));
                    }
                }
                if ((rep != null) && (rep.StatusCode == HttpStatusCode.Ok) && (rep.Content != null))
                {
                    string s = rep.Content.ReadAsStringAsync().GetResults();
                    string newUrl = GetAttribute(s, "odata.metadata");
                    if (!string.IsNullOrEmpty(newUrl))
                    {
                        string AssetsSuffix = GetAttributeUrl(s, "Assets");
                        if (!string.IsNullOrEmpty(AssetsSuffix))
                        {
                            string u = newUrl.Replace("$metadata", AssetsSuffix);
                            rep = await hc.GetAsync(new Uri(u));
                            if ((rep != null) && (rep.StatusCode == HttpStatusCode.Ok) && (rep.Content != null))
                            {
                                s = await rep.Content.ReadAsStringAsync();
                            }
                        }
                        if (!string.IsNullOrEmpty(s))
                        {
                            int pos = 0;
                            while (pos >= 0)
                            {
                                string val = GetAttributeFromPos(s, ref pos, "\"Name\"");
                                if (!string.IsNullOrEmpty(val))
                                {
                                    list.Add(val);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + e.Message);
            }

            return list;
        }
        public static async System.Threading.Tasks.Task<string> GetAPIUrl(string Token, string accountName, string azureRegion)
        {
            //GET https://media.windows.net/ HTTP/1.1
            //Authorization: Bearer http% 3a % 2f % 2fschemas.xmlsoap.org % 2fws % 2f2005 % 2f05 % 2fidentity % 2fclaims % 2fnameidentifier = amstestaccount001 & urn % 3aSubscriptionId = z7f19258 - 6753 - 4ca2 - b1ae - 193798e2c9d8 & http % 3a % 2f % 2fschemas.microsoft.com % 2faccesscontrolservice % 2f2010 % 2f07 % 2fclaims % 2fidentityprovider = https % 3a % 2f % 2fwamsprodglobal001acs.accesscontrol.windows.net % 2f & Audience = urn % 3aWindowsAzureMediaServices & ExpiresOn = 1421500579 & Issuer = https % 3a % 2f % 2fwamsprodglobal001acs.accesscontrol.windows.net % 2f & HMACSHA256 = ElVWXOnMVggFQl % 2ft9vhdcv1qH1n % 2fE8l3hRef4zPmrzg % 3d
            //x - ms - version: 2.11
            //Accept: application / json
            //Host: media.windows.net
            Uri restAPIUri = null;
            if (!string.IsNullOrEmpty(accountName) && !string.IsNullOrEmpty(azureRegion))
                restAPIUri = new Uri("https://" + accountName + ".restv2." + azureRegion + ".media.azure.net/");
            else
                restAPIUri = new Uri("https://media.windows.net/");

            string url = string.Empty;
            try
            {
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.TryAppendWithoutValidation("Authorization", "Bearer " + Token);
                hc.DefaultRequestHeaders.TryAppendWithoutValidation("x-ms-version", "2.11");
                hc.DefaultRequestHeaders.TryAppendWithoutValidation("Accept", "application/json");
                hc.DefaultRequestHeaders.Remove("Accept-Encoding");

                HttpResponseMessage rep = await hc.GetAsync(restAPIUri);
                if ((rep != null) && (rep.StatusCode == HttpStatusCode.MovedPermanently) && (rep.Content != null))
                {
                    string s = await rep.Content.ReadAsStringAsync();
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
                                    System.Diagnostics.Debug.WriteLine("Redirected Url: " + url);
                                    rep = await hc.GetAsync(new Uri(url));

                                }
                            }
                        }
                    }
                }
                if ((rep != null) && (rep.StatusCode == HttpStatusCode.Ok) && (rep.Content != null))
                {
                    string s = await rep.Content.ReadAsStringAsync();
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
                                    System.Diagnostics.Debug.WriteLine("Url: " + url);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + e.Message);
                url = string.Empty;
            }
            return url;
        }

        public static async System.Threading.Tasks.Task<string> GetLegacyToken(string AccountName, string AccountKey)
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
                hc.DefaultRequestHeaders.Add("Expect", "100-continue");
                hc.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                hc.DefaultRequestHeaders.Add("Accept", "application/json");
                string AccountKeyUrlEncoded = System.Net.WebUtility.UrlEncode(AccountKey);
                string ToEncode = "grant_type=client_credentials&client_id=" + AccountName + "&client_secret=" + AccountKeyUrlEncoded + "&scope=urn%3aWindowsAzureMediaServices";
                HttpStringContent hContent = new HttpStringContent(ToEncode);
                hContent.Headers.Remove("Content-Type");
                hc.DefaultRequestHeaders.Remove("Accept-Encoding");
                hContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                HttpResponseMessage rep = await hc.PostAsync(new Uri("https://wamsprodglobal001acs.accesscontrol.windows.net/v2/OAuth2-13"), hContent);
                if ((rep != null) && (rep.StatusCode == Windows.Web.Http.HttpStatusCode.Ok) && (rep.Content != null))
                {
                    string s = await rep.Content.ReadAsStringAsync();
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
                System.Diagnostics.Debug.WriteLine("Exception " + e.Message);
                token = string.Empty;
            }
            return token;
        }
        public static async System.Threading.Tasks.Task<string> GetAzureADServicePrincipalToken(string ApplicationID, string ApplicationKey, string tenantDomain)
        {
            //POST https://wamsprodglobal001acs.accesscontrol.windows.net/v2/OAuth2-13 HTTP/1.1
            //Content - Type: application / x - www - form - urlencoded
            //Host: wamsprodglobal001acs.accesscontrol.windows.net
            //Content - Length: 120
            //Expect: 100 -continue
            //Connection: Keep - Alive
            //Accept: application / json
            string resourceUrl = "https://rest.media.azure.net";
            
            string token = string.Empty;
            try
            {
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.Add("Expect", "100-continue");
                hc.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                hc.DefaultRequestHeaders.Add("Accept", "application/json");
                string ToEncode = "resource=" + System.Net.WebUtility.UrlEncode(resourceUrl) + "&client_id=" + ApplicationID + "&client_secret=" + System.Net.WebUtility.UrlEncode(ApplicationKey) + "&grant_type=client_credentials";
                HttpStringContent hContent = new HttpStringContent(ToEncode);
                hContent.Headers.Remove("Content-Type");
                hc.DefaultRequestHeaders.Remove("Accept-Encoding");
                hContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                HttpResponseMessage rep = await hc.PostAsync(new Uri("https://login.microsoftonline.com/" + tenantDomain + "/oauth2/token"), hContent);
                if ((rep != null) && (rep.StatusCode == Windows.Web.Http.HttpStatusCode.Ok) && (rep.Content != null))
                {
                    string s = await rep.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(s))
                    {
                        int pos = s.IndexOf("access_token");
                        if (pos > 0)
                        {
                            int start = s.IndexOf(":", pos);
                            int end = s.IndexOf("}", pos);
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
                System.Diagnostics.Debug.WriteLine("Exception " + e.Message);
                token = string.Empty;
            }
            return token;
        }
        public static async System.Threading.Tasks.Task<string> GetAzureADUserInteractiveToken(string ApplicationID, string tenantDomain)
        {
            string token = string.Empty;
            string authority = "https://login.microsoftonline.com/" + tenantDomain;
            try
            { 
                WebAccountProvider wap = await WebAuthenticationCoreManager.FindAccountProviderAsync("https://login.microsoft.com", authority);
                string resource = "https://rest.media.azure.net";
                WebTokenRequest wtr = new WebTokenRequest(wap, string.Empty, ApplicationID);
                if (wtr != null)
                {
                    wtr.Properties.Add("resource", resource);
                    WebTokenRequestResult wtrr = await WebAuthenticationCoreManager.RequestTokenAsync(wtr);
                    if (wtrr.ResponseStatus == WebTokenRequestStatus.Success)
                    {
                        token = wtrr.ResponseData[0].Token;

                        var account = wtrr.ResponseData[0].WebAccount;

                        var properties = wtrr.ResponseData[0].Properties;
                    }

                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + e.Message);
                token = string.Empty;
            }
            return token;
        }
        public static async System.Threading.Tasks.Task<string> GetAzureADUserInteractiveTokenold(string ApplicationID, string tenantDomain)
        {
            //POST https://wamsprodglobal001acs.accesscontrol.windows.net/v2/OAuth2-13 HTTP/1.1
            //Content - Type: application / x - www - form - urlencoded
            //Host: wamsprodglobal001acs.accesscontrol.windows.net
            //Content - Length: 120
            //Expect: 100 -continue
            //Connection: Keep - Alive
            //Accept: application / json
            string resourceUrl = "https://rest.media.azure.net";
            string redirectUri = "https://AzureMediaServicesNativeSDK";
            string token = string.Empty;
            try
            {
                HttpClient hc = new HttpClient();
                hc.DefaultRequestHeaders.Add("Expect", "100-continue");
                hc.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                hc.DefaultRequestHeaders.Add("Accept", "application/json");
                string ToEncode = "resource=" + System.Net.WebUtility.UrlEncode(resourceUrl) + "&client_id=" + ApplicationID + "&redirect_uri=" + System.Net.WebUtility.UrlEncode(redirectUri) + "&response_mode=query&response_type=code&prompt=login";
                HttpStringContent hContent = new HttpStringContent(ToEncode);
                hContent.Headers.Remove("Content-Type");
                hc.DefaultRequestHeaders.Remove("Accept-Encoding");
                hContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                return "https://login.microsoftonline.com/" + tenantDomain + "/oauth2/authorize?" + hContent;
                /*
                HttpResponseMessage rep = await hc.PostAsync(new Uri("https://login.microsoftonline.com/"+ tenantDomain + "/oauth2/authorize"), hContent);
                if ((rep != null) && (rep.StatusCode == Windows.Web.Http.HttpStatusCode.Ok) && (rep.Content != null))
                {
                    string s = await rep.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(s))
                    {
                        token = s;
                    }
                }*/
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + e.Message);
                token = string.Empty;
            }
            return token;
        }
        void LoadSettings()
        {

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            object a ;

            if (localSettings.Values.TryGetValue(nameof(legacyAccountName), out a))
                legacyAccountName.Text = a.ToString();
            else
                legacyAccountName.Text = a.ToString();
            a = string.Empty;
            if (localSettings.Values.TryGetValue(nameof(legacyAccountNameKey), out a))
                legacyAccountNameKey.Text = a.ToString();
            else
                legacyAccountNameKey.Text = a.ToString();
            a = string.Empty;
            if (localSettings.Values.TryGetValue(nameof(azureRegion), out a))
                azureRegion.Text = a.ToString();
            else
                azureRegion.Text = a.ToString();
            a = string.Empty;
            if (localSettings.Values.TryGetValue(nameof(azureActiveDirectoryTenantDomain), out a))
                azureActiveDirectoryTenantDomain.Text = a.ToString();
            else
                azureActiveDirectoryTenantDomain.Text = a.ToString();
            a = string.Empty;
            if (localSettings.Values.TryGetValue(nameof(applicationID), out a))
                applicationID.Text = a.ToString();
            else
                applicationID.Text = a.ToString();
            a = string.Empty;
            if (localSettings.Values.TryGetValue(nameof(applicationKey), out a))
                applicationKey.Text = a.ToString();
            else
                applicationKey.Text = a.ToString();
        }
        void SaveSettings()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string a = string.Empty;
            localSettings.Values[nameof(legacyAccountName)] = legacyAccountName.Text;
            localSettings.Values[nameof(legacyAccountNameKey)] = legacyAccountNameKey.Text;
            localSettings.Values[nameof(azureRegion)] = azureRegion.Text;
            localSettings.Values[nameof(azureActiveDirectoryTenantDomain)] = azureActiveDirectoryTenantDomain.Text;
            localSettings.Values[nameof(applicationID)] = applicationID.Text;
            localSettings.Values[nameof(applicationKey)] = applicationKey.Text;
        }
        public MainPage()
        {
            this.InitializeComponent();
            AuthenticationWebView.NavigationCompleted += AuthenticationWebView_NavigationCompleted;
            AuthenticationWebView.DOMContentLoaded += AuthenticationWebView_DOMContentLoaded;
            AuthenticationWebView.NavigationStarting += AuthenticationWebView_NavigationStarting;
            AuthenticationWebView.Navigate(new Uri("https://www.google.fr"));
            LoadSettings();
        }

        private async void AuthenticationWebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            /*
            if ((args != null) && (args.Uri != null) && (args.Uri.AbsolutePath != "undefined"))
            {
                System.Diagnostics.Debug.WriteLine("Navigation Completed - Uri: " + args.Uri.ToString() + " " + args.ToString());
                args.Cancel = true;

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    async () =>
                    {
                        HttpClient hc = new HttpClient();
                        HttpResponseMessage hr = await hc.GetAsync(args.Uri);
                        if (hr != null)
                        {
                            string s = await hr.Content.ReadAsStringAsync();
                            AuthenticationWebView.NavigateToString(s);
                        }
                    }
                );
            }
            */
        }

        private void AuthenticationWebView_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            if ((args != null) && (args.Uri != null))
                System.Diagnostics.Debug.WriteLine("Navigation Completed - Uri: " + args.Uri.ToString() + " " + args.ToString());

        }

        private void AuthenticationWebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {

            if ((args != null) && (args.Uri != null))
                System.Diagnostics.Debug.WriteLine("Navigation Completed - Uri: " + args.Uri.ToString() + " " + args.ToString());
        }

        //string legacyAccountKey = "2n0gfPJzqlOY5MWFW38oVyQQer6vfnrQMmeFY8g2Jps=";
        //string legacyAccountName = "neamsaccount";
        //string azureregion = "northeurope";
        //string azureActiveDirectoryTenantDomain = "microsoft.onmicrosoft.com";
        //string applicationID = "1e5b04c6-391b-41f8-ace5-7a5e67fe232a";
        //string applicationKey = "07oVRNVe+i9HGXfGY/Kvt/tDGKgeHIfyUtjvJUl0904=";




        string clegacyAccountNameKey = "WXa5wXm86Gp2dr+QyWZMB3y+kA1mEEC8JN7ZSllOric=";
        string clegacyAccountName = "weamsaccount";
        string cazureRegion = "westeurope";
        string cazureActiveDirectoryTenantDomain = "flecoquihotmail.onmicrosoft.com";
        string capplicationID = "c1a69f3e-be7a-414f-ab12-5b51d47a8177";
        string capplicationKey = "gkVUkurVEwEp5LUt1es/9hFHlFd8HyyWmVw8sqhSlVs=";
        private async void Show(string msg)
        {
            var dialog = new MessageDialog(msg);
            await dialog.ShowAsync();
        }
        private async void LegacyAuthentication_Click(object sender, RoutedEventArgs e)
        {
            string Token =  await GetLegacyToken(legacyAccountName.Text, legacyAccountNameKey.Text);
            int count = -1;
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(Token))
            {

                System.Diagnostics.Debug.WriteLine("Token: " + Token);
                try
                {
                    string url = await GetAPIUrl(Token, null, null);
                    if (!string.IsNullOrEmpty(url))
                    {
                        List<string> assets = await GetAssetList(Token,null,null);

                        if (assets != null)
                        {
                            count = assets.Count();
                            foreach (var a in assets)
                            {
                                System.Diagnostics.Debug.WriteLine(a);
                            }
                            SaveSettings();
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
            if (count >= 0)
                Show("LegacyAuthentication successful: " + count.ToString() + " asset(s) found");
            else
                Show("LegacyAuthentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
        }

        private async void UserAuthentication_Click(object sender, RoutedEventArgs e)
        {
            string Token = await GetAzureADUserInteractiveToken(legacyAccountName.Text, azureActiveDirectoryTenantDomain.Text);
            if (!string.IsNullOrEmpty(Token))
            {
             //   await Windows.UI.Xaml.Controls.WebView.ClearTemporaryWebDataAsync();
              //  AuthenticationWebView.Refresh();
                AuthenticationWebView.Navigate(new Uri(Token));

                //AuthenticationWebView.NavigateToString("\r\n\r\n<!DOCTYPE html><html><head></head><body><button>Click</button>\r\n\r\n</body>\r\n</html>");
            }

            //var tokenCredentials = new AzureAdTokenCredentials(azureActiveDirectoryTenantDomain.Text, AzureEnvironments.AzureCloudEnvironment);
            //var tokenProvider = new AzureAdTokenProvider(tokenCredentials);

            //// Specify your REST API endpoint, for example "https://neamsaccount.restv2.northeurope.media.azure.net/API".
            //CloudMediaContext context = new CloudMediaContext(new Uri("https://" + legacyAccountName.Text + ".restv2." + azureRegion.Text + ".media.azure.net/API"), tokenProvider);

            //string msg = string.Empty;
            //int count = -1;
            //if (context != null)
            //{
            //    try
            //    {
            //        var assets = context.Assets;
            //        if (assets != null)
            //        {
            //            count = assets.Count();
            //            foreach (var a in assets)
            //            {
            //                System.Diagnostics.Debug.WriteLine(a.Name);
            //            }
            //            SaveSettings();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        msg = ex.Message;
            //    }
            //}

            //if (count >= 0)
            //    Show("UserAuthentication successful: " + count.ToString() + " asset(s) found");
            //else
            //    Show("UserAuthentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
        }
        private async void ServicePrincipalAuthentication_Click(object sender, RoutedEventArgs e)
        {
            string Token = await GetAzureADServicePrincipalToken(applicationID.Text, applicationKey.Text,azureActiveDirectoryTenantDomain.Text);
            int count = -1;
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(Token))
            {
                System.Diagnostics.Debug.WriteLine("Token: " + Token);
                try
                {
                    string url = await GetAPIUrl(Token, legacyAccountName.Text, azureRegion.Text);
                    //if (!string.IsNullOrEmpty(url))
                    {
                        List<string> assets = await GetAssetList(Token,legacyAccountName.Text,azureRegion.Text);

                        if (assets != null)
                        {
                            count = assets.Count();
                            foreach (var a in assets)
                            {
                                System.Diagnostics.Debug.WriteLine(a);
                            }
                            SaveSettings();
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
                if (count >= 0)
                    Show("ServicePrincipalAuthentication successful: " + count.ToString() + " asset(s) found");
                else
                    Show("ServicePrincipalAuthentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));

            }
            //var tokenCredentials = new AzureAdTokenCredentials(azureActiveDirectoryTenantDomain.Text,
            //                new AzureAdClientSymmetricKey(applicationID.Text, applicationKey.Text),
            //                AzureEnvironments.AzureCloudEnvironment);

            //var tokenProvider = new AzureAdTokenProvider(tokenCredentials);


            //// Specify your REST API endpoint, for example "https://accountname.restv2.westcentralus.media.azure.net/API".
            //CloudMediaContext context = new CloudMediaContext(new Uri("https://" + legacyAccountName.Text + ".restv2." + azureRegion.Text + ".media.azure.net/API"), tokenProvider);

            //string msg = string.Empty;
            //int count = -1;
            //if (context != null)
            //{
            //    try
            //    {
            //        var assets = context.Assets;
            //        if (assets != null)
            //        {
            //            count = assets.Count();
            //            foreach (var a in assets)
            //            {
            //                System.Diagnostics.Debug.WriteLine(a.Name);
            //            }
            //            SaveSettings();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        msg = ex.Message;
            //    }
            //}
            //if (count >= 0)
            //    Show("ServicePrincipalAuthentication successful: " + count.ToString() + " asset(s) found");
            //else
            //    Show("ServicePrincipalAuthentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
        }
    }
}
