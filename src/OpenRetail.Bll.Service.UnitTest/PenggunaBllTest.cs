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
    public class PenggunaBllTest
    {
		private ILog _log;
        private IPenggunaBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(PenggunaBllTest));
            _bll = new PenggunaBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void IsValidPenggunaTest()
        {
            var userName = "Kamarudin";
            var password = "20b1ce8e61ee5b49320ef0a78c75521b";

            var result = _bll.IsValidPengguna(userName, password);

            Assert.IsTrue(result);
                     
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 1;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("b597508d-1369-4a05-9d51-c666eda42063", obj.pengguna_id);
            Assert.AreEqual("11dc1faf-2c66-4525-932d-a90e24da8987", obj.role_id);
            Assert.AreEqual("Andrian", obj.nama_pengguna);
            Assert.AreEqual("20b1ce8e61ee5b49320ef0a78c75521b", obj.pass_pengguna);                                
            Assert.IsFalse(obj.is_active);

            var role = obj.Role;
            Assert.AreEqual("11dc1faf-2c66-4525-932d-a90e24da8987", role.role_id);
            Assert.AreEqual("Administrator", role.nama_role);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Pengguna
            {
                role_id = "c58ee40a-5ae2-4067-b6ad-8cae9c65913c",
                nama_pengguna = "Joko",
                pass_pengguna = "20b1ce8e61ee5b49320ef0a78c75521b",
                is_active = true
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.nama_pengguna);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.pengguna_id, newObj.pengguna_id);
            Assert.AreEqual(obj.role_id, newObj.role_id);
            Assert.AreEqual(obj.nama_pengguna, newObj.nama_pengguna);
            Assert.AreEqual(obj.pass_pengguna, newObj.pass_pengguna);
            Assert.AreEqual(obj.is_active, newObj.is_active);

        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new Pengguna
            {
                pengguna_id = "b597508d-1369-4a05-9d51-c666eda42063",
                role_id = "11dc1faf-2c66-4525-932d-a90e24da8987",
                nama_pengguna = "Andrian",
                pass_pengguna = "20b1ce8e61ee5b49320ef0a78c75521b",
                is_active = false
            };

            var result = _bll.Update(obj);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.nama_pengguna);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.pengguna_id, updatedObj.pengguna_id);
            Assert.AreEqual(obj.role_id, updatedObj.role_id);
            Assert.AreEqual(obj.nama_pengguna, updatedObj.nama_pengguna);
            Assert.AreEqual(obj.pass_pengguna, updatedObj.pass_pengguna);
            Assert.AreEqual(obj.is_active, updatedObj.is_active);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Pengguna
            {
                pengguna_id = "27ec113f-1803-48d4-867f-c5070fa10a48"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.pengguna_id);
            Assert.IsNull(deletedObj);
        }
    }
}     
