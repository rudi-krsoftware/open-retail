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
    public class MenuRepository : IMenuRepository
    {
        private IDapperContext _context;
		private ILog _log;
		
        public MenuRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public MenuAplikasi GetByID(string id)
        {
            MenuAplikasi obj = null;

            try
            {
                obj = _context.db.Get<MenuAplikasi>(id);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public MenuAplikasi GetByName(string name)
        {
            MenuAplikasi obj = null;

            try
            {
                obj = _context.db.GetAll<MenuAplikasi>()
                              .Where(f => f.nama_menu.ToLower() == name.ToLower())
                              .SingleOrDefault();
            }
            catch
            {
            }

            return obj;
        }

        public IList<MenuAplikasi> GetAll()
        {
            IList<MenuAplikasi> oList = new List<MenuAplikasi>();

            try
            {
                oList = _context.db.GetAll<MenuAplikasi>()
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(MenuAplikasi obj)
        {
            var result = 0;

            try
            {
                obj.menu_id = _context.GetGUID();

                _context.db.Insert<MenuAplikasi>(obj);
                result = 1;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Update(MenuAplikasi obj)
        {
            throw new NotImplementedException();
        }

        public int Delete(MenuAplikasi obj)
        {
            throw new NotImplementedException();
        }
    }
}     
