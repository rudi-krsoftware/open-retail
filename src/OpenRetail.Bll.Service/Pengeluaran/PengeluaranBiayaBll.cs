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
using System.Linq;

namespace OpenRetail.Bll.Service
{
    public class PengeluaranBiayaBll : IPengeluaranBiayaBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;
        private PengeluaranBiayaValidator _validator;

        private bool _isUseWebAPI;
        private string _baseUrl;

        public PengeluaranBiayaBll(ILog log)
        {
            _log = log;
            _validator = new PengeluaranBiayaValidator();
        }

        public PengeluaranBiayaBll(bool isUseWebAPI, string baseUrl, ILog log)
            : this(log)
        {
            _isUseWebAPI = isUseWebAPI;
            _baseUrl = baseUrl;
        }

        public IList<ItemPengeluaranBiaya> GetItemPengeluaranBiaya(string pengeluaranBiayaId)
        {
            throw new NotImplementedException();
        }

        public PengeluaranBiaya GetByID(string id)
        {
            PengeluaranBiaya obj = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                obj = _unitOfWork.PengeluaranBiayaRepository.GetByID(id);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    obj = _unitOfWork.PengeluaranBiayaRepository.GetByID(id);
                }
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

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.PengeluaranBiayaRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.PengeluaranBiayaRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
                }
            }

            return oList;
        }

        public IList<PengeluaranBiaya> GetAll()
        {
            IList<PengeluaranBiaya> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.PengeluaranBiayaRepository.GetAll();
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.PengeluaranBiayaRepository.GetAll();
                }
            }

            return oList;
        }

        public int Save(PengeluaranBiaya obj)
        {
            var result = 0;

            if (_isUseWebAPI)
            {
                obj.pengeluaran_id = Guid.NewGuid().ToString();

                foreach (var item in obj.item_pengeluaran_biaya)
                {
                    item.item_pengeluaran_id = Guid.NewGuid().ToString();
                }

                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.PengeluaranBiayaRepository.Save(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.PengeluaranBiayaRepository.Save(obj);
                }
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

            if (_isUseWebAPI)
            {
                foreach (var item in obj.item_pengeluaran_biaya.Where(f => f.entity_state == EntityState.Added))
                {
                    item.item_pengeluaran_id = Guid.NewGuid().ToString();
                }

                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.PengeluaranBiayaRepository.Update(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.PengeluaranBiayaRepository.Update(obj);
                }
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

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.PengeluaranBiayaRepository.Delete(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.PengeluaranBiayaRepository.Delete(obj);
                }
            }

            return result;
        }

        public string GetLastNota()
        {
            var lastNota = string.Empty;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                lastNota = _unitOfWork.PengeluaranBiayaRepository.GetLastNota();
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    lastNota = _unitOfWork.PengeluaranBiayaRepository.GetLastNota();
                }
            }

            return lastNota;
        }
    }
}