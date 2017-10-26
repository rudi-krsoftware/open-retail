namespace OpenRetail.App.Cashier.Transaksi
{
    partial class FrmInfoNotaTerakhir
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
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.gridList = new Syncfusion.Windows.Forms.Grid.GridListControl();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPelanggan = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtNotaTanggal = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.txtDiskon = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtPPN = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtGrandTotal = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.gridList, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel6, 0, 2);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 82F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(685, 416);
            this.tableLayoutPanel4.TabIndex = 6;
            // 
            // gridList
            // 
            this.gridList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridList.ItemHeight = 17;
            this.gridList.Location = new System.Drawing.Point(3, 61);
            this.gridList.Name = "gridList";
            this.gridList.Properties.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridList.SelectedIndex = -1;
            this.gridList.Size = new System.Drawing.Size(679, 270);
            this.gridList.TabIndex = 0;
            this.gridList.TopIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.txtPelanggan, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.txtNotaTanggal, 1, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 3;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(679, 52);
            this.tableLayoutPanel5.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pelanggan";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Nota / Tanggal";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtPelanggan
            // 
            this.txtPelanggan.AutoEnter = false;
            this.txtPelanggan.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtPelanggan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPelanggan.Enabled = false;
            this.txtPelanggan.EnterFocusColor = System.Drawing.Color.White;
            this.txtPelanggan.LeaveFocusColor = System.Drawing.Color.White;
            this.txtPelanggan.LetterOnly = false;
            this.txtPelanggan.Location = new System.Drawing.Point(89, 3);
            this.txtPelanggan.Name = "txtPelanggan";
            this.txtPelanggan.NumericOnly = false;
            this.txtPelanggan.SelectionText = false;
            this.txtPelanggan.Size = new System.Drawing.Size(587, 20);
            this.txtPelanggan.TabIndex = 1;
            this.txtPelanggan.ThousandSeparator = false;
            // 
            // txtNotaTanggal
            // 
            this.txtNotaTanggal.AutoEnter = false;
            this.txtNotaTanggal.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtNotaTanggal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotaTanggal.Enabled = false;
            this.txtNotaTanggal.EnterFocusColor = System.Drawing.Color.White;
            this.txtNotaTanggal.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNotaTanggal.LetterOnly = false;
            this.txtNotaTanggal.Location = new System.Drawing.Point(89, 28);
            this.txtNotaTanggal.Name = "txtNotaTanggal";
            this.txtNotaTanggal.NumericOnly = false;
            this.txtNotaTanggal.SelectionText = false;
            this.txtNotaTanggal.Size = new System.Drawing.Size(587, 20);
            this.txtNotaTanggal.TabIndex = 1;
            this.txtNotaTanggal.ThousandSeparator = false;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.38881F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.61119F));
            this.tableLayoutPanel6.Controls.Add(this.txtDiskon, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.txtPPN, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.txtGrandTotal, 1, 2);
            this.tableLayoutPanel6.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 337);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 4;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(679, 76);
            this.tableLayoutPanel6.TabIndex = 2;
            // 
            // txtDiskon
            // 
            this.txtDiskon.AutoEnter = false;
            this.txtDiskon.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtDiskon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDiskon.Enabled = false;
            this.txtDiskon.EnterFocusColor = System.Drawing.Color.White;
            this.txtDiskon.LeaveFocusColor = System.Drawing.Color.White;
            this.txtDiskon.LetterOnly = false;
            this.txtDiskon.Location = new System.Drawing.Point(576, 3);
            this.txtDiskon.MaxLength = 20;
            this.txtDiskon.Name = "txtDiskon";
            this.txtDiskon.NumericOnly = true;
            this.txtDiskon.SelectionText = false;
            this.txtDiskon.Size = new System.Drawing.Size(100, 20);
            this.txtDiskon.TabIndex = 0;
            this.txtDiskon.Text = "0";
            this.txtDiskon.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtDiskon.ThousandSeparator = true;
            // 
            // txtPPN
            // 
            this.txtPPN.AutoEnter = false;
            this.txtPPN.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtPPN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPPN.Enabled = false;
            this.txtPPN.EnterFocusColor = System.Drawing.Color.White;
            this.txtPPN.LeaveFocusColor = System.Drawing.Color.White;
            this.txtPPN.LetterOnly = false;
            this.txtPPN.Location = new System.Drawing.Point(576, 28);
            this.txtPPN.MaxLength = 20;
            this.txtPPN.Name = "txtPPN";
            this.txtPPN.NumericOnly = true;
            this.txtPPN.SelectionText = false;
            this.txtPPN.Size = new System.Drawing.Size(100, 20);
            this.txtPPN.TabIndex = 0;
            this.txtPPN.Text = "0";
            this.txtPPN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPPN.ThousandSeparator = true;
            // 
            // txtGrandTotal
            // 
            this.txtGrandTotal.AutoEnter = false;
            this.txtGrandTotal.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtGrandTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtGrandTotal.Enabled = false;
            this.txtGrandTotal.EnterFocusColor = System.Drawing.Color.White;
            this.txtGrandTotal.LeaveFocusColor = System.Drawing.Color.White;
            this.txtGrandTotal.LetterOnly = false;
            this.txtGrandTotal.Location = new System.Drawing.Point(576, 53);
            this.txtGrandTotal.MaxLength = 20;
            this.txtGrandTotal.Name = "txtGrandTotal";
            this.txtGrandTotal.NumericOnly = true;
            this.txtGrandTotal.SelectionText = false;
            this.txtGrandTotal.Size = new System.Drawing.Size(100, 20);
            this.txtGrandTotal.TabIndex = 0;
            this.txtGrandTotal.Text = "0";
            this.txtGrandTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtGrandTotal.ThousandSeparator = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(567, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "Diskon";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(567, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "PPN";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(567, 25);
            this.label5.TabIndex = 1;
            this.label5.Text = "Grand Total";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmInfoNotaTerakhir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 498);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Name = "FrmInfoNotaTerakhir";
            this.Text = "FrmInfoNotaTerakhir";
            this.Controls.SetChildIndex(this.tableLayoutPanel4, 0);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Syncfusion.Windows.Forms.Grid.GridListControl gridList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Helper.UserControl.AdvancedTextbox txtPelanggan;
        private Helper.UserControl.AdvancedTextbox txtNotaTanggal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private Helper.UserControl.AdvancedTextbox txtDiskon;
        private Helper.UserControl.AdvancedTextbox txtPPN;
        private Helper.UserControl.AdvancedTextbox txtGrandTotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;

    }
}