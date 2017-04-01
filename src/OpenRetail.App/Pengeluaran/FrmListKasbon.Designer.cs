namespace OpenRetail.App.Pengeluaran
{
    partial class FrmListKasbon
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
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.gridList = new Syncfusion.Windows.Forms.Grid.GridListControl();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.gridListHistoriPembayaran = new Syncfusion.Windows.Forms.Grid.GridListControl();
            this.pnlFooter2 = new System.Windows.Forms.Panel();
            this.btnHapusPembayaran = new System.Windows.Forms.Button();
            this.btnPerbaikiPembayaran = new System.Windows.Forms.Button();
            this.btnTambahPembayaran = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.filterRangeTanggal = new OpenRetail.App.UserControl.FilterRangeTanggal();
            this.chkTampilkanYangBelumLunas = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridListHistoriPembayaran)).BeginInit();
            this.pnlFooter2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(789, 367);
            this.tableLayoutPanel3.TabIndex = 6;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tableLayoutPanel4.Controls.Add(this.gridList, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 32);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(783, 332);
            this.tableLayoutPanel4.TabIndex = 3;
            // 
            // gridList
            // 
            this.gridList.BackColor = System.Drawing.SystemColors.Control;
            this.gridList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridList.ItemHeight = 17;
            this.gridList.Location = new System.Drawing.Point(3, 3);
            this.gridList.MultiColumn = false;
            this.gridList.Name = "gridList";
            this.gridList.Properties.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridList.Properties.ForceImmediateRepaint = false;
            this.gridList.Properties.MarkColHeader = false;
            this.gridList.Properties.MarkRowHeader = false;
            this.gridList.SelectedIndex = -1;
            this.gridList.Size = new System.Drawing.Size(502, 326);
            this.gridList.TabIndex = 1;
            this.gridList.TopIndex = 0;
            this.gridList.DoubleClick += new System.EventHandler(this.gridList_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(511, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(269, 326);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " [ Histori Pembayaran ] ";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.gridListHistoriPembayaran, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.pnlFooter2, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(263, 307);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // gridListHistoriPembayaran
            // 
            this.gridListHistoriPembayaran.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridListHistoriPembayaran.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridListHistoriPembayaran.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridListHistoriPembayaran.ItemHeight = 17;
            this.gridListHistoriPembayaran.Location = new System.Drawing.Point(3, 3);
            this.gridListHistoriPembayaran.MultiColumn = false;
            this.gridListHistoriPembayaran.Name = "gridListHistoriPembayaran";
            this.gridListHistoriPembayaran.Properties.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridListHistoriPembayaran.Properties.ForceImmediateRepaint = false;
            this.gridListHistoriPembayaran.Properties.MarkColHeader = false;
            this.gridListHistoriPembayaran.Properties.MarkRowHeader = false;
            this.gridListHistoriPembayaran.SelectedIndex = -1;
            this.gridListHistoriPembayaran.Size = new System.Drawing.Size(257, 261);
            this.gridListHistoriPembayaran.TabIndex = 0;
            this.gridListHistoriPembayaran.TopIndex = 0;
            // 
            // pnlFooter2
            // 
            this.pnlFooter2.Controls.Add(this.btnHapusPembayaran);
            this.pnlFooter2.Controls.Add(this.btnPerbaikiPembayaran);
            this.pnlFooter2.Controls.Add(this.btnTambahPembayaran);
            this.pnlFooter2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFooter2.Location = new System.Drawing.Point(3, 267);
            this.pnlFooter2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.pnlFooter2.Name = "pnlFooter2";
            this.pnlFooter2.Size = new System.Drawing.Size(257, 40);
            this.pnlFooter2.TabIndex = 1;
            // 
            // btnHapusPembayaran
            // 
            this.btnHapusPembayaran.Enabled = false;
            this.btnHapusPembayaran.Location = new System.Drawing.Point(171, 8);
            this.btnHapusPembayaran.Name = "btnHapusPembayaran";
            this.btnHapusPembayaran.Size = new System.Drawing.Size(75, 23);
            this.btnHapusPembayaran.TabIndex = 0;
            this.btnHapusPembayaran.Text = "Hapus";
            this.btnHapusPembayaran.UseVisualStyleBackColor = true;
            this.btnHapusPembayaran.Click += new System.EventHandler(this.btnHapusPembayaran_Click);
            // 
            // btnPerbaikiPembayaran
            // 
            this.btnPerbaikiPembayaran.Enabled = false;
            this.btnPerbaikiPembayaran.Location = new System.Drawing.Point(90, 8);
            this.btnPerbaikiPembayaran.Name = "btnPerbaikiPembayaran";
            this.btnPerbaikiPembayaran.Size = new System.Drawing.Size(75, 23);
            this.btnPerbaikiPembayaran.TabIndex = 0;
            this.btnPerbaikiPembayaran.Text = "Perbaiki";
            this.btnPerbaikiPembayaran.UseVisualStyleBackColor = true;
            this.btnPerbaikiPembayaran.Click += new System.EventHandler(this.btnPerbaikiPembayaran_Click);
            // 
            // btnTambahPembayaran
            // 
            this.btnTambahPembayaran.Location = new System.Drawing.Point(9, 8);
            this.btnTambahPembayaran.Name = "btnTambahPembayaran";
            this.btnTambahPembayaran.Size = new System.Drawing.Size(75, 23);
            this.btnTambahPembayaran.TabIndex = 0;
            this.btnTambahPembayaran.Text = "Tambah";
            this.btnTambahPembayaran.UseVisualStyleBackColor = true;
            this.btnTambahPembayaran.Click += new System.EventHandler(this.btnTambahPembayaran_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.filterRangeTanggal);
            this.flowLayoutPanel1.Controls.Add(this.chkTampilkanYangBelumLunas);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(789, 29);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // filterRangeTanggal
            // 
            this.filterRangeTanggal.Location = new System.Drawing.Point(3, 3);
            this.filterRangeTanggal.Name = "filterRangeTanggal";
            this.filterRangeTanggal.Size = new System.Drawing.Size(469, 23);
            this.filterRangeTanggal.TabIndex = 2;
            this.filterRangeTanggal.BtnTampilkanClicked += new OpenRetail.App.UserControl.FilterRangeTanggal.EventHandler(this.filterRangeTanggal_BtnTampilkanClicked);
            this.filterRangeTanggal.ChkTampilkanSemuaDataClicked += new OpenRetail.App.UserControl.FilterRangeTanggal.EventHandler(this.filterRangeTanggal_ChkTampilkanSemuaDataClicked);
            // 
            // chkTampilkanYangBelumLunas
            // 
            this.chkTampilkanYangBelumLunas.AutoSize = true;
            this.chkTampilkanYangBelumLunas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkTampilkanYangBelumLunas.Location = new System.Drawing.Point(478, 6);
            this.chkTampilkanYangBelumLunas.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.chkTampilkanYangBelumLunas.Name = "chkTampilkanYangBelumLunas";
            this.chkTampilkanYangBelumLunas.Size = new System.Drawing.Size(160, 20);
            this.chkTampilkanYangBelumLunas.TabIndex = 3;
            this.chkTampilkanYangBelumLunas.Text = "Tampilkan yang belum lunas";
            this.chkTampilkanYangBelumLunas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkTampilkanYangBelumLunas.UseVisualStyleBackColor = true;
            this.chkTampilkanYangBelumLunas.CheckedChanged += new System.EventHandler(this.chkTampilkanYangBelumLunas_CheckedChanged);
            // 
            // FrmListKasbon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 449);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmListKasbon";
            this.Text = "FrmListKasbon";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridListHistoriPembayaran)).EndInit();
            this.pnlFooter2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Syncfusion.Windows.Forms.Grid.GridListControl gridList;
        private UserControl.FilterRangeTanggal filterRangeTanggal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private Syncfusion.Windows.Forms.Grid.GridListControl gridListHistoriPembayaran;
        private System.Windows.Forms.Panel pnlFooter2;
        private System.Windows.Forms.Button btnTambahPembayaran;
        private System.Windows.Forms.Button btnHapusPembayaran;
        private System.Windows.Forms.Button btnPerbaikiPembayaran;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox chkTampilkanYangBelumLunas;
    }
}