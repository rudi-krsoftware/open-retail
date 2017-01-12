namespace OpenRetail.App.UserControl
{
    partial class FilterRangeTanggal
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpTanggalMulai = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpTanggalSelesai = new System.Windows.Forms.DateTimePicker();
            this.btnTampilkan = new System.Windows.Forms.Button();
            this.chkTampilkanSemuaData = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.dtpTanggalMulai);
            this.flowLayoutPanel1.Controls.Add(this.label2);
            this.flowLayoutPanel1.Controls.Add(this.dtpTanggalSelesai);
            this.flowLayoutPanel1.Controls.Add(this.btnTampilkan);
            this.flowLayoutPanel1.Controls.Add(this.chkTampilkanSemuaData);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(469, 28);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tanggal";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpTanggalMulai
            // 
            this.dtpTanggalMulai.CustomFormat = "dd/MM/yyyy";
            this.dtpTanggalMulai.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTanggalMulai.Location = new System.Drawing.Point(55, 3);
            this.dtpTanggalMulai.Name = "dtpTanggalMulai";
            this.dtpTanggalMulai.Size = new System.Drawing.Size(98, 20);
            this.dtpTanggalMulai.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(159, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "s.d";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtpTanggalSelesai
            // 
            this.dtpTanggalSelesai.CustomFormat = "dd/MM/yyyy";
            this.dtpTanggalSelesai.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTanggalSelesai.Location = new System.Drawing.Point(186, 3);
            this.dtpTanggalSelesai.Name = "dtpTanggalSelesai";
            this.dtpTanggalSelesai.Size = new System.Drawing.Size(98, 20);
            this.dtpTanggalSelesai.TabIndex = 1;
            // 
            // btnTampilkan
            // 
            this.btnTampilkan.Image = global::OpenRetail.App.Properties.Resources.search16;
            this.btnTampilkan.Location = new System.Drawing.Point(290, 3);
            this.btnTampilkan.Name = "btnTampilkan";
            this.btnTampilkan.Size = new System.Drawing.Size(37, 21);
            this.btnTampilkan.TabIndex = 2;
            this.btnTampilkan.UseVisualStyleBackColor = true;
            this.btnTampilkan.Click += new System.EventHandler(this.btnTampilkan_Click);
            // 
            // chkTampilkanSemuaData
            // 
            this.chkTampilkanSemuaData.AutoSize = true;
            this.chkTampilkanSemuaData.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkTampilkanSemuaData.Location = new System.Drawing.Point(333, 3);
            this.chkTampilkanSemuaData.Name = "chkTampilkanSemuaData";
            this.chkTampilkanSemuaData.Size = new System.Drawing.Size(133, 21);
            this.chkTampilkanSemuaData.TabIndex = 3;
            this.chkTampilkanSemuaData.Text = "Tampilkan semua data";
            this.chkTampilkanSemuaData.UseVisualStyleBackColor = true;
            this.chkTampilkanSemuaData.CheckedChanged += new System.EventHandler(this.chkTampilkanSemuaData_CheckedChanged);
            // 
            // FilterRangeTanggal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "FilterRangeTanggal";
            this.Size = new System.Drawing.Size(469, 28);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTanggalMulai;
        private System.Windows.Forms.DateTimePicker dtpTanggalSelesai;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnTampilkan;
        private System.Windows.Forms.CheckBox chkTampilkanSemuaData;

    }
}
