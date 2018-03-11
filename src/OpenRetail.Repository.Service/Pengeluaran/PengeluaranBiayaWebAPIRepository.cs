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
    public class PengeluaranBiayaWebAPIRepository : IPengeluaranBiayaRepository
    {
        private string _apiUrl = string.Empty;
        private ILog _log;
		
        public PengeluaranBiayaWebAPIRepository(string baseUrl, ILog log)
        {
            this._apiUrl = baseUrl + "api/pengeluaran_biaya/";
            this._log = log;
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

        public PengeluaranBiaya GetByID(string id)
        {
            PengeluaranBiaya obj = null;

			try
            {
                var client = new RestClient(_apiUrl);
                var request = new RestRequest(string.Format("get_by_id?id={0}", id), Method.GET);
                var response = client.Execute<OpenRetailWebApiGetResponse<PengeluaranBiaya>>(request).Data;

                if (response.Results.Count > 0)
                    obj = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<PengeluaranBiaya> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<PengeluaranBiaya> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PengeluaranBiaya> oList = new List<PengeluaranBiaya>();

            try
            {
                var client = new RestClient(_apiUrl);
                var request = new RestRequest(string.Format("get_by_tanggal?tanggal_mulai={0}&tanggal_selesai={1}", tanggalMulai, tanggalSelesai), Method.GET);
                var response = client.Execute<OpenRetailWebApiGetResponse<PengeluaranBiaya>>(request).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PengeluaranBiaya> GetAll()
        {
            IList<PengeluaranBiaya> oList = new List<PengeluaranBiaya>();

			try
            {
                var client = new RestClient(_apiUrl);
                var request = new RestRequest("get_all", Method.GET);
                var response = client.Execute<OpenRetailWebApiGetResponse<PengeluaranBiaya>>(request).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        private double GetTotalNota(PengeluaranBiaya obj)
        {
            var total = obj.item_pengeluaran_biaya.Where(f => f.JenisPengeluaran != null && f.entity_state != EntityState.Deleted)
                                                  .Sum(f => f.jumlah * f.harga);

            total = Math.Ceiling(total);
            return total;
        }

        public int Save(PengeluaranBiaya obj)
        {
            var result = 0;

			try
            {
                var client = new RestClient(_apiUrl);
                var request = new RestRequest("save", Method.POST);

                obj.tanggal = obj.tanggal.ToUtc();

                request.RequestFormat = DataFormat.Json;
                request.AddBody(obj);

                var response = client.Execute(request);
                var responseContent = JsonConvert.DeserializeObject<OpenRetailWebApiPostResponse>(response.Content);

                result = Convert.ToInt32(responseContent.Results);

                if (result > 0)
                {
                    obj.total = GetTotalNota(obj);

                    foreach (var item in obj.item_pengeluaran_biaya.Where(f => f.JenisPengeluaran != null))
                    {
                        item.entity_state = EntityState.Unchanged;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Update(PengeluaranBiaya obj)
        {
            var result = 0;

			try
            {
                var client = new RestClient(_apiUrl);
                var request = new RestRequest("update", Method.POST);

                obj.tanggal = obj.tanggal.ToUtc();

                request.RequestFormat = DataFormat.Json;
                request.AddBody(obj);

                var response = client.Execute(request);
                var responseContent = JsonConvert.DeserializeObject<OpenRetailWebApiPostResponse>(response.Content);

                result = Convert.ToInt32(responseContent.Results);

                if (result > 0)
                {
                    obj.total = GetTotalNota(obj);

                    foreach (var item in obj.item_pengeluaran_biaya.Where(f => f.JenisPengeluaran != null))
                    {
                        item.entity_state = EntityState.Unchanged;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Delete(PengeluaranBiaya obj)
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

        public IList<ItemPengeluaranBiaya> GetItemPengeluaranBiaya(string pengeluaranBiayaId)
        {
            throw new NotImplementedException();
        }
    }
}     
