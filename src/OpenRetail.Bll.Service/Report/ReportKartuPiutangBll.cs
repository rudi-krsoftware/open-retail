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
    public class ReportKartuPiutangBll : IReportKartuPiutangBll
    {
        private ILog _log;

        public ReportKartuPiutangBll(ILog log)
        {
            _log = log;
        }
        
        private void HitungSaldoAwal(IList<ReportKartuPiutang> oList)
        {
            var currentCustomerId = string.Empty;
            double saldo = 0;

            foreach (var item in oList)
            {                
                if (currentCustomerId != item.customer_id)
                {
                    if (currentCustomerId.Length > 0)
                    {
                        var oldCustomer = oList.LastOrDefault(f => f.customer_id == currentCustomerId);
                        oldCustomer.saldo_akhir = oldCustomer.saldo;
                    }

                    currentCustomerId = item.customer_id;
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

            var lastCustomer = oList.LastOrDefault();
            if (lastCustomer != null)
                lastCustomer.saldo_akhir = lastCustomer.saldo;
        }

        private void HitungSaldoAkhir(IList<ReportKartuPiutang> listOfSaldoAwal, IList<ReportKartuPiutang> listOfSaldoAkhir)
        {
            var currentCustomerId = string.Empty;
            var isFirstRecord = false;
            double saldo = 0;

            foreach (var item in listOfSaldoAkhir)
            {
                if (currentCustomerId != item.customer_id)
                {
                    if (currentCustomerId.Length > 0)
                    {
                        var oldCustomer = listOfSaldoAkhir.LastOrDefault(f => f.customer_id == currentCustomerId);
                        oldCustomer.saldo_akhir = oldCustomer.saldo;
                    }

                    saldo = 0;
                    currentCustomerId = item.customer_id;
                    isFirstRecord = true;
                }

                if (isFirstRecord)
                {
                    // copy saldo awal
                    var customerSaldoAwal = listOfSaldoAwal.LastOrDefault(f => f.customer_id == currentCustomerId);
                    if (customerSaldoAwal != null)
                    {
                        item.saldo_awal = customerSaldoAwal.saldo_akhir;
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

            var lastCustomer = listOfSaldoAkhir.LastOrDefault();
            if (lastCustomer != null)
                lastCustomer.saldo_akhir = lastCustomer.saldo;
        }

        private IList<ReportKartuPiutang> GetSaldoAwal(DateTime tanggal)
        {
            IList<ReportKartuPiutang> oList = new List<ReportKartuPiutang>();

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ReportKartuPiutangRepository.GetSaldoAwal(tanggal);                
            }

            return oList;
        }

        public IList<ReportKartuPiutang> GetByBulan(int bulan, int tahun)
        {
            IList<ReportKartuPiutang> oList = new List<ReportKartuPiutang>();

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ReportKartuPiutangRepository.GetByBulan(bulan, tahun);
            }

            if (oList.Count > 0)
            {
                var tanggalAwal = oList.Min(f => f.tanggal);

                // hitung saldo awal masing-masing customer
                var listOfSaldoAwal = GetSaldoAwal(tanggalAwal);
                HitungSaldoAwal(listOfSaldoAwal);

                // hitung saldo akhir
                HitungSaldoAkhir(listOfSaldoAwal, oList);
            }

            return oList;
        }

        public IList<ReportKartuPiutang> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportKartuPiutang> oList = new List<ReportKartuPiutang>();

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ReportKartuPiutangRepository.GetByBulan(bulanAwal, bulanAkhir, tahun);
            }

            if (oList.Count > 0)
            {
                var tanggalAwal = oList.Min(f => f.tanggal);

                // hitung saldo awal masing-masing customer
                var listOfSaldoAwal = GetSaldoAwal(tanggalAwal);
                HitungSaldoAwal(listOfSaldoAwal);

                // hitung saldo akhir
                HitungSaldoAkhir(listOfSaldoAwal, oList);
            }

            return oList;
        }

        public IList<ReportKartuPiutang> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportKartuPiutang> oList = new List<ReportKartuPiutang>();

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.ReportKartuPiutangRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            if (oList.Count > 0)
            {
                var tanggalAwal = oList.Min(f => f.tanggal);

                // hitung saldo awal masing-masing customer
                var listOfSaldoAwal = GetSaldoAwal(tanggalAwal);
                HitungSaldoAwal(listOfSaldoAwal);

                // hitung saldo akhir
                HitungSaldoAkhir(listOfSaldoAwal, oList);
            }

            return oList;
        }
    }
}
