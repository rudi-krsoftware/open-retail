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
    public class KabupatenBllTest
    {
		private ILog _log;
        private IKabupatenBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(KabupatenBllTest));
            _bll = new KabupatenBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByNameTest()
        {
            var name = "aceh";

            var index = 4;
            var oList = _bll.GetByName(name);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual(5, obj.kabupaten_id);
            Assert.AreEqual("Kabupaten", obj.tipe);
            Assert.AreEqual("Aceh Selatan", obj.nama_kabupaten);
            Assert.AreEqual("23719", obj.kode_pos);

            var provinsi = obj.Provinsi;
            Assert.AreEqual(21, obj.provinsi_id);
            Assert.AreEqual(21, provinsi.provinsi_id);
            Assert.AreEqual("Nanggroe Aceh Darussalam (NAD)", provinsi.nama_provinsi);                                
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 4;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual(5, obj.kabupaten_id);            
            Assert.AreEqual("Kabupaten", obj.tipe);
            Assert.AreEqual("Aceh Selatan", obj.nama_kabupaten);
            Assert.AreEqual("23719", obj.kode_pos);

            var provinsi = obj.Provinsi;
            Assert.AreEqual(21, obj.provinsi_id);
            Assert.AreEqual(21, provinsi.provinsi_id);
            Assert.AreEqual("Nanggroe Aceh Darussalam (NAD)", provinsi.nama_provinsi);                                
        }
    }
}     
