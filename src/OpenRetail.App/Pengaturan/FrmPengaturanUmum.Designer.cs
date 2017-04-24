namespace OpenRetail.App.Pengaturan
{
    partial class FrmPengaturanUmum
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbPrinter = new System.Windows.Forms.ComboBox();
            this.chkCetakOtomatis = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnLihatContohNotaPenjualan = new System.Windows.Forms.Button();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtHeader1 = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtHeader2 = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtHeader3 = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtHeader4 = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtHeader5 = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(412, 194);
            this.tableLayoutPanel3.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(406, 188);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel4);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(398, 162);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Printer";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.cmbPrinter, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.chkCetakOtomatis, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(392, 156);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nama Printer";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbPrinter
            // 
            this.cmbPrinter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrinter.FormattingEnabled = true;
            this.cmbPrinter.Location = new System.Drawing.Point(107, 3);
            this.cmbPrinter.Name = "cmbPrinter";
            this.cmbPrinter.Size = new System.Drawing.Size(282, 21);
            this.cmbPrinter.TabIndex = 1;
            // 
            // chkCetakOtomatis
            // 
            this.chkCetakOtomatis.AutoSize = true;
            this.chkCetakOtomatis.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkCetakOtomatis.Location = new System.Drawing.Point(3, 28);
            this.chkCetakOtomatis.Name = "chkCetakOtomatis";
            this.chkCetakOtomatis.Size = new System.Drawing.Size(98, 19);
            this.chkCetakOtomatis.TabIndex = 2;
            this.chkCetakOtomatis.Text = "Cetak Otomatis";
            this.chkCetakOtomatis.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(398, 162);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Nota Penjualan";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.btnLihatContohNotaPenjualan, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(398, 162);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // btnLihatContohNotaPenjualan
            // 
            this.btnLihatContohNotaPenjualan.Location = new System.Drawing.Point(3, 135);
            this.btnLihatContohNotaPenjualan.Name = "btnLihatContohNotaPenjualan";
            this.btnLihatContohNotaPenjualan.Size = new System.Drawing.Size(165, 23);
            this.btnLihatContohNotaPenjualan.TabIndex = 5;
            this.btnLihatContohNotaPenjualan.Text = "Lihat Contoh Nota Penjualan";
            this.btnLihatContohNotaPenjualan.UseVisualStyleBackColor = true;
            this.btnLihatContohNotaPenjualan.Click += new System.EventHandler(this.btnLihatContohNotaPenjualan_Click);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.label5, 0, 3);
            this.tableLayoutPanel6.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel6.Controls.Add(this.txtHeader1, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.txtHeader2, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.txtHeader3, 1, 2);
            this.tableLayoutPanel6.Controls.Add(this.txtHeader4, 1, 3);
            this.tableLayoutPanel6.Controls.Add(this.txtHeader5, 1, 4);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 5;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(392, 126);
            this.tableLayoutPanel6.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Header #1";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Location = new System.Drawing.Point(3, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Header #2";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(3, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Header #3";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Left;
            this.label5.Location = new System.Drawing.Point(3, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 25);
            this.label5.TabIndex = 0;
            this.label5.Text = "Header #4";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Left;
            this.label6.Location = new System.Drawing.Point(3, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 26);
            this.label6.TabIndex = 0;
            this.label6.Text = "Header #5";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHeader1
            // 
            this.txtHeader1.AutoEnter = true;
            this.txtHeader1.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtHeader1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHeader1.EnterFocusColor = System.Drawing.Color.White;
            this.txtHeader1.LeaveFocusColor = System.Drawing.Color.White;
            this.txtHeader1.LetterOnly = false;
            this.txtHeader1.Location = new System.Drawing.Point(67, 3);
            this.txtHeader1.MaxLength = 100;
            this.txtHeader1.Name = "txtHeader1";
            this.txtHeader1.NumericOnly = false;
            this.txtHeader1.SelectionText = false;
            this.txtHeader1.Size = new System.Drawing.Size(322, 20);
            this.txtHeader1.TabIndex = 0;
            this.txtHeader1.ThousandSeparator = false;
            // 
            // txtHeader2
            // 
            this.txtHeader2.AutoEnter = true;
            this.txtHeader2.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtHeader2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHeader2.EnterFocusColor = System.Drawing.Color.White;
            this.txtHeader2.LeaveFocusColor = System.Drawing.Color.White;
            this.txtHeader2.LetterOnly = false;
            this.txtHeader2.Location = new System.Drawing.Point(67, 28);
            this.txtHeader2.MaxLength = 100;
            this.txtHeader2.Name = "txtHeader2";
            this.txtHeader2.NumericOnly = false;
            this.txtHeader2.SelectionText = false;
            this.txtHeader2.Size = new System.Drawing.Size(322, 20);
            this.txtHeader2.TabIndex = 1;
            this.txtHeader2.ThousandSeparator = false;
            // 
            // txtHeader3
            // 
            this.txtHeader3.AutoEnter = true;
            this.txtHeader3.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtHeader3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHeader3.EnterFocusColor = System.Drawing.Color.White;
            this.txtHeader3.LeaveFocusColor = System.Drawing.Color.White;
            this.txtHeader3.LetterOnly = false;
            this.txtHeader3.Location = new System.Drawing.Point(67, 53);
            this.txtHeader3.MaxLength = 100;
            this.txtHeader3.Name = "txtHeader3";
            this.txtHeader3.NumericOnly = false;
            this.txtHeader3.SelectionText = false;
            this.txtHeader3.Size = new System.Drawing.Size(322, 20);
            this.txtHeader3.TabIndex = 2;
            this.txtHeader3.ThousandSeparator = false;
            // 
            // txtHeader4
            // 
            this.txtHeader4.AutoEnter = true;
            this.txtHeader4.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtHeader4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHeader4.EnterFocusColor = System.Drawing.Color.White;
            this.txtHeader4.LeaveFocusColor = System.Drawing.Color.White;
            this.txtHeader4.LetterOnly = false;
            this.txtHeader4.Location = new System.Drawing.Point(67, 78);
            this.txtHeader4.MaxLength = 100;
            this.txtHeader4.Name = "txtHeader4";
            this.txtHeader4.NumericOnly = false;
            this.txtHeader4.SelectionText = false;
            this.txtHeader4.Size = new System.Drawing.Size(322, 20);
            this.txtHeader4.TabIndex = 3;
            this.txtHeader4.ThousandSeparator = false;
            // 
            // txtHeader5
            // 
            this.txtHeader5.AutoEnter = true;
            this.txtHeader5.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtHeader5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHeader5.EnterFocusColor = System.Drawing.Color.White;
            this.txtHeader5.LeaveFocusColor = System.Drawing.Color.White;
            this.txtHeader5.LetterOnly = false;
            this.txtHeader5.Location = new System.Drawing.Point(67, 103);
            this.txtHeader5.MaxLength = 100;
            this.txtHeader5.Name = "txtHeader5";
            this.txtHeader5.NumericOnly = false;
            this.txtHeader5.SelectionText = false;
            this.txtHeader5.Size = new System.Drawing.Size(322, 20);
            this.txtHeader5.TabIndex = 4;
            this.txtHeader5.ThousandSeparator = false;
            // 
            // FrmPengaturanUmum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 276);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmPengaturanUmum";
            this.Text = "FrmPengaturanUmum";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbPrinter;
        private System.Windows.Forms.CheckBox chkCetakOtomatis;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btnLihatContohNotaPenjualan;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private UserControl.AdvancedTextbox txtHeader1;
        private UserControl.AdvancedTextbox txtHeader2;
        private UserControl.AdvancedTextbox txtHeader3;
        private UserControl.AdvancedTextbox txtHeader4;
        private UserControl.AdvancedTextbox txtHeader5;



    }
}