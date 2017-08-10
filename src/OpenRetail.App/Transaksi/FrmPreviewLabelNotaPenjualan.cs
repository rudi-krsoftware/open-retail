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

using Microsoft.Reporting.WinForms;
using System.Reflection;
using System.IO;

using log4net;
using OpenRetail.App.Helper;
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using ConceptCave.WaitCursor;

namespace OpenRetail.App.Transaksi
{
    public partial class FrmPreviewLabelNotaPenjualan : Form
    {
        private string _reportNameSpace = @"OpenRetail.Report.{0}.rdlc";
        private Assembly _assemblyReport;

        private ILog _log;
        private Customer _customer = null;
        private JualProduk _jual = null;        
        private Pengguna _pengguna;
        private Profil _profil;
        private PengaturanUmum _pengaturanUmum;

        public FrmPreviewLabelNotaPenjualan()
        {
            InitializeComponent();
            this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            this.reportViewer1.ZoomMode = ZoomMode.Percent;
            this.reportViewer1.ZoomPercent = 100;

            ColorManagerHelper.SetTheme(this, this);
            _assemblyReport = Assembly.LoadFrom("OpenRetail.Report.dll");            
        }

        public FrmPreviewLabelNotaPenjualan(string header, JualProduk jual)
            : this()
        {
            this.Text = header;
            this.lblHeader.Text = header;
            this._log = MainProgram.log;
            this._pengguna = MainProgram.pengguna;
            this._profil = MainProgram.profil;
            this._pengaturanUmum = MainProgram.pengaturanUmum;
            this._jual = jual;
            this._customer = this._jual.Customer;

            SetLabelNota();
            btnPreviewNota_Click(btnPreviewNota, new EventArgs());
        }

        private void SetLabelNota()
        {
            var dari1 = this._pengaturanUmum.list_of_label_nota[0].keterangan;
            var dari2 = this._pengaturanUmum.list_of_label_nota[1].keterangan;
            var dari3 = this._pengaturanUmum.list_of_label_nota[2].keterangan;

            dari1 = string.IsNullOrEmpty(this._jual.label_dari1) ? dari1 : this._jual.label_dari1;
            dari2 = string.IsNullOrEmpty(this._jual.label_dari2) ? dari2 : this._jual.label_dari2;
            dari3 = string.IsNullOrEmpty(this._jual.label_dari3) ? dari3 : this._jual.label_dari3;

            var kecamatan = string.IsNullOrEmpty(_customer.kecamatan) ? string.Empty : _customer.kecamatan;
            var kelurahan = string.IsNullOrEmpty(_customer.kelurahan) ? string.Empty : _customer.kelurahan;
            var kota = string.IsNullOrEmpty(_customer.kota) ? string.Empty : _customer.kota;
            var kodePos = (string.IsNullOrEmpty(_customer.kode_pos) || _customer.kode_pos == "0") ? string.Empty : _customer.kode_pos;
            var telepon = string.IsNullOrEmpty(_customer.telepon) ? string.Empty : _customer.telepon;

            // info alamat kirim berdasarkan data customer
            var kepada1 = _customer.nama_customer;
            var kepada2 = _customer.alamat;
            var kepada3 = string.Format("{0} - {1} - {2} - {3}", kecamatan, kelurahan, kota, kodePos);
            kepada3 = kepada3.Replace(" -  -  - ", "");

            var kepada4 = telepon;

            // info alamat kirim berdasarkan data alamat yang diedit pada saat penjualan
            kepada1 = string.IsNullOrEmpty(this._jual.kirim_kepada) ? kepada1 : this._jual.kirim_kepada;
            kepada2 = string.IsNullOrEmpty(this._jual.kirim_alamat) ? kepada2 : this._jual.kirim_alamat;
            kepada3 = string.IsNullOrEmpty(this._jual.kirim_kecamatan) ? kepada3 : this._jual.kirim_kecamatan;
            kepada4 = string.IsNullOrEmpty(this._jual.kirim_kelurahan) ? kepada4 : this._jual.kirim_kelurahan;

            // info alamat kirim yang diedit di label nota
            kepada1 = string.IsNullOrEmpty(this._jual.label_kepada1) ? kepada1 : this._jual.label_kepada1;
            kepada2 = string.IsNullOrEmpty(this._jual.label_kepada2) ? kepada2 : this._jual.label_kepada2;
            kepada3 = string.IsNullOrEmpty(this._jual.label_kepada3) ? kepada3 : this._jual.label_kepada3;
            kepada4 = string.IsNullOrEmpty(this._jual.label_kepada4) ? kepada4 : this._jual.label_kepada4;

            txtDari1.Text = dari1;
            txtDari2.Text = dari2;
            txtDari3.Text = dari3;

            txtKepada1.Text = kepada1;
            txtKepada2.Text = kepada2;
            txtKepada3.Text = kepada3;
            txtKepada4.Text = kepada4;
        }

