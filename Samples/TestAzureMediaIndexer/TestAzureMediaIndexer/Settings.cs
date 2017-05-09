using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace TestAzureMediaIndexer
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
}
