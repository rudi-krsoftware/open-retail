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
using OpenRetail.Bll.Api.Report;
using OpenRetail.Model.Report;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;
using System;
using System.Collections.Generic;

namespace OpenRetail.Bll.Service.Report
{
    public class ReportJualProdukBll : IReportJualProdukBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;

        public ReportJualProdukBll(ILog log)
        {
            _log = log;
        }

        public IList<ReportPenjualanProdukHeader> GetByBulan(int bulan, int tahun)
        {
            IList<ReportPenjualanProdukHeader> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.GetByBulan(bulan, tahun);
            }

            return oList;
        }

        public IList<ReportPenjualanProdukHeader> GetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPenjualanProdukHeader> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.GetByBulan(bulanAwal, bulanAkhir, tahun);
            }

            return oList;
        }

        public IList<ReportPenjualanProdukHeader> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPenjualanProdukHeader> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }

        public IList<ReportPenjualanProdukDetail> DetailGetByBulan(int bulan, int tahun)
        {
            IList<ReportPenjualanProdukDetail> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.DetailGetByBulan(bulan, tahun);
            }

            return oList;
        }

        public IList<ReportPenjualanProdukDetail> DetailGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPenjualanProdukDetail> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.DetailGetByBulan(bulanAwal, bulanAkhir, tahun);
            }

            return oList;
        }

        public IList<ReportPenjualanProdukDetail> DetailGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPenjualanProdukDetail> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.DetailGetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }

        public IList<ReportPenjualanProduk> PerProdukGetByBulan(int bulan, int tahun)
        {
            IList<ReportPenjualanProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.PerProdukGetByBulan(bulan, tahun);
            }

            return oList;
        }

        public IList<ReportPenjualanProduk> PerProdukGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            IList<ReportPenjualanProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.PerProdukGetByBulan(bulanAwal, bulanAkhir, tahun);
            }

            return oList;
        }

        public IList<ReportPenjualanProduk> PerProdukGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPenjualanProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.PerProdukGetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }

        public IList<ReportProdukFavorit> ProdukFavoritGetByBulan(int bulan, int tahun, int limit)
        {
            IList<ReportProdukFavorit> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.ProdukFavoritGetByBulan(bulan, tahun, limit);
            }

            return oList;
        }

        public IList<ReportProdukFavorit> ProdukFavoritGetByBulan(int bulanAwal, int bulanAkhir, int tahun, int limit)
        {
            throw new NotImplementedException();
        }

        public IList<ReportProdukFavorit> ProdukFavoritGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, int limit)
        {
            IList<ReportProdukFavorit> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.ProdukFavoritGetByTanggal(tanggalMulai, tanggalSelesai, limit);
            }

            return oList;
        }

        public IList<ReportPenjualanPerKasir> PerKasirGetByBulan(int bulan, int tahun)
        {
            IList<ReportPenjualanPerKasir> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.PerKasirGetByBulan(bulan, tahun);
            }

            return oList;
        }

        public IList<ReportPenjualanPerKasir> PerKasirGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            throw new NotImplementedException();
        }

        public IList<ReportPenjualanPerKasir> PerKasirGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPenjualanPerKasir> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.PerKasirGetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }

        public IList<ReportCustomerProduk> CustomerProdukGetByBulan(int bulan, int tahun)
        {
            IList<ReportCustomerProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.CustomerProdukGetByBulan(bulan, tahun);
            }

            return oList;
        }

        public IList<ReportCustomerProduk> CustomerProdukGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            throw new NotImplementedException();
        }

        public IList<ReportCustomerProduk> CustomerProdukGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportCustomerProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.CustomerProdukGetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }

        public IList<ReportPenjualanProdukPerGolongan> PerGolonganGetByBulan(int bulan, int tahun)
        {
            IList<ReportPenjualanProdukPerGolongan> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.PerGolonganGetByBulan(bulan, tahun);
            }

            return oList;
        }

        public IList<ReportPenjualanProdukPerGolongan> PerGolonganGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            throw new NotImplementedException();
        }

        public IList<ReportPenjualanProdukPerGolongan> PerGolonganGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPenjualanProdukPerGolongan> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.PerGolonganGetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }

        public IList<ReportPenjualanProduk> PerGolonganDetailGetByBulan(int bulan, int tahun)
        {
            IList<ReportPenjualanProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.PerGolonganDetailGetByBulan(bulan, tahun);
            }

            return oList;
        }

        public IList<ReportPenjualanProduk> PerGolonganDetailGetByBulan(int bulanAwal, int bulanAkhir, int tahun)
        {
            throw new NotImplementedException();
        }

        public IList<ReportPenjualanProduk> PerGolonganDetailGetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPenjualanProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportJualProdukRepository.PerGolonganDetailGetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }
    }
}