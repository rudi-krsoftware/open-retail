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
    public class ReportKartuHutangBll : IReportKartuHutangBll
    {
        private ILog _log;

        public ReportKartuHutangBll(ILog log)
        {
            _log = log;
        }

        private void HitungSaldoAwal(IList<ReportKartuHutang> oList)
        {
            var currentSupplierId = string.Empty;
            double saldo = 0;

            foreach (var item in oList)
            {
                if (currentSupplierId != item.supplier_id)
                {
                    if (currentSupplierId.Length > 0)
                    {
                        var oldSupplier = oList.LastOrDefault(f => f.supplier_id == currentSupplierId);
                        oldSupplier.saldo_akhir = oldSupplier.saldo;
                    }

                    currentSupplierId = item.supplier_id;
                    saldo = 0;
                }

                if (item.jenis == 1) // pembelian kredit
                {
                    saldo += item.total;
                }
                else // pembayaran hutang
                {
                    saldo -= item.total;
                }

                item.saldo = saldo;
            }

            var lastSupplier = oList.LastOrDefault();
            if (lastSupplier != null)
                lastSupplier.saldo_akhir = lastSupplier.saldo;
        }

        private void HitungSaldoAkhir(IList<ReportKartuHutang> listOfSaldoAwal, IList<ReportKartuHutang> listOfSaldoAkhir)
        {
            var currentSupplierId = string.Empty;
            var isFirstRecord = false;
            double saldo = 0;            

            foreach (var item in listOfSaldoAkhir)
            {
                if (currentSupplierId != item.supplier_id)
                {
                    if (currentSupplierId.Length > 0)
                    {
                        var oldSupplier = listOfSaldoAkhir.LastOrDefault(f => f.supplier_id == currentSupplierId);
                        oldSupplier.saldo_akhir = oldSupplier.saldo;
                    }

                    saldo = 0;
                    currentSupplierId = item.supplier_id;                    
                    isFirstRecord = true;
                }

                if (isFirstRecord)
                {
                    // copy saldo awal
                    var supplierSaldoAwal = listOfSaldoAwal.LastOrDefault(f => f.supplier_id == currentSupplierId);
                    if (supplierSaldoAwal != null)
                    {
                        item.saldo_awal = supplierSaldoAwal.saldo_akhir;
                        saldo = item.saldo_awal;
                    }

                    isFirstRecord = false;
                }

                if (item.jenis == 1) // pembelian kredit
                {
                    saldo += item.total;
                }
                else // pembayaran hutang
                {
                    saldo -= item.total;
                }

                item.saldo = saldo;
            }

            var lastSupplier = listOfSaldoAkhir.LastOrDefault();
            if (lastSupplier != null)
                lastSupplier.saldo_akhir = lastSupplier.saldo;
        }

        private IList<ReportKartuHutang> GetSaldoAwal(DateTime tanggal)
        {
            IList<ReportKartuHutang> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ReportKartuHutangRepository.GetSaldoAwal(tanggal);
            }

            return oList;
        }

        public IList<ReportKartuHutang> GetByBulan(int bulan, int tahun)
        {
            IList<ReportKartuHutang> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ReportKartuHutangRepository.GetByBulan(bulan, tahun);
            }

            if (oList.Count > 0)
            {
                var tanggalAwal = oList.Min(f => f.tanggal);

                // hitung saldo awal masing-masing supplier
                var listOfSaldoAwal = GetSaldoAwal(tanggalAwal);
                HitungSaldoAwal(listOfSaldoAwal);

                // hitung saldo akhir
                HitungSaldoAkhir(listOfSaldoAwal, oList);
            }

            return oList;
        }

        public IList<ReportKartuHutang> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportKartuHutang> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ReportKartuHutangRepository.GetByBulan(bulanAwal, bulanAkhir, tahun);
            }

            if (oList.Count > 0)
            {
                var tanggalAwal = oList.Min(f => f.tanggal);

                // hitung saldo awal masing-masing supplier
                var listOfSaldoAwal = GetSaldoAwal(tanggalAwal);
                HitungSaldoAwal(listOfSaldoAwal);

                // hitung saldo akhir
                HitungSaldoAkhir(listOfSaldoAwal, oList);
            }

            return oList;
        }

        public IList<ReportKartuHutang> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportKartuHutang> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ReportKartuHutangRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            if (oList.Count > 0)
            {
                var tanggalAwal = oList.Min(f => f.tanggal);

                // hitung saldo awal masing-masing supplier
                var listOfSaldoAwal = GetSaldoAwal(tanggalAwal);
                HitungSaldoAwal(listOfSaldoAwal);

                // hitung saldo akhir
                HitungSaldoAkhir(listOfSaldoAwal, oList);
            }

            return oList;
        }        
    }
}
