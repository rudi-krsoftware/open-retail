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
    public class JabatanBllTest
    {
        private IJabatanBll bll = null;

        [TestInitialize]
        public void Init()
        {
            bll = new JabatanBll();
        }

        [TestCleanup]
        public void CleanUp()
        {
            bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "edb47227-da98-4d97-bff2-b7ee41ff3400";
            var obj = bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("edb47227-da98-4d97-bff2-b7ee41ff3400", obj.jabatan_id);
            Assert.AreEqual("Owner", obj.nama_jabatan);
            Assert.AreEqual("Pemilik toko", obj.keterangan);
                     
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 2;
            var oList = bll.GetAll();
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

            var result = bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = bll.GetByID(obj.jabatan_id);
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
                jabatan_id = "a60c6afb-ea16-467d-98eb-d724c33fc578",
                nama_jabatan = "KA Gudang",
                keterangan = "Kepala Gudang"
        	};

            var validationError = new ValidationError();

            var result = bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = bll.GetByID(obj.jabatan_id);
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
                jabatan_id = "a60c6afb-ea16-467d-98eb-d724c33fc578"
            };

            var result = bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = bll.GetByID(obj.jabatan_id);
			Assert.IsNull(deletedObj);
        }
    }
}     
