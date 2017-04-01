namespace OpenRetail.App.Pengeluaran
{
    partial class FrmListPenggajianKaryawan
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
            this.gridList = new Syncfusion.Windows.Forms.Grid.GridListControl();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbBulan = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTahun = new System.Windows.Forms.ComboBox();
            this.btnTampilkan = new System.Windows.Forms.Button();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.gridList, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(789, 367);
            this.tableLayoutPanel3.TabIndex = 6;
            // 
            // gridList
            // 
            this.gridList.BackColor = System.Drawing.SystemColors.Control;
            this.gridList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridList.ItemHeight = 17;
            this.gridList.Location = new System.Drawing.Point(3, 32);
            this.gridList.MultiColumn = false;
            this.gridList.Name = "gridList";
            this.gridList.Properties.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridList.Properties.ForceImmediateRepaint = false;
            this.gridList.Properties.MarkColHeader = false;
            this.gridList.Properties.MarkRowHeader = false;
            this.gridList.SelectedIndex = -1;
            this.gridList.Size = new System.Drawing.Size(783, 332);
            this.gridList.TabIndex = 1;
            this.gridList.TopIndex = 0;
            this.gridList.DoubleClick += new System.EventHandler(this.gridList_DoubleClick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.cmbBulan);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.cmbTahun);
            this.flowLayoutPanel1.Controls.Add(this.btnTampilkan);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(789, 29);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Periode";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbBulan
            // 
            this.cmbBulan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBulan.FormattingEnabled = true;
            this.cmbBulan.Location = new System.Drawing.Point(52, 3);
            this.cmbBulan.Name = "cmbBulan";
            this.cmbBulan.Size = new System.Drawing.Size(100, 21);
            this.cmbBulan.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(158, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tahun";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbTahun
            // 
            this.cmbTahun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTahun.FormattingEnabled = true;
            this.cmbTahun.Location = new System.Drawing.Point(202, 3);
            this.cmbTahun.Name = "cmbTahun";
            this.cmbTahun.Size = new System.Drawing.Size(83, 21);
            this.cmbTahun.TabIndex = 1;
            // 
            // btnTampilkan
            // 
            this.btnTampilkan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTampilkan.Image = global::OpenRetail.App.Properties.Resources.search16;
            this.btnTampilkan.Location = new System.Drawing.Point(291, 3);
            this.btnTampilkan.Name = "btnTampilkan";
            this.btnTampilkan.Size = new System.Drawing.Size(37, 21);
            this.btnTampilkan.TabIndex = 2;
            this.btnTampilkan.UseVisualStyleBackColor = true;
            this.btnTampilkan.Click += new System.EventHandler(this.btnTampilkan_Click);
            // 
            // FrmListPenggajianKaryawan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 449);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmListPenggajianKaryawan";
            this.Text = "FrmListPenggajianKaryawan";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Syncfusion.Windows.Forms.Grid.GridListControl gridList;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbBulan;
        private System.Windows.Forms.ComboBox cmbTahun;
        private System.Windows.Forms.Button btnTampilkan;
        private System.Windows.Forms.Label label2;
    }
}