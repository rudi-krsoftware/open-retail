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

using log4net;
using OpenRetail.Bll.Api;
using OpenRetail.Model;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;
using System;
using System.Collections.Generic;

namespace OpenRetail.Bll.Service
{
    public class ReturBeliProdukBll : IReturBeliProdukBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;
        private ReturBeliProdukValidator _validator;

        public ReturBeliProdukBll(ILog log)
        {
            _log = log;
            _validator = new ReturBeliProdukValidator();
        }

        public ReturBeliProduk GetByID(string id)
        {
            ReturBeliProduk obj = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                obj = _unitOfWork.ReturBeliProdukRepository.GetByID(id);
            }

            return obj;
        }

        public string GetLastNota()
        {
            var lastNota = string.Empty;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                lastNota = _unitOfWork.ReturBeliProdukRepository.GetLastNota();
            }

            return lastNota;
        }

        public IList<ReturBeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReturBeliProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReturBeliProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }

        public IList<ReturBeliProduk> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<ReturBeliProduk> GetAll()
        {
            IList<ReturBeliProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReturBeliProdukRepository.GetAll();
            }

            return oList;
        }

        public int Save(ReturBeliProduk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                result = _unitOfWork.ReturBeliProdukRepository.Save(obj);
            }

            return result;
        }

        public int Save(ReturBeliProduk obj, ref ValidationError validationError)
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

        public int Update(ReturBeliProduk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                result = _unitOfWork.ReturBeliProdukRepository.Update(obj);
            }

            return result;
        }

        public int Update(ReturBeliProduk obj, ref ValidationError validationError)
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

        public int Delete(ReturBeliProduk obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                result = _unitOfWork.ReturBeliProdukRepository.Delete(obj);
            }

            return result;
        }
    }
}