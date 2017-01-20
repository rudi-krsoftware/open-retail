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
    public class JualProdukBll : IJualProdukBll
    {
		private ILog _log;
		private JualProdukValidator _validator;

		public JualProdukBll(ILog log)
        {
			_log = log;
            _validator = new JualProdukValidator();
        }

        public JualProduk GetByID(string id)
        {
            JualProduk obj = null;
            
            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                obj = uow.JualProdukRepository.GetByID(id);
            }

            return obj;
        }

        public IList<JualProduk> GetByName(string name)
        {
            IList<JualProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.JualProdukRepository.GetByName(name);
            }

            return oList;
        }

        public IList<JualProduk> GetAll()
        {
            IList<JualProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.JualProdukRepository.GetAll();
            }

            return oList;
        }

		public int Save(JualProduk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.JualProdukRepository.Save(obj);
            }

            return result;
        }

        public int Save(JualProduk obj, ref ValidationError validationError)
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

		public int Update(JualProduk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.JualProdukRepository.Update(obj);
            }

            return result;
        }

        public int Update(JualProduk obj, ref ValidationError validationError)
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

        public int Delete(JualProduk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.JualProdukRepository.Delete(obj);
            }

            return result;
        }

        public string GetLastNota()
        {
            var lastNota = string.Empty;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                lastNota = uow.JualProdukRepository.GetLastNota();
            }

            return lastNota;
        }

        public IList<JualProduk> GetAll(string name)
        {
            IList<JualProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.JualProdukRepository.GetAll(name);
            }

            return oList;
        }

        public IList<JualProduk> GetNotaCustomer(string id, string nota)
        {
            IList<JualProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.JualProdukRepository.GetNotaCustomer(id, nota);
            }

            return oList;
        }

        public IList<JualProduk> GetNotaKreditByCustomer(string id, bool isLunas)
        {
            IList<JualProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.JualProdukRepository.GetNotaKreditByCustomer(id, isLunas);
            }

            return oList;
        }

        public IList<JualProduk> GetNotaKreditByNota(string id, string nota)
        {
            IList<JualProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.JualProdukRepository.GetNotaKreditByNota(id, nota);
            }

            return oList;
        }

        public IList<JualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<JualProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.JualProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }

        public IList<JualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, string name)
        {
            IList<JualProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.JualProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai, name);
            }

            return oList;
        }

        public IList<ItemJualProduk> GetItemJual(string jualId)
        {
            IList<ItemJualProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.JualProdukRepository.GetItemJual(jualId);
            }

            return oList;
        }        
    }
}     
