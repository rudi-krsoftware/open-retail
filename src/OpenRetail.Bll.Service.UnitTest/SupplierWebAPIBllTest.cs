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
    public class SupplierWebAPIBllTest
    {
        private ILog _log;
        private ISupplierBll _bll;

        [TestInitialize]
        public void Init()
        {
            var baseUrl = "http://localhost/openretail_webapi/";

            _log = LogManager.GetLogger(typeof(SupplierWebAPIBllTest));
            _bll = new SupplierBll(true, baseUrl, _log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "cc793c38-f5d0-408e-bb1a-1b98953340b6";
            var obj = _bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("cc793c38-f5d0-408e-bb1a-1b98953340b6", obj.supplier_id);
            Assert.AreEqual("Aneka Komputer", obj.nama_supplier);
            Assert.AreEqual("Solo", obj.alamat);
            Assert.AreEqual("", obj.kontak);
            Assert.AreEqual("", obj.telepon);
            Assert.AreEqual(470000, obj.total_hutang);
            Assert.AreEqual(0, obj.total_pembayaran_hutang);
        }

        [TestMethod]
        public void GetByNameTest()
        {
            var name = "kom";

            var index = 0;
            var oList = _bll.GetByName(name);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("cc793c38-f5d0-408e-bb1a-1b98953340b6", obj.supplier_id);
            Assert.AreEqual("Aneka Komputer", obj.nama_supplier);
            Assert.AreEqual("Solo", obj.alamat);
            Assert.AreEqual("", obj.kontak);
            Assert.AreEqual("", obj.telepon);
            Assert.AreEqual(470000, obj.total_hutang);
            Assert.AreEqual(0, obj.total_pembayaran_hutang);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 1;
            var oList = _bll.GetAll();
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("cc793c38-f5d0-408e-bb1a-1b98953340b6", obj.supplier_id);
            Assert.AreEqual("Aneka Komputer", obj.nama_supplier);
            Assert.AreEqual("Solo", obj.alamat);
            Assert.AreEqual("", obj.kontak);
            Assert.AreEqual("", obj.telepon);
            Assert.AreEqual(470000, obj.total_hutang);
            Assert.AreEqual(0, obj.total_pembayaran_hutang);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Supplier
            {
                nama_supplier = "Pixel Computer",
                alamat = "Yogyakarta",
                kontak = "Rudi",
                telepon = "08138383838"
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.supplier_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.supplier_id, newObj.supplier_id);
            Assert.AreEqual(obj.nama_supplier, newObj.nama_supplier);
            Assert.AreEqual(obj.alamat, newObj.alamat);
            Assert.AreEqual(obj.kontak, newObj.kontak);
            Assert.AreEqual(obj.telepon, newObj.telepon);

        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new Supplier
            {
                supplier_id = "5b3704f5-83a4-4b98-a7a7-1b09fd62d356",
                nama_supplier = "Sigma Computer",
                alamat = "Sleman - Yogyakarta",
                kontak = "Andi",
                telepon = ""
            };

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.supplier_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.supplier_id, updatedObj.supplier_id);
            Assert.AreEqual(obj.nama_supplier, updatedObj.nama_supplier);
            Assert.AreEqual(obj.alamat, updatedObj.alamat);
            Assert.AreEqual(obj.kontak, updatedObj.kontak);
            Assert.AreEqual(obj.telepon, updatedObj.telepon);

        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Supplier
            {
                supplier_id = "5b3704f5-83a4-4b98-a7a7-1b09fd62d356"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.supplier_id);
            Assert.IsNull(deletedObj);
        }
    }
}
