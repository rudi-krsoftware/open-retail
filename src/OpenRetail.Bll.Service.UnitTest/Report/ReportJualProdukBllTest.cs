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
    public class ReportJualProdukBllTest
    {
        private ILog _log;
        private IReportJualProdukBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReportJualProdukBllTest));
            _bll = new ReportJualProdukBll(_log);
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
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);
            Assert.AreEqual("201703210056", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 21), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 3, 31), obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(1502000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.IsNull(obj.retur_jual_id);
        }

        [TestMethod]
        public void GetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 3;
            var tahun = 2017;

            var oList = _bll.GetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);
            Assert.AreEqual("201703210056", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 21), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 3, 31), obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(1502000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.IsNull(obj.retur_jual_id);
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 4, 1);

            var oList = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.nama_customer);
            Assert.AreEqual("201703210056", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 21), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 3, 31), obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(1502000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.IsNull(obj.retur_jual_id);
        }

        [TestMethod]
        public void DetailGetByBulanAndTahunTest()
        {
            var bulan = 3;
            var tahun = 2017;

            var oList = _bll.DetailGetByBulan(bulan, tahun);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);            

            // cek customer
            Assert.AreEqual("576c503f-69a7-46a5-b4be-107c634db7e3", obj.customer_id);
            Assert.AreEqual("Rudi", obj.nama_customer);

            // cek jual
            Assert.AreEqual(new DateTime(2017, 3, 2), obj.tanggal);
            Assert.AreEqual("201703210055", obj.nota);

            // cek item jual
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("Adaptor NB ACER", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(2, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(53000, obj.harga_beli);
            Assert.AreEqual(53000, obj.harga_jual);
            Assert.AreEqual(0, obj.diskon);
        }

        [TestMethod]
        public void DetailGetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 3;
            var tahun = 2017;

            var oList = _bll.DetailGetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            // cek customer
            Assert.AreEqual("576c503f-69a7-46a5-b4be-107c634db7e3", obj.customer_id);
            Assert.AreEqual("Rudi", obj.nama_customer);

            // cek jual
            Assert.AreEqual(new DateTime(2017, 3, 2), obj.tanggal);
            Assert.AreEqual("201703210055", obj.nota);

            // cek item jual
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("Adaptor NB ACER", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(2, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(53000, obj.harga_beli);
            Assert.AreEqual(53000, obj.harga_jual);
            Assert.AreEqual(0, obj.diskon);
        }

        [TestMethod]
        public void DetailGetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 3, 31);

            var oList = _bll.DetailGetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            // cek customer
            Assert.AreEqual("576c503f-69a7-46a5-b4be-107c634db7e3", obj.customer_id);
            Assert.AreEqual("Rudi", obj.nama_customer);

            // cek jual
            Assert.AreEqual(new DateTime(2017, 3, 2), obj.tanggal);
            Assert.AreEqual("201703210055", obj.nota);

            // cek item jual
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("Adaptor NB ACER", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(2, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(53000, obj.harga_beli);
            Assert.AreEqual(53000, obj.harga_jual);
            Assert.AreEqual(0, obj.diskon);
        }

        [TestMethod]
        public void PerProdukGetByBulanTest()
        {
            var bulan = 3;
            var tahun = 2017;

            var oList = _bll.PerProdukGetByBulan(bulan, tahun);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            Assert.AreEqual(new DateTime(2017, 3, 2), obj.tanggal);
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("Adaptor NB ACER", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(2, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(53000, obj.harga_beli);
            Assert.AreEqual(53000, obj.harga_jual);
            Assert.AreEqual(0, obj.diskon);
        }

        [TestMethod]
        public void PerProdukGetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 3;
            var tahun = 2017;

            var oList = _bll.DetailGetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            Assert.AreEqual(new DateTime(2017, 3, 2), obj.tanggal);
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("Adaptor NB ACER", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(2, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(53000, obj.harga_beli);
            Assert.AreEqual(53000, obj.harga_jual);
            Assert.AreEqual(0, obj.diskon);
        }

        [TestMethod]
        public void PerProdukGetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 3, 31);

            var oList = _bll.DetailGetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            Assert.AreEqual(new DateTime(2017, 3, 2), obj.tanggal);
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("Adaptor NB ACER", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(2, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(53000, obj.harga_beli);
            Assert.AreEqual(53000, obj.harga_jual);
            Assert.AreEqual(0, obj.diskon);
        }
    }
}
