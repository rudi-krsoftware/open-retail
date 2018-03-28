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
                var client = new RestClient(_apiUrl);
                var request = new RestRequest(string.Format("get_by_id?id={0}", id), Method.GET);
                var response = client.Execute<OpenRetailWebApiGetResponse<PembayaranKasbon>>(request).Data;

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
                var client = new RestClient(_apiUrl);
                var request = new RestRequest("get_last_nota", Method.GET);
                var response = client.Execute<OpenRetailWebApiGetResponse<string>>(request).Data;

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
                var client = new RestClient(_apiUrl);
                var request = new RestRequest(string.Format("get_by_kasbon_id?kasbon_id={0}", kasbonId), Method.GET);
                var response = client.Execute<OpenRetailWebApiGetResponse<PembayaranKasbon>>(request).Data;

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
                var client = new RestClient(_apiUrl);
                var request = new RestRequest(string.Format("get_by_gaji_karyawan?gaji_karyawan_id={0}", gajiKaryawanId), Method.GET);
                var response = client.Execute<OpenRetailWebApiGetResponse<PembayaranKasbon>>(request).Data;

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
                var client = new RestClient(_apiUrl);
                var request = new RestRequest("save", Method.POST);

                request.RequestFormat = DataFormat.Json;
                request.AddBody(obj);

                var response = client.Execute(request);
                var responseContent = JsonConvert.DeserializeObject<OpenRetailWebApiPostResponse>(response.Content);

                result = Convert.ToInt32(responseContent.Results);
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
                var client = new RestClient(_apiUrl);
                var request = new RestRequest("update", Method.POST);

                request.AddParameter("application/json", obj.ToJson(), ParameterType.RequestBody);

                var response = client.Execute(request);
                var responseContent = JsonConvert.DeserializeObject<OpenRetailWebApiPostResponse>(response.Content);

                result = Convert.ToInt32(responseContent.Results);
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
                var client = new RestClient(_apiUrl);
                var request = new RestRequest("delete", Method.POST);

                request.RequestFormat = DataFormat.Json;
                request.AddBody(obj);

                var response = client.Execute(request);
                var responseContent = JsonConvert.DeserializeObject<OpenRetailWebApiPostResponse>(response.Content);

                result = Convert.ToInt32(responseContent.Results);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }        
    }
}     
