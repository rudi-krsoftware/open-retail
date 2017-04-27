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
using OpenRetail.App.Pengeluaran;
using OpenRetail.App.Laporan;
using OpenRetail.App.Pengaturan;
using OpenRetail.App.Helper;
using ConceptCave.WaitCursor;
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using log4net;
using AutoUpdaterDotNET;
using System.Threading;

namespace OpenRetail.App.Main
{
    public partial class FrmMain : Form, IListener
    {
        //Disable close button
        private const int CP_DISABLE_CLOSE_BUTTON = 0x200;

        private FrmListGolongan _frmListGolongan;
        private FrmListProdukWithNavigation _frmListProduk;
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

        private FrmListPengeluaranBiaya _frmListPengeluaranBiaya;
        private FrmListKasbon _frmListKasbon;
        private FrmListPenggajianKaryawan _frmListPenggajianKaryawan;

        private FrmListHakAkses _frmListHakAkses;
        private FrmListOperator _frmListOperator;

        /// <summary>
        /// Variabel lokal untuk menampung menu id. 
        /// Menu id digunakan untuk mengeset hak akses masing-masing form yang diakses
        /// </summary>
        private Dictionary<string, string> _getMenuID;
        private ILog _log;
        private string _openRetailBaseUrl = "https://openretailblog.wordpress.com";

        private ThreadHelper _lightSleeper = new ThreadHelper();

        public bool IsLogout { get; private set; }

        public FrmMain()
        {
            InitializeComponent();
            mainDock.BackColor = Color.FromArgb(255, 255, 255);

            _log = MainProgram.log;

            AddEventToolbar();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            InitializeStatusBar();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                if (Utils.IsRunningUnderIDE())
                {
                    return base.CreateParams;
                }
                else
                {
                    var cp = base.CreateParams;
                    cp.ClassStyle = cp.ClassStyle | CP_DISABLE_CLOSE_BUTTON;

                    // bug fixed: flicker
                    // http://stackoverflow.com/questions/2612487/how-to-fix-the-flickering-in-user-controls
                    //cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED

                    return cp;
                }
            }
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

