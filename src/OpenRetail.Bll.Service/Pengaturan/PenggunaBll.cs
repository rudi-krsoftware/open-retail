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
    public class PenggunaBll : IPenggunaBll
    {
		private ILog _log;
		private PenggunaValidator _validator;

		public PenggunaBll(ILog log)
        {
			_log = log;
            _validator = new PenggunaValidator();
        }

        public Pengguna GetByID(string userName)
        {
            Pengguna obj = null;
            
            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                obj = uow.PenggunaRepository.GetByID(userName);
            }

            return obj;
        }

        public bool IsValidPengguna(string userName, string password)
        {
            var result = false;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PenggunaRepository.IsValidPengguna(userName, password);
            }

            return result;
        }

        public IList<Pengguna> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<Pengguna> GetAll()
        {
            IList<Pengguna> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.PenggunaRepository.GetAll();
            }

            return oList;
        }

		public int Save(Pengguna obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PenggunaRepository.Save(obj);
            }

            return result;
        }

        public int Save(Pengguna obj, ref ValidationError validationError)
        {
			var validatorResults = _validator.Validate(obj);

            if (!validatorResults.IsValid)
            {
                foreach (var failure in validatorResults.Errors)
                {
                    validationError.Message = failure.ErrorMessage;
                    validationError.PropertyName = failure.PropertyName;
                    return 0;
                }
            }

            obj.pass_pengguna = obj.konf_pass_pengguna;
            return Save(obj);
        }

		public int Update(Pengguna obj)
        {
            var result = 0;

            obj.pass_pengguna = obj.konf_pass_pengguna;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PenggunaRepository.Update(obj);
            }

            return result;
        }

        public int Delete(Pengguna obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PenggunaRepository.Delete(obj);
            }

            return result;
        }        
    }
}     
