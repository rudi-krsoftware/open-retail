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
using System.Net;
using System.Net.Http;
using System.Web.Http;

using log4net;
using OpenRetail.Model;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;
using OpenRetail.WebAPI.Models;
using OpenRetail.WebAPI.Models.DTO;
using OpenRetail.WebAPI.Controllers.Helper;

namespace OpenRetail.WebAPI.Controllers
{
    public interface IGolonganController : IBaseApiController<GolonganDTO>
    {
        IHttpActionResult GetByID(string id);
        IHttpActionResult GetByName(string name, bool useLikeOperator = true);
    }

    [RoutePrefix("api/golongan")]
    public class GolonganController : BaseApiController, IGolonganController
    {
        private IUnitOfWork _unitOfWork;
        private ILog _log;
        private HttpStatusCode _httpStatusCode = HttpStatusCode.BadRequest;
        private IHttpActionResult _response = null;

        public GolonganController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public GolonganController(IUnitOfWork unitOfWork, ILog log)
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
                var results = new List<Golongan>();
                var obj = _unitOfWork.GolonganRepository.GetByID(id);
                
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
        public IHttpActionResult GetByName(string name, bool useLikeOperator = true)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            try
            {
                var results = _unitOfWork.GolonganRepository.GetByName(name, useLikeOperator);

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
                var results = _unitOfWork.GolonganRepository.GetAll();

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
        public IHttpActionResult Save(GolonganDTO objDTO)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            if (!objDTO.IsValidate(Request, ref _response))
            {
                return _response;
            }

            try
            {
                var obj = AutoMapper.Mapper.Map<Golongan>(objDTO);

                var result = _unitOfWork.GolonganRepository.Save(obj);

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
        public IHttpActionResult Update(GolonganDTO objDTO)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            if (!objDTO.IsValidate(Request, ref _response))
            {
                return _response;
            }

            try
            {
                var obj = AutoMapper.Mapper.Map<Golongan>(objDTO);

                var result = _unitOfWork.GolonganRepository.Update(obj);

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
        public IHttpActionResult Delete(GolonganDTO objDTO)
        {
            _httpStatusCode = HttpStatusCode.BadRequest;
            _response = Content(_httpStatusCode, new ResponsePackage(_httpStatusCode));

            if (!objDTO.IsValidate(Request, ref _response))
            {
                return _response;
            }

            try
            {
                var obj = AutoMapper.Mapper.Map<Golongan>(objDTO);

                var result = _unitOfWork.GolonganRepository.Delete(obj);

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
