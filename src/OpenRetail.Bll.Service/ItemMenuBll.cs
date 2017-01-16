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
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;
 
namespace OpenRetail.Bll.Service
{    
    public class ItemMenuBll : IItemMenuBll
    {
		private ILog _log;

		public ItemMenuBll(ILog log)
        {
			_log = log;
        }

        public ItemMenu GetByID(string id)
        {
            ItemMenu obj = null;
            
            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                obj = uow.ItemMenuRepository.GetByID(id);
            }

            return obj;
        }

        public IList<ItemMenu> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<ItemMenu> GetByMenu(string menuId)
        {
            IList<ItemMenu> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ItemMenuRepository.GetByMenu(menuId);
            }

            return oList;
        }

        public IList<ItemMenu> GetAll()
        {
            IList<ItemMenu> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ItemMenuRepository.GetAll();
            }

            return oList;
        }

		public int Save(ItemMenu obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.ItemMenuRepository.Save(obj);
            }

            return result;
        }

		public int Update(ItemMenu obj)
        {
            throw new NotImplementedException();
        }

        public int Delete(ItemMenu obj)
        {
            throw new NotImplementedException();
        }        
    }
}     
