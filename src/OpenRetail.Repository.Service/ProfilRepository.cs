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
    public class ProfilRepository : IProfilRepository
    {
        private IDapperContext _context;
		private ILog _log;
		
        public ProfilRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public Profil GetByID(string id)
        {
            throw new NotImplementedException();
        }

        public Profil GetProfil()
        {
            Profil obj = null;

            try
            {
                obj = _context.db.GetAll<Profil>()
                              .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<Profil> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Save(Profil obj)
        {
            var result = 0;

            try
            {
                var profil = GetProfil();

                if (profil == null)
                {
                    obj.profil_id = _context.GetGUID();
                    _context.db.Insert<Profil>(obj);

                    result = 1;
                }
                else
                    result = _context.db.Update<Profil>(obj) ? 1 : 0;                
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Update(Profil obj)
        {
            throw new NotImplementedException();
        }

        public int Delete(Profil obj)
        {
            throw new NotImplementedException();
        }        
    }
}     
