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
using System.IO.Ports;
using GodSharp;

namespace OpenRetail.App.Cashier.Pengaturan
{
    public partial class FrmPengaturanUmum : FrmEntryStandard
    {        
        private IList<AdvancedTextbox> _listOfTxtHeaderNota = new List<AdvancedTextbox>();
        private IList<AdvancedTextbox> _listOfTxtFooterNota = new List<AdvancedTextbox>();
        private PengaturanUmum _pengaturanUmum = null;
        private SettingPort _settingPort = null;
        private SettingCustomerDisplay _settingCustomerDisplay = null;

        public FrmPengaturanUmum(string header, PengaturanUmum pengaturanUmum, 
            SettingPort settingPort, SettingCustomerDisplay settingCustomerDisplay) : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._pengaturanUmum = pengaturanUmum;
            this._settingPort = settingPort;
            this._settingCustomerDisplay = settingCustomerDisplay;

            SetInfoPrinter();
            SetInfoPort(_settingPort.portNumber);
            SetInfoCustomerDisplay();
            LoadHeaderNota();
            LoadFooterNota();
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

        private void SetInfoPort(string defaultPort)
        {
            cmbPort.Items.Clear();
            foreach (var port in SerialPort.GetPortNames())
            {
                cmbPort.Items.Add(port);
            }

            if (cmbPort.Items.Count == 0)
            {
                for (int i = 1; i < 10; i++)
                {
                    cmbPort.Items.Add(string.Format("COM{0}", i.ToString()));
                }
            }

            if (defaultPort.Length > 0)
                cmbPort.Text = defaultPort;
            else
            {
                if (cmbPort.Items.Count > 0)
                    cmbPort.SelectedIndex = 0;
            }            
        }

        private void SetInfoCustomerDisplay()
        {
            chkIsActiveCustomerDisplay.Checked = _settingCustomerDisplay.is_active_customer_display;
            txtKalimatPembukaBaris1.Text = _settingCustomerDisplay.opening_sentence_line1;
            txtKalimatPembukaBaris2.Text = _settingCustomerDisplay.opening_sentence_line2;
            txtKalimatPenutupBaris1.Text = _settingCustomerDisplay.closing_sentence_line1;
            txtKalimatPenutupBaris2.Text = _settingCustomerDisplay.closing_sentence_line2;
            updTampilKalimatPenutup.Value = _settingCustomerDisplay.delay_display_closing_sentence;
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

            if (rdoJenisPrinterMiniPOS.Checked)
            {
                chkUkuranFont.Checked = _pengaturanUmum.ukuran_font > 0;
                txtUkuranFont.Text = _pengaturanUmum.ukuran_font.ToString();
                txtUkuranFont.Enabled = chkUkuranFont.Checked;

                chkAutocut.Checked = _pengaturanUmum.is_autocut;
                chkOpenCashDrawer.Checked = _pengaturanUmum.is_open_cash_drawer;
            }
        }

        protected override void Simpan()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                // simpan pengaturan lokal (app.config)
                SimpanPengaturanLokal();

                // simpan header nota
                SimpanHeaderNota();

                // simpan footer nota minipos
                SimpanFooterNota();

