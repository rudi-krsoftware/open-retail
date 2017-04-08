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
using OpenRetail.Model;
using System.Globalization;
using System.Threading;
using OpenRetail.Model.Report;
using OpenRetail.App.Laporan;
using CrashReporterDotNET;
using OpenRetail.App.Helper;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace OpenRetail.App
{
    static class MainProgram
    {
        /// <summary>
        /// Instance log4net
        /// </summary>
        public static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly string stageOfDevelopment = "-beta";
        public static readonly string appName = "Open Retail Versi {0}{1} - Copyright © {2} Kamarudin";

        /// <summary>
        /// Kode unik untuk enkripsi password menggunakan metode md5
        /// Untuk alasan keamanan, sebaiknya nilai ini diganti
        /// </summary>
        public static readonly string securityCode = "BhGr7YwZpdX7ubFuZCuU";

        public static Profil profil = null;
        public static Pengguna pengguna = null;
        public static PengaturanUmum pengaturanUmum = null;

        private static bool _isLogout;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!Utils.IsRunningUnderIDE())
            {
                Application.ThreadException += delegate(object sender, ThreadExceptionEventArgs e)
                {
                    ReportCrash(e.Exception);
                };

                AppDomain.CurrentDomain.UnhandledException += delegate(object sender, UnhandledExceptionEventArgs e)
                {
                    ReportCrash((Exception)e.ExceptionObject);
                    Environment.Exit(0);
                };
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Login();
        }

        /// <summary>
        /// Method untuk mengirim bug/error program via email menggunakan library CrashReporter.NET
        /// </summary>
        /// <param name="exception"></param>
        static void ReportCrash(Exception exception)
        {
            // TODO: lengkapi property FromEmail, ToEmail, UserName dan Password
            var reportCrash = new ReportCrash
            {
                FromEmail = "",
                ToEmail = "",
                SmtpHost = "smtp.gmail.com",
                Port = 587,
                EnableSSL = true,
                UserName = "",
                Password = "",                
                AnalyzeWithDoctorDump = false
            };

            reportCrash.Send(exception);
        }

        static void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            _isLogout = ((FrmMain)sender).IsLogout;
        }        

        static void Login()
        {
            var frmMain = new FrmMain();
            frmMain.FormClosed -= frmMain_FormClosed;
            frmMain.FormClosed += frmMain_FormClosed;

            var frmLogin = new FrmLogin();
            if (frmLogin.ShowDialog(frmMain) == DialogResult.OK)
            {
                // set Default RegionalSetting menggunakan United States
                SetDefaultRegionalSetting();

                frmMain.InisialisasiData();
                Application.Run(frmMain);

                if (_isLogout)
                    Login();
                else
                    Application.Exit();
            }
            else
                Application.Exit();
        }

        static void SetDefaultRegionalSetting()
        {
            var cultureInfo = Thread.CurrentThread.CurrentCulture;
            var regionInfo = new RegionInfo(cultureInfo.LCID);

            string englishName = regionInfo.EnglishName;

            if (!(englishName == "United States"))
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                }
                catch
                {
                }
            }
        }
    }
}
