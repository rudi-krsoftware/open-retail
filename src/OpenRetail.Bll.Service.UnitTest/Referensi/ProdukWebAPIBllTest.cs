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
    public class ProdukWebAPIBllTest
    {
        private ILog _log;
        private IProdukBll _bll;

        [TestInitialize]
        public void Init()
        {
            var baseUrl = "http://localhost/openretail_webapi/";

            _log = LogManager.GetLogger(typeof(ProdukWebAPIBllTest));
            _bll = new ProdukBll(true, baseUrl, _log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetHargaGrosirTest()
        {
            var obj = _bll.GetByKode("201607000000053");

            if (obj.list_of_harga_grosir.Count > 0)
            {
                var jumlah = 9;
                var hargaGrosir = obj.list_of_harga_grosir
                                     .Where(f => f.produk_id == obj.produk_id && f.jumlah_minimal <= jumlah)
                                     .OrderByDescending(f => f.harga_ke)
                                     .LastOrDefault();

                Assert.AreEqual(50000, hargaGrosir.harga_grosir);
                Assert.AreEqual(5, hargaGrosir.jumlah_minimal);
                Assert.AreEqual(1, hargaGrosir.diskon);
            }
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "17c7626c-e5ca-43f2-b075-af6b6cbcbf83";
            var obj = _bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("17c7626c-e5ca-43f2-b075-af6b6cbcbf83", obj.produk_id);
            Assert.AreEqual("201607000000055", obj.kode_produk);
            Assert.AreEqual("CD ROM ALL Merk 2nd", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(0, obj.stok);
            Assert.AreEqual(20000, obj.harga_beli);
            Assert.AreEqual(50000, obj.harga_jual);
            Assert.AreEqual("2aae21ba-8954-4db6-a6dc-c648e27255ad", obj.golongan_id);
            Assert.AreEqual(0, obj.minimal_stok);
            Assert.AreEqual(0, obj.stok_gudang);
            Assert.AreEqual(0, obj.minimal_stok_gudang);

            var golongan = obj.Golongan;
            Assert.AreEqual("2aae21ba-8954-4db6-a6dc-c648e27255ad", golongan.golongan_id);
            Assert.AreEqual("Hardward 2nd", golongan.nama_golongan);
        }

        [TestMethod]
        public void GetByKodeProdukTest()
        {
            var kodeProduk = "201607000000055";
            var obj = _bll.GetByKode(kodeProduk);

            Assert.IsNotNull(obj);
            Assert.AreEqual("17c7626c-e5ca-43f2-b075-af6b6cbcbf83", obj.produk_id);
            Assert.AreEqual("201607000000055", obj.kode_produk);
            Assert.AreEqual("CD ROM ALL Merk 2nd", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(0, obj.stok);
            Assert.AreEqual(20000, obj.harga_beli);
            Assert.AreEqual(50000, obj.harga_jual);
            Assert.AreEqual("2aae21ba-8954-4db6-a6dc-c648e27255ad", obj.golongan_id);
            Assert.AreEqual(0, obj.minimal_stok);
            Assert.AreEqual(0, obj.stok_gudang);
            Assert.AreEqual(0, obj.minimal_stok_gudang);

            var golongan = obj.Golongan;
            Assert.AreEqual("2aae21ba-8954-4db6-a6dc-c648e27255ad", golongan.golongan_id);
            Assert.AreEqual("Hardward 2nd", golongan.nama_golongan);
        }

        [TestMethod]
        public void GetLastKodeProdukTest()
        {
            var lastKodeProduk = _bll.GetLastKodeProduk();
            Assert.AreEqual("201701120066", lastKodeProduk);

            lastKodeProduk = _bll.GetLastKodeProduk();
            Assert.AreEqual("201701120067", lastKodeProduk);
        }

        [TestMethod]
        public void GetByNameTest()
        {
            var name = "cd";

            var index = 0;
            var oList = _bll.GetByName(name);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("17c7626c-e5ca-43f2-b075-af6b6cbcbf83", obj.produk_id);
            Assert.AreEqual("201607000000055", obj.kode_produk);
            Assert.AreEqual("CD ROM ALL Merk 2nd", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(0, obj.stok);
            Assert.AreEqual(20000, obj.harga_beli);
            Assert.AreEqual(50000, obj.harga_jual);
            Assert.AreEqual("2aae21ba-8954-4db6-a6dc-c648e27255ad", obj.golongan_id);
            Assert.AreEqual(0, obj.minimal_stok);
            Assert.AreEqual(0, obj.stok_gudang);
            Assert.AreEqual(0, obj.minimal_stok_gudang);

            var golongan = obj.Golongan;
            Assert.AreEqual("2aae21ba-8954-4db6-a6dc-c648e27255ad", golongan.golongan_id);
            Assert.AreEqual("Hardward 2nd", golongan.nama_golongan);

        }

        [TestMethod]
        public void GetByGolonganTest()
        {
            var golonganId = "2aae21ba-8954-4db6-a6dc-c648e27255ad";

            var index = 0;
            var oList = _bll.GetByGolongan(golonganId);
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("17c7626c-e5ca-43f2-b075-af6b6cbcbf83", obj.produk_id);
            Assert.AreEqual("201607000000055", obj.kode_produk);
            Assert.AreEqual("CD ROM ALL Merk 2nd", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(0, obj.stok);
            Assert.AreEqual(20000, obj.harga_beli);
            Assert.AreEqual(50000, obj.harga_jual);
            Assert.AreEqual("2aae21ba-8954-4db6-a6dc-c648e27255ad", obj.golongan_id);
            Assert.AreEqual(0, obj.minimal_stok);
            Assert.AreEqual(0, obj.stok_gudang);
            Assert.AreEqual(0, obj.minimal_stok_gudang);

            var golongan = obj.Golongan;
            Assert.AreEqual("2aae21ba-8954-4db6-a6dc-c648e27255ad", golongan.golongan_id);
            Assert.AreEqual("Hardward 2nd", golongan.nama_golongan);

        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 3;
            var oList = _bll.GetAll();
            var obj = oList[index];

            Assert.IsNotNull(obj);
            Assert.AreEqual("17c7626c-e5ca-43f2-b075-af6b6cbcbf83", obj.produk_id);
            Assert.AreEqual("201607000000055", obj.kode_produk);
            Assert.AreEqual("CD ROM ALL Merk 2nd", obj.nama_produk);
            Assert.AreEqual("", obj.satuan);
            Assert.AreEqual(0, obj.stok);
            Assert.AreEqual(20000, obj.harga_beli);
            Assert.AreEqual(50000, obj.harga_jual);
            Assert.AreEqual("2aae21ba-8954-4db6-a6dc-c648e27255ad", obj.golongan_id);
            Assert.AreEqual(0, obj.minimal_stok);
            Assert.AreEqual(0, obj.stok_gudang);
            Assert.AreEqual(0, obj.minimal_stok_gudang);

            var golongan = obj.Golongan;
            Assert.AreEqual("2aae21ba-8954-4db6-a6dc-c648e27255ad", golongan.golongan_id);
            Assert.AreEqual("Hardward 2nd", golongan.nama_golongan);

        }

        [TestMethod]
        public void SaveTest()
        {
            var listOfHargaGrosir = new List<HargaGrosir>();
            listOfHargaGrosir.Add(new HargaGrosir { harga_ke = 1, harga_grosir = 15000, jumlah_minimal = 5, diskon = 1 });
            listOfHargaGrosir.Add(new HargaGrosir { harga_ke = 2, harga_grosir = 13000, jumlah_minimal = 10, diskon = 1.5 });
            listOfHargaGrosir.Add(new HargaGrosir { harga_ke = 3, harga_grosir = 10000, jumlah_minimal = 15, diskon = 2.5 });

            var obj = new Produk
            {
                kode_produk = "200111101234",
                nama_produk = "Harga dengan grosir",
                satuan = "",
                stok = 10,
                minimal_stok = 5,
                harga_beli = 1000000,
                harga_jual = 1500000,
                golongan_id = "0a8b59e5-bb3b-4081-b963-9dc9584dcda6",
                stok_gudang = 15,
                minimal_stok_gudang = 5
            };
            obj.list_of_harga_grosir = listOfHargaGrosir;

            var validationError = new ValidationError();

            var result = _bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = _bll.GetByID(obj.produk_id);
            Assert.IsNotNull(newObj);
            Assert.AreEqual(obj.produk_id, newObj.produk_id);
            Assert.AreEqual(obj.nama_produk, newObj.nama_produk);
            Assert.AreEqual(obj.satuan, newObj.satuan);
            Assert.AreEqual(obj.stok, newObj.stok);
            Assert.AreEqual(obj.harga_beli, newObj.harga_beli);
            Assert.AreEqual(obj.harga_jual, newObj.harga_jual);
            Assert.AreEqual(obj.kode_produk, newObj.kode_produk);
            Assert.AreEqual(obj.golongan_id, newObj.golongan_id);
            Assert.AreEqual(obj.minimal_stok, newObj.minimal_stok);
            Assert.AreEqual(obj.stok_gudang, newObj.stok_gudang);
            Assert.AreEqual(obj.minimal_stok_gudang, newObj.minimal_stok_gudang);

        }

        [TestMethod]
        public void UpdateTest()
        {
            var obj = _bll.GetByID("53e03588-b9a2-43df-ae59-283e72917f9a");
            obj.kode_produk_old = obj.kode_produk;

            var index = 0;
            foreach (var item in obj.list_of_harga_grosir)
            {
                obj.list_of_harga_grosir[index].harga_grosir -= 100;
                obj.list_of_harga_grosir[index].jumlah_minimal -= 2;
                obj.list_of_harga_grosir[index].diskon -= 0.5;

                index++;
            }

            var validationError = new ValidationError();

            var result = _bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.GetByID(obj.produk_id);
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.produk_id, updatedObj.produk_id);
            Assert.AreEqual(obj.nama_produk, updatedObj.nama_produk);
            Assert.AreEqual(obj.satuan, updatedObj.satuan);
            Assert.AreEqual(obj.stok, updatedObj.stok);
            Assert.AreEqual(obj.harga_beli, updatedObj.harga_beli);
            Assert.AreEqual(obj.harga_jual, updatedObj.harga_jual);
            Assert.AreEqual(obj.kode_produk, updatedObj.kode_produk);
            Assert.AreEqual(obj.golongan_id, updatedObj.golongan_id);
            Assert.AreEqual(obj.minimal_stok, updatedObj.minimal_stok);
            Assert.AreEqual(obj.stok_gudang, updatedObj.stok_gudang);
            Assert.AreEqual(obj.minimal_stok_gudang, updatedObj.minimal_stok_gudang);
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new Produk
            {
                produk_id = "53e03588-b9a2-43df-ae59-283e72917f9a"
            };

            var result = _bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = _bll.GetByID(obj.produk_id);
            Assert.IsNull(deletedObj);
        }
    }
}
