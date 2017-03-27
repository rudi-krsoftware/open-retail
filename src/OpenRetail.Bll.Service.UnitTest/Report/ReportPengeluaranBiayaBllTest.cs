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
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenRetail.Bll.Api.Report;
using OpenRetail.Bll.Service.Report;

namespace OpenRetail.Bll.Service.UnitTest.Report
{
    [TestClass]
    public class ReportPengeluaranBiayaBllTest
    {
        private ILog _log;
        private IReportPengeluaranBiayaBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReportPengeluaranBiayaBllTest));
            _bll = new ReportPengeluaranBiayaBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByBulanAndTahunTest()
        {
            var bulan = 3;
            var tahun = 2017;

            var oList = _bll.GetByBulan(bulan, tahun);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("201703270018", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 27), obj.tanggal);
            Assert.AreEqual("caf8e482-3f96-4fb9-b2fb-8fe7aad8f4cc", obj.jenis_pengeluaran_id);
            Assert.AreEqual("biaya baru", obj.nama_jenis_pengeluaran);
            Assert.AreEqual(3, obj.jumlah);
            Assert.AreEqual(2000, obj.harga);
            Assert.AreEqual(6000, obj.sub_total);
        }

        [TestMethod]
        public void GetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 3;
            var tahun = 2017;

            var oList = _bll.GetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("201703270018", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 27), obj.tanggal);
            Assert.AreEqual("caf8e482-3f96-4fb9-b2fb-8fe7aad8f4cc", obj.jenis_pengeluaran_id);
            Assert.AreEqual("biaya baru", obj.nama_jenis_pengeluaran);
            Assert.AreEqual(3, obj.jumlah);
            Assert.AreEqual(2000, obj.harga);
            Assert.AreEqual(6000, obj.sub_total);
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 3, 27);

            var oList = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("201703270018", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 27), obj.tanggal);
            Assert.AreEqual("caf8e482-3f96-4fb9-b2fb-8fe7aad8f4cc", obj.jenis_pengeluaran_id);
            Assert.AreEqual("biaya baru", obj.nama_jenis_pengeluaran);
            Assert.AreEqual(3, obj.jumlah);
            Assert.AreEqual(2000, obj.harga);
            Assert.AreEqual(6000, obj.sub_total);
        }
    }
}
