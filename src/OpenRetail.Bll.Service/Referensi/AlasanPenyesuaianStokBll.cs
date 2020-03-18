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
    public class AlasanPenyesuaianStokBll : IAlasanPenyesuaianStokBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;
        private AlasanPenyesuaianStokValidator _validator;

        private bool _isUseWebAPI;
        private string _baseUrl;

        public AlasanPenyesuaianStokBll(ILog log)
        {
            _log = log;
            _validator = new AlasanPenyesuaianStokValidator();
        }

        public AlasanPenyesuaianStokBll(bool isUseWebAPI, string baseUrl, ILog log)
            : this(log)
        {
            _isUseWebAPI = isUseWebAPI;
            _baseUrl = baseUrl;
        }

        public AlasanPenyesuaianStok GetByID(string id)
        {
            AlasanPenyesuaianStok obj = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                obj = _unitOfWork.AlasanPenyesuaianStokRepository.GetByID(id);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    obj = _unitOfWork.AlasanPenyesuaianStokRepository.GetByID(id);
                }
            }

            return obj;
        }

        public IList<AlasanPenyesuaianStok> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<AlasanPenyesuaianStok> GetAll()
        {
            IList<AlasanPenyesuaianStok> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.AlasanPenyesuaianStokRepository.GetAll();
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.AlasanPenyesuaianStokRepository.GetAll();
                }
            }

            return oList;
        }

        public int Save(AlasanPenyesuaianStok obj)
        {
            var result = 0;

            if (_isUseWebAPI)
            {
                obj.alasan_penyesuaian_stok_id = Guid.NewGuid().ToString();

                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.AlasanPenyesuaianStokRepository.Save(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.AlasanPenyesuaianStokRepository.Save(obj);
                }
            }

            return result;
        }

        public int Save(AlasanPenyesuaianStok obj, ref ValidationError validationError)
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

        public int Update(AlasanPenyesuaianStok obj)
        {
            var result = 0;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.AlasanPenyesuaianStokRepository.Update(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.AlasanPenyesuaianStokRepository.Update(obj);
                }
            }

            return result;
        }

        public int Update(AlasanPenyesuaianStok obj, ref ValidationError validationError)
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

        public int Delete(AlasanPenyesuaianStok obj)
        {
            var result = 0;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.AlasanPenyesuaianStokRepository.Delete(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.AlasanPenyesuaianStokRepository.Delete(obj);
                }
            }

            return result;
        }
    }
}