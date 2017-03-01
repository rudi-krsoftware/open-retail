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
    public class ReportBeliProdukBllTest
    {
        private ILog _log;
        private IReportBeliProdukBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReportBeliProdukBllTest));
            _bll = new ReportBeliProdukBll(_log);
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

            var index = 0;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("Pixel Computer", obj.nama_supplier);
            Assert.AreEqual("201701310072", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 31), obj.tanggal);
            Assert.IsNull(obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(952000, obj.total_nota);
            Assert.AreEqual(952000, obj.total_pelunasan);
            Assert.AreEqual("beli tunai", obj.keterangan);
        }

        [TestMethod]
        public void GetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 2;
            var tahun = 2017;

            var oList = _bll.GetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 0;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("Pixel Computer", obj.nama_supplier);
            Assert.AreEqual("201701310072", obj.nota);            
            Assert.AreEqual(new DateTime(2017, 1, 31), obj.tanggal);
            Assert.IsNull(obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(952000, obj.total_nota);
            Assert.AreEqual(952000, obj.total_pelunasan);
            Assert.AreEqual("beli tunai", obj.keterangan);
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 1, 31);

            var oList = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 0;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("Pixel Computer", obj.nama_supplier);
            Assert.AreEqual("201701310072", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 31), obj.tanggal);
            Assert.IsNull(obj.tanggal_tempo);
            Assert.AreEqual(0, obj.ppn);
            Assert.AreEqual(0, obj.diskon);
            Assert.AreEqual(952000, obj.total_nota);
            Assert.AreEqual(952000, obj.total_pelunasan);
            Assert.AreEqual("beli tunai", obj.keterangan);
        }

        [TestMethod]
        public void DetailGetByBulanAndTahunTest()
        {
            var bulan = 1;
            var tahun = 2017;

            var oList = _bll.DetailGetByBulan(bulan, tahun);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            // cek item beli
            Assert.AreEqual(3, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(200000, obj.harga);
            Assert.AreEqual(0, obj.diskon);

            Assert.AreEqual("53b63dc2-4505-4276-9886-3639b53b7458", obj.produk_id);
            Assert.AreEqual("Access Point TPLINK TL-MR3220 3,5G", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);

            // cek beli
            Assert.AreEqual(new DateTime(2017, 1, 31), obj.tanggal);
            Assert.AreEqual("201701310072", obj.nota);

            // cek supplier
            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", obj.supplier_id);
            Assert.AreEqual("Pixel Computer", obj.nama_supplier);
        }

        [TestMethod]
        public void DetailGetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 2;
            var tahun = 2017;

            var oList = _bll.DetailGetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            // cek item beli
            Assert.AreEqual(3, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(200000, obj.harga);
            Assert.AreEqual(0, obj.diskon);

            Assert.AreEqual("53b63dc2-4505-4276-9886-3639b53b7458", obj.produk_id);
            Assert.AreEqual("Access Point TPLINK TL-MR3220 3,5G", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);

            // cek beli
            Assert.AreEqual(new DateTime(2017, 1, 31), obj.tanggal);
            Assert.AreEqual("201701310072", obj.nota);

            // cek supplier
            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", obj.supplier_id);
            Assert.AreEqual("Pixel Computer", obj.nama_supplier);
        }

        [TestMethod]
        public void DetailGetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 1, 31);

            var oList = _bll.DetailGetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 1;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            // cek item beli
            Assert.AreEqual(3, obj.jumlah);
            Assert.AreEqual(0, obj.jumlah_retur);
            Assert.AreEqual(200000, obj.harga);
            Assert.AreEqual(0, obj.diskon);

            Assert.AreEqual("53b63dc2-4505-4276-9886-3639b53b7458", obj.produk_id);
            Assert.AreEqual("Access Point TPLINK TL-MR3220 3,5G", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);

            // cek beli
            Assert.AreEqual(new DateTime(2017, 1, 31), obj.tanggal);
            Assert.AreEqual("201701310072", obj.nota);

            // cek supplier
            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", obj.supplier_id);
            Assert.AreEqual("Pixel Computer", obj.nama_supplier);
        }
    }
}
