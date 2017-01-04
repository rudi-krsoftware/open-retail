using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
 
namespace OpenRetail.Bll.Service.UnitTest
{    
    [TestClass]
    public class AlasanPenyesuaianStokBllTest
    {
        private IAlasanPenyesuaianStokBll bll = null;

        [TestInitialize]
        public void Init()
        {
            bll = new AlasanPenyesuaianStokBll();
        }

        [TestCleanup]
        public void CleanUp()
        {
            bll = null;
        }

        [TestMethod]
        public void GetByIDTest()
        {
            var id = "e4ef2a27-6600-365f-1e07-2963d55cc4bf";
            var obj = bll.GetByID(id);

            Assert.IsNotNull(obj);
            Assert.AreEqual("e4ef2a27-6600-365f-1e07-2963d55cc4bf", obj.alasan_penyesuaian_stok_id);
            Assert.AreEqual("Koreksi (Koreksi karena kesalahan input)", obj.alasan);
                     
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 1;
            var oList = bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("e4ef2a27-6600-365f-1e07-2963d55cc4bf", obj.alasan_penyesuaian_stok_id);
            Assert.AreEqual("Koreksi (Koreksi karena kesalahan input)", obj.alasan);                               
                     
        }

        [TestMethod]
        public void SaveTest()
        {
            var obj = new AlasanPenyesuaianStok
            {
                alasan = "Dipake sendiri"
            };

            var validationError = new ValidationError();

            var result = bll.Save(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var newObj = bll.GetByID(obj.alasan_penyesuaian_stok_id);
			Assert.IsNotNull(newObj);
			Assert.AreEqual(obj.alasan_penyesuaian_stok_id, newObj.alasan_penyesuaian_stok_id);                                
            Assert.AreEqual(obj.alasan, newObj.alasan);                                
            
		}

        [TestMethod]
        public void UpdateTest()
        {
            var obj = new AlasanPenyesuaianStok
            {
                alasan_penyesuaian_stok_id = "ab6b9e7d-f0c2-4b49-b257-cf518f7af145",
                alasan = "Dipake sendiri (Prive)"
        	};

            var validationError = new ValidationError();

            var result = bll.Update(obj, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);

            var updatedObj = bll.GetByID(obj.alasan_penyesuaian_stok_id);
			Assert.IsNotNull(updatedObj);
            Assert.AreEqual(obj.alasan_penyesuaian_stok_id, updatedObj.alasan_penyesuaian_stok_id);                                
            Assert.AreEqual(obj.alasan, updatedObj.alasan);                                
            
        }

        [TestMethod]
        public void DeleteTest()
        {
            var obj = new AlasanPenyesuaianStok
            {
                alasan_penyesuaian_stok_id = "ab6b9e7d-f0c2-4b49-b257-cf518f7af145"
            };

            var result = bll.Delete(obj);
            Assert.IsTrue(result != 0);

            var deletedObj = bll.GetByID(obj.alasan_penyesuaian_stok_id);
			Assert.IsNull(deletedObj);
        }
    }
}     
