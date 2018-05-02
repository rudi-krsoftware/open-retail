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
using RestSharp;
using Newtonsoft.Json;
using OpenRetail.Helper;
using OpenRetail.Model;
using OpenRetail.Model.WebAPI;
using OpenRetail.Repository.Api;
 
namespace OpenRetail.Repository.Service
{        
    public class PembayaranKasbonWebAPIRepository : IPembayaranKasbonRepository
    {
        private string _apiUrl = string.Empty;
        private ILog _log;
		
        public PembayaranKasbonWebAPIRepository(string baseUrl, ILog log)
        {
            this._apiUrl = baseUrl + "api/pembayaran_kasbon/";
            this._log = log;
        }

        public PembayaranKasbon GetByID(string id)
        {
            PembayaranKasbon obj = null;

			try
            {
                var api = string.Format("get_by_id?id={0}", id);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PembayaranKasbon>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    obj = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public string GetLastNota()
        {
            var result = string.Empty;

            try
            {
                var api = "get_last_nota";
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<string>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    result = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public IList<PembayaranKasbon> GetByKasbonId(string kasbonId)
        {
            IList<PembayaranKasbon> oList = new List<PembayaranKasbon>();

            try
            {
                var api = string.Format("get_by_kasbon_id?kasbon_id={0}", kasbonId);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PembayaranKasbon>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PembayaranKasbon> GetByGajiKaryawan(string gajiKaryawanId)
        {
            IList<PembayaranKasbon> oList = new List<PembayaranKasbon>();

            try
            {
                var api = string.Format("get_by_gaji_karyawan?gaji_karyawan_id={0}", gajiKaryawanId);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PembayaranKasbon>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PembayaranKasbon> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<PembayaranKasbon> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Save(PembayaranKasbon obj)
        {
            var result = 0;

			try
            {
                var api = "save";
                var response = RestSharpHelper<OpenRetailWebApiPostResponse>.PostRequest(_apiUrl, api, obj);

                result = Convert.ToInt32(response.Results);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Update(PembayaranKasbon obj)
        {
            var result = 0;

            try
            {
                var api = "update";
                var response = RestSharpHelper<OpenRetailWebApiPostResponse>.PostRequest(_apiUrl, api, obj.ToJson());

                result = Convert.ToInt32(response.Results);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Delete(PembayaranKasbon obj)
        {
            var result = 0;

			try
            {
                var api = "delete";
                var response = RestSharpHelper<OpenRetailWebApiPostResponse>.PostRequest(_apiUrl, api, obj);

                result = Convert.ToInt32(response.Results);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }        
    }
}     
