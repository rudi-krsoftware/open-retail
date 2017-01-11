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
using System.Windows.Forms;

namespace OpenRetail.App.Helper
{
    /// <summary>
    /// Class untuk menghandle proses penekanan tombol keyboard
    /// </summary>
    public static class KeyPressHelper
    {
        /// <summary>
        /// Untuk mengecek apakah user menekan tombol Enter
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsEnter(KeyPressEventArgs e)
        {
            return (e.KeyChar == (char)Keys.Return);
        }

        public static bool IsEnter(KeyEventArgs e)
        {
            return (e.KeyCode == Keys.Enter);
        }

        /// <summary>
        /// Untuk mengecek apakah user menekan tombol Esc
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool IsEsc(KeyPressEventArgs e)
        {
            return (e.KeyChar == (char)Keys.Escape);
        }

        public static bool IsEsc(KeyEventArgs e)
        {
            return (e.KeyCode == Keys.Escape);
        }

        public static bool IsShortcutKey(Keys shortcut, KeyEventArgs e)
        {
            return (e.KeyCode == shortcut);
        }

        /// <summary>
        /// Untuk pindah ke inputakan berikutnya
        /// </summary>
        public static void NextFocus()
        {
            SendKeys.Send("{Tab}");
        }

        /// <summary>
        /// Untuk validasi input angka
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static bool NumericOnly(KeyPressEventArgs e)
        {
            string strValid = "0123456789.";

            if (strValid.IndexOf(e.KeyChar) < 0 && !(e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                return true; // not valid
            }
            else
            {
                return false; // valid
            }
        }

        /// <summary>
        /// Mengirim sinyal penekanan event enter
        /// </summary>
        /// <returns></returns>
        public static KeyPressEventArgs SendEnter()
        {
            return new KeyPressEventArgs((char)Keys.Return);
        }
    }
}
