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

namespace OpenRetail.Bll.Service.Report
{
    public class ReportPemasukanPengeluaranBll : IReportPemasukanPengeluaranBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;

        public ReportPemasukanPengeluaranBll(ILog log)
        {
            _log = log;
        }

        public ReportPemasukanPengeluaran GetByBulan(int bulan, int tahun)
        {
            ReportPemasukanPengeluaran obj = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                obj = _unitOfWork.ReportPemasukanPengeluaranRepository.GetByBulan(bulan, tahun);
            }

            return obj;
        }

        public ReportPemasukanPengeluaran GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            ReportPemasukanPengeluaran obj = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                obj = _unitOfWork.ReportPemasukanPengeluaranRepository.GetByTanggal(tanggalMulai, tanggalSelesai);
            }

            return obj;
        }
    }
}