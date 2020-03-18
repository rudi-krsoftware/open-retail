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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenRetail.Bll.Api.Report;
using OpenRetail.Bll.Service.Report;
using System;

namespace OpenRetail.Bll.Service.UnitTest.Report
{
    [TestClass]
    public class ReportPemasukanPengeluaranBllTest
    {
        private ILog _log;
        private IReportPemasukanPengeluaranBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReportPemasukanPengeluaranBllTest));
            _bll = new ReportPemasukanPengeluaranBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByBulanAndTahunTest()
        {
            var bulan = 10;
            var tahun = 2017;

            var obj = _bll.GetByBulan(bulan, tahun);

            Assert.IsNotNull(obj);
            Assert.AreEqual(15710303, obj.penjualan);
            Assert.AreEqual(862000, obj.pembelian);

            Assert.AreEqual(5, obj.list_of_beban.Count);

            var index = 4;
            var beban = obj.list_of_beban[index];
            Assert.AreEqual("Biaya Gaji Karyawan", beban.keterangan);
            Assert.AreEqual(1860000, beban.jumlah);
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 10, 24);
            var tanggalSelesai = new DateTime(2017, 10, 24);

            var obj = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);

            Assert.IsNotNull(obj);
            Assert.AreEqual(333000, obj.penjualan);
            Assert.AreEqual(862000, obj.pembelian);

            Assert.AreEqual(5, obj.list_of_beban.Count);

            var index = 4;
            var beban = obj.list_of_beban[index];
            Assert.AreEqual("Biaya Gaji Karyawan", beban.keterangan);
            Assert.AreEqual(1860000, beban.jumlah);
        }
    }
}