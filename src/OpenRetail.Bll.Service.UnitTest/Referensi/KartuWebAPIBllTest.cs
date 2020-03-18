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
    public class KartuWebAPIBllTest
    {
        private ILog _log;
        private IKartuBll _bll;

        [TestInitialize]
        public void Init()
        {
            var baseUrl = "http://localhost/openretail_webapi/";

            _log = LogManager.GetLogger(typeof(KartuWebAPIBllTest));
            _bll = new KartuBll(true, baseUrl, _log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 1;
            var oList = _bll.GetAll();
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("3f6c7750-e461-4c48-b2ae-738a50f83b50", obj.kartu_id);
            Assert.AreEqual("Visa", obj.nama_kartu);
            Assert.IsFalse(obj.is_debit);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Kartu
            {
                nama_kartu = "Debit",
                is_debit = true
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.kartu_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.kartu_id, newObj.kartu_id);
            Assert.AreEqual(obj.nama_kartu, newObj.nama_kartu);
            Assert.AreEqual(obj.is_debit, newObj.is_debit);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("3f6c7750-e461-4c48-b2ae-738a50f83b50");
            obj.nama_kartu = "Visa";
            obj.is_debit = false;

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.kartu_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.kartu_id, updatedObj.kartu_id);
            Assert.AreEqual(obj.nama_kartu, updatedObj.nama_kartu);
            Assert.AreEqual(obj.is_debit, updatedObj.is_debit);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Kartu
            {
                kartu_id = "cdac4989-433c-4287-8d05-25f3594bd09c"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.kartu_id);
            Assert.IsNull(deletedObj);
        }
    }
}