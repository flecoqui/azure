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
        System.Collections.Concurrent.ConcurrentQueue<string> MessageList;
        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
            // Initialize Log Message List
            MessageList = new System.Collections.Concurrent.ConcurrentQueue<string>();

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




        //string clegacyAccountNameKey = "WXa5wXm86Gp2dr+QyWZMB3y+kA1mEEC8JN7ZSllOric=";
        //string clegacyAccountName = "weamsaccount";
        //string cazureRegion = "westeurope";
        //string cazureActiveDirectoryTenantDomain = "flecoquihotmail.onmicrosoft.com";
        //string capplicationID = "c1a69f3e-be7a-414f-ab12-5b51d47a8177";
        //string capplicationKey = "gkVUkurVEwEp5LUt1es/9hFHlFd8HyyWmVw8sqhSlVs=";
        private void LegacyAuthentication_Click(object sender, RoutedEventArgs e)
        {
            LogMessage("Legacy Authentication: Account Name: " + legacyAccountName.Text + " Account Name Key: " + legacyAccountNameKey.Text);
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
                        LogMessage("Legacy Authentication successful");
                        count = assets.Count();
                        LogMessage("Getting Media Objects");
                        foreach (var a in assets)
                        {
                            Console.WriteLine(a.Name);
                        }
                        LogMessage(count.ToString() + " Media Object(s) found...");

                        SaveSettings();
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    LogMessage("Getting Media Objects - Exception: " + msg);

                }
                if (count >= 0)
                    MessageBox.Show("LegacyAuthentication successful: " + count.ToString() + " asset(s) found");
                else
                    MessageBox.Show("LegacyAuthentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
            }
            else
                MessageBox.Show("LegacyAuthentication failed" );
        }

        private void UserAuthentication_Click(object sender, RoutedEventArgs e)
        {


            LogMessage("Interactive User Authentication - Azure Tenant: " + azureActiveDirectoryTenantDomain.Text);
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
                        LogMessage("Interactive User Authentication successful");
                        LogMessage("Getting Media Objects");
                        count = assets.Count();
                        foreach (var a in assets)
                        {
                            Console.WriteLine(a.Name);
                        }
                        LogMessage(count.ToString() + " Media Object(s) found...");

                        SaveSettings();
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    LogMessage("Interactive User Authentication - Exception: " + ex.Message);
                }
                if (count >= 0)
                    MessageBox.Show("UserAuthentication successful: " + count.ToString() + " asset(s) found");
                else
                    MessageBox.Show("UserAuthentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
            }
            else
                MessageBox.Show("UserAuthentication failed"  );
        }
        private void ServicePrincipalAuthentication_Click(object sender, RoutedEventArgs e)
        {
            LogMessage("Service Principal Authentication: ApplicationID: " + applicationID.Text + " ApplicationKey: " + applicationKey.Text + " Azure Tenant: " + azureActiveDirectoryTenantDomain.Text);
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
                        LogMessage("Service Principal Authentication successful");
                        LogMessage("Getting Media Objects");
                        count = assets.Count();
                        foreach (var a in assets)
                        {
                            Console.WriteLine(a.Name);
                        }
                        LogMessage(count.ToString() + " Media Object(s) found...");
                        SaveSettings();
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                    LogMessage("Getting Media Objects - Exception: " + msg);
                }
                if (count >= 0)
                    MessageBox.Show("ServicePrincipalAuthentication successful: " + count.ToString() + " asset(s) found");
                else
                    MessageBox.Show("ServicePrincipalAuthentication failed" + (string.IsNullOrEmpty(msg) ? "" : ": Exception - " + msg));
            }
            else
                MessageBox.Show("ServicePrincipalAuthentication failed" );
        }


        #region Logs
        void PushMessage(string Message)
        {
            MessageList.Enqueue(Message);
        }
        bool PopMessage(out string Message)
        {
            Message = string.Empty;
            return MessageList.TryDequeue(out Message);
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
            await Dispatcher.InvokeAsync(
                () =>
                {

                    string result = string.Empty;
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
         //   tbsv.ChangeView(null, tbsv.ScrollableHeight, null, true);
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
