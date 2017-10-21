namespace OpenRetail.App.Referensi
{
    partial class FrmEntryKartu
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
            this.txtNamaKartu = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.label2 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.rdoKartuDebit = new System.Windows.Forms.RadioButton();
            this.rdoKartuKredit = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtNamaKartu, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(347, 52);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nama Kartu";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNamaKartu
            // 
            this.txtNamaKartu.AutoEnter = true;
            this.txtNamaKartu.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtNamaKartu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNamaKartu.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtNamaKartu.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNamaKartu.LetterOnly = false;
            this.txtNamaKartu.Location = new System.Drawing.Point(72, 3);
            this.txtNamaKartu.Name = "txtNamaKartu";
            this.txtNamaKartu.NumericOnly = false;
            this.txtNamaKartu.SelectionText = false;
            this.txtNamaKartu.Size = new System.Drawing.Size(272, 20);
            this.txtNamaKartu.TabIndex = 0;
            this.txtNamaKartu.Tag = "nama_golongan";
            this.txtNamaKartu.ThousandSeparator = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Jenis";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.rdoKartuDebit);
            this.flowLayoutPanel1.Controls.Add(this.rdoKartuKredit);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(69, 25);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(278, 25);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // rdoKartuDebit
            // 
            this.rdoKartuDebit.AutoSize = true;
            this.rdoKartuDebit.Checked = true;
            this.rdoKartuDebit.Location = new System.Drawing.Point(3, 3);
            this.rdoKartuDebit.Name = "rdoKartuDebit";
            this.rdoKartuDebit.Size = new System.Drawing.Size(78, 17);
            this.rdoKartuDebit.TabIndex = 0;
            this.rdoKartuDebit.TabStop = true;
            this.rdoKartuDebit.Text = "Kartu Debit";
            this.rdoKartuDebit.UseVisualStyleBackColor = true;
            // 
            // rdoKartuKredit
            // 
            this.rdoKartuKredit.AutoSize = true;
            this.rdoKartuKredit.Location = new System.Drawing.Point(87, 3);
            this.rdoKartuKredit.Name = "rdoKartuKredit";
            this.rdoKartuKredit.Size = new System.Drawing.Size(80, 17);
            this.rdoKartuKredit.TabIndex = 1;
            this.rdoKartuKredit.Text = "Kartu Kredit";
            this.rdoKartuKredit.UseVisualStyleBackColor = true;
            // 
            // FrmEntryKartu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 134);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmEntryKartu";
            this.Text = "FrmEntryKartu";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label1;
        private OpenRetail.Helper.UserControl.AdvancedTextbox txtNamaKartu;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton rdoKartuDebit;
        private System.Windows.Forms.RadioButton rdoKartuKredit;
    }
}