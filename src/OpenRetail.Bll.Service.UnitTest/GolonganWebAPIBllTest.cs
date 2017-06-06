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
    public class GolonganWebAPIBllTest
    {
        private ILog _log;
        private IGolonganBll _bll;

        [TestInitialize]
        public void Init()
        {
            var baseUrl = "http://localhost/openretail_webapi/";

            _log = LogManager.GetLogger(typeof(GolonganWebAPIBllTest));
            _bll = new GolonganBll(true, baseUrl, _log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "0a8b59e5-bb3b-4081-b963-9dc9584dcda6";
            var obj = _bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("0a8b59e5-bb3b-4081-b963-9dc9584dcda6", obj.golongan_id);
            Assert.AreEqual("Accessories", obj.nama_golongan);
            Assert.AreEqual(1.5, obj.diskon);
        }

        [TestMethod]
        public void GetByNameTest()
        {
            var name = "la";

            var index = 1;
            var oList = _bll.GetByName(name);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("fd9d730e-ed74-4041-9b17-6dd4433e6bc5", obj.golongan_id);
            Assert.AreEqual("Lainnya", obj.nama_golongan);
            Assert.AreEqual(1, obj.diskon);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 4;
            var oList = _bll.GetAll();
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("fd9d730e-ed74-4041-9b17-6dd4433e6bc5", obj.golongan_id);
            Assert.AreEqual("Lainnya", obj.nama_golongan);
            Assert.AreEqual(1, obj.diskon);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Golongan
            {
                nama_golongan = "Golongan baru",
                diskon = 100
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.golongan_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.golongan_id, newObj.golongan_id);
            Assert.AreEqual(obj.nama_golongan, newObj.nama_golongan);
            Assert.AreEqual(obj.diskon, newObj.diskon);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("a6371499-a314-4918-bc8f-70a0a14372d6");
            obj.nama_golongan = "Golongan Terbaru";
            obj.diskon = 2.5;

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.golongan_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.golongan_id, updatedObj.golongan_id);
            Assert.AreEqual(obj.nama_golongan, updatedObj.nama_golongan);
            Assert.AreEqual(obj.diskon, updatedObj.diskon);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Golongan
            {
                golongan_id = "a6371499-a314-4918-bc8f-70a0a14372d6"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.golongan_id);
            Assert.IsNull(deletedObj);
        }
    }
}
