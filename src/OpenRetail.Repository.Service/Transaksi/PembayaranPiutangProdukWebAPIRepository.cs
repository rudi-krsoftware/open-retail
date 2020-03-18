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
    public class PembayaranPiutangProdukWebAPIRepository : IPembayaranPiutangProdukRepository
    {
        private string _apiUrl = string.Empty;
        private ILog _log;

        public PembayaranPiutangProdukWebAPIRepository(string baseUrl, ILog log)
        {
            this._apiUrl = baseUrl + "api/pembayaran_piutang/";
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

        public PembayaranPiutangProduk GetByID(string id)
        {
            PembayaranPiutangProduk obj = null;

            try
            {
                var api = string.Format("get_by_id?id={0}", id);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PembayaranPiutangProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    obj = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public ItemPembayaranPiutangProduk GetByJualID(string id)
        {
            ItemPembayaranPiutangProduk obj = null;

            try
            {
                var api = string.Format("get_by_jual_id?id={0}", id);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<ItemPembayaranPiutangProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    obj = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<PembayaranPiutangProduk> GetByName(string name)
        {
            IList<PembayaranPiutangProduk> oList = new List<PembayaranPiutangProduk>();

            try
            {
                var api = string.Format("get_by_name?name={0}", name);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PembayaranPiutangProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PembayaranPiutangProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PembayaranPiutangProduk> oList = new List<PembayaranPiutangProduk>();

            try
            {
                var api = string.Format("get_by_tanggal?tanggalMulai={0}&tanggalSelesai={1}", tanggalMulai, tanggalSelesai);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PembayaranPiutangProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ItemPembayaranPiutangProduk> GetHistoriPembayaran(string jualId)
        {
            IList<ItemPembayaranPiutangProduk> oList = new List<ItemPembayaranPiutangProduk>();

            try
            {
                var api = string.Format("get_histori_pembayaran?jualId={0}", jualId);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<ItemPembayaranPiutangProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PembayaranPiutangProduk> GetAll()
        {
            IList<PembayaranPiutangProduk> oList = new List<PembayaranPiutangProduk>();

            try
            {
                var api = "get_all";
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<PembayaranPiutangProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(PembayaranPiutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Save(PembayaranPiutangProduk obj, bool isSaveFromPenjualan)
        {
            var result = 0;

            try
            {
                obj.tanggal = obj.tanggal.ToUtc();

                var api = string.Format("save?isSaveFromPenjualan={0}", isSaveFromPenjualan);
                var response = RestSharpHelper<OpenRetailWebApiPostResponse>.PostRequest(_apiUrl, api, obj);

                result = Convert.ToInt32(response.Results);

                if (result > 0)
                {
                    foreach (var item in obj.item_pembayaran_piutang.Where(f => f.JualProduk != null))
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

        public int Update(PembayaranPiutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Update(PembayaranPiutangProduk obj, bool isUpdateFromPenjualan)
        {
            var result = 0;

            try
            {
                obj.tanggal = obj.tanggal.ToUtc();

                var api = string.Format("update?isUpdateFromPenjualan={0}", isUpdateFromPenjualan);
                var response = RestSharpHelper<OpenRetailWebApiPostResponse>.PostRequest(_apiUrl, api, obj);

                result = Convert.ToInt32(response.Results);

                if (result > 0)
                {
                    foreach (var item in obj.item_pembayaran_piutang.Where(f => f.JualProduk != null))
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

        public int Delete(PembayaranPiutangProduk obj)
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