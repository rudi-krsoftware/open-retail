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
    public class ReportKasbonRepository : IReportKasbonRepository
    {
        private const string SQL_TEMPLATE_HEADER = @"SELECT m_karyawan.karyawan_id, m_karyawan.nama_karyawan, 
                                                     t_kasbon.tanggal, t_kasbon.nota, t_kasbon.nominal, t_kasbon.total_pelunasan, t_kasbon.keterangan
                                                     FROM public.t_kasbon INNER JOIN public.m_karyawan ON t_kasbon.karyawan_id = m_karyawan.karyawan_id
                                                     {WHERE}
                                                     ORDER BY t_kasbon.tanggal, m_karyawan.nama_karyawan";

        private const string SQL_TEMPLATE_DETAIL = @"SELECT m_karyawan.karyawan_id, m_karyawan.nama_karyawan, 
                                                     t_kasbon.tanggal AS tanggal_kasbon, t_kasbon.nota AS nota_kasbon, t_kasbon.nominal AS jumlah_kasbon, t_kasbon.total_pelunasan, t_kasbon.keterangan AS keterangan_kasbon, 
                                                     t_pembayaran_kasbon.nota AS nota_pembayaran, t_pembayaran_kasbon.tanggal AS tanggal_pembayaran, t_pembayaran_kasbon.nominal AS jumlah_pembayaran, t_pembayaran_kasbon.keterangan AS keterangan_pembayaran
                                                     FROM public.t_kasbon INNER JOIN public.m_karyawan ON t_kasbon.karyawan_id = m_karyawan.karyawan_id
                                                     INNER JOIN public.t_pembayaran_kasbon ON t_pembayaran_kasbon.kasbon_id = t_kasbon.kasbon_id
                                                     {WHERE}
                                                     ORDER BY m_karyawan.nama_karyawan, t_kasbon.tanggal, t_kasbon.nota, t_pembayaran_kasbon.tanggal, t_pembayaran_kasbon.nota";

        private IDapperContext _context;
        private ILog _log;

        public ReportKasbonRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ReportKasbonHeader> GetByBulan(int bulan, int tahun)
        {
            IList<ReportKasbonHeader> oList = new List<ReportKasbonHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("EXTRACT(MONTH FROM t_kasbon.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_kasbon.tanggal) = @tahun");

                oList = _context.db.Query<ReportKasbonHeader>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKasbonHeader> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportKasbonHeader> oList = new List<ReportKasbonHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("(EXTRACT(MONTH FROM t_kasbon.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_kasbon.tanggal) = @tahun");

                oList = _context.db.Query<ReportKasbonHeader>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKasbonHeader> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportKasbonHeader> oList = new List<ReportKasbonHeader>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("t_kasbon.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportKasbonHeader>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKasbonDetail> DetailGetByBulan(int bulan, int tahun)
        {
            IList<ReportKasbonDetail> oList = new List<ReportKasbonDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("EXTRACT(MONTH FROM t_kasbon.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_kasbon.tanggal) = @tahun");

                oList = _context.db.Query<ReportKasbonDetail>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKasbonDetail> DetailGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportKasbonDetail> oList = new List<ReportKasbonDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("(EXTRACT(MONTH FROM t_kasbon.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_kasbon.tanggal) = @tahun");

                oList = _context.db.Query<ReportKasbonDetail>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportKasbonDetail> DetailGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportKasbonDetail> oList = new List<ReportKasbonDetail>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DETAIL);

                whereBuilder.Add("t_kasbon.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportKasbonDetail>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
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
