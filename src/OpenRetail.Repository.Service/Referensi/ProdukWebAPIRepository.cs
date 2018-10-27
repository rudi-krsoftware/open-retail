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
    public class ProdukWebAPIRepository : IProdukRepository
    {
        private string _apiUrl = string.Empty;
        private ILog _log;
		
        public ProdukWebAPIRepository(string baseUrl, ILog log)
        {
            this._apiUrl = baseUrl + "api/produk/";
            this._log = log;
        }

        public Produk GetByID(string id)
        {
            Produk obj = null;

			try
            {
                var api = string.Format("get_by_id?id={0}", id);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Produk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    obj = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public Produk GetByKode(string kodeProduk, bool isCekStatusAktif = false)
        {
            Produk obj = null;

            try
            {
                var api = string.Format("get_by_kode?kodeProduk={0}&isCekStatusAktif={1}", kodeProduk, isCekStatusAktif);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Produk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    obj = response.Results[0];
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public string GetLastKodeProduk()
        {
            var result = string.Empty;

            try
            {
                var api = "get_last_kode_produk";
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

        public IList<Produk> GetByName(string name, bool isLoadHargaGrosir = true, bool isCekStatusAktif = false)
        {
            IList<Produk> oList = new List<Produk>();

			try
            {
                var api = string.Format("get_by_name?name={0}&isLoadHargaGrosir={1}&isCekStatusAktif={2}", name, isLoadHargaGrosir, isCekStatusAktif);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Produk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<Produk> GetByName(string name, string sortBy, int pageNumber, int pageSize, ref int pagesCount, bool isLoadHargaGrosir = true)
        {
            IList<Produk> oList = new List<Produk>();

            try
            {
                var api = string.Format("get_by_name_with_paging?name={0}&sortBy={1}&pageNumber={2}&pageSize={3}&isLoadHargaGrosir={4}", name, sortBy, pageNumber, pageSize, isLoadHargaGrosir);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Produk>>.GetRequest(_apiUrl, api).Data;

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

        public IList<Produk> GetByGolongan(string golonganId)
        {
            IList<Produk> oList = new List<Produk>();

            try
            {
                var api = string.Format("get_by_golongan?golonganId={0}", golonganId);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Produk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<Produk> GetByGolongan(string golonganId, string sortBy, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<Produk> oList = new List<Produk>();

            try
            {
                var api = string.Format("get_by_golongan_with_paging?golonganId={0}&sortBy={1}&pageNumber={2}&pageSize={3}", golonganId, sortBy, pageNumber, pageSize);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Produk>>.GetRequest(_apiUrl, api).Data;

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

        public IList<Produk> GetInfoMinimalStok()
        {
            IList<Produk> oList = new List<Produk>();

            try
            {
                var api = "get_info_minimal_stok";
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Produk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }        

        public IList<Produk> GetAll()
        {
            IList<Produk> oList = new List<Produk>();

			try
            {
                var api = "get_all";
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Produk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<Produk> GetAll(string sortBy)
        {
            IList<Produk> oList = new List<Produk>();

            try
            {
                var api = string.Format("get_all_sort_by?sortBy={0}", sortBy);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Produk>>.GetRequest(_apiUrl, api).Data;

                if (response.Results.Count > 0)
                    oList = response.Results;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<Produk> GetAll(string sortBy, int pageNumber, int pageSize, ref int pagesCount)
        {
            IList<Produk> oList = new List<Produk>();

            try
            {
                var api = string.Format("get_all_with_paging?sortBy={0}&pageNumber={1}&pageSize={2}", sortBy, pageNumber, pageSize);
                var response = RestSharpHelper<OpenRetailWebApiGetResponse<Produk>>.GetRequest(_apiUrl, api).Data;

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

        public int Save(Produk obj)
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

        public int Update(Produk obj)
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

        public int Delete(Produk obj)
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
