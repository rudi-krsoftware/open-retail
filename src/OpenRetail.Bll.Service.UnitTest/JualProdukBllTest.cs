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
    public class JualProdukBllTest
    {
		private ILog _log;
        private IJualProdukBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(JualProdukBllTest));
            _bll = new JualProdukBll(_log);
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
            Assert.AreEqual("201701180010", lastNota);

            lastNota = _bll.GetLastNota();
            Assert.AreEqual("201701180011", lastNota);
        }

        [TestMethod]
        public void GetByNameTest()
        {
            var name = "swalayan";

            var index = 1;
            var oList = _bll.GetByName(name);
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("376625eb-13ba-4620-bc12-e8260501b689", obj.jual_id);
            Assert.AreEqual("960a9111-a077-4e0e-a440-cef77293038a", obj.pengguna_id);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);                                
            Assert.AreEqual("12345", obj.nota);                                
            Assert.AreEqual(DateTime.Today, obj.tanggal);                                
            Assert.IsNull(obj.tanggal_tempo);                                
            Assert.AreEqual(15000, obj.ppn);                                
            Assert.AreEqual(5000, obj.diskon);
            Assert.AreEqual(1757200, obj.total_nota);                                
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("penjualan tunai", obj.keterangan);

            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.Customer.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.Customer.nama_customer);
            Assert.AreEqual("Seturan", obj.Customer.alamat);

            // tes detail table item jual  
            index = 2;
            Assert.AreEqual(3, obj.item_jual.Count);

            var itemJual = obj.item_jual[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemJual.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemJual.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemJual.Produk.nama_produk);

            Assert.AreEqual(70000, itemJual.harga_jual);
            Assert.AreEqual(15, itemJual.jumlah);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 2;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("376625eb-13ba-4620-bc12-e8260501b689", obj.jual_id);
            Assert.AreEqual("960a9111-a077-4e0e-a440-cef77293038a", obj.pengguna_id);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("12345", obj.nota);
            Assert.AreEqual(DateTime.Today, obj.tanggal);
            Assert.IsNull(obj.tanggal_tempo);
            Assert.AreEqual(15000, obj.ppn);
            Assert.AreEqual(5000, obj.diskon);
            Assert.AreEqual(1757200, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("penjualan tunai", obj.keterangan);

            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.Customer.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.Customer.nama_customer);
            Assert.AreEqual("Seturan", obj.Customer.alamat);

            // tes detail table item jual  
            index = 2;
            Assert.AreEqual(3, obj.item_jual.Count);

            var itemJual = obj.item_jual[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemJual.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemJual.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemJual.Produk.nama_produk);

            Assert.AreEqual(70000, itemJual.harga_jual);
            Assert.AreEqual(15, itemJual.jumlah);
                     
        }

        [TestMethod]
        public void GetAllAndNameTest()
        {
            var index = 1;
            var oList = _bll.GetAll("swalayan");
            var obj = oList[index];

            // tes header table (beli)
            Assert.IsNotNull(obj);
            Assert.AreEqual("376625eb-13ba-4620-bc12-e8260501b689", obj.jual_id);
            Assert.AreEqual("960a9111-a077-4e0e-a440-cef77293038a", obj.pengguna_id);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("12345", obj.nota);
            Assert.AreEqual(DateTime.Today, obj.tanggal);
            Assert.IsNull(obj.tanggal_tempo);
            Assert.AreEqual(15000, obj.ppn);
            Assert.AreEqual(5000, obj.diskon);
            Assert.AreEqual(1757200, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("penjualan tunai", obj.keterangan);

            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.Customer.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.Customer.nama_customer);
            Assert.AreEqual("Seturan", obj.Customer.alamat);

            // tes detail table item jual  
            index = 2;
            Assert.AreEqual(3, obj.item_jual.Count);

            var itemJual = obj.item_jual[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemJual.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemJual.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemJual.Produk.nama_produk);

            Assert.AreEqual(70000, itemJual.harga_jual);
            Assert.AreEqual(15, itemJual.jumlah);

        }

        [TestMethod]
        public void GetNotaCustomerTest()
        {
            var index = 0;
            var customerId = "c7b1ac7f-d201-474f-b018-1dc363d5d7f3";

            var oList = _bll.GetNotaCustomer(customerId, "123");
            var obj = oList[index];

            // tes header table (beli)
            Assert.IsNotNull(obj);
            Assert.AreEqual("376625eb-13ba-4620-bc12-e8260501b689", obj.jual_id);
            Assert.AreEqual("960a9111-a077-4e0e-a440-cef77293038a", obj.pengguna_id);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("12345", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 18), obj.tanggal);
            Assert.IsNull(obj.tanggal_tempo);
            Assert.AreEqual(15000, obj.ppn);
            Assert.AreEqual(5000, obj.diskon);
            Assert.AreEqual(1757200, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("penjualan tunai", obj.keterangan);

            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.Customer.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.Customer.nama_customer);
            Assert.AreEqual("Seturan", obj.Customer.alamat);

            // tes detail table item jual  
            index = 2;
            Assert.AreEqual(3, obj.item_jual.Count);

            var itemJual = obj.item_jual[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemJual.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemJual.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemJual.Produk.nama_produk);

            Assert.AreEqual(70000, itemJual.harga_jual);
            Assert.AreEqual(15, itemJual.jumlah);

        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var index = 1;
            var tglMulai = new DateTime(2017, 1, 1);
            var tglSelesai = new DateTime(2017, 1, 19);

            var oList = _bll.GetByTanggal(tglMulai, tglSelesai);
            var obj = oList[index];

            // tes header table (beli)
            Assert.IsNotNull(obj);
            Assert.AreEqual("376625eb-13ba-4620-bc12-e8260501b689", obj.jual_id);
            Assert.AreEqual("960a9111-a077-4e0e-a440-cef77293038a", obj.pengguna_id);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("12345", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 18), obj.tanggal);
            Assert.IsNull(obj.tanggal_tempo);
            Assert.AreEqual(15000, obj.ppn);
            Assert.AreEqual(5000, obj.diskon);
            Assert.AreEqual(1757200, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("penjualan tunai", obj.keterangan);

            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.Customer.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.Customer.nama_customer);
            Assert.AreEqual("Seturan", obj.Customer.alamat);

            // tes detail table item jual  
            index = 2;
            Assert.AreEqual(3, obj.item_jual.Count);

            var itemJual = obj.item_jual[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemJual.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemJual.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemJual.Produk.nama_produk);

            Assert.AreEqual(70000, itemJual.harga_jual);
            Assert.AreEqual(15, itemJual.jumlah);

        }

        [TestMethod]
        public void GetByTanggalAndNameTest()
        {
            var index = 0;
            var tglMulai = new DateTime(2017, 1, 1);
            var tglSelesai = new DateTime(2017, 1, 19);
            var name = "swalayan";

            var oList = _bll.GetByTanggal(tglMulai, tglSelesai, name);
            var obj = oList[index];

            // tes header table (beli)
            Assert.IsNotNull(obj);
            Assert.AreEqual("376625eb-13ba-4620-bc12-e8260501b689", obj.jual_id);
            Assert.AreEqual("960a9111-a077-4e0e-a440-cef77293038a", obj.pengguna_id);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("12345", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 18), obj.tanggal);
            Assert.IsNull(obj.tanggal_tempo);
            Assert.AreEqual(15000, obj.ppn);
            Assert.AreEqual(5000, obj.diskon);
            Assert.AreEqual(1757200, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("penjualan tunai", obj.keterangan);

            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.Customer.customer_id);
            Assert.AreEqual("Swalayan Citrouli", obj.Customer.nama_customer);
            Assert.AreEqual("Seturan", obj.Customer.alamat);

            // tes detail table item jual  
            index = 2;
            Assert.AreEqual(3, obj.item_jual.Count);

            var itemJual = obj.item_jual[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemJual.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemJual.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemJual.Produk.nama_produk);

            Assert.AreEqual(70000, itemJual.harga_jual);
            Assert.AreEqual(15, itemJual.jumlah);

        }

        [TestMethod]
        public void GetNotaKreditByCustomerTest()
        {
            var index = 0;
            var customerId = "576c503f-69a7-46a5-b4be-107c634db7e3";

            var oList = _bll.GetNotaKreditByCustomer(customerId, false);
            var obj = oList[index];

            // tes header table (beli)
            Assert.IsNotNull(obj);
            Assert.AreEqual("d1dbd28a-592f-4841-bfb6-bc41f48acf32", obj.jual_id);
            Assert.AreEqual("960a9111-a077-4e0e-a440-cef77293038a", obj.pengguna_id);
            Assert.AreEqual("576c503f-69a7-46a5-b4be-107c634db7e3", obj.customer_id);
            Assert.AreEqual("22222", obj.nota);
            Assert.AreEqual(new DateTime(2017, 1, 1), obj.tanggal);
            Assert.AreEqual(new DateTime(2017, 1, 25), obj.tanggal_tempo);
            Assert.AreEqual(20000, obj.ppn);
            Assert.AreEqual(7500, obj.diskon);
            Assert.AreEqual(3383000, obj.total_nota);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("tesssss", obj.keterangan);

            Assert.AreEqual("576c503f-69a7-46a5-b4be-107c634db7e3", obj.Customer.customer_id);
            Assert.AreEqual("Rudi", obj.Customer.nama_customer);
            Assert.AreEqual("", obj.Customer.alamat);

        }

        [TestMethod]
        public void GetItemJualTest()
        {
            var index = 2;
            var jualId = "d1dbd28a-592f-4841-bfb6-bc41f48acf32";

            var oList = _bll.GetItemJual(jualId);

            var itemBeli = oList[index];
            Assert.AreEqual("7f09a4aa-e660-4de3-a3aa-4b3244675f9f", itemBeli.Produk.produk_id);
            Assert.AreEqual("201607000000051", itemBeli.Produk.kode_produk);
            Assert.AreEqual("Access Point TPLINK TC-WA 500G", itemBeli.Produk.nama_produk);

            Assert.AreEqual(77000, itemBeli.harga_jual);
            Assert.AreEqual(22, itemBeli.jumlah);

        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new JualProduk
            {
                pengguna_id = "960a9111-a077-4e0e-a440-cef77293038a",
                customer_id = "c7b1ac7f-d201-474f-b018-1dc363d5d7f3",
                nota = "12345",
                tanggal = DateTime.Today,
                ppn = 15000,
                diskon = 5000,
                keterangan = "penjualan tunai"
            };

            var listOfItemJual = new List<ItemJualProduk>();
            listOfItemJual.Add(new ItemJualProduk { Produk = new Produk { produk_id = "eafc755f-cab6-4066-a793-660fcfab20d0" }, produk_id = "eafc755f-cab6-4066-a793-660fcfab20d0", harga_jual = 53000, jumlah = 5, diskon = 2 });
            listOfItemJual.Add(new ItemJualProduk { Produk = new Produk { produk_id = "6e587b32-9d87-4ec3-8e7c-ce15c7b0aecd" }, produk_id = "6e587b32-9d87-4ec3-8e7c-ce15c7b0aecd", harga_jual = 50000, jumlah = 10, diskon = 0 });
            listOfItemJual.Add(new ItemJualProduk { Produk = new Produk { produk_id = "7f09a4aa-e660-4de3-a3aa-4b3244675f9f" }, produk_id = "7f09a4aa-e660-4de3-a3aa-4b3244675f9f", harga_jual = 70000, jumlah = 15, diskon = 5 });

            obj.item_jual = listOfItemJual; // menghubungkan jual dan item jual

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.jual_id);
			Assert.IsNotNull(newObj);
			Assert.AreEqual(obj.jual_id, newObj.jual_id);                                
            Assert.AreEqual(obj.pengguna_id, newObj.pengguna_id);                                
            Assert.AreEqual(obj.customer_id, newObj.customer_id);                                
            Assert.AreEqual(obj.nota, newObj.nota);                                
            Assert.AreEqual(obj.tanggal, newObj.tanggal);                                
            Assert.AreEqual(obj.tanggal_tempo, newObj.tanggal_tempo);                                
            Assert.AreEqual(obj.ppn, newObj.ppn);                                
            Assert.AreEqual(obj.diskon, newObj.diskon);                                
            Assert.AreEqual(obj.total_nota, newObj.total_nota);                                
            //Assert.AreEqual(obj.total_pelunasan, newObj.total_pelunasan);                                
            Assert.AreEqual(obj.keterangan, newObj.keterangan);

            // tes hasil penyimpanan ke tabel item beli
            Assert.AreEqual(3, newObj.item_jual.Count);

            var index = 0;
            foreach (var itemJual in newObj.item_jual)
            {
                Assert.AreEqual(listOfItemJual[index].produk_id, itemJual.produk_id);
                Assert.AreEqual(listOfItemJual[index].harga_jual, itemJual.harga_jual);
                Assert.AreEqual(listOfItemJual[index].jumlah, itemJual.jumlah);
                Assert.AreEqual(listOfItemJual[index].diskon, itemJual.diskon);

                index++;
            }
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("d1dbd28a-592f-4841-bfb6-bc41f48acf32");
            obj.nota = "22222";
            obj.customer_id = "576c503f-69a7-46a5-b4be-107c634db7e3";
            obj.tanggal = new DateTime(2017, 1, 1);
            obj.tanggal_tempo = new DateTime(2017, 1, 25);
            obj.ppn = 20000;
            obj.diskon = 7500;
            obj.keterangan = "tesssss";

            foreach (var itemJual in obj.item_jual)
            {
                itemJual.jumlah = itemJual.jumlah + 1;
                itemJual.harga_jual = itemJual.harga_jual + 1000;
                itemJual.diskon = 0;
                itemJual.entity_state = EntityState.Modified;
            }

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.jual_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.jual_id, updatedObj.jual_id);
            Assert.AreEqual(obj.pengguna_id, updatedObj.pengguna_id);
            Assert.AreEqual(obj.customer_id, updatedObj.customer_id);
            Assert.AreEqual(obj.nota, updatedObj.nota);
            Assert.AreEqual(obj.tanggal, updatedObj.tanggal);
            Assert.AreEqual(obj.tanggal_tempo, updatedObj.tanggal_tempo);
            Assert.AreEqual(obj.ppn, updatedObj.ppn);
            Assert.AreEqual(obj.diskon, updatedObj.diskon);
            Assert.AreEqual(obj.total_nota, updatedObj.total_nota);
            //Assert.AreEqual(obj.total_pelunasan, updatedObj.total_pelunasan);
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);

            // tes hasil update ke tabel item beli
            Assert.AreEqual(3, updatedObj.item_jual.Count);

            var index = 0;
            foreach (var itemJualUpdated in updatedObj.item_jual)
            {
                Assert.AreEqual(obj.item_jual[index].produk_id, itemJualUpdated.produk_id);
                Assert.AreEqual(obj.item_jual[index].harga_jual, itemJualUpdated.harga_jual);
                Assert.AreEqual(obj.item_jual[index].jumlah, itemJualUpdated.jumlah);
                Assert.AreEqual(obj.item_jual[index].diskon, itemJualUpdated.diskon);

                index++;
            }          
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new JualProduk
            {
                jual_id = "94208d8c-b4c6-4e32-af37-3eec93f9ebf3"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.jual_id);
			Assert.IsNull(deletedObj);
        }
    }
}     
