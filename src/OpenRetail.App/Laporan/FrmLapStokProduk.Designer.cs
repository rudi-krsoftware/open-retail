namespace OpenRetail.App.Laporan
{
    partial class FrmLapStokProduk
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
            this.cmbStatusStok = new System.Windows.Forms.ComboBox();
            this.rdoStatusStok = new System.Windows.Forms.RadioButton();
            this.rdoStokBerdasarkanProduk = new System.Windows.Forms.RadioButton();
            this.rdoStokKurangDari = new System.Windows.Forms.RadioButton();
            this.rdoStokBerdasarkanSupplier = new System.Windows.Forms.RadioButton();
            this.rdoStokBerdasarkanGolongan = new System.Windows.Forms.RadioButton();
            this.txtStok = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.cmbGolongan = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.chkListOfProduk = new System.Windows.Forms.CheckedListBox();
            this.txtNamaProduk = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 205F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.cmbStatusStok, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.rdoStatusStok, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rdoStokBerdasarkanProduk, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.rdoStokKurangDari, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.rdoStokBerdasarkanSupplier, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.rdoStokBerdasarkanGolongan, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtStok, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbSupplier, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.cmbGolongan, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.panel2, 1, 4);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 204F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(546, 308);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // cmbStatusStok
            // 
            this.cmbStatusStok.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusStok.FormattingEnabled = true;
            this.cmbStatusStok.Items.AddRange(new object[] {
            "Semua",
            "Ada",
            "Kosong"});
            this.cmbStatusStok.Location = new System.Drawing.Point(208, 3);
            this.cmbStatusStok.Name = "cmbStatusStok";
            this.cmbStatusStok.Size = new System.Drawing.Size(96, 21);
            this.cmbStatusStok.TabIndex = 1;
            // 
            // rdoStatusStok
            // 
            this.rdoStatusStok.AutoSize = true;
            this.rdoStatusStok.Checked = true;
            this.rdoStatusStok.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoStatusStok.Location = new System.Drawing.Point(3, 3);
            this.rdoStatusStok.Name = "rdoStatusStok";
            this.rdoStatusStok.Size = new System.Drawing.Size(78, 19);
            this.rdoStatusStok.TabIndex = 0;
            this.rdoStatusStok.TabStop = true;
            this.rdoStatusStok.Text = "Status stok";
            this.rdoStatusStok.UseVisualStyleBackColor = true;
            this.rdoStatusStok.CheckedChanged += new System.EventHandler(this.rdoStatusStok_CheckedChanged);
            // 
            // rdoStokBerdasarkanProduk
            // 
            this.rdoStokBerdasarkanProduk.AutoSize = true;
            this.rdoStokBerdasarkanProduk.Location = new System.Drawing.Point(3, 103);
            this.rdoStokBerdasarkanProduk.Name = "rdoStokBerdasarkanProduk";
            this.rdoStokBerdasarkanProduk.Size = new System.Drawing.Size(199, 17);
            this.rdoStokBerdasarkanProduk.TabIndex = 8;
            this.rdoStokBerdasarkanProduk.Text = "Stok berdasarkan kode/nama produk";
            this.rdoStokBerdasarkanProduk.UseVisualStyleBackColor = true;
            this.rdoStokBerdasarkanProduk.CheckedChanged += new System.EventHandler(this.rdoStokBerdasarkanProduk_CheckedChanged);
            // 
            // rdoStokKurangDari
            // 
            this.rdoStokKurangDari.AutoSize = true;
            this.rdoStokKurangDari.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoStokKurangDari.Location = new System.Drawing.Point(3, 28);
            this.rdoStokKurangDari.Name = "rdoStokKurangDari";
            this.rdoStokKurangDari.Size = new System.Drawing.Size(103, 19);
            this.rdoStokKurangDari.TabIndex = 2;
            this.rdoStokKurangDari.Text = "Stok kurang dari";
            this.rdoStokKurangDari.UseVisualStyleBackColor = true;
            // 
            // rdoStokBerdasarkanSupplier
            // 
            this.rdoStokBerdasarkanSupplier.AutoSize = true;
            this.rdoStokBerdasarkanSupplier.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoStokBerdasarkanSupplier.Location = new System.Drawing.Point(3, 53);
            this.rdoStokBerdasarkanSupplier.Name = "rdoStokBerdasarkanSupplier";
            this.rdoStokBerdasarkanSupplier.Size = new System.Drawing.Size(148, 19);
            this.rdoStokBerdasarkanSupplier.TabIndex = 4;
            this.rdoStokBerdasarkanSupplier.Text = "Stok berdasarkan supplier";
            this.rdoStokBerdasarkanSupplier.UseVisualStyleBackColor = true;
            // 
            // rdoStokBerdasarkanGolongan
            // 
            this.rdoStokBerdasarkanGolongan.AutoSize = true;
            this.rdoStokBerdasarkanGolongan.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoStokBerdasarkanGolongan.Location = new System.Drawing.Point(3, 78);
            this.rdoStokBerdasarkanGolongan.Name = "rdoStokBerdasarkanGolongan";
            this.rdoStokBerdasarkanGolongan.Size = new System.Drawing.Size(156, 19);
            this.rdoStokBerdasarkanGolongan.TabIndex = 6;
            this.rdoStokBerdasarkanGolongan.Text = "Stok berdasarkan golongan";
            this.rdoStokBerdasarkanGolongan.UseVisualStyleBackColor = true;
            // 
            // txtStok
            // 
            this.txtStok.AutoEnter = true;
            this.txtStok.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtStok.EnterFocusColor = System.Drawing.Color.White;
            this.txtStok.LeaveFocusColor = System.Drawing.Color.White;
            this.txtStok.LetterOnly = false;
            this.txtStok.Location = new System.Drawing.Point(208, 28);
            this.txtStok.Name = "txtStok";
            this.txtStok.NumericOnly = true;
            this.txtStok.SelectionText = false;
            this.txtStok.Size = new System.Drawing.Size(48, 20);
            this.txtStok.TabIndex = 3;
            this.txtStok.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtStok.ThousandSeparator = false;
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSupplier.FormattingEnabled = true;
            this.cmbSupplier.Location = new System.Drawing.Point(208, 53);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(335, 21);
            this.cmbSupplier.TabIndex = 5;
            // 
            // cmbGolongan
            // 
            this.cmbGolongan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbGolongan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGolongan.FormattingEnabled = true;
            this.cmbGolongan.Location = new System.Drawing.Point(208, 78);
            this.cmbGolongan.Name = "cmbGolongan";
            this.cmbGolongan.Size = new System.Drawing.Size(335, 21);
            this.cmbGolongan.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.chkListOfProduk);
            this.panel2.Controls.Add(this.txtNamaProduk);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(205, 100);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(341, 204);
            this.panel2.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(303, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Enter";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkListOfProduk
            // 
            this.chkListOfProduk.CheckOnClick = true;
            this.chkListOfProduk.FormattingEnabled = true;
            this.chkListOfProduk.Location = new System.Drawing.Point(3, 31);
            this.chkListOfProduk.Name = "chkListOfProduk";
            this.chkListOfProduk.Size = new System.Drawing.Size(332, 169);
            this.chkListOfProduk.TabIndex = 10;
            // 
            // txtNamaProduk
            // 
            this.txtNamaProduk.AutoEnter = false;
            this.txtNamaProduk.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtNamaProduk.EnterFocusColor = System.Drawing.Color.White;
            this.txtNamaProduk.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNamaProduk.LetterOnly = false;
            this.txtNamaProduk.Location = new System.Drawing.Point(3, 5);
            this.txtNamaProduk.Name = "txtNamaProduk";
            this.txtNamaProduk.NumericOnly = false;
            this.txtNamaProduk.SelectionText = false;
            this.txtNamaProduk.Size = new System.Drawing.Size(294, 20);
            this.txtNamaProduk.TabIndex = 9;
            this.txtNamaProduk.ThousandSeparator = false;
            this.txtNamaProduk.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNamaProduk_KeyPress);
            // 
            // FrmLapStokProduk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 390);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmLapStokProduk";
            this.Text = "FrmLapStokProduk";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.ComboBox cmbStatusStok;
        private OpenRetail.Helper.UserControl.AdvancedTextbox txtNamaProduk;
        private System.Windows.Forms.RadioButton rdoStatusStok;
        private System.Windows.Forms.RadioButton rdoStokKurangDari;
        private System.Windows.Forms.RadioButton rdoStokBerdasarkanSupplier;
        private System.Windows.Forms.RadioButton rdoStokBerdasarkanGolongan;
        private System.Windows.Forms.RadioButton rdoStokBerdasarkanProduk;
        private Helper.UserControl.AdvancedTextbox txtStok;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.ComboBox cmbGolongan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckedListBox chkListOfProduk;
    }
}