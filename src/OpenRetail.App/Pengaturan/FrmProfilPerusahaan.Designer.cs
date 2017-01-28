namespace OpenRetail.App.Pengaturan
{
    partial class FrmProfilPerusahaan
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
            this.label4 = new System.Windows.Forms.Label();
            this.txtNamaPerusahaan = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtAlamat = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtKota = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtTelepon = new OpenRetail.App.UserControl.AdvancedTextbox();
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
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtNamaPerusahaan, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtAlamat, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtKota, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtTelepon, 1, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 5;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(439, 103);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nama Perusahaan";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Alamat";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "Kota";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "Telepon";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNamaPerusahaan
            // 
            this.txtNamaPerusahaan.AutoEnter = true;
            this.txtNamaPerusahaan.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtNamaPerusahaan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNamaPerusahaan.EnterFocusColor = System.Drawing.Color.White;
            this.txtNamaPerusahaan.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNamaPerusahaan.LetterOnly = false;
            this.txtNamaPerusahaan.Location = new System.Drawing.Point(104, 3);
            this.txtNamaPerusahaan.Name = "txtNamaPerusahaan";
            this.txtNamaPerusahaan.NumericOnly = false;
            this.txtNamaPerusahaan.SelectionText = false;
            this.txtNamaPerusahaan.Size = new System.Drawing.Size(332, 20);
            this.txtNamaPerusahaan.TabIndex = 0;
            this.txtNamaPerusahaan.Tag = "nama_profil";
            this.txtNamaPerusahaan.ThousandSeparator = false;
            // 
            // txtAlamat
            // 
            this.txtAlamat.AutoEnter = true;
            this.txtAlamat.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtAlamat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAlamat.EnterFocusColor = System.Drawing.Color.White;
            this.txtAlamat.LeaveFocusColor = System.Drawing.Color.White;
            this.txtAlamat.LetterOnly = false;
            this.txtAlamat.Location = new System.Drawing.Point(104, 28);
            this.txtAlamat.Name = "txtAlamat";
            this.txtAlamat.NumericOnly = false;
            this.txtAlamat.SelectionText = false;
            this.txtAlamat.Size = new System.Drawing.Size(332, 20);
            this.txtAlamat.TabIndex = 1;
            this.txtAlamat.Tag = "alamat";
            this.txtAlamat.ThousandSeparator = false;
            // 
            // txtKota
            // 
            this.txtKota.AutoEnter = true;
            this.txtKota.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtKota.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKota.EnterFocusColor = System.Drawing.Color.White;
            this.txtKota.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKota.LetterOnly = false;
            this.txtKota.Location = new System.Drawing.Point(104, 53);
            this.txtKota.Name = "txtKota";
            this.txtKota.NumericOnly = false;
            this.txtKota.SelectionText = false;
            this.txtKota.Size = new System.Drawing.Size(332, 20);
            this.txtKota.TabIndex = 2;
            this.txtKota.Tag = "kota";
            this.txtKota.ThousandSeparator = false;
            // 
            // txtTelepon
            // 
            this.txtTelepon.AutoEnter = false;
            this.txtTelepon.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtTelepon.EnterFocusColor = System.Drawing.Color.White;
            this.txtTelepon.LeaveFocusColor = System.Drawing.Color.White;
            this.txtTelepon.LetterOnly = false;
            this.txtTelepon.Location = new System.Drawing.Point(104, 78);
            this.txtTelepon.Name = "txtTelepon";
            this.txtTelepon.NumericOnly = false;
            this.txtTelepon.SelectionText = false;
            this.txtTelepon.Size = new System.Drawing.Size(123, 20);
            this.txtTelepon.TabIndex = 3;
            this.txtTelepon.Tag = "telepon";
            this.txtTelepon.ThousandSeparator = false;
            this.txtTelepon.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTelepon_KeyPress);
            // 
            // FrmProfilPerusahaan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 185);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmProfilPerusahaan";
            this.Text = "FrmProfilPerusahaan";
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
        private System.Windows.Forms.Label label4;
        private UserControl.AdvancedTextbox txtNamaPerusahaan;
        private UserControl.AdvancedTextbox txtAlamat;
        private UserControl.AdvancedTextbox txtKota;
        private UserControl.AdvancedTextbox txtTelepon;

    }
}