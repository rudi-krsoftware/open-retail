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
    public class PengeluaranBiayaBll : IPengeluaranBiayaBll
    {
		private ILog _log;
		private PengeluaranBiayaValidator _validator;

		public PengeluaranBiayaBll(ILog log)
        {
			_log = log;
            _validator = new PengeluaranBiayaValidator();
        }

        public IList<ItemPengeluaranBiaya> GetItemPengeluaranBiaya(string pengeluaranBiayaId)
        {
            IList<ItemPengeluaranBiaya> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.PengeluaranBiayaRepository.GetItemPengeluaranBiaya(pengeluaranBiayaId);
            }

            return oList;
        }

        public PengeluaranBiaya GetByID(string id)
        {
            PengeluaranBiaya obj = null;
            
            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                obj = uow.PengeluaranBiayaRepository.GetByID(id);
            }

            return obj;
        }

        public IList<PengeluaranBiaya> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<PengeluaranBiaya> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PengeluaranBiaya> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.PengeluaranBiayaRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }        

        public IList<PengeluaranBiaya> GetAll()
        {
            IList<PengeluaranBiaya> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.PengeluaranBiayaRepository.GetAll();
            }

            return oList;
        }

		public int Save(PengeluaranBiaya obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PengeluaranBiayaRepository.Save(obj);
            }

            return result;
        }

        public int Save(PengeluaranBiaya obj, ref ValidationError validationError)
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

		public int Update(PengeluaranBiaya obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PengeluaranBiayaRepository.Update(obj);
            }

            return result;
        }

        public int Update(PengeluaranBiaya obj, ref ValidationError validationError)
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

        public int Delete(PengeluaranBiaya obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PengeluaranBiayaRepository.Delete(obj);
            }

            return result;
        }

        public string GetLastNota()
        {
            var lastNota = string.Empty;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                lastNota = uow.PengeluaranBiayaRepository.GetLastNota();
            }

            return lastNota;
        }
    }
}     
