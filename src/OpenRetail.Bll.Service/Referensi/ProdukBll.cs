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
    public class ProdukBll : IProdukBll
    {
        private ILog _log;
		private ProdukValidator _validator;

		public ProdukBll(ILog log)
        {
            _log = log;
            _validator = new ProdukValidator();
        }

        public Produk GetByID(string id)
        {
            Produk obj = null;
            
            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                obj = uow.ProdukRepository.GetByID(id);
            }

            return obj;
        }

        public Produk GetByKode(string kodeProduk)
        {
            Produk obj = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                obj = uow.ProdukRepository.GetByKode(kodeProduk);
            }

            return obj;
        }        

        public string GetLastKodeProduk()
        {
            var lastNota = string.Empty;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                lastNota = uow.ProdukRepository.GetLastKodeProduk();
            }

            return lastNota;
        }

        public IList<Produk> GetByName(string name)
        {
            IList<Produk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ProdukRepository.GetByName(name);
            }

            return oList;
        }

        private string GetSortByFieldName(int sortByIndex)
        {
            return sortByIndex == 0 ? "m_produk.kode_produk" : "m_produk.nama_produk";
        }

        public IList<Produk> GetByName(string name, int sortByIndex, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<Produk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ProdukRepository.GetByName(name, GetSortByFieldName(sortByIndex), pageNumber, pageSize, ref pagesCount);
            }

            return oList;
        }

        public IList<Produk> GetByGolongan(string golonganId)
        {
            IList<Produk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ProdukRepository.GetByGolongan(golonganId);
            }

            return oList;
        }

        public IList<Produk> GetByGolongan(string golonganId, int sortByIndex, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<Produk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ProdukRepository.GetByGolongan(golonganId, GetSortByFieldName(sortByIndex), pageNumber, pageSize, ref pagesCount);
            }

            return oList;
        }

        public IList<Produk> GetInfoMinimalStok()
        {
            IList<Produk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ProdukRepository.GetInfoMinimalStok();
            }

            return oList;
        }

        public IList<Produk> GetAll()
        {
            IList<Produk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ProdukRepository.GetAll();
            }

            return oList;
        }

        public IList<Produk> GetAll(int sortByIndex)
        {
            IList<Produk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ProdukRepository.GetAll(GetSortByFieldName(sortByIndex));
            }

            return oList;
        }

        public IList<Produk> GetAll(int sortByIndex, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<Produk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ProdukRepository.GetAll(GetSortByFieldName(sortByIndex), pageNumber, pageSize, ref pagesCount);
            }

            return oList;
        }

		public int Save(Produk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.ProdukRepository.Save(obj);
            }

            return result;
        }

        public int Save(Produk obj, ref ValidationError validationError)
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

		public int Update(Produk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.ProdukRepository.Update(obj);
            }

            return result;
        }

        public int Update(Produk obj, ref ValidationError validationError)
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

        public int Delete(Produk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.ProdukRepository.Delete(obj);
            }

            return result;
        }        
    }
}     
