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
    public class KasbonBllTest
    {
		private ILog _log;
        private IKasbonBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(KasbonBllTest));
            _bll = new KasbonBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var index = 1;
            var tglMulai = new DateTime(2017, 1, 1);
            var tglSelesai = new DateTime(2017, 4, 19);

            var oList = _bll.GetByTanggal(tglMulai, tglSelesai);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("5f913b90-6ff2-4210-878d-ed4b2717050e", obj.kasbon_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual("201703270003", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 27), obj.tanggal);
            Assert.AreEqual(10000, obj.nominal);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("tessss", obj.keterangan);

            var karyawan = obj.Karyawan;
            Assert.AreEqual("d3506b64-df74-4283-b17a-6c5dbb770e85", obj.karyawan_id);
            Assert.AreEqual("d3506b64-df74-4283-b17a-6c5dbb770e85", karyawan.karyawan_id);
            Assert.AreEqual("Doni", karyawan.nama_karyawan);
        }

        [TestMethod]
        public void GetByKaryawanIdTest()
        {
            var index = 1;
            var karyawanId = "0e0251ab-7c99-40fc-85ec-7974eeebc800";

            var oList = _bll.GetByKaryawanId(karyawanId);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("7f4d4eed-6039-478c-b23d-3e48ebc350bb", obj.kasbon_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual("201703280009", obj.nota);
            Assert.AreEqual(new DateTime(2017, 3, 29), obj.tanggal);
            Assert.AreEqual(200000, obj.nominal);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("tambahan kasbon", obj.keterangan);

            var karyawan = obj.Karyawan;
            Assert.AreEqual("0e0251ab-7c99-40fc-85ec-7974eeebc800", obj.karyawan_id);
            Assert.AreEqual("0e0251ab-7c99-40fc-85ec-7974eeebc800", karyawan.karyawan_id);
            Assert.AreEqual("Andi", karyawan.nama_karyawan);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 1;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("5f913b90-6ff2-4210-878d-ed4b2717050e", obj.kasbon_id);
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual("201703270003", obj.nota);                                
            Assert.AreEqual(new DateTime(2017, 3, 27), obj.tanggal);                                
            Assert.AreEqual(10000, obj.nominal);
            Assert.AreEqual(0, obj.total_pelunasan);
            Assert.AreEqual("tessss", obj.keterangan);
            
            var karyawan = obj.Karyawan;
            Assert.AreEqual("d3506b64-df74-4283-b17a-6c5dbb770e85", obj.karyawan_id);
            Assert.AreEqual("d3506b64-df74-4283-b17a-6c5dbb770e85", karyawan.karyawan_id);
            Assert.AreEqual("Doni", karyawan.nama_karyawan);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Kasbon
            {
                karyawan_id = "d3506b64-df74-4283-b17a-6c5dbb770e85",
                pengguna_id = "00b5acfa-b533-454b-8dfd-e7881edd180f",
                nota = _bll.GetLastNota(),
                tanggal = DateTime.Today,
                nominal = 10000,
                keterangan = "tessss"
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.kasbon_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.kasbon_id, newObj.kasbon_id);
            Assert.AreEqual(obj.karyawan_id, newObj.karyawan_id);
            Assert.AreEqual(obj.nota, newObj.nota);
            Assert.AreEqual(obj.tanggal, newObj.tanggal);
            Assert.AreEqual(obj.nominal, newObj.nominal);
            Assert.AreEqual(obj.keterangan, newObj.keterangan);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("28b3f9e3-0504-4b7e-b9d5-a8984a2d0c81");
            obj.karyawan_id = "72f28a4f-f364-423a-a09b-2b9571543fde";
            obj.nota = "12345";
            obj.tanggal = new DateTime(2017, 3, 20);
            obj.nominal = 250000;
            obj.keterangan = "tess lagi";

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.kasbon_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.kasbon_id, updatedObj.kasbon_id);
            Assert.AreEqual(obj.karyawan_id, updatedObj.karyawan_id);
            Assert.AreEqual(obj.pengguna_id, updatedObj.pengguna_id);
            Assert.AreEqual(obj.nota, updatedObj.nota);
            Assert.AreEqual(obj.tanggal, updatedObj.tanggal);
            Assert.AreEqual(obj.nominal, updatedObj.nominal);
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Kasbon
            {
                kasbon_id = "034f800f-5dd6-4173-82cf-88f3879aedb9"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.kasbon_id);
            Assert.IsNull(deletedObj);
        }
    }
}     
