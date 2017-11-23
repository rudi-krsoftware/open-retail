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
            this.rdoStokKurangDari = new System.Windows.Forms.RadioButton();
            this.rdoStokBerdasarkanSupplier = new System.Windows.Forms.RadioButton();
            this.rdoStokBerdasarkanGolongan = new System.Windows.Forms.RadioButton();
            this.rdoStokBerdasarkanNama = new System.Windows.Forms.RadioButton();
            this.txtNamaProduk = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtStok = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.cmbGolongan = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.cmbStatusStok, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.rdoStatusStok, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.rdoStokKurangDari, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.rdoStokBerdasarkanSupplier, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.rdoStokBerdasarkanGolongan, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.rdoStokBerdasarkanNama, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.txtNamaProduk, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.txtStok, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.cmbSupplier, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.cmbGolongan, 1, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(437, 127);
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
            this.cmbStatusStok.Location = new System.Drawing.Point(183, 3);
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
            // rdoStokBerdasarkanNama
            // 
            this.rdoStokBerdasarkanNama.AutoSize = true;
            this.rdoStokBerdasarkanNama.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdoStokBerdasarkanNama.Location = new System.Drawing.Point(3, 103);
            this.rdoStokBerdasarkanNama.Name = "rdoStokBerdasarkanNama";
            this.rdoStokBerdasarkanNama.Size = new System.Drawing.Size(174, 19);
            this.rdoStokBerdasarkanNama.TabIndex = 8;
            this.rdoStokBerdasarkanNama.Text = "Stok berdasarkan nama produk";
            this.rdoStokBerdasarkanNama.UseVisualStyleBackColor = true;
            // 
            // txtNamaProduk
            // 
            this.txtNamaProduk.AutoEnter = false;
            this.txtNamaProduk.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtNamaProduk.EnterFocusColor = System.Drawing.Color.White;
            this.txtNamaProduk.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNamaProduk.LetterOnly = false;
            this.txtNamaProduk.Location = new System.Drawing.Point(183, 103);
            this.txtNamaProduk.Name = "txtNamaProduk";
            this.txtNamaProduk.NumericOnly = false;
            this.txtNamaProduk.SelectionText = false;
            this.txtNamaProduk.Size = new System.Drawing.Size(251, 20);
            this.txtNamaProduk.TabIndex = 9;
            this.txtNamaProduk.ThousandSeparator = false;
            // 
            // txtStok
            // 
            this.txtStok.AutoEnter = true;
            this.txtStok.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtStok.EnterFocusColor = System.Drawing.Color.White;
            this.txtStok.LeaveFocusColor = System.Drawing.Color.White;
            this.txtStok.LetterOnly = false;
            this.txtStok.Location = new System.Drawing.Point(183, 28);
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
            this.cmbSupplier.Location = new System.Drawing.Point(183, 53);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(251, 21);
            this.cmbSupplier.TabIndex = 5;
            // 
            // cmbGolongan
            // 
            this.cmbGolongan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbGolongan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGolongan.FormattingEnabled = true;
            this.cmbGolongan.Location = new System.Drawing.Point(183, 78);
            this.cmbGolongan.Name = "cmbGolongan";
            this.cmbGolongan.Size = new System.Drawing.Size(251, 21);
            this.cmbGolongan.TabIndex = 7;
            // 
            // FrmLapStokProduk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 209);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmLapStokProduk";
            this.Text = "FrmLapStokProduk";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
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
        private System.Windows.Forms.RadioButton rdoStokBerdasarkanNama;
        private Helper.UserControl.AdvancedTextbox txtStok;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.ComboBox cmbGolongan;
    }
}