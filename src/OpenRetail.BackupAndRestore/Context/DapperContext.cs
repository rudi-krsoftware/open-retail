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
using System.Data;
using System.Configuration;
using System.Data.Common;

namespace OpenRetail.BackupAndRestore.Context
{
    public interface IDapperContext : IDisposable
    {
        IDbConnection db { get; }
        bool IsOpenConnection();
    }

    public class DapperContext : IDapperContext
    {
        private IDbConnection _db;

        private readonly string _providerName;
        private readonly string _connectionString;

        public DapperContext(string pgPassword)
        {
            var server = ConfigurationManager.AppSettings["server"];
            var port = ConfigurationManager.AppSettings["port"];
            var pgUser = "postgres";

            _providerName = "Npgsql";
            _connectionString = string.Format("Server={0};Port={1};User Id={2};Password={3};", server, port, pgUser, pgPassword);

            if (_db == null)
            {
                _db = GetOpenConnection(_providerName, _connectionString);
            }
        }

        public bool IsOpenConnection()
        {
            var isConnected = false;

            try
            {
                using (var conn = GetOpenConnection(_providerName, _connectionString))
                {
                    isConnected = conn.State == ConnectionState.Open;
                }
            }
            catch
            {
            }

            return isConnected;
        }

        private IDbConnection GetOpenConnection(string providerName, string connectionString)
        {
            DbConnection conn = null;

            try
            {
                var provider = DbProviderFactories.GetFactory(providerName);
                conn = provider.CreateConnection();
                conn.ConnectionString = connectionString;
                conn.Open();
            }
            catch
            {
            }

            return conn;
        }

        public IDbConnection db
        {
            get { return _db ?? (_db = GetOpenConnection(_providerName, _connectionString)); }
        }

        public void Dispose()
        {
            if (_db != null)
            {
                try
                {
                    if (_db.State != ConnectionState.Closed)
                        _db.Close();
                }
                finally
                {
                    _db.Dispose();
                }
            }

            GC.SuppressFinalize(this);
        }
    }
}
