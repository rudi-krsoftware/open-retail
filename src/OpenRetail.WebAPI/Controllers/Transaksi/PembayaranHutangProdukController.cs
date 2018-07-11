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
using System.Net;
using System.Web.Http;
using OpenRetail.Model;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;
using OpenRetail.WebAPI.Models;
using OpenRetail.WebAPI.Models.DTO;
using OpenRetail.WebAPI.Controllers.Helper;

namespace OpenRetail.WebAPI.Controllers
{
    public interface IPembayaranHutangProdukController : IBaseApiController<PembayaranHutangProdukDTO>
    {
        IHttpActionResult Save(PembayaranHutangProdukDTO obj, bool isSaveFromPembelian);
        IHttpActionResult Update(PembayaranHutangProdukDTO obj, bool isUpdateFromPembelian);

        IHttpActionResult GetLastNota();
        IHttpActionResult GetByID(string id);
        IHttpActionResult GetByBeliID(string id);

        IHttpActionResult GetByName(string name);
        IHttpActionResult GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai);
        IHttpActionResult GetHistoriPembayaran(string beliId);
    }

    [RoutePrefix("api/pembayaran_hutang")]
    public class PembayaranHutangProdukController : BaseApiController, IPembayaranHutangProdukController
    {
        private IUnitOfWork _unitOfWork;
        private ILog _log;
        private HttpStatusCode _httpStatusCode = HttpStatusCode.BadRequest;
        private IHttpActionResult _response = null;
		
		public PembayaranHutangProdukController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public PembayaranHutangProdukController(IUnitOfWork unitOfWork, ILog log)
        {
            this._unitOfWork = unitOfWork;
            this._log = log;
        }

        [HttpGet, Route("get_last_nota")]
        public IHttpActionResult GetLastNota()
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = new List<string>();
                var obj = _unitOfWork.PembayaranHutangProdukRepository.GetLastNota();

                if (obj != null)
                    results.Add(obj);

                _httpStatusCode = HttpStatusCode.OK;
                var output = GenerateOutput(_httpStatusCode, results);

                _response = Content(_httpStatusCode, output);
            }
            catch (Exception ex)
            {
                if (_log != null)
                    _log.Error("Error:", ex);
            }

            return _response;
        }

        [HttpGet, Route("get_by_id")]
        public IHttpActionResult GetByID(string id)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = new List<PembayaranHutangProduk>();
                var obj = _unitOfWork.PembayaranHutangProdukRepository.GetByID(id);

                if (obj != null)
                    results.Add(obj);

                _httpStatusCode = HttpStatusCode.OK;
                var output = GenerateOutput(_httpStatusCode, results);

                _response = Content(_httpStatusCode, output);
            }
            catch (Exception ex)
            {
                if (_log != null)
                    _log.Error("Error:", ex);
            }

            return _response;
        }

        [HttpGet, Route("get_by_beli_id")]
        public IHttpActionResult GetByBeliID(string id)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = new List<ItemPembayaranHutangProduk>();
                var obj = _unitOfWork.PembayaranHutangProdukRepository.GetByBeliID(id);

                if (obj != null)
                    results.Add(obj);

                _httpStatusCode = HttpStatusCode.OK;
                var output = GenerateOutput(_httpStatusCode, results);

                _response = Content(_httpStatusCode, output);
            }
            catch (Exception ex)
            {
                if (_log != null)
                    _log.Error("Error:", ex);
            }

            return _response;
        }

        [HttpGet, Route("get_by_name")]
        public IHttpActionResult GetByName(string name)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = _unitOfWork.PembayaranHutangProdukRepository.GetByName(name.NullToString());

                _httpStatusCode = HttpStatusCode.OK;
                var output = GenerateOutput(_httpStatusCode, results);

                _response = Content(_httpStatusCode, output);
            }
            catch (Exception ex)
            {
                if (_log != null)
                    _log.Error("Error:", ex);
            }

            return _response;
        }

        [HttpGet, Route("get_by_tanggal")]
        public IHttpActionResult GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = _unitOfWork.PembayaranHutangProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);

                _httpStatusCode = HttpStatusCode.OK;
                var output = GenerateOutput(_httpStatusCode, results);

                _response = Content(_httpStatusCode, output);
            }
            catch (Exception ex)
            {
                if (_log != null)
                    _log.Error("Error:", ex);
            }

            return _response;
        }

        [HttpGet, Route("get_histori_pembayaran")]
        public IHttpActionResult GetHistoriPembayaran(string beliId)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = _unitOfWork.PembayaranHutangProdukRepository.GetHistoriPembayaran(beliId);

                _httpStatusCode = HttpStatusCode.OK;
                var output = GenerateOutput(_httpStatusCode, results);

                _response = Content(_httpStatusCode, output);
            }
            catch (Exception ex)
            {
                if (_log != null)
                    _log.Error("Error:", ex);
            }

            return _response;
        }

        [HttpGet, Route("get_all")]
        public IHttpActionResult GetAll()
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = _unitOfWork.PembayaranHutangProdukRepository.GetAll();

                _httpStatusCode = HttpStatusCode.OK;
                var output = GenerateOutput(_httpStatusCode, results);

                _response = Content(_httpStatusCode, output);
            }
            catch (Exception ex)
            {
                if (_log != null)
                    _log.Error("Error:", ex);
            }

            return _response;
        }

        public IHttpActionResult Save(PembayaranHutangProdukDTO objDTO)
        {
            throw new NotImplementedException();
        }

        [HttpPost, Route("save")]
        public IHttpActionResult Save(PembayaranHutangProdukDTO objDTO, bool isSaveFromPembelian)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            if (!objDTO.IsValidate(Request, ref _response))
            {
                return _response;
            }

            try
            {
                var obj = AutoMapper.Mapper.Map<PembayaranHutangProduk>(objDTO);

                var result = _unitOfWork.PembayaranHutangProdukRepository.Save(obj, isSaveFromPembelian);

                _httpStatusCode = HttpStatusCode.OK;
                var output = GenerateOutput(_httpStatusCode, result);

                _response = Content(_httpStatusCode, output);
            }
            catch (Exception ex)
            {
                if (_log != null)
                    _log.Error("Error:", ex);
            }

            return _response;
        }        

        public IHttpActionResult Update(PembayaranHutangProdukDTO objDTO)
        {
            throw new NotImplementedException();
        }

        [HttpPost, Route("update")]
        public IHttpActionResult Update(PembayaranHutangProdukDTO objDTO, bool isUpdateFromPembelian)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            if (!objDTO.IsValidate(Request, ref _response))
            {
                return _response;
            }

            try
            {
                var obj = AutoMapper.Mapper.Map<PembayaranHutangProduk>(objDTO);

                var result = _unitOfWork.PembayaranHutangProdukRepository.Update(obj, isUpdateFromPembelian);

                _httpStatusCode = HttpStatusCode.OK;
                var output = GenerateOutput(_httpStatusCode, result);

                _response = Content(_httpStatusCode, output);
            }
            catch (Exception ex)
            {
                if (_log != null)
                    _log.Error("Error:", ex);
            }

            return _response;
        }

        [HttpPost, Route("delete")]
        public IHttpActionResult Delete(PembayaranHutangProdukDTO objDTO)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            if (!objDTO.IsValidate(Request, ref _response))
            {
                return _response;
            }

            try
            {
                var obj = AutoMapper.Mapper.Map<PembayaranHutangProduk>(objDTO);

                var result = _unitOfWork.PembayaranHutangProdukRepository.Delete(obj);

                _httpStatusCode = HttpStatusCode.OK;
                var output = GenerateOutput(_httpStatusCode, result);

                _response = Content(_httpStatusCode, output);
            }
            catch (Exception ex)
            {
                if (_log != null)
                    _log.Error("Error:", ex);
            }

            return _response;
        }        
    }
}
