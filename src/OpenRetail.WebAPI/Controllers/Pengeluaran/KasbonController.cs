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
	public interface IKasbonController : IBaseApiController<KasbonDTO>
    {
        IHttpActionResult GetByID(string id);
        IHttpActionResult GetByName(string name);

        IHttpActionResult GetLastNota();
        IHttpActionResult GetByKaryawanId(string karyawanId);
        IHttpActionResult GetByStatus(bool isLunas);
        IHttpActionResult GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai);
    }

	[RoutePrefix("api/kasbon")]
    public class KasbonController : BaseApiController, IKasbonController
    {
        private IUnitOfWork _unitOfWork;
        private ILog _log;
        private HttpStatusCode _httpStatusCode = HttpStatusCode.BadRequest;
        private IHttpActionResult _response = null;
		
		public KasbonController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public KasbonController(IUnitOfWork unitOfWork, ILog log)
        {
            this._unitOfWork = unitOfWork;
            this._log = log;
        }

		[HttpGet, Route("get_by_id")]
        public IHttpActionResult GetByID(string id)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = new List<Kasbon>();
                var obj = _unitOfWork.KasbonRepository.GetByID(id);
                
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
            throw new NotImplementedException();
        }

        [HttpGet, Route("get_last_nota")]
        public IHttpActionResult GetLastNota()
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = new List<string>();
                var obj = _unitOfWork.KasbonRepository.GetLastNota();

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

        [HttpGet, Route("get_by_karyawan_id")]
        public IHttpActionResult GetByKaryawanId(string karyawan_id)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = _unitOfWork.KasbonRepository.GetByKaryawanId(karyawan_id);

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

        [HttpGet, Route("get_by_status")]
        public IHttpActionResult GetByStatus(bool is_lunas)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = _unitOfWork.KasbonRepository.GetByStatus(is_lunas);

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
        public IHttpActionResult GetByTanggal(DateTime tanggal_mulai, DateTime tanggal_selesai)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = _unitOfWork.KasbonRepository.GetByTanggal(tanggal_mulai, tanggal_selesai);

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
                var results = _unitOfWork.KasbonRepository.GetAll();

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

		[HttpPost, Route("save")]
        public IHttpActionResult Save(KasbonDTO objDTO)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            if (!objDTO.IsValidate(Request, ref _response))
            {
                return _response;
            }

            try
            {
                var obj = AutoMapper.Mapper.Map<Kasbon>(objDTO);

                var result = _unitOfWork.KasbonRepository.Save(obj);

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

		[HttpPost, Route("update")]
        public IHttpActionResult Update(KasbonDTO objDTO)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            if (!objDTO.IsValidate(Request, ref _response))
            {
                return _response;
            }

            try
            {
                var obj = AutoMapper.Mapper.Map<Kasbon>(objDTO);

                var result = _unitOfWork.KasbonRepository.Update(obj);

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
        public IHttpActionResult Delete(KasbonDTO objDTO)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            if (!objDTO.IsValidate(Request, ref _response))
            {
                return _response;
            }

            try
            {
                var obj = AutoMapper.Mapper.Map<Kasbon>(objDTO);

                var result = _unitOfWork.KasbonRepository.Delete(obj);

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
