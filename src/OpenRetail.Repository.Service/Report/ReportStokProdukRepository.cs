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
    public class ReportStokProdukRepository : IReportStokProdukRepository
    {
        private const string SQL_TEMPLATE_STOK_PRODUK = @"SELECT m_produk.produk_id, m_produk.nama_produk, m_produk.satuan, m_produk.stok, m_produk.stok_gudang, m_produk.harga_beli, m_produk.harga_jual, 
                                                          m_golongan.golongan_id, m_golongan.nama_golongan
                                                          FROM public.m_golongan INNER JOIN public.m_produk ON m_produk.golongan_id = m_golongan.golongan_id
                                                          {WHERE}
                                                          ORDER BY m_produk.nama_produk";

        private const string SQL_TEMPLATE_PENYESUAIAN_STOK = @"SELECT t_penyesuaian_stok.penyesuaian_stok_id, t_penyesuaian_stok.tanggal, t_penyesuaian_stok.penambahan_stok, t_penyesuaian_stok.pengurangan_stok, t_penyesuaian_stok.penambahan_stok_gudang, t_penyesuaian_stok.pengurangan_stok_gudang, t_penyesuaian_stok.keterangan, 
                                                               m_produk.produk_id, m_produk.nama_produk, m_alasan_penyesuaian_stok.alasan_penyesuaian_stok_id, m_alasan_penyesuaian_stok.alasan
                                                               FROM public.m_produk INNER JOIN public.t_penyesuaian_stok ON t_penyesuaian_stok.produk_id = m_produk.produk_id
                                                               INNER JOIN public.m_alasan_penyesuaian_stok ON t_penyesuaian_stok.alasan_penyesuaian_id = m_alasan_penyesuaian_stok.alasan_penyesuaian_stok_id
                                                               {WHERE}
                                                               ORDER BY t_penyesuaian_stok.tanggal, m_produk.nama_produk";

        private IDapperContext _context;
        private ILog _log;
        
        private string _sql;

        public ReportStokProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }
        
        public IList<ReportStokProduk> GetStokByStatus(StatusStok statusStok)
        {
            IList<ReportStokProduk> oList = new List<ReportStokProduk>();

            try
            {
                switch (statusStok)
                {
                    case StatusStok.Ada:
                        _sql = SQL_TEMPLATE_STOK_PRODUK.Replace("{WHERE}", "WHERE (m_produk.stok + m_produk.stok_gudang) > 0");
                        break;

                    case StatusStok.Kosong:
                        _sql = SQL_TEMPLATE_STOK_PRODUK.Replace("{WHERE}", "WHERE (m_produk.stok + m_produk.stok_gudang) <= 0");
                        break;

                    default: // semua produk
                        _sql = SQL_TEMPLATE_STOK_PRODUK.Replace("{WHERE}", "");
                        break;
                }

                oList = _context.db.Query<ReportStokProduk>(_sql).ToList();

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }
        
        public IList<ReportPenyesuaianStokProduk> GetPenyesuaianStokByBulan(int bulan, int tahun)
        {
            IList<ReportPenyesuaianStokProduk> oList = new List<ReportPenyesuaianStokProduk>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_PENYESUAIAN_STOK);

                whereBuilder.Add("EXTRACT(MONTH FROM t_penyesuaian_stok.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_penyesuaian_stok.tanggal) = @tahun");

                oList = _context.db.Query<ReportPenyesuaianStokProduk>(whereBuilder.ToSql(), new { bulan, tahun }).ToList();

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPenyesuaianStokProduk> GetPenyesuaianStokByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPenyesuaianStokProduk> oList = new List<ReportPenyesuaianStokProduk>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_PENYESUAIAN_STOK);

                whereBuilder.Add("t_penyesuaian_stok.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPenyesuaianStokProduk>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }
    }
}
