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
    public class PembayaranHutangProdukWebAPIRepository : IPembayaranHutangProdukRepository
    {
        private string _apiUrl = string.Empty;
        private ILog _log;

        public PembayaranHutangProdukWebAPIRepository(string baseUrl, ILog log)
        {
            this._apiUrl = baseUrl + "api/pembayaran_hutang/";
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

        public PembayaranHutangProduk GetByID(string id)
        {
            PembayaranHutangProduk obj = null;

            try
            {
                var api = string.Format("get_by_id?id={0}", id);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PembayaranHutangProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    obj = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public ItemPembayaranHutangProduk GetByBeliID(string id)
        {
            ItemPembayaranHutangProduk obj = null;

            try
            {
                var api = string.Format("get_by_beli_id?id={0}", id);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<ItemPembayaranHutangProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    obj = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<PembayaranHutangProduk> GetByName(string name)
        {
            IList<PembayaranHutangProduk> oList = new List<PembayaranHutangProduk>();

            try
            {
                var api = string.Format("get_by_name?name={0}", name);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PembayaranHutangProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PembayaranHutangProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PembayaranHutangProduk> oList = new List<PembayaranHutangProduk>();

            try
            {
                var api = string.Format("get_by_tanggal?tanggalMulai={0}&tanggalSelesai={1}", tanggalMulai, tanggalSelesai);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PembayaranHutangProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ItemPembayaranHutangProduk> GetHistoriPembayaran(string beliId)
        {
            IList<ItemPembayaranHutangProduk> oList = new List<ItemPembayaranHutangProduk>();

            try
            {
                var api = string.Format("get_histori_pembayaran?beliId={0}", beliId);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<ItemPembayaranHutangProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PembayaranHutangProduk> GetAll()
        {
            IList<PembayaranHutangProduk> oList = new List<PembayaranHutangProduk>();

            try
            {
                var api = "get_all";
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PembayaranHutangProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(PembayaranHutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Save(PembayaranHutangProduk obj, bool isSaveFromPembelian)
        {
            var result = 0;

            try
            {
                obj.tanggal = obj.tanggal.ToUtc();

                var api = string.Format("save?isSaveFromPembelian={0}", isSaveFromPembelian);
                var response = RestSharpHelper<OpenRetailWebApiPostResponse>.PostRequest(_apiUrl, api, obj);

                result = Convert.ToInt32(response.Results);

                if (result > 0)
                {
                    foreach (var item in obj.item_pembayaran_hutang.Where(f => f.BeliProduk != null))
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

        public int Update(PembayaranHutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Update(PembayaranHutangProduk obj, bool isUpdateFromPembelian)
        {
            var result = 0;

            try
            {
                obj.tanggal = obj.tanggal.ToUtc();

                var api = string.Format("update?isUpdateFromPembelian={0}", isUpdateFromPembelian);
                var response = RestSharpHelper<OpenRetailWebApiPostResponse>.PostRequest(_apiUrl, api, obj);

                result = Convert.ToInt32(response.Results);

                if (result > 0)
                {
                    foreach (var item in obj.item_pembayaran_hutang.Where(f => f.BeliProduk != null))
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

        public int Delete(PembayaranHutangProduk obj)
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