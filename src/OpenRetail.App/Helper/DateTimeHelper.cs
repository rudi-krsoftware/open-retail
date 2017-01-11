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

namespace OpenRetail.App.Helper
{
    public static class DateTimeHelper
    {

        public static DateTime GetNullDateTime()
        {
            return new DateTime(0001, 1, 1);
        }

        /// <summary>
        /// Method untuk mengecek apakah sebuah tanggal null atau tidak
        /// </summary>
        /// <param name="tanggal"></param>
        /// <returns></returns>
        public static bool IsNull(DateTime tanggal)
        {
            var result = true;

            try
            {
                result = tanggal == DateTime.MinValue || tanggal == new DateTime(1753, 1, 1) ||
                         tanggal == new DateTime(0001, 1, 1);
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Untuk mengkonversi data tanggal ke format dd/MM/yyyy
        /// </summary>
        /// <param name="tanggal">Data tanggal dengan tipe DateTime</param>
        /// <returns></returns>
        public static string DateToString(Nullable<DateTime> tanggal, string format = "dd/MM/yyyy")
        {
            var result = string.Empty;

            try
            {
                if (!(tanggal == DateTime.MinValue || tanggal == new DateTime(1753, 1, 1)))
                    result = string.Format("{0:" + format + "}", tanggal);
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Untuk mengkonversi data tanggal ke format hari bulan tahun
        /// </summary>
        /// <param name="hari"></param>
        /// <param name="bulan"></param>
        /// <param name="tahun"></param>
        /// <returns></returns>
        public static string DateToString(int hari, int bulan, int tahun)
        {
            string result = hari + " " + DayMonthHelper.GetBulanIndonesia(bulan) + " " + tahun;

            return result;
        }

        public static string TimeToString(DateTime jam, string format = "HH:mm:ss")
        {
            string result = jam.ToString(format);

            return result;
        }

        public static DateTime GetTime(int hour, int minute, int second = 0)
        {
            return new DateTime(1753, 1, 1, hour, minute, second);
        }

        public static DateTime GetTime(string jam)
        {
            var words = jam.Split(':');
            var time = new DateTime(1753, 1, 1, int.Parse(words[0]), int.Parse(words[1]), 0);

            return time;
        }

        /// <summary>
        /// Untuk mengkonversi data string ke format tanggal
        /// </summary>
        /// <param name="tanggal">Data tanggal dengan format dd/MM/yyyy</param>
        /// <returns></returns>
        public static DateTime StringToDateTime(string tanggal)
        {
            var words = tanggal.Split('/');

            var tgl = new DateTime(int.Parse(words[2]), int.Parse(words[1]), int.Parse(words[0]));

            return tgl;
        }

        public static bool IsValidTimeValue(string jam)
        {
            var result = false;

            try
            {
                var words = jam.Split(':');
                var time = new DateTime(1753, 1, 1, int.Parse(words[0]), int.Parse(words[1]), 0);

                result = true;
            }
            catch
            {
            }

            return result;
        }

        public static bool IsValidRangeTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            return tanggalMulai <= tanggalSelesai;
        }
    }
}
