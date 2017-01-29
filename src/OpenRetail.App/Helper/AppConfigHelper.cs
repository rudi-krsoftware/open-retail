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

            return section.Settings[sectionName].Value;
        }

        public static void SaveValue(string sectionName, string value, string appConfigFile)
        {
            var configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = appConfigFile;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            var section = (AppSettingsSection)configuration.GetSection(SECTION_NAME);

            section.Settings[sectionName].Value = value;
            configuration.Save(ConfigurationSaveMode.Modified, false);

            ConfigurationManager.RefreshSection(SECTION_NAME);
        }
    }
}
