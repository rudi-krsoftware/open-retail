namespace OpenRetail.BackupAndRestore.Main
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnSelesai = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnProsesBackup = new System.Windows.Forms.Button();
            this.btnLokasiPenyimpananFileBackup = new System.Windows.Forms.Button();
            this.txtNamaFileBackup = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.txtLokasiPenyimpananFileBackup = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblInfoBackup = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnLokasiFileBackup = new System.Windows.Forms.Button();
            this.txtLokasiFileBackup = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblInfoRestore = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnProsesRestore = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.txtServer = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtLog = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnlHeader, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(622, 41);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(3, 3);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(616, 35);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(3, 7);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(294, 17);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Backup dan Restore OpenRetail Database";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.pnlFooter, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 575);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(622, 41);
            this.tableLayoutPanel2.TabIndex = 6;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFooter.Controls.Add(this.btnSelesai);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFooter.Location = new System.Drawing.Point(3, 3);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(616, 35);
            this.pnlFooter.TabIndex = 0;
            // 
            // btnSelesai
            // 
            this.btnSelesai.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelesai.Location = new System.Drawing.Point(530, 6);
            this.btnSelesai.Name = "btnSelesai";
            this.btnSelesai.Size = new System.Drawing.Size(75, 23);
            this.btnSelesai.TabIndex = 1;
            this.btnSelesai.Text = "Esc Selesai";
            this.btnSelesai.UseVisualStyleBackColor = true;
            this.btnSelesai.Click += new System.EventHandler(this.btnSelesai_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 34);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(616, 253);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnProsesBackup);
            this.tabPage1.Controls.Add(this.btnLokasiPenyimpananFileBackup);
            this.tabPage1.Controls.Add(this.txtNamaFileBackup);
            this.tabPage1.Controls.Add(this.txtLokasiPenyimpananFileBackup);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.lblInfoBackup);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(608, 227);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Backup Database";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnProsesBackup
            // 
            this.btnProsesBackup.Location = new System.Drawing.Point(522, 197);
            this.btnProsesBackup.Name = "btnProsesBackup";
            this.btnProsesBackup.Size = new System.Drawing.Size(75, 23);
            this.btnProsesBackup.TabIndex = 3;
            this.btnProsesBackup.Text = "Proses";
            this.btnProsesBackup.UseVisualStyleBackColor = true;
            this.btnProsesBackup.Click += new System.EventHandler(this.btnProsesBackup_Click);
            // 
            // btnLokasiPenyimpananFileBackup
            // 
            this.btnLokasiPenyimpananFileBackup.Image = global::OpenRetail.BackupAndRestore.Properties.Resources.open16;
            this.btnLokasiPenyimpananFileBackup.Location = new System.Drawing.Point(565, 168);
            this.btnLokasiPenyimpananFileBackup.Name = "btnLokasiPenyimpananFileBackup";
            this.btnLokasiPenyimpananFileBackup.Size = new System.Drawing.Size(32, 23);
            this.btnLokasiPenyimpananFileBackup.TabIndex = 2;
            this.btnLokasiPenyimpananFileBackup.UseVisualStyleBackColor = true;
            this.btnLokasiPenyimpananFileBackup.Click += new System.EventHandler(this.btnLokasiPenyimpananFileBackup_Click);
            // 
            // txtNamaFileBackup
            // 
            this.txtNamaFileBackup.AutoEnter = false;
            this.txtNamaFileBackup.BackColor = System.Drawing.Color.White;
            this.txtNamaFileBackup.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtNamaFileBackup.Enabled = false;
            this.txtNamaFileBackup.EnterFocusColor = System.Drawing.Color.White;
            this.txtNamaFileBackup.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNamaFileBackup.LetterOnly = false;
            this.txtNamaFileBackup.Location = new System.Drawing.Point(172, 144);
            this.txtNamaFileBackup.Name = "txtNamaFileBackup";
            this.txtNamaFileBackup.NumericOnly = false;
            this.txtNamaFileBackup.SelectionText = false;
            this.txtNamaFileBackup.Size = new System.Drawing.Size(224, 20);
            this.txtNamaFileBackup.TabIndex = 0;
            this.txtNamaFileBackup.ThousandSeparator = false;
            // 
            // txtLokasiPenyimpananFileBackup
            // 
            this.txtLokasiPenyimpananFileBackup.AutoEnter = false;
            this.txtLokasiPenyimpananFileBackup.BackColor = System.Drawing.Color.White;
            this.txtLokasiPenyimpananFileBackup.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtLokasiPenyimpananFileBackup.EnterFocusColor = System.Drawing.Color.White;
            this.txtLokasiPenyimpananFileBackup.LeaveFocusColor = System.Drawing.Color.White;
            this.txtLokasiPenyimpananFileBackup.LetterOnly = false;
            this.txtLokasiPenyimpananFileBackup.Location = new System.Drawing.Point(172, 170);
            this.txtLokasiPenyimpananFileBackup.Name = "txtLokasiPenyimpananFileBackup";
            this.txtLokasiPenyimpananFileBackup.NumericOnly = false;
            this.txtLokasiPenyimpananFileBackup.ReadOnly = true;
            this.txtLokasiPenyimpananFileBackup.SelectionText = false;
            this.txtLokasiPenyimpananFileBackup.Size = new System.Drawing.Size(387, 20);
            this.txtLokasiPenyimpananFileBackup.TabIndex = 1;
            this.txtLokasiPenyimpananFileBackup.ThousandSeparator = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Nama File Backup";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 173);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(164, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Lokasi Penyimpanan File Backup";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInfoBackup
            // 
            this.lblInfoBackup.BackColor = System.Drawing.Color.Transparent;
            this.lblInfoBackup.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoBackup.Location = new System.Drawing.Point(6, 32);
            this.lblInfoBackup.Name = "lblInfoBackup";
            this.lblInfoBackup.Size = new System.Drawing.Size(591, 109);
            this.lblInfoBackup.TabIndex = 0;
            this.lblInfoBackup.Text = "lblInfoBackup";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Backup Database";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnLokasiFileBackup);
            this.tabPage2.Controls.Add(this.txtLokasiFileBackup);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.lblInfoRestore);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.btnProsesRestore);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(608, 227);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Restore Database";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnLokasiFileBackup
            // 
            this.btnLokasiFileBackup.Image = global::OpenRetail.BackupAndRestore.Properties.Resources.open16;
            this.btnLokasiFileBackup.Location = new System.Drawing.Point(565, 168);
            this.btnLokasiFileBackup.Name = "btnLokasiFileBackup";
            this.btnLokasiFileBackup.Size = new System.Drawing.Size(32, 23);
            this.btnLokasiFileBackup.TabIndex = 5;
            this.btnLokasiFileBackup.UseVisualStyleBackColor = true;
            this.btnLokasiFileBackup.Click += new System.EventHandler(this.btnLokasiFileBackup_Click);
            // 
            // txtLokasiFileBackup
            // 
            this.txtLokasiFileBackup.AutoEnter = false;
            this.txtLokasiFileBackup.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtLokasiFileBackup.EnterFocusColor = System.Drawing.Color.White;
            this.txtLokasiFileBackup.LeaveFocusColor = System.Drawing.Color.White;
            this.txtLokasiFileBackup.LetterOnly = false;
            this.txtLokasiFileBackup.Location = new System.Drawing.Point(106, 170);
            this.txtLokasiFileBackup.Name = "txtLokasiFileBackup";
            this.txtLokasiFileBackup.NumericOnly = false;
            this.txtLokasiFileBackup.ReadOnly = true;
            this.txtLokasiFileBackup.SelectionText = false;
            this.txtLokasiFileBackup.Size = new System.Drawing.Size(453, 20);
            this.txtLokasiFileBackup.TabIndex = 4;
            this.txtLokasiFileBackup.ThousandSeparator = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Nama File Backup";
            // 
            // lblInfoRestore
            // 
            this.lblInfoRestore.BackColor = System.Drawing.Color.Transparent;
            this.lblInfoRestore.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInfoRestore.Location = new System.Drawing.Point(6, 32);
            this.lblInfoRestore.Name = "lblInfoRestore";
            this.lblInfoRestore.Size = new System.Drawing.Size(591, 130);
            this.lblInfoRestore.TabIndex = 2;
            this.lblInfoRestore.Text = "lblInfoRestore";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 14);
            this.label4.TabIndex = 2;
            this.label4.Text = "Restore Database";
            // 
            // btnProsesRestore
            // 
            this.btnProsesRestore.Location = new System.Drawing.Point(522, 197);
            this.btnProsesRestore.Name = "btnProsesRestore";
            this.btnProsesRestore.Size = new System.Drawing.Size(75, 23);
            this.btnProsesRestore.TabIndex = 1;
            this.btnProsesRestore.Text = "Proses";
            this.btnProsesRestore.UseVisualStyleBackColor = true;
            this.btnProsesRestore.Click += new System.EventHandler(this.btnProsesRestore_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Server Database";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 1;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Controls.Add(this.tabControl1, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel7, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.groupBox1, 0, 2);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 3;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 259F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(622, 534);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel7.Controls.Add(this.txtServer, 1, 0);
            this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(616, 25);
            this.tableLayoutPanel7.TabIndex = 0;
            // 
            // txtServer
            // 
            this.txtServer.AutoEnter = false;
            this.txtServer.BackColor = System.Drawing.Color.White;
            this.txtServer.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtServer.EnterFocusColor = System.Drawing.Color.White;
            this.txtServer.LeaveFocusColor = System.Drawing.Color.White;
            this.txtServer.LetterOnly = false;
            this.txtServer.Location = new System.Drawing.Point(96, 3);
            this.txtServer.Name = "txtServer";
            this.txtServer.NumericOnly = false;
            this.txtServer.SelectionText = false;
            this.txtServer.Size = new System.Drawing.Size(84, 20);
            this.txtServer.TabIndex = 0;
            this.txtServer.Text = "172.16.15.12";
            this.txtServer.ThousandSeparator = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtLog);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 293);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(616, 238);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " [ Log Backup && Restore ] ";
            // 
            // txtLog
            // 
            this.txtLog.AutoEnter = false;
            this.txtLog.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.EnterFocusColor = System.Drawing.Color.White;
            this.txtLog.LeaveFocusColor = System.Drawing.Color.White;
            this.txtLog.LetterOnly = false;
            this.txtLog.Location = new System.Drawing.Point(3, 16);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.NumericOnly = false;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.SelectionText = false;
            this.txtLog.Size = new System.Drawing.Size(610, 219);
            this.txtLog.TabIndex = 3;
            this.txtLog.ThousandSeparator = false;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 616);
            this.Controls.Add(this.tableLayoutPanel6);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Backup dan Restore OpenRetail Database";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmMain_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnSelesai;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblInfoBackup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private Helper.UserControl.AdvancedTextbox txtServer;
        private Helper.UserControl.AdvancedTextbox txtLokasiPenyimpananFileBackup;
        private System.Windows.Forms.Button btnLokasiPenyimpananFileBackup;
        private System.Windows.Forms.Button btnProsesBackup;
        private Helper.UserControl.AdvancedTextbox txtLog;
        private System.Windows.Forms.Button btnProsesRestore;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
        private System.Windows.Forms.GroupBox groupBox1;
        private Helper.UserControl.AdvancedTextbox txtNamaFileBackup;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblInfoRestore;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLokasiFileBackup;
        private Helper.UserControl.AdvancedTextbox txtLokasiFileBackup;
        private System.Windows.Forms.Label label6;
    }
}