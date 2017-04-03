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
using System.Threading.Tasks;

using log4net;
using Dapper;
using Dapper.Contrib.Extensions;

using OpenRetail.Model;
using OpenRetail.Repository.Api;
 
namespace OpenRetail.Repository.Service
{        
    public class DatabaseVersionRepository : IDatabaseVersionRepository
    {
        private IDapperContext _context;
		private ILog _log;
		
        public DatabaseVersionRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public DatabaseVersion Get()
        {
            DatabaseVersion obj = null;

            try
            {
                obj = _context.db.GetAll<DatabaseVersion>()
                              .SingleOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public int UpdateVersion()
        {
            var result = 0;

            try
            {
                var sql = @"UPDATE m_database_version SET version_number = version_number + 1";

                result = _context.db.Execute(sql);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public IList<DatabaseVersion> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Save(DatabaseVersion obj)
        {
            throw new NotImplementedException();
        }

        public int Update(DatabaseVersion obj)
        {
            throw new NotImplementedException();
        }

        public int Delete(DatabaseVersion obj)
        {
            throw new NotImplementedException();
        }                
    }
}     
