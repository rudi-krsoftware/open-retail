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
    public class PembayaranKasbonBll : IPembayaranKasbonBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;
        private PembayaranKasbonValidator _validator;

        private bool _isUseWebAPI;
        private string _baseUrl;

        public PembayaranKasbonBll(ILog log)
        {
            _log = log;
            _validator = new PembayaranKasbonValidator();
        }

        public PembayaranKasbonBll(bool isUseWebAPI, string baseUrl, ILog log)
            : this(log)
        {
            _isUseWebAPI = isUseWebAPI;
            _baseUrl = baseUrl;
        }

        public string GetLastNota()
        {
            var lastNota = string.Empty;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                lastNota = _unitOfWork.PembayaranKasbonRepository.GetLastNota();
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    lastNota = _unitOfWork.PembayaranKasbonRepository.GetLastNota();
                }
            }

            return lastNota;
        }

        public PembayaranKasbon GetByID(string id)
        {
            PembayaranKasbon obj = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                obj = _unitOfWork.PembayaranKasbonRepository.GetByID(id);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    obj = _unitOfWork.PembayaranKasbonRepository.GetByID(id);
                }
            }

            return obj;
        }

        public IList<PembayaranKasbon> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<PembayaranKasbon> GetByKasbonId(string kasbonId)
        {
            IList<PembayaranKasbon> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.PembayaranKasbonRepository.GetByKasbonId(kasbonId);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.PembayaranKasbonRepository.GetByKasbonId(kasbonId);
                }
            }

            return oList;
        }

        public IList<PembayaranKasbon> GetByGajiKaryawan(string gajiKaryawanId)
        {
            IList<PembayaranKasbon> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.PembayaranKasbonRepository.GetByGajiKaryawan(gajiKaryawanId);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.PembayaranKasbonRepository.GetByGajiKaryawan(gajiKaryawanId);
                }
            }

            return oList;
        }

        public IList<PembayaranKasbon> GetAll()
        {
            throw new NotImplementedException();
        }

		public int Save(PembayaranKasbon obj)
        {
            var result = 0;

            if (_isUseWebAPI)
            {
				obj.pembayaran_kasbon_id = Guid.NewGuid().ToString();
            
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.PembayaranKasbonRepository.Save(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
				{
					_unitOfWork = new UnitOfWork(context, _log);
					result = _unitOfWork.PembayaranKasbonRepository.Save(obj);
				}
            }

            return result;
        }

        public int Save(PembayaranKasbon obj, ref ValidationError validationError)
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

		public int Update(PembayaranKasbon obj)
        {
            var result = 0;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.PembayaranKasbonRepository.Update(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.PembayaranKasbonRepository.Update(obj);
                }
            }

            return result;
        }

        public int Update(PembayaranKasbon obj, ref ValidationError validationError)
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

        public int Delete(PembayaranKasbon obj)
        {
            var result = 0;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.PembayaranKasbonRepository.Delete(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.PembayaranKasbonRepository.Delete(obj);
                }
            }

            return result;
        }        
    }
}     
