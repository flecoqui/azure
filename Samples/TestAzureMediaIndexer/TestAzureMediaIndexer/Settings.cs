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
