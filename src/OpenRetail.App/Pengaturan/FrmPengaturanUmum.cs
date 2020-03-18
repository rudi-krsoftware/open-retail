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

using ConceptCave.WaitCursor;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.Helper;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Helper.UserControl;
using OpenRetail.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

namespace OpenRetail.App.Pengaturan
{
    public partial class FrmPengaturanUmum : FrmEntryStandard
    {
        private IList<AdvancedTextbox> _listOfTxtHeaderNota = new List<AdvancedTextbox>();
        private IList<AdvancedTextbox> _listOfTxtHeaderNotaMiniPOS = new List<AdvancedTextbox>();
        private IList<AdvancedTextbox> _listOfTxtFooterNotaMiniPOS = new List<AdvancedTextbox>();
        private IList<AdvancedTextbox> _listOfTxtLabelNota = new List<AdvancedTextbox>();
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
            LoadHeaderNotaMiniPOS();
            LoadFooterNotaMinniPOS();
            LoadLabelNota();
            LoadSettingLainnya();
        }

        private void LoadHeaderNota()
        {
            _listOfTxtHeaderNota.Add(txtHeader1);
            _listOfTxtHeaderNota.Add(txtHeader2);
            _listOfTxtHeaderNota.Add(txtHeader3);
            _listOfTxtHeaderNota.Add(txtHeader4);
            _listOfTxtHeaderNota.Add(txtHeader5);

            IHeaderNotaBll bll = new HeaderNotaBll();
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

        private void LoadHeaderNotaMiniPOS()
        {
            _listOfTxtHeaderNotaMiniPOS.Add(txtHeaderMiniPOS1);
            _listOfTxtHeaderNotaMiniPOS.Add(txtHeaderMiniPOS2);
            _listOfTxtHeaderNotaMiniPOS.Add(txtHeaderMiniPOS3);
            _listOfTxtHeaderNotaMiniPOS.Add(txtHeaderMiniPOS4);
            _listOfTxtHeaderNotaMiniPOS.Add(txtHeaderMiniPOS5);

            IHeaderNotaMiniPosBll bll = new HeaderNotaMiniPosBll();
            var listOfHeaderNota = bll.GetAll();

            var index = 0;
            foreach (var item in listOfHeaderNota)
            {
                var txtHeader = _listOfTxtHeaderNotaMiniPOS[index];
                txtHeader.Tag = item.header_nota_id;
                txtHeader.Text = item.keterangan;

                index++;
            }
        }

        private void LoadFooterNotaMinniPOS()
        {
            _listOfTxtFooterNotaMiniPOS.Add(txtFooterMiniPOS1);
            _listOfTxtFooterNotaMiniPOS.Add(txtFooterMiniPOS2);
            _listOfTxtFooterNotaMiniPOS.Add(txtFooterMiniPOS3);

            IFooterNotaMiniPosBll bll = new FooterNotaMiniPosBll();
            var listOfFooterNota = bll.GetAll();

            var index = 0;
            foreach (var item in listOfFooterNota)
            {
                var txtFooter = _listOfTxtFooterNotaMiniPOS[index];
                txtFooter.Tag = item.footer_nota_id;
                txtFooter.Text = item.keterangan;

                index++;
            }
        }

        private void LoadLabelNota()
        {
            _listOfTxtLabelNota.Add(txtDari1);
            _listOfTxtLabelNota.Add(txtDari2);
            _listOfTxtLabelNota.Add(txtDari3);

            ILabelNotaBll bll = new LabelNotaBll();
            var listOfLabelNota = bll.GetAll();

            var index = 0;
            foreach (var item in listOfLabelNota)
            {
                var txtDari = _listOfTxtLabelNota[index];
                txtDari.Tag = item.label_nota_id;
                txtDari.Text = item.keterangan;

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
                    rdoJenisPrinterInkJet.Checked = true;
                    break;
            }

            // setting khusus printer mini pos
            chkCetakCustomer.Checked = _pengaturanUmum.is_cetak_customer;
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

        private void LoadSettingLainnya()
        {
            chkTampilkanInfoMinimalStokProduk.Checked = _pengaturanUmum.is_show_minimal_stok;
            chkCustomerWajibDiisi.Checked = _pengaturanUmum.is_customer_required;
            chkCetakKeteranganNota.Checked = _pengaturanUmum.is_cetak_keterangan_nota;
            chkStokProdukBolehMinus.Checked = _pengaturanUmum.is_stok_produk_boleh_minus;
            chkFokusKeKolomJumlah.Checked = _pengaturanUmum.is_fokus_input_kolom_jumlah;
            chkUpdateHargaJual.Checked = _pengaturanUmum.is_update_harga_jual;
            chkSingkatPenulisanOngkir.Checked = _pengaturanUmum.is_singkat_penulisan_ongkir;
            chkTampilkanKeteranganTambahanItemJual.Checked = _pengaturanUmum.is_tampilkan_keterangan_tambahan_item_jual;
            txtKeteranganTambahanItemJual.Text = _pengaturanUmum.keterangan_tambahan_item_jual;
        }

        protected override void Simpan()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                // simpan pengaturan lokal (app.config)
                SimpanPengaturanLokal();

                // simpan pengaturan global (databases)
                SimpanPengaturanGlobal();

                // simpan header nota
                SimpanHeaderNota();

                // simpan header nota minipos
                SimpanHeaderNotaMiniPOS();

                // simpan footer nota minipos
                SimpanFooterNotaMiniPOS();

                // simpan label nota
                SimpanLabelNota();

                this.Close();
            }
        }

