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
    public class PrinterMiniPOS : IRAWPrinting
    {
        private string _printerName = string.Empty;

        public PrinterMiniPOS(string printerName)
        {
            _printerName = printerName;
        }

        public void Cetak(JualProduk jual, IList<HeaderNota> listOfHeaderNota, int jumlahBaris = 29, int jumlahKarakter = 80, bool isCetakKeteranganNota = true, string infoCopyright = "")
        {
            throw new NotImplementedException();
        }

        public void Cetak(IList<ReportMesinKasir> listOfMesinKasir, IList<HeaderNotaMiniPos> listOfHeaderNota, int jumlahKarakter, int lineFeed, int ukuranFont = 0,
            string autocutCode = "", string infoCopyright1 = "", string infoCopyright2 = "")
        {
            var garisPemisah = StringHelper.PrintChar('=', jumlahKarakter);
            var textToPrint = new StringBuilder();            
            
            var totalSaldoAwal = 0d;
            var totalItem = 0;
            var totalDiskon = 0d;
            var totalPPN = 0d;
            var grandTotal = 0d;
            var maxFormatNumber = 10;

            if (!Utils.IsRunningUnderIDE())
            {
                textToPrint.Append(ESCCommandHelper.InitializePrinter());

                if (ukuranFont > 0)
                    textToPrint.Append(ESCCommandHelper.FontNormal(ukuranFont));
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

                    textToPrint.Append(CenterText(header.keterangan.Length, jumlahKarakter)).Append(header.keterangan).Append(ESCCommandHelper.LineFeed(1));
                }
            }

            textToPrint.Append(ESCCommandHelper.LineFeed(1));

            textToPrint.Append("Laporan Penjualan Per Kasir").Append(ESCCommandHelper.LineFeed(1));
            textToPrint.Append("Per tanggal: ").Append(DateTimeHelper.DateToString(DateTime.Today)).Append(ESCCommandHelper.LineFeed(2));

            if (!Utils.IsRunningUnderIDE())
                textToPrint.Append(ESCCommandHelper.LeftText());

            var kasir = listOfMesinKasir[0].Pengguna.nama_pengguna;

            textToPrint.Append("Kasir      : ").Append(kasir).Append(ESCCommandHelper.LineFeed(1));
            textToPrint.Append(garisPemisah).Append(ESCCommandHelper.LineFeed(2));

            var isAdaTransaksi = false;

            foreach (var mesin in listOfMesinKasir.Where(f => f.saldo_awal > 0 || (f.jual != null && f.jual.total_nota > 0)))
            {
                isAdaTransaksi = true;

                textToPrint.Append("Login      : ").Append(DateTimeHelper.DateToString(mesin.tanggal_sistem, "dd/MM/yyyy HH:mm:ss")).Append(ESCCommandHelper.LineFeed(1));
                textToPrint.Append("Saldo Awal : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(mesin.saldo_awal), maxFormatNumber)).Append(ESCCommandHelper.LineFeed(1));
                textToPrint.Append(garisPemisah).Append(ESCCommandHelper.LineFeed(1));

                if (mesin.jual.total_nota > 0)
                {
                    foreach (var produk in mesin.item_jual)
                    {
                        textToPrint.Append(StringHelper.FixedLength(produk.nama_produk, garisPemisah.Length)).Append(ESCCommandHelper.LineFeed(1));

                        var jumlah = StringHelper.RightAlignment(produk.jumlah.ToString(), 4);
                        textToPrint.Append(jumlah);

                        textToPrint.Append("  " + StringHelper.FixedLength("x", 3));

                        var harga = StringHelper.RightAlignment(NumberHelper.NumberToString(produk.harga_jual), maxFormatNumber);
                        textToPrint.Append(harga);

                        var diskon = StringHelper.RightAlignment(produk.diskon.ToString(), 7);
                        textToPrint.Append(diskon);

                        var subTotal = produk.jumlah * produk.harga_jual_setelah_diskon;

                        var sSubTotal = StringHelper.RightAlignment(NumberHelper.NumberToString(subTotal), garisPemisah.Length - 26);
                        textToPrint.Append(sSubTotal).Append(ESCCommandHelper.LineFeed(1));
                    }

                    textToPrint.Append(garisPemisah).Append(ESCCommandHelper.LineFeed(1));
                    textToPrint.Append("Total item : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(mesin.item_jual.Count), maxFormatNumber)).Append(ESCCommandHelper.LineFeed(1));
                    textToPrint.Append("Diskon     : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(mesin.jual.diskon), maxFormatNumber)).Append(ESCCommandHelper.LineFeed(1));
                    textToPrint.Append("PPN        : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(mesin.jual.ppn), maxFormatNumber)).Append(ESCCommandHelper.LineFeed(1));
                    textToPrint.Append("Total      : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(mesin.jual.grand_total), maxFormatNumber)).Append(ESCCommandHelper.LineFeed(1));
                    textToPrint.Append(garisPemisah).Append(ESCCommandHelper.LineFeed(1));

                    totalItem += mesin.item_jual.Count;
                    totalDiskon += mesin.jual.diskon;
                    totalPPN += mesin.jual.ppn;
                    grandTotal += mesin.jual.grand_total;
                }
                else
                {
                    textToPrint.Append(">> Belum ada transaksi <<").Append(ESCCommandHelper.LineFeed(1));
                }

                textToPrint.Append(ESCCommandHelper.LineFeed(1));

                totalSaldoAwal += mesin.saldo_awal;
            }

            if (isAdaTransaksi)
            {
                textToPrint.Append("Grand Total:").Append(ESCCommandHelper.LineFeed(1));
                textToPrint.Append("Saldo Awal : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(totalSaldoAwal), maxFormatNumber)).Append(ESCCommandHelper.LineFeed(1));
                textToPrint.Append("Total item : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(totalItem), maxFormatNumber)).Append(ESCCommandHelper.LineFeed(1));
                textToPrint.Append("Diskon     : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(totalDiskon), maxFormatNumber)).Append(ESCCommandHelper.LineFeed(1));
                textToPrint.Append("PPN        : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(totalPPN), maxFormatNumber)).Append(ESCCommandHelper.LineFeed(1));
                textToPrint.Append("Total      : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(grandTotal), maxFormatNumber)).Append(ESCCommandHelper.LineFeed(1));
            }
            else
            {
                textToPrint.Append(">> Belum ada transaksi <<").Append(ESCCommandHelper.LineFeed(1));
            }            

            if (infoCopyright1.Length > 0)
            {
                textToPrint.Append(ESCCommandHelper.LineFeed(1));
                textToPrint.Append(CenterText(infoCopyright1.Length, jumlahKarakter)).Append(infoCopyright1).Append(ESCCommandHelper.LineFeed(1));
                textToPrint.Append(CenterText(infoCopyright2.Length, jumlahKarakter)).Append(infoCopyright2).Append(ESCCommandHelper.LineFeed(1));
            }

            textToPrint.Append(ESCCommandHelper.LineFeed(lineFeed));

            if (!Utils.IsRunningUnderIDE())
            {
                if (autocutCode.Length > 0)
                    textToPrint.Append(ESCCommandHelper.CustomeCode(autocutCode));
            }                

            if (!Utils.IsRunningUnderIDE())
            {
                RawPrintHelper.SendStringToPrinter(_printerName, textToPrint.ToString());
            }
            else
            {
                RawPrintHelper.SendStringToFile(textToPrint.ToString());
            }
        }

        public void Cetak(JualProduk jual, IList<HeaderNotaMiniPos> listOfHeaderNota, IList<FooterNotaMiniPos> listOfFooterNota,
            int jumlahKarakter, int lineFeed, bool isCetakCustomer = true, bool isCetakKeteranganNota = true, int ukuranFont = 0,
            string autocutCode = "", string openCashDrawerCode = "", string infoCopyright1 = "", string infoCopyright2 = "")
        {
            var garisPemisah = StringHelper.PrintChar('=', jumlahKarakter);

            var textToPrint = new StringBuilder();

            if (!Utils.IsRunningUnderIDE())
            {
                textToPrint.Append(ESCCommandHelper.InitializePrinter());

                if (ukuranFont > 0)
                    textToPrint.Append(ESCCommandHelper.FontNormal(ukuranFont));
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

                    textToPrint.Append(CenterText(header.keterangan.Length, jumlahKarakter)).Append(header.keterangan).Append(ESCCommandHelper.LineFeed(1));
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

            if (jual.tanggal_tempo != null)
            {
                textToPrint.Append("Tempo  : ").Append(DateTimeHelper.DateToString(jual.tanggal_tempo)).Append(ESCCommandHelper.LineFeed(1));
            }            

            if (isCetakCustomer)
            {
                textToPrint.Append(ESCCommandHelper.LineFeed(1));

                // cetak info customer
                textToPrint.Append("Kepada: ").Append(ESCCommandHelper.LineFeed(1));

                var namaCustomer = jual.is_sdac == true ? jual.Customer.nama_customer : jual.kirim_kepada;
                var alamat = jual.is_sdac == true ? jual.Customer.alamat.NullToString() : jual.kirim_alamat.NullToString();
                var telepon = jual.is_sdac == true ? jual.Customer.telepon.NullToString() : jual.kirim_telepon.NullToString();

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

                if (item.keterangan.Length > 0)
                    textToPrint.Append(item.keterangan).Append(ESCCommandHelper.LineFeed(1));

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

            // cetak footer
            foreach (var footer in listOfFooterNota)
            {
                if (footer.keterangan.Length > 0)
                {
                    if (footer.keterangan.Length > garisPemisah.Length)
                    {
                        footer.keterangan = StringHelper.FixedLength(footer.keterangan, garisPemisah.Length);
                    }
                    
                    textToPrint.Append(CenterText(footer.keterangan.Length, jumlahKarakter)).Append(footer.keterangan).Append(ESCCommandHelper.LineFeed(1));
                }
            }

            if (infoCopyright1.Length > 0)
            {
                textToPrint.Append(ESCCommandHelper.LineFeed(1));
                textToPrint.Append(CenterText(infoCopyright1.Length, jumlahKarakter)).Append(infoCopyright1).Append(ESCCommandHelper.LineFeed(1));
                textToPrint.Append(CenterText(infoCopyright2.Length, jumlahKarakter)).Append(infoCopyright2).Append(ESCCommandHelper.LineFeed(1));
            }

            textToPrint.Append(ESCCommandHelper.LineFeed(lineFeed));

            if (!Utils.IsRunningUnderIDE())
            {
                if (autocutCode.Length > 0)
                    textToPrint.Append(ESCCommandHelper.CustomeCode(autocutCode));

                if (openCashDrawerCode.Length > 0)
                    textToPrint.Append(ESCCommandHelper.CustomeCode(openCashDrawerCode));
            }

            if (!Utils.IsRunningUnderIDE())
            {
                RawPrintHelper.SendStringToPrinter(_printerName, textToPrint.ToString());
            }
            else
            {
                RawPrintHelper.SendStringToFile(textToPrint.ToString());
            }
        }

        private string CenterText(int panjangString, int jumlahKarakter)
        {
            var div = (double)(jumlahKarakter - panjangString) / 2;
            var posisiTengah = Math.Ceiling(div);
            var result = StringHelper.PrintChar(' ', Convert.ToInt32(posisiTengah));

            return result;
        }
    }
}
