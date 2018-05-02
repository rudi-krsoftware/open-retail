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

        public PengeluaranBiaya GetByID(string id)
        {
            PengeluaranBiaya obj = null;

			try
            {
                var api = string.Format("get_by_id?id={0}", id);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PengeluaranBiaya>>.GetRequest(_apiUrl, api).Data;

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
                var api = string.Format("get_by_tanggal?tanggal_mulai={0}&tanggal_selesai={1}", tanggalMulai, tanggalSelesai);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PengeluaranBiaya>>.GetRequest(_apiUrl, api).Data;

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
                var api = "get_all";
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PengeluaranBiaya>>.GetRequest(_apiUrl, api).Data;

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
                obj.tanggal = obj.tanggal.ToUtc();

                var api = "save";
                var response = RestSharpHelper<OpenRetailWebApiPostResponse>.PostRequest(_apiUrl, api, obj);

                result = Convert.ToInt32(response.Results);

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
                obj.tanggal = obj.tanggal.ToUtc();

                var api = "update";
                var response = RestSharpHelper<OpenRetailWebApiPostResponse>.PostRequest(_apiUrl, api, obj);

                result = Convert.ToInt32(response.Results);

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

        public IList<ItemPengeluaranBiaya> GetItemPengeluaranBiaya(string pengeluaranBiayaId)
        {
            throw new NotImplementedException();
        }
    }
}     
