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
    public class SettingAplikasiBllTest
    {
        private ISettingAplikasiBll _bll;

        [TestInitialize]
        public void Init()
        {
            _bll = new SettingAplikasiBll();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "1ef7cbcb-bf73-44be-aad2-b9d8e1b1a178";
            var obj = _bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("1ef7cbcb-bf73-44be-aad2-b9d8e1b1a178", obj.setting_aplikasi_id);
            Assert.IsTrue(obj.is_update_harga_jual_master_produk);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 0;
            var oList = _bll.GetAll();
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("1ef7cbcb-bf73-44be-aad2-b9d8e1b1a178", obj.setting_aplikasi_id);
            Assert.IsTrue(obj.is_update_harga_jual_master_produk);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new SettingAplikasi
            {
                setting_aplikasi_id = "1ef7cbcb-bf73-44be-aad2-b9d8e1b1a178",
                is_update_harga_jual_master_produk = true
            };

            var result = _bll.Update(obj);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.setting_aplikasi_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.setting_aplikasi_id, updatedObj.setting_aplikasi_id);
            Assert.IsTrue(obj.is_update_harga_jual_master_produk);
        }
    }
}
