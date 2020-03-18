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
using OpenRetail.Model;
using OpenRetail.Model.Report;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Api.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenRetail.Repository.Service.Report
{
    public class ReportStokProdukRepository : IReportStokProdukRepository
    {
        private const string SQL_TEMPLATE_STOK_PRODUK = @"SELECT m_produk.produk_id, m_produk.nama_produk, m_produk.satuan, m_produk.stok, m_produk.stok_gudang, m_produk.harga_beli, m_produk.harga_jual,
                                                          m_golongan.golongan_id, m_golongan.nama_golongan
                                                          FROM public.m_golongan INNER JOIN public.m_produk ON m_produk.golongan_id = m_golongan.golongan_id
                                                          {WHERE}
                                                          ORDER BY m_produk.nama_produk";

        private const string SQL_TEMPLATE_STOK_PRODUK_BY_SUPPLIER = @"SELECT m_produk.produk_id, m_produk.nama_produk, m_produk.satuan, m_produk.stok, m_produk.stok_gudang, m_produk.harga_beli, m_produk.harga_jual,
                                                                      m_golongan.golongan_id, m_golongan.nama_golongan
                                                                      FROM public.m_golongan INNER JOIN public.m_produk ON m_produk.golongan_id = m_golongan.golongan_id
                                                                      INNER JOIN public.t_item_beli_produk ON t_item_beli_produk.produk_id = m_produk.produk_id
                                                                      INNER JOIN public.t_beli_produk ON t_item_beli_produk.beli_produk_id = t_beli_produk.beli_produk_id
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

        private IList<HargaGrosir> GetListHargaGrosir(string produkId)
        {
            IHargaGrosirRepository repo = new HargaGrosirRepository(_context, _log);

            return repo.GetListHargaGrosir(produkId);
        }

        private void SetHargaGrosir(IList<ReportStokProduk> oList)
        {
            foreach (var item in oList)
            {
                var listOfHargaGrosir = GetListHargaGrosir(item.produk_id);
                if (listOfHargaGrosir.Count == 3)
                {
                    item.harga_grosir1 = listOfHargaGrosir[0].harga_grosir;
                    item.harga_grosir2 = listOfHargaGrosir[1].harga_grosir;
                    item.harga_grosir3 = listOfHargaGrosir[2].harga_grosir;
                }
            }
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

                if (oList.Count > 0)
                    SetHargaGrosir(oList);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportStokProduk> GetStokKurangDari(double stok)
        {
            IList<ReportStokProduk> oList = new List<ReportStokProduk>();

            try
            {
                _sql = SQL_TEMPLATE_STOK_PRODUK.Replace("{WHERE}", "WHERE (m_produk.stok + m_produk.stok_gudang) < @stok");

                oList = _context.db.Query<ReportStokProduk>(_sql, new { stok }).ToList();

                if (oList.Count > 0)
                    SetHargaGrosir(oList);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportStokProduk> GetStokBerdasarkanSupplier(string supplierId)
        {
            IList<ReportStokProduk> oList = new List<ReportStokProduk>();

            try
            {
                _sql = SQL_TEMPLATE_STOK_PRODUK_BY_SUPPLIER.Replace("{WHERE}", "WHERE t_beli_produk.supplier_id = @supplierId");

                oList = _context.db.Query<ReportStokProduk>(_sql, new { supplierId }).ToList();

                if (oList.Count > 0)
                    SetHargaGrosir(oList);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportStokProduk> GetStokBerdasarkanGolongan(string golonganId)
        {
            IList<ReportStokProduk> oList = new List<ReportStokProduk>();

            try
            {
                _sql = SQL_TEMPLATE_STOK_PRODUK.Replace("{WHERE}", "WHERE m_golongan.golongan_id = @golonganId");

                oList = _context.db.Query<ReportStokProduk>(_sql, new { golonganId }).ToList();

                if (oList.Count > 0)
                    SetHargaGrosir(oList);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportStokProduk> GetStokBerdasarkanKode(IList<string> listOfKode)
        {
            IList<ReportStokProduk> oList = new List<ReportStokProduk>();

            try
            {
                var sb = new StringBuilder();

                foreach (var item in listOfKode)
                {
                    sb.Append("'").Append(item).Append("'").Append(",");
                }

                var param = sb.ToString();
                param = param.Substring(0, param.Length - 1);

                _sql = SQL_TEMPLATE_STOK_PRODUK.Replace("{WHERE}", "WHERE LOWER(m_produk.kode_produk) IN (" + param + ")");

                oList = _context.db.Query<ReportStokProduk>(_sql).ToList();

                if (oList.Count > 0)
                    SetHargaGrosir(oList);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReportStokProduk> GetStokBerdasarkanNama(string name)
        {
            IList<ReportStokProduk> oList = new List<ReportStokProduk>();

            try
            {
                name = "%" + name.ToLower() + "%";
                _sql = SQL_TEMPLATE_STOK_PRODUK.Replace("{WHERE}", "WHERE LOWER(m_produk.nama_produk) LIKE @name");

                oList = _context.db.Query<ReportStokProduk>(_sql, new { name }).ToList();

                if (oList.Count > 0)
                    SetHargaGrosir(oList);
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