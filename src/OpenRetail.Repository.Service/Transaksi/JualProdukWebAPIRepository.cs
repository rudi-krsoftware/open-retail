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
    public class JualProdukWebAPIRepository : IJualProdukRepository
    {
        private string _apiUrl = string.Empty;
        private ILog _log;

        public JualProdukWebAPIRepository(string baseUrl, ILog log)
        {
            this._apiUrl = baseUrl + "api/jual_produk/";
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

        public JualProduk GetByID(string id)
        {
            JualProduk obj = null;

            try
            {
                var api = string.Format("get_by_id?id={0}", id);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    obj = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public JualProduk GetListItemNotaTerakhir(string penggunaId, string mesinId)
        {
            JualProduk obj = null;

            try
            {
                var api = string.Format("get_list_item_nota_terakhir?penggunaId={0}&mesinId={1}", penggunaId, mesinId);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    obj = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<JualProduk> GetAll()
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                var api = "get_all";
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<JualProduk> GetAll(string name)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                var api = string.Format("get_all_filter_by?name={0}", name);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<JualProduk> GetAll(int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                var api = string.Format("get_all_with_paging?pageNumber={0}&pageSize={1}", pageNumber, pageSize);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

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

        public IList<JualProduk> GetNotaCustomer(string id, string nota)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                var api = string.Format("get_nota_customer?id={0}&nota={1}", id, nota);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<JualProduk> GetNotaKreditByCustomer(string id, bool isLunas)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                var api = string.Format("get_nota_kredit_customer_by_status?id={0}&isLunas={1}", id, isLunas);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<JualProduk> GetNotaKreditByNota(string id, string nota)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                var api = string.Format("get_nota_kredit_customer_by_nota?id={0}&nota={1}", id, nota);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<JualProduk> GetByName(string name)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                var api = string.Format("get_by_name?name={0}", name);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<JualProduk> GetByName(string name, bool isCekKeteranganItemJual, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                var api = string.Format("get_by_name_with_paging?name={0}&isCekKeteranganItemJual={1}&pageNumber={2}&pageSize={3}", name, isCekKeteranganItemJual, pageNumber, pageSize);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

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

        public IList<JualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                var api = string.Format("get_by_tanggal?tanggalMulai={0}&tanggalSelesai={1}", tanggalMulai, tanggalSelesai);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<JualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                var api = string.Format("get_by_tanggal_with_paging?tanggalMulai={0}&tanggalSelesai={1}&pageNumber={2}&pageSize={3}", tanggalMulai, tanggalSelesai, pageNumber, pageSize);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

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

        public IList<JualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, string name)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                var api = string.Format("get_by_tanggal_with_name?tanggalMulai={0}&tanggalSelesai={1}&name={2}", tanggalMulai, tanggalSelesai, name);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<JualProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ItemJualProduk> GetItemJual(string jualId)
        {
            IList<ItemJualProduk> oList = new List<ItemJualProduk>();

            try
            {
                var api = string.Format("get_item_jual?jualId={0}", jualId);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<ItemJualProduk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        private double GetTotalNota(JualProduk obj)
        {
            var total = obj.item_jual.Where(f => f.Produk != null && f.entity_state != EntityState.Deleted)
                                     .Sum(f => (f.jumlah - f.jumlah_retur) * (f.harga_jual - (f.diskon / 100 * f.harga_jual)));

            return Math.Round(total, MidpointRounding.AwayFromZero);
        }

        public int Save(JualProduk obj)
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

                    foreach (var item in obj.item_jual.Where(f => f.Produk != null))
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

        public int Update(JualProduk obj)
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

                    foreach (var item in obj.item_jual.Where(f => f.Produk != null))
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

        public int Delete(JualProduk obj)
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


        public IList<JualProduk> GetByLimit(DateTime tanggalMulai, DateTime tanggalSelesai, int limit)
        {
            throw new NotImplementedException();
        }
    }
}
