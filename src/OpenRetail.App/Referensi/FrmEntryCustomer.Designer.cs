namespace OpenRetail.App.Referensi
{
    partial class FrmEntryCustomer
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
            this.txtCustomer = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAlamat = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtKontak = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtTelepon = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtPlafonPiutang = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtKodePos = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDiskon = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbProvinsi = new System.Windows.Forms.ComboBox();
            this.cmbKabupaten = new System.Windows.Forms.ComboBox();
            this.cmbKecamatan = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtCustomer, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.txtAlamat, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.txtKontak, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.txtTelepon, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.txtPlafonPiutang, 1, 9);
            this.tableLayoutPanel3.Controls.Add(this.txtKodePos, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.label9, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.label10, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.txtDiskon, 1, 8);
            this.tableLayoutPanel3.Controls.Add(this.label13, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label12, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.cmbProvinsi, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbKabupaten, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.cmbKecamatan, 1, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 11;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(577, 252);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Customer";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCustomer
            // 
            this.txtCustomer.AutoEnter = true;
            this.txtCustomer.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtCustomer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCustomer.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtCustomer.LeaveFocusColor = System.Drawing.Color.White;
            this.txtCustomer.LetterOnly = false;
            this.txtCustomer.Location = new System.Drawing.Point(95, 3);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.NumericOnly = false;
            this.txtCustomer.SelectionText = false;
            this.txtCustomer.Size = new System.Drawing.Size(479, 20);
            this.txtCustomer.TabIndex = 0;
            this.txtCustomer.Tag = "nama_customer";
            this.txtCustomer.ThousandSeparator = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Alamat";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "Kontak";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "Telepon/HP";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 225);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 25);
            this.label5.TabIndex = 1;
            this.label5.Text = "Plafon Piutang";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtAlamat
            // 
            this.txtAlamat.AutoEnter = true;
            this.txtAlamat.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtAlamat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAlamat.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtAlamat.LeaveFocusColor = System.Drawing.Color.White;
            this.txtAlamat.LetterOnly = false;
            this.txtAlamat.Location = new System.Drawing.Point(95, 103);
            this.txtAlamat.Name = "txtAlamat";
            this.txtAlamat.NumericOnly = false;
            this.txtAlamat.SelectionText = false;
            this.txtAlamat.Size = new System.Drawing.Size(479, 20);
            this.txtAlamat.TabIndex = 4;
            this.txtAlamat.Tag = "alamat";
            this.txtAlamat.ThousandSeparator = false;
            // 
            // txtKontak
            // 
            this.txtKontak.AutoEnter = true;
            this.txtKontak.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtKontak.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKontak.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtKontak.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKontak.LetterOnly = false;
            this.txtKontak.Location = new System.Drawing.Point(95, 153);
            this.txtKontak.Name = "txtKontak";
            this.txtKontak.NumericOnly = false;
            this.txtKontak.SelectionText = false;
            this.txtKontak.Size = new System.Drawing.Size(479, 20);
            this.txtKontak.TabIndex = 6;
            this.txtKontak.Tag = "kontak";
            this.txtKontak.ThousandSeparator = false;
            // 
            // txtTelepon
            // 
            this.txtTelepon.AutoEnter = true;
            this.txtTelepon.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtTelepon.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtTelepon.LeaveFocusColor = System.Drawing.Color.White;
            this.txtTelepon.LetterOnly = false;
            this.txtTelepon.Location = new System.Drawing.Point(95, 178);
            this.txtTelepon.Name = "txtTelepon";
            this.txtTelepon.NumericOnly = false;
            this.txtTelepon.SelectionText = false;
            this.txtTelepon.Size = new System.Drawing.Size(115, 20);
            this.txtTelepon.TabIndex = 7;
            this.txtTelepon.Tag = "telepon";
            this.txtTelepon.ThousandSeparator = false;
            // 
            // txtPlafonPiutang
            // 
            this.txtPlafonPiutang.AutoEnter = false;
            this.txtPlafonPiutang.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtPlafonPiutang.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtPlafonPiutang.LeaveFocusColor = System.Drawing.Color.White;
            this.txtPlafonPiutang.LetterOnly = false;
            this.txtPlafonPiutang.Location = new System.Drawing.Point(95, 228);
            this.txtPlafonPiutang.MaxLength = 20;
            this.txtPlafonPiutang.Name = "txtPlafonPiutang";
            this.txtPlafonPiutang.NumericOnly = true;
            this.txtPlafonPiutang.SelectionText = false;
            this.txtPlafonPiutang.Size = new System.Drawing.Size(115, 20);
            this.txtPlafonPiutang.TabIndex = 9;
            this.txtPlafonPiutang.Text = "0";
            this.txtPlafonPiutang.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPlafonPiutang.ThousandSeparator = true;
            this.txtPlafonPiutang.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPlafonPiutang_KeyPress);
            // 
            // txtKodePos
            // 
            this.txtKodePos.AutoEnter = true;
            this.txtKodePos.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtKodePos.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtKodePos.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKodePos.LetterOnly = false;
            this.txtKodePos.Location = new System.Drawing.Point(95, 128);
            this.txtKodePos.Name = "txtKodePos";
            this.txtKodePos.NumericOnly = false;
            this.txtKodePos.SelectionText = false;
            this.txtKodePos.Size = new System.Drawing.Size(66, 20);
            this.txtKodePos.TabIndex = 5;
            this.txtKodePos.Tag = "kode_pos";
            this.txtKodePos.ThousandSeparator = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Left;
            this.label9.Location = new System.Drawing.Point(3, 125);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 25);
            this.label9.TabIndex = 5;
            this.label9.Text = "Kode Pos";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Left;
            this.label10.Location = new System.Drawing.Point(3, 200);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 25);
            this.label10.TabIndex = 9;
            this.label10.Text = "Diskon Reseller";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDiskon
            // 
            this.txtDiskon.AutoEnter = true;
            this.txtDiskon.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtDiskon.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtDiskon.LeaveFocusColor = System.Drawing.Color.White;
            this.txtDiskon.LetterOnly = false;
            this.txtDiskon.Location = new System.Drawing.Point(95, 203);
            this.txtDiskon.MaxLength = 5;
            this.txtDiskon.Name = "txtDiskon";
            this.txtDiskon.NumericOnly = true;
            this.txtDiskon.SelectionText = false;
            this.txtDiskon.Size = new System.Drawing.Size(43, 20);
            this.txtDiskon.TabIndex = 8;
            this.txtDiskon.Text = "0";
            this.txtDiskon.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDiskon.ThousandSeparator = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Dock = System.Windows.Forms.DockStyle.Left;
            this.label13.Location = new System.Drawing.Point(3, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(44, 25);
            this.label13.TabIndex = 12;
            this.label13.Text = "Provinsi";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Left;
            this.label12.Location = new System.Drawing.Point(3, 50);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 25);
            this.label12.TabIndex = 5;
            this.label12.Text = "Kota/Kabupaten";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(3, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 25);
            this.label6.TabIndex = 5;
            this.label6.Text = "Kecamatan";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbProvinsi
            // 
            this.cmbProvinsi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbProvinsi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProvinsi.FormattingEnabled = true;
            this.cmbProvinsi.Location = new System.Drawing.Point(95, 28);
            this.cmbProvinsi.Name = "cmbProvinsi";
            this.cmbProvinsi.Size = new System.Drawing.Size(479, 21);
            this.cmbProvinsi.TabIndex = 1;
            this.cmbProvinsi.Tag = "ignore";
            this.cmbProvinsi.SelectedIndexChanged += new System.EventHandler(this.cmbProvinsi_SelectedIndexChanged);
            // 
            // cmbKabupaten
            // 
            this.cmbKabupaten.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbKabupaten.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKabupaten.FormattingEnabled = true;
            this.cmbKabupaten.Location = new System.Drawing.Point(95, 53);
            this.cmbKabupaten.Name = "cmbKabupaten";
            this.cmbKabupaten.Size = new System.Drawing.Size(479, 21);
            this.cmbKabupaten.TabIndex = 2;
            this.cmbKabupaten.Tag = "ignore";
            this.cmbKabupaten.SelectedIndexChanged += new System.EventHandler(this.cmbKabupaten_SelectedIndexChanged);
            // 
            // cmbKecamatan
            // 
            this.cmbKecamatan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbKecamatan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKecamatan.FormattingEnabled = true;
            this.cmbKecamatan.Location = new System.Drawing.Point(95, 78);
            this.cmbKecamatan.Name = "cmbKecamatan";
            this.cmbKecamatan.Size = new System.Drawing.Size(479, 21);
            this.cmbKecamatan.TabIndex = 3;
            this.cmbKecamatan.Tag = "ignore";
            // 
            // FrmEntryCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 334);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmEntryCustomer";
            this.Text = "FrmEntryCustomer";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private OpenRetail.Helper.UserControl.AdvancedTextbox txtCustomer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private OpenRetail.Helper.UserControl.AdvancedTextbox txtAlamat;
        private OpenRetail.Helper.UserControl.AdvancedTextbox txtKontak;
        private OpenRetail.Helper.UserControl.AdvancedTextbox txtTelepon;
        private OpenRetail.Helper.UserControl.AdvancedTextbox txtPlafonPiutang;
        private System.Windows.Forms.Label label6;
        private OpenRetail.Helper.UserControl.AdvancedTextbox txtKodePos;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private OpenRetail.Helper.UserControl.AdvancedTextbox txtDiskon;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbProvinsi;
        private System.Windows.Forms.ComboBox cmbKabupaten;
        private System.Windows.Forms.ComboBox cmbKecamatan;
    }
}