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
    public class KabupatenBll : IKabupatenBll
    {
		private ILog _log;
		private KabupatenValidator _validator;

        public KabupatenBll()
        {
            _validator = new KabupatenValidator();
        }

		public KabupatenBll(ILog log)
        {
			_log = log;
            _validator = new KabupatenValidator();
        }

        public Kabupaten GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Kabupaten> GetByName(string name)
        {
            IList<Kabupaten> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.KabupatenRepository.GetByName(name);
            }

            return oList;
        }

        public IList<Kabupaten> GetAll()
        {
            IList<Kabupaten> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.KabupatenRepository.GetAll();
            }

            return oList;
        }

		public int Save(Kabupaten obj)
        {
            throw new NotImplementedException();
        }

        public int Save(Kabupaten obj, ref ValidationError validationError)
        {
            throw new NotImplementedException();
        }

		public int Update(Kabupaten obj)
        {
            throw new NotImplementedException();
        }

        public int Update(Kabupaten obj, ref ValidationError validationError)
        {
            throw new NotImplementedException();
        }

        public int Delete(Kabupaten obj)
        {
            throw new NotImplementedException();
        }
    }
}     
