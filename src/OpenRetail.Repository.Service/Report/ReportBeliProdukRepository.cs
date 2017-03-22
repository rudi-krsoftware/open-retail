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
    public class ReportBeliProdukRepository : IReportBeliProdukRepository
    {
        private const string SQL_TEMPLATE_HEADER = @"SELECT t_beli_produk.tanggal, t_beli_produk.tanggal_tempo, t_beli_produk.nota, m_supplier.supplier_id, m_supplier.nama_supplier, 
                                                     t_beli_produk.total_nota, t_beli_produk.diskon, t_beli_produk.ppn, t_beli_produk.total_pelunasan
                                                     FROM public.t_beli_produk INNER JOIN public.m_supplier ON t_beli_produk.supplier_id = m_supplier.supplier_id
                                                     {WHERE}
                                                     ORDER BY t_beli_produk.tanggal, t_beli_produk.nota";

        private const string SQL_TEMPLATE_DETAIL = @"SELECT m_supplier.supplier_id, m_supplier.nama_supplier, t_beli_produk.tanggal, t_beli_produk.nota, m_produk.produk_id, m_produk.nama_produk, m_produk.satuan, t_item_beli_produk.jumlah, t_item_beli_produk.jumlah_retur, t_item_beli_produk.harga, t_item_beli_produk.diskon 
                                                     FROM public.t_beli_produk INNER JOIN public.m_supplier ON m_supplier.supplier_id = t_beli_produk.supplier_id
                                                     INNER JOIN public.t_item_beli_produk ON t_item_beli_produk.beli_produk_id = t_beli_produk.beli_produk_id
                                                     INNER JOIN public.m_produk ON t_item_beli_produk.produk_id = m_produk.produk_id
                                                     {WHERE}
                                                     ORDER BY t_beli_produk.tanggal, t_beli_produk.nota, m_produk.nama_produk";

        private IDapperContext _context;
        private ILog _log;

        public ReportBeliProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ReportPembelianProdukHeader> GetByBulan(int bulan, int tahun)
        {
            IList<ReportPembelianProdukHeader> oList = new List<ReportPembelianProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("EXTRACT(MONTH FROM t_beli_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPembelianProdukHeader>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembelianProdukHeader> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPembelianProdukHeader> oList = new List<ReportPembelianProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("(EXTRACT(MONTH FROM t_beli_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPembelianProdukHeader>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembelianProdukHeader> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPembelianProdukHeader> oList = new List<ReportPembelianProdukHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("t_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPembelianProdukHeader>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembelianProdukDetail> DetailGetByBulan(int bulan, int tahun)
        {
            IList<ReportPembelianProdukDetail> oList = new List<ReportPembelianProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("EXTRACT(MONTH FROM t_beli_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPembelianProdukDetail>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembelianProdukDetail> DetailGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPembelianProdukDetail> oList = new List<ReportPembelianProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("(EXTRACT(MONTH FROM t_beli_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun");

                oList = _context.db.Query<ReportPembelianProdukDetail>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPembelianProdukDetail> DetailGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPembelianProdukDetail> oList = new List<ReportPembelianProdukDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("t_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPembelianProdukDetail>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
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
