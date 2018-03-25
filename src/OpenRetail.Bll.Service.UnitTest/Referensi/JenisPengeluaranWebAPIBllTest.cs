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
    public class JenisPengeluaranWebAPIBllTest
    {
        private ILog _log;
        private IJenisPengeluaranBll _bll;

        [TestInitialize]
        public void Init()
        {
            var baseUrl = "http://localhost/openretail_webapi/";

            _log = LogManager.GetLogger(typeof(JenisPengeluaranWebAPIBllTest));
            _bll = new JenisPengeluaranBll(true, baseUrl, _log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "1619f95e-eac3-40e9-ba8a-af2230d3c470";
            var obj = _bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("1619f95e-eac3-40e9-ba8a-af2230d3c470", obj.jenis_pengeluaran_id);
            Assert.AreEqual("Biaya Lain Lain Penjualan", obj.nama_jenis_pengeluaran);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 4;
            var oList = _bll.GetAll();
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("1619f95e-eac3-40e9-ba8a-af2230d3c470", obj.jenis_pengeluaran_id);
            Assert.AreEqual("Biaya Lain Lain Penjualan", obj.nama_jenis_pengeluaran);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new JenisPengeluaran
            {
                nama_jenis_pengeluaran = "Bayar Listrik Baru"
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.jenis_pengeluaran_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.jenis_pengeluaran_id, newObj.jenis_pengeluaran_id);
            Assert.AreEqual(obj.nama_jenis_pengeluaran, newObj.nama_jenis_pengeluaran);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new JenisPengeluaran
            {
                jenis_pengeluaran_id = "26b062ad-7469-42c4-9326-bb84feeca746",
                nama_jenis_pengeluaran = "Biaya Listrik Naik"
            };

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.jenis_pengeluaran_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.jenis_pengeluaran_id, updatedObj.jenis_pengeluaran_id);
            Assert.AreEqual(obj.nama_jenis_pengeluaran, updatedObj.nama_jenis_pengeluaran);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new JenisPengeluaran
            {
                jenis_pengeluaran_id = "f896b0f7-9675-4dc2-bf03-06e1dc6977d1"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.jenis_pengeluaran_id);
            Assert.IsNull(deletedObj);
        }
    }
}
