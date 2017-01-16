namespace OpenRetail.App.Pengaturan
{
    partial class FrmEntryOperator
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbRole = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.rdoAktif = new System.Windows.Forms.RadioButton();
            this.rdoNonAktif = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNama = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtPassword = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.txtKonfirmasiPassword = new OpenRetail.App.UserControl.AdvancedTextbox();
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
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.cmbRole, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.txtNama, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.txtPassword, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.txtKonfirmasiPassword, 1, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 6;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(329, 127);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nama";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 25);
            this.label3.TabIndex = 1;
            this.label3.Text = "Konf. Password";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(3, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 25);
            this.label4.TabIndex = 1;
            this.label4.Text = "Role";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbRole
            // 
            this.cmbRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRole.FormattingEnabled = true;
            this.cmbRole.Location = new System.Drawing.Point(90, 78);
            this.cmbRole.Name = "cmbRole";
            this.cmbRole.Size = new System.Drawing.Size(236, 21);
            this.cmbRole.TabIndex = 3;
            this.cmbRole.Tag = "ignore";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.rdoAktif);
            this.flowLayoutPanel1.Controls.Add(this.rdoNonAktif);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(87, 100);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(242, 25);
            this.flowLayoutPanel1.TabIndex = 4;
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(3, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 25);
            this.label5.TabIndex = 1;
            this.label5.Text = "Status Aktif";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtNama
            // 
            this.txtNama.AutoEnter = true;
            this.txtNama.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtNama.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNama.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtNama.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNama.LetterOnly = false;
            this.txtNama.Location = new System.Drawing.Point(90, 3);
            this.txtNama.Name = "txtNama";
            this.txtNama.NumericOnly = false;
            this.txtNama.SelectionText = false;
            this.txtNama.Size = new System.Drawing.Size(236, 20);
            this.txtNama.TabIndex = 0;
            this.txtNama.Tag = "nama_pengguna";
            this.txtNama.ThousandSeparator = false;
            // 
            // txtPassword
            // 
            this.txtPassword.AutoEnter = true;
            this.txtPassword.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtPassword.LeaveFocusColor = System.Drawing.Color.White;
            this.txtPassword.LetterOnly = false;
            this.txtPassword.Location = new System.Drawing.Point(90, 28);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.NumericOnly = false;
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.SelectionText = false;
            this.txtPassword.Size = new System.Drawing.Size(236, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Tag = "pass_pengguna";
            this.txtPassword.ThousandSeparator = false;
            // 
            // txtKonfirmasiPassword
            // 
            this.txtKonfirmasiPassword.AutoEnter = true;
            this.txtKonfirmasiPassword.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtKonfirmasiPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKonfirmasiPassword.EnterFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtKonfirmasiPassword.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKonfirmasiPassword.LetterOnly = false;
            this.txtKonfirmasiPassword.Location = new System.Drawing.Point(90, 53);
            this.txtKonfirmasiPassword.Name = "txtKonfirmasiPassword";
            this.txtKonfirmasiPassword.NumericOnly = false;
            this.txtKonfirmasiPassword.PasswordChar = '*';
            this.txtKonfirmasiPassword.SelectionText = false;
            this.txtKonfirmasiPassword.Size = new System.Drawing.Size(236, 20);
            this.txtKonfirmasiPassword.TabIndex = 2;
            this.txtKonfirmasiPassword.Tag = "konf_pass_pengguna";
            this.txtKonfirmasiPassword.ThousandSeparator = false;
            // 
            // FrmEntryOperator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 209);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmEntryOperator";
            this.Text = "FrmEntryOperator";
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbRole;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton rdoAktif;
        private System.Windows.Forms.RadioButton rdoNonAktif;
        private System.Windows.Forms.Label label5;
        private UserControl.AdvancedTextbox txtNama;
        private UserControl.AdvancedTextbox txtPassword;
        private UserControl.AdvancedTextbox txtKonfirmasiPassword;

    }
}