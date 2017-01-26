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

using Newtonsoft.Json;
using Dapper.Contrib.Extensions;

namespace OpenRetail.Model
{
    public static class ModelExtension
    {
        /// <summary>
        /// Method untuk mendapatkan nama tabel dari class model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetTableName<T>(this T obj)
        {
            var tableName = string.Empty;
            var type = typeof(T);

            // Get instance of the attribute.
            var tableAttribute = (TableAttribute)Attribute.GetCustomAttribute(type, typeof(TableAttribute));

            if (!(tableAttribute == null))
                tableName = tableAttribute.Name;

            return tableName;
        }

        /// <summary>
        /// Method untuk mengecek apakah sebuah tanggal null atau tidak
        /// </summary>
        /// <param name="tanggal"></param>
        /// <returns></returns>
        public static bool IsNull(this Nullable<DateTime> tanggal)
        {
            var result = true;

            try
            {
                result = tanggal == DateTime.MinValue || tanggal == new DateTime(1753, 1, 1) ||
                         tanggal == new DateTime(0001, 1, 1) || tanggal == null;
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Method untuk mengkonversi nilai object ke format json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
