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
    public class BeliProdukBll : IBeliProdukBll
    {
        private ILog _log;
		private BeliProdukValidator _validator;

		public BeliProdukBll(ILog log)
        {
            _log = log;
            _validator = new BeliProdukValidator();
        }

        public BeliProduk GetByID(string id)
        {
            BeliProduk obj = null;
            
            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                obj = uow.BeliProdukRepository.GetByID(id);
            }

            return obj;
        }

        public IList<BeliProduk> GetByName(string name)
        {
            IList<BeliProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.BeliProdukRepository.GetByName(name);
            }

            return oList;
        }

        public IList<BeliProduk> GetAll()
        {
            IList<BeliProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.BeliProdukRepository.GetAll();
            }

            return oList;
        }

		public int Save(BeliProduk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.BeliProdukRepository.Save(obj);
            }

            return result;
        }

        public int Save(BeliProduk obj, ref ValidationError validationError)
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

		public int Update(BeliProduk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.BeliProdukRepository.Update(obj);
            }

            return result;
        }

        public int Update(BeliProduk obj, ref ValidationError validationError)
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

        public int Delete(BeliProduk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.BeliProdukRepository.Delete(obj);
            }

            return result;
        }

        public string GetLastNota()
        {
            var lastNota = string.Empty;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                lastNota = uow.BeliProdukRepository.GetLastNota();
            }

            return lastNota;
        }

        public IList<BeliProduk> GetAll(string name)
        {
            IList<BeliProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.BeliProdukRepository.GetAll(name);
            }

            return oList;
        }

        public IList<BeliProduk> GetNotaSupplier(string id, string nota)
        {
            IList<BeliProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.BeliProdukRepository.GetNotaSupplier(id, nota);
            }

            return oList;
        }

        public IList<BeliProduk> GetNotaKreditBySupplier(string id, bool isLunas)
        {
            IList<BeliProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.BeliProdukRepository.GetNotaKreditBySupplier(id, isLunas);
            }

            return oList;
        }

        public IList<BeliProduk> GetNotaKreditByNota(string id, string nota)
        {
            IList<BeliProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.BeliProdukRepository.GetNotaKreditByNota(id, nota);
            }

            return oList;
        }

        public IList<BeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<BeliProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.BeliProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }

        public IList<BeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, string name)
        {
            IList<BeliProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.BeliProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai, name);
            }

            return oList;
        }

        public IList<ItemBeliProduk> GetItemBeli(string beliId)
        {
            IList<ItemBeliProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.BeliProdukRepository.GetItemBeli(beliId);
            }

            return oList;
        }        
    }
}     
