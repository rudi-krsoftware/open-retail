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
    public class ReportJualProdukRepository : IReportJualProdukRepository
    {
        private const string SQL_TEMPLATE_HEADER = @"SELECT t_jual_produk.retur_jual_id, t_jual_produk.nota, t_jual_produk.tanggal, t_jual_produk.tanggal_tempo, 
                                                     t_jual_produk.ppn, t_jual_produk.ongkos_kirim, t_jual_produk.diskon, t_jual_produk.total_nota, t_jual_produk.total_pelunasan, t_jual_produk.keterangan, t_jual_produk.tanggal_sistem,
                                                     m_customer.customer_id, m_customer.nama_customer, m_pengguna.pengguna_id, m_pengguna.nama_pengguna,
                                                     m_role.role_id, m_role.nama_role, m_shift.shift_id, m_shift.nama_shift
                                                     FROM public.t_jual_produk INNER JOIN public.m_pengguna ON t_jual_produk.pengguna_id = m_pengguna.pengguna_id
                                                     LEFT JOIN public.m_customer ON m_customer.customer_id = t_jual_produk.customer_id
                                                     INNER JOIN m_role ON m_role.role_id = m_pengguna.role_id
                                                     LEFT JOIN m_shift ON m_shift.shift_id = t_jual_produk.shift_id
                                                     {WHERE}
                                                     ORDER BY t_jual_produk.tanggal, t_jual_produk.nota";

        private const string SQL_TEMPLATE_DETAIL = @"SELECT m_produk.produk_id, m_produk.nama_produk, m_produk.satuan, t_item_jual_produk.jumlah, t_item_jual_produk.jumlah_retur, t_item_jual_produk.harga_beli, 
                                                     t_item_jual_produk.harga_jual, t_item_jual_produk.diskon, 
                                                     t_jual_produk.tanggal, t_jual_produk.tanggal_tempo, t_jual_produk.nota, 
                                                     m_customer.customer_id, m_customer.nama_customer
                                                     FROM public.t_jual_produk LEFT JOIN public.m_customer ON m_customer.customer_id = t_jual_produk.customer_id
                                                     INNER JOIN public.t_item_jual_produk ON t_item_jual_produk.jual_id = t_jual_produk.jual_id
                                                     INNER JOIN public.m_produk ON t_item_jual_produk.produk_id = m_produk.produk_id
                                                     {WHERE}
                                                     ORDER BY t_jual_produk.tanggal, t_jual_produk.nota, m_produk.nama_produk";

        private const string SQL_TEMPLATE_PER_PRODUK = @"SELECT m_customer.customer_id, m_customer.nama_customer, t_jual_produk.tanggal, m_produk.produk_id, m_produk.nama_produk, m_produk.satuan, SUM(t_item_jual_produk.jumlah) AS jumlah, 
                                                         SUM(t_item_jual_produk.jumlah_retur) AS jumlah_retur, t_item_jual_produk.harga_beli, t_item_jual_produk.harga_jual, t_item_jual_produk.diskon
                                                         FROM public.t_jual_produk LEFT JOIN public.m_customer ON m_customer.customer_id = t_jual_produk.customer_id 
                                                         INNER JOIN public.t_item_jual_produk ON t_item_jual_produk.jual_id = t_jual_produk.jual_id
                                                         INNER JOIN public.m_produk ON t_item_jual_produk.produk_id = m_produk.produk_id
                                                         {WHERE}
                                                         GROUP BY m_customer.customer_id, m_customer.nama_customer, t_jual_produk.tanggal, m_produk.produk_id, m_produk.nama_produk, m_produk.satuan, t_item_jual_produk.harga_beli, t_item_jual_produk.harga_jual, t_item_jual_produk.diskon                                            
                                                         ORDER BY t_jual_produk.tanggal, m_produk.nama_produk";

        private IDapperContext _context;
        private ILog _log;

        public ReportJualProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ReportPenjualanProdukHeader> GetByBulan(int bulan, int tahun)
        {
            IList<ReportPenjualanProdukHeader> oList = new List<ReportPenjualanProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("EXTRACT(MONTH FROM t_jual_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPenjualanProdukHeader>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPenjualanProdukHeader> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPenjualanProdukHeader> oList = new List<ReportPenjualanProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("(EXTRACT(MONTH FROM t_jual_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPenjualanProdukHeader>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPenjualanProdukHeader> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPenjualanProdukHeader> oList = new List<ReportPenjualanProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("t_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPenjualanProdukHeader>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPenjualanProdukDetail> DetailGetByBulan(int bulan, int tahun)
        {
            IList<ReportPenjualanProdukDetail> oList = new List<ReportPenjualanProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("EXTRACT(MONTH FROM t_jual_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPenjualanProdukDetail>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPenjualanProdukDetail> DetailGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPenjualanProdukDetail> oList = new List<ReportPenjualanProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("(EXTRACT(MONTH FROM t_jual_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPenjualanProdukDetail>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPenjualanProdukDetail> DetailGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPenjualanProdukDetail> oList = new List<ReportPenjualanProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("t_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPenjualanProdukDetail>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPenjualanProduk> PerProdukGetByBulan(int bulan, int tahun)
        {
            IList<ReportPenjualanProduk> oList = new List<ReportPenjualanProduk>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_PER_PRODUK);

                whereBuilder.Add("EXTRACT(MONTH FROM t_jual_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPenjualanProduk>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPenjualanProduk> PerProdukGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPenjualanProduk> oList = new List<ReportPenjualanProduk>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_PER_PRODUK);

                whereBuilder.Add("(EXTRACT(MONTH FROM t_jual_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPenjualanProduk>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPenjualanProduk> PerProdukGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPenjualanProduk> oList = new List<ReportPenjualanProduk>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_PER_PRODUK);

                whereBuilder.Add("t_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPenjualanProduk>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
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
