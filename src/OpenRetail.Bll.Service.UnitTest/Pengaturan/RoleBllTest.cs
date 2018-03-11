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
    public class RoleBllTest
    {
		private ILog _log;
        private IRoleBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(RoleBllTest));
            _bll = new RoleBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "c58ee40a-5ae2-4067-b6ad-8cae9c65913c";
            var obj = _bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("c58ee40a-5ae2-4067-b6ad-8cae9c65913c", obj.role_id);
            Assert.AreEqual("Owner", obj.nama_role);
            Assert.IsTrue(obj.is_active);
                     
        }

        [TestMethod]
        public void GetByStatusTest()
        {
            var index = 1;
            var oList = _bll.GetByStatus(true);
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("c58ee40a-5ae2-4067-b6ad-8cae9c65913c", obj.role_id);
            Assert.AreEqual("Owner", obj.nama_role);
            Assert.IsTrue(obj.is_active);                            
                     
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 2;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("c58ee40a-5ae2-4067-b6ad-8cae9c65913c", obj.role_id);
            Assert.AreEqual("Owner", obj.nama_role);
            Assert.IsTrue(obj.is_active);                           
                     
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Role
            {
                nama_role = "Supervisor",
                is_active = true
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.role_id);
			Assert.IsNotNull(newObj);
			Assert.AreEqual(obj.role_id, newObj.role_id);                                
            Assert.AreEqual(obj.nama_role, newObj.nama_role);                                
            Assert.AreEqual(obj.is_active, newObj.is_active);                                
            
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new Role
            {
                role_id = "42d0a7b9-2b1a-4ad7-b6ad-b7c8b65ef04a",
                nama_role = "Kasir",
                is_active = true
        	};

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.role_id);
			Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.role_id, updatedObj.role_id);                                
            Assert.AreEqual(obj.nama_role, updatedObj.nama_role);                                
            Assert.AreEqual(obj.is_active, updatedObj.is_active);                                
            
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Role
            {
                role_id = "ca001999-f31a-4746-aea3-3de628e9bfdd"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.role_id);
			Assert.IsNull(deletedObj);
        }
    }
}     
