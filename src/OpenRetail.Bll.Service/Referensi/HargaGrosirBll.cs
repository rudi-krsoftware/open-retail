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
using System.Collections.Generic;

namespace OpenRetail.Bll.Service
{
    public class HargaGrosirBll : IHargaGrosirBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;

        private bool _isUseWebAPI;
        private string _baseUrl;

        public HargaGrosirBll(ILog log)
        {
            _log = log;
        }

        public HargaGrosirBll(bool isUseWebAPI, string baseUrl, ILog log)
            : this(log)
        {
            _isUseWebAPI = isUseWebAPI;
            _baseUrl = baseUrl;
        }

        public IList<HargaGrosir> GetListHargaGrosir(string produkId)
        {
            IList<HargaGrosir> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.HargaGrosirRepository.GetListHargaGrosir(produkId);
            }

            return oList;
        }
    }
}