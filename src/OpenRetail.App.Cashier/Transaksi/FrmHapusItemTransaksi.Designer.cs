namespace OpenRetail.App.Cashier.Transaksi
{
    partial class FrmHapusItemTransaksi
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
            this.txtNomorTransaksi = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtNomorTransaksi, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(302, 37);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 35);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nomor Transaksi";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNomorTransaksi
            // 
            this.txtNomorTransaksi.AutoEnter = false;
            this.txtNomorTransaksi.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtNomorTransaksi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNomorTransaksi.EnterFocusColor = System.Drawing.Color.White;
            this.txtNomorTransaksi.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNomorTransaksi.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNomorTransaksi.LetterOnly = false;
            this.txtNomorTransaksi.Location = new System.Drawing.Point(170, 3);
            this.txtNomorTransaksi.MaxLength = 3;
            this.txtNomorTransaksi.Name = "txtNomorTransaksi";
            this.txtNomorTransaksi.NumericOnly = true;
            this.txtNomorTransaksi.SelectionText = false;
            this.txtNomorTransaksi.Size = new System.Drawing.Size(129, 30);
            this.txtNomorTransaksi.TabIndex = 0;
            this.txtNomorTransaksi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNomorTransaksi.ThousandSeparator = false;
            this.txtNomorTransaksi.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNomorTransaksi_KeyPress);
            // 
            // FrmHapusItemTransaksi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 119);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmHapusItemTransaksi";
            this.Text = "FrmHapusItemTransaksi";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private Helper.UserControl.AdvancedTextbox txtNomorTransaksi;
    }
}