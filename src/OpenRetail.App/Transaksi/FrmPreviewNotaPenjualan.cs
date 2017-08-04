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
    public partial class FrmPreviewNotaPenjualan : Form
    {
        private string _reportNameSpace = @"OpenRetail.Report.{0}.rdlc";
        private Assembly _assemblyReport;

        private ILog _log;
        private Customer _customer = null;
        private JualProduk _jual = null;        
        private Pengguna _pengguna;
        private Profil _profil;
        private PengaturanUmum _pengaturanUmum;

        public FrmPreviewNotaPenjualan()
        {
            InitializeComponent();
            this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            this.reportViewer1.ZoomMode = ZoomMode.Percent;
            this.reportViewer1.ZoomPercent = 100;

            ColorManagerHelper.SetTheme(this, this);
            _assemblyReport = Assembly.LoadFrom("OpenRetail.Report.dll");
        }

        public FrmPreviewNotaPenjualan(string header, JualProduk jual)
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

            chkIsSdac.Checked = this._jual.is_sdac;
            chkIsSdac_CheckedChanged(chkIsSdac, new EventArgs());
            btnPreviewNota_Click(btnPreviewNota, new EventArgs());
        }        

        private void chkIsSdac_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;

            txtKepada1.Enabled = !chk.Checked;
            txtKepada2.Enabled = !chk.Checked;
            txtKepada3.Enabled = !chk.Checked;
            txtKepada4.Enabled = !chk.Checked;            

            var kecamatan = string.IsNullOrEmpty(_customer.kecamatan) ? string.Empty : _customer.kecamatan;
            var kelurahan = string.IsNullOrEmpty(_customer.kelurahan) ? string.Empty : _customer.kelurahan;
            var kota = string.IsNullOrEmpty(_customer.kota) ? string.Empty : _customer.kota;
            var kodePos = (string.IsNullOrEmpty(_customer.kode_pos) || _customer.kode_pos == "0") ? string.Empty : _customer.kode_pos;
            var telepon = string.IsNullOrEmpty(_customer.telepon) ? string.Empty : _customer.telepon;

            var kepada1 = _customer.nama_customer;
            var kepada2 = _customer.alamat;
            var kepada3 = string.Format("{0} - {1} - {2} - {3}", kecamatan, kelurahan, kota, kodePos);
            kepada3 = kepada3.Replace(" -  -  - ", "");

            var kepada4 = telepon;

            if (!chk.Checked)
            {
                kepada1 = string.IsNullOrEmpty(_jual.kirim_kepada) ? kepada1 : _jual.kirim_kepada;
                kepada2 = string.IsNullOrEmpty(_jual.kirim_alamat) ? kepada2 : _jual.kirim_alamat;
                kepada3 = string.IsNullOrEmpty(_jual.kirim_kecamatan) ? kepada3 : _jual.kirim_kecamatan;
                kepada4 = string.IsNullOrEmpty(_jual.kirim_kelurahan) ? kepada4 : _jual.kirim_kelurahan;
            }

            txtKepada1.Text = kepada1;
            txtKepada2.Text = kepada2;
            txtKepada3.Text = kepada3;
            txtKepada4.Text = kepada4;
        }

        private void btnPreviewNota_Click(object sender, EventArgs e)
        {
            _jual.is_sdac = chkIsSdac.Checked;

            if (!chkIsSdac.Checked)
            {
                _jual.kirim_kepada = txtKepada1.Text;
                _jual.kirim_alamat = txtKepada2.Text;
                _jual.kirim_kecamatan = txtKepada3.Text;
                _jual.kirim_kelurahan = txtKepada4.Text;
            }
            
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                PreviewNota(_jual);
            }
        }

        private void PreviewNota(JualProduk jual, bool isPreview = true)
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

                // set header nota
                var parameters = new List<ReportParameter>();
                var index = 1;

                foreach (var item in _pengaturanUmum.list_of_header_nota)
                {
                    var paramName = string.Format("header{0}", index);
                    parameters.Add(new ReportParameter(paramName, item.keterangan));

                    index++;
                }

                foreach (var item in listOfItemNota)
                {
                    item.is_sdac = chkIsSdac.Checked;

                    if (!chkIsSdac.Checked)
                    {
                        item.kirim_kepada = txtKepada1.Text;
                        item.kirim_alamat = txtKepada2.Text;
                        item.kirim_kecamatan = txtKepada3.Text;
                        item.kirim_kelurahan = txtKepada4.Text;
                    }                    
                }

                // set footer nota
                var dt = DateTime.Now;
                var kotaAndTanggal = string.Format("{0}, {1}", _profil.kota, dt.Day + " " + DayMonthHelper.GetBulanIndonesia(dt.Month) + " " + dt.Year);

                parameters.Add(new ReportParameter("kota", kotaAndTanggal));
                parameters.Add(new ReportParameter("footer", _pengguna.nama_pengguna));

                var reportName = jual.is_dropship ? "RvNotaPenjualanProdukTanpaLabelDropship" : "RvNotaPenjualanProdukTanpaLabel";

                if (isPreview)
                {
                    reportName = string.Format(_reportNameSpace, reportName);
                    var stream = _assemblyReport.GetManifestResourceStream(reportName);

                    this.reportViewer1.LocalReport.DataSources.Clear();
                    this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
                    this.reportViewer1.LocalReport.LoadReportDefinition(stream);

                    if (!(parameters == null))
                        this.reportViewer1.LocalReport.SetParameters(parameters);

                    this.reportViewer1.RefreshReport();
                }
                else
                {
                    var printReport = new ReportViewerPrintHelper(reportName, reportDataSource, parameters, _pengaturanUmum.nama_printer);
                    printReport.Print();
                }                
            }
        }

        private void btnCetakNota_Click(object sender, EventArgs e)
        {
            if (MsgHelper.MsgKonfirmasi("Apakah proses pencetakan ingin dilanjutkan ?"))
            {
                using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                {
                    IJualProdukBll bll = new JualProdukBll(_log);
                    var result = bll.Update(_jual);

                    PreviewNota(_jual, false);
                }
            }
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmPreviewNotaPenjualan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEsc(e))
                this.Close();
        }
    }
}
