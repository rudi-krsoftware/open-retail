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

using OpenRetail.Model;
 
namespace OpenRetail.Repository.Api
{        
    public interface IJualProdukRepository : IBaseRepository<JualProduk>
    {
        string GetLastNota();
		JualProduk GetByID(string id);

        IList<JualProduk> GetAll(string name);

        /// <summary>
        /// Method untuk mendapatkan informasi pembelian masing-masing supplier
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="nota"></param>
        /// <returns></returns>
        IList<JualProduk> GetNotaCustomer(string id, string nota);

        /// <summary>
        /// Method untuk mendapatkan informasi pembelian kredit yang lunas/belum masing-masing supplier
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="isLunas"></param>
        /// <returns></returns>
        IList<JualProduk> GetNotaKreditByCustomer(string id, bool isLunas);

        /// <summary>
        /// Method untuk mendapatkan informasi pembelian kredit yang lunas/belum masing-masing customer berdasarkan nota
        /// </summary>
        /// <param name="id">customer id</param>
        /// <param name="nota"></param>
        /// <returns></returns>
        IList<JualProduk> GetNotaKreditByNota(string id, string nota);

        IList<JualProduk> GetByName(string name);
        IList<JualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai);
        IList<JualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, string name);

        IList<ItemJualProduk> GetItemJual(string jualId);
    }
}     
