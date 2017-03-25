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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbStatusStok = new System.Windows.Forms.ComboBox();
            this.txtNamaProduk = new OpenRetail.App.UserControl.AdvancedTextbox();
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
            this.tableLayoutPanel3.Controls.Add(this.cmbStatusStok, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtNamaProduk, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(392, 52);
            this.tableLayoutPanel3.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Status Stok";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Nama Produk";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbStatusStok
            // 
            this.cmbStatusStok.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatusStok.FormattingEnabled = true;
            this.cmbStatusStok.Items.AddRange(new object[] {
            "Semua",
            "Ada",
            "Kosong"});
            this.cmbStatusStok.Location = new System.Drawing.Point(81, 3);
            this.cmbStatusStok.Name = "cmbStatusStok";
            this.cmbStatusStok.Size = new System.Drawing.Size(96, 21);
            this.cmbStatusStok.TabIndex = 2;
            // 
            // txtNamaProduk
            // 
            this.txtNamaProduk.AutoEnter = false;
            this.txtNamaProduk.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtNamaProduk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNamaProduk.EnterFocusColor = System.Drawing.Color.White;
            this.txtNamaProduk.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNamaProduk.LetterOnly = false;
            this.txtNamaProduk.Location = new System.Drawing.Point(81, 28);
            this.txtNamaProduk.Name = "txtNamaProduk";
            this.txtNamaProduk.NumericOnly = false;
            this.txtNamaProduk.SelectionText = false;
            this.txtNamaProduk.Size = new System.Drawing.Size(308, 20);
            this.txtNamaProduk.TabIndex = 3;
            this.txtNamaProduk.ThousandSeparator = false;
            // 
            // FrmLapStokProduk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 134);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbStatusStok;
        private UserControl.AdvancedTextbox txtNamaProduk;
    }
}