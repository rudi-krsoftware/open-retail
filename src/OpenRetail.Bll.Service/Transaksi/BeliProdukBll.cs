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
    public class BeliProdukBll : IBeliProdukBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;
        private BeliProdukValidator _validator;

        private bool _isUseWebAPI;
        private string _baseUrl;

        public BeliProdukBll(ILog log)
        {
            _log = log;
            _validator = new BeliProdukValidator();
        }

        public BeliProdukBll(bool isUseWebAPI, string baseUrl, ILog log)
            : this(log)
        {
            _isUseWebAPI = isUseWebAPI;
            _baseUrl = baseUrl;
        }

        public BeliProduk GetByID(string id)
        {
            BeliProduk obj = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                obj = _unitOfWork.BeliProdukRepository.GetByID(id);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    obj = _unitOfWork.BeliProdukRepository.GetByID(id);
                }
            }

            return obj;
        }

        public IList<BeliProduk> GetByName(string name)
        {
            IList<BeliProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.BeliProdukRepository.GetByName(name);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.BeliProdukRepository.GetByName(name);
                }
            }

            return oList;
        }

        public IList<BeliProduk> GetByName(string name, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<BeliProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.BeliProdukRepository.GetByName(name, pageNumber, pageSize, ref pagesCount);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.BeliProdukRepository.GetByName(name, pageNumber, pageSize, ref pagesCount);
                }
            }

            return oList;
        }

        public IList<BeliProduk> GetAll()
        {
            IList<BeliProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.BeliProdukRepository.GetAll();
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.BeliProdukRepository.GetAll();
                }
            }

            return oList;
        }

        public IList<BeliProduk> GetAll(int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<BeliProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.BeliProdukRepository.GetAll(pageNumber, pageSize, ref pagesCount);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.BeliProdukRepository.GetAll(pageNumber, pageSize, ref pagesCount);
                }
            }

            return oList;
        }

        public int Save(BeliProduk obj)
        {
            var result = 0;

            if (_isUseWebAPI)
            {
                obj.beli_produk_id = Guid.NewGuid().ToString();

                foreach (var item in obj.item_beli)
                {
                    item.item_beli_produk_id = Guid.NewGuid().ToString();
                }

                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.BeliProdukRepository.Save(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.BeliProdukRepository.Save(obj);
                }
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

            if (_isUseWebAPI)
            {
                foreach (var item in obj.item_beli.Where(f => f.entity_state == EntityState.Added))
                {
                    item.item_beli_produk_id = Guid.NewGuid().ToString();
                }

                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.BeliProdukRepository.Update(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.BeliProdukRepository.Update(obj);
                }
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

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.BeliProdukRepository.Delete(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.BeliProdukRepository.Delete(obj);
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
                lastNota = _unitOfWork.BeliProdukRepository.GetLastNota();
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    lastNota = _unitOfWork.BeliProdukRepository.GetLastNota();
                }
            }

            return lastNota;
        }

        public IList<BeliProduk> GetAll(string name)
        {
            IList<BeliProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.BeliProdukRepository.GetAll(name);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.BeliProdukRepository.GetAll(name);
                }
            }

            return oList;
        }

        public IList<BeliProduk> GetNotaSupplier(string id, string nota)
        {
            IList<BeliProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.BeliProdukRepository.GetNotaSupplier(id, nota);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.BeliProdukRepository.GetNotaSupplier(id, nota);
                }
            }

            return oList;
        }

        public IList<BeliProduk> GetNotaKreditBySupplier(string id, bool isLunas)
        {
            IList<BeliProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.BeliProdukRepository.GetNotaKreditBySupplier(id, isLunas);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.BeliProdukRepository.GetNotaKreditBySupplier(id, isLunas);
                }
            }

            return oList;
        }

        public IList<BeliProduk> GetNotaKreditByNota(string id, string nota)
        {
            IList<BeliProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.BeliProdukRepository.GetNotaKreditByNota(id, nota);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.BeliProdukRepository.GetNotaKreditByNota(id, nota);
                }
            }

            return oList;
        }

        public IList<BeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<BeliProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.BeliProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.BeliProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
                }
            }

            return oList;
        }

        public IList<BeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<BeliProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.BeliProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai, pageNumber, pageSize, ref pagesCount);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.BeliProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai, pageNumber, pageSize, ref pagesCount);
                }
            }

            return oList;
        }

        public IList<BeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, string name)
        {
            IList<BeliProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.BeliProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai, name);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.BeliProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai, name);
                }
            }

            return oList;
        }

        public IList<ItemBeliProduk> GetItemBeli(string beliId)
        {
            IList<ItemBeliProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.BeliProdukRepository.GetItemBeli(beliId);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.BeliProdukRepository.GetItemBeli(beliId);
                }
            }

            return oList;
        }
    }
}