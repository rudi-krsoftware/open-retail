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
    public class PrinterDotMatrix : IRAWPrinting
    {
        private string _printerName = string.Empty;

        public PrinterDotMatrix(string printerName)
        {
            _printerName = printerName;
        }

        public void Cetak(JualProduk jual, IList<HeaderNotaMiniPos> listOfHeaderNota, IList<FooterNotaMiniPos> listOfFooterNota, 
            int jumlahKarakter, int lineFeed, bool isCetakCustomer = true, bool isCetakKeteranganNota = true, int ukuranFont = 0)
        {
            throw new NotImplementedException();
        }

        public void Cetak(IList<ReportMesinKasir> listOfMesinKasir, IList<HeaderNotaMiniPos> listOfHeaderNota, int jumlahKarakter, int lineFeed)
        {
            throw new NotImplementedException();
        }

        public void Cetak(JualProduk jual, IList<HeaderNota> listOfHeaderNota, int jumlahBaris = 29, int jumlahKarakter = 80, bool isCetakKeteranganNota = true)
        {
            var garisPemisah = StringHelper.PrintChar('=', jumlahKarakter);

            var textToPrint = new StringBuilder();

            var rowCount = 0; // jumlah baris yang tercetak

            if (!Utils.IsRunningUnderIDE())
            {
                textToPrint.Append(ESCCommandHelper.InitializePrinter());
                textToPrint.Append(ESCCommandHelper.CenterText());
            }

            // cetak header
            foreach (var header in listOfHeaderNota)
            {
                if (header.keterangan.Length > 0)
                {
                    if (header.keterangan.Length > garisPemisah.Length)
                    {
                        header.keterangan = StringHelper.FixedLength(header.keterangan, garisPemisah.Length);
                    }

                    textToPrint.Append(header.keterangan).Append(ESCCommandHelper.LineFeed(1));

                    rowCount++;
                }
            }

            textToPrint.Append(ESCCommandHelper.LineFeed(1));
            rowCount++;

            if (!Utils.IsRunningUnderIDE())
            {
                textToPrint.Append(ESCCommandHelper.LeftText());
            }

            // cetak informasi nota
            textToPrint.Append("Nota   : ").Append(jual.nota).Append(ESCCommandHelper.LineFeed(1));
            textToPrint.Append("Tanggal: ").Append(DateTimeHelper.DateToString(jual.tanggal));

            if (jual.tanggal_tempo != null)
            {
                textToPrint.Append(StringHelper.PrintChar(' ', 4));
                textToPrint.Append("Tempo: ").Append(DateTimeHelper.DateToString(jual.tanggal_tempo)).Append(ESCCommandHelper.LineFeed(2));
            }
            else
            {
                textToPrint.Append(ESCCommandHelper.LineFeed(2));
            }

            rowCount += 4;

            // cetak informasi customer
            var namaCustomer = string.Empty;

            if (jual.is_sdac)
            {
                if (jual.Customer != null)
                {
                    namaCustomer = StringHelper.FixedLength(jual.Customer.nama_customer.NullToString(), jumlahKarakter - 10);
                }
            }
            else
            {
                namaCustomer = StringHelper.FixedLength(jual.kirim_kepada.NullToString(), jumlahKarakter - 10);
            }

            if (namaCustomer.Length > 0)
            {
                var alamat1 = jual.is_sdac ? jual.Customer.alamat.NullToString() : jual.kirim_alamat.NullToString();
                var alamat2 = string.Empty;

                var sb = new StringBuilder();

                if (jual.is_sdac)
                {
                    var customer = jual.Customer;
                    alamat2 = customer == null ? string.Empty : customer.get_wilayah_lengkap;
                }
                else
                {
                    alamat2 = jual.kirim_kecamatan;
                }

                textToPrint.Append("Kepada : ").Append(namaCustomer).Append(ESCCommandHelper.LineFeed(1));
                rowCount++;

                textToPrint.Append("Alamat : ");

                var isAddLineFeed = true;

                if (alamat1.Length > 0)
                {
                    textToPrint.Append(StringHelper.FixedLength(alamat1, jumlahKarakter - 10)).Append(ESCCommandHelper.LineFeed(1));
                    rowCount++;
                    isAddLineFeed = false;
                }

                if (alamat2.Length > 0)
                {
                    textToPrint.Append(StringHelper.PrintChar(' ', 9)).Append(StringHelper.FixedLength(alamat2, jumlahKarakter - 10)).Append(ESCCommandHelper.LineFeed(1));
                    rowCount++;
                    isAddLineFeed = false;
                }

                if (isAddLineFeed)
                {
                    textToPrint.Append(ESCCommandHelper.LineFeed(1));
                }
            }                        

            textToPrint.Append(garisPemisah).Append(ESCCommandHelper.LineFeed(1));

            // header tabel
            textToPrint.Append("NO");
            textToPrint.Append(StringHelper.PrintChar(' ', 2));
            textToPrint.Append("PRODUK");
            textToPrint.Append(StringHelper.PrintChar(' ', 34));
            textToPrint.Append("JUMLAH");
            textToPrint.Append(StringHelper.PrintChar(' ', 2));
            textToPrint.Append("HARGA");
            textToPrint.Append(StringHelper.PrintChar(' ', 4));
            textToPrint.Append("DISC (%)");
            textToPrint.Append(StringHelper.PrintChar(' ', 2));
            textToPrint.Append("SUB TOTAL").Append(ESCCommandHelper.LineFeed(1));

            textToPrint.Append(garisPemisah).Append(ESCCommandHelper.LineFeed(1));

            rowCount += 3;

            var lengthProduk = 37;
            var lengthJumlah = 5;
            var lengthHarga = 9;
            var lengthDisc = 5;
            var lengthSubTotal = 11;

            // cetak item
            var noUrut = 1;
            foreach (var item in jual.item_jual)
            {
                var strNoUrut = StringHelper.RightAlignment(noUrut.ToString(), 2);
                textToPrint.Append(strNoUrut).Append(StringHelper.PrintChar(' ', 2));

                var produk = StringHelper.FixedLength(item.Produk.nama_produk, lengthProduk);
                textToPrint.Append(produk).Append(StringHelper.PrintChar(' ', 2));

                var strJumlah = StringHelper.RightAlignment((item.jumlah - item.jumlah_retur).ToString(), lengthJumlah);
                textToPrint.Append(strJumlah).Append(StringHelper.PrintChar(' ', 2));

                var strHarga = StringHelper.RightAlignment(NumberHelper.NumberToString(item.harga_setelah_diskon), lengthHarga);
                textToPrint.Append(strHarga).Append(StringHelper.PrintChar(' ', 2));

                var strDisc = StringHelper.RightAlignment((item.diskon).ToString(), lengthDisc);
                textToPrint.Append(strDisc).Append(StringHelper.PrintChar(' ', 3));

                var subTotal = (item.jumlah - item.jumlah_retur) * item.harga_setelah_diskon;
                var strSubTotal = StringHelper.RightAlignment(NumberHelper.NumberToString(subTotal), lengthSubTotal);
                textToPrint.Append(strSubTotal).Append(ESCCommandHelper.LineFeed(1));

                if (item.keterangan.Length > 0)
                    textToPrint.Append(StringHelper.PrintChar(' ', 4)).Append(item.keterangan).Append(ESCCommandHelper.LineFeed(1));

                noUrut++;
                rowCount++;
            }

            // cetak footer
            textToPrint.Append(garisPemisah).Append(ESCCommandHelper.LineFeed(1));

            var lengthOngkosKirim = 11;

            if (jual.ongkos_kirim > 0)
            {
                textToPrint.Append(StringHelper.PrintChar(' ', 56));
                var strOngkosKirim = StringHelper.RightAlignment(NumberHelper.NumberToString(jual.ongkos_kirim), lengthOngkosKirim);
                textToPrint.Append("Ongkos Kirim:").Append(strOngkosKirim).Append(ESCCommandHelper.LineFeed(1));

                rowCount++;
            }
            
            if (jual.diskon > 0)
            {
                var strDiscNota = StringHelper.RightAlignment(NumberHelper.NumberToString(jual.diskon), lengthOngkosKirim);

                textToPrint.Append(StringHelper.PrintChar(' ', 56));
                textToPrint.Append("Diskon      :").Append(strDiscNota).Append(ESCCommandHelper.LineFeed(1));

                rowCount++;
            }

            if (jual.ppn > 0)
            {
                var strPPN = StringHelper.RightAlignment(NumberHelper.NumberToString(jual.ppn), lengthOngkosKirim);

                textToPrint.Append(StringHelper.PrintChar(' ', 56));
                textToPrint.Append("PPN         :").Append(strPPN).Append(ESCCommandHelper.LineFeed(1));

                rowCount++;
            }

            var strGrandTotal = StringHelper.RightAlignment(NumberHelper.NumberToString(jual.grand_total), lengthOngkosKirim);
            textToPrint.Append(StringHelper.PrintChar(' ', 56));
            textToPrint.Append("Total       :").Append(strGrandTotal).Append(ESCCommandHelper.LineFeed(1));

            textToPrint.Append(garisPemisah).Append(ESCCommandHelper.LineFeed(1));

            textToPrint.Append(StringHelper.PrintChar(' ', 8)).Append("Penerima");
            textToPrint.Append(StringHelper.PrintChar(' ', 40)).Append("Hormat Kami");
            textToPrint.Append(ESCCommandHelper.LineFeed(3));

            textToPrint.Append(StringHelper.PrintChar(' ', 6)).Append("------------");
            textToPrint.Append(StringHelper.PrintChar(' ', 37)).Append("-------------");

            if (isCetakKeteranganNota && jual.keterangan.Length > 0)
            {
                textToPrint.Append(ESCCommandHelper.LineFeed(1)).Append("Keterangan: ").Append(ESCCommandHelper.LineFeed(1));
                textToPrint.Append("* ");

                var splitKeterangan = StringHelper.SplitByLength(jual.keterangan, 78);
                foreach (var ket in splitKeterangan)
                {
                    textToPrint.Append(ket).Append(ESCCommandHelper.LineFeed(1));
                }

            }

            rowCount += 6;

            // perhitungan sisa kertas untuk keperluan line feed
            var listOfMaxRow = new Dictionary<int, int>();
            for (int i = 1; i < 11; i++)
            {
                var key = jumlahBaris * i;
                var value = key + 4;

                listOfMaxRow.Add(key, value);
            }

            var maxJumlahBaris = 0; // maksimal jumlah baris tercetak dalam satu halaman
            foreach (var item in listOfMaxRow)
            {
                maxJumlahBaris = item.Value;

                if (rowCount <= item.Key)
                    break;
            }

            var lineFeed = maxJumlahBaris - rowCount;

            textToPrint.Append(ESCCommandHelper.LineFeed(lineFeed));

            if (!Utils.IsRunningUnderIDE())
            {
                RawPrintHelper.SendStringToPrinter(_printerName, textToPrint.ToString());
            }
            else
            {
                RawPrintHelper.SendStringToFile(textToPrint.ToString());
            }
        }        
    }
}
