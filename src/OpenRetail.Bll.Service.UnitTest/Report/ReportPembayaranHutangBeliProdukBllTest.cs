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
    public class ReportPembayaranHutangBeliProdukBllTest
    {
        private ILog _log;
        private IReportPembayaranHutangBeliProdukBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReportPembayaranHutangBeliProdukBllTest));
            _bll = new ReportPembayaranHutangBeliProdukBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByBulanAndTahunTest()
        {
            var bulan = 2;
            var tahun = 2017;

            var oList = _bll.GetByBulan(bulan, tahun);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", obj.supplier_id);
            Assert.AreEqual("Pixel Computer", obj.nama_supplier);
            Assert.AreEqual(new DateTime(2017, 2, 4), obj.tanggal);
            Assert.AreEqual(1825000, obj.total_pembayaran);
            Assert.AreEqual("Pembelian tunai produk", obj.keterangan);
        }

        [TestMethod]
        public void GetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 2;
            var tahun = 2017;

            var oList = _bll.GetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 5;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", obj.supplier_id);
            Assert.AreEqual("Pixel Computer", obj.nama_supplier);
            Assert.AreEqual(new DateTime(2017, 2, 4), obj.tanggal);
            Assert.AreEqual(1825000, obj.total_pembayaran);
            Assert.AreEqual("Pembelian tunai produk", obj.keterangan);
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 2, 1);
            var tanggalSelesai = new DateTime(2017, 2, 28);

            var oList = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 2;
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", obj.supplier_id);
            Assert.AreEqual("Pixel Computer", obj.nama_supplier);
            Assert.AreEqual(new DateTime(2017, 2, 4), obj.tanggal);
            Assert.AreEqual(1825000, obj.total_pembayaran);
            Assert.AreEqual("Pembelian tunai produk", obj.keterangan);
        }

        [TestMethod]
        public void DetailGetByBulanAndTahunTest()
        {
            var bulan = 2;
            var tahun = 2017;

            var oList = _bll.DetailGetByBulan(bulan, tahun);

            var index = 4;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            Assert.AreEqual("85ecb92b-3cb7-4d98-8390-cc76a942b880", obj.supplier_id);
            Assert.AreEqual("Sigma komputer", obj.nama_supplier);
            Assert.AreEqual("201702010080", obj.nota_beli);
            Assert.AreEqual("201702010047", obj.nota_bayar);
            Assert.AreEqual(new DateTime(2017, 2, 1), obj.tanggal);
            Assert.AreEqual(100000, obj.ppn);
            Assert.AreEqual(150000, obj.diskon);
            Assert.AreEqual(1651000, obj.total_nota);
            Assert.AreEqual(1601000, obj.pelunasan);            
            Assert.AreEqual("", obj.keterangan_beli);
            Assert.AreEqual("Pembelian tunai produk", obj.keterangan_bayar);            
        }

        [TestMethod]
        public void DetailGetByRangeBulanAndTahunTest()
        {
            var bulanAwal = 1;
            var bulanAkhir = 2;
            var tahun = 2017;

            var oList = _bll.DetailGetByBulan(bulanAwal, bulanAkhir, tahun);

            var index = 5;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            Assert.AreEqual("85ecb92b-3cb7-4d98-8390-cc76a942b880", obj.supplier_id);
            Assert.AreEqual("Sigma komputer", obj.nama_supplier);
            Assert.AreEqual("201702010080", obj.nota_beli);
            Assert.AreEqual("201702010047", obj.nota_bayar);
            Assert.AreEqual(new DateTime(2017, 2, 1), obj.tanggal);
            Assert.AreEqual(100000, obj.ppn);
            Assert.AreEqual(150000, obj.diskon);
            Assert.AreEqual(1651000, obj.total_nota);
            Assert.AreEqual(1601000, obj.pelunasan);
            Assert.AreEqual("", obj.keterangan_beli);
            Assert.AreEqual("Pembelian tunai produk", obj.keterangan_bayar);         
        }

        [TestMethod]
        public void DetailGetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 2, 28);

            var oList = _bll.DetailGetByTanggal(tanggalMulai, tanggalSelesai);

            var index = 5;
            var obj = oList[index];

            Assert.IsNotNull(obj);

            Assert.AreEqual("85ecb92b-3cb7-4d98-8390-cc76a942b880", obj.supplier_id);
            Assert.AreEqual("Sigma komputer", obj.nama_supplier);
            Assert.AreEqual("201702010080", obj.nota_beli);
            Assert.AreEqual("201702010047", obj.nota_bayar);
            Assert.AreEqual(new DateTime(2017, 2, 1), obj.tanggal);
            Assert.AreEqual(100000, obj.ppn);
            Assert.AreEqual(150000, obj.diskon);
            Assert.AreEqual(1651000, obj.total_nota);
            Assert.AreEqual(1601000, obj.pelunasan);
            Assert.AreEqual("", obj.keterangan_beli);
            Assert.AreEqual("Pembelian tunai produk", obj.keterangan_bayar);        
        }
    }
}
