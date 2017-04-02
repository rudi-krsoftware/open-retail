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
    public class ReportGajiKaryawanRepository : IReportGajiKaryawanRepository
    {
        private const string SQL_TEMPLATE_HEADER = @"SELECT m_karyawan.karyawan_id, m_karyawan.nama_karyawan, m_karyawan.jenis_gajian, m_jabatan.nama_jabatan, 
                                                     t_gaji_karyawan.tanggal, t_gaji_karyawan.bulan, t_gaji_karyawan.tahun, 
                                                     t_gaji_karyawan.kehadiran, t_gaji_karyawan.absen, t_gaji_karyawan.gaji_pokok, t_gaji_karyawan.lembur, t_gaji_karyawan.bonus, 
                                                     t_gaji_karyawan.potongan, t_gaji_karyawan.jam, t_gaji_karyawan.lainnya, t_gaji_karyawan.keterangan, 
                                                     t_gaji_karyawan.jumlah_hari, t_gaji_karyawan.tunjangan
                                                     FROM public.t_gaji_karyawan INNER JOIN public.m_karyawan ON t_gaji_karyawan.karyawan_id = m_karyawan.karyawan_id
                                                     INNER JOIN public.m_jabatan ON m_karyawan.jabatan_id = m_jabatan.jabatan_id
                                                     {WHERE}
                                                     ORDER BY t_gaji_karyawan.tanggal, m_karyawan.nama_karyawan";

        private IDapperContext _context;
        private ILog _log;

        public ReportGajiKaryawanRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ReportGajiKaryawan> GetByBulan(int bulan, int tahun)
        {
            IList<ReportGajiKaryawan> oList = new List<ReportGajiKaryawan>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("t_gaji_karyawan.bulan = @bulan");
                whereBuilder.Add("t_gaji_karyawan.tahun = @tahun");

                oList = _context.db.Query<ReportGajiKaryawan>(whereBuilder.ToSql(), new { bulan, tahun })
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportGajiKaryawan> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            throw new NotImplementedException();
        }

        public IList<ReportGajiKaryawan> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportGajiKaryawan> oList = new List<ReportGajiKaryawan>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HEADER);

                whereBuilder.Add("t_gaji_karyawan.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportGajiKaryawan>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
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
