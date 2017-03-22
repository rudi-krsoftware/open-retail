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
    public class ReportPembayaranHutangBeliProdukRepository : IReportPembayaranHutangBeliProdukRepository
    {
        private const string SQL_TEMPLATE_HEADER = @"SELECT s.supplier_id, s.nama_supplier, p.tanggal, SUM(i.nominal) AS total_pembayaran, p.keterangan
                                                     FROM public.t_pembayaran_hutang_produk p INNER JOIN public.m_supplier s ON p.supplier_id = s.supplier_id
                                                     INNER JOIN public.t_item_pembayaran_hutang_produk i ON i.pembayaran_hutang_produk_id = p.pembayaran_hutang_produk_id
                                                     {WHERE}
                                                     GROUP BY s.supplier_id, s.nama_supplier, p.tanggal, p.keterangan
                                                     ORDER BY p.tanggal, s.nama_supplier";

        private const string SQL_TEMPLATE_DETAIL = @"SELECT s.supplier_id, s.nama_supplier, b.nota AS nota_beli, p.nota AS nota_bayar, p.tanggal, b.ppn, b.diskon, b.total_nota, i.nominal AS pelunasan, b.keterangan AS keterangan_beli, p.keterangan AS keterangan_bayar
                                                     FROM public.t_beli_produk b INNER JOIN public.t_item_pembayaran_hutang_produk i ON i.beli_produk_id = b.beli_produk_id
                                                     INNER JOIN public.m_supplier s ON b.supplier_id = s.supplier_id
                                                     INNER JOIN public.t_pembayaran_hutang_produk p ON i.pembayaran_hutang_produk_id = p.pembayaran_hutang_produk_id
                                                     {WHERE}
                                                     ORDER BY s.nama_supplier, p.tanggal, p.nota";

        private IDapperContext _context;
        private ILog _log;

        public ReportPembayaranHutangBeliProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ReportPembayaranHutangPembelianProdukHeader> GetByBulan(int bulan, int tahun)
        {
            IList<ReportPembayaranHutangPembelianProdukHeader> oList = new List<ReportPembayaranHutangPembelianProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("EXTRACT(MONTH FROM p.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM p.tanggal) = @tahun");

                oList = _context.db.Query<ReportPembayaranHutangPembelianProdukHeader>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembayaranHutangPembelianProdukHeader> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPembayaranHutangPembelianProdukHeader> oList = new List<ReportPembayaranHutangPembelianProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("(EXTRACT(MONTH FROM p.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM p.tanggal) = @tahun");

                oList = _context.db.Query<ReportPembayaranHutangPembelianProdukHeader>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembayaranHutangPembelianProdukHeader> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPembayaranHutangPembelianProdukHeader> oList = new List<ReportPembayaranHutangPembelianProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("p.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPembayaranHutangPembelianProdukHeader>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembayaranHutangPembelianProdukDetail> DetailGetByBulan(int bulan, int tahun)
        {
            IList<ReportPembayaranHutangPembelianProdukDetail> oList = new List<ReportPembayaranHutangPembelianProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("EXTRACT(MONTH FROM p.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM p.tanggal) = @tahun");

                oList = _context.db.Query<ReportPembayaranHutangPembelianProdukDetail>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembayaranHutangPembelianProdukDetail> DetailGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPembayaranHutangPembelianProdukDetail> oList = new List<ReportPembayaranHutangPembelianProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("(EXTRACT(MONTH FROM p.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM p.tanggal) = @tahun");

                oList = _context.db.Query<ReportPembayaranHutangPembelianProdukDetail>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembayaranHutangPembelianProdukDetail> DetailGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPembayaranHutangPembelianProdukDetail> oList = new List<ReportPembayaranHutangPembelianProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("p.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPembayaranHutangPembelianProdukDetail>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
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