        /// <summary>
        /// Simpan pengaturan aplikasi di masing-masing pc (app.config)
        /// </summary>
        private void SimpanPengaturanLokal()
        {
            var appConfigFile = string.Format("{0}\\OpenRetail.exe.config", Utils.GetAppPath());

            _pengaturanUmum.nama_printer = cmbPrinter.Text;
            _pengaturanUmum.is_auto_print = chkCetakOtomatis.Checked;

            var jenisPrinter = JenisPrinter.InkJet;

            if (rdoJenisPrinterDotMatrix.Checked)
                jenisPrinter = JenisPrinter.DotMatrix;
            else if (rdoJenisPrinterMiniPOS.Checked)
                jenisPrinter = JenisPrinter.MiniPOS;

            _pengaturanUmum.jenis_printer = jenisPrinter;
            _pengaturanUmum.is_cetak_customer = chkCetakCustomer.Checked;
            _pengaturanUmum.is_show_minimal_stok = chkTampilkanInfoMinimalStokProduk.Checked;
            _pengaturanUmum.is_customer_required = chkCustomerWajibDiisi.Checked;
            _pengaturanUmum.is_cetak_keterangan_nota = chkCetakKeteranganNota.Checked;
            _pengaturanUmum.is_singkat_penulisan_ongkir = chkSingkatPenulisanOngkir.Checked;
            _pengaturanUmum.jumlah_karakter = Convert.ToInt32(txtJumlahKarakter.Text);
            _pengaturanUmum.jumlah_gulung = Convert.ToInt32(txtJumlahGulung.Text);
            _pengaturanUmum.ukuran_font = Convert.ToInt32(txtUkuranFont.Text);
            _pengaturanUmum.is_autocut = chkAutocut.Checked;
            _pengaturanUmum.is_open_cash_drawer = chkOpenCashDrawer.Checked;

            // simpan info printer
            AppConfigHelper.SaveValue("printerName", cmbPrinter.Text, appConfigFile);
            AppConfigHelper.SaveValue("isAutoPrinter", chkCetakOtomatis.Checked.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("jenis_printer", Convert.ToString((int)jenisPrinter), appConfigFile);

            // simpan info printer mini pos
            AppConfigHelper.SaveValue("isCetakCustomer", chkCetakCustomer.Checked.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("isShowMinimalStok", chkTampilkanInfoMinimalStokProduk.Checked.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("isCustomerRequired", chkCustomerWajibDiisi.Checked.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("isCetakKeteranganNota", chkCetakKeteranganNota.Checked.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("isSingkatPenulisanOngkir", chkSingkatPenulisanOngkir.Checked.ToString(), appConfigFile);
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

        /// <summary>
        /// Simpan pengaturan aplikasi di database (global)
        /// </summary>
        private void SimpanPengaturanGlobal()
        {
            ISettingAplikasiBll settingAplikasiBll = new SettingAplikasiBll();
            var settingAplikasi = settingAplikasiBll.GetAll().SingleOrDefault();

            if (settingAplikasi != null)
            {
                settingAplikasi.is_update_harga_jual_master_produk = chkUpdateHargaJual.Checked;
                settingAplikasi.is_stok_produk_boleh_minus = chkStokProdukBolehMinus.Checked;
                settingAplikasi.is_fokus_input_kolom_jumlah = chkFokusKeKolomJumlah.Checked;
                settingAplikasi.is_tampilkan_keterangan_tambahan_item_jual = chkTampilkanKeteranganTambahanItemJual.Checked;
                settingAplikasi.keterangan_tambahan_item_jual = txtKeteranganTambahanItemJual.Text;

                var result = settingAplikasiBll.Update(settingAplikasi);
                if (result > 0)
                {
                    _pengaturanUmum.is_update_harga_jual = chkUpdateHargaJual.Checked;
                    _pengaturanUmum.is_stok_produk_boleh_minus = chkStokProdukBolehMinus.Checked;
                    _pengaturanUmum.is_fokus_input_kolom_jumlah = chkFokusKeKolomJumlah.Checked;
                    _pengaturanUmum.is_tampilkan_keterangan_tambahan_item_jual = chkTampilkanKeteranganTambahanItemJual.Checked;
                    _pengaturanUmum.keterangan_tambahan_item_jual = txtKeteranganTambahanItemJual.Text;
                }
            }
        }

        private void SimpanHeaderNota()
        {
            IHeaderNotaBll headerNotaBll = new HeaderNotaBll();

            var index = 0;
            foreach (var item in _listOfTxtHeaderNota)
            {
                var headerNota = new HeaderNota
                {
                    header_nota_id = item.Tag.ToString(),
                    keterangan = item.Text
                };

                var result = headerNotaBll.Update(headerNota);
                if (result > 0)
                {
                    _pengaturanUmum.list_of_header_nota[index].header_nota_id = headerNota.header_nota_id;
                    _pengaturanUmum.list_of_header_nota[index].keterangan = headerNota.keterangan;
                }

                index++;
            }
        }

        private void SimpanHeaderNotaMiniPOS()
        {
            IHeaderNotaMiniPosBll headerNotaBll = new HeaderNotaMiniPosBll();

            var index = 0;
            foreach (var item in _listOfTxtHeaderNotaMiniPOS)
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

        private void SimpanFooterNotaMiniPOS()
        {
            IFooterNotaMiniPosBll footerNotaBll = new FooterNotaMiniPosBll();

            var index = 0;
            foreach (var item in _listOfTxtFooterNotaMiniPOS)
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

        private void SimpanLabelNota()
        {
            ILabelNotaBll labelNotaBll = new LabelNotaBll();

            var index = 0;
            foreach (var item in _listOfTxtLabelNota)
            {
                var labelNota = new LabelNota
                {
                    label_nota_id = item.Tag.ToString(),
                    keterangan = item.Text
                };

                var result = labelNotaBll.Update(labelNota);
                if (result > 0)
                {
                    _pengaturanUmum.list_of_label_nota[index].label_nota_id = labelNota.label_nota_id;
                    _pengaturanUmum.list_of_label_nota[index].keterangan = labelNota.keterangan;
                }

                index++;
            }
        }

        private void btnLihatContohNotaPenjualan_Click(object sender, EventArgs e)
        {
            var jualProdukId = string.Empty;

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                ICetakNotaBll bll = new CetakNotaDummyBll();
                var listOfJual = bll.GetNotaPenjualan(jualProdukId);

                if (listOfJual.Count > 0)
                {
                    var reportDataSource = new ReportDataSource
                    {
                        Name = "NotaPenjualan",
                        Value = listOfJual
                    };

                    var parameters = new List<ReportParameter>();
                    var index = 1;

                    foreach (var txtHeaderNota in _listOfTxtHeaderNota)
                    {
                        var paramName = string.Format("header{0}", index);
                        parameters.Add(new ReportParameter(paramName, txtHeaderNota.Text));

                        index++;
                    }

                    foreach (var item in listOfJual)
                    {
                        item.label_dari1 = txtDari1.Text;
                        item.label_dari2 = txtDari2.Text;
                        item.label_dari3 = txtDari3.Text;

                        if (_pengaturanUmum.is_singkat_penulisan_ongkir)
                        {
                            item.ongkos_kirim /= 1000;
                        }
                    }

                    var dt = DateTime.Now;
                    var kotaAndTanggal = string.Format("{0}, {1}", MainProgram.profil.kota, dt.Day + " " + DayMonthHelper.GetBulanIndonesia(dt.Month) + " " + dt.Year);

                    parameters.Add(new ReportParameter("kota", kotaAndTanggal));
                    parameters.Add(new ReportParameter("footer", MainProgram.pengguna.nama_pengguna));

                    var frmPreviewReport = new FrmPreviewReport("Contoh Nota Penjualan", "RvNotaPenjualanProdukLabel", reportDataSource, parameters);
                    frmPreviewReport.ShowDialog();
                }
            }
        }

        private void chkPrinterMiniPOS_CheckedChanged(object sender, EventArgs e)
        {
            txtJumlahKarakter.Enabled = ((CheckBox)sender).Checked;
            txtJumlahGulung.Enabled = txtJumlahKarakter.Enabled;

            if (txtJumlahKarakter.Enabled)
            {
                txtJumlahKarakter.BackColor = Color.White;
                txtJumlahGulung.BackColor = Color.White;
            }
        }

        private void btnLihatContohNotaPenjualanMiniPOS_Click(object sender, EventArgs e)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                var parameters = new List<ReportParameter>();
                var index = 1;

                foreach (var txtHeaderNota in _listOfTxtHeaderNotaMiniPOS)
                {
                    var paramName = string.Format("header{0}", index);
                    parameters.Add(new ReportParameter(paramName, txtHeaderNota.Text));

                    index++;
                }

                index = 1;
                foreach (var txtFooterNota in _listOfTxtFooterNotaMiniPOS)
                {
                    var paramName = string.Format("footer{0}", index);
                    parameters.Add(new ReportParameter(paramName, txtFooterNota.Text));

                    index++;
                }

                var reportName = "RvNotaPenjualanMiniPOSTanpaCustomer";

                if (chkCetakCustomer.Checked)
                    reportName = "RvNotaPenjualanMiniPOS";

                var frmPreviewReport = new FrmPreviewReport("Contoh Nota Penjualan MINI POS", reportName, new ReportDataSource(), parameters);
                frmPreviewReport.ShowDialog();
            }
        }

        private void rdoJenisPrinterInkJet_CheckedChanged(object sender, EventArgs e)
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
        }

        private void rdoJenisPrinterDotMatrix_CheckedChanged(object sender, EventArgs e)
        {
            rdoJenisPrinterInkJet_CheckedChanged(sender, e);
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
        }

        private void chkUkuranFont_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            txtUkuranFont.Enabled = chk.Checked;

            txtUkuranFont.Text = "0";
            if (chk.Checked)
                txtUkuranFont.Text = _pengaturanUmum.ukuran_font.ToString();
        }

        private void chkTampilkanKeteranganTambahanItemJual_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            txtKeteranganTambahanItemJual.Enabled = chk.Checked;

            txtKeteranganTambahanItemJual.Text = "Keterangan";
            if (chk.Checked)
                txtKeteranganTambahanItemJual.Text = _pengaturanUmum.keterangan_tambahan_item_jual;
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

        private void chkIsActiveCustomerDisplay_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;

            cmbPort.Enabled = chk.Checked;
            btnTesKoneksi.Enabled = chk.Checked;
            grpKalimatPembuka.Enabled = chk.Checked;
            grpKalimatPenutup.Enabled = chk.Checked;
        }

        private void btnTesKoneksi_Click(object sender, EventArgs e)
        {
            const int MAX_LENGTH = 20;

            var appName = "OpenRetail Server";
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
    }
}