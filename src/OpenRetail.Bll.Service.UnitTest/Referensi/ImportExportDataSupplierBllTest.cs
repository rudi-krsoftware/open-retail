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
    public class ImportExportDataSupplierBllTest
    {
        private ILog _log;
        private IImportExportDataBll<Supplier> _bll;
        private const string WorksheetName = "supplier";

        [TestInitialize]
        public void Init()
        {
            var fileName = @"D:\Project Non IC\Aplikasi Open Retail\src\OpenRetail.App\bin\Debug\File Import Excel\Master Data\data_supplier.xlsx";

            _log = LogManager.GetLogger(typeof(ImportExportDataSupplierBllTest));
            _bll = new ImportExportDataSupplierBll(fileName, _log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void IsOpenedTest()
        {
            var result = _bll.IsOpened();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsValidFormatTest()
        {
            var result = _bll.IsValidFormat(WorksheetName);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ImportTest()
        {
            var rowCount = 0;
            var result = _bll.Import(WorksheetName, ref rowCount);

            Assert.IsTrue(result);
            Assert.AreEqual(5, rowCount);
        }
    }
}
