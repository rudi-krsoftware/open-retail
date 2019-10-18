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
    public class ReportRugiLabaRepository : IReportRugiLabaRepository
    {
        private const string SQL_TEMPLATE_PENJUALAN = @"SELECT COALESCE(SUM(total_nota), SUM(total_nota), 0)
                                                        FROM t_jual_produk
                                                        {WHERE}";

        private const string SQL_TEMPLATE_DISKON_NOTA = @"SELECT COALESCE(SUM(diskon), SUM(diskon), 0)
                                                          FROM t_jual_produk
                                                          {WHERE}";

        private const string SQL_TEMPLATE_DISKON_PRODUK = @"SELECT COALESCE(SUM(t_item_jual_produk.jumlah - t_item_jual_produk.jumlah_retur), SUM(t_item_jual_produk.jumlah - t_item_jual_produk.jumlah_retur), 0) * 
                                                            COALESCE(SUM(t_item_jual_produk.diskon), SUM(t_item_jual_produk.diskon), 0)
                                                            FROM t_jual_produk INNER JOIN t_item_jual_produk ON t_jual_produk.jual_id = t_item_jual_produk.jual_id
                                                            {WHERE}";

        private const string SQL_TEMPLATE_HPP = @"SELECT SUM(t_item_jual_produk.harga_beli * (t_item_jual_produk.jumlah - t_item_jual_produk.jumlah_retur))
                                                  FROM t_jual_produk INNER JOIN t_item_jual_produk ON t_jual_produk.jual_id = t_item_jual_produk.jual_id
                                                  {WHERE}";

        private const string SQL_TEMPLATE_RETUR_PENJUALAN = @"SELECT COALESCE(SUM(total_nota), SUM(total_nota), 0)
                                                              FROM t_retur_jual_produk
                                                              {WHERE}";

        private IDapperContext _context;
        private ILog _log;

        public ReportRugiLabaRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
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

        private double GetHPP(int bulan, int tahun)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HPP);

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

        private double GetHPP(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_HPP);

                whereBuilder.Add("tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                result = _context.db.QuerySingleOrDefault<double>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai });
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        private double GetDiskonNota(int bulan, int tahun)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DISKON_NOTA);

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

        private double GetDiskonNota(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DISKON_NOTA);                
                whereBuilder.Add("tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                result = _context.db.QuerySingleOrDefault<double>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai });
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        private double GetDiskonProduk(int bulan, int tahun)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DISKON_PRODUK);

                whereBuilder.Add("EXTRACT(MONTH FROM t_jual_produk.tanggal) = @bulan");
                whereBuilder.Add("EXTRACT(YEAR FROM t_jual_produk.tanggal) = @tahun");

                result = _context.db.QuerySingleOrDefault<double>(whereBuilder.ToSql(), new { bulan, tahun });
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        private double GetDiskonProduk(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_DISKON_PRODUK);
                whereBuilder.Add("t_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                result = _context.db.QuerySingleOrDefault<double>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai });
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        private double GetReturPenjualan(int bulan, int tahun)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_RETUR_PENJUALAN);

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

        private double GetReturPenjualan(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            double result = 0;

            try
            {
                var whereBuilder = new WhereBuilder(SQL_TEMPLATE_RETUR_PENJUALAN);
                whereBuilder.Add("tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");

                result = _context.db.QuerySingleOrDefault<double>(whereBuilder.ToSql(), new { tanggalMulai, tanggalSelesai });
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public ReportRugiLaba GetByBulan(int bulan, int tahun)
        {
            ReportRugiLaba obj = null;

            try
            {
                var diskonNota = GetDiskonNota(bulan, tahun);
                var diskonProduk = GetDiskonProduk(bulan, tahun);
                var retur = GetReturPenjualan(bulan, tahun);

                obj = new ReportRugiLaba();
                obj.penjualan = GetPenjualan(bulan, tahun) + diskonProduk + retur;
                obj.diskon = diskonNota + diskonProduk;
                obj.hpp = GetHPP(bulan, tahun);
                obj.return_penjualan = retur;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public ReportRugiLaba GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            ReportRugiLaba obj = null;

            try
            {
                var diskonNota = GetDiskonNota(tanggalMulai, tanggalSelesai);
                var diskonProduk = GetDiskonProduk(tanggalMulai, tanggalSelesai);
                var retur = GetReturPenjualan(tanggalMulai, tanggalSelesai);

                obj = new ReportRugiLaba();
                obj.penjualan = GetPenjualan(tanggalMulai, tanggalSelesai) + diskonProduk + retur;
                obj.diskon = diskonNota + diskonProduk;
                obj.hpp = GetHPP(tanggalMulai, tanggalSelesai);
                obj.return_penjualan = retur;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }
    }
}
