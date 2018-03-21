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
    public class JabatanWebAPIBllTest
    {
        private ILog _log;
        private IJabatanBll _bll;

        [TestInitialize]
        public void Init()
        {
            var baseUrl = "http://localhost/openretail_webapi/";

            _log = LogManager.GetLogger(typeof(JabatanWebAPIBllTest));
            _bll = new JabatanBll(true, baseUrl, _log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "edb47227-da98-4d97-bff2-b7ee41ff3400";
            var obj = _bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("edb47227-da98-4d97-bff2-b7ee41ff3400", obj.jabatan_id);
            Assert.AreEqual("Owner", obj.nama_jabatan);
            Assert.AreEqual("Pemilik toko", obj.keterangan);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 2;
            var oList = _bll.GetAll();
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("edb47227-da98-4d97-bff2-b7ee41ff3400", obj.jabatan_id);
            Assert.AreEqual("Owner", obj.nama_jabatan);
            Assert.AreEqual("Pemilik toko", obj.keterangan);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Jabatan
            {
                nama_jabatan = "Gudang",
                keterangan = "Bagian Gudang"
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.jabatan_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.jabatan_id, newObj.jabatan_id);
            Assert.AreEqual(obj.nama_jabatan, newObj.nama_jabatan);
            Assert.AreEqual(obj.keterangan, newObj.keterangan);

        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new Jabatan
            {
                jabatan_id = "df864174-857d-4746-876f-ca1d59fb4df8",
                nama_jabatan = "KA Gudang",
                keterangan = "Kepala Gudang"
            };

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.jabatan_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.jabatan_id, updatedObj.jabatan_id);
            Assert.AreEqual(obj.nama_jabatan, updatedObj.nama_jabatan);
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);

        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Jabatan
            {
                jabatan_id = "df864174-857d-4746-876f-ca1d59fb4df8"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.jabatan_id);
            Assert.IsNull(deletedObj);
        }
    }
}
