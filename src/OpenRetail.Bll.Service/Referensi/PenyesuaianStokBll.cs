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
    public class PenyesuaianStokBll : IPenyesuaianStokBll
    {
		private ILog _log;
		private PenyesuaianStokValidator _validator;

		public PenyesuaianStokBll(ILog log)
        {
			_log = log;
            _validator = new PenyesuaianStokValidator();
        }

        public PenyesuaianStok GetByID(string id)
        {
            PenyesuaianStok obj = null;
            
            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                obj = uow.PenyesuaianStokRepository.GetByID(id);
            }

            return obj;
        }

        public IList<PenyesuaianStok> GetByName(string name)
        {
            IList<PenyesuaianStok> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.PenyesuaianStokRepository.GetByName(name);
            }

            return oList;
        }

        public IList<PenyesuaianStok> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PenyesuaianStok> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.PenyesuaianStokRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }

        public IList<PenyesuaianStok> GetAll()
        {
            IList<PenyesuaianStok> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.PenyesuaianStokRepository.GetAll();
            }

            return oList;
        }

		public int Save(PenyesuaianStok obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PenyesuaianStokRepository.Save(obj);
            }

            return result;
        }

        public int Save(PenyesuaianStok obj, ref ValidationError validationError)
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

		public int Update(PenyesuaianStok obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PenyesuaianStokRepository.Update(obj);
            }

            return result;
        }

        public int Update(PenyesuaianStok obj, ref ValidationError validationError)
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

        public int Delete(PenyesuaianStok obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                result = uow.PenyesuaianStokRepository.Delete(obj);
            }

            return result;
        }        
    }
}     
