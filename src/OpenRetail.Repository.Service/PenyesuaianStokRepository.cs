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
using Dapper.Contrib.Extensions;

using OpenRetail.Model;
using OpenRetail.Repository.Api;
 
namespace OpenRetail.Repository.Service
{        
    public class PenyesuaianStokRepository : IPenyesuaianStokRepository
    {
        private const string SQL_TEMPLATE = @"SELECT t_penyesuaian_stok.penyesuaian_stok_id, t_penyesuaian_stok.tanggal, t_penyesuaian_stok.penambahan_stok, t_penyesuaian_stok.pengurangan_stok, 
                                              t_penyesuaian_stok.penambahan_stok_gudang, t_penyesuaian_stok.pengurangan_stok_gudang, t_penyesuaian_stok.keterangan, t_penyesuaian_stok.tanggal_sistem, 
                                              m_produk.produk_id, m_produk.kode_produk, m_produk.nama_produk, m_produk.satuan, m_produk.stok, m_produk.stok_gudang,
                                              m_alasan_penyesuaian_stok.alasan_penyesuaian_stok_id, m_alasan_penyesuaian_stok.alasan
                                              FROM public.m_alasan_penyesuaian_stok INNER JOIN public.t_penyesuaian_stok ON t_penyesuaian_stok.alasan_penyesuaian_id = m_alasan_penyesuaian_stok.alasan_penyesuaian_stok_id 
                                              INNER JOIN public.m_produk ON t_penyesuaian_stok.produk_id = m_produk.produk_id
                                              {WHERE}
                                              {ORDER BY}";

        private IDapperContext _context;
		private ILog _log;
        private string _sql;

        public PenyesuaianStokRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<PenyesuaianStok> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<PenyesuaianStok> oList = _context.db.Query<PenyesuaianStok, Produk, AlasanPenyesuaianStok, PenyesuaianStok>(sql, (ps, p, ap) =>
            {
                ps.produk_id = p.produk_id; ps.Produk = p;
                ps.alasan_penyesuaian_id = ap.alasan_penyesuaian_stok_id; ps.AlasanPenyesuaianStok = ap;

                return ps;
            }, param, splitOn: "produk_id, alasan_penyesuaian_stok_id");

            return oList;
        }

        public PenyesuaianStok GetByID(string id)
        {
            PenyesuaianStok obj = null;

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_penyesuaian_stok.penyesuaian_stok_id = @id");
                _sql = _sql.Replace("{ORDER BY}", "");

                obj = MappingRecordToObject(_sql, new { id }).SingleOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<PenyesuaianStok> GetByName(string name)
        {
            IList<PenyesuaianStok> oList = new List<PenyesuaianStok>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE LOWER(m_produk.nama_produk) LIKE @name OR LOWER(t_penyesuaian_stok.keterangan) LIKE @name");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_penyesuaian_stok.tanggal");

                name = "%" + name.ToLower() + "%";

                oList = MappingRecordToObject(_sql, new { name }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PenyesuaianStok> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PenyesuaianStok> oList = new List<PenyesuaianStok>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_penyesuaian_stok.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_penyesuaian_stok.tanggal");

                oList = MappingRecordToObject(_sql, new { tanggalMulai, tanggalSelesai }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PenyesuaianStok> GetAll()
        {
            IList<PenyesuaianStok> oList = new List<PenyesuaianStok>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_penyesuaian_stok.tanggal");

                oList = MappingRecordToObject(_sql).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(PenyesuaianStok obj)
        {
            var result = 0;

            try
            {
                obj.penyesuaian_stok_id = _context.GetGUID();

                _context.db.Insert<PenyesuaianStok>(obj);
                result = 1;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Update(PenyesuaianStok obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Update<PenyesuaianStok>(obj) ? 1 : 0;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Delete(PenyesuaianStok obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<PenyesuaianStok>(obj) ? 1 : 0;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }        
    }
}     
