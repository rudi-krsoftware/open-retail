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
    public class CustomerWebAPIBllTest
    {
        private ILog _log;
        private ICustomerBll _bll;

        [TestInitialize]
        public void Init()
        {
            var baseUrl = "http://localhost/openretail_webapi/";

            _log = LogManager.GetLogger(typeof(CustomerWebAPIBllTest));
            _bll = new CustomerBll(true, baseUrl, _log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "a16c67a0-bb93-459c-8765-27cac61e4e83";
            var obj = _bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("a16c67a0-bb93-459c-8765-27cac61e4e83", obj.customer_id);
            Assert.AreEqual("Callista Distribution JKT", obj.nama_customer);
            Assert.AreEqual("Harco Mangga Dua Elektronik Blok F no.10", obj.alamat);
            Assert.AreEqual("", obj.kontak);
            Assert.AreEqual("0813 8176 9915", obj.telepon);
            Assert.AreEqual(0, obj.plafon_piutang);
            Assert.AreEqual(0, obj.total_piutang);
            Assert.AreEqual(0, obj.total_pembayaran_piutang);
            Assert.AreEqual("Sleman", obj.kecamatan);
            Assert.AreEqual("Condong Catur", obj.kelurahan);
            Assert.AreEqual("Yogyakarta", obj.kota);
            Assert.AreEqual("28115", obj.kode_pos);
            Assert.AreEqual(0, obj.diskon);

        }

        [TestMethod]
        public void GetByNameTest()
        {
            var name = "jkt";

            var index = 0;
            var oList = _bll.GetByName(name);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("a16c67a0-bb93-459c-8765-27cac61e4e83", obj.customer_id);
            Assert.AreEqual("Callista Distribution JKT", obj.nama_customer);
            Assert.AreEqual("Harco Mangga Dua Elektronik Blok F no.10", obj.alamat);
            Assert.AreEqual("", obj.kontak);
            Assert.AreEqual("0813 8176 9915", obj.telepon);
            Assert.AreEqual(0, obj.plafon_piutang);
            Assert.AreEqual(0, obj.total_piutang);
            Assert.AreEqual(0, obj.total_pembayaran_piutang);
            Assert.AreEqual("Sleman", obj.kecamatan);
            Assert.AreEqual("Condong Catur", obj.kelurahan);
            Assert.AreEqual("Yogyakarta", obj.kota);
            Assert.AreEqual("28115", obj.kode_pos);
            Assert.AreEqual(0, obj.diskon);

        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 2;
            var oList = _bll.GetAll();
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("a16c67a0-bb93-459c-8765-27cac61e4e83", obj.customer_id);
            Assert.AreEqual("Callista Distribution JKT", obj.nama_customer);
            Assert.AreEqual("Harco Mangga Dua Elektronik Blok F no.10", obj.alamat);
            Assert.AreEqual("", obj.kontak);
            Assert.AreEqual("0813 8176 9915", obj.telepon);
            Assert.AreEqual(0, obj.plafon_piutang);
            Assert.AreEqual(0, obj.total_piutang);
            Assert.AreEqual(0, obj.total_pembayaran_piutang);
            Assert.AreEqual("Sleman", obj.kecamatan);
            Assert.AreEqual("Condong Catur", obj.kelurahan);
            Assert.AreEqual("Yogyakarta", obj.kota);
            Assert.AreEqual("28115", obj.kode_pos);
            Assert.AreEqual(0, obj.diskon);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Customer
            {
                nama_customer = "Swalayan WS",
                alamat = "Jl. Magelang",
                kontak = "",
                telepon = "08138383838"
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.customer_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.customer_id, newObj.customer_id);
            Assert.AreEqual(obj.nama_customer, newObj.nama_customer);
            Assert.AreEqual(obj.alamat, newObj.alamat);
            Assert.AreEqual(obj.kontak, newObj.kontak);
            Assert.AreEqual(obj.telepon, newObj.telepon);

        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new Customer
            {
                customer_id = "34881f91-33c6-48a8-a19b-75b01ed36ff2",
                nama_customer = "Swalayan WS Medari",
                alamat = "Jl. Magelang",
                kontak = "Mas Adi",
                telepon = "0274-4444433"
            };

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.customer_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.customer_id, updatedObj.customer_id);
            Assert.AreEqual(obj.nama_customer, updatedObj.nama_customer);
            Assert.AreEqual(obj.alamat, updatedObj.alamat);
            Assert.AreEqual(obj.kontak, updatedObj.kontak);
            Assert.AreEqual(obj.telepon, updatedObj.telepon);

        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Customer
            {
                customer_id = "34881f91-33c6-48a8-a19b-75b01ed36ff2"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.customer_id);
            Assert.IsNull(deletedObj);
        }
    }
}
