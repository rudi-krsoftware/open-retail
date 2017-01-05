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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
 
namespace OpenRetail.Bll.Service.UnitTest
{    
    [TestClass]
    public class GolonganBllTest
    {
        private IGolonganBll bll = null;

        [TestInitialize]
        public void Init()
        {
            bll = new GolonganBll();
        }

        [TestCleanup]
        public void CleanUp()
        {
            bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "6ae85958-80c6-4f3a-bc01-53a715e25bf1";
            var obj = bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("6ae85958-80c6-4f3a-bc01-53a715e25bf1", obj.golongan_id);
            Assert.AreEqual("Hardware new", obj.nama_golongan);
                     
        }

        [TestMethod]
        public void GetByNameTest()
        {
            var name = "hard";

            var index = 1;
            var oList = bll.GetByName(name);
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("6ae85958-80c6-4f3a-bc01-53a715e25bf1", obj.golongan_id);
            Assert.AreEqual("Hardware new", obj.nama_golongan);                                               
                     
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 2;
            var oList = bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("6ae85958-80c6-4f3a-bc01-53a715e25bf1", obj.golongan_id);
            Assert.AreEqual("Hardware new", obj.nama_golongan);                    
                     
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Golongan
            {
                nama_golongan = "Lainnya"
            };

            var validationError = new ValidationError();

            var result = bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = bll.GetByID(obj.golongan_id);
			Assert.IsNotNull(newObj);
			Assert.AreEqual(obj.golongan_id, newObj.golongan_id);                                
            Assert.AreEqual(obj.nama_golongan, newObj.nama_golongan);                                
            
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new Golongan
            {
                golongan_id = "4758c3ec-e931-40d5-a903-75b7dc48bee1",
                nama_golongan = "Lain-lain"
        	};

            var validationError = new ValidationError();

            var result = bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = bll.GetByID(obj.golongan_id);
			Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.golongan_id, updatedObj.golongan_id);                                
            Assert.AreEqual(obj.nama_golongan, updatedObj.nama_golongan);                                
            
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Golongan
            {
                golongan_id = "4758c3ec-e931-40d5-a903-75b7dc48bee1"
            };

            var result = bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = bll.GetByID(obj.golongan_id);
			Assert.IsNull(deletedObj);
        }
    }
}     
