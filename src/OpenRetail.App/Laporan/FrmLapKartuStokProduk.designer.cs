namespace OpenRetail.App.Laporan
{
    partial class FrmLapKartuStokProduk
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
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkListOfProduk = new System.Windows.Forms.CheckedListBox();
            this.txtNamaProduk = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkFilterTambahan = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.rdoTanggal, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rdoBulan, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel2, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(401, 53);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // rdoTanggal
            // 
            this.rdoTanggal.AutoSize = true;
            this.rdoTanggal.Checked = true;
            this.rdoTanggal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rdoTanggal.Location = new System.Drawing.Point(3, 3);
            this.rdoTanggal.Name = "rdoTanggal";
            this.rdoTanggal.Size = new System.Drawing.Size(64, 19);
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
            this.rdoBulan.Size = new System.Drawing.Size(64, 19);
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
            this.flowLayoutPanel1.Location = new System.Drawing.Point(70, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(331, 25);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // dtpTanggalMulai
            // 
            this.dtpTanggalMulai.CustomFormat = "dd/MM/yyyy";
            this.dtpTanggalMulai.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTanggalMulai.Location = new System.Drawing.Point(3, 3);
            this.dtpTanggalMulai.Name = "dtpTanggalMulai";
            this.dtpTanggalMulai.Size = new System.Drawing.Size(144, 20);
            this.dtpTanggalMulai.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(153, 0);
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
            this.dtpTanggalSelesai.Location = new System.Drawing.Point(180, 3);
            this.dtpTanggalSelesai.Name = "dtpTanggalSelesai";
            this.dtpTanggalSelesai.Size = new System.Drawing.Size(144, 20);
            this.dtpTanggalSelesai.TabIndex = 1;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.cmbBulan);
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.cmbTahun);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(70, 25);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(331, 25);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // cmbBulan
            // 
            this.cmbBulan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBulan.FormattingEnabled = true;
            this.cmbBulan.Location = new System.Drawing.Point(3, 3);
            this.cmbBulan.Name = "cmbBulan";
            this.cmbBulan.Size = new System.Drawing.Size(144, 21);
            this.cmbBulan.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(153, 0);
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
            this.cmbTahun.Location = new System.Drawing.Point(197, 3);
            this.cmbTahun.Name = "cmbTahun";
            this.cmbTahun.Size = new System.Drawing.Size(127, 21);
            this.cmbTahun.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 141F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(407, 238);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkFilterTambahan);
            this.groupBox1.Controls.Add(this.chkListOfProduk);
            this.groupBox1.Controls.Add(this.txtNamaProduk);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(401, 173);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "   ";
            // 
            // chkListOfProduk
            // 
            this.chkListOfProduk.CheckOnClick = true;
            this.chkListOfProduk.Enabled = false;
            this.chkListOfProduk.FormattingEnabled = true;
            this.chkListOfProduk.Location = new System.Drawing.Point(117, 43);
            this.chkListOfProduk.Name = "chkListOfProduk";
            this.chkListOfProduk.Size = new System.Drawing.Size(277, 124);
            this.chkListOfProduk.TabIndex = 1;
            // 
            // txtNamaProduk
            // 
            this.txtNamaProduk.AutoEnter = false;
            this.txtNamaProduk.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtNamaProduk.Enabled = false;
            this.txtNamaProduk.EnterFocusColor = System.Drawing.Color.White;
            this.txtNamaProduk.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNamaProduk.LetterOnly = false;
            this.txtNamaProduk.Location = new System.Drawing.Point(117, 17);
            this.txtNamaProduk.Name = "txtNamaProduk";
            this.txtNamaProduk.NumericOnly = false;
            this.txtNamaProduk.SelectionText = false;
            this.txtNamaProduk.Size = new System.Drawing.Size(237, 20);
            this.txtNamaProduk.TabIndex = 0;
            this.txtNamaProduk.ThousandSeparator = false;
            this.txtNamaProduk.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNamaProduk_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(360, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Enter";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Kode/Nama Produk";
            // 
            // chkFilterTambahan
            // 
            this.chkFilterTambahan.AutoSize = true;
            this.chkFilterTambahan.Location = new System.Drawing.Point(13, -1);
            this.chkFilterTambahan.Name = "chkFilterTambahan";
            this.chkFilterTambahan.Size = new System.Drawing.Size(102, 17);
            this.chkFilterTambahan.TabIndex = 2;
            this.chkFilterTambahan.Text = "Filter Tambahan";
            this.chkFilterTambahan.UseVisualStyleBackColor = true;
            this.chkFilterTambahan.CheckedChanged += new System.EventHandler(this.chkFilterTambahan_CheckedChanged);
            // 
            // FrmLapKartuStokProduk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 320);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Name = "FrmLapKartuStokProduk";
            this.Text = "FrmLapKartuStokProduk";
            this.Controls.SetChildIndex(this.tableLayoutPanel4, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton rdoTanggal;
        private System.Windows.Forms.RadioButton rdoBulan;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.DateTimePicker dtpTanggalMulai;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTanggalSelesai;
        private System.Windows.Forms.ComboBox cmbBulan;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbTahun;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox chkListOfProduk;
        private Helper.UserControl.AdvancedTextbox txtNamaProduk;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkFilterTambahan;
    }
}