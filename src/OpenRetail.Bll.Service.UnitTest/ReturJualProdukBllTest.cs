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
    public class ReturJualProdukBllTest
    {
		private ILog _log;
        private IReturJualProdukBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ReturJualProdukBllTest));
            _bll = new ReturJualProdukBll(_log);
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
            Assert.AreEqual("201701260006", lastNota);

            lastNota = _bll.GetLastNota();
            Assert.AreEqual("201701260007", lastNota);
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
            Assert.AreEqual("2fb82570-d64e-4f2d-bd03-aab5bdf75884", obj.retur_jual_id);
            Assert.AreEqual("27d40236-c8ab-44be-bc47-7a9bbd68c31e", obj.jual_id);
            Assert.AreEqual("af01c916-7976-4518-a563-9d2a1851a912", obj.customer_id);
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
            Assert.AreEqual(3500, itemRetur.harga_jual);

            // tes customer
            var customer = obj.Customer;
            Assert.AreEqual("af01c916-7976-4518-a563-9d2a1851a912", customer.customer_id);
            Assert.AreEqual("TE Shop", customer.nama_customer);
            Assert.AreEqual("Yogyakarta", customer.alamat);

            // tes jual
            var jual = obj.JualProduk;
            Assert.AreEqual("27d40236-c8ab-44be-bc47-7a9bbd68c31e", jual.jual_id);
            Assert.AreEqual("201701200045", jual.nota);
        }

        [TestMethod]
        public void GetAllTest()
        {            
            var index = 0;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("", obj.retur_jual_id);                                
            Assert.AreEqual("", obj.jual_id);                                
            Assert.AreEqual("", obj.pengguna_id);                                
            Assert.AreEqual("", obj.customer_id);                                
            Assert.AreEqual("", obj.nota);                                
            Assert.AreEqual("", obj.tanggal);                                
            Assert.AreEqual("", obj.keterangan);                                
            Assert.AreEqual("", obj.tanggal_sistem);                                
            Assert.AreEqual("", obj.total_nota);                                
                     
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new ReturJualProduk
            {
                jual_id = "376625eb-13ba-4620-bc12-e8260501b689",
                customer_id = "c7b1ac7f-d201-474f-b018-1dc363d5d7f3",
                nota = _bll.GetLastNota(),
                tanggal = DateTime.Today,
                keterangan = "keterangan header"
            };

            var listOfItemRetur = new List<ItemReturJualProduk>();
            listOfItemRetur.Add(new ItemReturJualProduk { item_jual_id = "3db2b20c-2e31-4934-b04a-a77f7ff85419", Produk = new Produk { produk_id = "eafc755f-cab6-4066-a793-660fcfab20d0" }, produk_id = "eafc755f-cab6-4066-a793-660fcfab20d0", harga_jual = 53000, jumlah = 5, jumlah_retur = 2 });
            listOfItemRetur.Add(new ItemReturJualProduk { item_jual_id = "7ea1f32f-b47f-4945-a7ed-3e6da34f5108", Produk = new Produk { produk_id = "6e587b32-9d87-4ec3-8e7c-ce15c7b0aecd" }, produk_id = "6e587b32-9d87-4ec3-8e7c-ce15c7b0aecd", harga_jual = 50000, jumlah = 10, jumlah_retur = 5 });

            obj.item_retur = listOfItemRetur; // menghubungkan retur dan item retur

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.retur_jual_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.retur_jual_id, newObj.retur_jual_id);
            Assert.AreEqual(obj.jual_id, newObj.jual_id);
            Assert.AreEqual(obj.pengguna_id, newObj.pengguna_id);
            Assert.AreEqual(obj.customer_id, newObj.customer_id);
            Assert.AreEqual(obj.nota, newObj.nota);
            Assert.AreEqual(obj.tanggal, newObj.tanggal);
            Assert.AreEqual(obj.keterangan, newObj.keterangan);
            Assert.AreEqual(obj.total_nota, newObj.total_nota); 

        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("a1597295-c11d-4ea2-b074-e8c6369bf028");
            obj.nota = "201701260011";
            obj.keterangan = "keterangan header";

            foreach (var itemRetur in obj.item_retur)
            {
                itemRetur.jumlah_retur += 1;
                itemRetur.harga_jual += 1000;
                itemRetur.entity_state = EntityState.Modified;
            }

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.retur_jual_id);
            Assert.IsNotNull(updatedObj);

            Assert.AreEqual(obj.retur_jual_id, updatedObj.retur_jual_id);
            Assert.AreEqual(obj.jual_id, updatedObj.jual_id);
            Assert.AreEqual(obj.pengguna_id, updatedObj.pengguna_id);
            Assert.AreEqual(obj.customer_id, updatedObj.customer_id);
            Assert.AreEqual(obj.nota, updatedObj.nota);
            Assert.AreEqual(obj.tanggal, updatedObj.tanggal);
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);
            Assert.AreEqual(obj.total_nota, updatedObj.total_nota);

        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new ReturJualProduk
            {
                retur_jual_id = "a1597295-c11d-4ea2-b074-e8c6369bf028"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.retur_jual_id);
            Assert.IsNull(deletedObj);
        }
    }
}     
