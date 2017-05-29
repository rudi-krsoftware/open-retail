/**
 * Copyright (C) 2017 Kamarudin (http://coding4ever.net/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * The latest version of this file can be found at https://github.com/rudi-krsoftware/open-retail
 */

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

        private static bool IsSectionExist(string sectionName, AppSettingsSection appSetting)
        {
            var keyCount = appSetting.Settings.AllKeys
                                     .Where(key => key == sectionName).Count();

            return keyCount > 0;
        }

        public static string GetValue(string sectionName, string appConfigFile)
        {
            var configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = appConfigFile;

            var configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            var section = (AppSettingsSection)configuration.GetSection(SECTION_NAME);

            var result = string.Empty;

            try
            {
                if (IsSectionExist(sectionName, section))
                    result = section.Settings[sectionName].Value;
            }
            catch
            {
            }

            return result ;
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
