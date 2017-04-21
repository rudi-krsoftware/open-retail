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
using OpenRetail.Model.Report;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Api.Report;

namespace OpenRetail.Repository.Service.Report
{
    public class ReportPiutangJualProdukRepository : IReportPiutangJualProdukRepository
    {
        private const string SQL_TEMPLATE_HEADER = @"SELECT m_customer.customer_id, m_customer.nama_customer, SUM(t_jual_produk.ppn) AS ppn, SUM(t_jual_produk.ongkos_kirim) AS ongkos_kirim, SUM(t_jual_produk.diskon) AS diskon, 
                                                     SUM(t_jual_produk.total_nota) AS total_nota, SUM(t_jual_produk.total_pelunasan) AS total_pelunasan
                                                     FROM public.m_customer RIGHT JOIN public.t_jual_produk ON t_jual_produk.customer_id = m_customer.customer_id
                                                     {WHERE}
                                                     GROUP BY m_customer.customer_id, m_customer.nama_customer
                                                     HAVING SUM(t_jual_produk.total_nota - t_jual_produk.diskon + t_jual_produk.ppn + t_jual_produk.ongkos_kirim) - SUM(t_jual_produk.total_pelunasan) <> 0
                                                     ORDER BY m_customer.nama_customer";

        private const string SQL_TEMPLATE_DETAIL = @"SELECT m_customer.customer_id, m_customer.nama_customer, t_jual_produk.nota, t_jual_produk.tanggal, t_jual_produk.tanggal_tempo, 
                                                     t_jual_produk.ppn, t_jual_produk.ongkos_kirim, t_jual_produk.diskon, t_jual_produk.total_nota, t_jual_produk.total_pelunasan
                                                     FROM public.m_customer RIGHT JOIN public.t_jual_produk ON t_jual_produk.customer_id = m_customer.customer_id
                                                     {WHERE}
                                                     ORDER BY m_customer.nama_customer, t_jual_produk.tanggal, t_jual_produk.nota";

        private const string SQL_TEMPLATE_PER_PRODUK = @"SELECT m_customer.customer_id, m_customer.nama_customer,
                                                         t_jual_produk.jual_id, t_jual_produk.nota, t_jual_produk.tanggal, t_jual_produk.tanggal_tempo, t_jual_produk.ppn, t_jual_produk.ongkos_kirim, t_jual_produk.diskon AS diskon_nota, 
                                                         t_jual_produk.total_nota, t_jual_produk.total_pelunasan,
                                                         m_produk.produk_id, m_produk.nama_produk, m_produk.satuan, t_item_jual_produk.jumlah, t_item_jual_produk.jumlah_retur, t_item_jual_produk.diskon, t_item_jual_produk.harga_jual
                                                         FROM public.t_jual_produk INNER JOIN public.t_item_jual_produk ON t_item_jual_produk.jual_id = t_jual_produk.jual_id
                                                         LEFT JOIN public.m_customer ON t_jual_produk.customer_id = m_customer.customer_id
                                                         INNER JOIN public.m_produk ON t_item_jual_produk.produk_id = m_produk.produk_id
                                                         {WHERE}
                                                         ORDER BY m_customer.nama_customer, t_jual_produk.tanggal, t_jual_produk.nota, m_produk.nama_produk";

        private IDapperContext _context;
        private ILog _log;

