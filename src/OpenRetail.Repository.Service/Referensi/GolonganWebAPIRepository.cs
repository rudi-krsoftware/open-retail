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

using log4net;
using RestSharp;
using Newtonsoft.Json;
using OpenRetail.Helper;
using OpenRetail.Model;
using OpenRetail.Model.WebAPI;
using OpenRetail.Repository.Api;

namespace OpenRetail.Repository.Service
{
    public class GolonganWebAPIRepository : IGolonganRepository
    {
        private string _apiUrl = string.Empty;
        private ILog _log;

        public GolonganWebAPIRepository(string baseUrl, ILog log)
        {
            this._apiUrl = baseUrl + "api/golongan/";
            this._log = log;            
        }

        public Golongan GetByID(string id)
        {
            Golongan obj = null;

            try
            {
                var api = string.Format("get_by_id?id={0}", id);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Golongan>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    obj = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<Golongan> GetByName(string name, bool useLikeOperator = true)
        {
            IList<Golongan> oList = new List<Golongan>();

            try
            {
                var api = string.Format("get_by_name?name={0}&useLikeOperator={1}", name, useLikeOperator);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Golongan>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<Golongan> GetAll()
        {
            IList<Golongan> oList = new List<Golongan>();

            try
            {
                var api = "get_all";
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Golongan>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(Golongan obj)
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

        public int Update(Golongan obj)
        {
            var result = 0;

            try
            {
                var api = "update";
                var response = RestSharpHelper<OpenRetailWebApiPostResponse>.PostRequest(_apiUrl, api, obj);

                result = Convert.ToInt32(response.Results);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Delete(Golongan obj)
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
