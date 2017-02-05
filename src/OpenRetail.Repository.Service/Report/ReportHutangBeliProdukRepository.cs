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

using log4net;
using Dapper;
using OpenRetail.Model;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Api.Report;

namespace OpenRetail.Repository.Service.Report
{
    public class ReportHutangBeliProdukRepository : IReportHutangBeliProdukRepository
    {
        private const string SQL_TEMPLATE_HEADER = @"SELECT SUM(t_beli_produk.ppn) AS ppn, SUM(t_beli_produk.diskon) AS diskon, SUM(t_beli_produk.total_nota) AS total_nota, SUM(t_beli_produk.total_pelunasan) AS total_pelunasan,
                                                     m_supplier.supplier_id, m_supplier.nama_supplier
                                                     FROM public.m_supplier INNER JOIN public.t_beli_produk ON t_beli_produk.supplier_id = m_supplier.supplier_id                                                     
                                                     {WHERE}
                                                     GROUP BY m_supplier.supplier_id, m_supplier.nama_supplier
                                                     HAVING (SUM(t_beli_produk.total_nota - t_beli_produk.diskon + t_beli_produk.ppn) - SUM(t_beli_produk.total_pelunasan)) <> 0
                                                     ORDER BY m_supplier.nama_supplier";

        private const string SQL_TEMPLATE_DETAIL = @"SELECT t_beli_produk.beli_produk_id, t_beli_produk.nota, t_beli_produk.tanggal, t_beli_produk.tanggal_tempo, t_beli_produk.ppn, t_beli_produk.diskon, t_beli_produk.total_nota, t_beli_produk.total_pelunasan, 
                                                     m_supplier.supplier_id, m_supplier.nama_supplier
                                                     FROM public.m_supplier INNER JOIN public.t_beli_produk ON t_beli_produk.supplier_id = m_supplier.supplier_id
                                                     {WHERE}
                                                     ORDER BY m_supplier.nama_supplier, t_beli_produk.tanggal, t_beli_produk.nota";

        private IDapperContext _context;
        private ILog _log;
        private string _sql;
        private string _where;

        public ReportHutangBeliProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<BeliProduk> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<BeliProduk> oList = _context.db.Query<BeliProduk, Supplier, BeliProduk>(sql, (b, s) =>
            {
                b.supplier_id = s.supplier_id; b.Supplier = s;

                return b;
            }, param, splitOn: "supplier_id");

            return oList;
        }

        public IList<BeliProduk> GetByBulan(int bulan, int tahun)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            //try
            //{
                _where = @"WHERE t_beli_produk.tanggal_tempo IS NOT NULL AND 
                           EXTRACT(MONTH FROM t_beli_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun";

                _sql = SQL_TEMPLATE_HEADER.Replace("{WHERE}", _where);

                oList = MappingRecordToObject(_sql, new { bulan, tahun }).ToList();
            //}
            //catch (Exception ex)
            //{
            //    _log.Error("Error:", ex);
            //}

            return oList;
        }

        public IList<BeliProduk> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                _where = @"WHERE t_beli_produk.tanggal_tempo IS NOT NULL AND 
                           (EXTRACT(MONTH FROM t_beli_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir) AND EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun";

                _sql = SQL_TEMPLATE_HEADER.Replace("{WHERE}", _where);

                oList = MappingRecordToObject(_sql, new { bulanAwal, bulanAkhir, tahun }).ToList();
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
                _where = @"WHERE t_beli_produk.tanggal_tempo IS NOT NULL AND 
                           t_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai";

                _sql = SQL_TEMPLATE_HEADER.Replace("{WHERE}", _where);

                oList = MappingRecordToObject(_sql, new { tanggalMulai, tanggalSelesai }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> DetailGetByBulan(int bulan, int tahun)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                _where = @"WHERE t_beli_produk.tanggal_tempo IS NOT NULL AND
                           ((t_beli_produk.total_nota - t_beli_produk.diskon + t_beli_produk.ppn) - t_beli_produk.total_pelunasan) <> 0 AND 
                           EXTRACT(MONTH FROM t_beli_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun";

                _sql = SQL_TEMPLATE_DETAIL.Replace("{WHERE}", _where);

                oList = MappingRecordToObject(_sql, new { bulan, tahun }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> DetailGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                _where = @"WHERE t_beli_produk.tanggal_tempo IS NOT NULL AND
                           ((t_beli_produk.total_nota - t_beli_produk.diskon + t_beli_produk.ppn) - t_beli_produk.total_pelunasan) <> 0 AND 
                           (EXTRACT(MONTH FROM t_beli_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir) AND EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun";

                _sql = SQL_TEMPLATE_DETAIL.Replace("{WHERE}", _where);

                oList = MappingRecordToObject(_sql, new { bulanAwal, bulanAkhir, tahun }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> DetailGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                _where = @"WHERE t_beli_produk.tanggal_tempo IS NOT NULL AND
                           ((t_beli_produk.total_nota - t_beli_produk.diskon + t_beli_produk.ppn) - t_beli_produk.total_pelunasan) <> 0 AND 
                           t_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai";

                _sql = SQL_TEMPLATE_DETAIL.Replace("{WHERE}", _where);

                oList = MappingRecordToObject(_sql, new { tanggalMulai, tanggalSelesai }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }        
    }
}
