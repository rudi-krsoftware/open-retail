namespace OpenRetail.App.Referensi
{
    partial class FrmEntryJenisPengeluaran
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
            this.txtJenisPengeluaran = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtJenisPengeluaran, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(383, 26);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Jenis Pengeluaran";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtJenisPengeluaran
            // 
            this.txtJenisPengeluaran.AutoEnter = false;
            this.txtJenisPengeluaran.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtJenisPengeluaran.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJenisPengeluaran.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtJenisPengeluaran.LeaveFocusColor = System.Drawing.Color.White;
            this.txtJenisPengeluaran.LetterOnly = false;
            this.txtJenisPengeluaran.Location = new System.Drawing.Point(103, 3);
            this.txtJenisPengeluaran.Name = "txtJenisPengeluaran";
            this.txtJenisPengeluaran.NumericOnly = false;
            this.txtJenisPengeluaran.SelectionText = false;
            this.txtJenisPengeluaran.Size = new System.Drawing.Size(277, 20);
            this.txtJenisPengeluaran.TabIndex = 0;
            this.txtJenisPengeluaran.Tag = "nama_jenis_pengeluaran";
            this.txtJenisPengeluaran.ThousandSeparator = false;
            this.txtJenisPengeluaran.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtJenisPengeluaran_KeyPress);
            // 
            // FrmEntryJenisPengeluaran
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 108);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmEntryJenisPengeluaran";
            this.Text = "FrmEntryJenisPengeluaran";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private UserControl.AdvancedTextbox txtJenisPengeluaran;
    }
}