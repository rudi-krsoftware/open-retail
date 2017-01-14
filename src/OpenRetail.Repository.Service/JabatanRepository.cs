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
using Dapper.Contrib.Extensions;

using OpenRetail.Model;
using OpenRetail.Repository.Api;
 
namespace OpenRetail.Repository.Service
{        
    public class JabatanRepository : IJabatanRepository
    {
        private IDapperContext _context;
        private ILog _log;

        public JabatanRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public Jabatan GetByID(string id)
        {
            Jabatan obj = null;

            try
            {
                obj = _context.db.Get<Jabatan>(id);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<Jabatan> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<Jabatan> GetAll()
        {
            IList<Jabatan> oList = new List<Jabatan>();

            try
            {
                oList = _context.db.GetAll<Jabatan>()
                                .OrderBy(f => f.nama_jabatan)
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(Jabatan obj)
        {
            var result = 0;

            try
            {
                obj.jabatan_id = _context.GetGUID();

                _context.db.Insert<Jabatan>(obj);
                result = 1;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Update(Jabatan obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Update<Jabatan>(obj) ? 1 : 0;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Delete(Jabatan obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<Jabatan>(obj) ? 1 : 0;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }
    }
}     
