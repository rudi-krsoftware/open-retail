namespace OpenRetail.App.Transaksi
{
    partial class FrmEntryAlamatKirim
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
            this.chkIsSdac = new System.Windows.Forms.CheckBox();
            this.pnlAlamatKirim = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtKepada = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtAlamat = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtKecamatan = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtKelurahan = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtKota = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtKodePos = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtTelepon = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlAlamatKirim.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.chkIsSdac, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.pnlAlamatKirim, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(459, 207);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // chkIsSdac
            // 
            this.chkIsSdac.AutoSize = true;
            this.chkIsSdac.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkIsSdac.Location = new System.Drawing.Point(3, 3);
            this.chkIsSdac.Name = "chkIsSdac";
            this.chkIsSdac.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.chkIsSdac.Size = new System.Drawing.Size(235, 19);
            this.chkIsSdac.TabIndex = 0;
            this.chkIsSdac.Text = "Alamat kirim sama dengan alamat customer";
            this.chkIsSdac.UseVisualStyleBackColor = true;
            this.chkIsSdac.CheckedChanged += new System.EventHandler(this.chkIsSdac_CheckedChanged);
            // 
            // pnlAlamatKirim
            // 
            this.pnlAlamatKirim.Controls.Add(this.tableLayoutPanel4);
            this.pnlAlamatKirim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlAlamatKirim.Location = new System.Drawing.Point(3, 28);
            this.pnlAlamatKirim.Name = "pnlAlamatKirim";
            this.pnlAlamatKirim.Size = new System.Drawing.Size(453, 176);
            this.pnlAlamatKirim.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.txtKepada, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.txtAlamat, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.txtKecamatan, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtKelurahan, 1, 3);
            this.tableLayoutPanel4.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel4.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel4.Controls.Add(this.txtKota, 1, 4);
            this.tableLayoutPanel4.Controls.Add(this.txtKodePos, 1, 5);
            this.tableLayoutPanel4.Controls.Add(this.label7, 0, 6);
            this.tableLayoutPanel4.Controls.Add(this.txtTelepon, 1, 6);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 8;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(453, 176);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kepada";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Alamat";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Kecamatan";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(3, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Kelurahan";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtKepada
            // 
            this.txtKepada.AutoEnter = true;
            this.txtKepada.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtKepada.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKepada.EnterFocusColor = System.Drawing.Color.White;
            this.txtKepada.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKepada.LetterOnly = false;
            this.txtKepada.Location = new System.Drawing.Point(70, 3);
            this.txtKepada.Name = "txtKepada";
            this.txtKepada.NumericOnly = false;
            this.txtKepada.SelectionText = false;
            this.txtKepada.Size = new System.Drawing.Size(380, 20);
            this.txtKepada.TabIndex = 0;
            this.txtKepada.Tag = "kepada";
            this.txtKepada.ThousandSeparator = false;
            // 
            // txtAlamat
            // 
            this.txtAlamat.AutoEnter = true;
            this.txtAlamat.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtAlamat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAlamat.EnterFocusColor = System.Drawing.Color.White;
            this.txtAlamat.LeaveFocusColor = System.Drawing.Color.White;
            this.txtAlamat.LetterOnly = false;
            this.txtAlamat.Location = new System.Drawing.Point(70, 28);
            this.txtAlamat.Name = "txtAlamat";
            this.txtAlamat.NumericOnly = false;
            this.txtAlamat.SelectionText = false;
            this.txtAlamat.Size = new System.Drawing.Size(380, 20);
            this.txtAlamat.TabIndex = 1;
            this.txtAlamat.Tag = "alamat";
            this.txtAlamat.ThousandSeparator = false;
            // 
            // txtKecamatan
            // 
            this.txtKecamatan.AutoEnter = true;
            this.txtKecamatan.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtKecamatan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKecamatan.EnterFocusColor = System.Drawing.Color.White;
            this.txtKecamatan.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKecamatan.LetterOnly = false;
            this.txtKecamatan.Location = new System.Drawing.Point(70, 53);
            this.txtKecamatan.Name = "txtKecamatan";
            this.txtKecamatan.NumericOnly = false;
            this.txtKecamatan.SelectionText = false;
            this.txtKecamatan.Size = new System.Drawing.Size(380, 20);
            this.txtKecamatan.TabIndex = 2;
            this.txtKecamatan.Tag = "kecamatan";
            this.txtKecamatan.ThousandSeparator = false;
            // 
            // txtKelurahan
            // 
            this.txtKelurahan.AutoEnter = true;
            this.txtKelurahan.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtKelurahan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKelurahan.EnterFocusColor = System.Drawing.Color.White;
            this.txtKelurahan.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKelurahan.LetterOnly = false;
            this.txtKelurahan.Location = new System.Drawing.Point(70, 78);
            this.txtKelurahan.Name = "txtKelurahan";
            this.txtKelurahan.NumericOnly = false;
            this.txtKelurahan.SelectionText = false;
            this.txtKelurahan.Size = new System.Drawing.Size(380, 20);
            this.txtKelurahan.TabIndex = 3;
            this.txtKelurahan.Tag = "kelurahan";
            this.txtKelurahan.ThousandSeparator = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(3, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 25);
            this.label5.TabIndex = 0;
            this.label5.Text = "Kota";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(3, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 25);
            this.label6.TabIndex = 0;
            this.label6.Text = "Kode Pos";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtKota
            // 
            this.txtKota.AutoEnter = true;
            this.txtKota.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtKota.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKota.EnterFocusColor = System.Drawing.Color.White;
            this.txtKota.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKota.LetterOnly = false;
            this.txtKota.Location = new System.Drawing.Point(70, 103);
            this.txtKota.Name = "txtKota";
            this.txtKota.NumericOnly = false;
            this.txtKota.SelectionText = false;
            this.txtKota.Size = new System.Drawing.Size(380, 20);
            this.txtKota.TabIndex = 4;
            this.txtKota.Tag = "kota";
            this.txtKota.ThousandSeparator = false;
            // 
            // txtKodePos
            // 
            this.txtKodePos.AutoEnter = true;
            this.txtKodePos.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtKodePos.EnterFocusColor = System.Drawing.Color.White;
            this.txtKodePos.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKodePos.LetterOnly = false;
            this.txtKodePos.Location = new System.Drawing.Point(70, 128);
            this.txtKodePos.Name = "txtKodePos";
            this.txtKodePos.NumericOnly = false;
            this.txtKodePos.SelectionText = false;
            this.txtKodePos.Size = new System.Drawing.Size(66, 20);
            this.txtKodePos.TabIndex = 5;
            this.txtKodePos.Tag = "kode_pos";
            this.txtKodePos.ThousandSeparator = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Left;
            this.label7.Location = new System.Drawing.Point(3, 150);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 25);
            this.label7.TabIndex = 6;
            this.label7.Text = "Telepon";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTelepon
            // 
            this.txtTelepon.AutoEnter = false;
            this.txtTelepon.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtTelepon.EnterFocusColor = System.Drawing.Color.White;
            this.txtTelepon.LeaveFocusColor = System.Drawing.Color.White;
            this.txtTelepon.LetterOnly = false;
            this.txtTelepon.Location = new System.Drawing.Point(70, 153);
            this.txtTelepon.Name = "txtTelepon";
            this.txtTelepon.NumericOnly = false;
            this.txtTelepon.SelectionText = false;
            this.txtTelepon.Size = new System.Drawing.Size(123, 20);
            this.txtTelepon.TabIndex = 6;
            this.txtTelepon.Tag = "telepon";
            this.txtTelepon.ThousandSeparator = false;
            this.txtTelepon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTelepon_KeyPress);
            // 
            // FrmEntryAlamatKirim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 289);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmEntryAlamatKirim";
            this.Text = "FrmEntryAlamatKirim";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.pnlAlamatKirim.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.CheckBox chkIsSdac;
        private System.Windows.Forms.Panel pnlAlamatKirim;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private UserControl.AdvancedTextbox txtKepada;
        private UserControl.AdvancedTextbox txtAlamat;
        private UserControl.AdvancedTextbox txtKecamatan;
        private UserControl.AdvancedTextbox txtKelurahan;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private UserControl.AdvancedTextbox txtKota;
        private UserControl.AdvancedTextbox txtKodePos;
        private System.Windows.Forms.Label label7;
        private UserControl.AdvancedTextbox txtTelepon;
    }
}