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
using System.Windows.Forms;
using System.Reflection;

using log4net;
using OpenRetail.App.Referensi;
using OpenRetail.App.Transaksi;
using OpenRetail.App.Main;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace OpenRetail.App
{
    static class MainProgram
    {
        /// <summary>
        /// Instance log4net
        /// </summary>
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly string appName = "Open Retail Versi {0} - Copyright © 2017 Kamarudin";

        /// <summary>
        /// unik kode untuk enkripsi password menggunakan metode md5
        /// </summary>
        public static readonly string securityCode = "BhGr7YwZpdX7ubFuZCuU";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.GlobalContext.Properties["UserName"] = "Admin"; // TODO: fix me (ganti dengan user yang login)

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
