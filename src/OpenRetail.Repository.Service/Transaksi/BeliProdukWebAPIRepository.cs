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
    public class BeliProdukWebAPIRepository : IBeliProdukRepository
    {
        private string _apiUrl = string.Empty;
        private ILog _log;

        public BeliProdukWebAPIRepository(string baseUrl, ILog log)
        {
            this._apiUrl = baseUrl + "api/beli_produk/";
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

        public BeliProduk GetByID(string id)
        {
            BeliProduk obj = null;

            try
            {
                var api = string.Format("get_by_id?id={0}", id);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<BeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    obj = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<BeliProduk> GetAll()
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                var api = "get_all";
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<BeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetAll(string name)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                var api = string.Format("get_all_filter_by?name={0}", name);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<BeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetAll(int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                var api = string.Format("get_all_with_paging?pageNumber={0}&pageSize={1}", pageNumber, pageSize);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<BeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                {
                    pagesCount = response.Status.PagesCount;
                    oList = response.Results;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetNotaSupplier(string id, string nota)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                var api = string.Format("get_nota_supplier?id={0}&nota={1}", id, nota);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<BeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetNotaKreditBySupplier(string id, bool isLunas)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                var api = string.Format("get_nota_kredit_supplier_by_status?id={0}&isLunas={1}", id, isLunas);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<BeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetNotaKreditByNota(string id, string nota)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                var api = string.Format("get_nota_kredit_supplier_by_nota?id={0}&nota={1}", id, nota);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<BeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetByName(string name)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                var api = string.Format("get_by_name?name={0}", name);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<BeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetByName(string name, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                var api = string.Format("get_by_name_with_paging?name={0}&pageNumber={1}&pageSize={2}", name, pageNumber, pageSize);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<BeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                {
                    pagesCount = response.Status.PagesCount;
                    oList = response.Results;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                var api = string.Format("get_by_tanggal?tanggalMulai={0}&tanggalSelesai={1}", tanggalMulai, tanggalSelesai);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<BeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                var api = string.Format("get_by_tanggal_with_paging?tanggalMulai={0}&tanggalSelesai={1}&pageNumber={2}&pageSize={3}", tanggalMulai, tanggalSelesai, pageNumber, pageSize);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<BeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                {
                    pagesCount = response.Status.PagesCount;
                    oList = response.Results;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, string name)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                var api = string.Format("get_by_tanggal_with_name?tanggalMulai={0}&tanggalSelesai={1}&name={2}", tanggalMulai, tanggalSelesai, name);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<BeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ItemBeliProduk> GetItemBeli(string beliId)
        {
            IList<ItemBeliProduk> oList = new List<ItemBeliProduk>();

            try
            {
                var api = string.Format("get_item_beli?beliId={0}", beliId);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<ItemBeliProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        private double GetTotalNota(BeliProduk obj)
        {
            var total = obj.item_beli.Where(f => f.Produk != null && f.entity_state != EntityState.Deleted)
                                     .Sum(f => (f.jumlah - f.jumlah_retur) * (f.harga - (f.diskon / 100 * f.harga)));

            return Math.Round(total, MidpointRounding.AwayFromZero);
        }

        public int Save(BeliProduk obj)
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
                    obj.total_nota = GetTotalNota(obj);

                    // jika pembelian tunai, langsung insert ke pembayaran hutang
                    if (obj.tanggal_tempo.IsNull())
                        obj.total_pelunasan = obj.grand_total;

                    foreach (var item in obj.item_beli.Where(f => f.Produk != null))
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

        public int Update(BeliProduk obj)
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
                    obj.total_nota = GetTotalNota(obj);

                    // jika terjadi perubahan status nota dari tunai ke kredit
                    if (obj.tanggal_tempo_old.IsNull() && !obj.tanggal_tempo.IsNull())
                    {
                        obj.total_pelunasan = 0;
                    }
                    else if (obj.tanggal_tempo.IsNull()) // jika penjualan tunai, langsung update ke pembayaran piutang
                    {
                        obj.total_pelunasan = obj.grand_total;
                    }

                    foreach (var item in obj.item_beli.Where(f => f.Produk != null))
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

        public int Delete(BeliProduk obj)
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