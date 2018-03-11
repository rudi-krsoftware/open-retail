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
    public class PembayaranKasbonBllTest
    {
		private ILog _log;
        private IPembayaranKasbonBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(PembayaranKasbonBllTest));
            _bll = new PembayaranKasbonBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByKasbonIdTest()
        {
            var kasbonId = "d6ba5c9e-b0ba-40ba-9dc8-f631fc499aab";

            var index = 1;
            var oList = _bll.GetByKasbonId(kasbonId);
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("0b3c6fb9-7410-49a9-b248-cd32a2f6ee48", obj.pembayaran_kasbon_id);
            Assert.AreEqual("d6ba5c9e-b0ba-40ba-9dc8-f631fc499aab", obj.kasbon_id);                                
            Assert.IsNull(obj.gaji_karyawan_id);                                
            Assert.AreEqual(new DateTime(2017, 4, 17), obj.tanggal);                                
            Assert.AreEqual(75000, obj.nominal);
            Assert.AreEqual("cicilan ke #dua", obj.keterangan);
            Assert.AreEqual("2017032800021", obj.nota);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);                                
        }

        [TestMethod]
        public void GetByGajiKaryawanTest()
        {
            var gajiKaryawanId = "d6ba5c9e-b0ba-40ba-9dc8-f631fc499aab";

            var index = 1;
            var oList = _bll.GetByGajiKaryawan(gajiKaryawanId);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("0b3c6fb9-7410-49a9-b248-cd32a2f6ee48", obj.pembayaran_kasbon_id);
            Assert.AreEqual("d6ba5c9e-b0ba-40ba-9dc8-f631fc499aab", obj.kasbon_id);
            Assert.IsNull(obj.gaji_karyawan_id);
            Assert.AreEqual(new DateTime(2017, 4, 17), obj.tanggal);
            Assert.AreEqual(75000, obj.nominal);
            Assert.AreEqual("cicilan ke #dua", obj.keterangan);
            Assert.AreEqual("2017032800021", obj.nota);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new PembayaranKasbon
            {
                kasbon_id = "d6ba5c9e-b0ba-40ba-9dc8-f631fc499aab",
                gaji_karyawan_id = null,
                tanggal = new DateTime(2017, 4, 15),
                nominal = 50000,
                keterangan = "cicilan ke #3",
                nota = _bll.GetLastNota(),
                pengguna_id = "00b5acfa-b533-454b-8dfd-e7881edd180f"
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.pembayaran_kasbon_id);
			Assert.IsNotNull(newObj);
			Assert.AreEqual(obj.pembayaran_kasbon_id, newObj.pembayaran_kasbon_id);                                
            Assert.AreEqual(obj.kasbon_id, newObj.kasbon_id);                                
            Assert.AreEqual(obj.gaji_karyawan_id, newObj.gaji_karyawan_id);                                
            Assert.AreEqual(obj.tanggal, newObj.tanggal);                                
            Assert.AreEqual(obj.nominal, newObj.nominal);                                
            Assert.AreEqual(obj.keterangan, newObj.keterangan);                                
            Assert.AreEqual(obj.nota, newObj.nota);                                
            Assert.AreEqual(obj.pengguna_id, newObj.pengguna_id);                                
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("0b3c6fb9-7410-49a9-b248-cd32a2f6ee48");
            obj.tanggal = new DateTime(2017, 4, 17);
            obj.nominal = 75000;
            obj.keterangan = "cicilan ke #dua";
            obj.nota = "2017032800021";

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.pembayaran_kasbon_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.pembayaran_kasbon_id, updatedObj.pembayaran_kasbon_id);
            Assert.AreEqual(obj.kasbon_id, updatedObj.kasbon_id);
            Assert.AreEqual(obj.gaji_karyawan_id, updatedObj.gaji_karyawan_id);
            Assert.AreEqual(obj.tanggal, updatedObj.tanggal);
            Assert.AreEqual(obj.nominal, updatedObj.nominal);
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);
            Assert.AreEqual(obj.nota, updatedObj.nota);
            Assert.AreEqual(obj.pengguna_id, updatedObj.pengguna_id);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new PembayaranKasbon
            {
                pembayaran_kasbon_id = "4e646d45-b733-4961-aceb-f53be8e7b242"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.pembayaran_kasbon_id);
            Assert.IsNull(deletedObj);
        }
    }
}     
