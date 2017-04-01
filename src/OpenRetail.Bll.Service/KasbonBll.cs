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
    public class KasbonBll : IKasbonBll
    {
		private ILog _log;
		private KasbonValidator _validator;

		public KasbonBll(ILog log)
        {
			_log = log;
            _validator = new KasbonValidator();
        }

        public Kasbon GetByID(string id)
        {
            Kasbon obj = null;
            
            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                obj = uow.KasbonRepository.GetByID(id);
            }

            return obj;
        }

        public IList<Kasbon> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<Kasbon> GetByStatus(bool isLunas)
        {
            IList<Kasbon> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.KasbonRepository.GetByStatus(isLunas);
            }

            return oList;
        }

        public IList<Kasbon> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<Kasbon> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.KasbonRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }

        public IList<Kasbon> GetByKaryawanId(string karyawanId)
        {
            IList<Kasbon> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.KasbonRepository.GetByKaryawanId(karyawanId);
            }

            return oList;
        }

        public IList<Kasbon> GetAll()
        {
            IList<Kasbon> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.KasbonRepository.GetAll();
            }

            return oList;
        }

		public int Save(Kasbon obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.KasbonRepository.Save(obj);
            }

            return result;
        }

        public int Save(Kasbon obj, ref ValidationError validationError)
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

            return Save(obj);
        }

		public int Update(Kasbon obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.KasbonRepository.Update(obj);
            }

            return result;
        }

        public int Update(Kasbon obj, ref ValidationError validationError)
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

            return Update(obj);
        }

        public int Delete(Kasbon obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.KasbonRepository.Delete(obj);
            }

            return result;
        }

        public string GetLastNota()
        {
            var lastNota = string.Empty;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                lastNota = uow.KasbonRepository.GetLastNota();
            }

            return lastNota;
        }        
    }
}     
