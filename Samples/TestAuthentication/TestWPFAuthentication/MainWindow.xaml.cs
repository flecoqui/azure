using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.MediaServices;
using Microsoft.WindowsAzure.MediaServices.Client;

namespace TestWPFAuthentication
{
    public static class SettingsExtensions
    {
        public static bool TryGetValue<T>(this System.Configuration.ApplicationSettingsBase settings, string key, out T value)
        {
            if (settings.Properties[key] != null)
            {
                value = (T)settings[key];
                return true;
            }

            value = default(T);
            return false;
        }

        public static bool ContainsKey(this System.Configuration.ApplicationSettingsBase settings, string key)
        {
            return settings.Properties[key] != null;
        }

        public static void SetValue<T>(this System.Configuration.ApplicationSettingsBase settings, string key, T value)
        {
            settings[key] = value;
            settings.Save();
        }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        void LoadSettings()
        {
            string a = string.Empty;
            
            if (TestWPFAuthentication.Properties.Settings.Default.TryGetValue(nameof(legacyAccountName), out a))
                legacyAccountName.Text = a;
            else
                legacyAccountName.Text = a;
            a = string.Empty;
            if (TestWPFAuthentication.Properties.Settings.Default.TryGetValue(nameof(legacyAccountNameKey), out a))
                legacyAccountNameKey.Text = a;
            else
                legacyAccountNameKey.Text = a;
            a = string.Empty;
            if (TestWPFAuthentication.Properties.Settings.Default.TryGetValue(nameof(azureRegion), out a))
                azureRegion.Text = a;
            else
                azureRegion.Text = a;
            a = string.Empty;
            if (TestWPFAuthentication.Properties.Settings.Default.TryGetValue(nameof(azureActiveDirectoryTenantDomain), out a))
                azureActiveDirectoryTenantDomain.Text = a;
            else
                azureActiveDirectoryTenantDomain.Text = a;
            a = string.Empty;
            if (TestWPFAuthentication.Properties.Settings.Default.TryGetValue(nameof(applicationID), out a))
                applicationID.Text = a;
            else
                applicationID.Text = a;
            a = string.Empty;
            if (TestWPFAuthentication.Properties.Settings.Default.TryGetValue(nameof(applicationKey), out a))
                applicationKey.Text = a;
            else
                applicationKey.Text = a;
        }
        void SaveSettings()
        {
            string a = string.Empty;
            TestWPFAuthentication.Properties.Settings.Default.SetValue(nameof(legacyAccountName), legacyAccountName.Text);
            TestWPFAuthentication.Properties.Settings.Default.SetValue(nameof(legacyAccountNameKey), legacyAccountNameKey.Text);
            TestWPFAuthentication.Properties.Settings.Default.SetValue(nameof(azureRegion), azureRegion.Text);
            TestWPFAuthentication.Properties.Settings.Default.SetValue(nameof(azureActiveDirectoryTenantDomain), azureActiveDirectoryTenantDomain.Text);
            TestWPFAuthentication.Properties.Settings.Default.SetValue(nameof(applicationID), applicationID.Text);
            TestWPFAuthentication.Properties.Settings.Default.SetValue(nameof(applicationKey), applicationKey.Text);
        }
        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
            //legacyAccountNameKey.Text = clegacyAccountNameKey ;
            //legacyAccountName.Text = clegacyAccountName ;
            //azureRegion.Text = cazureRegion;
            //azureActiveDirectoryTenantDomain.Text = cazureActiveDirectoryTenantDomain ;
            //applicationID.Text = capplicationID ;
            //applicationKey.Text = capplicationKey ;
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
                            Console.WriteLine(a.Name);
                        }
                        SaveSettings();
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message; 
                }
            }
            if(count>=0)
                MessageBox.Show("LegacyAuthentication successful: " + count.ToString() + " asset(s) found");
            else
                MessageBox.Show("LegacyAuthentication failed" + (string.IsNullOrEmpty(msg)?"":": Exception - " + msg));
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
                            Console.WriteLine(a.Name);
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
                MessageBox.Show("UserAuthentication successful: " + count.ToString() + " asset(s) found");
            else
                MessageBox.Show("UserAuthentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
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
                            Console.WriteLine(a.Name);
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
                MessageBox.Show("ServicePrincipalAuthentication successful: " + count.ToString() + " asset(s) found");
            else
                MessageBox.Show("ServicePrincipalAuthentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
        }
    }
}
