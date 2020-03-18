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

using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenRetail.Bll.Api;
using OpenRetail.Model;
using System;

namespace OpenRetail.Bll.Service.UnitTest
{
    [TestClass]
    public class DropshipperWebAPIBllTest
    {
        private ILog _log;
        private IDropshipperBll _bll;

        [TestInitialize]
        public void Init()
        {
            var baseUrl = "http://localhost/openretail_webapi/";

            _log = LogManager.GetLogger(typeof(DropshipperBllTest));
            _bll = new DropshipperBll(true, baseUrl, _log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByNameTest()
        {
            var name = "maypes";

            var index = 0;
            var oList = _bll.GetByName(name);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("23649ae7-3435-4a5e-81d6-f7697a5b3775", obj.dropshipper_id);
            Assert.AreEqual("Madelena Maypes", obj.nama_dropshipper);
            Assert.AreEqual("2 Nancy Place", obj.alamat);
            Assert.AreEqual("Maypes", obj.kontak);
            Assert.AreEqual("566-561-8188", obj.telepon);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 1;
            var oList = _bll.GetAll();
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("23649ae7-3435-4a5e-81d6-f7697a5b3775", obj.dropshipper_id);
            Assert.AreEqual("Madelena Maypes", obj.nama_dropshipper);
            Assert.AreEqual("2 Nancy Place", obj.alamat);
            Assert.AreEqual("Maypes", obj.kontak);
            Assert.AreEqual("566-561-8188", obj.telepon);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Dropshipper
            {
                nama_dropshipper = "Lacie Kaesmans",
                alamat = "110 Mosinee Hill",
                kontak = "Lacie",
                telepon = "212-367-3754"
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.dropshipper_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.dropshipper_id, newObj.dropshipper_id);
            Assert.AreEqual(obj.nama_dropshipper, newObj.nama_dropshipper);
            Assert.AreEqual(obj.alamat, newObj.alamat);
            Assert.AreEqual(obj.kontak, newObj.kontak);
            Assert.AreEqual(obj.telepon, newObj.telepon);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("23649ae7-3435-4a5e-81d6-f7697a5b3775");
            obj.nama_dropshipper = "Madelena Maypes";
            obj.alamat = "2 Nancy Place";
            obj.kontak = "Maypes";
            obj.telepon = "566-561-8188";

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.dropshipper_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.dropshipper_id, updatedObj.dropshipper_id);
            Assert.AreEqual(obj.nama_dropshipper, updatedObj.nama_dropshipper);
            Assert.AreEqual(obj.alamat, updatedObj.alamat);
            Assert.AreEqual(obj.kontak, updatedObj.kontak);
            Assert.AreEqual(obj.telepon, updatedObj.telepon);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Dropshipper
            {
                dropshipper_id = "810efc21-9809-4730-b18b-065e7e1b9368"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.dropshipper_id);
            Assert.IsNull(deletedObj);
        }
    }
}