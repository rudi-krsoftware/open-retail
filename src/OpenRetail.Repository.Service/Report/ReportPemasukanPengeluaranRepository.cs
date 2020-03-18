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

using log4net;
using OpenRetail.Model.Report;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Api.Report;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenRetail.Repository.Service.Report
{
    public class ReportPemasukanPengeluaranRepository : IReportPemasukanPengeluaranRepository
    {
        private const string SQL_TEMPLATE_PENJUALAN = @"SELECT SUM(total_nota - diskon + ongkos_kirim + ppn)
                                                        FROM t_jual_produk
                                                        {WHERE}";

        private const string SQL_TEMPLATE_PEMBELIAN = @"SELECT SUM(total_nota - diskon + ppn)
                                                        FROM t_beli_produk
                                                        {WHERE}";

        private const string SQL_TEMPLATE_GAJI_KARYAWAN = @"SELECT
                                                              SUM(CASE
                                                                  WHEN jumlah_hari > 0 THEN jumlah_hari * gaji_pokok
                                                                  ELSE gaji_pokok
                                                              END) AS gaji_pokok,
                                                              SUM(CASE
                                                                  WHEN jam > 0 THEN jam * lembur
                                                                  ELSE lembur
                                                              END) AS lembur,
                                                            SUM(tunjangan) AS tunjangan, SUM(bonus) AS bonus, SUM(potongan) AS potongan
                                                            FROM t_gaji_karyawan
                                                            {WHERE}";

        private const string SQL_TEMPLATE_BEBAN = @"SELECT m_jenis_pengeluaran.nama_jenis_pengeluaran AS keterangan, SUM(t_item_pengeluaran_biaya.jumlah * t_item_pengeluaran_biaya.harga) AS jumlah
                                                    FROM public.t_pengeluaran_biaya INNER JOIN public.t_item_pengeluaran_biaya ON t_item_pengeluaran_biaya.pengeluaran_id = t_pengeluaran_biaya.pengeluaran_id
                                                    INNER JOIN public.m_jenis_pengeluaran ON t_item_pengeluaran_biaya.jenis_pengeluaran_id = m_jenis_pengeluaran.jenis_pengeluaran_id
                                                    {WHERE}
                                                    GROUP BY m_jenis_pengeluaran.nama_jenis_pengeluaran
                                                    ORDER BY m_jenis_pengeluaran.nama_jenis_pengeluaran";

        private IDapperContext _context;
        private ILog _log;

        public ReportPemasukanPengeluaranRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public ReportPemasukanPengeluaran GetByBulan(int bulan, int tahun)
        {
            ReportPemasukanPengeluaran obj = null;

            try
            {
                obj = new ReportPemasukanPengeluaran();
                obj.penjualan = GetPenjualan(bulan, tahun);
                obj.pembelian = GetPembelian(bulan, tahun);
                obj.list_of_beban = GetBeban(bulan, tahun);

                var gajiKaryawan = GetGajiKaryawan(bulan, tahun);
                if (gajiKaryawan > 0)
                {
                    if (obj.list_of_beban == null)
                    {
                        obj.list_of_beban = new List<ReportBebanUsaha>();
                    }

                    obj.list_of_beban.Add(new ReportBebanUsaha { keterangan = "Biaya Gaji Karyawan", jumlah = gajiKaryawan });
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public ReportPemasukanPengeluaran GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            ReportPemasukanPengeluaran obj = null;

            try
            {
                obj = new ReportPemasukanPengeluaran();
                obj.penjualan = GetPenjualan(tanggalMulai, tanggalSelesai);
                obj.pembelian = GetPembelian(tanggalMulai, tanggalSelesai);
                obj.list_of_beban = GetBeban(tanggalMulai, tanggalSelesai);

                var gajiKaryawan = GetGajiKaryawan(tanggalMulai, tanggalSelesai);
                if (gajiKaryawan > 0)
                {
                    if (obj.list_of_beban == null)
                    {
                        obj.list_of_beban = new List<ReportBebanUsaha>();
                    }

                    obj.list_of_beban.Add(new ReportBebanUsaha { keterangan = "Biaya Gaji Karyawan", jumlah = gajiKaryawan });
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        private double GetPenjualan(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_PENJUALAN);

                whereBuilder.Add("tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                result = _context.db.QuerySingleOrDefault<double>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai });
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        private double GetPenjualan(int bulan, int tahun)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_PENJUALAN);

                whereBuilder.Add("EXTRACT(MONTH FROM tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM tanggal) = @tahun");

                result = _context.db.QuerySingleOrDefault<double>(whereBuilder.ToSql(), new { bulan, tahun });
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        private double GetPembelian(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_PEMBELIAN);

                whereBuilder.Add("tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                result = _context.db.QuerySingleOrDefault<double>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai });
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        private double GetPembelian(int bulan, int tahun)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_PEMBELIAN);

                whereBuilder.Add("EXTRACT(MONTH FROM tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM tanggal) = @tahun");

                result = _context.db.QuerySingleOrDefault<double>(whereBuilder.ToSql(), new { bulan, tahun });
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        private double GetGajiKaryawan(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_GAJI_KARYAWAN);

                whereBuilder.Add("tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                var obj = _context.db.QuerySingleOrDefault<dynamic>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai });

                if (obj != null)
                {
                    result = (double)(obj.gaji_pokok + obj.tunjangan + obj.lembur + obj.bonus - obj.potongan);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        private double GetGajiKaryawan(int bulan, int tahun)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_GAJI_KARYAWAN);

                whereBuilder.Add("bulan = @bulan");
                whereBuilder.Add("tahun = @tahun");

                var obj = _context.db.QuerySingleOrDefault<dynamic>(whereBuilder.ToSql(), new { bulan, tahun });

                if (obj != null)
                {
                    result = (double)(obj.gaji_pokok + obj.tunjangan + obj.lembur + obj.bonus - obj.potongan);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        private IList<ReportBebanUsaha> GetBeban(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportBebanUsaha> oList = new List<ReportBebanUsaha>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_BEBAN);

                whereBuilder.Add("t_pengeluaran_biaya.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                oList = _context.db.Query<ReportBebanUsaha>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai })
                                   .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        private IList<ReportBebanUsaha> GetBeban(int bulan, int tahun)
        {
            IList<ReportBebanUsaha> oList = new List<ReportBebanUsaha>();

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_BEBAN);

                whereBuilder.Add("EXTRACT(MONTH FROM t_pengeluaran_biaya.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_pengeluaran_biaya.tanggal) = @tahun");

                oList = _context.db.Query<ReportBebanUsaha>(whereBuilder.ToSql(), new { bulan, tahun })
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