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
using OpenRetail.App.Helper;

namespace OpenRetail.App.Transaksi.PrinterMiniPOS
{
    public class PrinterMiniPOS : IPrinterMiniPOS
    {
        private string _printerName = string.Empty;

        public PrinterMiniPOS(string printerName)
        {
            _printerName = printerName;
        }

        public void Cetak(JualProduk jual, IList<HeaderNotaMiniPos> listOfHeaderNota, IList<FooterNotaMiniPos> listOfFooterNota, int jumlahKarakter, int lineFeed, bool isCetakCustomer = true)
        {
            var garisPemisah = StringHelper.PrintChar('=', jumlahKarakter);

            var textToPrint = new StringBuilder();

            if (!Utils.IsRunningUnderIDE())
            {
                textToPrint.Append(ESCCommandHelper.InitializePrinter());
                textToPrint.Append(ESCCommandHelper.LineSpacing());
                textToPrint.Append(ESCCommandHelper.CenterText());
            }

            // cetak header
            foreach (var header in listOfHeaderNota)
            {
                if (header.keterangan.Length > 0)
                {
                    header.keterangan = StringHelper.FixedLength(header.keterangan, garisPemisah.Length);
                    textToPrint.Append(header.keterangan).Append(ESCCommandHelper.LineFeed(1));
                }
            }

            // cetak garis
            textToPrint.Append(garisPemisah).Append(ESCCommandHelper.LineFeed(1));

            if (!Utils.IsRunningUnderIDE())
                textToPrint.Append(ESCCommandHelper.LeftText());

            // set tanggal, jam, user
            var tanggal = StringHelper.FixedLength(DateTimeHelper.DateToString(DateTime.Now), 10);
            var jam = DateTimeHelper.TimeToString(DateTime.Now);

            var kasir = StringHelper.PrintChar(' ', (garisPemisah.Length - 18 - jual.Pengguna.nama_pengguna.Length) / 2) + jual.Pengguna.nama_pengguna;
            kasir = StringHelper.FixedLength(kasir, garisPemisah.Length - 18);

            // cetak tanggal, kasir, jam
            textToPrint.Append(tanggal);
            textToPrint.Append(kasir); // pengguna
            textToPrint.Append(jam).Append(ESCCommandHelper.LineFeed(2)); // jam

            // cetak info nota
            textToPrint.Append("Nota   : ").Append(jual.nota).Append(ESCCommandHelper.LineFeed(1));
            textToPrint.Append("Tanggal: ").Append(DateTimeHelper.DateToString(jual.tanggal)).Append(ESCCommandHelper.LineFeed(1));
            textToPrint.Append("Tempo  : ").Append(jual.tanggal_tempo == null ? "-" : DateTimeHelper.DateToString(jual.tanggal_tempo)).Append(ESCCommandHelper.LineFeed(1));

            if (isCetakCustomer)
            {
                textToPrint.Append(ESCCommandHelper.LineFeed(1));

                // cetak info customer
                textToPrint.Append("Kepada: ").Append(ESCCommandHelper.LineFeed(1));

                var namaCustomer = jual.is_sdac == true ? jual.Customer.nama_customer : jual.kirim_kepada;
                var alamat = jual.is_sdac == true ? jual.Customer.alamat : jual.kirim_alamat;
                var telepon = jual.is_sdac == true ? jual.Customer.telepon : jual.kirim_telepon;

                textToPrint.Append(namaCustomer).Append(ESCCommandHelper.LineFeed(1));

                if (alamat.Length > 0)
                    textToPrint.Append(alamat).Append(ESCCommandHelper.LineFeed(1));

                if (telepon.Length > 0)
                    textToPrint.Append("HP: " + telepon).Append(ESCCommandHelper.LineFeed(1));
            }            

            // cetak garis
            textToPrint.Append(garisPemisah).Append(ESCCommandHelper.LineFeed(1));

            // cetak item
            foreach (var item in jual.item_jual)
            {
                var produk = StringHelper.FixedLength(item.Produk.nama_produk, garisPemisah.Length);
                textToPrint.Append(produk).Append(ESCCommandHelper.LineFeed(1));

                var jumlah = StringHelper.RightAlignment(item.jumlah.ToString(), 4);
                textToPrint.Append(jumlah);

                textToPrint.Append("  " + StringHelper.FixedLength("x", 3));

                var harga = StringHelper.RightAlignment(NumberHelper.NumberToString(item.harga_jual), 10);
                textToPrint.Append(harga);

                var diskon = StringHelper.RightAlignment(item.diskon.ToString(), 7);
                textToPrint.Append(diskon);

                var subTotal = (item.jumlah - item.jumlah_retur) * item.harga_setelah_diskon;

                var sSubTotal = StringHelper.RightAlignment(NumberHelper.NumberToString(subTotal), garisPemisah.Length - 26);
                textToPrint.Append(sSubTotal).Append(ESCCommandHelper.LineFeed(1));
            }

            // cetak garis
            textToPrint.Append(garisPemisah).Append(ESCCommandHelper.LineFeed(1));

            var fixedLengthLabelFooter = 12;
            var fixedLengthValueFooter = garisPemisah.Length - fixedLengthLabelFooter - 3;

            // cetak footer
            if (jual.ongkos_kirim > 0)
            {
                textToPrint.Append(StringHelper.FixedLength("Kurir", fixedLengthLabelFooter));
                textToPrint.Append(" : " + jual.kurir).Append(ESCCommandHelper.LineFeed(1));

                textToPrint.Append(StringHelper.FixedLength("Ongkos Kirim", fixedLengthLabelFooter));
                textToPrint.Append(" : " + StringHelper.RightAlignment(NumberHelper.NumberToString(jual.ongkos_kirim), fixedLengthValueFooter)).Append(ESCCommandHelper.LineFeed(1));
            }

            textToPrint.Append(StringHelper.FixedLength("Total Item", fixedLengthLabelFooter));
            textToPrint.Append(" : " + StringHelper.RightAlignment(NumberHelper.NumberToString(jual.item_jual.Count), fixedLengthValueFooter)).Append(ESCCommandHelper.LineFeed(1));

            textToPrint.Append(StringHelper.FixedLength("Diskon", fixedLengthLabelFooter));
            textToPrint.Append(" : " + StringHelper.RightAlignment(NumberHelper.NumberToString(jual.diskon), fixedLengthValueFooter)).Append(ESCCommandHelper.LineFeed(1));

            textToPrint.Append(StringHelper.FixedLength("PPN", fixedLengthLabelFooter));
            textToPrint.Append(" : " + StringHelper.RightAlignment(NumberHelper.NumberToString(jual.ppn), fixedLengthValueFooter)).Append(ESCCommandHelper.LineFeed(1));

            textToPrint.Append(StringHelper.FixedLength("Total", fixedLengthLabelFooter));
            textToPrint.Append(" : " + StringHelper.RightAlignment(NumberHelper.NumberToString(jual.grand_total), fixedLengthValueFooter)).Append(ESCCommandHelper.LineFeed(1));

            if (jual.jumlah_bayar > 0)
            {
                textToPrint.Append(StringHelper.FixedLength("Jumlah Bayar", fixedLengthLabelFooter));
                textToPrint.Append(" : " + StringHelper.RightAlignment(NumberHelper.NumberToString(jual.jumlah_bayar), fixedLengthValueFooter)).Append(ESCCommandHelper.LineFeed(1));

                textToPrint.Append(StringHelper.FixedLength("Kembali", fixedLengthLabelFooter));
                textToPrint.Append(" : " + StringHelper.RightAlignment(NumberHelper.NumberToString(jual.jumlah_bayar - jual.grand_total), fixedLengthValueFooter)).Append(ESCCommandHelper.LineFeed(1));
            }            

            // cetak garis
            textToPrint.Append(garisPemisah).Append(ESCCommandHelper.LineFeed(2));

            if (!Utils.IsRunningUnderIDE())
            {
                textToPrint.Append(ESCCommandHelper.CenterText());
            }

            // cetak footer
            foreach (var footer in listOfFooterNota)
            {
                if (footer.keterangan.Length > 0)
                {
                    footer.keterangan = StringHelper.FixedLength(footer.keterangan, garisPemisah.Length);
                    textToPrint.Append(footer.keterangan).Append(ESCCommandHelper.LineFeed(1));
                }
            }

            textToPrint.Append(ESCCommandHelper.LineFeed(lineFeed));

            if (!Utils.IsRunningUnderIDE())
            {
                RawPrinterHelper.SendStringToPrinter(_printerName, textToPrint.ToString());
            }
            else
            {
                RawPrinterHelper.SendStringToFile(textToPrint.ToString());
            }            
        }
    }
}
