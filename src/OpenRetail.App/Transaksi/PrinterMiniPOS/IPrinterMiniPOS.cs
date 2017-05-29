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

namespace OpenRetail.App.Transaksi.PrinterMiniPOS
{
    public interface IPrinterMiniPOS
    {
        /// <summary>
        /// Override method untuk mencetak nota mini pos
        /// </summary>
        /// <param name="jual">objek jual</param>
        /// <param name="listOfHeaderNota">list objek header nota</param>
        /// <param name="listOfFooterNota">list objek footer nota</param>        
        /// <param name="jumlahKarakter">maksimal jumlah karakter yang tercetak</param>
        /// <param name="lineFeed">jumlah gulung kertas setelah pencetakan selesai</param>
        void Cetak(JualProduk jual, IList<HeaderNotaMiniPos> listOfHeaderNota, IList<FooterNotaMiniPos> listOfFooterNota, int jumlahKarakter, int lineFeed, bool isCetakCustomer = true);
    }
}
