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

using OpenRetail.Model.Report;
using OpenRetail.Bll.Api.Report;
using OpenRetail.Bll.Service.Report;

namespace OpenRetail.Bll.Service.UnitTest.Report
{
    [TestClass]
    public class ReportStokProdukBllTest
    {
        private ILog _log;
        private IReportStokProdukBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReportStokProdukBllTest));
            _bll = new ReportStokProdukBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetStokByStatusSemuaTest()
        {
            var oList = _bll.GetStokByStatus(StatusStok.Semua);

            var index = 2;
            var obj = oList[index];

            // tes produk
            Assert.IsNotNull(obj);
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("Adaptor NB ACER", obj.nama_produk);
            Assert.AreEqual(0, obj.stok);
            Assert.AreEqual(0, obj.stok_gudang);
            Assert.AreEqual(53000, obj.harga_beli);
            Assert.AreEqual(53000, obj.harga_jual);

            // tes golongan
            Assert.AreEqual("0a8b59e5-bb3b-4081-b963-9dc9584dcda6", obj.golongan_id);
            Assert.AreEqual("Accessories", obj.nama_golongan);
        }

        [TestMethod]
        public void GetStokByStatusAdaTest()
        {
            var oList = _bll.GetStokByStatus(StatusStok.Ada);

            var index = 1;
            var obj = oList[index];

            // tes produk
            Assert.IsNotNull(obj);
            Assert.AreEqual("53b63dc2-4505-4276-9886-3639b53b7458", obj.produk_id);
            Assert.AreEqual("Access Point TPLINK TL-MR3220 3,5G", obj.nama_produk);
            Assert.AreEqual(4, obj.stok);
            Assert.AreEqual(35, obj.stok_gudang);
            Assert.AreEqual(200000, obj.harga_beli);
            Assert.AreEqual(350000, obj.harga_jual);

            // tes golongan
            Assert.AreEqual("0a8b59e5-bb3b-4081-b963-9dc9584dcda6", obj.golongan_id);
            Assert.AreEqual("Accessories", obj.nama_golongan);
        }

        [TestMethod]
        public void GetStokByStatusKosongTest()
        {
            var oList = _bll.GetStokByStatus(StatusStok.Kosong);

            var index = 0;
            var obj = oList[index];

            // tes produk
            Assert.IsNotNull(obj);
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("Adaptor NB ACER", obj.nama_produk);
            Assert.AreEqual(0, obj.stok);
            Assert.AreEqual(0, obj.stok_gudang);
            Assert.AreEqual(53000, obj.harga_beli);
            Assert.AreEqual(53000, obj.harga_jual);

            // tes golongan
            Assert.AreEqual("0a8b59e5-bb3b-4081-b963-9dc9584dcda6", obj.golongan_id);
            Assert.AreEqual("Accessories", obj.nama_golongan);
        }

        [TestMethod]
        public void GetPenyesuaianStokByBulanAndTahunTest()
        {
            var bulan = 1;
            var tahun = 2017;

            var oList = _bll.GetPenyesuaianStokByBulan(bulan, tahun);

            var index = 1;
            var obj = oList[index];

            // tes penyesuaian
            Assert.IsNotNull(obj);
            Assert.AreEqual("561f8b6e-9c6a-47d4-b4cd-e1c2cf3552ac", obj.penyesuaian_stok_id);
            Assert.AreEqual(new DateTime(2017, 1, 14), obj.tanggal);
            Assert.AreEqual(1, obj.penambahan_stok);
            Assert.AreEqual(2, obj.pengurangan_stok);
            Assert.AreEqual(3, obj.penambahan_stok_gudang);
            Assert.AreEqual(4, obj.pengurangan_stok_gudang);
            Assert.AreEqual("tesss", obj.keterangan);

            // tes produk
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("Adaptor NB ACER", obj.nama_produk);

            // tes alasan penyesuaian
            Assert.AreEqual("f9b35798-6725-244f-fec0-fdee38c5ad44", obj.alasan_penyesuaian_stok_id);
            Assert.AreEqual("Pindah stok gudang ke etalase", obj.alasan);
        }

        [TestMethod]
        public void GetPenyesuaianStokByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 3, 24);

            var oList = _bll.GetPenyesuaianStokByTanggal(tanggalMulai, tanggalSelesai);

            var index = 1;
            var obj = oList[index];

            // tes penyesuaian
            Assert.IsNotNull(obj);
            Assert.AreEqual("561f8b6e-9c6a-47d4-b4cd-e1c2cf3552ac", obj.penyesuaian_stok_id);
            Assert.AreEqual(new DateTime(2017, 1, 14), obj.tanggal);
            Assert.AreEqual(1, obj.penambahan_stok);
            Assert.AreEqual(2, obj.pengurangan_stok);
            Assert.AreEqual(3, obj.penambahan_stok_gudang);
            Assert.AreEqual(4, obj.pengurangan_stok_gudang);
            Assert.AreEqual("tesss", obj.keterangan);

            // tes produk
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("Adaptor NB ACER", obj.nama_produk);

            // tes alasan penyesuaian
            Assert.AreEqual("f9b35798-6725-244f-fec0-fdee38c5ad44", obj.alasan_penyesuaian_stok_id);
            Assert.AreEqual("Pindah stok gudang ke etalase", obj.alasan);
        }
    }
}
