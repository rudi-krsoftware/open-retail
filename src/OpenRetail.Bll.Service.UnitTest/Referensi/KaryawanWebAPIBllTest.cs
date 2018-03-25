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
    public class KaryawanWebAPIBllTest
    {
        private ILog _log;
        private IKaryawanBll _bll;

        [TestInitialize]
        public void Init()
        {
            var baseUrl = "http://localhost/openretail_webapi/";

            _log = LogManager.GetLogger(typeof(KaryawanWebAPIBllTest));
            _bll = new KaryawanBll(true, baseUrl, _log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "72f28a4f-f364-423a-a09b-2b9571543fde";
            var obj = _bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("72f28a4f-f364-423a-a09b-2b9571543fde", obj.karyawan_id);
            Assert.AreEqual("f1e4ea09-b777-4e56-bb90-db2bf9211468", obj.jabatan_id);
            Assert.AreEqual("Bangkit", obj.nama_karyawan);
            Assert.AreEqual("Klaten", obj.alamat);
            Assert.AreEqual("0813283838383", obj.telepon);
            Assert.AreEqual(200000, obj.gaji_pokok);
            Assert.IsTrue(obj.is_active);
            Assert.AreEqual(JenisGajian.Mingguan, obj.jenis_gajian);
            Assert.AreEqual(60000, obj.gaji_lembur);
            Assert.AreEqual(200000, obj.total_kasbon);
            Assert.AreEqual(50000, obj.total_pembayaran_kasbon);
        }

        [TestMethod]
        public void GetByNameTest()
        {
            var name = "bang";

            var index = 0;
            var oList = _bll.GetByName(name);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("72f28a4f-f364-423a-a09b-2b9571543fde", obj.karyawan_id);
            Assert.AreEqual("f1e4ea09-b777-4e56-bb90-db2bf9211468", obj.jabatan_id);
            Assert.AreEqual("Bangkit", obj.nama_karyawan);
            Assert.AreEqual("Klaten", obj.alamat);
            Assert.AreEqual("0813283838383", obj.telepon);
            Assert.AreEqual(200000, obj.gaji_pokok);
            Assert.IsTrue(obj.is_active);
            Assert.AreEqual(JenisGajian.Mingguan, obj.jenis_gajian);
            Assert.AreEqual(60000, obj.gaji_lembur);
            Assert.AreEqual(200000, obj.total_kasbon);
            Assert.AreEqual(50000, obj.total_pembayaran_kasbon);

        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 2;
            var oList = _bll.GetAll();
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("72f28a4f-f364-423a-a09b-2b9571543fde", obj.karyawan_id);
            Assert.AreEqual("f1e4ea09-b777-4e56-bb90-db2bf9211468", obj.jabatan_id);
            Assert.AreEqual("Bangkit", obj.nama_karyawan);
            Assert.AreEqual("Klaten", obj.alamat);
            Assert.AreEqual("0813283838383", obj.telepon);
            Assert.AreEqual(200000, obj.gaji_pokok);
            Assert.IsTrue(obj.is_active);
            Assert.AreEqual(JenisGajian.Mingguan, obj.jenis_gajian);
            Assert.AreEqual(60000, obj.gaji_lembur);
            Assert.AreEqual(200000, obj.total_kasbon);
            Assert.AreEqual(50000, obj.total_pembayaran_kasbon);

        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new Karyawan
            {
                jabatan_id = "120d3472-ea93-4e29-8abd-5bd7044d26db",
                nama_karyawan = "Doni",
                alamat = "Yogyakarta",
                telepon = "",
                gaji_pokok = 100000,
                is_active = true,
                jenis_gajian = JenisGajian.Mingguan,
                gaji_lembur = 50000
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.karyawan_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.karyawan_id, newObj.karyawan_id);
            Assert.AreEqual(obj.jabatan_id, newObj.jabatan_id);
            Assert.AreEqual(obj.nama_karyawan, newObj.nama_karyawan);
            Assert.AreEqual(obj.alamat, newObj.alamat);
            Assert.AreEqual(obj.telepon, newObj.telepon);
            Assert.AreEqual(obj.gaji_pokok, newObj.gaji_pokok);
            Assert.AreEqual(obj.is_active, newObj.is_active);
            Assert.AreEqual(obj.keterangan, newObj.keterangan);
            Assert.AreEqual(obj.jenis_gajian, newObj.jenis_gajian);
            Assert.AreEqual(obj.gaji_lembur, newObj.gaji_lembur);

        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new Karyawan
            {
                karyawan_id = "72f28a4f-f364-423a-a09b-2b9571543fde",
                jabatan_id = "f1e4ea09-b777-4e56-bb90-db2bf9211468",
                nama_karyawan = "Bangkit",
                alamat = "Klaten",
                telepon = "0813283838383",
                gaji_pokok = 200000,
                is_active = true,
                jenis_gajian = JenisGajian.Mingguan,
                gaji_lembur = 60000
            };

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.karyawan_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.karyawan_id, updatedObj.karyawan_id);
            Assert.AreEqual(obj.jabatan_id, updatedObj.jabatan_id);
            Assert.AreEqual(obj.nama_karyawan, updatedObj.nama_karyawan);
            Assert.AreEqual(obj.alamat, updatedObj.alamat);
            Assert.AreEqual(obj.telepon, updatedObj.telepon);
            Assert.AreEqual(obj.gaji_pokok, updatedObj.gaji_pokok);
            Assert.AreEqual(obj.is_active, updatedObj.is_active);
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);
            Assert.AreEqual(obj.jenis_gajian, updatedObj.jenis_gajian);
            Assert.AreEqual(obj.gaji_lembur, updatedObj.gaji_lembur);
            Assert.AreEqual(obj.total_kasbon, updatedObj.total_kasbon);
            Assert.AreEqual(obj.total_pembayaran_kasbon, updatedObj.total_pembayaran_kasbon);

        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Karyawan
            {
                karyawan_id = "8b8c9988-e868-40d5-9972-42655cd28618"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.karyawan_id);
            Assert.IsNull(deletedObj);
        }
    }
}