        private void btnPreviewNota_Click(object sender, EventArgs e)
        {
            _jual.label_dari1 = txtDari1.Text;
            _jual.label_dari2 = txtDari2.Text;
            _jual.label_dari3 = txtDari3.Text;

            _jual.label_kepada1 = txtKepada1.Text;
            _jual.label_kepada2 = txtKepada2.Text;
            _jual.label_kepada3 = txtKepada3.Text;
            _jual.label_kepada4 = txtKepada4.Text;
            
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                PreviewLabelNota(_jual);
            }
        }

        private void PreviewLabelNota(JualProduk jual, bool isPreview = true)
        {
            ICetakNotaBll cetakBll = new CetakNotaBll(_log);
            var listOfItemNota = cetakBll.GetNotaPenjualan(jual.jual_id);

            if (listOfItemNota.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "NotaPenjualan",
                    Value = listOfItemNota
                };

                foreach (var item in listOfItemNota)
                {
                    item.label_dari1 = txtDari1.Text;
                    item.label_dari2 = txtDari2.Text;
                    item.label_dari3 = txtDari3.Text;

                    item.label_kepada1 = txtKepada1.Text;
                    item.label_kepada2 = txtKepada2.Text;
                    item.label_kepada3 = txtKepada3.Text;
                    item.label_kepada4 = txtKepada4.Text;

                    if (_pengaturanUmum.is_singkat_penulisan_ongkir && item.ongkos_kirim > 0)
                    {
                        item.ongkos_kirim /= 1000;
                        item.label_ongkos_kirim = item.ongkos_kirim.ToString();
                    }
                    else
                    {
                        item.label_ongkos_kirim = NumberHelper.NumberToString(item.ongkos_kirim);
                    }                        
                }

                var reportName = "RvLabelNotaPenjualan";

                if (isPreview)
                {
                    reportName = string.Format(_reportNameSpace, reportName);
                    var stream = _assemblyReport.GetManifestResourceStream(reportName);

                    this.reportViewer1.LocalReport.DataSources.Clear();
                    this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    this.reportViewer1.LocalReport.LoadReportDefinition(stream);

                    this.reportViewer1.RefreshReport();
                }
                else
                {
                    var printReport = new ReportViewerPrintHelper(reportName, reportDataSource, printerName: _pengaturanUmum.nama_printer);
                    printReport.Print();
                }                
            }
        }

        private void btnCetakLabelNota_Click(object sender, EventArgs e)
        {
            if (MsgHelper.MsgKonfirmasi("Apakah proses pencetakan ingin dilanjutkan ?"))
            {
                using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                {
                    IJualProdukBll bll = new JualProdukBll(_log);
                    var result = bll.Update(_jual);

                    PreviewLabelNota(_jual, false);
                }
            }
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmPreviewLabelNotaPenjualan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEsc(e))
                this.Close();
        }
    }
}
