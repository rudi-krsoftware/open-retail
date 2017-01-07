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
    public class SupplierBllTest
    {
        private ISupplierBll bll = null;

        [TestInitialize]
        public void Init()
        {
            bll = new SupplierBll();
        }

        [TestCleanup]
        public void CleanUp()
        {
            bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "7859f705-6b0f-4c06-9bd0-e9f5f7b87414";
            var obj = bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("7859f705-6b0f-4c06-9bd0-e9f5f7b87414", obj.supplier_id);
            Assert.AreEqual("Sigma Computer", obj.nama_supplier);
            Assert.AreEqual("Yogyakarta", obj.alamat);
            Assert.AreEqual("Andi", obj.kontak);
            Assert.AreEqual("", obj.telepon);
            Assert.AreEqual(1000000, obj.total_hutang);
            Assert.AreEqual(600000, obj.total_pembayaran_hutang);
                     
        }

        [TestMethod]
        public void GetByNameTest()
        {
            var name = "computer";

            var index = 1;
            var oList = bll.GetByName(name);
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("7859f705-6b0f-4c06-9bd0-e9f5f7b87414", obj.supplier_id);
            Assert.AreEqual("Sigma Computer", obj.nama_supplier);
            Assert.AreEqual("Yogyakarta", obj.alamat);
            Assert.AreEqual("Andi", obj.kontak);
            Assert.AreEqual("", obj.telepon);
            Assert.AreEqual(1000000, obj.total_hutang);
            Assert.AreEqual(600000, obj.total_pembayaran_hutang);                                
                     
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 1;
            var oList = bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("7859f705-6b0f-4c06-9bd0-e9f5f7b87414", obj.supplier_id);
            Assert.AreEqual("Sigma Computer", obj.nama_supplier);
            Assert.AreEqual("Yogyakarta", obj.alamat);
            Assert.AreEqual("Andi", obj.kontak);
            Assert.AreEqual("", obj.telepon);
            Assert.AreEqual(1000000, obj.total_hutang);
            Assert.AreEqual(600000, obj.total_pembayaran_hutang);                                
                     
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Supplier
            {
                nama_supplier = "Pixel Computer",
                alamat = "Solo",
                kontak = "Yusuf",
                telepon = "08138383838"
            };

            var validationError = new ValidationError();

            var result = bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = bll.GetByID(obj.supplier_id);
			Assert.IsNotNull(newObj);
			Assert.AreEqual(obj.supplier_id, newObj.supplier_id);                                
            Assert.AreEqual(obj.nama_supplier, newObj.nama_supplier);                                
            Assert.AreEqual(obj.alamat, newObj.alamat);                                
            Assert.AreEqual(obj.kontak, newObj.kontak);                                
            Assert.AreEqual(obj.telepon, newObj.telepon);                                
            
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new Supplier
            {
                supplier_id = "7859f705-6b0f-4c06-9bd0-e9f5f7b87414",
                nama_supplier = "Sigma Computer",
                alamat = "Yogyakarta",
                kontak = "Andi",
                telepon = ""
        	};

            var validationError = new ValidationError();

            var result = bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = bll.GetByID(obj.supplier_id);
			Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.supplier_id, updatedObj.supplier_id);                                
            Assert.AreEqual(obj.nama_supplier, updatedObj.nama_supplier);                                
            Assert.AreEqual(obj.alamat, updatedObj.alamat);                                
            Assert.AreEqual(obj.kontak, updatedObj.kontak);                                
            Assert.AreEqual(obj.telepon, updatedObj.telepon);                                
            
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Supplier
            {
                supplier_id = "7859f705-6b0f-4c06-9bd0-e9f5f7b87414"
            };

            var result = bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = bll.GetByID(obj.supplier_id);
			Assert.IsNull(deletedObj);
        }
    }
}     
