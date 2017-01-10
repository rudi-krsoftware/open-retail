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
    public static class DayMonthHelper
    {

        /// <summary>
        /// Untuk mengecek tanggal minimal
        /// </summary>
        /// <param name="tanggal"></param>
        /// <returns></returns>
        public static bool IsMinDate(DateTime tanggal)
        {
            try
            {
                return (tanggal == DateTime.MinValue || tanggal == new DateTime(1753, 1, 1));
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Untuk mendapatkan index hari
        /// </summary>
        /// <param name="tanggal">Tanggal yang ingin dicari index harinya</param>
        /// <returns></returns>
        private static int Weekday(DateTime tanggal)
        {
            DayOfWeek startOfWeek = DayOfWeek.Sunday;
            return (tanggal.DayOfWeek - startOfWeek + 7) % 7;
        }

        /// <summary>
        /// Untuk mendapatkan nama hari berdasarkan tanggal
        /// </summary>
        /// <param name="tanggal">Tanggal yang ingin dicari harinya</param>
        /// <returns></returns>
        public static string GetHariIndonesia(DateTime tanggal)
        {
            string[] hari = { "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jum'at", "Sabtu" };

            return hari[Weekday(tanggal)];
        }

        /// <summary>
        /// Untuk mendapatkan nama bulan dalam format huruf berdasarkan bulan angka
        /// </summary>
        /// <param name="bulan">Diisi dengan bulan angka</param>
        /// <returns></returns>
        public static string GetBulanIndonesia(int bulan)
        {
            string[] bulans = { 
                                "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", 
                                "Agustus", "September", "Oktober", "November", "Desember" 
                              };

            return bulans[bulan - 1];
        }

        /// <summary>
        /// Untuk mendapatkan index bulan berdasarkan nama bulan
        /// </summary>
        /// <param name="bulan">Diisi dengan nama bulan. Misal Januari</param>
        /// <returns></returns>
        public static int GetBulanAngka(string bulan)
        {
            Dictionary<string, string> daftarBulan = new Dictionary<string, string>();

            daftarBulan.Clear();
            daftarBulan.Add("Januari", "1");
            daftarBulan.Add("Februari", "2");
            daftarBulan.Add("Maret", "3");
            daftarBulan.Add("April", "4");
            daftarBulan.Add("Mei", "5");
            daftarBulan.Add("Juni", "6");
            daftarBulan.Add("Juli", "7");
            daftarBulan.Add("Agustus", "8");
            daftarBulan.Add("September", "9");
            daftarBulan.Add("Oktober", "10");
            daftarBulan.Add("November", "11");
            daftarBulan.Add("Desember", "12");

            return Convert.ToInt32(daftarBulan[StringHelper.Split(bulan, 0)]);
        }

        /// <summary>
        /// Untuk mendapatkan informasi daftar bulan dan tahun
        /// </summary>
        /// <param name="tahunMulai">Diisi dengan tahun mulai, nilai default 2011</param>
        /// <param name="isBulanOnly">Diisi dengan nilai true atau false, nilai default false</param>
        /// <returns></returns>
        public static List<string> GetListBulan(int tahunMulai = 2011, bool isBulanOnly = false)
        {
            var listBulan = new List<string>();

            for (int tahun = tahunMulai; tahun <= DateTime.Today.Year; tahun++)
            {
                for (int bulan = 1; bulan < 13; bulan++)
                {
                    if (isBulanOnly)
                    {
                        listBulan.Add(GetBulanIndonesia(bulan));
                    }
                    else
                    {
                        listBulan.Add(GetBulanIndonesia(bulan) + " " + tahun);
                    }
                }
            }

            return listBulan;
        }
    }
}