                this.Close();    
            }            
        }

        /// <summary>
        /// Simpan pengaturan aplikasi di masing-masing pc (app.config)
        /// </summary>
        private void SimpanPengaturanLokal()
        {
            var appConfigFile = string.Format("{0}\\OpenRetailCashier.exe.config", Utils.GetAppPath());

            _pengaturanUmum.nama_printer = cmbPrinter.Text;
            _pengaturanUmum.is_auto_print = chkCetakOtomatis.Checked;

            var jenisPrinter = JenisPrinter.InkJet;

            if (rdoJenisPrinterDotMatrix.Checked)
                jenisPrinter = JenisPrinter.DotMatrix;
            else if (rdoJenisPrinterMiniPOS.Checked)
                jenisPrinter = JenisPrinter.MiniPOS;

            _pengaturanUmum.jenis_printer = jenisPrinter;
            _pengaturanUmum.jumlah_karakter = Convert.ToInt32(txtJumlahKarakter.Text);
            _pengaturanUmum.jumlah_gulung = Convert.ToInt32(txtJumlahGulung.Text);
            _pengaturanUmum.ukuran_font = Convert.ToInt32(txtUkuranFont.Text);
            _pengaturanUmum.is_autocut = chkAutocut.Checked;
            _pengaturanUmum.is_open_cash_drawer = chkOpenCashDrawer.Checked;

            // simpan info printer
            AppConfigHelper.SaveValue("printerName", _pengaturanUmum.nama_printer, appConfigFile);
            AppConfigHelper.SaveValue("isAutoPrinter", _pengaturanUmum.is_auto_print.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("jenis_printer", Convert.ToString((int)jenisPrinter), appConfigFile);

            // simpan info printer mini pos
            AppConfigHelper.SaveValue("jumlahKarakter", txtJumlahKarakter.Text, appConfigFile);
            AppConfigHelper.SaveValue("jumlahGulung", txtJumlahGulung.Text, appConfigFile);
            AppConfigHelper.SaveValue("ukuranFont", txtUkuranFont.Text, appConfigFile);
            AppConfigHelper.SaveValue("isAutocut", chkAutocut.Checked.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("autocutCode", _pengaturanUmum.autocut_code, appConfigFile);
            AppConfigHelper.SaveValue("isOpenCashDrawer", chkOpenCashDrawer.Checked.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("openCashDrawerCode", _pengaturanUmum.open_cash_drawer_code, appConfigFile);

            // simpan setting port
            _settingPort.portNumber = cmbPort.Text;
            AppConfigHelper.SaveValue("portNumber", cmbPort.Text, appConfigFile);

            // simpan setting customer display
            _settingCustomerDisplay.is_active_customer_display = chkIsActiveCustomerDisplay.Checked;
            _settingCustomerDisplay.opening_sentence_line1 = txtKalimatPembukaBaris1.Text;
            _settingCustomerDisplay.opening_sentence_line2 = txtKalimatPembukaBaris2.Text;
            _settingCustomerDisplay.closing_sentence_line1 = txtKalimatPenutupBaris1.Text;
            _settingCustomerDisplay.closing_sentence_line2 = txtKalimatPenutupBaris2.Text;
            _settingCustomerDisplay.delay_display_closing_sentence = (int)updTampilKalimatPenutup.Value;

            AppConfigHelper.SaveValue("isActiveCustomerDisplay", _settingCustomerDisplay.is_active_customer_display.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("customerDisplayOpeningSentenceLine1", _settingCustomerDisplay.opening_sentence_line1, appConfigFile);
            AppConfigHelper.SaveValue("customerDisplayOpeningSentenceLine2", _settingCustomerDisplay.opening_sentence_line2, appConfigFile);
            AppConfigHelper.SaveValue("customerDisplayClosingSentenceLine1", _settingCustomerDisplay.closing_sentence_line1, appConfigFile);
            AppConfigHelper.SaveValue("customerDisplayClosingSentenceLine2", _settingCustomerDisplay.closing_sentence_line2, appConfigFile);
            AppConfigHelper.SaveValue("customerDisplayDelayDisplayClosingSentence", _settingCustomerDisplay.delay_display_closing_sentence.ToString(), appConfigFile);
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
            chkUkuranFont.Enabled = false;
            chkUkuranFont.Checked = false;
            txtUkuranFont.Enabled = false;
            txtUkuranFont.Text = "0";

            chkAutocut.Enabled = false;
            chkAutocut.Checked = false;
            chkOpenCashDrawer.Enabled = false;
            chkOpenCashDrawer.Checked = false;

            btnShowAutocutCode.Enabled = false;
            btnShowOpenCashDrawerCode.Enabled = false;

            groupBox4.Enabled = false;            
        }

        private void rdoJenisPrinterMiniPOS_CheckedChanged(object sender, EventArgs e)
        {
            txtJumlahKarakter.Enabled = true;
            txtJumlahKarakter.BackColor = Color.White;

            txtJumlahGulung.Enabled = true;
            txtJumlahGulung.BackColor = Color.White;

            chkUkuranFont.Enabled = true;
            chkUkuranFont.Checked = _pengaturanUmum.ukuran_font > 0;

            chkAutocut.Enabled = true;
            chkAutocut.Checked = _pengaturanUmum.is_autocut;

            chkOpenCashDrawer.Enabled = true;
            chkOpenCashDrawer.Checked = _pengaturanUmum.is_open_cash_drawer;

            groupBox4.Enabled = true;

        }

        private void chkUkuranFont_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            txtUkuranFont.Enabled = chk.Checked;

            txtUkuranFont.Text = "0";
            if (chk.Checked)
                txtUkuranFont.Text = _pengaturanUmum.ukuran_font.ToString();
        }

        private void chkAutocut_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            btnShowAutocutCode.Enabled = chk.Checked;
        }

        private void chkOpenCashDrawer_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            btnShowOpenCashDrawerCode.Enabled = chk.Checked;
        }

        private void btnShowAutocutCode_Click(object sender, EventArgs e)
        {
            var frm = new FrmEntryCustomeCode("Edit Kode Autocut", _pengaturanUmum, true);
            frm.ShowDialog();
        }

        private void btnShowOpenCashDrawerCode_Click(object sender, EventArgs e)
        {
            var frm = new FrmEntryCustomeCode("Edit Kode Open Cash Drawer", _pengaturanUmum, false);
            frm.ShowDialog();
        }

        private void btnTesKoneksi_Click(object sender, EventArgs e)
        {
            const int MAX_LENGTH = 20;

            var appName = "OpenRetail Cashier";
            var version = string.Format("v{0}", MainProgram.currentVersion);

            var displayLine1 = string.Format("{0}{1}", StringHelper.CenterAlignment(appName.Length, MAX_LENGTH), appName);
            var displayLine2 = string.Format("{0}{1}", StringHelper.CenterAlignment(version.Length, MAX_LENGTH), version);

            System.Diagnostics.Debug.Print("displayLine1: {0}", displayLine1);
            System.Diagnostics.Debug.Print("displayLine2: {0}", displayLine2);

            if (!Utils.IsRunningUnderIDE())
            {
                GodSerialPort serialPort = null;

                if (!GodSerialPortHelper.IsConnected(serialPort, _settingPort))
                {
                    MsgHelper.MsgWarning("Koneksi ke customer display, silahkan coba port yang lain.");
                    return;
                }

                GodSerialPortHelper.SendStringToCustomerDisplay(displayLine1, displayLine2, serialPort);
            }
        }

        private void chkIsActiveCustomerDisplay_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;

            cmbPort.Enabled = chk.Checked;
            btnTesKoneksi.Enabled = chk.Checked;
            grpKalimatPembuka.Enabled = chk.Checked;
            grpKalimatPenutup.Enabled = chk.Checked;
        }
    }
}
