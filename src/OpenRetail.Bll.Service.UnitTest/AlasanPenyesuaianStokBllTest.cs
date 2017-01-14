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
    public class AlasanPenyesuaianStokBllTest
    {
        private ILog _log;
        private IAlasanPenyesuaianStokBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(AlasanPenyesuaianStokBllTest));
            _bll = new AlasanPenyesuaianStokBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "e4ef2a27-6600-365f-1e07-2963d55cc4bf";
            var obj = _bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("e4ef2a27-6600-365f-1e07-2963d55cc4bf", obj.alasan_penyesuaian_stok_id);
            Assert.AreEqual("Koreksi (Koreksi karena kesalahan input)", obj.alasan);
                     
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 1;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("e4ef2a27-6600-365f-1e07-2963d55cc4bf", obj.alasan_penyesuaian_stok_id);
            Assert.AreEqual("Koreksi (Koreksi karena kesalahan input)", obj.alasan);                               
                     
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new AlasanPenyesuaianStok
            {
                alasan = "Dipake sendiri"
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.alasan_penyesuaian_stok_id);
			Assert.IsNotNull(newObj);
			Assert.AreEqual(obj.alasan_penyesuaian_stok_id, newObj.alasan_penyesuaian_stok_id);                                
            Assert.AreEqual(obj.alasan, newObj.alasan);                                
            
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new AlasanPenyesuaianStok
            {
                alasan_penyesuaian_stok_id = "ab6b9e7d-f0c2-4b49-b257-cf518f7af145",
                alasan = "Dipake sendiri (Prive)"
        	};

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.alasan_penyesuaian_stok_id);
			Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.alasan_penyesuaian_stok_id, updatedObj.alasan_penyesuaian_stok_id);                                
            Assert.AreEqual(obj.alasan, updatedObj.alasan);                                
            
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new AlasanPenyesuaianStok
            {
                alasan_penyesuaian_stok_id = "ab6b9e7d-f0c2-4b49-b257-cf518f7af145"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.alasan_penyesuaian_stok_id);
			Assert.IsNull(deletedObj);
        }
    }
}     
