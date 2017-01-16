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
    public class MenuBllTest
    {
		private ILog _log;
        private IMenuBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(MenuBllTest));
            _bll = new MenuBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "5df78447-219a-47c8-8a28-53b8e71ffb9d";
            var obj = _bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("5df78447-219a-47c8-8a28-53b8e71ffb9d", obj.menu_id);
            Assert.AreEqual("mnuTrxPembelianBahanBaku", obj.nama_menu);
            Assert.AreEqual("Pembelian Bahan Baku", obj.judul_menu);
            Assert.AreEqual("cc69c800-5e36-4dc5-9242-4191f1373983", obj.parent_id);
            Assert.AreEqual(1, obj.order_number);
            Assert.IsTrue(obj.is_active);
            Assert.AreEqual("FrmListTransaksiPembelianBahanBaku", obj.nama_form);
            Assert.IsTrue(obj.is_enabled);
                     
        }

        [TestMethod]
        public void GetByNameTest()
        {
            var menuName = "mnuTrxPembelianBahanBaku";
            
            var obj = _bll.GetByName(menuName);
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("5df78447-219a-47c8-8a28-53b8e71ffb9d", obj.menu_id);
            Assert.AreEqual("mnuTrxPembelianBahanBaku", obj.nama_menu);
            Assert.AreEqual("Pembelian Bahan Baku", obj.judul_menu);
            Assert.AreEqual("cc69c800-5e36-4dc5-9242-4191f1373983", obj.parent_id);
            Assert.AreEqual(1, obj.order_number);
            Assert.IsTrue(obj.is_active);
            Assert.AreEqual("FrmListTransaksiPembelianBahanBaku", obj.nama_form);
            Assert.IsTrue(obj.is_enabled);                              
                     
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 2;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("5df78447-219a-47c8-8a28-53b8e71ffb9d", obj.menu_id);
            Assert.AreEqual("mnuTrxPembelianBahanBaku", obj.nama_menu);
            Assert.AreEqual("Pembelian Bahan Baku", obj.judul_menu);
            Assert.AreEqual("cc69c800-5e36-4dc5-9242-4191f1373983", obj.parent_id);
            Assert.AreEqual(1, obj.order_number);
            Assert.IsTrue(obj.is_active);
            Assert.AreEqual("FrmListTransaksiPembelianBahanBaku", obj.nama_form);
            Assert.IsTrue(obj.is_enabled);                               
                     
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new MenuAplikasi
            {
                nama_menu = "mnuRefGolongan",
                judul_menu = "Golongan",
                parent_id = "5df78447-219a-47c8-8a28-53b8e71ffb9d",
                order_number = 1,
                is_active = true,
                nama_form = "FrmGolongan",
                is_enabled = true
            };

            var result = _bll.Save(obj);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.menu_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.menu_id, newObj.menu_id);
            Assert.AreEqual(obj.nama_menu, newObj.nama_menu);
            Assert.AreEqual(obj.judul_menu, newObj.judul_menu);
            Assert.AreEqual(obj.parent_id, newObj.parent_id);
            Assert.AreEqual(obj.order_number, newObj.order_number);
            Assert.AreEqual(obj.is_active, newObj.is_active);
            Assert.AreEqual(obj.nama_form, newObj.nama_form);
            Assert.AreEqual(obj.is_enabled, newObj.is_enabled);                                
            
		}
    }
}     
