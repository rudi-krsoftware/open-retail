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

using OpenRetail.Model;
using OpenRetail.Bll.Api.Report;
using OpenRetail.Bll.Service.Report;

namespace OpenRetail.Bll.Service.UnitTest.Report
{
    [TestClass]
    public class ReportGajiKaryawanBllTest
    {
        private ILog _log;
        private IReportGajiKaryawanBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReportGajiKaryawanBllTest));
            _bll = new ReportGajiKaryawanBll(_log);
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

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("d3506b64-df74-4283-b17a-6c5dbb770e85", obj.karyawan_id);
            Assert.AreEqual("Doni", obj.nama_karyawan);
            Assert.AreEqual("Kasir", obj.nama_jabatan);
            Assert.AreEqual(JenisGajian.Mingguan, obj.jenis_gajian);
            Assert.AreEqual(new DateTime(2017, 3, 31), obj.tanggal);
            Assert.AreEqual(3, obj.bulan);
            Assert.AreEqual(2017, obj.tahun);
            Assert.AreEqual(20, obj.kehadiran);
            Assert.AreEqual(5, obj.absen);
            Assert.AreEqual(1500000, obj.gaji_pokok);
            Assert.AreEqual(1000, obj.lembur);
            Assert.AreEqual(150000, obj.bonus);
            Assert.AreEqual(50000, obj.potongan);
            Assert.AreEqual(1, obj.jam);
            Assert.AreEqual(6, obj.jumlah_hari);
            Assert.AreEqual(0, obj.tunjangan);
        }
    }
}
