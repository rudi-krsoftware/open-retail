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

namespace OpenRetail.WebAPI.Controllers
{
    [RoutePrefix("api/golongan")]
    public class GolonganController : ApiController
    {
        private IUnitOfWork _unitOfWork;
        private ILog _log;

        public GolonganController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public GolonganController(IUnitOfWork unitOfWork, ILog log)
        {
            this._unitOfWork = unitOfWork;
            this._log = log;
        }

        [HttpGet, Route("get_all")]
        public IHttpActionResult GetAll()
        {
            IHttpActionResult response = null;

            var listOfGolongan = _unitOfWork.GolonganRepository.GetAll();

            if (listOfGolongan.Count > 0)
            {
                response = Content(HttpStatusCode.OK, new ResponsePackage(listOfGolongan));
            }
            else
            {
                response = Content(HttpStatusCode.InternalServerError, new ResponsePackage("Response does not contain any data."));
            }

            return response;
        }
    }
}
