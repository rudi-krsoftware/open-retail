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
    public class DatabaseVersionBllTest
    {
        private ILog _log;
        private IDatabaseVersionBll _bll;

        [TestInitialize]
        public void Init()
        {
            _log = LogManager.GetLogger(typeof(DatabaseVersionBllTest));
            _bll = new DatabaseVersionBll(_log);
        }

        [TestCleanup]
        public void CleanUp()
        {
            _bll = null;
        }

        [TestMethod]
        public void GetTest()
        {
            var obj = _bll.Get();

            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.version_number);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var result = _bll.UpdateVersion();

            Assert.IsTrue(result != 0);

            var updatedObj = _bll.Get();
            Assert.IsNotNull(updatedObj);
            Assert.AreEqual(2, updatedObj.version_number);
        }
    }
}
