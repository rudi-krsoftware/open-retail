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
using System.IO;
using System.Linq;
using System.Text;

namespace OpenRetail.App.Helper
{
    public static class StringHelper
    {
        public static char ConvertToUpper(System.Windows.Forms.KeyPressEventArgs e)
        {
            return Convert.ToChar(e.KeyChar.ToString().ToUpper());
        }

        public static void SendStringToText(string data, string fileName)
        {
            using (var writer = new StreamWriter(fileName, false))
            {
                writer.WriteLine(data);
            }
        }

        public static string PrintChar(char karakter, int jumlahKarakter)
        {
            var result = string.Empty;

            for (int i = 0; i < jumlahKarakter; i++)
            {
                result += karakter;
            }

            return result;
        }

        public static string FixedLength(string s, int fixedLength)
        {
            if (s.Length > fixedLength)
                s = s.Substring(0, fixedLength);

            var result = s;
            var sLength = s.Length;

            var sisa = fixedLength - sLength;

            for (int i = 0; i < sisa; i++)
            {
                result += " ";
            }

            return result;
        }

        /// <summary>
        /// Untuk menampilkan string dengan format rata kanan
        /// </summary>
        /// <param name="s"></param>
        /// <param name="fixedLength"></param>
        /// <returns></returns>
        public static string RightAlignment(string s, int fixedLength)
        {
            if (s.Length > fixedLength)
                s = s.Substring(0, fixedLength);

            var result = s;

            var sLength = s.Length;
            var sisa = fixedLength - sLength;

            var space = string.Empty;
            for (int i = 0; i < sisa; i++)
            {
                space += " ";
            }

            result = space + result;

            return result;
        }

        /// <summary>
        /// Fungsi untuk memecah string berdasarkan karakter spasi
        /// </summary>
        /// <param name="s">string yang ingin dipecah</param>
        /// <param name="index">index string yang diingin diambil</param>
        /// <returns></returns>
        public static string Split(string s, int index)
        {
            string[] words = s.Split(' ');

            words[index] = words[index].ToString().TrimStart();
            words[index] = words[index].ToString().TrimEnd();

            return words[index];
        }

        /// <summary>
        /// Fungsi untuk memecah string berdasarkan karakter yang diinginkan
        /// </summary>
        /// <param name="s">string yang ingin dipecah</param>
        /// <param name="index">index string yang diingin diambil</param>
        /// <param name="separator">karakter pemisah string</param>
        /// <returns></returns>
        public static string Split(string s, short index, char separator)
        {
            string[] words = s.Split(separator);

            words[index] = words[index].ToString().TrimStart();
            words[index] = words[index].ToString().TrimEnd();

            return words[index];
        }
    }
}
