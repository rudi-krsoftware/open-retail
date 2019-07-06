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
    public class JualProdukBll : IJualProdukBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;
		private JualProdukValidator _validator;

        private bool _isUseWebAPI;
        private string _baseUrl;

		public JualProdukBll(ILog log)
        {
			_log = log;
            _validator = new JualProdukValidator();
        }

        public JualProdukBll(bool isUseWebAPI, string baseUrl, ILog log)
            : this(log)
        {
            _isUseWebAPI = isUseWebAPI;
            _baseUrl = baseUrl;
        }

        public JualProduk GetByID(string id)
        {
            JualProduk obj = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                obj = _unitOfWork.JualProdukRepository.GetByID(id);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    obj = _unitOfWork.JualProdukRepository.GetByID(id);
                }
            }            

            return obj;
        }

        public JualProduk GetListItemNotaTerakhir(string penggunaId, string mesinId)
        {
            JualProduk obj = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                obj = _unitOfWork.JualProdukRepository.GetListItemNotaTerakhir(penggunaId, mesinId);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    obj = _unitOfWork.JualProdukRepository.GetListItemNotaTerakhir(penggunaId, mesinId);
                }
            }            

            return obj;
        }

        public IList<JualProduk> GetByName(string name)
        {
            IList<JualProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetByName(name);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetByName(name);
                }
            }            

            return oList;
        }

        public IList<JualProduk> GetByName(string name, bool isCekKeteranganItemJual, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<JualProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetByName(name, isCekKeteranganItemJual, pageNumber, pageSize, ref pagesCount);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetByName(name, isCekKeteranganItemJual, pageNumber, pageSize, ref pagesCount);
                }
            }            

            return oList;
        }

        public IList<JualProduk> GetAll()
        {
            IList<JualProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetAll();
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetAll();
                }
            }            

            return oList;
        }

        public IList<JualProduk> GetAll(int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<JualProduk> oList = null;            

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetAll(pageNumber, pageSize, ref pagesCount);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetAll(pageNumber, pageSize, ref pagesCount);
                }
            }

            return oList;
        }

		public int Save(JualProduk obj)
        {
            var result = 0;            

            if (_isUseWebAPI)
            {
                obj.jual_id = Guid.NewGuid().ToString();

                foreach (var item in obj.item_jual)
                {
                    item.item_jual_id = Guid.NewGuid().ToString();
                }

                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.JualProdukRepository.Save(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.JualProdukRepository.Save(obj);
                }
            }

            return result;
        }

        public int Save(JualProduk obj, ref ValidationError validationError)
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

		public int Update(JualProduk obj)
        {
            var result = 0;

            if (_isUseWebAPI)
            {
                foreach (var item in obj.item_jual.Where(f => f.entity_state == EntityState.Added))
                {
                    item.item_jual_id = Guid.NewGuid().ToString();
                }

                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.JualProdukRepository.Update(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.JualProdukRepository.Update(obj);
                }
            }            

            return result;
        }

        public int Update(JualProduk obj, ref ValidationError validationError)
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

        public int Delete(JualProduk obj)
        {
            var result = 0;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                result = _unitOfWork.JualProdukRepository.Delete(obj);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    result = _unitOfWork.JualProdukRepository.Delete(obj);
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
                lastNota = _unitOfWork.JualProdukRepository.GetLastNota();
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    lastNota = _unitOfWork.JualProdukRepository.GetLastNota();
                }
            }            

            return lastNota;
        }

        public IList<JualProduk> GetAll(string name)
        {            
            IList<JualProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetAll(name);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetAll(name);
                }
            }            

            return oList;
        }

        public IList<JualProduk> GetNotaCustomer(string id, string nota)
        {
            IList<JualProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetNotaCustomer(id, nota);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetNotaCustomer(id, nota);
                }
            }            

            return oList;
        }

        public IList<JualProduk> GetNotaKreditByCustomer(string id, bool isLunas)
        {
            IList<JualProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetNotaKreditByCustomer(id, isLunas);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetNotaKreditByCustomer(id, isLunas);
                }
            }            

            return oList;
        }

        public IList<JualProduk> GetNotaKreditByNota(string id, string nota)
        {
            IList<JualProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetNotaKreditByNota(id, nota);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetNotaKreditByNota(id, nota);
                }
            }            

            return oList;
        }

        public IList<JualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<JualProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
                }
            }            

            return oList;
        }

        public IList<JualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<JualProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai, pageNumber, pageSize, ref pagesCount);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai, pageNumber, pageSize, ref pagesCount);
                }
            }            

            return oList;
        }

        public IList<JualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, string name)
        {
            IList<JualProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai, name);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai, name);
                }
            }            

            return oList;
        }

        public IList<JualProduk> GetByLimit(DateTime tanggalMulai, DateTime tanggalSelesai, int limit)
        {
            IList<JualProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetByLimit(tanggalMulai, tanggalSelesai, limit);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetByLimit(tanggalMulai, tanggalSelesai, limit);
                }
            }

            return oList;
        }

        public IList<ItemJualProduk> GetItemJual(string jualId)
        {
            IList<ItemJualProduk> oList = null;

            if (_isUseWebAPI)
            {
                _unitOfWork = new UnitOfWork(_isUseWebAPI, _baseUrl, _log);
                oList = _unitOfWork.JualProdukRepository.GetItemJual(jualId);
            }
            else
            {
                using (IDapperContext context = new DapperContext())
                {
                    _unitOfWork = new UnitOfWork(context, _log);
                    oList = _unitOfWork.JualProdukRepository.GetItemJual(jualId);
                }
            }            

            return oList;
        }        
    }
}     
