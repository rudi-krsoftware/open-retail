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
    public class RolePrivilegeBllTest
    {
		private ILog _log;
        private IRolePrivilegeBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(RolePrivilegeBllTest));
            _bll = new RolePrivilegeBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByRoleTest()
        {
            var roleId = "11dc1faf-2c66-4525-932d-a90e24da8987";

            var index = 4;
            var oList = _bll.GetByRole(roleId);
            Assert.AreEqual(197, oList.Count);     

            var obj = oList[index];
            Assert.IsNotNull(obj);
            Assert.AreEqual("11dc1faf-2c66-4525-932d-a90e24da8987", obj.role_id);
            Assert.AreEqual("5df78447-219a-47c8-8a28-53b8e71ffb9d", obj.menu_id);                                
            Assert.AreEqual(1, obj.grant_id);                                
            Assert.IsTrue(obj.is_grant);                                
                     
        }

        [TestMethod]
        public void GetByRoleAndMenuTest()
        {
            var roleId = "11dc1faf-2c66-4525-932d-a90e24da8987";
            var menuId = "5df78447-219a-47c8-8a28-53b8e71ffb9d";

            var index = 3;
            var oList = _bll.GetByRoleAndMenu(roleId, menuId);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("11dc1faf-2c66-4525-932d-a90e24da8987", obj.role_id);
            Assert.AreEqual("5df78447-219a-47c8-8a28-53b8e71ffb9d", obj.menu_id);
            Assert.AreEqual(0, obj.grant_id);
            Assert.IsFalse(obj.is_grant);

        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 3;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("11dc1faf-2c66-4525-932d-a90e24da8987", obj.role_id);
            Assert.AreEqual("a138abfd-73da-438e-a0fe-aa3e6c6ddce9", obj.menu_id);                                
            Assert.AreEqual(3, obj.grant_id);                                
            Assert.IsTrue(obj.is_grant);                                
                     
        }

        [TestMethod]
        public void SaveNewTest()
        {
            var obj = new RolePrivilege
            {
                role_id = "11dc1faf-2c66-4525-932d-a90e24da8987",
                menu_id = "a138abfd-73da-438e-a0fe-aa3e6c6ddce9",
                grant_id = 0,
                is_grant = true
            };

            var result = _bll.Save(obj);
            Assert.IsTrue(result != 0);
            
		}

        [TestMethod]
        public void SaveUpdateTest()
        {
            var obj = new RolePrivilege
            {
                role_id = "11dc1faf-2c66-4525-932d-a90e24da8987",
                menu_id = "a138abfd-73da-438e-a0fe-aa3e6c6ddce9",
                grant_id = 0,
                is_grant = false
            };

            var result = _bll.Save(obj);
            Assert.IsTrue(result != 0);

        }
    }
}     
