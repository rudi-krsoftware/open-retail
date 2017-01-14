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
    public class PenyesuaianStokBllTest
    {
		private ILog _log;
        private IPenyesuaianStokBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(PenyesuaianStokBllTest));
            _bll = new PenyesuaianStokBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "561f8b6e-9c6a-47d4-b4cd-e1c2cf3552ac";
            var obj = _bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("561f8b6e-9c6a-47d4-b4cd-e1c2cf3552ac", obj.penyesuaian_stok_id);
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("f9b35798-6725-244f-fec0-fdee38c5ad44", obj.alasan_penyesuaian_id);
            Assert.AreEqual(new DateTime(2017, 1, 14), obj.tanggal);
            Assert.AreEqual(1, obj.penambahan_stok);
            Assert.AreEqual(2, obj.pengurangan_stok);            
            Assert.AreEqual(3, obj.penambahan_stok_gudang);
            Assert.AreEqual(4, obj.pengurangan_stok_gudang);
            Assert.AreEqual("tesss", obj.keterangan);

            var produk = obj.Produk;
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", produk.produk_id);
            Assert.AreEqual("201607000000053", produk.kode_produk);
            Assert.AreEqual("Adaptor NB ACER", produk.nama_produk);

            var alasanPenyesuaian = obj.AlasanPenyesuaianStok;
            Assert.AreEqual("f9b35798-6725-244f-fec0-fdee38c5ad44", alasanPenyesuaian.alasan_penyesuaian_stok_id);
            Assert.AreEqual("Pindah stok gudang ke etalase", alasanPenyesuaian.alasan);
        }

        [TestMethod]
        public void GetByNameTest()
        {
            var name = "adaptor";

            var index = 0;
            var oList = _bll.GetByName(name);
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("561f8b6e-9c6a-47d4-b4cd-e1c2cf3552ac", obj.penyesuaian_stok_id);
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("f9b35798-6725-244f-fec0-fdee38c5ad44", obj.alasan_penyesuaian_id);
            Assert.AreEqual(new DateTime(2017, 1, 14), obj.tanggal);
            Assert.AreEqual(1, obj.penambahan_stok);
            Assert.AreEqual(2, obj.pengurangan_stok);
            Assert.AreEqual(3, obj.penambahan_stok_gudang);
            Assert.AreEqual(4, obj.pengurangan_stok_gudang);
            Assert.AreEqual("tesss", obj.keterangan);

            var produk = obj.Produk;
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", produk.produk_id);
            Assert.AreEqual("201607000000053", produk.kode_produk);
            Assert.AreEqual("Adaptor NB ACER", produk.nama_produk);

            var alasanPenyesuaian = obj.AlasanPenyesuaianStok;
            Assert.AreEqual("f9b35798-6725-244f-fec0-fdee38c5ad44", alasanPenyesuaian.alasan_penyesuaian_stok_id);
            Assert.AreEqual("Pindah stok gudang ke etalase", alasanPenyesuaian.alasan);                              
                     
        }

        [TestMethod]
        public void GetByTanggalTest()
        {
            var tanggalMulai = new DateTime(2017, 1, 1);
            var tanggalSelesai = new DateTime(2017, 1, 15);

            var index = 1;
            var oList = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("561f8b6e-9c6a-47d4-b4cd-e1c2cf3552ac", obj.penyesuaian_stok_id);
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("f9b35798-6725-244f-fec0-fdee38c5ad44", obj.alasan_penyesuaian_id);
            Assert.AreEqual(new DateTime(2017, 1, 14), obj.tanggal);
            Assert.AreEqual(1, obj.penambahan_stok);
            Assert.AreEqual(2, obj.pengurangan_stok);
            Assert.AreEqual(3, obj.penambahan_stok_gudang);
            Assert.AreEqual(4, obj.pengurangan_stok_gudang);
            Assert.AreEqual("tesss", obj.keterangan);

            var produk = obj.Produk;
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", produk.produk_id);
            Assert.AreEqual("201607000000053", produk.kode_produk);
            Assert.AreEqual("Adaptor NB ACER", produk.nama_produk);

            var alasanPenyesuaian = obj.AlasanPenyesuaianStok;
            Assert.AreEqual("f9b35798-6725-244f-fec0-fdee38c5ad44", alasanPenyesuaian.alasan_penyesuaian_stok_id);
            Assert.AreEqual("Pindah stok gudang ke etalase", alasanPenyesuaian.alasan);

        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 1;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("561f8b6e-9c6a-47d4-b4cd-e1c2cf3552ac", obj.penyesuaian_stok_id);
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", obj.produk_id);
            Assert.AreEqual("f9b35798-6725-244f-fec0-fdee38c5ad44", obj.alasan_penyesuaian_id);
            Assert.AreEqual(new DateTime(2017, 1, 14), obj.tanggal);
            Assert.AreEqual(1, obj.penambahan_stok);
            Assert.AreEqual(2, obj.pengurangan_stok);
            Assert.AreEqual(3, obj.penambahan_stok_gudang);
            Assert.AreEqual(4, obj.pengurangan_stok_gudang);
            Assert.AreEqual("tesss", obj.keterangan);

            var produk = obj.Produk;
            Assert.AreEqual("eafc755f-cab6-4066-a793-660fcfab20d0", produk.produk_id);
            Assert.AreEqual("201607000000053", produk.kode_produk);
            Assert.AreEqual("Adaptor NB ACER", produk.nama_produk);

            var alasanPenyesuaian = obj.AlasanPenyesuaianStok;
            Assert.AreEqual("f9b35798-6725-244f-fec0-fdee38c5ad44", alasanPenyesuaian.alasan_penyesuaian_stok_id);
            Assert.AreEqual("Pindah stok gudang ke etalase", alasanPenyesuaian.alasan);                               
                     
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new PenyesuaianStok
            {
                produk_id = "eafc755f-cab6-4066-a793-660fcfab20d0",
                alasan_penyesuaian_id = "f9b35798-6725-244f-fec0-fdee38c5ad44",
                tanggal = DateTime.Today,
                penambahan_stok = 1,
                pengurangan_stok = 2,
                keterangan = "tesss",
                penambahan_stok_gudang = 3,
                pengurangan_stok_gudang = 4
            };

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.penyesuaian_stok_id);
			Assert.IsNotNull(newObj);
			Assert.AreEqual(obj.penyesuaian_stok_id, newObj.penyesuaian_stok_id);                                
            Assert.AreEqual(obj.produk_id, newObj.produk_id);                                
            Assert.AreEqual(obj.alasan_penyesuaian_id, newObj.alasan_penyesuaian_id);                                
            Assert.AreEqual(obj.tanggal, newObj.tanggal);                                
            Assert.AreEqual(obj.penambahan_stok, newObj.penambahan_stok);                                
            Assert.AreEqual(obj.pengurangan_stok, newObj.pengurangan_stok);                                
            Assert.AreEqual(obj.keterangan, newObj.keterangan);                                
            Assert.AreEqual(obj.penambahan_stok_gudang, newObj.penambahan_stok_gudang);                                
            Assert.AreEqual(obj.pengurangan_stok_gudang, newObj.pengurangan_stok_gudang);                                
            
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new PenyesuaianStok
            {
                penyesuaian_stok_id = "6daeacbb-85a0-4b60-8ead-a77984448110",
                produk_id = "53b63dc2-4505-4276-9886-3639b53b7458",
                alasan_penyesuaian_id = "1c23364b-e65d-62ef-4180-b2f3f7f560c1",
                tanggal = new DateTime(2017, 1, 10),
                penambahan_stok = 5,
                pengurangan_stok = 4,
                keterangan = "tess keterangan",
                penambahan_stok_gudang = 1,
                pengurangan_stok_gudang = 5
        	};

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.penyesuaian_stok_id);
			Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.penyesuaian_stok_id, updatedObj.penyesuaian_stok_id);                                
            Assert.AreEqual(obj.produk_id, updatedObj.produk_id);                                
            Assert.AreEqual(obj.alasan_penyesuaian_id, updatedObj.alasan_penyesuaian_id);                                
            Assert.AreEqual(obj.tanggal, updatedObj.tanggal);                                
            Assert.AreEqual(obj.penambahan_stok, updatedObj.penambahan_stok);                                
            Assert.AreEqual(obj.pengurangan_stok, updatedObj.pengurangan_stok);                                
            Assert.AreEqual(obj.keterangan, updatedObj.keterangan);                                
            Assert.AreEqual(obj.penambahan_stok_gudang, updatedObj.penambahan_stok_gudang);                                
            Assert.AreEqual(obj.pengurangan_stok_gudang, updatedObj.pengurangan_stok_gudang);                                
            
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new PenyesuaianStok
            {
                penyesuaian_stok_id = "2a143e0d-d8c0-4f15-8216-813ec8ddf64c"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.penyesuaian_stok_id);
			Assert.IsNull(deletedObj);
        }
    }
}     
