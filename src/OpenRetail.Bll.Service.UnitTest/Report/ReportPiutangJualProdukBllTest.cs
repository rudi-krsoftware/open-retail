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
    public class ReportPiutangJualProdukBllTest
    {
        private ILog _log;
        private IReportPiutangJualProdukBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReportPiutangJualProdukBllTest));
            _bll = new ReportPiutangJualProdukBll(_log);
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

            var index = 0;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(1842000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
        }

        [TestMethod]
        public void GetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 3;
            var tahun = 2017;

            var oList = _bll.GetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 0;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(1842000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 3, 22);

            var oList = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 0;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(1842000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
        }

        [TestMethod]
        public void DetailGetByBulanAndTahunTest()
        {
            var bulan = 3;
            var tahun = 2017;

            var oList = _bll.DetailGetByBulan(bulan, tahun);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            // cek customer
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);

            // cek jual
            Assert.AreEqual("201703210057", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 21), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 4, 1), obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(340000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
        }

        [TestMethod]
        public void DetailGetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 3;
            var tahun = 2017;

            var oList = _bll.DetailGetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            // cek customer
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);

            // cek jual
            Assert.AreEqual("201703210057", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 21), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 4, 1), obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(340000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
        }

        [TestMethod]
        public void DetailGetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 3, 31);

            var oList = _bll.DetailGetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            // cek customer
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);

            // cek jual
            Assert.AreEqual("201703210057", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 21), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 4, 1), obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(340000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
        }

        [TestMethod]
        public void PerProdukGetByBulanTest()
        {
            var bulan = 3;
            var tahun = 2017;

            var oList = _bll.PerProdukGetByBulan(bulan, tahun);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            // cek customer
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);

            // cek jual
            Assert.AreEqual("201703210056", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 21), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 3, 31), obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon_nota);
            Assert.AreEqual(1502000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);

            // cek item jual
            Assert.AreEqual("53b63dc2-4505-4276-9886-3639b53b7458", obj.produk_id);
            Assert.AreEqual("Access Point TPLINK TL-MR3220 3,5G", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(3, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(350000, obj.harga_jual);
        }

        [TestMethod]
        public void PerProdukGetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 3;
            var tahun = 2017;

            var oList = _bll.PerProdukGetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            // cek customer
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);

            // cek jual
            Assert.AreEqual("201703210056", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 21), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 3, 31), obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon_nota);
            Assert.AreEqual(1502000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);

            // cek item jual
            Assert.AreEqual("53b63dc2-4505-4276-9886-3639b53b7458", obj.produk_id);
            Assert.AreEqual("Access Point TPLINK TL-MR3220 3,5G", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(3, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(350000, obj.harga_jual);
        }

        [TestMethod]
        public void PerProdukGetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 3, 31);

            var oList = _bll.PerProdukGetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            // cek customer
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);

            // cek jual
            Assert.AreEqual("201703210056", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 21), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 3, 31), obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon_nota);
            Assert.AreEqual(1502000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);

            // cek item jual
            Assert.AreEqual("53b63dc2-4505-4276-9886-3639b53b7458", obj.produk_id);
            Assert.AreEqual("Access Point TPLINK TL-MR3220 3,5G", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(3, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(350000, obj.harga_jual);
        }
    }
}
