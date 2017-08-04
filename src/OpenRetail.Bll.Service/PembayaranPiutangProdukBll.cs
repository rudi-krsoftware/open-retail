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
    public class PembayaranPiutangProdukBll : IPembayaranPiutangProdukBll
    {
		private ILog _log;
		private PembayaranPiutangProdukValidator _validator;

		public PembayaranPiutangProdukBll(ILog log)
        {
			_log = log;
            _validator = new PembayaranPiutangProdukValidator();
        }

        public PembayaranPiutangProduk GetByID(string id)
        {
            PembayaranPiutangProduk obj = null;
            
            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                obj = uow.PembayaranPiutangProdukRepository.GetByID(id);
            }

            return obj;
        }

        public string GetLastNota()
        {
            var lastNota = string.Empty;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                lastNota = uow.PembayaranPiutangProdukRepository.GetLastNota();
            }

            return lastNota;
        }

        public ItemPembayaranPiutangProduk GetByJualID(string id)
        {
            ItemPembayaranPiutangProduk obj = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                obj = uow.PembayaranPiutangProdukRepository.GetByJualID(id);
            }

            return obj;
        }

        public IList<PembayaranPiutangProduk> GetByName(string name)
        {
            IList<PembayaranPiutangProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.PembayaranPiutangProdukRepository.GetByName(name);
            }

            return oList;
        }        

        public IList<PembayaranPiutangProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PembayaranPiutangProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.PembayaranPiutangProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }

        public IList<PembayaranPiutangProduk> GetAll()
        {
            IList<PembayaranPiutangProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.PembayaranPiutangProdukRepository.GetAll();
            }

            return oList;
        }

		public int Save(PembayaranPiutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Save(PembayaranPiutangProduk obj, bool isSaveFromPenjualan, ref ValidationError validationError)
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

            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PembayaranPiutangProdukRepository.Save(obj, isSaveFromPenjualan);
            }

            return result;
        }

		public int Update(PembayaranPiutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Update(PembayaranPiutangProduk obj, bool isSaveFromPenjualan, ref ValidationError validationError)
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

            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PembayaranPiutangProdukRepository.Update(obj, isSaveFromPenjualan);
            }

            return result;
        }

        public int Delete(PembayaranPiutangProduk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PembayaranPiutangProdukRepository.Delete(obj);
            }

            return result;
        }        
    }
}     
