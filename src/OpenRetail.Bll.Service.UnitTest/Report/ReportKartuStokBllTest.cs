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
    public class ReportKartuStokBllTest
    {
        private ILog _log;
        private IReportKartuStokBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReportKartuStokBllTest));
            _bll = new ReportKartuStokBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByBulanAndTahunTest()
        {
            var bulan = 1;
            var tahun = 2017;

            var oList = _bll.GetByBulan(bulan, tahun);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.jenis_nota);
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", obj.produk_id);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", obj.nama_produk);
            Assert.AreEqual("201701310073", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 31), obj.tanggal);
            Assert.AreEqual("Sigma komputer", obj.supplier_or_customer);
            Assert.AreEqual(4, obj.qty);
            Assert.AreEqual("beli kredit", obj.keterangan);
        }

        [TestMethod]
        public void GetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 2;
            var tahun = 2017;

            var oList = _bll.GetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.jenis_nota);
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", obj.produk_id);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", obj.nama_produk);
            Assert.AreEqual("201701310073", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 31), obj.tanggal);
            Assert.AreEqual("Sigma komputer", obj.supplier_or_customer);
            Assert.AreEqual(4, obj.qty);
            Assert.AreEqual("beli kredit", obj.keterangan);
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 2, 28);

            var oList = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.jenis_nota);
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", obj.produk_id);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", obj.nama_produk);
            Assert.AreEqual("201701310073", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 31), obj.tanggal);
            Assert.AreEqual("Sigma komputer", obj.supplier_or_customer);
            Assert.AreEqual(4, obj.qty);
            Assert.AreEqual("beli kredit", obj.keterangan);
        }
    }
}