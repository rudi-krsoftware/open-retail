using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenRetail.App.Helper
{
    public static class Utils
    {
        /// <summary>
        /// Untuk mengecek apakah aplikasi dijalankan dari IDE atau tidak
        /// </summary>
        /// <returns></returns>
        public static bool IsRunningUnderIDE()
        {
            return System.Diagnostics.Debugger.IsAttached;
        }

        /// <summary>
        /// Untuk mendapatkan folder instalasi
        /// </summary>
        /// <returns></returns>
        public static string GetAppPath()
        {
            return Directory.GetCurrentDirectory();
        }


        /// <summary>
        /// Untuk mendapatkan versi aplikasi dengan format : Major.Minor.Revision
        /// </summary>
        /// <param name="appExe">Assembly name</param>
        /// <returns></returns>
        public static string GetCurrentVersion(string appExe)
        {
            var fvi = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            var version = fvi.ProductMajorPart + "." + fvi.ProductMinorPart + "." + fvi.ProductBuildPart;

            return version;
        }
    }
}
