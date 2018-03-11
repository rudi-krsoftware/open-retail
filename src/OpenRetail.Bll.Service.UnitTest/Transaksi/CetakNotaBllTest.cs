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
using System.Threading.Tasks;
using System.Reflection;

using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;

namespace OpenRetail.Bll.Service.UnitTest
{
    [TestClass]
    public class CetakNotaBllTest
    {
        private ILog _log;
        private ICetakNotaBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(CetakNotaBllTest));
            _bll = new CetakNotaBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetNotaPembelianTest()
        {
            var beliProdukId = "6f59a7de-70d0-4aeb-8d8a-042041290a3f";

            var index = 2;
            var oList = _bll.GetNotaPembelian(beliProdukId);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("Sigma komputer", obj.nama_supplier);
            Assert.AreEqual("Yogyakarta", obj.alamat);
            Assert.AreEqual("201701310073", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 31), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 2, 4), obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon_nota);
            Assert.AreEqual(980000, obj.total_nota);
            Assert.AreEqual("201607000000055", obj.kode_produk);
            Assert.AreEqual("CD ROM ALL Merk 2nd", obj.nama_produk);
            Assert.AreEqual(20000, obj.harga);
            Assert.AreEqual(5, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(0, obj.diskon);
        }

        [TestMethod]
        public void GetNotaPenjualanTest()
        {
            var jualProdukId = "422ae0ed-41c2-4b9c-8f5d-63a24cd7363d";

            var index = 2;
            var oList = _bll.GetNotaPenjualan(jualProdukId);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);
            Assert.AreEqual("Seturan", obj.alamat);
            Assert.AreEqual("201703210056", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 21), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 3, 31), obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon_nota);
            Assert.AreEqual(1046000, obj.total_nota);
            Assert.AreEqual("201607000000052", obj.kode_produk);
            Assert.AreEqual("Access Point TPLINK TL-MR3220 3,5G", obj.nama_produk);
            Assert.AreEqual(350000, obj.harga);
            Assert.AreEqual(3, obj.jumlah);
            Assert.AreEqual(1, obj.jumlah_retur);
            Assert.AreEqual(0, obj.diskon);
        }
    }
}
