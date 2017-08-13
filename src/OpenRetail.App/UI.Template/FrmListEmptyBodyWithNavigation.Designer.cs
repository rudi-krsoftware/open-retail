namespace OpenRetail.App.UI.Template
{
    partial class FrmListEmptyBodyWithNavigation
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnImport = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnSelesai = new System.Windows.Forms.Button();
            this.btnHapus = new System.Windows.Forms.Button();
            this.btnPerbaiki = new System.Windows.Forms.Button();
            this.btnTambah = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.mnuPopupImportData = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuBukaFileMaster = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuImportFileMaster = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExportData = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnMoveFirst = new System.Windows.Forms.Button();
            this.btnMovePrevious = new System.Windows.Forms.Button();
            this.btnMoveNext = new System.Windows.Forms.Button();
            this.btnMoveLast = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.updLimit = new System.Windows.Forms.NumericUpDown();
            this.lblInfoNavigasi = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.mnuPopupImportData.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updLimit)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.pnlHeader, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(646, 41);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.btnImport);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeader.Location = new System.Drawing.Point(3, 3);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(640, 35);
            this.pnlHeader.TabIndex = 0;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Image = global::OpenRetail.App.Properties.Resources.import_data16;
            this.btnImport.Location = new System.Drawing.Point(591, 4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(39, 25);
            this.btnImport.TabIndex = 3;
            this.toolTip1.SetToolTip(this.btnImport, "Impor data dari excel");
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Visible = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(3, 7);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(58, 17);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Header";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.pnlFooter, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 405);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(646, 41);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFooter.Controls.Add(this.btnSelesai);
            this.pnlFooter.Controls.Add(this.btnHapus);
            this.pnlFooter.Controls.Add(this.btnPerbaiki);
            this.pnlFooter.Controls.Add(this.btnTambah);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFooter.Location = new System.Drawing.Point(3, 3);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(640, 35);
            this.pnlFooter.TabIndex = 0;
            // 
            // btnSelesai
            // 
            this.btnSelesai.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelesai.Location = new System.Drawing.Point(555, 6);
            this.btnSelesai.Name = "btnSelesai";
            this.btnSelesai.Size = new System.Drawing.Size(75, 23);
            this.btnSelesai.TabIndex = 3;
            this.btnSelesai.Text = "Esc Selesai";
            this.btnSelesai.UseVisualStyleBackColor = true;
            this.btnSelesai.Click += new System.EventHandler(this.btnSelesai_Click);
            // 
            // btnHapus
            // 
            this.btnHapus.Enabled = false;
            this.btnHapus.Location = new System.Drawing.Point(177, 6);
            this.btnHapus.Name = "btnHapus";
            this.btnHapus.Size = new System.Drawing.Size(75, 23);
            this.btnHapus.TabIndex = 2;
            this.btnHapus.Tag = "3";
            this.btnHapus.Text = "F10 Hapus";
            this.btnHapus.UseVisualStyleBackColor = true;
            this.btnHapus.Click += new System.EventHandler(this.btnHapus_Click);
            // 
            // btnPerbaiki
            // 
            this.btnPerbaiki.Enabled = false;
            this.btnPerbaiki.Location = new System.Drawing.Point(94, 6);
            this.btnPerbaiki.Name = "btnPerbaiki";
            this.btnPerbaiki.Size = new System.Drawing.Size(75, 23);
            this.btnPerbaiki.TabIndex = 1;
            this.btnPerbaiki.Tag = "2";
            this.btnPerbaiki.Text = "F9 Perbaiki";
            this.btnPerbaiki.UseVisualStyleBackColor = true;
            this.btnPerbaiki.Click += new System.EventHandler(this.btnPerbaiki_Click);
            // 
            // btnTambah
            // 
            this.btnTambah.Location = new System.Drawing.Point(11, 6);
            this.btnTambah.Name = "btnTambah";
            this.btnTambah.Size = new System.Drawing.Size(75, 23);
            this.btnTambah.TabIndex = 0;
            this.btnTambah.Tag = "1";
            this.btnTambah.Text = "F8 Tambah";
            this.btnTambah.UseVisualStyleBackColor = true;
            this.btnTambah.Click += new System.EventHandler(this.btnTambah_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Informasi";
            // 
            // mnuPopupImportData
            // 
            this.mnuPopupImportData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBukaFileMaster,
            this.mnuImportFileMaster,
            this.toolStripSeparator1,
            this.mnuExportData});
            this.mnuPopupImportData.Name = "mnuPopupImportData";
            this.mnuPopupImportData.Size = new System.Drawing.Size(189, 98);
            // 
            // mnuBukaFileMaster
            // 
            this.mnuBukaFileMaster.Name = "mnuBukaFileMaster";
            this.mnuBukaFileMaster.Size = new System.Drawing.Size(188, 22);
            this.mnuBukaFileMaster.Text = "Buka File Master ...";
            this.mnuBukaFileMaster.Visible = false;
            this.mnuBukaFileMaster.Click += new System.EventHandler(this.mnuBukaFileMaster_Click);
            // 
            // mnuImportFileMaster
            // 
            this.mnuImportFileMaster.Name = "mnuImportFileMaster";
            this.mnuImportFileMaster.Size = new System.Drawing.Size(188, 22);
            this.mnuImportFileMaster.Text = "Import Data Master ...";
            this.mnuImportFileMaster.Click += new System.EventHandler(this.mnuImportFileMaster_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
            // 
            // mnuExportData
            // 
            this.mnuExportData.Name = "mnuExportData";
            this.mnuExportData.Size = new System.Drawing.Size(188, 22);
            this.mnuExportData.Text = "Export Data ...";
            this.mnuExportData.Click += new System.EventHandler(this.mnuExportData_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 370);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(646, 35);
            this.tableLayoutPanel3.TabIndex = 6;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.lblInfoNavigasi, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(640, 29);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnMoveFirst);
            this.flowLayoutPanel1.Controls.Add(this.btnMovePrevious);
            this.flowLayoutPanel1.Controls.Add(this.btnMoveNext);
            this.flowLayoutPanel1.Controls.Add(this.btnMoveLast);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.updLimit);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(448, 29);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnMoveFirst
            // 
            this.btnMoveFirst.Location = new System.Drawing.Point(3, 3);
            this.btnMoveFirst.Name = "btnMoveFirst";
            this.btnMoveFirst.Size = new System.Drawing.Size(38, 23);
            this.btnMoveFirst.TabIndex = 0;
            this.btnMoveFirst.Text = "|<";
            this.btnMoveFirst.UseVisualStyleBackColor = true;
            this.btnMoveFirst.Click += new System.EventHandler(this.btnMoveFirst_Click);
            // 
            // btnMovePrevious
            // 
            this.btnMovePrevious.Location = new System.Drawing.Point(47, 3);
            this.btnMovePrevious.Name = "btnMovePrevious";
            this.btnMovePrevious.Size = new System.Drawing.Size(38, 23);
            this.btnMovePrevious.TabIndex = 0;
            this.btnMovePrevious.Text = "<";
            this.btnMovePrevious.UseVisualStyleBackColor = true;
            this.btnMovePrevious.Click += new System.EventHandler(this.btnMovePrevious_Click);
            // 
            // btnMoveNext
            // 
            this.btnMoveNext.Location = new System.Drawing.Point(91, 3);
            this.btnMoveNext.Name = "btnMoveNext";
            this.btnMoveNext.Size = new System.Drawing.Size(38, 23);
            this.btnMoveNext.TabIndex = 0;
            this.btnMoveNext.Text = ">";
            this.btnMoveNext.UseVisualStyleBackColor = true;
            this.btnMoveNext.Click += new System.EventHandler(this.btnMoveNext_Click);
            // 
            // btnMoveLast
            // 
            this.btnMoveLast.Location = new System.Drawing.Point(135, 3);
            this.btnMoveLast.Name = "btnMoveLast";
            this.btnMoveLast.Size = new System.Drawing.Size(38, 23);
            this.btnMoveLast.TabIndex = 0;
            this.btnMoveLast.Text = ">|";
            this.btnMoveLast.UseVisualStyleBackColor = true;
            this.btnMoveLast.Click += new System.EventHandler(this.btnMoveLast_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(179, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Limit";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // updLimit
            // 
            this.updLimit.Dock = System.Windows.Forms.DockStyle.Left;
            this.updLimit.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.updLimit.Location = new System.Drawing.Point(213, 5);
            this.updLimit.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.updLimit.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.updLimit.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.updLimit.Name = "updLimit";
            this.updLimit.ReadOnly = true;
            this.updLimit.Size = new System.Drawing.Size(42, 20);
            this.updLimit.TabIndex = 2;
            this.updLimit.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.updLimit.ValueChanged += new System.EventHandler(this.updLimit_ValueChanged);
            // 
            // lblInfoNavigasi
            // 
            this.lblInfoNavigasi.AutoSize = true;
            this.lblInfoNavigasi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfoNavigasi.Location = new System.Drawing.Point(451, 0);
            this.lblInfoNavigasi.Name = "lblInfoNavigasi";
            this.lblInfoNavigasi.Size = new System.Drawing.Size(186, 29);
            this.lblInfoNavigasi.TabIndex = 1;
            this.lblInfoNavigasi.Text = "lblInfoNavigasi";
            this.lblInfoNavigasi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FrmListEmptyBodyWithNavigation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 446);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "FrmListEmptyBodyWithNavigation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmListEmptyBodyWithNavigation";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmListEmptyBodyWithNavigation_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FrmListEmptyBodyWithNavigation_KeyPress);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.mnuPopupImportData.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updLimit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnSelesai;
        protected System.Windows.Forms.Button btnHapus;
        protected System.Windows.Forms.Button btnPerbaiki;
        protected System.Windows.Forms.Button btnTambah;
        protected System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip mnuPopupImportData;
        protected System.Windows.Forms.ToolStripMenuItem mnuBukaFileMaster;
        protected System.Windows.Forms.ToolStripMenuItem mnuImportFileMaster;
        protected System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnMoveFirst;
        private System.Windows.Forms.Button btnMovePrevious;
        private System.Windows.Forms.Button btnMoveNext;
        private System.Windows.Forms.Button btnMoveLast;
        private System.Windows.Forms.Label lblInfoNavigasi;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        protected System.Windows.Forms.ToolStripMenuItem mnuExportData;
        private System.Windows.Forms.Label label1;
        protected System.Windows.Forms.NumericUpDown updLimit;
    }
}