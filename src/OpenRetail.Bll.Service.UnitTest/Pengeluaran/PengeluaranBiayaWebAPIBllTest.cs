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
    public class PengeluaranBiayaWebAPIBllTest
    {
        private ILog _log;
        private IPengeluaranBiayaBll _bll;

        [TestInitialize]
        public void Init()
        {
            var baseUrl = "http://localhost/openretail_webapi/";

            _log = LogManager.GetLogger(typeof(PengeluaranBiayaWebAPIBllTest));
            _bll = new PengeluaranBiayaBll(true, baseUrl, _log);
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
            Assert.AreEqual("201706120033", lastNota);

            lastNota = _bll.GetLastNota();
            Assert.AreEqual("201706120034", lastNota);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 1;
            var oList = _bll.GetAll();
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("bfa85912-b32f-4846-bc8a-747811f5350a", obj.pengeluaran_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual("201703270019", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 27), obj.tanggal);
            Assert.AreEqual(2210000, obj.total);
            Assert.AreEqual("", obj.keterangan);

            // tes detail pengeluaran
            index = 1;
            Assert.AreEqual(2, obj.item_pengeluaran_biaya.Count);

            var itemPengeluaran = obj.item_pengeluaran_biaya[index];
            Assert.AreEqual("3b926134-93e7-4e28-aa14-6d601f7b66db", itemPengeluaran.item_pengeluaran_id);
            Assert.AreEqual("c2116c49-a940-4385-be94-302470b67b83", itemPengeluaran.JenisPengeluaran.jenis_pengeluaran_id);
            Assert.AreEqual("Biaya Penyusutan Kendaraan", itemPengeluaran.JenisPengeluaran.nama_jenis_pengeluaran);

            Assert.AreEqual(2000000, itemPengeluaran.harga);
            Assert.AreEqual(1, itemPengeluaran.jumlah);
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var index = 1;
            var tglMulai = new DateTime(2017, 1, 1);
            var tglSelesai = new DateTime(2017, 3, 27);

            var oList = _bll.GetByTanggal(tglMulai, tglSelesai);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("bfa85912-b32f-4846-bc8a-747811f5350a", obj.pengeluaran_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual("201703270019", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 27), obj.tanggal);
            Assert.AreEqual(2210000, obj.total);
            Assert.AreEqual("", obj.keterangan);

            // tes detail pengeluaran
            index = 1;
            Assert.AreEqual(2, obj.item_pengeluaran_biaya.Count);

            var itemPengeluaran = obj.item_pengeluaran_biaya[index];
            Assert.AreEqual("3b926134-93e7-4e28-aa14-6d601f7b66db", itemPengeluaran.item_pengeluaran_id);
            Assert.AreEqual("c2116c49-a940-4385-be94-302470b67b83", itemPengeluaran.JenisPengeluaran.jenis_pengeluaran_id);
            Assert.AreEqual("Biaya Penyusutan Kendaraan", itemPengeluaran.JenisPengeluaran.nama_jenis_pengeluaran);

            Assert.AreEqual(2000000, itemPengeluaran.harga);
            Assert.AreEqual(1, itemPengeluaran.jumlah);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new PengeluaranBiaya
            {
                pengguna_id = "00b5acfa-b533-454b-8dfd-e7881edd180f",
                nota = _bll.GetLastNota(),
                tanggal = DateTime.Today,
                keterangan = "tes keterangan"
            };

            var listOfItemPengeluaran = new List<ItemPengeluaranBiaya>();
            listOfItemPengeluaran.Add(new ItemPengeluaranBiaya { JenisPengeluaran = new JenisPengeluaran { jenis_pengeluaran_id = "6c262064-6453-4bea-9e0f-5ae1810d0557" }, jenis_pengeluaran_id = "6c262064-6453-4bea-9e0f-5ae1810d0557", pengguna_id = obj.pengguna_id, harga = 50000, jumlah = 5 });
            listOfItemPengeluaran.Add(new ItemPengeluaranBiaya { JenisPengeluaran = new JenisPengeluaran { jenis_pengeluaran_id = "c2116c49-a940-4385-be94-302470b67b83" }, jenis_pengeluaran_id = "c2116c49-a940-4385-be94-302470b67b83", pengguna_id = obj.pengguna_id, harga = 25000, jumlah = 10 });
            listOfItemPengeluaran.Add(new ItemPengeluaranBiaya { JenisPengeluaran = new JenisPengeluaran { jenis_pengeluaran_id = "2cc2ae56-dc3b-4991-af56-7768ae10816a" }, jenis_pengeluaran_id = "2cc2ae56-dc3b-4991-af56-7768ae10816a", pengguna_id = obj.pengguna_id, harga = 30000, jumlah = 15 });

            obj.item_pengeluaran_biaya = listOfItemPengeluaran; // menghubungkan jual dan item jual

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.pengeluaran_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.pengeluaran_id, newObj.pengeluaran_id);
            Assert.AreEqual(obj.pengguna_id, newObj.pengguna_id);
            Assert.AreEqual(obj.nota, newObj.nota);
            Assert.AreEqual(obj.tanggal, newObj.tanggal);
            Assert.AreEqual(obj.total, newObj.total);
            Assert.AreEqual(obj.keterangan, newObj.keterangan);

            var index = 0;
            foreach (var itemPengeluaran in newObj.item_pengeluaran_biaya)
            {
                Assert.AreEqual(listOfItemPengeluaran[index].jenis_pengeluaran_id, itemPengeluaran.jenis_pengeluaran_id);
                Assert.AreEqual(listOfItemPengeluaran[index].harga, itemPengeluaran.harga);
                Assert.AreEqual(listOfItemPengeluaran[index].jumlah, itemPengeluaran.jumlah);

                index++;
            }
        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("0a6ca3a2-bfdc-4707-99b4-5f10326d8c75");
            obj.nota = "11111";
            obj.tanggal = new DateTime(2017, 3, 27);
            obj.keterangan = "keterangan";

            foreach (var itemPengeluaran in obj.item_pengeluaran_biaya)
            {
                itemPengeluaran.jumlah = itemPengeluaran.jumlah + 2;
                itemPengeluaran.harga = itemPengeluaran.harga + 1500;
                itemPengeluaran.entity_state = EntityState.Modified;
            }

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.pengeluaran_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.pengeluaran_id, updatedObj.pengeluaran_id);
            Assert.AreEqual(obj.pengguna_id, updatedObj.pengguna_id);
            Assert.AreEqual(obj.nota, updatedObj.nota);
            Assert.AreEqual(obj.tanggal, updatedObj.tanggal);
            Assert.AreEqual(obj.total, updatedObj.total);
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);
            Assert.AreEqual(obj.tanggal_sistem, updatedObj.tanggal_sistem);

            var index = 0;
            foreach (var itemJualUpdated in updatedObj.item_pengeluaran_biaya)
            {
                Assert.AreEqual(obj.item_pengeluaran_biaya[index].harga, itemJualUpdated.harga);
                Assert.AreEqual(obj.item_pengeluaran_biaya[index].jumlah, itemJualUpdated.jumlah);

                index++;
            }
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new PengeluaranBiaya
            {
                pengeluaran_id = "5b7fb72b-5cea-407f-8654-986de89e1cf9"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.pengeluaran_id);
            Assert.IsNull(deletedObj);
        }
    }
}
