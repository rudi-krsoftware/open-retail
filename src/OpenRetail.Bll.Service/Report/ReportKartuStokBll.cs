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
using OpenRetail.Model.Report;
using OpenRetail.Bll.Api.Report;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;

namespace OpenRetail.Bll.Service.Report
{
    public class ReportKartuStokBll : IReportKartuStokBll
    {
        private ILog _log;

        public ReportKartuStokBll(ILog log)
        {
            _log = log;
        }

        private void HitungSaldoAwal(IList<ReportKartuStok> oList)
        {
            var currentProdukId = string.Empty;
            double saldo = 0;

            foreach (var item in oList)
            {
                if (currentProdukId != item.produk_id)
                {
                    if (currentProdukId.Length > 0)
                    {
                        var oldProduk = oList.LastOrDefault(f => f.produk_id == currentProdukId);
                        oldProduk.saldo_akhir = oldProduk.saldo;
                    }

                    currentProdukId = item.produk_id;
                    saldo = 0;
                }

                if (item.jenis_nota == 1 || item.jenis_nota == 2) // produk masuk dari transaksi pembelian atau retur penjualan
                {
                    saldo += item.masuk;
                }
                else // produk keluar dari transaksi penjualan atau retur pembelian
                {
                    saldo -= item.keluar;
                }
                
                item.saldo = saldo;
            }

            var lastProduk = oList.LastOrDefault();
            if (lastProduk != null)
                lastProduk.saldo_akhir = lastProduk.saldo;
        }

        private void HitungSaldoAkhir(IList<ReportKartuStok> listOfSaldoAwal, IList<ReportKartuStok> listOfSaldoAkhir, IList<ReportKartuStok> listOfStokAwal)
        {
            var currentProdukId = string.Empty;
            var isFirstRecord = false;
            double saldo = 0;

            foreach (var item in listOfSaldoAkhir)
            {
                if (currentProdukId != item.produk_id)
                {
                    if (currentProdukId.Length > 0)
                    {
                        var oldProduk = listOfSaldoAkhir.LastOrDefault(f => f.produk_id == currentProdukId);
                        oldProduk.saldo_akhir = oldProduk.saldo;
                    }

                    saldo = 0;
                    currentProdukId = item.produk_id;
                    isFirstRecord = true;
                }

                if (isFirstRecord)
                {
                    var stokAwal = listOfStokAwal.Where(f => f.produk_id == currentProdukId)
                                                 .SingleOrDefault();

                    // copy saldo awal
                    var produkSaldoAwal = listOfSaldoAwal.LastOrDefault(f => f.produk_id == currentProdukId);
                    if (produkSaldoAwal != null)
                    {                        
                        if (stokAwal != null)
                            item.saldo_awal = produkSaldoAwal.saldo_akhir + stokAwal.stok_awal;
                        else
                            item.saldo_awal = produkSaldoAwal.saldo_akhir;                        
                    }
                    else
                    {
                        if (stokAwal != null)
                            item.saldo_awal = stokAwal.stok_awal;
                    }

                    saldo = item.saldo_awal;
                    isFirstRecord = false;
                }

                if (item.jenis_nota == 1 || item.jenis_nota == 2) // produk masuk dari transaksi pembelian atau retur penjualan
                {
                    saldo += item.masuk;
                }
                else // produk keluar dari transaksi penjualan atau retur pembelian
                {
                    saldo -= item.keluar;
                }

                item.saldo = saldo;
            }

            var lastProduk = listOfSaldoAkhir.LastOrDefault();
            if (lastProduk != null)
                lastProduk.saldo_akhir = lastProduk.saldo;
        }

        private IList<ReportKartuStok> GetSaldoAwal(DateTime tanggal)
        {
            IList<ReportKartuStok> oList = new List<ReportKartuStok>();

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ReportKartuStokRepository.GetSaldoAwal(tanggal);
            }

            return oList;
        }

        private void HitungStokAwal(IList<ReportKartuStok> listOfSaldoAkhir, ref IList<ReportKartuStok> listOfDistinctProduk)
        {
            IList<ReportKartuStok> listAllTransaction = new List<ReportKartuStok>();
            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);

                IList<string> listOfProdukId = listOfSaldoAkhir.Select(f => f.produk_id).Distinct().ToList();

                listAllTransaction = uow.ReportKartuStokRepository.GetAll(listOfProdukId);
            }

            listOfDistinctProduk = listAllTransaction.GroupBy(f => new { f.produk_id, f.stok_akhir })
                                                     .Select(f => f.First())
                                                     .ToList();

            foreach (var item in listOfDistinctProduk)
            {
                var qtyPembelian = listAllTransaction.Where(f => f.jenis_nota == 1 && f.produk_id == item.produk_id)
                                                     .Sum(f => f.qty);

                var qtyReturPenjualan = listAllTransaction.Where(f => f.jenis_nota == 2 && f.produk_id == item.produk_id)
                                                          .Sum(f => f.qty);

                var qtyPenjualan = listAllTransaction.Where(f => f.jenis_nota == 3 && f.produk_id == item.produk_id)
                                                     .Sum(f => f.qty);

                var qtyReturPembelian = listAllTransaction.Where(f => f.jenis_nota == 4 && f.produk_id == item.produk_id)
                                                          .Sum(f => f.qty);

                item.stok_awal = item.stok_akhir + qtyPenjualan + qtyReturPembelian - qtyPembelian - qtyReturPenjualan;
            }
        }

        public IList<ReportKartuStok> GetByBulan(int bulan, int tahun)
        {
            IList<ReportKartuStok> oList = new List<ReportKartuStok>();

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ReportKartuStokRepository.GetByBulan(bulan, tahun);
            }

            if (oList.Count > 0)
            {
                var tanggalAwal = oList.Min(f => f.tanggal);

                var listOfSaldoAwal = GetSaldoAwal(tanggalAwal);

                // hitung stok awal
                IList<ReportKartuStok> listOfDistinctProduk = new List<ReportKartuStok>();
                HitungStokAwal(oList, ref listOfDistinctProduk);
                
                // hitung saldo awal
                HitungSaldoAwal(listOfSaldoAwal);

                // hitung saldo akhir
                HitungSaldoAkhir(listOfSaldoAwal, oList, listOfDistinctProduk);
            }

            return oList;
        }        

        public IList<ReportKartuStok> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            throw new NotImplementedException();
        }

        public IList<ReportKartuStok> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportKartuStok> oList = new List<ReportKartuStok>();

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ReportKartuStokRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            if (oList.Count > 0)
            {
                var tanggalAwal = oList.Min(f => f.tanggal);

                var listOfSaldoAwal = GetSaldoAwal(tanggalAwal);

                // hitung stok awal
                IList<ReportKartuStok> listOfDistinctProduk = new List<ReportKartuStok>();
                HitungStokAwal(oList, ref listOfDistinctProduk);

                // hitung saldo awal
                HitungSaldoAwal(listOfSaldoAwal);

                // hitung saldo akhir
                HitungSaldoAkhir(listOfSaldoAwal, oList, listOfDistinctProduk);
            }

            return oList;
        }
    }
}
