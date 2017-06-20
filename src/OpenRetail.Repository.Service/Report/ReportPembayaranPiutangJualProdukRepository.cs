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
    public class ReportPembayaranPiutangJualProdukRepository : IReportPembayaranPiutangJualProdukRepository
    {
        private const string SQL_TEMPLATE_HEADER = @"SELECT c.customer_id, c.nama_customer, p.tanggal, SUM(i.nominal) AS total_pembayaran, p.keterangan
                                                     FROM public.t_pembayaran_piutang_produk p LEFT JOIN public.m_customer c ON p.customer_id = c.customer_id
                                                     INNER JOIN public.t_item_pembayaran_piutang_produk i ON i.pembayaran_piutang_id = p.pembayaran_piutang_id
                                                     {WHERE}
                                                     GROUP BY c.customer_id, c.nama_customer, p.tanggal, p.keterangan
                                                     ORDER BY p.tanggal, c.nama_customer";

        private const string SQL_TEMPLATE_DETAIL = @"SELECT c.customer_id, c.nama_customer, j.nota AS nota_jual, p.nota AS nota_bayar, p.tanggal, j.ppn, j.diskon, j.ongkos_kirim, j.total_nota, 
                                                     i.nominal AS pelunasan, j.keterangan AS keterangan_jual, p.keterangan AS keterangan_bayar
                                                     FROM public.t_jual_produk j INNER JOIN public.t_item_pembayaran_piutang_produk i ON i.jual_id = j.jual_id
                                                     LEFT JOIN public.m_customer c ON j.customer_id = c.customer_id
                                                     INNER JOIN public.t_pembayaran_piutang_produk p ON i.pembayaran_piutang_id = p.pembayaran_piutang_id
                                                     {WHERE}
                                                     ORDER BY c.nama_customer, p.tanggal, p.nota";

        private IDapperContext _context;
        private ILog _log;

        public ReportPembayaranPiutangJualProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ReportPembayaranPiutangPenjualanProdukHeader> GetByBulan(int bulan, int tahun)
        {
            IList<ReportPembayaranPiutangPenjualanProdukHeader> oList = new List<ReportPembayaranPiutangPenjualanProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("EXTRACT(MONTH FROM p.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM p.tanggal) = @tahun");

                oList = _context.db.Query<ReportPembayaranPiutangPenjualanProdukHeader>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembayaranPiutangPenjualanProdukHeader> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPembayaranPiutangPenjualanProdukHeader> oList = new List<ReportPembayaranPiutangPenjualanProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("(EXTRACT(MONTH FROM p.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM p.tanggal) = @tahun");

                oList = _context.db.Query<ReportPembayaranPiutangPenjualanProdukHeader>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembayaranPiutangPenjualanProdukHeader> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPembayaranPiutangPenjualanProdukHeader> oList = new List<ReportPembayaranPiutangPenjualanProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("p.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPembayaranPiutangPenjualanProdukHeader>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembayaranPiutangPenjualanProdukDetail> DetailGetByBulan(int bulan, int tahun)
        {
            IList<ReportPembayaranPiutangPenjualanProdukDetail> oList = new List<ReportPembayaranPiutangPenjualanProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("EXTRACT(MONTH FROM p.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM p.tanggal) = @tahun");

                oList = _context.db.Query<ReportPembayaranPiutangPenjualanProdukDetail>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembayaranPiutangPenjualanProdukDetail> DetailGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPembayaranPiutangPenjualanProdukDetail> oList = new List<ReportPembayaranPiutangPenjualanProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("(EXTRACT(MONTH FROM p.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM p.tanggal) = @tahun");

                oList = _context.db.Query<ReportPembayaranPiutangPenjualanProdukDetail>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembayaranPiutangPenjualanProdukDetail> DetailGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPembayaranPiutangPenjualanProdukDetail> oList = new List<ReportPembayaranPiutangPenjualanProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("p.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPembayaranPiutangPenjualanProdukDetail>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
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
