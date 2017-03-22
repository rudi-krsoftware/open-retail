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
    public class ReportReturBeliProdukRepository : IReportReturBeliProdukRepository
    {
        private const string SQL_TEMPLATE_HEADER = @"SELECT m_supplier.supplier_id, m_supplier.nama_supplier, t_retur_beli_produk.nota AS nota_retur, t_retur_beli_produk.tanggal AS tanggal_retur, t_retur_beli_produk.total_nota AS total_retur, t_retur_beli_produk.keterangan,
                                                     t_beli_produk.nota as nota_beli
                                                     FROM public.t_retur_beli_produk INNER JOIN public.t_beli_produk ON t_retur_beli_produk.beli_produk_id = t_beli_produk.beli_produk_id
                                                     INNER JOIN public.m_supplier ON t_retur_beli_produk.supplier_id = m_supplier.supplier_id
                                                     {WHERE}
                                                     ORDER BY t_retur_beli_produk.tanggal, t_retur_beli_produk.nota";

        private const string SQL_TEMPLATE_DETAIL = @"SELECT m_supplier.supplier_id, m_supplier.nama_supplier, 
                                                     t_retur_beli_produk.nota AS nota_retur, t_retur_beli_produk.tanggal AS tanggal_retur, t_item_retur_beli_produk.jumlah_retur, t_item_retur_beli_produk.harga, 
                                                     t_beli_produk.nota AS nota_beli, m_produk.nama_produk, m_produk.satuan
                                                     FROM public.t_retur_beli_produk INNER JOIN public.t_beli_produk ON t_retur_beli_produk.beli_produk_id = t_beli_produk.beli_produk_id
                                                     INNER JOIN public.m_supplier ON t_retur_beli_produk.supplier_id = m_supplier.supplier_id
                                                     INNER JOIN public.t_item_retur_beli_produk ON t_item_retur_beli_produk.retur_beli_produk_id = t_retur_beli_produk.retur_beli_produk_id
                                                     INNER JOIN public.m_produk ON t_item_retur_beli_produk.produk_id = m_produk.produk_id
                                                     {WHERE}
                                                     ORDER BY t_retur_beli_produk.tanggal, t_retur_beli_produk.nota, m_produk.nama_produk";

        private IDapperContext _context;
        private ILog _log;

        public ReportReturBeliProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ReportReturPembelianProdukHeader> GetByBulan(int bulan, int tahun)
        {
            IList<ReportReturPembelianProdukHeader> oList = new List<ReportReturPembelianProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("EXTRACT(MONTH FROM t_retur_beli_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_retur_beli_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportReturPembelianProdukHeader>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportReturPembelianProdukHeader> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportReturPembelianProdukHeader> oList = new List<ReportReturPembelianProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("(EXTRACT(MONTH FROM t_retur_beli_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_retur_beli_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportReturPembelianProdukHeader>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportReturPembelianProdukHeader> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportReturPembelianProdukHeader> oList = new List<ReportReturPembelianProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("t_retur_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportReturPembelianProdukHeader>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportReturPembelianProdukDetail> DetailGetByBulan(int bulan, int tahun)
        {
            IList<ReportReturPembelianProdukDetail> oList = new List<ReportReturPembelianProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("EXTRACT(MONTH FROM t_retur_beli_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_retur_beli_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportReturPembelianProdukDetail>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportReturPembelianProdukDetail> DetailGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportReturPembelianProdukDetail> oList = new List<ReportReturPembelianProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("(EXTRACT(MONTH FROM t_retur_beli_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_retur_beli_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportReturPembelianProdukDetail>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportReturPembelianProdukDetail> DetailGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportReturPembelianProdukDetail> oList = new List<ReportReturPembelianProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("t_retur_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportReturPembelianProdukDetail>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
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
