using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace OpenRetail.App.Helper
{
    public static class AppConfigHelper
    {
        private const string SECTION_NAME = "appSettings";

        public static string GetValue(string sectionName, string appConfigFile)
        {
            var configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = appConfigFile;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            var section = (AppSettingsSection)configuration.GetSection(SECTION_NAME);

            var result = string.Empty;

            try
            {
                result = section.Settings[sectionName].Value;
            }
            catch
            {
            }

            return result ;
        }

        private static bool IsSectionExist(string sectionName, AppSettingsSection appSetting)
        {
            var keyCount = appSetting.Settings.AllKeys
                                     .Where(key => key == sectionName).Count();

            return keyCount > 0;
        }

        public static void SaveValue(string sectionName, string value, string appConfigFile)
        {
            var configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = appConfigFile;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            var section = (AppSettingsSection)configuration.GetSection(SECTION_NAME);

            try
            {
                if (IsSectionExist(sectionName, section))
                {
                    section.Settings[sectionName].Value = value;
                }
                else
                {
                    section.Settings.Add(sectionName, value);
                }

                configuration.Save(ConfigurationSaveMode.Modified, false);
                ConfigurationManager.RefreshSection(SECTION_NAME);
            }
            catch
            {
            }
        }
    }
}
