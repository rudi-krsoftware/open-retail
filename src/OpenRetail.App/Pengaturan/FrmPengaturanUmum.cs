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
using System.Drawing.Printing;
using Microsoft.Reporting.WinForms;
using ConceptCave.WaitCursor;
using OpenRetail.Helper.UserControl;

namespace OpenRetail.App.Pengaturan
{
    public partial class FrmPengaturanUmum : FrmEntryStandard
    {
        private IList<AdvancedTextbox> _listOfTxtHeaderNota = new List<AdvancedTextbox>();
        private IList<AdvancedTextbox> _listOfTxtHeaderNotaMiniPOS = new List<AdvancedTextbox>();
        private IList<AdvancedTextbox> _listOfTxtFooterNotaMiniPOS = new List<AdvancedTextbox>();
        private IList<AdvancedTextbox> _listOfTxtLabelNota = new List<AdvancedTextbox>();
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
        }

        private void LoadSettingLainnya()
        {
            chkTampilkanInfoMinimalStokProduk.Checked = _pengaturanUmum.is_show_minimal_stok;
            chkCustomerWajibDiisi.Checked = _pengaturanUmum.is_customer_required;
            chkStokProdukBolehMinus.Checked = _pengaturanUmum.is_stok_produk_boleh_minus;
            chkFokusKeKolomJumlah.Checked = _pengaturanUmum.is_fokus_input_kolom_jumlah;
            chkUpdateHargaJual.Checked = _pengaturanUmum.is_update_harga_jual;
            chkSingkatPenulisanOngkir.Checked = _pengaturanUmum.is_singkat_penulisan_ongkir;            
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
            _pengaturanUmum.is_singkat_penulisan_ongkir = chkSingkatPenulisanOngkir.Checked;
            _pengaturanUmum.jumlah_karakter = Convert.ToInt32(txtJumlahKarakter.Text);
            _pengaturanUmum.jumlah_gulung = Convert.ToInt32(txtJumlahGulung.Text);                

            // simpan info printer
            AppConfigHelper.SaveValue("printerName", cmbPrinter.Text, appConfigFile);
            AppConfigHelper.SaveValue("isAutoPrinter", chkCetakOtomatis.Checked.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("jenis_printer", Convert.ToString((int)jenisPrinter), appConfigFile);

            // simpan info printer mini pos
            AppConfigHelper.SaveValue("isCetakCustomer", chkCetakCustomer.Checked.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("isShowMinimalStok", chkTampilkanInfoMinimalStokProduk.Checked.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("isCustomerRequired", chkCustomerWajibDiisi.Checked.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("isSingkatPenulisanOngkir", chkSingkatPenulisanOngkir.Checked.ToString(), appConfigFile);
            AppConfigHelper.SaveValue("jumlahKarakter", txtJumlahKarakter.Text, appConfigFile);
            AppConfigHelper.SaveValue("jumlahGulung", txtJumlahGulung.Text, appConfigFile);
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

                var result = settingAplikasiBll.Update(settingAplikasi);
                if (result > 0)
                {
                    _pengaturanUmum.is_update_harga_jual = chkUpdateHargaJual.Checked;
                    _pengaturanUmum.is_stok_produk_boleh_minus = chkStokProdukBolehMinus.Checked;
                    _pengaturanUmum.is_fokus_input_kolom_jumlah = chkFokusKeKolomJumlah.Checked;                    
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
        }
    }
}
