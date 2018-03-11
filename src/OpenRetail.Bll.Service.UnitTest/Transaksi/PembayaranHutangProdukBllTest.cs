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
    public class PembayaranHutangProdukBllTest
    {
        private ILog _log;
        private IPembayaranHutangProdukBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(PembayaranHutangProdukBllTest));
            _bll = new PembayaranHutangProdukBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByBeliIDTest()
        {
            var id = "0983d9b8-7abe-4be2-9383-16607fcfc91a";
            var obj = _bll.GetByBeliID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("6d1c2bb2-1e08-4c9c-b740-e12f2e6dbcfa", obj.item_pembayaran_hutang_produk_id);
            Assert.AreEqual("0983d9b8-7abe-4be2-9383-16607fcfc91a", obj.beli_produk_id);
            Assert.AreEqual(500000, obj.nominal);
            Assert.AreEqual("keterangan #1", obj.keterangan);
            Assert.AreEqual("ket header", obj.PembayaranHutangProduk.keterangan);
            Assert.AreEqual("BB-123456", obj.PembayaranHutangProduk.nota);
            Assert.IsTrue(obj.PembayaranHutangProduk.is_tunai);
                     
        }

        [TestMethod]
        public void GetAllTest()
        {            
            var index = 0;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("d4e66a6c-c0b2-49e1-be4d-b33e7b1bd565", obj.pembayaran_hutang_produk_id);
            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", obj.supplier_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);                                
            Assert.AreEqual(new DateTime(2017, 1, 9), obj.tanggal);
            Assert.AreEqual("ket header", obj.keterangan);                                
            Assert.AreEqual("BB-123456", obj.nota);                                
            Assert.IsTrue(obj.is_tunai);

            var supplier = obj.Supplier;
            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", supplier.supplier_id);
            Assert.AreEqual("Pixel Computer", supplier.nama_supplier);                     
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var index = 0;
            var tglMulai = new DateTime(2017, 1, 1);
            var tglSelesai = new DateTime(2017, 1, 9);

            var oList = _bll.GetByTanggal(tglMulai, tglSelesai);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("d4e66a6c-c0b2-49e1-be4d-b33e7b1bd565", obj.pembayaran_hutang_produk_id);
            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", obj.supplier_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual(new DateTime(2017, 1, 9), obj.tanggal);
            Assert.AreEqual("ket header", obj.keterangan);
            Assert.AreEqual("BB-123456", obj.nota);
            Assert.IsTrue(obj.is_tunai);

            var supplier = obj.Supplier;
            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", supplier.supplier_id);
            Assert.AreEqual("Pixel Computer", supplier.nama_supplier);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new PembayaranHutangProduk
            {
                supplier_id = "e6201c8e-74e3-467c-a463-c8ea1763668e",
                pengguna_id = "00b5acfa-b533-454b-8dfd-e7881edd180f",
                tanggal = DateTime.Today,
                keterangan = "keterangan header",
                nota = "BB-12345",
                is_tunai = false
            };

            var listOfItemPembayaranHutang = new List<ItemPembayaranHutangProduk>();
            listOfItemPembayaranHutang.Add(new ItemPembayaranHutangProduk { BeliProduk = new BeliProduk { beli_produk_id = "0983d9b8-7abe-4be2-9383-16607fcfc91a" }, beli_produk_id = "0983d9b8-7abe-4be2-9383-16607fcfc91a", nominal = 500000, keterangan = "keterangan #1" });
            listOfItemPembayaranHutang.Add(new ItemPembayaranHutangProduk { BeliProduk = new BeliProduk { beli_produk_id = "70c46d69-ca7c-46b2-bd18-ebf03a28d02b" }, beli_produk_id = "70c46d69-ca7c-46b2-bd18-ebf03a28d02b", nominal = 700000, keterangan = "keterangan #2" });

            obj.item_pembayaran_hutang = listOfItemPembayaranHutang;

            var validationError = new ValidationError();

            var result = _bll.Save(obj, false, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.pembayaran_hutang_produk_id);
			Assert.IsNotNull(newObj);
			Assert.AreEqual(obj.pembayaran_hutang_produk_id, newObj.pembayaran_hutang_produk_id);                                
            Assert.AreEqual(obj.supplier_id, newObj.supplier_id);                                
            Assert.AreEqual(obj.pengguna_id, newObj.pengguna_id);                                
            Assert.AreEqual(obj.tanggal, newObj.tanggal);                                
            Assert.AreEqual(obj.keterangan, newObj.keterangan);                                
            Assert.AreEqual(obj.nota, newObj.nota);                                
            Assert.AreEqual(obj.is_tunai, newObj.is_tunai);                                
            
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("d4e66a6c-c0b2-49e1-be4d-b33e7b1bd565");
            obj.tanggal = new DateTime(2017, 1, 9);
            obj.keterangan = "ket header";
            obj.nota = "BB-123456";
            obj.is_tunai = true;

            var itemPembayaran = obj.item_pembayaran_hutang[1];
            itemPembayaran.nominal = 750000;
            itemPembayaran.keterangan = "keterangan #22";
            itemPembayaran.entity_state = EntityState.Modified;

            var validationError = new ValidationError();

            var result = _bll.Update(obj, false, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.pembayaran_hutang_produk_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.pembayaran_hutang_produk_id, updatedObj.pembayaran_hutang_produk_id);
            Assert.AreEqual(obj.supplier_id, updatedObj.supplier_id);
            Assert.AreEqual(obj.pengguna_id, updatedObj.pengguna_id);
            Assert.AreEqual(obj.tanggal, updatedObj.tanggal);
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);
            Assert.AreEqual(obj.nota, updatedObj.nota);
            Assert.AreEqual(obj.is_tunai, updatedObj.is_tunai);                                
            
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new PembayaranHutangProduk
            {
                pembayaran_hutang_produk_id = "d4e66a6c-c0b2-49e1-be4d-b33e7b1bd565"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.pembayaran_hutang_produk_id);
			Assert.IsNull(deletedObj);
        }
    }
}     
