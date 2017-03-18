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

using OpenRetail.Model.Report;

namespace OpenRetail.Repository.Api.Report
{
    public interface IReportKartuHutangRepository
    {
        IList<ReportKartuHutang> GetSaldoAwal(DateTime tanggal);
        IList<ReportKartuHutang> GetByBulan(int bulan, int tahun);
        IList<ReportKartuHutang> GetByBulan(int bulanAwal, int bulanAkhir, int tahun);
        IList<ReportKartuHutang> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai);
    }
}
