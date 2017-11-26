namespace OpenRetail.App.Cashier.Main
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.statusStripEx1 = new Syncfusion.Windows.Forms.Tools.StatusStripEx();
            this.sbJam = new Syncfusion.Windows.Forms.Tools.StatusStripLabel();
            this.statusStripLabel2 = new Syncfusion.Windows.Forms.Tools.StatusStripLabel();
            this.sbTanggal = new Syncfusion.Windows.Forms.Tools.StatusStripLabel();
            this.statusStripLabel4 = new Syncfusion.Windows.Forms.Tools.StatusStripLabel();
            this.sbOperator = new Syncfusion.Windows.Forms.Tools.StatusStripLabel();
            this.statusStripLabel6 = new Syncfusion.Windows.Forms.Tools.StatusStripLabel();
            this.sbNamaAplikasi = new Syncfusion.Windows.Forms.Tools.StatusStripLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuTransaksi = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPenjualanProduk = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLaporan = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLapPenjualanProduk = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPengaturan = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSettingPrinter = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBantuan = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBlogOpenRetail = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFanPageOpenRetail = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuPetunjukPenggunaanOpenRetail = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRegistrasi = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDukungPengembanganOpenRetail = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOnlineUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCekUpdateTerbaru = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuKeluar = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuGantiUser = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuKeluarDariProgram = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.mainDock = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tbPenjualanProduk = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.tbLapPenjualanProduk = new System.Windows.Forms.ToolStripButton();
            this.mnuGroupOpenRetail = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripEx1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStripEx1
            // 
            this.statusStripEx1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStripEx1.BeforeTouchSize = new System.Drawing.Size(827, 22);
            this.statusStripEx1.Dock = Syncfusion.Windows.Forms.Tools.DockStyleEx.Bottom;
            this.statusStripEx1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbJam,
            this.statusStripLabel2,
            this.sbTanggal,
            this.statusStripLabel4,
            this.sbOperator,
            this.statusStripLabel6,
            this.sbNamaAplikasi});
            this.statusStripEx1.Location = new System.Drawing.Point(0, 388);
            this.statusStripEx1.MetroColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(206)))), ((int)(((byte)(255)))));
            this.statusStripEx1.Name = "statusStripEx1";
            this.statusStripEx1.OfficeColorScheme = Syncfusion.Windows.Forms.Tools.ToolStripEx.ColorScheme.Silver;
            this.statusStripEx1.Size = new System.Drawing.Size(827, 22);
            this.statusStripEx1.TabIndex = 3;
            this.statusStripEx1.Text = "statusStripEx1";
            // 
            // sbJam
            // 
            this.sbJam.Image = global::OpenRetail.App.Cashier.Properties.Resources.clock32;
            this.sbJam.Margin = new System.Windows.Forms.Padding(0, 4, 0, 2);
            this.sbJam.Name = "sbJam";
            this.sbJam.Size = new System.Drawing.Size(65, 16);
            this.sbJam.Text = "00:00:00";
            // 
            // statusStripLabel2
            // 
            this.statusStripLabel2.Margin = new System.Windows.Forms.Padding(0, 4, 0, 2);
            this.statusStripLabel2.Name = "statusStripLabel2";
            this.statusStripLabel2.Size = new System.Drawing.Size(10, 15);
            this.statusStripLabel2.Text = "|";
            // 
            // sbTanggal
            // 
            this.sbTanggal.Image = global::OpenRetail.App.Cashier.Properties.Resources.calendar32;
            this.sbTanggal.Margin = new System.Windows.Forms.Padding(0, 4, 0, 2);
            this.sbTanggal.Name = "sbTanggal";
            this.sbTanggal.Size = new System.Drawing.Size(60, 16);
            this.sbTanggal.Text = "Hari, ...";
            // 
            // statusStripLabel4
            // 
            this.statusStripLabel4.Margin = new System.Windows.Forms.Padding(0, 4, 0, 2);
            this.statusStripLabel4.Name = "statusStripLabel4";
            this.statusStripLabel4.Size = new System.Drawing.Size(10, 15);
            this.statusStripLabel4.Text = "|";
            // 
            // sbOperator
            // 
            this.sbOperator.Image = global::OpenRetail.App.Cashier.Properties.Resources.user32;
            this.sbOperator.Margin = new System.Windows.Forms.Padding(0, 4, 0, 2);
            this.sbOperator.Name = "sbOperator";
            this.sbOperator.Size = new System.Drawing.Size(77, 16);
            this.sbOperator.Text = "operator...";
            // 
            // statusStripLabel6
            // 
            this.statusStripLabel6.Margin = new System.Windows.Forms.Padding(0, 4, 0, 2);
            this.statusStripLabel6.Name = "statusStripLabel6";
            this.statusStripLabel6.Size = new System.Drawing.Size(10, 15);
            this.statusStripLabel6.Text = "|";
            // 
            // sbNamaAplikasi
            // 
            this.sbNamaAplikasi.Margin = new System.Windows.Forms.Padding(0, 4, 0, 2);
            this.sbNamaAplikasi.Name = "sbNamaAplikasi";
            this.sbNamaAplikasi.Size = new System.Drawing.Size(53, 15);
            this.sbNamaAplikasi.Text = "sistem ...";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTransaksi,
            this.mnuLaporan,
            this.mnuPengaturan,
            this.mnuBantuan,
            this.mnuOnlineUpdate,
            this.mnuKeluar});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(827, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuTransaksi
            // 
            this.mnuTransaksi.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuPenjualanProduk});
            this.mnuTransaksi.Name = "mnuTransaksi";
            this.mnuTransaksi.Size = new System.Drawing.Size(68, 20);
            this.mnuTransaksi.Text = "Transaksi";
            // 
            // mnuPenjualanProduk
            // 
            this.mnuPenjualanProduk.Name = "mnuPenjualanProduk";
            this.mnuPenjualanProduk.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
            this.mnuPenjualanProduk.Size = new System.Drawing.Size(205, 22);
            this.mnuPenjualanProduk.Tag = "FrmListPenjualanProduk";
            this.mnuPenjualanProduk.Text = "Penjualan Produk";
            this.mnuPenjualanProduk.Click += new System.EventHandler(this.mnuPenjualanProduk_Click);
            // 
            // mnuLaporan
            // 
            this.mnuLaporan.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLapPenjualanProduk});
            this.mnuLaporan.Name = "mnuLaporan";
            this.mnuLaporan.Size = new System.Drawing.Size(62, 20);
            this.mnuLaporan.Text = "Laporan";
            // 
            // mnuLapPenjualanProduk
            // 
            this.mnuLapPenjualanProduk.Name = "mnuLapPenjualanProduk";
            this.mnuLapPenjualanProduk.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.mnuLapPenjualanProduk.Size = new System.Drawing.Size(214, 22);
            this.mnuLapPenjualanProduk.Tag = "FrmLapPenjualanProduk";
            this.mnuLapPenjualanProduk.Text = "Penjualan Per Kasir";
            this.mnuLapPenjualanProduk.Click += new System.EventHandler(this.mnuLapPenjualanProduk_Click);
            // 
            // mnuPengaturan
            // 
            this.mnuPengaturan.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSettingPrinter});
            this.mnuPengaturan.Name = "mnuPengaturan";
            this.mnuPengaturan.Size = new System.Drawing.Size(80, 20);
            this.mnuPengaturan.Text = "Pengaturan";
            // 
            // mnuSettingPrinter
            // 
            this.mnuSettingPrinter.Name = "mnuSettingPrinter";
            this.mnuSettingPrinter.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.mnuSettingPrinter.Size = new System.Drawing.Size(190, 22);
            this.mnuSettingPrinter.Tag = "FrmPengaturanUmum";
            this.mnuSettingPrinter.Text = "Setting Printer";
            this.mnuSettingPrinter.Click += new System.EventHandler(this.mnuSettingPrinter_Click);
            // 
            // mnuBantuan
            // 
            this.mnuBantuan.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBlogOpenRetail,
            this.mnuFanPageOpenRetail,
            this.mnuGroupOpenRetail,
            this.toolStripSeparator16,
            this.mnuPetunjukPenggunaanOpenRetail,
            this.toolStripSeparator14,
            this.mnuRegistrasi,
            this.mnuDukungPengembanganOpenRetail,
            this.toolStripSeparator15,
            this.mnuAbout});
            this.mnuBantuan.Name = "mnuBantuan";
            this.mnuBantuan.Size = new System.Drawing.Size(63, 20);
            this.mnuBantuan.Text = "Bantuan";
            // 
            // mnuBlogOpenRetail
            // 
            this.mnuBlogOpenRetail.Name = "mnuBlogOpenRetail";
            this.mnuBlogOpenRetail.Size = new System.Drawing.Size(264, 22);
            this.mnuBlogOpenRetail.Text = "Blog OpenRetail";
            this.mnuBlogOpenRetail.Visible = false;
            // 
            // mnuFanPageOpenRetail
            // 
            this.mnuFanPageOpenRetail.Name = "mnuFanPageOpenRetail";
            this.mnuFanPageOpenRetail.Size = new System.Drawing.Size(264, 22);
            this.mnuFanPageOpenRetail.Text = "Fan Page OpenRetail";
            this.mnuFanPageOpenRetail.Click += new System.EventHandler(this.mnuFanPageOpenRetail_Click);
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(261, 6);
            // 
            // mnuPetunjukPenggunaanOpenRetail
            // 
            this.mnuPetunjukPenggunaanOpenRetail.Name = "mnuPetunjukPenggunaanOpenRetail";
            this.mnuPetunjukPenggunaanOpenRetail.Size = new System.Drawing.Size(264, 22);
            this.mnuPetunjukPenggunaanOpenRetail.Text = "Petunjuk Penggunaan OpenRetail";
            this.mnuPetunjukPenggunaanOpenRetail.Click += new System.EventHandler(this.mnuPetunjukPenggunaanOpenRetail_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            this.toolStripSeparator14.Size = new System.Drawing.Size(261, 6);
            // 
            // mnuRegistrasi
            // 
            this.mnuRegistrasi.Name = "mnuRegistrasi";
            this.mnuRegistrasi.Size = new System.Drawing.Size(264, 22);
            this.mnuRegistrasi.Text = "Registrasi";
            this.mnuRegistrasi.Click += new System.EventHandler(this.mnuRegistrasi_Click);
            // 
            // mnuDukungPengembanganOpenRetail
            // 
            this.mnuDukungPengembanganOpenRetail.Name = "mnuDukungPengembanganOpenRetail";
            this.mnuDukungPengembanganOpenRetail.Size = new System.Drawing.Size(264, 22);
            this.mnuDukungPengembanganOpenRetail.Text = "Dukung Pengembangan OpenRetail";
            this.mnuDukungPengembanganOpenRetail.Click += new System.EventHandler(this.mnuDukungPengembanganOpenRetail_Click);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            this.toolStripSeparator15.Size = new System.Drawing.Size(261, 6);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(264, 22);
            this.mnuAbout.Text = "About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // mnuOnlineUpdate
            // 
            this.mnuOnlineUpdate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCekUpdateTerbaru});
            this.mnuOnlineUpdate.Name = "mnuOnlineUpdate";
            this.mnuOnlineUpdate.Size = new System.Drawing.Size(95, 20);
            this.mnuOnlineUpdate.Text = "Online Update";
            // 
            // mnuCekUpdateTerbaru
            // 
            this.mnuCekUpdateTerbaru.Name = "mnuCekUpdateTerbaru";
            this.mnuCekUpdateTerbaru.Size = new System.Drawing.Size(179, 22);
            this.mnuCekUpdateTerbaru.Text = "Cek Update Terbaru";
            this.mnuCekUpdateTerbaru.Click += new System.EventHandler(this.mnuCekUpdateTerbaru_Click);
            // 
            // mnuKeluar
            // 
            this.mnuKeluar.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGantiUser,
            this.mnuKeluarDariProgram});
            this.mnuKeluar.Name = "mnuKeluar";
            this.mnuKeluar.Size = new System.Drawing.Size(52, 20);
            this.mnuKeluar.Text = "Keluar";
            // 
            // mnuGantiUser
            // 
            this.mnuGantiUser.Name = "mnuGantiUser";
            this.mnuGantiUser.Size = new System.Drawing.Size(174, 22);
            this.mnuGantiUser.Text = "Ganti User";
            this.mnuGantiUser.Click += new System.EventHandler(this.mnuGantiUser_Click);
            // 
            // mnuKeluarDariProgram
            // 
            this.mnuKeluarDariProgram.Name = "mnuKeluarDariProgram";
            this.mnuKeluarDariProgram.Size = new System.Drawing.Size(174, 22);
            this.mnuKeluarDariProgram.Text = "Keluar dari Aplikasi";
            this.mnuKeluarDariProgram.Click += new System.EventHandler(this.mnuKeluarDariProgram_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // mainDock
            // 
            this.mainDock.AllowEndUserDocking = false;
            this.mainDock.AllowEndUserNestedDocking = false;
            this.mainDock.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mainDock.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mainDock.BackgroundImage")));
            this.mainDock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainDock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainDock.Location = new System.Drawing.Point(0, 24);
            this.mainDock.Name = "mainDock";
            this.mainDock.Size = new System.Drawing.Size(827, 364);
            this.mainDock.TabIndex = 14;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbPenjualanProduk,
            this.toolStripSeparator12,
            this.tbLapPenjualanProduk});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(827, 39);
            this.toolStrip1.TabIndex = 19;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.Visible = false;
            // 
            // tbPenjualanProduk
            // 
            this.tbPenjualanProduk.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbPenjualanProduk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbPenjualanProduk.Name = "tbPenjualanProduk";
            this.tbPenjualanProduk.Size = new System.Drawing.Size(23, 36);
            this.tbPenjualanProduk.Tag = "FrmListPenjualanProduk";
            this.tbPenjualanProduk.Text = "Penjualan Produk";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 39);
            // 
            // tbLapPenjualanProduk
            // 
            this.tbLapPenjualanProduk.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbLapPenjualanProduk.Image = ((System.Drawing.Image)(resources.GetObject("tbLapPenjualanProduk.Image")));
            this.tbLapPenjualanProduk.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbLapPenjualanProduk.Name = "tbLapPenjualanProduk";
            this.tbLapPenjualanProduk.Size = new System.Drawing.Size(36, 36);
            this.tbLapPenjualanProduk.Tag = "FrmLapPenjualanProduk";
            this.tbLapPenjualanProduk.Text = "Laporan Penjualan Produk";
            // 
            // mnuGroupOpenRetail
            // 
            this.mnuGroupOpenRetail.Name = "mnuGroupOpenRetail";
            this.mnuGroupOpenRetail.Size = new System.Drawing.Size(264, 22);
            this.mnuGroupOpenRetail.Text = "Group OpenRetail";
            this.mnuGroupOpenRetail.Click += new System.EventHandler(this.mnuGroupOpenRetail_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 410);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.mainDock);
            this.Controls.Add(this.statusStripEx1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.statusStripEx1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Syncfusion.Windows.Forms.Tools.StatusStripEx statusStripEx1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuTransaksi;
        private System.Windows.Forms.ToolStripMenuItem mnuLaporan;
        private System.Windows.Forms.ToolStripMenuItem mnuPengaturan;
        private System.Windows.Forms.ToolStripMenuItem mnuKeluar;
        private System.Windows.Forms.ToolStripMenuItem mnuPenjualanProduk;
        private Syncfusion.Windows.Forms.Tools.StatusStripLabel sbJam;
        private Syncfusion.Windows.Forms.Tools.StatusStripLabel statusStripLabel2;
        private Syncfusion.Windows.Forms.Tools.StatusStripLabel sbTanggal;
        private Syncfusion.Windows.Forms.Tools.StatusStripLabel statusStripLabel4;
        private Syncfusion.Windows.Forms.Tools.StatusStripLabel sbOperator;
        private Syncfusion.Windows.Forms.Tools.StatusStripLabel statusStripLabel6;
        private Syncfusion.Windows.Forms.Tools.StatusStripLabel sbNamaAplikasi;
        private System.Windows.Forms.ToolStripMenuItem mnuLapPenjualanProduk;
        private System.Windows.Forms.ToolStripMenuItem mnuGantiUser;
        private System.Windows.Forms.ToolStripMenuItem mnuKeluarDariProgram;
        private System.Windows.Forms.Timer timer1;
        private WeifenLuo.WinFormsUI.Docking.DockPanel mainDock;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tbPenjualanProduk;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripButton tbLapPenjualanProduk;
        private System.Windows.Forms.ToolStripMenuItem mnuBantuan;
        private System.Windows.Forms.ToolStripMenuItem mnuRegistrasi;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStripMenuItem mnuSettingPrinter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripMenuItem mnuBlogOpenRetail;
        private System.Windows.Forms.ToolStripMenuItem mnuDukungPengembanganOpenRetail;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripMenuItem mnuOnlineUpdate;
        private System.Windows.Forms.ToolStripMenuItem mnuCekUpdateTerbaru;
        private System.Windows.Forms.ToolStripMenuItem mnuPetunjukPenggunaanOpenRetail;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripMenuItem mnuFanPageOpenRetail;
        private System.Windows.Forms.ToolStripMenuItem mnuGroupOpenRetail;
    }
}