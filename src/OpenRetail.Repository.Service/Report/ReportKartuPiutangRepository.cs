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
    public class ReportKartuPiutangRepository : IReportKartuPiutangRepository
    {
        private const string SQL_TEMPLATE = @"SELECT m_customer.customer_id, m_customer.nama_customer, t_jual_produk.tanggal, m_produk.nama_produk, m_produk.satuan, 
                                              SUM(t_item_jual_produk.jumlah - t_item_jual_produk.jumlah_retur) AS jumlah, 
                                              (SUM((t_item_jual_produk.harga_jual - (t_item_jual_produk.harga_jual * t_item_jual_produk.diskon / 100)) * (t_item_jual_produk.jumlah - t_item_jual_produk.jumlah_retur)) - t_jual_produk.diskon) + t_jual_produk.ppn + t_jual_produk.ongkos_kirim AS total, 1 AS jenis
                                              FROM public.t_item_jual_produk INNER JOIN public.m_produk ON t_item_jual_produk.produk_id = m_produk.produk_id
                                              INNER JOIN public.t_jual_produk ON t_item_jual_produk.jual_id = t_jual_produk.jual_id
                                              INNER JOIN public.m_customer ON m_customer.customer_id = t_jual_produk.customer_id
                                              {WHERE_1}
                                              GROUP BY m_customer.customer_id, m_customer.nama_customer, t_jual_produk.tanggal, t_jual_produk.diskon, t_jual_produk.ppn, t_jual_produk.ongkos_kirim, m_produk.nama_produk, m_produk.satuan                                                   
                                              UNION
                                              SELECT m_customer.customer_id, m_customer.nama_customer, t_pembayaran_piutang_produk.tanggal, t_pembayaran_piutang_produk.keterangan AS nama_produk, '' AS satuan, 0 AS jumlah, SUM(t_item_pembayaran_piutang_produk.nominal) AS total, 2 AS jenis
                                              FROM public.t_pembayaran_piutang_produk INNER JOIN public.t_item_pembayaran_piutang_produk ON t_item_pembayaran_piutang_produk.pembayaran_piutang_id = t_pembayaran_piutang_produk.pembayaran_piutang_id
                                              INNER JOIN public.m_customer ON m_customer.customer_id = t_pembayaran_piutang_produk.customer_id
                                              {WHERE_2}
                                              GROUP BY m_customer.customer_id, m_customer.nama_customer, t_pembayaran_piutang_produk.tanggal, t_pembayaran_piutang_produk.keterangan
                                              ORDER BY 2, 3, 8";

        private IDapperContext _context;
        private ILog _log;
        private string _sql;
        private string _where;

        public ReportKartuPiutangRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ReportKartuPiutang> GetSaldoAwal(DateTime tanggal)
        {
            IList<ReportKartuPiutang> oList = new List<ReportKartuPiutang>();

            try
            {
                _where = @"WHERE t_jual_produk.tanggal_tempo IS NOT NULL AND t_jual_produk.tanggal < @tanggal";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = @"WHERE t_pembayaran_piutang_produk.tanggal < @tanggal AND t_pembayaran_piutang_produk.is_tunai = 'f'";
                _sql = _sql.Replace("{WHERE_2}", _where);

                oList = _context.db.Query<ReportKartuPiutang>(_sql, new { tanggal })
                                .ToList();

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKartuPiutang> GetByBulan(int bulan, int tahun)
        {
            IList<ReportKartuPiutang> oList = new List<ReportKartuPiutang>();

            try
            {
                _where = @"WHERE t_jual_produk.tanggal_tempo IS NOT NULL AND 
                           EXTRACT(MONTH FROM t_jual_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = @"WHERE EXTRACT(MONTH FROM t_pembayaran_piutang_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_pembayaran_piutang_produk.tanggal) = @tahun AND 
                           t_pembayaran_piutang_produk.is_tunai = 'f'";
                _sql = _sql.Replace("{WHERE_2}", _where);

                oList = _context.db.Query<ReportKartuPiutang>(_sql, new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKartuPiutang> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportKartuPiutang> oList = new List<ReportKartuPiutang>();

            try
            {
                _where = @"WHERE t_jual_produk.tanggal_tempo IS NOT NULL AND 
                           (EXTRACT(MONTH FROM t_jual_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir) AND EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = @"WHERE (EXTRACT(MONTH FROM t_pembayaran_piutang_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir) AND EXTRACT(YEAR FROM t_pembayaran_piutang_produk.tanggal) = @tahun AND 
                           t_pembayaran_piutang_produk.is_tunai = 'f'";
                _sql = _sql.Replace("{WHERE_2}", _where);

                oList = _context.db.Query<ReportKartuPiutang>(_sql, new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKartuPiutang> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportKartuPiutang> oList = new List<ReportKartuPiutang>();

            try
            {
                _where = @"WHERE t_jual_produk.tanggal_tempo IS NOT NULL AND t_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = @"WHERE t_pembayaran_piutang_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai AND t_pembayaran_piutang_produk.is_tunai = 'f'";
                _sql = _sql.Replace("{WHERE_2}", _where);

                oList = _context.db.Query<ReportKartuPiutang>(_sql, new { tanggalMulai, tanggalSelesai })
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
