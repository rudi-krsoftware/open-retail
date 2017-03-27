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
    public class ReportPengeluaranBiayaRepository : IReportPengeluaranBiayaRepository
    {
        private const string SQL_TEMPLATE = @"SELECT t_pengeluaran_biaya.nota, t_pengeluaran_biaya.tanggal, m_jenis_pengeluaran.jenis_pengeluaran_id, m_jenis_pengeluaran.nama_jenis_pengeluaran, t_item_pengeluaran_biaya.jumlah, t_item_pengeluaran_biaya.harga
                                              FROM public.t_pengeluaran_biaya INNER JOIN public.t_item_pengeluaran_biaya ON t_item_pengeluaran_biaya.pengeluaran_id = t_pengeluaran_biaya.pengeluaran_id
                                              INNER JOIN public.m_jenis_pengeluaran ON t_item_pengeluaran_biaya.jenis_pengeluaran_id = m_jenis_pengeluaran.jenis_pengeluaran_id
                                              {WHERE}
                                              ORDER BY t_pengeluaran_biaya.tanggal";


        private IDapperContext _context;
        private ILog _log;
        
        public ReportPengeluaranBiayaRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ReportPengeluaranBiaya> GetByBulan(int bulan, int tahun)
        {
            IList<ReportPengeluaranBiaya> oList = new List<ReportPengeluaranBiaya>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE);

                whereBuilder.Add("EXTRACT(MONTH FROM t_pengeluaran_biaya.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_pengeluaran_biaya.tanggal) = @tahun");

                oList = _context.db.Query<ReportPengeluaranBiaya>(whereBuilder.ToSql(), new { bulan, tahun }).ToList();

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPengeluaranBiaya> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPengeluaranBiaya> oList = new List<ReportPengeluaranBiaya>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE);

                whereBuilder.Add("(EXTRACT(MONTH FROM t_pengeluaran_biaya.tanggal) BETWEEN @bulanAwal AND @bulanAkhir)");
                whereBuilder.Add("EXTRACT(YEAR FROM t_pengeluaran_biaya.tanggal) = @tahun");

                oList = _context.db.Query<ReportPengeluaranBiaya>(whereBuilder.ToSql(), new { bulanAwal, bulanAkhir, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportPengeluaranBiaya> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPengeluaranBiaya> oList = new List<ReportPengeluaranBiaya>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE);

                whereBuilder.Add("t_pengeluaran_biaya.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportPengeluaranBiaya>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }
    }
}
