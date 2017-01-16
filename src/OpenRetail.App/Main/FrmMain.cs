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
using WeifenLuo.WinFormsUI.Docking;

using OpenRetail.App.Referensi;
using OpenRetail.App.Transaksi;
using OpenRetail.App.Helper;
using ConceptCave.WaitCursor;

namespace OpenRetail.App.Main
{
    public partial class FrmMain : Form
    {
        private FrmListGolongan _frmListGolongan;
        private FrmListProduk _frmListProduk;
        private FrmListPenyesuaianStok _frmListPenyesuaianStok;

        private FrmListCustomer _frmListCustomer;
        private FrmListSupplier _frmListSupplier;

        private FrmListJabatan _frmListJabatan;
        private FrmListKaryawan _frmListKaryawan;

        private FrmListJenisPengeluaran _frmListJenisPengeluaran;

        private FrmListPembelianProduk _frmListPembelianProduk;

        public FrmMain()
        {
            InitializeComponent();
            InitializeStatusBar();
            AddEventToolbar();
        }

        private void InitializeStatusBar()
        {
            var dt = DateTime.Now;

            sbJam.Text = string.Format("{0:HH:mm:ss}", dt);
            sbTanggal.Text = string.Format("{0}, {1}", DayMonthHelper.GetHariIndonesia(dt), dt.Day + " " + DayMonthHelper.GetBulanIndonesia(dt.Month) + " " + dt.Year);

            // TODO: fix me (di aktifkan lagi setelah module pengguna selesai)
            //if (MainProgram.pengguna != null)
            //    sbUser.Text = string.Format("Operator : {0}", MainProgram.pengguna.nama_pengguna);

            var versi = Utils.GetCurrentVersion("OpenRetail");

            var appName = string.Format(MainProgram.appName, versi);

            this.Text = appName;
            sbNamaAplikasi.Text = appName.Replace("&", "&&");
        }

        private void AddEventToolbar()
        {
            tbGolongan.Click += mnuGolongan_Click;
            tbProduk.Click += mnuProduk_Click;
            tbPenyesuaianStok.Click += mnuPenyesuaianStok_Click;
            tbSupplier.Click += mnuSupplier_Click;
            tbCustomer.Click += mnuCustomer_Click;
            tbPembelian.Click += mnuPembelianProduk_Click;
            tbPenjualan.Click += mnuPembelianProduk_Click;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            sbJam.Text = string.Format("{0:HH:mm:ss}", DateTime.Now);
        }

        private bool IsChildFormExists(Form frm)
        {
            return !(frm == null || frm.IsDisposed);
        }

        private void CloseAllDocuments()
        {
            if (this.mainDock.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (var form in MdiChildren)
                    form.Close();
            }
            else
            {
                var documents = this.mainDock.DocumentsToArray();
                foreach (var content in documents)
                    content.DockHandler.Close();
            }
        }

        private string GetTitleMenu(object sender)
        {
            var title = string.Empty;

            if (sender is ToolStripMenuItem)
            {
                title = ((ToolStripMenuItem)sender).Text;
            }
            else
            {
                title = ((ToolStripButton)sender).Text;
            }

            return title;
        }

        private void mnuGolongan_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListGolongan))
                _frmListGolongan = new FrmListGolongan(header);

            _frmListGolongan.Show(this.mainDock);
        }

        private void mnuProduk_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListProduk))
                _frmListProduk = new FrmListProduk(header);

            _frmListProduk.Show(this.mainDock);
        }

        private void mnuSupplier_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListSupplier))
                _frmListSupplier = new FrmListSupplier(header);

            _frmListSupplier.Show(this.mainDock);
        }

        private void mnuCustomer_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListCustomer))
                _frmListCustomer = new FrmListCustomer(header);

            _frmListCustomer.Show(this.mainDock);
        }

        private void mnuJabatan_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListJabatan))
                _frmListJabatan = new FrmListJabatan(header);

            _frmListJabatan.Show(this.mainDock);
        }

        private void mnuKeluarDariProgram_Click(object sender, EventArgs e)
        {
            if (MsgHelper.MsgKonfirmasi("Apakah proses ingin dilanjutkan ?"))
            {
                using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                {
                    CloseAllDocuments();

                    //this.FrmMain_Load(sender, null);
                    Application.Exit();
                }
            } 
        }

        private void mnuPembelianProduk_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListPembelianProduk))
                _frmListPembelianProduk = new FrmListPembelianProduk(header);

            _frmListPembelianProduk.Show(this.mainDock);
        }

        private void mnuJenisPengeluaran_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListJenisPengeluaran))
                _frmListJenisPengeluaran = new FrmListJenisPengeluaran(header);

            _frmListJenisPengeluaran.Show(this.mainDock);
        }

        private void karyawanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListKaryawan))
                _frmListKaryawan = new FrmListKaryawan(header);

            _frmListKaryawan.Show(this.mainDock);
        }

        private void mnuPenyesuaianStok_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListPenyesuaianStok))
                _frmListPenyesuaianStok = new FrmListPenyesuaianStok(header);

            _frmListPenyesuaianStok.Show(this.mainDock);
        }
    }
}
