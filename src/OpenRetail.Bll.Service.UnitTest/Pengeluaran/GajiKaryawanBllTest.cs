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
    public class GajiKaryawanBllTest
    {
		private ILog _log;
        private IGajiKaryawanBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(GajiKaryawanBllTest));
            _bll = new GajiKaryawanBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetLastNotaTest()
        {
            var lastNota = _bll.GetLastNota();
            Assert.AreEqual("201703310001", lastNota);

            lastNota = _bll.GetLastNota();
            Assert.AreEqual("201703310002", lastNota);
        }

        [TestMethod]
        public void GetByBulanAndTahunTest()
        {
            var bulan = 3;
            var tahun = 2017;

            var index = 1;
            var oList = _bll.GetByBulanAndTahun(bulan, tahun);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("229d712c-a1c5-45e4-be20-2c07bff86406", obj.gaji_karyawan_id);            
            Assert.AreEqual("00b5acfa-b533-454b-8dfd-e7881edd180f", obj.pengguna_id);
            Assert.AreEqual(3, obj.bulan);
            Assert.AreEqual(2017, obj.tahun);
            Assert.AreEqual(20, obj.kehadiran);
            Assert.AreEqual(5, obj.absen);
            Assert.AreEqual(1500000, obj.gaji_pokok);
            Assert.AreEqual(1000, obj.lembur);
            Assert.AreEqual(150000, obj.bonus);
            Assert.AreEqual(50000, obj.potongan);
            Assert.AreEqual(1, obj.jam);
            Assert.AreEqual(0, obj.lainnya);
            Assert.AreEqual("tesss lagi", obj.keterangan);
            Assert.AreEqual(6, obj.jumlah_hari);
            Assert.AreEqual(0, obj.tunjangan);
            Assert.AreEqual(new DateTime(2017, 3, 31), obj.tanggal);
            Assert.AreEqual("201703310004", obj.nota);

            // tes cek data karyawan
            var karyawan = obj.Karyawan;
            Assert.AreEqual("d3506b64-df74-4283-b17a-6c5dbb770e85", obj.karyawan_id);
            Assert.AreEqual("d3506b64-df74-4283-b17a-6c5dbb770e85", karyawan.karyawan_id);
            Assert.AreEqual("Doni", karyawan.nama_karyawan);

            // tes cek data jabatan
            var jabatan = karyawan.Jabatan;
            Assert.AreEqual("120d3472-ea93-4e29-8abd-5bd7044d26db", jabatan.jabatan_id);
            Assert.AreEqual("Kasir", jabatan.nama_jabatan);
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new GajiKaryawan
            {
                karyawan_id = "72f28a4f-f364-423a-a09b-2b9571543fde",
                pengguna_id = "00b5acfa-b533-454b-8dfd-e7881edd180f",
                tanggal = DateTime.Today,
                nota = _bll.GetLastNota(),
                bulan = 3,
                tahun = 2017,
                kehadiran = 24,
                absen = 1,
                gaji_pokok = 3000000,
                lembur = 0,
                bonus = 0,
                potongan = 0,
                jam = 1,
                keterangan = "tesss",
                jumlah_hari = 6,
                tunjangan = 0             
            };

            // item pembayaran kasbon
            var listOfPembayaranKasbon = new List<PembayaranKasbon>();

            var pembayaranKasbon1 = new PembayaranKasbon
            {
                kasbon_id = "d6ba5c9e-b0ba-40ba-9dc8-f631fc499aab",
                Kasbon = new Kasbon { kasbon_id = "d6ba5c9e-b0ba-40ba-9dc8-f631fc499aab" },
                nominal = 600000,
                keterangan = "Pembayaran dari gaji"
            };

            var pembayaranKasbon2 = new PembayaranKasbon
            {
                kasbon_id = "89a3fbb2-441c-4043-b858-755e112cd997",
                Kasbon = new Kasbon { kasbon_id = "89a3fbb2-441c-4043-b858-755e112cd997" },
                nominal = 100000,
                keterangan = "Pembayaran dari gaji"
            };

            listOfPembayaranKasbon.Add(pembayaranKasbon1);
            listOfPembayaranKasbon.Add(pembayaranKasbon2);
            obj.item_pembayaran_kasbon = listOfPembayaranKasbon;

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.gaji_karyawan_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.gaji_karyawan_id, newObj.gaji_karyawan_id);
            Assert.AreEqual(obj.karyawan_id, newObj.karyawan_id);
            Assert.AreEqual(obj.pengguna_id, newObj.pengguna_id);
            Assert.AreEqual(obj.tanggal, newObj.tanggal);
            Assert.AreEqual(obj.nota, newObj.nota);
            Assert.AreEqual(obj.bulan, newObj.bulan);
            Assert.AreEqual(obj.tahun, newObj.tahun);
            Assert.AreEqual(obj.kehadiran, newObj.kehadiran);
            Assert.AreEqual(obj.absen, newObj.absen);
            Assert.AreEqual(obj.gaji_pokok, newObj.gaji_pokok);
            Assert.AreEqual(obj.lembur, newObj.lembur);
            Assert.AreEqual(obj.bonus, newObj.bonus);
            Assert.AreEqual(obj.potongan, newObj.potongan);
            Assert.AreEqual(obj.jam, newObj.jam);
            Assert.AreEqual(obj.lainnya, newObj.lainnya);
            Assert.AreEqual(obj.keterangan, newObj.keterangan);
            Assert.AreEqual(obj.jumlah_hari, newObj.jumlah_hari);
            Assert.AreEqual(obj.tunjangan, newObj.tunjangan);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("229d712c-a1c5-45e4-be20-2c07bff86406");
            obj.karyawan_id = "d3506b64-df74-4283-b17a-6c5dbb770e85";
            obj.kehadiran = 20;
            obj.absen = 5;
            obj.gaji_pokok = 1500000;
            obj.lembur = 1000;
            obj.bonus = 150000;
            obj.potongan = 50000;
            obj.keterangan = "tesss lagi";

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.gaji_karyawan_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.gaji_karyawan_id, updatedObj.gaji_karyawan_id);
            Assert.AreEqual(obj.karyawan_id, updatedObj.karyawan_id);
            Assert.AreEqual(obj.pengguna_id, updatedObj.pengguna_id);
            Assert.AreEqual(obj.tanggal, updatedObj.tanggal);
            Assert.AreEqual(obj.nota, updatedObj.nota);
            Assert.AreEqual(obj.bulan, updatedObj.bulan);
            Assert.AreEqual(obj.tahun, updatedObj.tahun);
            Assert.AreEqual(obj.kehadiran, updatedObj.kehadiran);
            Assert.AreEqual(obj.absen, updatedObj.absen);
            Assert.AreEqual(obj.gaji_pokok, updatedObj.gaji_pokok);
            Assert.AreEqual(obj.lembur, updatedObj.lembur);
            Assert.AreEqual(obj.bonus, updatedObj.bonus);
            Assert.AreEqual(obj.potongan, updatedObj.potongan);
            Assert.AreEqual(obj.jam, updatedObj.jam);
            Assert.AreEqual(obj.lainnya, updatedObj.lainnya);
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);
            Assert.AreEqual(obj.jumlah_hari, updatedObj.jumlah_hari);
            Assert.AreEqual(obj.tunjangan, updatedObj.tunjangan);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new GajiKaryawan
            {
                gaji_karyawan_id = "a86d7da1-e0a0-4f4e-896c-5d6d7be8be8f"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.gaji_karyawan_id);
            Assert.IsNull(deletedObj);
        }
    }
}     
