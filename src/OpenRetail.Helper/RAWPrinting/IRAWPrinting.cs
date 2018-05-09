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

using OpenRetail.Model;
using OpenRetail.Model.Report;

namespace OpenRetail.Helper.RAWPrinting
{
    public interface IRAWPrinting
    {
        /// <summary>
        /// Override method untuk mencetak nota jual menggunakan printer mini pos
        /// </summary>
        /// <param name="jual">objek jual</param>
        /// <param name="listOfHeaderNota">list objek header nota</param>
        /// <param name="listOfFooterNota">list objek footer nota</param>        
        /// <param name="jumlahKarakter">maksimal jumlah karakter yang tercetak</param>
        /// <param name="lineFeed">jumlah gulung kertas setelah pencetakan selesai</param>
        void Cetak(JualProduk jual, IList<HeaderNotaMiniPos> listOfHeaderNota, IList<FooterNotaMiniPos> listOfFooterNota,
            int jumlahKarakter, int lineFeed, bool isCetakCustomer = true, bool isCetakKeteranganNota = true, int ukuranFont = 0, 
            string autocutCode = "", string openCashDrawerCode = "", string infoCopyright1 = "", string infoCopyright2 = "");

        /// <summary>
        /// Override method untuk mencetak laporan kasir menggunakan printer mini pos
        /// </summary>
        /// <param name="listOfMesinKasir"></param>
        /// <param name="listOfHeaderNota"></param>
        /// <param name="jumlahKarakter"></param>
        void Cetak(IList<ReportMesinKasir> listOfMesinKasir, IList<HeaderNotaMiniPos> listOfHeaderNota, int jumlahKarakter, int lineFeed, int ukuranFont = 0,
            string autocutCode = "", string infoCopyright1 = "", string infoCopyright2 = "");

        /// <summary>
        /// Override method untuk mencetak nota jual menggunakan printer dot matrix
        /// </summary>
        /// <param name="jual">Objek jual</param>
        /// <param name="listOfHeaderNota">List of header nota</param>
        /// <param name="jumlahBaris">Maksimal jumlah baris yang tercetak dalam satu halaman</param>
        /// <param name="jumlahKarakter">Maksimal jumlah karakter/kolom</param>
        void Cetak(JualProduk jual, IList<HeaderNota> listOfHeaderNota, int jumlahBaris = 29, int jumlahKarakter = 80, bool isCetakKeteranganNota = true, string infoCopyright = "");
    }
}
