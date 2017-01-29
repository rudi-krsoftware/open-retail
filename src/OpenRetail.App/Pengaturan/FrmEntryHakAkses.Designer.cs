namespace OpenRetail.App.Pengaturan
{
    partial class FrmEntryHakAkses
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.rdoAktif = new System.Windows.Forms.RadioButton();
            this.rdoNonAktif = new System.Windows.Forms.RadioButton();
            this.txtNamaRole = new OpenRetail.App.UserControl.AdvancedTextbox();
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
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtNamaRole, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(317, 49);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Hak Akses";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Status";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.rdoAktif);
            this.flowLayoutPanel1.Controls.Add(this.rdoNonAktif);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(65, 25);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(252, 25);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // rdoAktif
            // 
            this.rdoAktif.AutoSize = true;
            this.rdoAktif.Checked = true;
            this.rdoAktif.Location = new System.Drawing.Point(3, 3);
            this.rdoAktif.Name = "rdoAktif";
            this.rdoAktif.Size = new System.Drawing.Size(46, 17);
            this.rdoAktif.TabIndex = 0;
            this.rdoAktif.TabStop = true;
            this.rdoAktif.Text = "Aktif";
            this.rdoAktif.UseVisualStyleBackColor = true;
            // 
            // rdoNonAktif
            // 
            this.rdoNonAktif.AutoSize = true;
            this.rdoNonAktif.Location = new System.Drawing.Point(55, 3);
            this.rdoNonAktif.Name = "rdoNonAktif";
            this.rdoNonAktif.Size = new System.Drawing.Size(69, 17);
            this.rdoNonAktif.TabIndex = 1;
            this.rdoNonAktif.Text = "Non Aktif";
            this.rdoNonAktif.UseVisualStyleBackColor = true;
            // 
            // txtNamaRole
            // 
            this.txtNamaRole.AutoEnter = true;
            this.txtNamaRole.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtNamaRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNamaRole.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtNamaRole.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNamaRole.LetterOnly = false;
            this.txtNamaRole.Location = new System.Drawing.Point(68, 3);
            this.txtNamaRole.Name = "txtNamaRole";
            this.txtNamaRole.NumericOnly = false;
            this.txtNamaRole.SelectionText = false;
            this.txtNamaRole.Size = new System.Drawing.Size(246, 20);
            this.txtNamaRole.TabIndex = 0;
            this.txtNamaRole.Tag = "nama_role";
            this.txtNamaRole.ThousandSeparator = false;
            // 
            // FrmEntryHakAkses
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 131);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmEntryHakAkses";
            this.Text = "FrmEntryRole";
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton rdoAktif;
        private System.Windows.Forms.RadioButton rdoNonAktif;
        private UserControl.AdvancedTextbox txtNamaRole;


    }
}