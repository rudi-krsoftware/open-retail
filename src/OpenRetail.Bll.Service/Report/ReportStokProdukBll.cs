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
    public class ReportStokProdukBll : IReportStokProdukBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;

        public ReportStokProdukBll(ILog log)
        {
            _log = log;
        }

        public IList<ReportStokProduk> GetStokByStatus(StatusStok statusStok)
        {
            IList<ReportStokProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportStokProdukRepository.GetStokByStatus(statusStok);
            }

            return oList;
        }

        public IList<ReportStokProduk> GetStokKurangDari(double stok)
        {
            IList<ReportStokProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportStokProdukRepository.GetStokKurangDari(stok);
            }

            return oList;
        }

        public IList<ReportStokProduk> GetStokBerdasarkanSupplier(string supplierId)
        {
            IList<ReportStokProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportStokProdukRepository.GetStokBerdasarkanSupplier(supplierId);
            }

            return oList;
        }

        public IList<ReportStokProduk> GetStokBerdasarkanGolongan(string golonganId)
        {
            IList<ReportStokProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportStokProdukRepository.GetStokBerdasarkanGolongan(golonganId);
            }

            return oList;
        }

        public IList<ReportStokProduk> GetStokBerdasarkanKode(IList<string> listOfKode)
        {
            IList<ReportStokProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportStokProdukRepository.GetStokBerdasarkanKode(listOfKode);
            }

            return oList;
        }

        public IList<ReportStokProduk> GetStokBerdasarkanNama(string name)
        {
            IList<ReportStokProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportStokProdukRepository.GetStokBerdasarkanNama(name);
            }

            return oList;
        }

        public IList<ReportPenyesuaianStokProduk> GetPenyesuaianStokByBulan(int bulan, int tahun)
        {
            IList<ReportPenyesuaianStokProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportStokProdukRepository.GetPenyesuaianStokByBulan(bulan, tahun);
            }

            return oList;
        }

        public IList<ReportPenyesuaianStokProduk> GetPenyesuaianStokByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReportPenyesuaianStokProduk> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.ReportStokProdukRepository.GetPenyesuaianStokByTanggal(tanggalMulai, tanggalSelesai);
            }

            return oList;
        }
    }
}