        public ReportPiutangJualProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ReportPiutangPenjualanProdukHeader> GetByBulan(int bulan, int tahun)
        {
            IList<ReportPiutangPenjualanProdukHeader> oList = new List<ReportPiutangPenjualanProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("t_jual_produk.tanggal_tempo IS NOT NULL");
                whereBuilder.Add("EXTRACT(MONTH FROM t_jual_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPiutangPenjualanProdukHeader>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPiutangPenjualanProdukHeader> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPiutangPenjualanProdukHeader> oList = new List<ReportPiutangPenjualanProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("t_jual_produk.tanggal_tempo IS NOT NULL");
                whereBuilder.Add("(EXTRACT(MONTH FROM t_jual_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");
                
                oList = _context.db.Query<ReportPiutangPenjualanProdukHeader>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPiutangPenjualanProdukHeader> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPiutangPenjualanProdukHeader> oList = new List<ReportPiutangPenjualanProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("t_jual_produk.tanggal_tempo IS NOT NULL");
                whereBuilder.Add("t_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPiutangPenjualanProdukHeader>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPiutangPenjualanProdukDetail> DetailGetByBulan(int bulan, int tahun)
        {
            IList<ReportPiutangPenjualanProdukDetail> oList = new List<ReportPiutangPenjualanProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("t_jual_produk.tanggal_tempo IS NOT NULL");
                whereBuilder.Add("(t_jual_produk.total_nota - t_jual_produk.diskon + t_jual_produk.ppn + t_jual_produk.ongkos_kirim) - t_jual_produk.total_pelunasan <> 0");
                whereBuilder.Add("EXTRACT(MONTH FROM t_jual_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPiutangPenjualanProdukDetail>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPiutangPenjualanProdukDetail> DetailGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPiutangPenjualanProdukDetail> oList = new List<ReportPiutangPenjualanProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("t_jual_produk.tanggal_tempo IS NOT NULL");
                whereBuilder.Add("(t_jual_produk.total_nota - t_jual_produk.diskon + t_jual_produk.ppn + t_jual_produk.ongkos_kirim) - t_jual_produk.total_pelunasan <> 0");
                whereBuilder.Add("(EXTRACT(MONTH FROM t_jual_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPiutangPenjualanProdukDetail>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPiutangPenjualanProdukDetail> DetailGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPiutangPenjualanProdukDetail> oList = new List<ReportPiutangPenjualanProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("t_jual_produk.tanggal_tempo IS NOT NULL");
                whereBuilder.Add("(t_jual_produk.total_nota - t_jual_produk.diskon + t_jual_produk.ppn + t_jual_produk.ongkos_kirim) - t_jual_produk.total_pelunasan <> 0");
                whereBuilder.Add("t_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPiutangPenjualanProdukDetail>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPiutangPenjualanProduk> PerProdukGetByBulan(int bulan, int tahun)
        {
            IList<ReportPiutangPenjualanProduk> oList = new List<ReportPiutangPenjualanProduk>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_PER_PRODUK);

                whereBuilder.Add("t_jual_produk.tanggal_tempo IS NOT NULL");
                whereBuilder.Add("(t_jual_produk.total_nota - t_jual_produk.diskon + t_jual_produk.ppn + t_jual_produk.ongkos_kirim) - t_jual_produk.total_pelunasan <> 0");
                whereBuilder.Add("EXTRACT(MONTH FROM t_jual_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPiutangPenjualanProduk>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPiutangPenjualanProduk> PerProdukGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPiutangPenjualanProduk> oList = new List<ReportPiutangPenjualanProduk>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_PER_PRODUK);

                whereBuilder.Add("t_jual_produk.tanggal_tempo IS NOT NULL");
                whereBuilder.Add("(t_jual_produk.total_nota - t_jual_produk.diskon + t_jual_produk.ppn + t_jual_produk.ongkos_kirim) - t_jual_produk.total_pelunasan <> 0");
                whereBuilder.Add("(EXTRACT(MONTH FROM t_jual_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPiutangPenjualanProduk>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPiutangPenjualanProduk> PerProdukGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPiutangPenjualanProduk> oList = new List<ReportPiutangPenjualanProduk>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_PER_PRODUK);

                whereBuilder.Add("t_jual_produk.tanggal_tempo IS NOT NULL");
                whereBuilder.Add("(t_jual_produk.total_nota - t_jual_produk.diskon + t_jual_produk.ppn + t_jual_produk.ongkos_kirim) - t_jual_produk.total_pelunasan <> 0");
                whereBuilder.Add("t_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPiutangPenjualanProduk>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
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
