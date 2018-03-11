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
    public class FooterNotaMiniPosBllTest
    {
        private IFooterNotaMiniPosBll _bll;

        [TestInitialize]
        public void Init()
        {
            _bll = new FooterNotaMiniPosBll();
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetAllTest()
        {
            var index = 1;
            var oList = _bll.GetAll();
            var obj = oList[index];
                 
            Assert.IsNotNull(obj);
            Assert.AreEqual("63602003-1368-81e4-dd6c-0bc7b25417d7", obj.footer_nota_id);
            Assert.AreEqual("=== LAYANAN KONSUMEN ===", obj.keterangan);                                
            Assert.AreEqual(2, obj.order_number);
            Assert.IsTrue(obj.is_active);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var listOfFooterNota = _bll.GetAll();
            Assert.AreEqual(3, listOfFooterNota.Count);

            var headerNota = listOfFooterNota[1];
            headerNota.keterangan = "=== LAYANAN KONSUMEN ===";

            var validationError = new ValidationError();

            var result = _bll.Update(headerNota, ref validationError);
            Console.WriteLine("Error : " + validationError.Message);

            Assert.IsTrue(result != 0);                            
        }
    }
}     
