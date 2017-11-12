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

using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Helper;
using ConceptCave.WaitCursor;
using OpenRetail.Helper.UserControl;
using System.Drawing.Printing;

namespace OpenRetail.App.Cashier.Pengaturan
{
    public partial class FrmPengaturanUmum : FrmEntryStandard
    {
        private IList<AdvancedTextbox> _listOfTxtHeaderNota = new List<AdvancedTextbox>();
        private IList<AdvancedTextbox> _listOfTxtFooterNota = new List<AdvancedTextbox>();
        private PengaturanUmum _pengaturanUmum = null;
        
        public FrmPengaturanUmum(string header, PengaturanUmum pengaturanUmum)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._pengaturanUmum = pengaturanUmum;            
            
            SetInfoPrinter();
            LoadHeaderNota();
            LoadFooterNota();
            LoadSettingLainnya();
        }

        private void LoadHeaderNota()
        {
            _listOfTxtHeaderNota.Add(txtHeaderMiniPOS1);
            _listOfTxtHeaderNota.Add(txtHeaderMiniPOS2);
            _listOfTxtHeaderNota.Add(txtHeaderMiniPOS3);
            _listOfTxtHeaderNota.Add(txtHeaderMiniPOS4);
            _listOfTxtHeaderNota.Add(txtHeaderMiniPOS5);

            IHeaderNotaMiniPosBll bll = new HeaderNotaMiniPosBll();
            var listOfHeaderNota = bll.GetAll();

            var index = 0;
            foreach (var item in listOfHeaderNota)
            {
                var txtHeader = _listOfTxtHeaderNota[index];
                txtHeader.Tag = item.header_nota_id;
                txtHeader.Text = item.keterangan;

                index++;
            }
        }

        private void LoadFooterNota()
        {
            _listOfTxtFooterNota.Add(txtFooterMiniPOS1);
            _listOfTxtFooterNota.Add(txtFooterMiniPOS2);
            _listOfTxtFooterNota.Add(txtFooterMiniPOS3);

            IFooterNotaMiniPosBll bll = new FooterNotaMiniPosBll();
            var listOfFooterNota = bll.GetAll();

            var index = 0;
            foreach (var item in listOfFooterNota)
            {
                var txtFooter = _listOfTxtFooterNota[index];
                txtFooter.Tag = item.footer_nota_id;
                txtFooter.Text = item.keterangan;

                index++;
            }
        }

        private void LoadSettingLainnya()
        {
            chkStokProdukBolehMinus.Checked = _pengaturanUmum.is_stok_produk_boleh_minus;
        }

        private void LoadPrinter(string defaultPrinter)
        {
            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                cmbPrinter.Items.Add(printer);
            }

            if (defaultPrinter.Length > 0)
                cmbPrinter.Text = defaultPrinter;
            else
            {
                if (cmbPrinter.Items.Count > 0)
                    cmbPrinter.SelectedIndex = 0;
            }
        }

        private void SetInfoPrinter()
        {
            // setting general
            LoadPrinter(this._pengaturanUmum.nama_printer);
            chkCetakOtomatis.Checked = this._pengaturanUmum.is_auto_print;

            switch (this._pengaturanUmum.jenis_printer)
            {
                case JenisPrinter.DotMatrix:
                    rdoJenisPrinterDotMatrix.Checked = true;
                    break;

                case JenisPrinter.MiniPOS:
                    rdoJenisPrinterMiniPOS.Checked = true;
                    break;

                default:
                    break;
            }

            // setting khusus printer mini pos            
            txtJumlahKarakter.Text = _pengaturanUmum.jumlah_karakter.ToString();
            txtJumlahGulung.Text = _pengaturanUmum.jumlah_gulung.ToString();
        }

        protected override void Simpan()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _pengaturanUmum.nama_printer = cmbPrinter.Text;
                _pengaturanUmum.is_auto_print = chkCetakOtomatis.Checked;
                _pengaturanUmum.is_stok_produk_boleh_minus = chkStokProdukBolehMinus.Checked;

                var jenisPrinter = JenisPrinter.InkJet;

                if (rdoJenisPrinterDotMatrix.Checked)
                    jenisPrinter = JenisPrinter.DotMatrix;
                else if (rdoJenisPrinterMiniPOS.Checked)
                    jenisPrinter = JenisPrinter.MiniPOS;

                _pengaturanUmum.jenis_printer = jenisPrinter;
                _pengaturanUmum.jumlah_karakter = Convert.ToInt32(txtJumlahKarakter.Text);
                _pengaturanUmum.jumlah_gulung = Convert.ToInt32(txtJumlahGulung.Text);

                var appConfigFile = string.Format("{0}\\OpenRetailCashier.exe.config", Utils.GetAppPath());

                AppConfigHelper.SaveValue("isStokProdukBolehMinus", chkStokProdukBolehMinus.Checked.ToString(), appConfigFile);

                // simpan info printer
                AppConfigHelper.SaveValue("printerName", cmbPrinter.Text, appConfigFile);
                AppConfigHelper.SaveValue("isAutoPrinter", chkCetakOtomatis.Checked.ToString(), appConfigFile);
                AppConfigHelper.SaveValue("jenis_printer", Convert.ToString((int)jenisPrinter), appConfigFile);

                // simpan info printer mini pos
                AppConfigHelper.SaveValue("jumlahKarakter", txtJumlahKarakter.Text, appConfigFile);
                AppConfigHelper.SaveValue("jumlahGulung", txtJumlahGulung.Text, appConfigFile);

                // simpan header nota
                SimpanHeaderNota();

                // simpan footer nota minipos
                SimpanFooterNota();

                this.Close();    
            }            
        }

        private void SimpanHeaderNota()
        {
            IHeaderNotaMiniPosBll headerNotaBll = new HeaderNotaMiniPosBll();

            var index = 0;
            foreach (var item in _listOfTxtHeaderNota)
            {
                var headerNota = new HeaderNotaMiniPos
                {
                    header_nota_id = item.Tag.ToString(),
                    keterangan = item.Text
                };

                var result = headerNotaBll.Update(headerNota);
                if (result > 0)
                {
                    _pengaturanUmum.list_of_header_nota_mini_pos[index].header_nota_id = headerNota.header_nota_id;
                    _pengaturanUmum.list_of_header_nota_mini_pos[index].keterangan = headerNota.keterangan;
                }

                index++;
            }
        }

        private void SimpanFooterNota()
        {
            IFooterNotaMiniPosBll footerNotaBll = new FooterNotaMiniPosBll();

            var index = 0;
            foreach (var item in _listOfTxtFooterNota)
            {
                var footerNota = new FooterNotaMiniPos
                {
                    footer_nota_id = item.Tag.ToString(),
                    keterangan = item.Text
                };

                var result = footerNotaBll.Update(footerNota);
                if (result > 0)
                {
                    _pengaturanUmum.list_of_footer_nota_mini_pos[index].footer_nota_id = footerNota.footer_nota_id;
                    _pengaturanUmum.list_of_footer_nota_mini_pos[index].keterangan = footerNota.keterangan;
                }

                index++;
            }
        }

        private void rdoJenisPrinterDotMatrix_CheckedChanged(object sender, EventArgs e)
        {
            txtJumlahKarakter.Enabled = false;
            txtJumlahGulung.Enabled = false;
            groupBox4.Enabled = false;  
        }

        private void rdoJenisPrinterMiniPOS_CheckedChanged(object sender, EventArgs e)
        {
            txtJumlahKarakter.Enabled = true;
            txtJumlahGulung.Enabled = true;
            groupBox4.Enabled = true;
        }
    }
}
