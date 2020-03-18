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
using OpenRetail.Helper;
using OpenRetail.Model;
using OpenRetail.Model.WebAPI;
using OpenRetail.Repository.Api;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenRetail.Repository.Service
{
    public class GajiKaryawanWebAPIRepository : IGajiKaryawanRepository
    {
        private string _apiUrl = string.Empty;
        private ILog _log;

        public GajiKaryawanWebAPIRepository(string baseUrl, ILog log)
        {
            this._apiUrl = baseUrl + "api/gaji_karyawan/";
            this._log = log;
        }

        public GajiKaryawan GetByID(string id)
        {
            GajiKaryawan obj = null;

            try
            {
                var api = string.Format("get_by_id?id={0}", id);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<GajiKaryawan>>.GetRequest(_apiUrl, api).Data;

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

        public IList<GajiKaryawan> GetByBulanAndTahun(int bulan, int tahun)
        {
            IList<GajiKaryawan> oList = new List<GajiKaryawan>();

            try
            {
                var api = string.Format("get_by_bulan_and_tahun?bulan={0}&tahun={1}", bulan, tahun);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<GajiKaryawan>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<GajiKaryawan> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<GajiKaryawan> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Save(GajiKaryawan obj)
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

        public int Update(GajiKaryawan obj)
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

        public int Delete(GajiKaryawan obj)
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