        public void InisialisasiData()
        {
            SetMenuId();
            SetDisabledMenuAndToolbar(menuStrip1, toolStrip1);
            
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;            
        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            _lightSleeper.Cancel();

            if (args != null)
            {
                if (args.IsUpdateAvailable)
                {                    
                    var msg = "Update terbaru versi {0} sudah tersedia. Saat ini Anda sedang menggunakan Versi {1}\n\nApakah Anda ingin memperbarui aplikasi ini sekarang ?";

                    var installedVersion = string.Format("{0}.{1}.{2}.{3} (v{0}.{1}.{2}{4})", args.InstalledVersion.Major, args.InstalledVersion.Minor, args.InstalledVersion.Build, args.InstalledVersion.Revision, MainProgram.stageOfDevelopment);
                    var currentVersion = string.Format("{0}.{1}.{2}.{3}", args.CurrentVersion.Major, args.CurrentVersion.Minor, args.CurrentVersion.Build, args.CurrentVersion.Revision);

                    var dialogResult = MessageBox.Show(string.Format(msg, currentVersion, installedVersion), "Update Tersedia",
                                                       MessageBoxButtons.YesNo,
                                                       MessageBoxIcon.Information);

                    if (dialogResult.Equals(DialogResult.Yes))
                    {
                        try
                        {
                            AutoUpdater.DownloadUpdate();
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Tidak ada update yang tersedia, silahkan dicoba lagi nanti.", "Update belum tersedia", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Gagal melakukan koneksi ke server, silahkan dicoba lagi nanti.", "Cek update terbaru gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetMenuId()
        {
            IMenuBll menuBll = new MenuBll(_log);
            var listOfMenu = menuBll.GetAll().Where(f => f.parent_id != null && f.nama_form.Length > 0)
                                             .ToList();
            _getMenuID = new Dictionary<string, string>();

            foreach (var item in listOfMenu)
            {
                _getMenuID.Add(item.nama_form, item.menu_id);
            }
        }

        /// <summary>
        /// Method untuk menonaktifkan menu dan toolbar yang belum aktif (membaca setting tabel m_menu)
        /// </summary>
        /// <param name="menuStrip"></param>
        /// <param name="toolStrip"></param>
        private void SetDisabledMenuAndToolbar(MenuStrip menuStrip, ToolStrip toolStrip)
        {
            IMenuBll menuBll = new MenuBll(_log);
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

        public void InitializeStatusBar()
        {
            var dt = DateTime.Now;

            sbJam.Text = string.Format("{0:HH:mm:ss}", dt);
            sbTanggal.Text = string.Format("{0}, {1}", DayMonthHelper.GetHariIndonesia(dt), dt.Day + " " + DayMonthHelper.GetBulanIndonesia(dt.Month) + " " + dt.Year);

            if (MainProgram.pengguna != null)
                sbOperator.Text = string.Format("Operator : {0}", MainProgram.pengguna.nama_pengguna);

            var firstReleaseYear = 2017;
            var currentYear = DateTime.Today.Year;
            var copyright = currentYear > firstReleaseYear ? string.Format("{0} - {1}", firstReleaseYear, currentYear) : firstReleaseYear.ToString();

            var versi = Utils.GetCurrentVersion("OpenRetail");
            var appName = string.Format(MainProgram.appName, versi, MainProgram.stageOfDevelopment, copyright);

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
            tbPengeluaranBiaya.Click += mnuPengeluaranBiaya_Click;
            tbPenggajian.Click += mnuPenggajian_Click;
            tbLapPembelianProduk.Click += mnuLapPembelianProduk_Click;
            tbLapPenjualanProduk.Click += mnuLapPenjualanProduk_Click;
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
            foreach (var form in MdiChildren)
            {
                form.Close();
            }                
        }

        private void ShowForm<T>(object sender, ref T form)
        {
            var header = GetMenuTitle(sender);
            var menuId = _getMenuID[GetFormName(sender)];

            if (!IsChildFormExists((DockContent)(object)form))
                form = (T)Activator.CreateInstance(typeof(T), header, MainProgram.pengguna, menuId);

            ((DockContent)(object)form).Show(this.mainDock);
        }

        private void ShowFormDialog<T>(object sender)
        {
            var header = GetMenuTitle(sender);
            var menuName = GetMenuName(sender);

            if (menuName.Substring(0, 6) == "mnuLap")
            {
                header = string.Format("Laporan {0}", GetMenuTitle(sender));
            }

            if (RolePrivilegeHelper.IsHaveHakAkses(menuName, MainProgram.pengguna, GrantState.SELECT))
            {
                var form = (T)Activator.CreateInstance(typeof(T), header);
                ((Form)(object)form).ShowDialog();
            }
            else
                MsgHelper.MsgWarning("Maaf Anda tidak mempunyai otoritas untuk mengakses menu ini");
        }

        private string GetMenuTitle(object sender)
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

        private string GetMenuName(object sender)
        {
            var menuName = string.Empty;

            if (sender is ToolStripMenuItem)
            {
                menuName = ((ToolStripMenuItem)sender).Name;
            }
            else
            {
                menuName = ((ToolStripButton)sender).Name;
                menuName = string.Format("mnu{0}", menuName.Substring(2));
            }

            return menuName;
        }

        private string GetFormName(object sender)
        {
            var formName = string.Empty;

            if (sender is ToolStripMenuItem)
            {
                formName = ((ToolStripMenuItem)sender).Tag.ToString();
            }
            else
            {
                formName = ((ToolStripButton)sender).Tag.ToString();
            }

            return formName;
        }

        private void mnuGolongan_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListGolongan>(sender, ref _frmListGolongan);
        }        

        private void mnuProduk_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListProdukWithNavigation>(sender, ref _frmListProduk);
        }

        private void mnuSupplier_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListSupplier>(sender, ref _frmListSupplier);
        }

        private void mnuCustomer_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListCustomer>(sender, ref _frmListCustomer);
        }

        private void mnuJabatan_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListJabatan>(sender, ref _frmListJabatan);
        }        

        private void mnuPembelianProduk_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListPembelianProduk>(sender, ref _frmListPembelianProduk);
        }

