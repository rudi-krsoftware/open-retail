namespace OpenRetail.App.UI.Template
{
    partial class FrmSettingReportStandard
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnSelesai = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.chkListBox = new System.Windows.Forms.CheckedListBox();
            this.chkBoxTitle = new System.Windows.Forms.CheckBox();
            this.chkPilihSemua = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.rdoTanggal = new System.Windows.Forms.RadioButton();
            this.rdoBulan = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.dtpTanggalMulai = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpTanggalSelesai = new System.Windows.Forms.DateTimePicker();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.cmbBulan = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTahun = new System.Windows.Forms.ComboBox();
            this.chkTampilkanNota = new System.Windows.Forms.CheckBox();
            this.chkTampilkanRincianNota = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.77215F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.22785F));
            this.tableLayoutPanel1.Controls.Add(this.pnlFooter, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlHeader, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(670, 386);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pnlFooter, 2);
            this.pnlFooter.Controls.Add(this.btnSelesai);
            this.pnlFooter.Controls.Add(this.btnPreview);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFooter.Location = new System.Drawing.Point(3, 349);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(664, 34);
            this.pnlFooter.TabIndex = 3;
            // 
            // btnSelesai
            // 
            this.btnSelesai.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelesai.Location = new System.Drawing.Point(578, 6);
            this.btnSelesai.Name = "btnSelesai";
            this.btnSelesai.Size = new System.Drawing.Size(75, 23);
            this.btnSelesai.TabIndex = 3;
            this.btnSelesai.Text = "Esc Selesai";
            this.btnSelesai.UseVisualStyleBackColor = true;
            this.btnSelesai.Click += new System.EventHandler(this.btnSelesai_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreview.Location = new System.Drawing.Point(497, 6);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 23);
            this.btnPreview.TabIndex = 2;
            this.btnPreview.Tag = "3";
            this.btnPreview.Text = "F10 Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.pnlHeader, 2);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(3, 3);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(664, 34);
            this.pnlHeader.TabIndex = 3;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(3, 7);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(58, 17);
            this.lblHeader.TabIndex = 3;
            this.lblHeader.Text = "Header";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.chkListBox, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.chkBoxTitle, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.chkPilihSemua, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 43);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(273, 300);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // chkListBox
            // 
            this.chkListBox.CheckOnClick = true;
            this.chkListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkListBox.Enabled = false;
            this.chkListBox.FormattingEnabled = true;
            this.chkListBox.Location = new System.Drawing.Point(3, 28);
            this.chkListBox.Name = "chkListBox";
            this.chkListBox.Size = new System.Drawing.Size(267, 244);
            this.chkListBox.TabIndex = 1;
            // 
            // chkBoxTitle
            // 
            this.chkBoxTitle.AutoSize = true;
            this.chkBoxTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkBoxTitle.Location = new System.Drawing.Point(3, 3);
            this.chkBoxTitle.Name = "chkBoxTitle";
            this.chkBoxTitle.Size = new System.Drawing.Size(267, 19);
            this.chkBoxTitle.TabIndex = 0;
            this.chkBoxTitle.Text = "checkBox1";
            this.chkBoxTitle.UseVisualStyleBackColor = true;
            this.chkBoxTitle.CheckedChanged += new System.EventHandler(this.chkBoxTitle_CheckedChanged);
            // 
            // chkPilihSemua
            // 
            this.chkPilihSemua.AutoSize = true;
            this.chkPilihSemua.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkPilihSemua.Enabled = false;
            this.chkPilihSemua.Location = new System.Drawing.Point(3, 278);
            this.chkPilihSemua.Name = "chkPilihSemua";
            this.chkPilihSemua.Size = new System.Drawing.Size(267, 19);
            this.chkPilihSemua.TabIndex = 2;
            this.chkPilihSemua.Text = "Pilihan semua";
            this.chkPilihSemua.UseVisualStyleBackColor = true;
            this.chkPilihSemua.CheckedChanged += new System.EventHandler(this.chkPilihSemua_CheckedChanged);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.rdoTanggal, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rdoBulan, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.chkTampilkanNota, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.chkTampilkanRincianNota, 1, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(282, 43);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(385, 300);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // rdoTanggal
            // 
            this.rdoTanggal.AutoSize = true;
            this.rdoTanggal.Checked = true;
            this.rdoTanggal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoTanggal.Location = new System.Drawing.Point(3, 3);
            this.rdoTanggal.Name = "rdoTanggal";
            this.rdoTanggal.Size = new System.Drawing.Size(99, 19);
            this.rdoTanggal.TabIndex = 0;
            this.rdoTanggal.TabStop = true;
            this.rdoTanggal.Text = "Tanggal";
            this.rdoTanggal.UseVisualStyleBackColor = true;
            // 
            // rdoBulan
            // 
            this.rdoBulan.AutoSize = true;
            this.rdoBulan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoBulan.Location = new System.Drawing.Point(3, 28);
            this.rdoBulan.Name = "rdoBulan";
            this.rdoBulan.Size = new System.Drawing.Size(99, 19);
            this.rdoBulan.TabIndex = 1;
            this.rdoBulan.Text = "Bulan";
            this.rdoBulan.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.dtpTanggalMulai);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.dtpTanggalSelesai);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(105, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(280, 25);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // dtpTanggalMulai
            // 
            this.dtpTanggalMulai.CustomFormat = "dd/MM/yyyy";
            this.dtpTanggalMulai.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTanggalMulai.Location = new System.Drawing.Point(3, 3);
            this.dtpTanggalMulai.Name = "dtpTanggalMulai";
            this.dtpTanggalMulai.Size = new System.Drawing.Size(100, 20);
            this.dtpTanggalMulai.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(109, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 26);
            this.label1.TabIndex = 1;
            this.label1.Text = "s.d";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpTanggalSelesai
            // 
            this.dtpTanggalSelesai.CustomFormat = "dd/MM/yyyy";
            this.dtpTanggalSelesai.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTanggalSelesai.Location = new System.Drawing.Point(136, 3);
            this.dtpTanggalSelesai.Name = "dtpTanggalSelesai";
            this.dtpTanggalSelesai.Size = new System.Drawing.Size(100, 20);
            this.dtpTanggalSelesai.TabIndex = 1;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.cmbBulan);
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.cmbTahun);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(105, 25);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(280, 25);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // cmbBulan
            // 
            this.cmbBulan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBulan.FormattingEnabled = true;
            this.cmbBulan.Location = new System.Drawing.Point(3, 3);
            this.cmbBulan.Name = "cmbBulan";
            this.cmbBulan.Size = new System.Drawing.Size(100, 21);
            this.cmbBulan.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(109, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tahun";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbTahun
            // 
            this.cmbTahun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTahun.FormattingEnabled = true;
            this.cmbTahun.Location = new System.Drawing.Point(153, 3);
            this.cmbTahun.Name = "cmbTahun";
            this.cmbTahun.Size = new System.Drawing.Size(83, 21);
            this.cmbTahun.TabIndex = 1;
            // 
            // chkTampilkanNota
            // 
            this.chkTampilkanNota.AutoSize = true;
            this.chkTampilkanNota.Location = new System.Drawing.Point(3, 53);
            this.chkTampilkanNota.Name = "chkTampilkanNota";
            this.chkTampilkanNota.Size = new System.Drawing.Size(99, 17);
            this.chkTampilkanNota.TabIndex = 2;
            this.chkTampilkanNota.Text = "Tampilkan nota";
            this.chkTampilkanNota.UseVisualStyleBackColor = true;
            this.chkTampilkanNota.CheckedChanged += new System.EventHandler(this.chkTampilkanNota_CheckedChanged);
            // 
            // chkTampilkanRincianNota
            // 
            this.chkTampilkanRincianNota.AutoSize = true;
            this.chkTampilkanRincianNota.Enabled = false;
            this.chkTampilkanRincianNota.Location = new System.Drawing.Point(108, 53);
            this.chkTampilkanRincianNota.Name = "chkTampilkanRincianNota";
            this.chkTampilkanRincianNota.Size = new System.Drawing.Size(133, 17);
            this.chkTampilkanRincianNota.TabIndex = 3;
            this.chkTampilkanRincianNota.Text = "Tampilkan rincian nota";
            this.chkTampilkanRincianNota.UseVisualStyleBackColor = true;
            this.chkTampilkanRincianNota.Visible = false;
            // 
            // FrmSettingReportStandard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 386);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmSettingReportStandard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmSettingReportStandard";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSettingReportStandard_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmSettingReportStandard_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        protected System.Windows.Forms.CheckedListBox chkListBox;
        protected System.Windows.Forms.CheckBox chkBoxTitle;
        private System.Windows.Forms.CheckBox chkPilihSemua;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        protected System.Windows.Forms.RadioButton rdoTanggal;
        protected System.Windows.Forms.RadioButton rdoBulan;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        protected System.Windows.Forms.DateTimePicker dtpTanggalMulai;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.DateTimePicker dtpTanggalSelesai;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        protected System.Windows.Forms.ComboBox cmbBulan;
        private System.Windows.Forms.Label label2;
        protected System.Windows.Forms.ComboBox cmbTahun;
        protected System.Windows.Forms.CheckBox chkTampilkanNota;
        protected System.Windows.Forms.CheckBox chkTampilkanRincianNota;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnSelesai;
        protected System.Windows.Forms.Button btnPreview;
    }
}