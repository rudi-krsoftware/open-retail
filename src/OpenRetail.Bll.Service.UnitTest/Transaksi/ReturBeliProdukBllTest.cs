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
    public class ReturBeliProdukBllTest
    {
		private ILog _log;
        private IReturBeliProdukBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReturBeliProdukBllTest));
            _bll = new ReturBeliProdukBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetLastNotaTest()
        {
            var lastNota = _bll.GetLastNota();
            Assert.AreEqual("201701210002", lastNota);

            lastNota = _bll.GetLastNota();
            Assert.AreEqual("201701210003", lastNota);
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var index = 0;
            var tglMulai = new DateTime(2017, 1, 1);
            var tglSelesai = new DateTime(2017, 1, 21);

            var oList = _bll.GetByTanggal(tglMulai, tglSelesai);
            var obj = oList[index];

            // tes retur     
            Assert.IsNotNull(obj);
            Assert.AreEqual("2fb82570-d64e-4f2d-bd03-aab5bdf75884", obj.retur_beli_produk_id);
            Assert.AreEqual("27d40236-c8ab-44be-bc47-7a9bbd68c31e", obj.beli_produk_id);
            Assert.AreEqual("af01c916-7976-4518-a563-9d2a1851a912", obj.supplier_id);
            Assert.AreEqual("201701210010", obj.nota);
            Assert.AreEqual(DateTime.Today, obj.tanggal);
            Assert.AreEqual("keterangan", obj.keterangan);
            Assert.AreEqual(160000, obj.total_nota);

            // tes item retur
            Assert.AreEqual(2, obj.item_retur.Count);

            index = 1;
            var itemRetur = obj.item_retur[index];
            Assert.AreEqual("d7e888eb-6f9b-43ef-9a72-212588d2fb38", itemRetur.Produk.produk_id);
            Assert.AreEqual("12345", itemRetur.Produk.kode_produk);
            Assert.AreEqual("susu coklat", itemRetur.Produk.nama_produk);
            Assert.AreEqual(2, itemRetur.jumlah_retur);
            Assert.AreEqual(3500, itemRetur.harga);

            // tes supplier
            var supplier = obj.Supplier;
            Assert.AreEqual("af01c916-7976-4518-a563-9d2a1851a912", supplier.supplier_id);
            Assert.AreEqual("TE Shop", supplier.nama_supplier);
            Assert.AreEqual("Yogyakarta", supplier.alamat);

            // tes beli
            var beli = obj.BeliProduk;
            Assert.AreEqual("27d40236-c8ab-44be-bc47-7a9bbd68c31e", beli.beli_produk_id);
            Assert.AreEqual("201701200045", beli.nota);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 0;
            var oList = _bll.GetAll();
            var obj = oList[index];
            
            // tes retur     
            Assert.IsNotNull(obj);
            Assert.AreEqual("2fb82570-d64e-4f2d-bd03-aab5bdf75884", obj.retur_beli_produk_id);
            Assert.AreEqual("27d40236-c8ab-44be-bc47-7a9bbd68c31e", obj.beli_produk_id);
            Assert.AreEqual("af01c916-7976-4518-a563-9d2a1851a912", obj.supplier_id);
            Assert.AreEqual("201701210010", obj.nota);                                
            Assert.AreEqual(DateTime.Today, obj.tanggal);
            Assert.AreEqual("keterangan", obj.keterangan);
            Assert.AreEqual(160000, obj.total_nota);

            // tes item retur
            Assert.AreEqual(2, obj.item_retur.Count);

            index = 1;
            var itemRetur = obj.item_retur[index];
            Assert.AreEqual("d7e888eb-6f9b-43ef-9a72-212588d2fb38", itemRetur.Produk.produk_id);
            Assert.AreEqual("12345", itemRetur.Produk.kode_produk);
            Assert.AreEqual("susu coklat", itemRetur.Produk.nama_produk);
            Assert.AreEqual(2, itemRetur.jumlah_retur);
            Assert.AreEqual(3500, itemRetur.harga);

            // tes supplier
            var supplier = obj.Supplier;
            Assert.AreEqual("af01c916-7976-4518-a563-9d2a1851a912", supplier.supplier_id);
            Assert.AreEqual("TE Shop", supplier.nama_supplier);
            Assert.AreEqual("Yogyakarta", supplier.alamat);

            // tes beli
            var beli = obj.BeliProduk;
            Assert.AreEqual("27d40236-c8ab-44be-bc47-7a9bbd68c31e", beli.beli_produk_id);
            Assert.AreEqual("201701200045", beli.nota);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new ReturBeliProduk
            {
                beli_produk_id = "27d40236-c8ab-44be-bc47-7a9bbd68c31e",
                supplier_id = "af01c916-7976-4518-a563-9d2a1851a912",
                nota = _bll.GetLastNota(),
                tanggal = DateTime.Today,
                keterangan = "keterangan header"
            };

            var listOfItemBeli = new List<ItemReturBeliProduk>();
            listOfItemBeli.Add(new ItemReturBeliProduk { item_beli_id = "a53a632c-2759-4d85-acc6-0cbb18a0c88b", Produk = new Produk { produk_id = "6e587b32-9d87-4ec3-8e7c-ce15c7b0aecd" }, produk_id = "6e587b32-9d87-4ec3-8e7c-ce15c7b0aecd", harga = 50000, jumlah = 4, jumlah_retur = 2 });
            listOfItemBeli.Add(new ItemReturBeliProduk { item_beli_id = "c414c56c-fd01-4e88-bae1-96bfe0f8196a", Produk = new Produk { produk_id = "d7e888eb-6f9b-43ef-9a72-212588d2fb38" }, produk_id = "d7e888eb-6f9b-43ef-9a72-212588d2fb38", harga = 2500,jumlah = 2, jumlah_retur = 1 });

            obj.item_retur = listOfItemBeli; // menghubungkan retur dan item retur
            
            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.retur_beli_produk_id);
			Assert.IsNotNull(newObj);
			Assert.AreEqual(obj.retur_beli_produk_id, newObj.retur_beli_produk_id);                                
            Assert.AreEqual(obj.beli_produk_id, newObj.beli_produk_id);                                
            Assert.AreEqual(obj.pengguna_id, newObj.pengguna_id);                                
            Assert.AreEqual(obj.supplier_id, newObj.supplier_id);                                
            Assert.AreEqual(obj.nota, newObj.nota);                                
            Assert.AreEqual(obj.tanggal, newObj.tanggal);                                
            Assert.AreEqual(obj.keterangan, newObj.keterangan);                                
            Assert.AreEqual(obj.total_nota, newObj.total_nota);                                
            
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("2fb82570-d64e-4f2d-bd03-aab5bdf75884");
            obj.nota = "201701210010";
            obj.keterangan = "keterangan";

            foreach (var itemRetur in obj.item_retur)
            {
                itemRetur.jumlah_retur += 1;
                itemRetur.harga += 1000;
                itemRetur.entity_state = EntityState.Modified;
            }

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.retur_beli_produk_id);
            Assert.IsNotNull(updatedObj);

            Assert.AreEqual(obj.retur_beli_produk_id, updatedObj.retur_beli_produk_id);
            Assert.AreEqual(obj.beli_produk_id, updatedObj.beli_produk_id);
            Assert.AreEqual(obj.pengguna_id, updatedObj.pengguna_id);
            Assert.AreEqual(obj.supplier_id, updatedObj.supplier_id);
            Assert.AreEqual(obj.nota, updatedObj.nota);
            Assert.AreEqual(obj.tanggal, updatedObj.tanggal);
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);
            Assert.AreEqual(obj.total_nota, updatedObj.total_nota);

        }
    }
}     