        private void mnuJenisPengeluaran_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListJenisPengeluaran>(sender, ref _frmListJenisPengeluaran);
        }

        private void mnuKaryawan_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListKaryawan>(sender, ref _frmListKaryawan);
        }

        private void mnuPenyesuaianStok_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListPenyesuaianStok>(sender, ref _frmListPenyesuaianStok);
        }

        private void mnuManajemenOperator_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListOperator>(sender, ref _frmListOperator);
        }

        private void mnuHakAksesAplikasi_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListHakAkses>(sender, ref _frmListHakAkses);
        }

        private void mnuPembayaranHutangPembelianProduk_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListPembayaranHutangPembelianProduk>(sender, ref _frmListPembayaranHutangPembelianProduk);
        }

        private void mnuReturPembelianProduk_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListReturPembelianProduk>(sender, ref _frmListReturPembelianProduk);
        }

        private void mnuPenjualanProduk_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListPenjualanProduk>(sender, ref _frmListPenjualanProduk);
        }

        private void mnuPembayaranPiutangPenjualanProduk_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListPembayaranPiutangPenjualanProduk>(sender, ref _frmListPembayaranPiutangPenjualanProduk);
        }

        private void mnuReturPenjualanProduk_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListReturPenjualanProduk>(sender, ref _frmListReturPenjualanProduk);
        }

        private void mnuProfilPerusahaan_Click(object sender, EventArgs e)
        {
            var header = GetMenuTitle(sender);
            var menuName = GetMenuName(sender);

            if (RolePrivilegeHelper.IsHaveHakAkses(menuName, MainProgram.pengguna, GrantState.UPDATE))
            {
                var frmProfil = new FrmProfilPerusahaan(header, MainProgram.profil);
                frmProfil.Listener = this;
                frmProfil.ShowDialog();
            }
            else
                MsgHelper.MsgWarning("Maaf Anda tidak mempunyai otoritas untuk mengakses menu ini");
        }

        private void mnuPengaturanUmum_Click(object sender, EventArgs e)
        {
            var header = GetMenuTitle(sender);
            var menuName = GetMenuName(sender);

            if (RolePrivilegeHelper.IsHaveHakAkses(menuName, MainProgram.pengguna, GrantState.UPDATE))
            {
                var frmPengaturan = new FrmPengaturanUmum(header, MainProgram.pengaturanUmum);
                frmPengaturan.ShowDialog();
            }
            else
                MsgHelper.MsgWarning("Maaf Anda tidak mempunyai otoritas untuk mengakses menu ini");
        }

        private void mnuGantiUser_Click(object sender, EventArgs e)
        {            
            if (MsgHelper.MsgKonfirmasi("Apakah proses ingin dilanjutkan ?"))
            {
                using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                {
                    AutoUpdater.CheckForUpdateEvent -= AutoUpdaterOnCheckForUpdateEvent;
                    CloseAllDocuments();

                    this.IsLogout = true;
                    this.Close();
                }
            }
        }

        private void mnuKeluarDariProgram_Click(object sender, EventArgs e)
        {
            if (MsgHelper.MsgKonfirmasi("Apakah proses ingin dilanjutkan ?"))
            {
                using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                {
                    CloseAllDocuments();
                    this.Close();
                }
            }
        }

        private void OpenUrl(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        private void mnuBlogOpenRetail_Click(object sender, EventArgs e)
        {
            var url = _openRetailBaseUrl;
            OpenUrl(url);
        }

        private void mnuFanPageOpenRetail_Click(object sender, EventArgs e)
        {
            var url = "https://www.facebook.com/openretail/";
            OpenUrl(url);
        }

        private void mnuPetunjukPenggunaanOpenRetail_Click(object sender, EventArgs e)
        {
            var url = _openRetailBaseUrl + "/petunjuk-penggunaan-openretail/";
            OpenUrl(url);
        }

        private void mnuRegistrasi_Click(object sender, EventArgs e)
        {
            var url = _openRetailBaseUrl + "/registrasi/";
            OpenUrl(url);
        }        

        private void mnuDukungPengembanganOpenRetail_Click(object sender, EventArgs e)
        {
            var url = _openRetailBaseUrl + "/kontribusi/";
            OpenUrl(url);
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            var frmAbout = new FrmAbout();
            frmAbout.ShowDialog();
        }

        private void mnuLapPembelianProduk_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapPembelianProduk>(sender);
        }

        public void Ok(object sender, object data)
        {
            if (data is Profil)
            {
                MainProgram.profil = (Profil)data;
                InitializeStatusBar();
            }
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            throw new NotImplementedException();
        }

        private void mnuLapHutangPembelianProduk_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapHutangPembelianProduk>(sender);
        }

        private void mnuLapPembayaranHutangPembelianProduk_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapPembayaranHutangPembelianProduk>(sender);
        }

        private void mnuLapKartuHutangPembelianProduk_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapKartuHutangPembelianProduk>(sender);
        }

        private void mnuLapReturPembelianProduk_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapReturPembelianProduk>(sender);
        }

        private void mnuLapPenjualanProduk_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapPenjualanProduk>(sender);
        }

        private void mnuLapPenjualanPerProduk_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapPenjualanPerProduk>(sender);
        }

        private void mnuLapPiutangPenjualanProduk_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapPiutangPenjualanProduk>(sender);
        }

        private void mnuLapPembayaranPiutangPenjualanProduk_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapPembayaranPiutangPenjualanProduk>(sender);
        }

        private void mnuLapKartuPiutangPenjualanProduk_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapKartuPiutangPenjualanProduk>(sender);
        }

        private void mnuLapReturPenjualanProduk_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapReturPenjualanProduk>(sender);
        }

        private void mnuLapStokProduk_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapStokProduk>(sender);
        }

        private void mnuLapPenyesuaianStok_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapPenyesuaianStok>(sender);
        }

        private void mnuPengeluaranBiaya_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListPengeluaranBiaya>(sender, ref _frmListPengeluaranBiaya);
        }

        private void mnuLapPengeluaranBiaya_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapPengeluaranBiaya>(sender);
        }

        private void mnuKasbon_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListKasbon>(sender, ref _frmListKasbon);
        }

        private void mnuLapKasbon_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapKasbon>(sender);
        }

        private void mnuPenggajian_Click(object sender, EventArgs e)
        {
            ShowForm<FrmListPenggajianKaryawan>(sender, ref _frmListPenggajianKaryawan);
        }

        private void mnuLapPenggajian_Click(object sender, EventArgs e)
        {
            ShowFormDialog<FrmLapPenggajianKaryawan>(sender);
        }

        private void mnuCekUpdateTerbaru_Click(object sender, EventArgs e)
        {
            if (MainProgram.onlineUpdateUrlInfo.Length > 0)
            {
                using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                {
                    AutoUpdater.Start(MainProgram.onlineUpdateUrlInfo);

                    while (!_lightSleeper.HasBeenCanceled)
                    {
                        _lightSleeper.Sleep(10000);
                    } 
                }
            }
            else
                MsgHelper.MsgWarning("Maaf link/url Online Update belum diset !!!\nProses cek update terbaru batal.");
        }
    }
}
