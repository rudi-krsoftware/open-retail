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
using Dapper;

using System.Data;
using OpenRetail.Model;
using OpenRetail.Repository.Api;
 
namespace OpenRetail.Repository.Service
{        
    public class HargaGrosirRepository : IHargaGrosirRepository
    {
        private IDapperContext _context;
		private ILog _log;
		
        public HargaGrosirRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public HargaGrosir GetHargaGrosir(string produkId, int hargaKe, IDbTransaction transaction = null)
        {
            HargaGrosir obj = null;

            try
            {
                var sql = @"SELECT harga_grosir_id, produk_id, harga_ke, harga_grosir, jumlah_minimal, diskon 
                            FROM m_harga_grosir 
                            WHERE produk_id = @produkId AND harga_ke = @hargaKe";

                obj = _context.db.Query<HargaGrosir>(sql, new { produkId, hargaKe }, transaction)
                              .SingleOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<HargaGrosir> GetListHargaGrosir(string produkId)
        {
            IList<HargaGrosir> oList = new List<HargaGrosir>();

            try
            {
                var sql = @"SELECT harga_grosir_id, produk_id, harga_ke, harga_grosir, jumlah_minimal, diskon 
                            FROM m_harga_grosir 
                            WHERE produk_id = @produkId
                            ORDER BY harga_ke";

                oList = _context.db.Query<HargaGrosir>(sql, new { produkId })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }
    }
}     
