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
    public class ReportReturJualProdukRepository : IReportReturJualProdukRepository
    {
        private const string SQL_TEMPLATE_HEADER = @"SELECT m_customer.customer_id, m_customer.nama_customer, t_retur_jual_produk.nota AS nota_retur, 
                                                     t_retur_jual_produk.tanggal AS tanggal_retur, t_retur_jual_produk.total_nota AS total_retur, t_retur_jual_produk.keterangan, t_jual_produk.nota as nota_jual
                                                     FROM public.t_retur_jual_produk INNER JOIN public.t_jual_produk ON t_retur_jual_produk.jual_id = t_jual_produk.jual_id
                                                     INNER JOIN public.m_customer ON t_retur_jual_produk.customer_id = m_customer.customer_id
                                                     {WHERE}
                                                     ORDER BY t_retur_jual_produk.tanggal, t_retur_jual_produk.nota";

        private const string SQL_TEMPLATE_DETAIL = @"SELECT m_customer.customer_id, m_customer.nama_customer, t_retur_jual_produk.nota AS nota_retur, 
                                                     t_retur_jual_produk.tanggal AS tanggal_retur, t_item_retur_jual_produk.jumlah_retur, t_item_retur_jual_produk.harga_jual AS harga, 
                                                     t_jual_produk.nota AS nota_jual, m_produk.nama_produk, m_produk.satuan
                                                     FROM public.t_retur_jual_produk INNER JOIN public.t_jual_produk ON t_retur_jual_produk.jual_id = t_jual_produk.jual_id
                                                     INNER JOIN public.m_customer ON t_retur_jual_produk.customer_id = m_customer.customer_id
                                                     INNER JOIN public.t_item_retur_jual_produk ON t_item_retur_jual_produk.retur_jual_id = t_retur_jual_produk.retur_jual_id
                                                     INNER JOIN public.m_produk ON t_item_retur_jual_produk.produk_id = m_produk.produk_id
                                                     {WHERE}
                                                     ORDER BY t_retur_jual_produk.tanggal, t_retur_jual_produk.nota, m_produk.nama_produk";

        private IDapperContext _context;
        private ILog _log;

        public ReportReturJualProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ReportReturPenjualanProdukHeader> GetByBulan(int bulan, int tahun)
        {
            IList<ReportReturPenjualanProdukHeader> oList = new List<ReportReturPenjualanProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("EXTRACT(MONTH FROM t_retur_jual_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_retur_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportReturPenjualanProdukHeader>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportReturPenjualanProdukHeader> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportReturPenjualanProdukHeader> oList = new List<ReportReturPenjualanProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("(EXTRACT(MONTH FROM t_retur_jual_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_retur_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportReturPenjualanProdukHeader>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportReturPenjualanProdukHeader> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportReturPenjualanProdukHeader> oList = new List<ReportReturPenjualanProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("t_retur_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportReturPenjualanProdukHeader>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportReturPenjualanProdukDetail> DetailGetByBulan(int bulan, int tahun)
        {
            IList<ReportReturPenjualanProdukDetail> oList = new List<ReportReturPenjualanProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("EXTRACT(MONTH FROM t_retur_jual_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_retur_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportReturPenjualanProdukDetail>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportReturPenjualanProdukDetail> DetailGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportReturPenjualanProdukDetail> oList = new List<ReportReturPenjualanProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("(EXTRACT(MONTH FROM t_retur_jual_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_retur_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportReturPenjualanProdukDetail>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportReturPenjualanProdukDetail> DetailGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportReturPenjualanProdukDetail> oList = new List<ReportReturPenjualanProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("t_retur_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportReturPenjualanProdukDetail>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
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
