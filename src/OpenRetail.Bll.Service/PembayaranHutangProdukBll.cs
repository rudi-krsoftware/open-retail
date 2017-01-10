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

using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;
 
namespace OpenRetail.Bll.Service
{    
    public class PembayaranHutangProdukBll : IPembayaranHutangProdukBll
    {
		private PembayaranHutangProdukValidator _validator;

		public PembayaranHutangProdukBll()
        {
            _validator = new PembayaranHutangProdukValidator();
        }

        public PembayaranHutangProduk GetByID(string id)
        {
            PembayaranHutangProduk obj = null;
            
            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context);
                obj = uow.PembayaranHutangProdukRepository.GetByID(id);
            }

            return obj;
        }

        public IList<PembayaranHutangProduk> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<PembayaranHutangProduk> GetAll()
        {
            IList<PembayaranHutangProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context);
                oList = uow.PembayaranHutangProdukRepository.GetAll();
            }

            return oList;
        }

		public int Save(PembayaranHutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Save(PembayaranHutangProduk obj, bool isSaveFromPembelian, ref ValidationError validationError)
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
                IUnitOfWork uow = new UnitOfWork(context);
                result = uow.PembayaranHutangProdukRepository.Save(obj, isSaveFromPembelian);
            }

            return result;
        }

		public int Update(PembayaranHutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Update(PembayaranHutangProduk obj, bool isUpdateFromPembelian, ref ValidationError validationError)
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
                IUnitOfWork uow = new UnitOfWork(context);
                result = uow.PembayaranHutangProdukRepository.Update(obj, isUpdateFromPembelian);
            }

            return result;
        }

        public int Delete(PembayaranHutangProduk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context);
                result = uow.PembayaranHutangProdukRepository.Delete(obj);
            }

            return result;
        }

        public string GetLastNota()
        {
            var lastNota = string.Empty;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context);
                lastNota = uow.PembayaranHutangProdukRepository.GetLastNota();
            }

            return lastNota;
        }

        public ItemPembayaranHutangProduk GetByBeliID(string id)
        {
            ItemPembayaranHutangProduk obj = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context);
                obj = uow.PembayaranHutangProdukRepository.GetByBeliID(id);
            }

            return obj;
        }

        public IList<PembayaranHutangProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PembayaranHutangProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context);
                oList = uow.PembayaranHutangProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }
    }
}     
