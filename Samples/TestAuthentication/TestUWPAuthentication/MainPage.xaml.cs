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
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestUWPAuthentication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        void LoadSettings()
        {

            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string a = string.Empty;

            if (localSettings.Values.TryGetValue(nameof(legacyAccountName), out a))
                legacyAccountName.Text = a;
            else
                legacyAccountName.Text = a;
            a = string.Empty;
            if (localSettings.Values.TryGetValue(nameof(legacyAccountNameKey), out a))
                legacyAccountNameKey.Text = a;
            else
                legacyAccountNameKey.Text = a;
            a = string.Empty;
            if (localSettings.Values.TryGetValue(nameof(azureRegion), out a))
                azureRegion.Text = a;
            else
                azureRegion.Text = a;
            a = string.Empty;
            if (localSettings.Values.TryGetValue(nameof(azureActiveDirectoryTenantDomain), out a))
                azureActiveDirectoryTenantDomain.Text = a;
            else
                azureActiveDirectoryTenantDomain.Text = a;
            a = string.Empty;
            if (localSettings.Values.TryGetValue(nameof(applicationID), out a))
                applicationID.Text = a;
            else
                applicationID.Text = a;
            a = string.Empty;
            if (localSettings.Values.TryGetValue(nameof(applicationKey), out a))
                applicationKey.Text = a;
            else
                applicationKey.Text = a;
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
        private void LegacyAuthentication_Click(object sender, RoutedEventArgs e)
        {
            CloudMediaContext context = new Microsoft.WindowsAzure.MediaServices.Client.CloudMediaContext(legacyAccountName.Text, legacyAccountNameKey.Text);
            int count = -1;
            string msg = string.Empty;
            if (context != null)
            {
                try
                {
                    var assets = context.Assets;
                    if (assets != null)
                    {
                        count = assets.Count();
                        foreach (var a in assets)
                        {
                            System.Diagnostics.Debug.WriteLine(a.Name);
                        }
                        SaveSettings();
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

        private void UserAuthentication_Click(object sender, RoutedEventArgs e)
        {


            var tokenCredentials = new AzureAdTokenCredentials(azureActiveDirectoryTenantDomain.Text, AzureEnvironments.AzureCloudEnvironment);
            var tokenProvider = new AzureAdTokenProvider(tokenCredentials);

            // Specify your REST API endpoint, for example "https://neamsaccount.restv2.northeurope.media.azure.net/API".
            CloudMediaContext context = new CloudMediaContext(new Uri("https://" + legacyAccountName.Text + ".restv2." + azureRegion.Text + ".media.azure.net/API"), tokenProvider);

            string msg = string.Empty;
            int count = -1;
            if (context != null)
            {
                try
                {
                    var assets = context.Assets;
                    if (assets != null)
                    {
                        count = assets.Count();
                        foreach (var a in assets)
                        {
                            System.Diagnostics.Debug.WriteLine(a.Name);
                        }
                        SaveSettings();
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
            
            if (count >= 0)
                Show("UserAuthentication successful: " + count.ToString() + " asset(s) found");
            else
                Show("UserAuthentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
        }
        private void ServicePrincipalAuthentication_Click(object sender, RoutedEventArgs e)
        {
            var tokenCredentials = new AzureAdTokenCredentials(azureActiveDirectoryTenantDomain.Text,
                            new AzureAdClientSymmetricKey(applicationID.Text, applicationKey.Text),
                            AzureEnvironments.AzureCloudEnvironment);

            var tokenProvider = new AzureAdTokenProvider(tokenCredentials);


            // Specify your REST API endpoint, for example "https://accountname.restv2.westcentralus.media.azure.net/API".
            CloudMediaContext context = new CloudMediaContext(new Uri("https://" + legacyAccountName.Text + ".restv2." + azureRegion.Text + ".media.azure.net/API"), tokenProvider);

            string msg = string.Empty;
            int count = -1;
            if (context != null)
            {
                try
                {
                    var assets = context.Assets;
                    if (assets != null)
                    {
                        count = assets.Count();
                        foreach (var a in assets)
                        {
                            System.Diagnostics.Debug.WriteLine(a.Name);
                        }
                        SaveSettings();
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
            if (count >= 0)
                Show("ServicePrincipalAuthentication successful: " + count.ToString() + " asset(s) found");
            else
                Show("ServicePrincipalAuthentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
        }
    }
}
