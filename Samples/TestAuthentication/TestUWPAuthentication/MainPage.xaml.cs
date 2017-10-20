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
using TestUWPAuthentication.AzureMediaServicesREST;

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


        public async System.Threading.Tasks.Task<string> GetLegacyToken(string AccountName, string AccountKey)
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
                LogMessage("Exception " + e.Message);
                token = string.Empty;
            }
            return token;
        }
        public async System.Threading.Tasks.Task<string> GetAzureADServicePrincipalToken(string ApplicationID, string ApplicationKey, string tenantDomain)
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
                LogMessage("Exception " + e.Message);
                token = string.Empty;
            }
            return token;
        }
        public  async System.Threading.Tasks.Task<string> GetAzureADUserInteractiveTokenUWP(string ApplicationID, string tenantDomain)
        {
            string token = string.Empty;
            string authority = "https://login.microsoftonline.com/" + tenantDomain;
            try
            {
                //                WebAccountProvider wap = await WebAuthenticationCoreManager.FindAccountProviderAsync("https://login.microsoft.com", authority);
                WebAccountProvider wap = await WebAuthenticationCoreManager.FindAccountProviderAsync("https://login.microsoftonline.com/", authority);
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
                LogMessage("Exception " + e.Message);
                token = string.Empty;
            }
            return token;
        }
        void LoadSettings()
        {

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            object a;

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
            logs.TextChanged += Logs_TextChanged;
            LoadSettings();
        }

        //string legacyAccountKey = "2n0gfPJzqlOY5MWFW38oVyQQer6vfnrQMmeFY8g2Jps=";
        //string legacyAccountName = "neamsaccount";
        //string azureregion = "northeurope";
        //string azureActiveDirectoryTenantDomain = "microsoft.onmicrosoft.com";
        //string applicationID = "1e5b04c6-391b-41f8-ace5-7a5e67fe232a";
        //string applicationKey = "07oVRNVe+i9HGXfGY/Kvt/tDGKgeHIfyUtjvJUl0904=";

        //string clegacyAccountNameKey = "WXa5wXm86Gp2dr+QyWZMB3y+kA1mEEC8JN7ZSllOric=";
        //string clegacyAccountName = "weamsaccount";
        //string cazureRegion = "westeurope";
        //string ClientID = "d476653d-842c-4f52-862d-397463ada5e7"
        //string cazureActiveDirectoryTenantDomain = "flecoquihotmail.onmicrosoft.com";
        //string capplicationID = "c1a69f3e-be7a-414f-ab12-5b51d47a8177";
        //string capplicationKey = "gkVUkurVEwEp5LUt1es/9hFHlFd8HyyWmVw8sqhSlVs=";

        private async void Show(string msg)
        {
            var dialog = new MessageDialog(msg);
            await dialog.ShowAsync();
        }
        private async void LegacyAuthentication_Click(object sender, RoutedEventArgs e)
        {
            LogMessage("Legacy Authentication: Account Name: " + legacyAccountName.Text + " Account Name Key: " + legacyAccountNameKey.Text );
            string Token = await GetLegacyToken(legacyAccountName.Text, legacyAccountNameKey.Text);
            int count = -1;
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(Token))
            {
                LogMessage("Legacy Authentication successful");
                LogMessage("Token: " + Token);
                try
                {
                    LogMessage("Getting Media Objects" );
                    count += await GetMediaObjectsCount<Asset>(Token, null, null);
                    count += await GetMediaObjectsCount<MediaProcessor>(Token, null, null);
                    count += await GetMediaObjectsCount<Channel>(Token, null, null);
                    count += await GetMediaObjectsCount<AzureMediaServicesREST.Program>(Token, null, null);
                    count += await GetMediaObjectsCount<StreamingEndpoint>(Token, null, null);
                    count += await GetMediaObjectsCount<Locator>(Token, null, null);
                    count += await GetMediaObjectsCount<AccessPolicie>(Token, null, null);
                    SaveSettings();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    LogMessage("Getting Media Objects - Exception: " + msg);
                }
                if (count >= 0)
                    Show("Legacy Authentication successful: " + count.ToString() + " Media Services Objects(s) found");
                else
                    Show("Legacy Authentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
            }
            else
                Show("Legacy Authentication failed");
        }

        private async void UserAuthentication_Click(object sender, RoutedEventArgs e)
        {
            string msg = string.Empty;
            int count = -1;
            string Token = null;
            LogMessage("Interactive User Authentication - Azure Tenant: " + azureActiveDirectoryTenantDomain.Text);
            string authority = "https://login.microsoftonline.com/" + azureActiveDirectoryTenantDomain.Text;
            Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext ac =
                new Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationContext(authority);
            string resourceUrl = "https://rest.media.azure.net";
            string redirectUri = "https://AzureMediaServicesNativeSDK";

            Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult result = null;
            try
            {
                Microsoft.IdentityModel.Clients.ActiveDirectory.IPlatformParameters param = new Microsoft.IdentityModel.Clients.ActiveDirectory.PlatformParameters(Microsoft.IdentityModel.Clients.ActiveDirectory.PromptBehavior.Always, false); ;
                result = await ac.AcquireTokenAsync(resourceUrl, "d476653d-842c-4f52-862d-397463ada5e7", new Uri(redirectUri), param);
            }
            catch(Exception ex)
            {
                LogMessage("Interactive User Authentication - Exception: " + ex.Message);
            }

            if (result != null)
            {
                Token = result.AccessToken;
                if (!string.IsNullOrEmpty(Token))
                {
                    LogMessage("Interactive User Authentication successful");
                    LogMessage("Token: " + Token);

                    try
                    {
                        LogMessage("Getting Media Objects - Account Name: " + legacyAccountName.Text + " from region " + azureRegion.Text);
                        count += await GetMediaObjectsCount<Asset>(Token, legacyAccountName.Text, azureRegion.Text);
                        count += await GetMediaObjectsCount<MediaProcessor>(Token, legacyAccountName.Text, azureRegion.Text);
                        count += await GetMediaObjectsCount<Channel>(Token, legacyAccountName.Text, azureRegion.Text);
                        count += await GetMediaObjectsCount<AzureMediaServicesREST.Program>(Token, legacyAccountName.Text, azureRegion.Text);
                        count += await GetMediaObjectsCount<StreamingEndpoint>(Token, legacyAccountName.Text, azureRegion.Text);
                        count += await GetMediaObjectsCount<Locator>(Token, legacyAccountName.Text, azureRegion.Text);
                        count += await GetMediaObjectsCount<AccessPolicie>(Token, legacyAccountName.Text, azureRegion.Text);
                        SaveSettings();
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                        LogMessage("Getting Media Objects - Exception: " + msg);
                    }
                    if (count >= 0)
                        Show("Interactive User Authentication successful: " + count.ToString() + " asset(s) found");
                    else
                        Show("Interactive User Authentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
                }
                else
                    Show("Interactive User Authentication failed");
            }
            else
                Show("Interactive User Authentication failed");
        }
        public async System.Threading.Tasks.Task<int> GetMediaObjectsCount<T>(string Token, string accountName, string azureRegion, string forcedUri = null)
        {
            int count = -1;
            List<T> objects = await AzureMediaServicesClient.GetMediaObjects<T>(Token, accountName, azureRegion,forcedUri);

            if (objects != null)
            {
                count = objects.Count();
                foreach (var a in objects)
                {
                    LogMessage(a.ToString());
                }
            }
            return count;
        }
        private async void ServicePrincipalAuthentication_Click(object sender, RoutedEventArgs e)
        {
            LogMessage("Service Principal Authentication: ApplicationID: " + applicationID.Text + " ApplicationKey: " + applicationKey.Text + " Azure Tenant: " + azureActiveDirectoryTenantDomain.Text);
            string Token = await GetAzureADServicePrincipalToken(applicationID.Text, applicationKey.Text,azureActiveDirectoryTenantDomain.Text);
            int count = -1;
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(Token))
            {
                LogMessage("Service Principal Authentication successful");
                LogMessage("Token: " + Token);
                try
                {
                    LogMessage("Getting Media Objects - Account Name: " + legacyAccountName.Text + " from region " + azureRegion.Text);
                    count += await GetMediaObjectsCount<Asset>(Token, legacyAccountName.Text, azureRegion.Text);
                    count += await GetMediaObjectsCount<MediaProcessor>(Token, legacyAccountName.Text, azureRegion.Text);
                    count += await GetMediaObjectsCount<Channel>(Token, legacyAccountName.Text, azureRegion.Text);
                    count += await GetMediaObjectsCount<AzureMediaServicesREST.Program>(Token, legacyAccountName.Text, azureRegion.Text);
                    count += await GetMediaObjectsCount<StreamingEndpoint>(Token, legacyAccountName.Text, azureRegion.Text);
                    count += await GetMediaObjectsCount<Locator>(Token, legacyAccountName.Text, azureRegion.Text);
                    count += await GetMediaObjectsCount<AccessPolicie>(Token, legacyAccountName.Text, azureRegion.Text);
                    SaveSettings();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    LogMessage("Getting Media Objects - Exception: " + msg);
                }
                if (count >= 0)
                    Show("ServicePrincipalAuthentication successful: " + count.ToString() + " Media Services Objects(s) found");
                else
                    Show("ServicePrincipalAuthentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
            }
            else
                Show("ServicePrincipalAuthentication failed");
        }


        #region Logs
        void PushMessage(string Message)
        {
            App app = Windows.UI.Xaml.Application.Current as App;
            if (app != null)
                app.MessageList.Enqueue(Message);
        }
        bool PopMessage(out string Message)
        {
            Message = string.Empty;
            App app = Windows.UI.Xaml.Application.Current as App;
            if (app != null)
                return app.MessageList.TryDequeue(out Message);
            return false;
        }
        /// <summary>
        /// Display Message on the application page
        /// </summary>
        /// <param name="Message">String to display</param>
        async void LogMessage(string Message)
        {
            string Text = string.Format("{0:d/M/yyyy HH:mm:ss.fff}", DateTime.Now) + " " + Message + "\n";
            PushMessage(Text);
            System.Diagnostics.Debug.WriteLine(Text);
            await DisplayLogMessage();
        }
        /// <summary>
        /// Display Message on the application page
        /// </summary>
        /// <param name="Message">String to display</param>
        async System.Threading.Tasks.Task<bool> DisplayLogMessage()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {

                    string result;
                    while (PopMessage(out result))
                    {
                        logs.Text += result;
                        if (logs.Text.Length > 16000)
                        {
                            string LocalString = logs.Text;
                            while (LocalString.Length > 12000)
                            {
                                int pos = LocalString.IndexOf('\n');
                                if (pos == -1)
                                    pos = LocalString.IndexOf('\r');


                                if ((pos >= 0) && (pos < LocalString.Length))
                                {
                                    LocalString = LocalString.Substring(pos + 1);
                                }
                                else
                                    break;
                            }
                            logs.Text = LocalString;
                        }
                    }
                }
            );
            return true;
        }
        /// <summary>
        /// This method is called when the content of the Logs TextBox changed  
        /// The method scroll to the bottom of the TextBox
        /// </summary>
        void Logs_TextChanged(object sender, TextChangedEventArgs e)
        {
            //  logs.Focus(FocusState.Programmatic);
            // logs.Select(logs.Text.Length, 0);
            var tbsv = GetFirstDescendantScrollViewer(logs);
            tbsv.ChangeView(null, tbsv.ScrollableHeight, null, true);
        }
        /// <summary>
        /// Retrieve the ScrollViewer associated with a control  
        /// </summary>
        ScrollViewer GetFirstDescendantScrollViewer(DependencyObject parent)
        {
            var c = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < c; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var sv = child as ScrollViewer;
                if (sv != null)
                    return sv;
                sv = GetFirstDescendantScrollViewer(child);
                if (sv != null)
                    return sv;
            }

            return null;
        }
        #endregion


    }
}
