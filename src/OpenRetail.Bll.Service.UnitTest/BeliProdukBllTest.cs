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
    public class BeliProdukBllTest
    {
        private ILog _log;
        private IBeliProdukBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(BeliProdukBllTest));
            _bll = new BeliProdukBll(_log);
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
            Assert.AreEqual("201701100003", lastNota);

            lastNota = _bll.GetLastNota();
            Assert.AreEqual("201701100004", lastNota);
        }

        [TestMethod]
        public void GetByNameTest()
        {
            var name = "pix";

            var index = 0;
            var oList = _bll.GetByName(name);
            var obj = oList[index];
            
            // tes header table beli                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("70c46d69-ca7c-46b2-bd18-ebf03a28d02b", obj.beli_produk_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", obj.supplier_id);
            Assert.IsNull(obj.retur_beli_produk_id);
            Assert.AreEqual("22222", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 1), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 1, 25), obj.tanggal_tempo);
            Assert.AreEqual(20000, obj.ppn);
            Assert.AreEqual(7500, obj.diskon);
            Assert.AreEqual(2021000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("tesssss", obj.keterangan);

            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", obj.Supplier.supplier_id);
            Assert.AreEqual("Pixel Computer", obj.Supplier.nama_supplier);
            Assert.AreEqual("Solo", obj.Supplier.alamat);    
            
            // tes detail table item beli  
            index = 2;
            Assert.AreEqual(3, obj.item_beli.Count);

            var itemBeli = obj.item_beli[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemBeli.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemBeli.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemBeli.Produk.nama_produk);

            Assert.AreEqual(71000, itemBeli.harga);
            Assert.AreEqual(16, itemBeli.jumlah);
                     
        }

        [TestMethod]
        public void GetAllTest()
        {            
            var index = 0;
            var oList = _bll.GetAll();
            var obj = oList[index];

            // tes header table (beli)     
            Assert.IsNotNull(obj);
            Assert.AreEqual("3d4ce868-3f9d-4a6a-88ba-a88ee70ef013", obj.beli_produk_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual("7560fd72-0538-4307-8f15-14ef32cf5158", obj.supplier_id);                                
            Assert.IsNull(obj.retur_beli_produk_id);
            Assert.AreEqual("1234447", obj.nota);                                
            Assert.AreEqual(new DateTime(2017, 1, 1), obj.tanggal);                                
            Assert.IsNull(obj.tanggal_tempo);                                
            Assert.AreEqual(20000, obj.ppn);                                
            Assert.AreEqual(7500, obj.diskon);
            Assert.AreEqual(2021000, obj.total_nota);                                
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("tesssss", obj.keterangan);

            Assert.AreEqual("7560fd72-0538-4307-8f15-14ef32cf5158", obj.Supplier.supplier_id);
            Assert.AreEqual("Toko Komputer \"XYZ\"", obj.Supplier.nama_supplier);

            // tes detail table (item beli)
            index = 2;
            Assert.AreEqual(3, obj.item_beli.Count);

            var itemBeli = obj.item_beli[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemBeli.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemBeli.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemBeli.Produk.nama_produk);

            Assert.AreEqual(71000, itemBeli.harga);
            Assert.AreEqual(16, itemBeli.jumlah);
        }

        [TestMethod]
        public void GetAllAndNameTest()
        {
            var index = 0;
            var oList = _bll.GetAll("xyz");
            var obj = oList[index];

            // tes header table (beli)
            Assert.IsNotNull(obj);
            Assert.AreEqual("3d4ce868-3f9d-4a6a-88ba-a88ee70ef013", obj.beli_produk_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual("7560fd72-0538-4307-8f15-14ef32cf5158", obj.supplier_id);
            Assert.IsNull(obj.retur_beli_produk_id);
            Assert.AreEqual("1234447", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 1), obj.tanggal);
            Assert.IsNull(obj.tanggal_tempo);
            Assert.AreEqual(20000, obj.ppn);
            Assert.AreEqual(7500, obj.diskon);
            Assert.AreEqual(2021000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("tesssss", obj.keterangan);

            Assert.AreEqual("7560fd72-0538-4307-8f15-14ef32cf5158", obj.Supplier.supplier_id);
            Assert.AreEqual("Toko Komputer \"XYZ\"", obj.Supplier.nama_supplier);

            // tes detail table (item beli)
            index = 2;
            Assert.AreEqual(3, obj.item_beli.Count);

            var itemBeli = obj.item_beli[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemBeli.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemBeli.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemBeli.Produk.nama_produk);

            Assert.AreEqual(71000, itemBeli.harga);
            Assert.AreEqual(16, itemBeli.jumlah);

        }

        [TestMethod]
        public void GetNotaSupplierTest()
        {
            var index = 0;
            var supplierId = "7560fd72-0538-4307-8f15-14ef32cf5158";
            var oList = _bll.GetNotaSupplier(supplierId, "123");

            var obj = oList[index];

            // tes header table (beli)
            Assert.IsNotNull(obj);
            Assert.AreEqual("3d4ce868-3f9d-4a6a-88ba-a88ee70ef013", obj.beli_produk_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual("7560fd72-0538-4307-8f15-14ef32cf5158", obj.supplier_id);
            Assert.IsNull(obj.retur_beli_produk_id);
            Assert.AreEqual("1234447", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 1), obj.tanggal);
            Assert.IsNull(obj.tanggal_tempo);
            Assert.AreEqual(20000, obj.ppn);
            Assert.AreEqual(7500, obj.diskon);
            Assert.AreEqual(2021000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("tesssss", obj.keterangan);

            Assert.AreEqual("7560fd72-0538-4307-8f15-14ef32cf5158", obj.Supplier.supplier_id);
            Assert.AreEqual("Toko Komputer \"XYZ\"", obj.Supplier.nama_supplier);

            // tes detail table (item beli)
            index = 2;
            Assert.AreEqual(3, obj.item_beli.Count);

            var itemBeli = obj.item_beli[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemBeli.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemBeli.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemBeli.Produk.nama_produk);

            Assert.AreEqual(71000, itemBeli.harga);
            Assert.AreEqual(16, itemBeli.jumlah);

        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var index = 0;
            var tglMulai = new DateTime(2017, 1, 1);
            var tglSelesai = new DateTime(2017, 1, 10);

            var oList = _bll.GetByTanggal(tglMulai, tglSelesai);

            var obj = oList[index];

            // tes header table (beli)
            Assert.IsNotNull(obj);
            Assert.AreEqual("3d4ce868-3f9d-4a6a-88ba-a88ee70ef013", obj.beli_produk_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual("7560fd72-0538-4307-8f15-14ef32cf5158", obj.supplier_id);
            Assert.IsNull(obj.retur_beli_produk_id);
            Assert.AreEqual("1234447", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 1), obj.tanggal);
            Assert.IsNull(obj.tanggal_tempo);
            Assert.AreEqual(20000, obj.ppn);
            Assert.AreEqual(7500, obj.diskon);
            Assert.AreEqual(2021000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("tesssss", obj.keterangan);

            Assert.AreEqual("7560fd72-0538-4307-8f15-14ef32cf5158", obj.Supplier.supplier_id);
            Assert.AreEqual("Toko Komputer \"XYZ\"", obj.Supplier.nama_supplier);

            // tes detail table (item beli)
            index = 2;
            Assert.AreEqual(3, obj.item_beli.Count);

            var itemBeli = obj.item_beli[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemBeli.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemBeli.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemBeli.Produk.nama_produk);

            Assert.AreEqual(71000, itemBeli.harga);
            Assert.AreEqual(16, itemBeli.jumlah);

        }

        [TestMethod]
        public void GetByTanggalAndNameTest()
        {
            var index = 0;
            var tglMulai = new DateTime(2017, 1, 1);
            var tglSelesai = new DateTime(2017, 1, 10);
            var name = "komputer";

            var oList = _bll.GetByTanggal(tglMulai, tglSelesai, name);

            var obj = oList[index];

            // tes header table (beli)
            Assert.IsNotNull(obj);
            Assert.AreEqual("3d4ce868-3f9d-4a6a-88ba-a88ee70ef013", obj.beli_produk_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual("7560fd72-0538-4307-8f15-14ef32cf5158", obj.supplier_id);
            Assert.IsNull(obj.retur_beli_produk_id);
            Assert.AreEqual("1234447", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 1), obj.tanggal);
            Assert.IsNull(obj.tanggal_tempo);
            Assert.AreEqual(20000, obj.ppn);
            Assert.AreEqual(7500, obj.diskon);
            Assert.AreEqual(2021000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("tesssss", obj.keterangan);

            Assert.AreEqual("7560fd72-0538-4307-8f15-14ef32cf5158", obj.Supplier.supplier_id);
            Assert.AreEqual("Toko Komputer \"XYZ\"", obj.Supplier.nama_supplier);

            // tes detail table (item beli)
            index = 2;
            Assert.AreEqual(3, obj.item_beli.Count);

            var itemBeli = obj.item_beli[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemBeli.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemBeli.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemBeli.Produk.nama_produk);

            Assert.AreEqual(71000, itemBeli.harga);
            Assert.AreEqual(16, itemBeli.jumlah);

        }

        [TestMethod]
        public void GetNotaKreditBySupplierTest()
        {
            var index = 0;
            var supplierId = "e6201c8e-74e3-467c-a463-c8ea1763668e";
            var oList = _bll.GetNotaKreditBySupplier(supplierId, false);

            var obj = oList[index];

            // tes header table (beli)
            Assert.IsNotNull(obj);
            Assert.AreEqual("70c46d69-ca7c-46b2-bd18-ebf03a28d02b", obj.beli_produk_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", obj.supplier_id);
            Assert.IsNull(obj.retur_beli_produk_id);
            Assert.AreEqual("22222", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 1), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 1, 25), obj.tanggal_tempo);
            Assert.AreEqual(20000, obj.ppn);
            Assert.AreEqual(7500, obj.diskon);
            Assert.AreEqual(2021000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("tesssss", obj.keterangan);

            Assert.AreEqual("e6201c8e-74e3-467c-a463-c8ea1763668e", obj.Supplier.supplier_id);
            Assert.AreEqual("Pixel Computer", obj.Supplier.nama_supplier);
            Assert.AreEqual("Solo", obj.Supplier.alamat);
        }

        [TestMethod]
        public void GetItemBeliTest()
        {
            var index = 2;
            var beliId = "70c46d69-ca7c-46b2-bd18-ebf03a28d02b";

            var oList = _bll.GetItemBeli(beliId);

            var itemBeli = oList[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemBeli.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemBeli.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemBeli.Produk.nama_produk);

            Assert.AreEqual(71000, itemBeli.harga);
            Assert.AreEqual(16, itemBeli.jumlah);

        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new BeliProduk
            {
                pengguna_id = "00b5acfa-b533-454b-8dfd-e7881edd180f",
                supplier_id = "e6201c8e-74e3-467c-a463-c8ea1763668e",
                nota = "12345",
                tanggal = DateTime.Today,
                ppn = 15000,
                diskon = 5000,
                keterangan = "pembelian tunai"
            };

            var listOfItemBeli = new List<ItemBeliProduk>();
            listOfItemBeli.Add(new ItemBeliProduk { Produk = new Produk { produk_id = "eafc755f-cab6-4066-a793-660fcfab20d0" }, produk_id = "eafc755f-cab6-4066-a793-660fcfab20d0", harga = 53000, jumlah = 5, diskon = 2 });
            listOfItemBeli.Add(new ItemBeliProduk { Produk = new Produk { produk_id = "6e587b32-9d87-4ec3-8e7c-ce15c7b0aecd" }, produk_id = "6e587b32-9d87-4ec3-8e7c-ce15c7b0aecd", harga = 50000, jumlah = 10, diskon = 0 });
            listOfItemBeli.Add(new ItemBeliProduk { Produk = new Produk { produk_id = "7f09a4aa-e660-4de3-a3aa-4b3244675f9f" }, produk_id = "7f09a4aa-e660-4de3-a3aa-4b3244675f9f", harga = 70000, jumlah = 15, diskon = 5 });

            obj.item_beli = listOfItemBeli; // menghubungkan beli dan item beli

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            // tes hasil penyimpanan ke tabel beli
            var newObj = _bll.GetByID(obj.beli_produk_id);
			Assert.IsNotNull(newObj);
			Assert.AreEqual(obj.beli_produk_id, newObj.beli_produk_id);                                
            Assert.AreEqual(obj.pengguna_id, newObj.pengguna_id);                                
            Assert.AreEqual(obj.supplier_id, newObj.supplier_id);                                
            Assert.AreEqual(obj.nota, newObj.nota);                                
            Assert.AreEqual(obj.tanggal, newObj.tanggal);                                
            Assert.AreEqual(obj.tanggal_tempo, newObj.tanggal_tempo);                                
            Assert.AreEqual(obj.ppn, newObj.ppn);                                
            Assert.AreEqual(obj.diskon, newObj.diskon);                                
            Assert.AreEqual(obj.total_nota, newObj.total_nota);                                
            //Assert.AreEqual(obj.total_pelunasan, newObj.total_pelunasan);                                
            Assert.AreEqual(obj.keterangan, newObj.keterangan);

            // tes hasil penyimpanan ke tabel item beli
            Assert.AreEqual(3, newObj.item_beli.Count);

            var index = 0;
            foreach (var itemBeli in newObj.item_beli)
            {
                Assert.AreEqual(listOfItemBeli[index].produk_id, itemBeli.produk_id);
                Assert.AreEqual(listOfItemBeli[index].harga, itemBeli.harga);
                Assert.AreEqual(listOfItemBeli[index].jumlah, itemBeli.jumlah);
                Assert.AreEqual(listOfItemBeli[index].diskon, itemBeli.diskon);

                index++;
            }
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("70c46d69-ca7c-46b2-bd18-ebf03a28d02b");
            obj.nota = "22222";
            obj.tanggal = new DateTime(2017, 1, 1);
            obj.tanggal_tempo = new DateTime(2017, 1, 25);
            obj.ppn = 20000;
            obj.diskon = 7500;
            obj.keterangan = "tesssss";

            foreach (var itemBeli in obj.item_beli)
            {                
                itemBeli.jumlah = itemBeli.jumlah + 1;
                itemBeli.harga = itemBeli.harga + 1000;
                itemBeli.diskon = 0;
                itemBeli.entity_state = EntityState.Modified;
            }

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.beli_produk_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.beli_produk_id, updatedObj.beli_produk_id);
            Assert.AreEqual(obj.pengguna_id, updatedObj.pengguna_id);
            Assert.AreEqual(obj.supplier_id, updatedObj.supplier_id);
            Assert.AreEqual(obj.nota, updatedObj.nota);
            Assert.AreEqual(obj.tanggal, updatedObj.tanggal);
            Assert.AreEqual(obj.tanggal_tempo, updatedObj.tanggal_tempo);
            Assert.AreEqual(obj.ppn, updatedObj.ppn);
            Assert.AreEqual(obj.diskon, updatedObj.diskon);
            Assert.AreEqual(obj.total_nota, updatedObj.total_nota);
            //Assert.AreEqual(obj.total_pelunasan, newObj.total_pelunasan);                                
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);

            // tes hasil update ke tabel item beli
            Assert.AreEqual(3, updatedObj.item_beli.Count);

            var index = 0;
            foreach (var itemBeliUpdated in updatedObj.item_beli)
            {
                Assert.AreEqual(obj.item_beli[index].produk_id, itemBeliUpdated.produk_id);
                Assert.AreEqual(obj.item_beli[index].harga, itemBeliUpdated.harga);
                Assert.AreEqual(obj.item_beli[index].jumlah, itemBeliUpdated.jumlah);
                Assert.AreEqual(obj.item_beli[index].diskon, itemBeliUpdated.diskon);

                index++;
            }                                
            
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new BeliProduk
            {
                beli_produk_id = "9fdd5459-f9cb-4361-bce7-7edd32f4eb13"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.beli_produk_id);
			Assert.IsNull(deletedObj);
        }
    }
}     
