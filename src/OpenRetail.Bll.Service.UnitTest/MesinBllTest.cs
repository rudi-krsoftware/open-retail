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
    public class MesinBllTest
    {
		private ILog _log;
        private IMesinBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(MesinBllTest));
            _bll = new MesinBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var penggunaId = "00b5acfa-b533-454b-8dfd-e7881edd180f";
            var tanggal = DateTime.Today;

            var index = 1;
            var oList = _bll.GetByTanggal(penggunaId, tanggal);
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("ebfdae76-2577-4070-aaac-16fffc09d6f5", obj.mesin_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);                                
            Assert.AreEqual(DateTime.Today, obj.tanggal);                                
            Assert.AreEqual(300000, obj.saldo_awal);                                
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Mesin
            {
                pengguna_id = "00b5acfa-b533-454b-8dfd-e7881edd180f",
                tanggal = DateTime.Today,
                saldo_awal = 200000
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.mesin_id);
			Assert.IsNotNull(newObj);
			Assert.AreEqual(obj.mesin_id, newObj.mesin_id);                                
            Assert.AreEqual(obj.pengguna_id, newObj.pengguna_id);                                
            Assert.AreEqual(obj.tanggal, newObj.tanggal);                                
            Assert.AreEqual(obj.saldo_awal, newObj.saldo_awal);                                
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("ebfdae76-2577-4070-aaac-16fffc09d6f5");
            obj.saldo_awal = 300000;
            obj.uang_keluar = 100000;

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.mesin_id);
			Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.mesin_id, updatedObj.mesin_id);                                
            Assert.AreEqual(obj.pengguna_id, updatedObj.pengguna_id);                                
            Assert.AreEqual(obj.tanggal, updatedObj.tanggal);                                
            Assert.AreEqual(obj.saldo_awal, updatedObj.saldo_awal);                                
            Assert.AreEqual(obj.uang_masuk, updatedObj.uang_masuk);                                
            Assert.AreEqual(obj.tanggal_sistem, updatedObj.tanggal_sistem);                                
            Assert.AreEqual(obj.shift_id, updatedObj.shift_id);                                
            Assert.AreEqual(obj.uang_keluar, updatedObj.uang_keluar);                                
            
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Mesin
            {
                mesin_id = "6870dec2-3f4b-4952-9174-d6d40f254573"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.mesin_id);
			Assert.IsNull(deletedObj);
        }
    }
}     
