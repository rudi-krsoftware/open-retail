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
    public class ReportKasbonBllTest
    {
        private ILog _log;
        private IReportKasbonBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReportKasbonBllTest));
            _bll = new ReportKasbonBll(_log);
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

            var index = 3;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("0e0251ab-7c99-40fc-85ec-7974eeebc800", obj.karyawan_id);
            Assert.AreEqual("Andi", obj.nama_karyawan);
            Assert.AreEqual("201703280009", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 29), obj.tanggal);
            Assert.AreEqual(200000, obj.nominal);
            Assert.AreEqual(200000, obj.total_pelunasan);
            Assert.AreEqual("tambahan kasbon", obj.keterangan);
        }

        [TestMethod]
        public void GetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 3;
            var tahun = 2017;

            var oList = _bll.GetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 3;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("0e0251ab-7c99-40fc-85ec-7974eeebc800", obj.karyawan_id);
            Assert.AreEqual("Andi", obj.nama_karyawan);
            Assert.AreEqual("201703280009", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 29), obj.tanggal);
            Assert.AreEqual(200000, obj.nominal);
            Assert.AreEqual(200000, obj.total_pelunasan);
            Assert.AreEqual("tambahan kasbon", obj.keterangan);
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 3, 1);
            var tanggalSelesai = new DateTime(2017, 3, 30);

            var oList = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 3;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("0e0251ab-7c99-40fc-85ec-7974eeebc800", obj.karyawan_id);
            Assert.AreEqual("Andi", obj.nama_karyawan);
            Assert.AreEqual("201703280009", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 29), obj.tanggal);
            Assert.AreEqual(200000, obj.nominal);
            Assert.AreEqual(200000, obj.total_pelunasan);
            Assert.AreEqual("tambahan kasbon", obj.keterangan);
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

            // header kasbon
            Assert.AreEqual("0e0251ab-7c99-40fc-85ec-7974eeebc800", obj.karyawan_id);
            Assert.AreEqual("Andi", obj.nama_karyawan);
            Assert.AreEqual(new DateTime(2017, 3, 29), obj.tanggal_kasbon);
            Assert.AreEqual("201703280009", obj.nota_kasbon);
            Assert.AreEqual(200000, obj.jumlah_kasbon);
            Assert.AreEqual(200000, obj.total_pelunasan);
            Assert.AreEqual("tambahan kasbon", obj.keterangan_kasbon);

            // detail kasbon
            Assert.AreEqual(new DateTime(2017, 3, 30), obj.tanggal_pembayaran);
            Assert.AreEqual("201703300030", obj.nota_pembayaran);
            Assert.AreEqual(50000, obj.jumlah_pembayaran);
            Assert.AreEqual("pelunasan", obj.keterangan_pembayaran);
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

            // header kasbon
            Assert.AreEqual("0e0251ab-7c99-40fc-85ec-7974eeebc800", obj.karyawan_id);
            Assert.AreEqual("Andi", obj.nama_karyawan);
            Assert.AreEqual(new DateTime(2017, 3, 29), obj.tanggal_kasbon);
            Assert.AreEqual("201703280009", obj.nota_kasbon);
            Assert.AreEqual(200000, obj.jumlah_kasbon);
            Assert.AreEqual(200000, obj.total_pelunasan);
            Assert.AreEqual("tambahan kasbon", obj.keterangan_kasbon);

            // detail kasbon
            Assert.AreEqual(new DateTime(2017, 3, 30), obj.tanggal_pembayaran);
            Assert.AreEqual("201703300030", obj.nota_pembayaran);
            Assert.AreEqual(50000, obj.jumlah_pembayaran);
            Assert.AreEqual("pelunasan", obj.keterangan_pembayaran);
        }

        [TestMethod]
        public void DetailGetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 3, 30);

            var oList = _bll.DetailGetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            // header kasbon
            Assert.AreEqual("0e0251ab-7c99-40fc-85ec-7974eeebc800", obj.karyawan_id);
            Assert.AreEqual("Andi", obj.nama_karyawan);
            Assert.AreEqual(new DateTime(2017, 3, 29), obj.tanggal_kasbon);
            Assert.AreEqual("201703280009", obj.nota_kasbon);
            Assert.AreEqual(200000, obj.jumlah_kasbon);
            Assert.AreEqual(200000, obj.total_pelunasan);
            Assert.AreEqual("tambahan kasbon", obj.keterangan_kasbon);

            // detail kasbon
            Assert.AreEqual(new DateTime(2017, 3, 30), obj.tanggal_pembayaran);
            Assert.AreEqual("201703300030", obj.nota_pembayaran);
            Assert.AreEqual(50000, obj.jumlah_pembayaran);
            Assert.AreEqual("pelunasan", obj.keterangan_pembayaran);
        }
    }
}
