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
using OpenRetail.Helper;
using GodSharp;

namespace OpenRetail.App.Transaksi
{
    public partial class FrmEntryPenjualanProduk
    {
        private const int MAX_LENGTH = 20; // maksimal karakter customer display

        private void DisplayItemProduct(ItemJualProduk itemJual)
        {
            var produk = itemJual.Produk;

            var jumlah = itemJual.jumlah - itemJual.jumlah_retur;
            var hargaJual = itemJual.harga_setelah_diskon;

            if (produk != null)
            {
                if (!(hargaJual > 0))
                {
                    double diskon = itemJual.diskon;
                    double diskonRupiah = 0;

                    if (!(diskon > 0))
                    {
                        if (_customer != null)
                            diskon = _customer.diskon;

                        if (!(diskon > 0))
                        {
                            var diskonProduk = GetDiskonJualFix(produk, jumlah, produk.diskon);
                            diskon = diskonProduk > 0 ? diskonProduk : produk.Golongan.diskon;
                        }
                    }

                    hargaJual = GetHargaJualFix(produk, jumlah, produk.harga_jual);

                    diskonRupiah = diskon / 100 * hargaJual;
                    hargaJual -= diskonRupiah;
                }
            }

            var subTotal = StringHelper.RightAlignment(Convert.ToString(jumlah * hargaJual), MAX_LENGTH - (jumlah.ToString().Length + hargaJual.ToString().Length + 1));

            var displayLine1 = StringHelper.FixedLength(produk.nama_produk, MAX_LENGTH);
            var displayLine2 = string.Format("{0}x{1}{2}", jumlah, hargaJual, subTotal);

            System.Diagnostics.Debug.Print("displayLine1: {0}", displayLine1);
            System.Diagnostics.Debug.Print("displayLine2: {0}", displayLine2);

            if (!Utils.IsRunningUnderIDE() && _settingCustomerDisplay.is_active_customer_display)
            {
                GodSerialPort serialPort = null;

                if (!GodSerialPortHelper.IsConnected(serialPort, _settingPort))
                {
                    return;
                }

                GodSerialPortHelper.SendStringToCustomerDisplay(displayLine1, displayLine2, serialPort);
            }
        }

        private void DisplayKalimatPembuka()
        {
            var displayLine1 = string.Format("{0}{1}", StringHelper.CenterAlignment(_settingCustomerDisplay.opening_sentence_line1.Length, MAX_LENGTH),
                _settingCustomerDisplay.opening_sentence_line1);

            var displayLine2 = string.Format("{0}{1}", StringHelper.CenterAlignment(_settingCustomerDisplay.opening_sentence_line2.Length, MAX_LENGTH),
                _settingCustomerDisplay.opening_sentence_line2);

            System.Diagnostics.Debug.Print("displayLine1: {0}", displayLine1);
            System.Diagnostics.Debug.Print("displayLine2: {0}", displayLine2);

            if (!Utils.IsRunningUnderIDE() && _settingCustomerDisplay.is_active_customer_display)
            {
                GodSerialPort serialPort = null;

                if (!GodSerialPortHelper.IsConnected(serialPort, _settingPort))
                {
                    return;
                }

                GodSerialPortHelper.SendStringToCustomerDisplay(displayLine1, displayLine2, serialPort);
            }
        }

        private void DisplayKalimatPenutup()
        {
            var displayLine1 = string.Format("{0}{1}", StringHelper.CenterAlignment(_settingCustomerDisplay.closing_sentence_line1.Length, MAX_LENGTH),
                _settingCustomerDisplay.closing_sentence_line1);

            var displayLine2 = string.Format("{0}{1}", StringHelper.CenterAlignment(_settingCustomerDisplay.closing_sentence_line2.Length, MAX_LENGTH),
                _settingCustomerDisplay.closing_sentence_line2);

            System.Diagnostics.Debug.Print("displayLine1: {0}", displayLine1);
            System.Diagnostics.Debug.Print("displayLine2: {0}", displayLine2);

            if (!Utils.IsRunningUnderIDE() && _settingCustomerDisplay.is_active_customer_display)
            {
                GodSerialPort serialPort = null;

                if (!GodSerialPortHelper.IsConnected(serialPort, _settingPort))
                {
                    return;
                }

                GodSerialPortHelper.SendStringToCustomerDisplay(displayLine1, displayLine2, serialPort);
            }
        }

        private void DisplayTotal(string total)
        {
            var displayLine1 = "Total";
            var displayLine2 = StringHelper.RightAlignment(total, MAX_LENGTH);

            System.Diagnostics.Debug.Print("displayLine1: {0}", displayLine1);
            System.Diagnostics.Debug.Print("displayLine2: {0}", displayLine2);

            if (!Utils.IsRunningUnderIDE() && _settingCustomerDisplay.is_active_customer_display)
            {
                GodSerialPort serialPort = null;

                if (!GodSerialPortHelper.IsConnected(serialPort, _settingPort))
                {
                    return;
                }

                GodSerialPortHelper.SendStringToCustomerDisplay(displayLine1, displayLine2, serialPort);
            }
        }

        private void DisplayKembalian(string kembalian)
        {
            var displayLine1 = "Kembalian";
            var displayLine2 = StringHelper.RightAlignment(kembalian, MAX_LENGTH);

            System.Diagnostics.Debug.Print("displayLine1: {0}", displayLine1);
            System.Diagnostics.Debug.Print("displayLine2: {0}", displayLine2);

            if (!Utils.IsRunningUnderIDE() && _settingCustomerDisplay.is_active_customer_display)
            {
                GodSerialPort serialPort = null;

                if (!GodSerialPortHelper.IsConnected(serialPort, _settingPort))
                {
                    return;
                }

                GodSerialPortHelper.SendStringToCustomerDisplay(displayLine1, displayLine2, serialPort);
            }
        }
    }
}
