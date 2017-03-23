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
    public class ReportKartuHutangRepository : IReportKartuHutangRepository
    {
        private const string SQL_TEMPLATE = @"SELECT m_supplier.supplier_id, m_supplier.nama_supplier, t_beli_produk.tanggal, m_produk.nama_produk, m_produk.satuan, 
                                              SUM(t_item_beli_produk.jumlah - t_item_beli_produk.jumlah_retur) AS jumlah, 
                                              (SUM((t_item_beli_produk.harga - (t_item_beli_produk.harga * t_item_beli_produk.diskon / 100)) * (t_item_beli_produk.jumlah - t_item_beli_produk.jumlah_retur)) - t_beli_produk.diskon) + t_beli_produk.ppn AS total, 1 AS jenis
                                              FROM public.t_item_beli_produk INNER JOIN public.m_produk ON t_item_beli_produk.produk_id = m_produk.produk_id
                                              INNER JOIN public.t_beli_produk ON t_item_beli_produk.beli_produk_id = t_beli_produk.beli_produk_id
                                              INNER JOIN public.m_supplier ON m_supplier.supplier_id = t_beli_produk.supplier_id
                                              {WHERE_1}
                                              GROUP BY m_supplier.supplier_id, m_supplier.nama_supplier, t_beli_produk.tanggal, t_beli_produk.diskon, t_beli_produk.ppn, m_produk.nama_produk, m_produk.satuan                                                   
                                              UNION
                                              SELECT m_supplier.supplier_id, m_supplier.nama_supplier, t_pembayaran_hutang_produk.tanggal, t_pembayaran_hutang_produk.keterangan AS nama_produk, '' AS satuan, 0 AS jumlah, SUM(t_item_pembayaran_hutang_produk.nominal) AS total, 2 AS jenis
                                              FROM public.t_pembayaran_hutang_produk INNER JOIN public.t_item_pembayaran_hutang_produk ON t_item_pembayaran_hutang_produk.pembayaran_hutang_produk_id = t_pembayaran_hutang_produk.pembayaran_hutang_produk_id
                                              INNER JOIN public.m_supplier ON m_supplier.supplier_id = t_pembayaran_hutang_produk.supplier_id
                                              {WHERE_2}
                                              GROUP BY m_supplier.supplier_id, m_supplier.nama_supplier, t_pembayaran_hutang_produk.tanggal, t_pembayaran_hutang_produk.keterangan
                                              ORDER BY 2, 3, 8";

        private IDapperContext _context;
        private ILog _log;
        private string _sql;
        private string _where;

        public ReportKartuHutangRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }        

        public IList<ReportKartuHutang> GetSaldoAwal(DateTime tanggal)
        {
            IList<ReportKartuHutang> oList = new List<ReportKartuHutang>();

            try
            {
                _where = @"WHERE t_beli_produk.tanggal_tempo IS NOT NULL AND t_beli_produk.tanggal < @tanggal";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = @"WHERE t_pembayaran_hutang_produk.tanggal < @tanggal AND t_pembayaran_hutang_produk.is_tunai = 'f'";                
                _sql = _sql.Replace("{WHERE_2}", _where);

                oList = _context.db.Query<ReportKartuHutang>(_sql, new { tanggal })
                                .ToList();

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKartuHutang> GetByBulan(int bulan, int tahun)
        {
            IList<ReportKartuHutang> oList = new List<ReportKartuHutang>();

            try
            {
                _where = @"WHERE t_beli_produk.tanggal_tempo IS NOT NULL AND 
                           EXTRACT(MONTH FROM t_beli_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = @"WHERE EXTRACT(MONTH FROM t_pembayaran_hutang_produk.tanggal) = @bulan AND EXTRACT(YEAR FROM t_pembayaran_hutang_produk.tanggal) = @tahun AND 
                           t_pembayaran_hutang_produk.is_tunai = 'f'";
                _sql = _sql.Replace("{WHERE_2}", _where);

                oList = _context.db.Query<ReportKartuHutang>(_sql, new { bulan, tahun })
                                .ToList();                
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }        

        public IList<ReportKartuHutang> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportKartuHutang> oList = new List<ReportKartuHutang>();

            try
            {                
                _where = @"WHERE t_beli_produk.tanggal_tempo IS NOT NULL AND 
                           (EXTRACT(MONTH FROM t_beli_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir) AND EXTRACT(YEAR FROM t_beli_produk.tanggal) = @tahun";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = @"WHERE (EXTRACT(MONTH FROM t_pembayaran_hutang_produk.tanggal) BETWEEN @bulanAwal AND @bulanAkhir) AND EXTRACT(YEAR FROM t_pembayaran_hutang_produk.tanggal) = @tahun AND 
                           t_pembayaran_hutang_produk.is_tunai = 'f'";
                _sql = _sql.Replace("{WHERE_2}", _where);

                oList = _context.db.Query<ReportKartuHutang>(_sql, new { bulanAwal, bulanAkhir, tahun })
                                .ToList();                                
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }        

        public IList<ReportKartuHutang> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportKartuHutang> oList = new List<ReportKartuHutang>();

            try
            {
                _where = @"WHERE t_beli_produk.tanggal_tempo IS NOT NULL AND t_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai";
                _sql = SQL_TEMPLATE.Replace("{WHERE_1}", _where);

                _where = @"WHERE t_pembayaran_hutang_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai AND t_pembayaran_hutang_produk.is_tunai = 'f'";
                _sql = _sql.Replace("{WHERE_2}", _where);

                oList = _context.db.Query<ReportKartuHutang>(_sql, new { tanggalMulai, tanggalSelesai })
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
