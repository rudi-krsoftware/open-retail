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
    public class ReportMesinKasirBllTest
    {
        private ILog _log;
        private IReportMesinKasirBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReportMesinKasirBllTest));
            _bll = new ReportMesinKasirBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void PerKasirGetByPenggunaIdTest()
        {
            var penggunaId = "00b5acfa-b533-454b-8dfd-e7881edd180f";
            var oList = _bll.PerKasirGetByPenggunaId(penggunaId);

            var index = 0;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("d1208856-cbd3-4b59-880c-717192064ad0", obj.mesin_id);
            Assert.AreEqual(new DateTime(2017, 10, 19), obj.tanggal);
            Assert.AreEqual(0, obj.saldo_awal);

            // cek data jual
            var jual = obj.jual;
            Assert.IsNotNull(jual);
            Assert.AreEqual(0, jual.ppn);
            Assert.AreEqual(0, jual.diskon);
            Assert.AreEqual(90303, jual.total_nota);

            // cek item produk
            var produk = obj.item_jual[1];
            Assert.IsNotNull(produk);
            Assert.AreEqual("0150360f-b039-4980-a399-960f1d0beebc", produk.produk_id);
            Assert.AreEqual("Flashdisk 4 Gb PQI U273", produk.nama_produk);
            Assert.AreEqual(49000, produk.harga_jual);
            Assert.AreEqual(1, produk.jumlah);
            Assert.AreEqual(0.5, produk.diskon);
        }
    }
}
