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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenRetail.Helper;
using OpenRetail.Model;
using OpenRetail.Model.Report;
using OpenRetail.Bll.Api.Report;
using OpenRetail.Bll.Service.Report;
using log4net;
using OpenRetail.Helper.RAWPrinting;

namespace OpenRetail.App.Cashier.Laporan
{
    public partial class FrmLapPenjualan : Form
    {
        private ILog _log;
        private PengaturanUmum _pengaturanUmum = null;
        private Pengguna _pengguna;
        private IList<ReportMesinKasir> _listOfMesinKasir;

        public FrmLapPenjualan(string header, Pengguna pengguna, PengaturanUmum pengaturanUmum)
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            this._log = MainProgram.log;
            this._pengguna = pengguna;
            this._pengaturanUmum = pengaturanUmum;            

            this.Text = header;
            this.lblHeader.Text = header;

            txtOutput.Text = GenerateReport();
        }        

        private string GenerateReport()
        {
            var txtOutput = new StringBuilder();
            var garisPemisah = StringHelper.PrintChar('=', 40);
            
            var totalSaldoAwal = 0d;
            var totalItem = 0;
            var totalDiskon = 0d;
            var totalPPN = 0d;
            var grandTotal = 0d;
            var maxFormatNumber = 10;

            txtOutput.Append("Laporan Penjualan Per Kasir").Append(Environment.NewLine);
            txtOutput.Append("Per tanggal: ").Append(DateTimeHelper.DateToString(DateTime.Today)).Append(Environment.NewLine).Append(Environment.NewLine);

            txtOutput.Append("Kasir      : ").Append(_pengguna.nama_pengguna).Append(Environment.NewLine);
            txtOutput.Append(garisPemisah).Append(Environment.NewLine).Append(Environment.NewLine);

            IReportMesinKasirBll bll = new ReportMesinKasirBll(_log);

            var isAdaTransaksi = false;

            _listOfMesinKasir = bll.PerKasirGetByPenggunaId(_pengguna.pengguna_id);
            foreach (var mesin in _listOfMesinKasir.Where(f => f.saldo_awal > 0 || (f.jual != null && f.jual.total_nota > 0)))
            {
                isAdaTransaksi = true;

                txtOutput.Append("Login      : ").Append(DateTimeHelper.DateToString(mesin.tanggal_sistem, "dd/MM/yyyy HH:mm:ss")).Append(Environment.NewLine);
                txtOutput.Append("Saldo Awal : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(mesin.saldo_awal), maxFormatNumber)).Append(Environment.NewLine);
                txtOutput.Append(garisPemisah).Append(Environment.NewLine);

                if (mesin.jual.total_nota > 0)
                {
                    foreach (var produk in mesin.item_jual)
                    {
                        txtOutput.Append(StringHelper.FixedLength(produk.nama_produk, garisPemisah.Length)).Append(Environment.NewLine);

                        var jumlah = StringHelper.RightAlignment(produk.jumlah.ToString(), 4);
                        txtOutput.Append(jumlah);

                        txtOutput.Append("  " + StringHelper.FixedLength("x", 3));

                        var harga = StringHelper.RightAlignment(NumberHelper.NumberToString(produk.harga_jual), maxFormatNumber);
                        txtOutput.Append(harga);

                        var diskon = StringHelper.RightAlignment(produk.diskon.ToString(), 7);
                        txtOutput.Append(diskon);

                        var subTotal = produk.jumlah * produk.harga_jual_setelah_diskon;

                        var sSubTotal = StringHelper.RightAlignment(NumberHelper.NumberToString(subTotal), garisPemisah.Length - 26);
                        txtOutput.Append(sSubTotal).Append(Environment.NewLine);
                    }

                    txtOutput.Append(garisPemisah).Append(Environment.NewLine);
                    txtOutput.Append("Total item : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(mesin.item_jual.Count), maxFormatNumber)).Append(Environment.NewLine);
                    txtOutput.Append("Diskon     : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(mesin.jual.diskon), maxFormatNumber)).Append(Environment.NewLine);
                    txtOutput.Append("PPN        : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(mesin.jual.ppn), maxFormatNumber)).Append(Environment.NewLine);
                    txtOutput.Append("Total      : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(mesin.jual.grand_total), maxFormatNumber)).Append(Environment.NewLine);
                    txtOutput.Append(garisPemisah).Append(Environment.NewLine);

                    totalItem += mesin.item_jual.Count;
                    totalDiskon += mesin.jual.diskon;
                    totalPPN += mesin.jual.ppn;
                    grandTotal += mesin.jual.grand_total;
                }
                else
                {
                    txtOutput.Append(">> Belum ada transaksi <<").Append(Environment.NewLine);
                }

                txtOutput.Append(Environment.NewLine);
                totalSaldoAwal += mesin.saldo_awal;
            }

            if (isAdaTransaksi)
            {
                txtOutput.Append("Grand Total:").Append(Environment.NewLine);
                txtOutput.Append("Saldo Awal : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(totalSaldoAwal), maxFormatNumber)).Append(Environment.NewLine);
                txtOutput.Append("Total item : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(totalItem), maxFormatNumber)).Append(Environment.NewLine);
                txtOutput.Append("Diskon     : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(totalDiskon), maxFormatNumber)).Append(Environment.NewLine);
                txtOutput.Append("PPN        : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(totalPPN), maxFormatNumber)).Append(Environment.NewLine);
                txtOutput.Append("Total      : ").Append(StringHelper.RightAlignment(NumberHelper.NumberToString(grandTotal), maxFormatNumber)).Append(Environment.NewLine);
            }
            else
            {
                txtOutput.Append(">> Belum ada transaksi <<").Append(Environment.NewLine);
            }            

            return txtOutput.ToString();
        }

        private void CetakLaporan()
        {
            if (MsgHelper.MsgKonfirmasi("Apakah proses pencetakan ingin dilanjutkan ?"))
            {
                IRAWPrinting printerMiniPos = new PrinterMiniPOS(_pengaturanUmum.nama_printer);
                printerMiniPos.Cetak(_listOfMesinKasir, _pengaturanUmum.list_of_header_nota_mini_pos, _pengaturanUmum.jumlah_karakter, 
                    _pengaturanUmum.jumlah_gulung, ukuranFont: _pengaturanUmum.ukuran_font);

                this.Close();
            }            
        }

        private void btnCetak_Click(object sender, EventArgs e)
        {
            CetakLaporan();
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmLapPenjualan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEsc(e))
                this.Close();
        }

        private void FrmLapPenjualan_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F10:
                    e.SuppressKeyPress = true;
                    break;

                case Keys.F11:
                    if (btnCetak.Enabled)
                        CetakLaporan();

                    break;

                default:
                    break;
            }
        }
    }
}
