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
    public class ReportBeliProdukRepository : IReportBeliProdukRepository
    {
        private const string SQL_TEMPLATE_HEADER = @"SELECT t_beli_produk.beli_produk_id, t_beli_produk.nota, t_beli_produk.tanggal, t_beli_produk.tanggal_tempo, t_beli_produk.ppn, t_beli_produk.diskon, t_beli_produk.total_nota, t_beli_produk.total_pelunasan, t_beli_produk.keterangan,   
                                                     m_supplier.supplier_id, m_supplier.nama_supplier, m_pengguna.pengguna_id, m_pengguna.nama_pengguna
                                                     FROM public.t_beli_produk INNER JOIN public.m_pengguna ON t_beli_produk.pengguna_id = m_pengguna.pengguna_id
                                                     INNER JOIN public.m_supplier ON m_supplier.supplier_id = t_beli_produk.supplier_id
                                                     {WHERE}
                                                     {ORDER BY}";

        private const string SQL_TEMPLATE_DETAIL = @"SELECT t_item_beli_produk.item_beli_produk_id, t_item_beli_produk.jumlah, t_item_beli_produk.jumlah_retur, t_item_beli_produk.harga, t_item_beli_produk.diskon, 
                                                     m_produk.produk_id, m_produk.nama_produk, m_produk.satuan, t_beli_produk.beli_produk_id, t_beli_produk.tanggal, t_beli_produk.nota, 
                                                     m_supplier.supplier_id, m_supplier.nama_supplier
                                                     FROM public.t_beli_produk INNER JOIN public.m_supplier ON m_supplier.supplier_id = t_beli_produk.supplier_id
                                                     INNER JOIN public.t_item_beli_produk ON t_item_beli_produk.beli_produk_id = t_beli_produk.beli_produk_id
                                                     INNER JOIN public.m_produk ON t_item_beli_produk.produk_id = m_produk.produk_id
                                                     {WHERE}
                                                     {ORDER BY}";

        private IDapperContext _context;
        private ILog _log;
        private string _sql;

        public ReportBeliProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<BeliProduk> MappingRecordToObjectHeader(string sql, object param = null)
        {
            IEnumerable<BeliProduk> oList = _context.db.Query<BeliProduk, Supplier, Pengguna, BeliProduk>(sql, (b, s, p) =>
            {
                b.supplier_id = s.supplier_id; b.Supplier = s;
                b.pengguna_id = p.pengguna_id; b.Pengguna = p;

                return b;
            }, param, splitOn: "supplier_id, pengguna_id");

            return oList;
        }

        private IList<ItemBeliProduk> MappingRecordToObjectDetail(string sql, object param = null)
        {
            IList<ItemBeliProduk> oList = _context.db.Query<ItemBeliProduk, Produk, BeliProduk, Supplier, ItemBeliProduk>(sql, (i, p, b, s) =>
            {
                b.supplier_id = s.supplier_id; b.Supplier = s;
                i.produk_id = p.produk_id; i.Produk = p;
                i.beli_produk_id = b.beli_produk_id; i.BeliProduk = b;

                return i;
            }, param, splitOn: "produk_id, beli_produk_id, supplier_id").ToList();

            return oList;
        }

        public IList<BeliProduk> GetByBulan(int bulan, int tahun)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                _sql = SQL_TEMPLATE_HEADER.Replace("{WHERE}", "WHERE EXTRACT(MONTH FROM t_beli_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota");

                oList = MappingRecordToObjectHeader(_sql, new { bulan, tahun }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                _sql = SQL_TEMPLATE_HEADER.Replace("{WHERE}", "WHERE (EXTRACT(MONTH FROM t_beli_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir) AND EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota");

                oList = MappingRecordToObjectHeader(_sql, new { bulanAwal, bulanAkhir, tahun }).ToList();
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
                _sql = SQL_TEMPLATE_HEADER.Replace("{WHERE}", "WHERE t_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota");

                oList = MappingRecordToObjectHeader(_sql, new { tanggalMulai, tanggalSelesai }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ItemBeliProduk> DetailGetByBulan(int bulan, int tahun)
        {
            IList<ItemBeliProduk> oList = new List<ItemBeliProduk>();

            try
            {
                _sql = SQL_TEMPLATE_DETAIL.Replace("{WHERE}", "WHERE EXTRACT(MONTH FROM t_beli_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota, m_produk.nama_produk");

                oList = MappingRecordToObjectDetail(_sql, new { bulan, tahun }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ItemBeliProduk> DetailGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ItemBeliProduk> oList = new List<ItemBeliProduk>();

            try
            {
                _sql = SQL_TEMPLATE_DETAIL.Replace("{WHERE}", "WHERE (EXTRACT(MONTH FROM t_beli_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir) AND EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota, m_produk.nama_produk");

                oList = MappingRecordToObjectDetail(_sql, new { bulanAwal, bulanAkhir, tahun }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ItemBeliProduk> DetailGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ItemBeliProduk> oList = new List<ItemBeliProduk>();

            try
            {
                _sql = SQL_TEMPLATE_DETAIL.Replace("{WHERE}", "WHERE t_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota, m_produk.nama_produk");

                oList = MappingRecordToObjectDetail(_sql, new { tanggalMulai, tanggalSelesai }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }        
    }
}
