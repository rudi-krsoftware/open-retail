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
    public class PembayaranPiutangProdukBll : IPembayaranPiutangProdukBll
    {
		private ILog _log;
        private IUnitOfWork _unitOfWork;
		private PembayaranPiutangProdukValidator _validator;

        private bool _isUseWebAPI;
        private string _baseUrl;

		public PembayaranPiutangProdukBll(ILog log)
        {
			_log = log;
            _validator = new PembayaranPiutangProdukValidator();
        }

        public PembayaranPiutangProdukBll(bool isUseWebAPI, string baseUrl, ILog log)
            : this(log)
        {
            _isUseWebAPI = isUseWebAPI;
            _baseUrl = baseUrl;
        }

        public PembayaranPiutangProduk GetByID(string id)
        {
            PembayaranPiutangProduk obj = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                obj = _unitOfWork.PembayaranPiutangProdukRepository.GetByID(id);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    obj = _unitOfWork.PembayaranPiutangProdukRepository.GetByID(id);
                }
            }            

            return obj;
        }

        public string GetLastNota()
        {
            var lastNota = string.Empty;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                lastNota = _unitOfWork.PembayaranPiutangProdukRepository.GetLastNota();
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    lastNota = _unitOfWork.PembayaranPiutangProdukRepository.GetLastNota();
                }
            }            

            return lastNota;
        }

        public ItemPembayaranPiutangProduk GetByJualID(string id)
        {
            ItemPembayaranPiutangProduk obj = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                obj = _unitOfWork.PembayaranPiutangProdukRepository.GetByJualID(id);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    obj = _unitOfWork.PembayaranPiutangProdukRepository.GetByJualID(id);
                }
            }            

            return obj;
        }

        public IList<PembayaranPiutangProduk> GetByName(string name)
        {
            IList<PembayaranPiutangProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.PembayaranPiutangProdukRepository.GetByName(name);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.PembayaranPiutangProdukRepository.GetByName(name);
                }
            }             

            return oList;
        }        

        public IList<PembayaranPiutangProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PembayaranPiutangProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.PembayaranPiutangProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.PembayaranPiutangProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
                }
            }            

            return oList;
        }

        public IList<PembayaranPiutangProduk> GetAll()
        {
            IList<PembayaranPiutangProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.PembayaranPiutangProdukRepository.GetAll();
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.PembayaranPiutangProdukRepository.GetAll();
                }
            }            

            return oList;
        }

        public IList<ItemPembayaranPiutangProduk> GetHistoriPembayaran(string jualId)
        {
            IList<ItemPembayaranPiutangProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.PembayaranPiutangProdukRepository.GetHistoriPembayaran(jualId);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.PembayaranPiutangProdukRepository.GetHistoriPembayaran(jualId);
                }
            }            

            return oList;
        }

		public int Save(PembayaranPiutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Save(PembayaranPiutangProduk obj, bool isSaveFromPenjualan, ref ValidationError validationError)
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
                obj.pembayaran_piutang_id = Guid.NewGuid().ToString();

                foreach (var item in obj.item_pembayaran_piutang)
                {
                    item.item_pembayaran_piutang_id = Guid.NewGuid().ToString();
                }

                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.PembayaranPiutangProdukRepository.Save(obj, isSaveFromPenjualan);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.PembayaranPiutangProdukRepository.Save(obj, isSaveFromPenjualan);
                }
            }            

            return result;
        }

		public int Update(PembayaranPiutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Update(PembayaranPiutangProduk obj, bool isSaveFromPenjualan, ref ValidationError validationError)
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
                foreach (var item in obj.item_pembayaran_piutang.Where(f => f.entity_state == EntityState.Added))
                {
                    item.item_pembayaran_piutang_id = Guid.NewGuid().ToString();
                }

                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.PembayaranPiutangProdukRepository.Update(obj, isSaveFromPenjualan);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.PembayaranPiutangProdukRepository.Update(obj, isSaveFromPenjualan);
                }
            }            

            return result;
        }

        public int Delete(PembayaranPiutangProduk obj)
        {
            var result = 0;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.PembayaranPiutangProdukRepository.Delete(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.PembayaranPiutangProdukRepository.Delete(obj);
                }
            }            

            return result;
        }        
    }
}     
