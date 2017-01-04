using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
