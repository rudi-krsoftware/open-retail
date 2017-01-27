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
using OpenRetail.App.Pengaturan;
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;

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
        private FrmListPembayaranHutangPembelianProduk _frmListPembayaranHutangPembelianProduk;
        private FrmListReturPembelianProduk _frmListReturPembelianProduk;

        private FrmListPenjualanProduk _frmListPenjualanProduk;
        private FrmListPembayaranPiutangPenjualanProduk _frmListPembayaranPiutangPenjualanProduk;
        private FrmListReturPenjualanProduk _frmListReturPenjualanProduk;

        private FrmListHakAkses _frmListHakAkses;
        private FrmListOperator _frmListOperator;

        public FrmMain()
        {
            InitializeComponent();

            InitializeStatusBar();
            AddEventToolbar();
            SetDisabledMenuAndToolbar(menuStrip1, toolStrip1);
        }

        private void WriteOutput(string s)
        {
            System.Diagnostics.Debug.Print(s);
        }

        private IEnumerable<ToolStripMenuItem> GetItems(ToolStripMenuItem menuItem)
        {
            foreach (var item in menuItem.DropDownItems)
            {
                if (item is ToolStripMenuItem)
                {
                    var dropDownItem = (ToolStripMenuItem)item;

                    if (dropDownItem.HasDropDownItems)
                    {
                        foreach (ToolStripMenuItem subItem in GetItems(dropDownItem))
                            yield return subItem;
                    }

                    yield return (ToolStripMenuItem)item;
                }
            }
        }

        /// <summary>
        /// Method untuk menonaktifkan menu dan toolbar yang belum aktif (membaca setting tabel m_menu)
        /// </summary>
        /// <param name="menuStrip"></param>
        /// <param name="toolStrip"></param>
        private void SetDisabledMenuAndToolbar(MenuStrip menuStrip, ToolStrip toolStrip)
        {
            IMenuBll menuBll = new MenuBll(MainProgram.log);
            var listOfMenu = menuBll.GetAll()
                                    .Where(f => f.parent_id != null && f.nama_form.Length > 0)
                                    .ToList();
            
            // perulangan untuk mengecek menu dan sub menu
            foreach (ToolStripMenuItem parentMenu in menuStrip.Items)
            {
                var listOfChildMenu = GetItems(parentMenu);

                foreach (var childMenu in listOfChildMenu)
                {
                    var menu = listOfMenu.Where(f => f.nama_menu == childMenu.Name)
                                         .SingleOrDefault();
                    if (menu != null)
                    {
                        childMenu.Enabled = menu.is_enabled;
                    }
                }
            }

            // perulangan untuk mengecek item toolbar
            foreach (ToolStripItem item in toolStrip.Items)
            {
                var menu = listOfMenu.Where(f => f.nama_menu.Substring(3) == item.Name.Substring(2))
                                     .SingleOrDefault();
                if (menu != null)
                {
                    item.Enabled = menu.is_enabled;
                }
            }
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
            tbPembelianProduk.Click += mnuPembelianProduk_Click;
            tbPenjualanProduk.Click += mnuPenjualanProduk_Click;
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

        private void mnuManajemenOperator_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListOperator))
                _frmListOperator = new FrmListOperator(header);

            _frmListOperator.Show(this.mainDock);
        }

        private void mnuHakAksesAplikasi_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListHakAkses))
                _frmListHakAkses = new FrmListHakAkses(header);

            _frmListHakAkses.Show(this.mainDock);
        }

        private void mnuPembayaranHutangPembelianProduk_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListPembayaranHutangPembelianProduk))
                _frmListPembayaranHutangPembelianProduk = new FrmListPembayaranHutangPembelianProduk(header);

            _frmListPembayaranHutangPembelianProduk.Show(this.mainDock);
        }

        private void mnuReturPembelianProduk_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListReturPembelianProduk))
                _frmListReturPembelianProduk = new FrmListReturPembelianProduk(header);

            _frmListReturPembelianProduk.Show(this.mainDock);
        }

        private void mnuPenjualanProduk_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListPenjualanProduk))
                _frmListPenjualanProduk = new FrmListPenjualanProduk(header);

            _frmListPenjualanProduk.Show(this.mainDock);
        }

        private void mnuPembayaranPiutangPenjualanProduk_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListPembayaranPiutangPenjualanProduk))
                _frmListPembayaranPiutangPenjualanProduk = new FrmListPembayaranPiutangPenjualanProduk(header);

            _frmListPembayaranPiutangPenjualanProduk.Show(this.mainDock);
        }

        private void mnuReturPenjualanProduk_Click(object sender, EventArgs e)
        {
            var header = GetTitleMenu(sender);

            if (!IsChildFormExists(_frmListReturPenjualanProduk))
                _frmListReturPenjualanProduk = new FrmListReturPenjualanProduk(header);

            _frmListReturPenjualanProduk.Show(this.mainDock);
        }
    }
}
