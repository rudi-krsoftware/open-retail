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
    public class PembayaranHutangProdukBll : IPembayaranHutangProdukBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;
        private PembayaranHutangProdukValidator _validator;

        private bool _isUseWebAPI;
        private string _baseUrl;

        public PembayaranHutangProdukBll(ILog log)
        {
            _log = log;
            _validator = new PembayaranHutangProdukValidator();
        }

        public PembayaranHutangProdukBll(bool isUseWebAPI, string baseUrl, ILog log)
            : this(log)
        {
            _isUseWebAPI = isUseWebAPI;
            _baseUrl = baseUrl;
        }

        public PembayaranHutangProduk GetByID(string id)
        {
            PembayaranHutangProduk obj = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                obj = _unitOfWork.PembayaranHutangProdukRepository.GetByID(id);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    obj = _unitOfWork.PembayaranHutangProdukRepository.GetByID(id);
                }
            }

            return obj;
        }

        public IList<PembayaranHutangProduk> GetByName(string name)
        {
            IList<PembayaranHutangProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.PembayaranHutangProdukRepository.GetByName(name);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.PembayaranHutangProdukRepository.GetByName(name);
                }
            }

            return oList;
        }

        public IList<PembayaranHutangProduk> GetAll()
        {
            IList<PembayaranHutangProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.PembayaranHutangProdukRepository.GetAll();
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.PembayaranHutangProdukRepository.GetAll();
                }
            }

            return oList;
        }

        public IList<ItemPembayaranHutangProduk> GetHistoriPembayaran(string beliId)
        {
            IList<ItemPembayaranHutangProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.PembayaranHutangProdukRepository.GetHistoriPembayaran(beliId);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.PembayaranHutangProdukRepository.GetHistoriPembayaran(beliId);
                }
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

            if (_isUseWebAPI)
            {
                obj.pembayaran_hutang_produk_id = Guid.NewGuid().ToString();

                foreach (var item in obj.item_pembayaran_hutang)
                {
                    item.item_pembayaran_hutang_produk_id = Guid.NewGuid().ToString();
                }

                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.PembayaranHutangProdukRepository.Save(obj, isSaveFromPembelian);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.PembayaranHutangProdukRepository.Save(obj, isSaveFromPembelian);
                }
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

            if (_isUseWebAPI)
            {
                foreach (var item in obj.item_pembayaran_hutang.Where(f => f.entity_state == EntityState.Added))
                {
                    item.item_pembayaran_hutang_produk_id = Guid.NewGuid().ToString();
                }

                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.PembayaranHutangProdukRepository.Update(obj, isUpdateFromPembelian);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.PembayaranHutangProdukRepository.Update(obj, isUpdateFromPembelian);
                }
            }

            return result;
        }

        public int Delete(PembayaranHutangProduk obj)
        {
            var result = 0;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.PembayaranHutangProdukRepository.Delete(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.PembayaranHutangProdukRepository.Delete(obj);
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
                lastNota = _unitOfWork.PembayaranHutangProdukRepository.GetLastNota();
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    lastNota = _unitOfWork.PembayaranHutangProdukRepository.GetLastNota();
                }
            }

            return lastNota;
        }

        public ItemPembayaranHutangProduk GetByBeliID(string id)
        {
            ItemPembayaranHutangProduk obj = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                obj = _unitOfWork.PembayaranHutangProdukRepository.GetByBeliID(id);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    obj = _unitOfWork.PembayaranHutangProdukRepository.GetByBeliID(id);
                }
            }

            return obj;
        }

        public IList<PembayaranHutangProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PembayaranHutangProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.PembayaranHutangProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.PembayaranHutangProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
                }
            }

            return oList;
        }
    }
}