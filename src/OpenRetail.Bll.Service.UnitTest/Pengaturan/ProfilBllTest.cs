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
    public class ProfilBllTest
    {
		private ILog _log;
        private IProfilBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(ProfilBllTest));
            _bll = new ProfilBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetProfilTest()
        {
            var obj = _bll.GetProfil();

            Assert.IsNotNull(obj);
            Assert.AreEqual("4874663b-e365-4905-bb2c-e8a716577ade", obj.profil_id);
            Assert.AreEqual("PT. XYZ", obj.nama_profil);
            Assert.AreEqual("Ringroad Utara", obj.alamat);
            Assert.AreEqual("Yogyakarta", obj.kota);
            Assert.AreEqual("0274-123456789", obj.telepon);
                     
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Profil
            {
                nama_profil = "PT. XYZ",
                alamat = "Ringroad Utara",
                kota = "Yogyakarta",
                telepon = "0274-123456789"
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetProfil();
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.profil_id, newObj.profil_id);
            Assert.AreEqual(obj.nama_profil, newObj.nama_profil);
            Assert.AreEqual(obj.alamat, newObj.alamat);
            Assert.AreEqual(obj.kota, newObj.kota);
            Assert.AreEqual(obj.telepon, newObj.telepon);

        }
    }
}     
