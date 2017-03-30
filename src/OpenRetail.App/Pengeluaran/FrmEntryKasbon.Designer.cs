namespace OpenRetail.App.Pengeluaran
{
    partial class FrmEntryKasbon
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbKaryawan = new System.Windows.Forms.ComboBox();
            this.dtpTanggal = new System.Windows.Forms.DateTimePicker();
            this.txtNota = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtJumlah = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtKeterangan = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.cmbKaryawan, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.dtpTanggal, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtNota, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtJumlah, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtKeterangan, 1, 4);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(419, 128);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nota";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tanggal";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "Karyawan";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 25);
            this.label5.TabIndex = 1;
            this.label5.Text = "Keterangan";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(62, 25);
            this.label6.TabIndex = 5;
            this.label6.Text = "Jumlah";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbKaryawan
            // 
            this.cmbKaryawan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKaryawan.FormattingEnabled = true;
            this.cmbKaryawan.Location = new System.Drawing.Point(71, 53);
            this.cmbKaryawan.Name = "cmbKaryawan";
            this.cmbKaryawan.Size = new System.Drawing.Size(345, 21);
            this.cmbKaryawan.TabIndex = 2;
            this.cmbKaryawan.Tag = "ignore";
            // 
            // dtpTanggal
            // 
            this.dtpTanggal.Location = new System.Drawing.Point(71, 28);
            this.dtpTanggal.Name = "dtpTanggal";
            this.dtpTanggal.Size = new System.Drawing.Size(200, 20);
            this.dtpTanggal.TabIndex = 1;
            // 
            // txtNota
            // 
            this.txtNota.AutoEnter = true;
            this.txtNota.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtNota.EnterFocusColor = System.Drawing.Color.White;
            this.txtNota.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNota.LetterOnly = false;
            this.txtNota.Location = new System.Drawing.Point(71, 3);
            this.txtNota.Name = "txtNota";
            this.txtNota.NumericOnly = false;
            this.txtNota.SelectionText = false;
            this.txtNota.Size = new System.Drawing.Size(100, 20);
            this.txtNota.TabIndex = 0;
            this.txtNota.Tag = "nota";
            this.txtNota.ThousandSeparator = false;
            // 
            // txtJumlah
            // 
            this.txtJumlah.AutoEnter = true;
            this.txtJumlah.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtJumlah.EnterFocusColor = System.Drawing.Color.White;
            this.txtJumlah.LeaveFocusColor = System.Drawing.Color.White;
            this.txtJumlah.LetterOnly = false;
            this.txtJumlah.Location = new System.Drawing.Point(71, 78);
            this.txtJumlah.MaxLength = 20;
            this.txtJumlah.Name = "txtJumlah";
            this.txtJumlah.NumericOnly = true;
            this.txtJumlah.SelectionText = false;
            this.txtJumlah.Size = new System.Drawing.Size(100, 20);
            this.txtJumlah.TabIndex = 3;
            this.txtJumlah.Tag = "nominal";
            this.txtJumlah.Text = "0";
            this.txtJumlah.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtJumlah.ThousandSeparator = true;
            // 
            // txtKeterangan
            // 
            this.txtKeterangan.AutoEnter = false;
            this.txtKeterangan.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtKeterangan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKeterangan.EnterFocusColor = System.Drawing.Color.White;
            this.txtKeterangan.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKeterangan.LetterOnly = false;
            this.txtKeterangan.Location = new System.Drawing.Point(71, 103);
            this.txtKeterangan.Name = "txtKeterangan";
            this.txtKeterangan.NumericOnly = false;
            this.txtKeterangan.SelectionText = false;
            this.txtKeterangan.Size = new System.Drawing.Size(345, 20);
            this.txtKeterangan.TabIndex = 4;
            this.txtKeterangan.Tag = "keterangan";
            this.txtKeterangan.ThousandSeparator = false;
            this.txtKeterangan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKeterangan_KeyPress);
            // 
            // FrmEntryKasbon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 210);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmEntryKasbon";
            this.Text = "FrmEntryKasbon";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbKaryawan;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private UserControl.AdvancedTextbox txtNota;
        private UserControl.AdvancedTextbox txtJumlah;
        private UserControl.AdvancedTextbox txtKeterangan;


    }
}