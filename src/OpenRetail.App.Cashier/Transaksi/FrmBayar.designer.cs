namespace OpenRetail.App.Cashier.Transaksi
{
    partial class FrmBayar
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
            this.txtTotal = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkBayarViaKartu = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDiskon = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtPPN = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtGrandTotal = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtBayarTunai = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.cmbKartu = new System.Windows.Forms.ComboBox();
            this.txtBayarKartu = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtNoKartu = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtKembalian = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtTotal, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.chkBayarViaKartu, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.txtDiskon, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtPPN, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.txtGrandTotal, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.txtBayarTunai, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.cmbKartu, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.txtBayarKartu, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.txtNoKartu, 1, 8);
            this.tableLayoutPanel3.Controls.Add(this.label8, 0, 10);
            this.tableLayoutPanel3.Controls.Add(this.txtKembalian, 1, 10);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 12;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(433, 340);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtTotal
            // 
            this.txtTotal.AutoEnter = true;
            this.txtTotal.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTotal.Enabled = false;
            this.txtTotal.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.LeaveFocusColor = System.Drawing.Color.White;
            this.txtTotal.LetterOnly = false;
            this.txtTotal.Location = new System.Drawing.Point(173, 3);
            this.txtTotal.MaxLength = 20;
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.NumericOnly = true;
            this.txtTotal.SelectionText = false;
            this.txtTotal.Size = new System.Drawing.Size(257, 30);
            this.txtTotal.TabIndex = 0;
            this.txtTotal.Tag = "";
            this.txtTotal.Text = "0";
            this.txtTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotal.ThousandSeparator = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 35);
            this.label2.TabIndex = 1;
            this.label2.Text = "Diskon";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 35);
            this.label3.TabIndex = 2;
            this.label3.Text = "PPN";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 25);
            this.label4.TabIndex = 2;
            this.label4.Text = "Grand Total";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(164, 35);
            this.label5.TabIndex = 2;
            this.label5.Text = "Bayar Tunai";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkBayarViaKartu
            // 
            this.chkBayarViaKartu.AutoSize = true;
            this.chkBayarViaKartu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkBayarViaKartu.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBayarViaKartu.Location = new System.Drawing.Point(3, 188);
            this.chkBayarViaKartu.Name = "chkBayarViaKartu";
            this.chkBayarViaKartu.Size = new System.Drawing.Size(164, 31);
            this.chkBayarViaKartu.TabIndex = 5;
            this.chkBayarViaKartu.Text = "Bayar via Kartu";
            this.chkBayarViaKartu.UseVisualStyleBackColor = true;
            this.chkBayarViaKartu.CheckedChanged += new System.EventHandler(this.chkBayarViaKartu_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 222);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(164, 35);
            this.label6.TabIndex = 2;
            this.label6.Text = "Jumlah";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(3, 257);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(164, 35);
            this.label7.TabIndex = 2;
            this.label7.Text = "No. Kartu";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDiskon
            // 
            this.txtDiskon.AutoEnter = true;
            this.txtDiskon.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtDiskon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDiskon.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtDiskon.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDiskon.LeaveFocusColor = System.Drawing.Color.White;
            this.txtDiskon.LetterOnly = false;
            this.txtDiskon.Location = new System.Drawing.Point(173, 38);
            this.txtDiskon.MaxLength = 20;
            this.txtDiskon.Name = "txtDiskon";
            this.txtDiskon.NumericOnly = true;
            this.txtDiskon.SelectionText = true;
            this.txtDiskon.Size = new System.Drawing.Size(257, 30);
            this.txtDiskon.TabIndex = 1;
            this.txtDiskon.Tag = "";
            this.txtDiskon.Text = "0";
            this.txtDiskon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDiskon.ThousandSeparator = true;
            // 
            // txtPPN
            // 
            this.txtPPN.AutoEnter = true;
            this.txtPPN.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtPPN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPPN.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtPPN.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPPN.LeaveFocusColor = System.Drawing.Color.White;
            this.txtPPN.LetterOnly = false;
            this.txtPPN.Location = new System.Drawing.Point(173, 73);
            this.txtPPN.MaxLength = 20;
            this.txtPPN.Name = "txtPPN";
            this.txtPPN.NumericOnly = true;
            this.txtPPN.SelectionText = true;
            this.txtPPN.Size = new System.Drawing.Size(257, 30);
            this.txtPPN.TabIndex = 2;
            this.txtPPN.Tag = "";
            this.txtPPN.Text = "0";
            this.txtPPN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPPN.ThousandSeparator = true;
            // 
            // txtGrandTotal
            // 
            this.txtGrandTotal.AutoEnter = true;
            this.txtGrandTotal.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtGrandTotal.Enabled = false;
            this.txtGrandTotal.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtGrandTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGrandTotal.LeaveFocusColor = System.Drawing.Color.White;
            this.txtGrandTotal.LetterOnly = false;
            this.txtGrandTotal.Location = new System.Drawing.Point(173, 108);
            this.txtGrandTotal.MaxLength = 20;
            this.txtGrandTotal.Name = "txtGrandTotal";
            this.txtGrandTotal.NumericOnly = true;
            this.txtGrandTotal.SelectionText = false;
            this.txtGrandTotal.Size = new System.Drawing.Size(257, 30);
            this.txtGrandTotal.TabIndex = 3;
            this.txtGrandTotal.Tag = "";
            this.txtGrandTotal.Text = "0";
            this.txtGrandTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGrandTotal.ThousandSeparator = true;
            // 
            // txtBayarTunai
            // 
            this.txtBayarTunai.AutoEnter = false;
            this.txtBayarTunai.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtBayarTunai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBayarTunai.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtBayarTunai.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBayarTunai.LeaveFocusColor = System.Drawing.Color.White;
            this.txtBayarTunai.LetterOnly = false;
            this.txtBayarTunai.Location = new System.Drawing.Point(173, 153);
            this.txtBayarTunai.MaxLength = 20;
            this.txtBayarTunai.Name = "txtBayarTunai";
            this.txtBayarTunai.NumericOnly = true;
            this.txtBayarTunai.SelectionText = true;
            this.txtBayarTunai.Size = new System.Drawing.Size(257, 30);
            this.txtBayarTunai.TabIndex = 4;
            this.txtBayarTunai.Tag = "";
            this.txtBayarTunai.Text = "0";
            this.txtBayarTunai.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBayarTunai.ThousandSeparator = true;
            this.txtBayarTunai.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBayarTunai_KeyPress);
            // 
            // cmbKartu
            // 
            this.cmbKartu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbKartu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKartu.Enabled = false;
            this.cmbKartu.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbKartu.FormattingEnabled = true;
            this.cmbKartu.Location = new System.Drawing.Point(173, 188);
            this.cmbKartu.Name = "cmbKartu";
            this.cmbKartu.Size = new System.Drawing.Size(257, 33);
            this.cmbKartu.TabIndex = 6;
            // 
            // txtBayarKartu
            // 
            this.txtBayarKartu.AutoEnter = true;
            this.txtBayarKartu.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtBayarKartu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBayarKartu.Enabled = false;
            this.txtBayarKartu.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtBayarKartu.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBayarKartu.LeaveFocusColor = System.Drawing.Color.White;
            this.txtBayarKartu.LetterOnly = false;
            this.txtBayarKartu.Location = new System.Drawing.Point(173, 225);
            this.txtBayarKartu.MaxLength = 20;
            this.txtBayarKartu.Name = "txtBayarKartu";
            this.txtBayarKartu.NumericOnly = true;
            this.txtBayarKartu.SelectionText = false;
            this.txtBayarKartu.Size = new System.Drawing.Size(257, 30);
            this.txtBayarKartu.TabIndex = 7;
            this.txtBayarKartu.Tag = "";
            this.txtBayarKartu.Text = "0";
            this.txtBayarKartu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBayarKartu.ThousandSeparator = true;
            // 
            // txtNoKartu
            // 
            this.txtNoKartu.AutoEnter = true;
            this.txtNoKartu.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtNoKartu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNoKartu.Enabled = false;
            this.txtNoKartu.EnterFocusColor = System.Drawing.Color.White;
            this.txtNoKartu.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoKartu.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNoKartu.LetterOnly = false;
            this.txtNoKartu.Location = new System.Drawing.Point(173, 260);
            this.txtNoKartu.MaxLength = 20;
            this.txtNoKartu.Name = "txtNoKartu";
            this.txtNoKartu.NumericOnly = false;
            this.txtNoKartu.SelectionText = false;
            this.txtNoKartu.Size = new System.Drawing.Size(257, 30);
            this.txtNoKartu.TabIndex = 8;
            this.txtNoKartu.ThousandSeparator = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(3, 302);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(164, 35);
            this.label8.TabIndex = 2;
            this.label8.Text = "Kembalian";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtKembalian
            // 
            this.txtKembalian.AutoEnter = true;
            this.txtKembalian.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtKembalian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKembalian.Enabled = false;
            this.txtKembalian.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtKembalian.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKembalian.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKembalian.LetterOnly = false;
            this.txtKembalian.Location = new System.Drawing.Point(173, 305);
            this.txtKembalian.MaxLength = 20;
            this.txtKembalian.Name = "txtKembalian";
            this.txtKembalian.NumericOnly = true;
            this.txtKembalian.SelectionText = false;
            this.txtKembalian.Size = new System.Drawing.Size(257, 30);
            this.txtKembalian.TabIndex = 3;
            this.txtKembalian.Tag = "";
            this.txtKembalian.Text = "0";
            this.txtKembalian.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtKembalian.ThousandSeparator = true;
            // 
            // FrmBayar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 422);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmBayar";
            this.Text = "FrmBayar";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private OpenRetail.Helper.UserControl.AdvancedTextbox txtTotal;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkBayarViaKartu;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private Helper.UserControl.AdvancedTextbox txtDiskon;
        private Helper.UserControl.AdvancedTextbox txtPPN;
        private Helper.UserControl.AdvancedTextbox txtGrandTotal;
        private Helper.UserControl.AdvancedTextbox txtBayarTunai;
        private System.Windows.Forms.ComboBox cmbKartu;
        private Helper.UserControl.AdvancedTextbox txtBayarKartu;
        private Helper.UserControl.AdvancedTextbox txtNoKartu;
        private System.Windows.Forms.Label label8;
        private Helper.UserControl.AdvancedTextbox txtKembalian;
    }
}