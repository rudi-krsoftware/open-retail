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
    public class PembayaranPiutangProdukBllTest
    {
		private ILog _log;
        private IPembayaranPiutangProdukBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(PembayaranPiutangProdukBllTest));
            _bll = new PembayaranPiutangProdukBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByJualIDTest()
        {
            var id = "376625eb-13ba-4620-bc12-e8260501b689";
            var obj = _bll.GetByJualID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("e547aa43-82db-466e-9701-5936ffa951cc", obj.item_pembayaran_piutang_id);
            Assert.AreEqual("376625eb-13ba-4620-bc12-e8260501b689", obj.jual_id);
            Assert.AreEqual(500000, obj.nominal);
            Assert.AreEqual("keterangan #1", obj.keterangan);
            Assert.AreEqual("tesss", obj.PembayaranPiutangProduk.keterangan);
            Assert.AreEqual("BP-12345", obj.PembayaranPiutangProduk.nota);
            Assert.IsTrue(obj.PembayaranPiutangProduk.is_tunai);

        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var index = 0;
            var tglMulai = new DateTime(2017, 1, 1);
            var tglSelesai = new DateTime(2017, 1, 19);

            var oList = _bll.GetByTanggal(tglMulai, tglSelesai);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("10479db1-699e-40d0-b6f9-1f030792ca24", obj.pembayaran_piutang_id);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("960a9111-a077-4e0e-a440-cef77293038a", obj.pengguna_id);
            Assert.AreEqual(DateTime.Today, obj.tanggal);
            Assert.AreEqual("tesss", obj.keterangan);
            Assert.AreEqual("BP-12345", obj.nota);
            Assert.IsTrue(obj.is_tunai);

            var customer = obj.Customer;
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", customer.customer_id);
            Assert.AreEqual("Swalayan Citrouli", customer.nama_customer);   
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 0;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("10479db1-699e-40d0-b6f9-1f030792ca24", obj.pembayaran_piutang_id);
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", obj.customer_id);
            Assert.AreEqual("960a9111-a077-4e0e-a440-cef77293038a", obj.pengguna_id);                                
            Assert.AreEqual(DateTime.Today, obj.tanggal);
            Assert.AreEqual("tesss", obj.keterangan);
            Assert.AreEqual("BP-12345", obj.nota);                                
            Assert.IsTrue(obj.is_tunai);

            var customer = obj.Customer;
            Assert.AreEqual("c7b1ac7f-d201-474f-b018-1dc363d5d7f3", customer.customer_id);
            Assert.AreEqual("Swalayan Citrouli", customer.nama_customer);                            
                     
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new PembayaranPiutangProduk
            {
                customer_id = "c7b1ac7f-d201-474f-b018-1dc363d5d7f3",
                pengguna_id = "960a9111-a077-4e0e-a440-cef77293038a",
                tanggal = DateTime.Today,
                keterangan = "tesss",
                nota = "BP-12345",
                is_tunai = true
            };

            var listOfItemPembayaranPiutang = new List<ItemPembayaranPiutangProduk>();
            listOfItemPembayaranPiutang.Add(new ItemPembayaranPiutangProduk { JualProduk = new JualProduk { jual_id = "376625eb-13ba-4620-bc12-e8260501b689" }, jual_id = "376625eb-13ba-4620-bc12-e8260501b689", nominal = 500000, keterangan = "keterangan #1" });
            listOfItemPembayaranPiutang.Add(new ItemPembayaranPiutangProduk { JualProduk = new JualProduk { jual_id = "e4c2c4e7-5236-44ac-98e0-b53171bc2386" }, jual_id = "e4c2c4e7-5236-44ac-98e0-b53171bc2386", nominal = 700000, keterangan = "keterangan #2" });

            obj.item_pembayaran_piutang = listOfItemPembayaranPiutang;

            var validationError = new ValidationError();

            var result = _bll.Save(obj, false, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.pembayaran_piutang_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.pembayaran_piutang_id, newObj.pembayaran_piutang_id);
            Assert.AreEqual(obj.customer_id, newObj.customer_id);
            Assert.AreEqual(obj.pengguna_id, newObj.pengguna_id);
            Assert.AreEqual(obj.tanggal, newObj.tanggal);
            Assert.AreEqual(obj.keterangan, newObj.keterangan);
            Assert.AreEqual(obj.nota, newObj.nota);
            Assert.AreEqual(obj.is_tunai, newObj.is_tunai);                                
            
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("72c0bace-02d0-4c80-91f2-10c08431347e");
            obj.tanggal = new DateTime(2017, 1, 9);
            obj.keterangan = "ket header";
            obj.nota = "BX-123456";
            obj.is_tunai = true;

            var itemPembayaran = obj.item_pembayaran_piutang[1];
            itemPembayaran.nominal = 750000;
            itemPembayaran.keterangan = "keterangan #22";
            itemPembayaran.entity_state = EntityState.Modified;

            var validationError = new ValidationError();

            var result = _bll.Update(obj, false, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.pembayaran_piutang_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.pembayaran_piutang_id, updatedObj.pembayaran_piutang_id);
            Assert.AreEqual(obj.customer_id, updatedObj.customer_id);
            Assert.AreEqual(obj.pengguna_id, updatedObj.pengguna_id);
            Assert.AreEqual(obj.tanggal, updatedObj.tanggal);
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);
            Assert.AreEqual(obj.nota, updatedObj.nota);
            Assert.AreEqual(obj.is_tunai, updatedObj.is_tunai);                                
            
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new PembayaranPiutangProduk
            {
                pembayaran_piutang_id = "72c0bace-02d0-4c80-91f2-10c08431347e"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.pembayaran_piutang_id);
			Assert.IsNull(deletedObj);
        }
    }
}     
