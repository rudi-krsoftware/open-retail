namespace OpenRetail.App.Transaksi
{
    partial class FrmListPenjualanProdukWithNavigation
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
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.gridList = new Syncfusion.Windows.Forms.Grid.GridListControl();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.filterRangeTanggal = new OpenRetail.Helper.UserControl.FilterRangeTanggal();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCari = new System.Windows.Forms.Button();
            this.txtNamaCustomer = new OpenRetail.Helper.UserControl.AdvancedTextbox();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.tableLayoutPanel6.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(1592, 4);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.gridList, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(789, 367);
            this.tableLayoutPanel5.TabIndex = 6;
            // 
            // gridList
            // 
            this.gridList.BackColor = System.Drawing.SystemColors.Control;
            this.gridList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridList.ItemHeight = 17;
            this.gridList.Location = new System.Drawing.Point(3, 32);
            this.gridList.Name = "gridList";
            this.gridList.Properties.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridList.SelectedIndex = -1;
            this.gridList.Size = new System.Drawing.Size(783, 332);
            this.gridList.TabIndex = 1;
            this.gridList.TopIndex = 0;
            this.gridList.DoubleClick += new System.EventHandler(this.gridList_DoubleClick);
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 2;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.7237F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.2763F));
            this.tableLayoutPanel6.Controls.Add(this.filterRangeTanggal, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.flowLayoutPanel2, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(789, 29);
            this.tableLayoutPanel6.TabIndex = 3;
            // 
            // filterRangeTanggal
            // 
            this.filterRangeTanggal.Location = new System.Drawing.Point(3, 3);
            this.filterRangeTanggal.Name = "filterRangeTanggal";
            this.filterRangeTanggal.Size = new System.Drawing.Size(469, 23);
            this.filterRangeTanggal.TabIndex = 2;
            this.filterRangeTanggal.BtnTampilkanClicked += new OpenRetail.Helper.UserControl.FilterRangeTanggal.EventHandler(this.filterRangeTanggal_BtnTampilkanClicked);
            this.filterRangeTanggal.ChkTampilkanSemuaDataClicked += new OpenRetail.Helper.UserControl.FilterRangeTanggal.EventHandler(this.filterRangeTanggal_ChkTampilkanSemuaDataClicked);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnCari);
            this.flowLayoutPanel2.Controls.Add(this.txtNamaCustomer);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(487, 0);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(302, 29);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // btnCari
            // 
            this.btnCari.Image = global::OpenRetail.App.Properties.Resources.search16;
            this.btnCari.Location = new System.Drawing.Point(262, 3);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(37, 23);
            this.btnCari.TabIndex = 1;
            this.toolTip1.SetToolTip(this.btnCari, "Cari nama customer");
            this.btnCari.UseVisualStyleBackColor = true;
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // txtNamaCustomer
            // 
            this.txtNamaCustomer.AutoEnter = false;
            this.txtNamaCustomer.Conversion = OpenRetail.Helper.UserControl.EConversion.Normal;
            this.txtNamaCustomer.EnterFocusColor = System.Drawing.Color.White;
            this.txtNamaCustomer.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNamaCustomer.LetterOnly = false;
            this.txtNamaCustomer.Location = new System.Drawing.Point(38, 3);
            this.txtNamaCustomer.Name = "txtNamaCustomer";
            this.txtNamaCustomer.NumericOnly = false;
            this.txtNamaCustomer.SelectionText = false;
            this.txtNamaCustomer.Size = new System.Drawing.Size(218, 20);
            this.txtNamaCustomer.TabIndex = 0;
            this.txtNamaCustomer.ThousandSeparator = false;
            this.txtNamaCustomer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNamaCustomer_KeyPress);
            // 
            // FrmListPenjualanProdukWithNavigation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 449);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Name = "FrmListPenjualanProdukWithNavigation";
            this.Text = "FrmListPenjualanProdukWithNavigation";
            this.Controls.SetChildIndex(this.tableLayoutPanel5, 0);
            this.tableLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private Syncfusion.Windows.Forms.Grid.GridListControl gridList;
        private OpenRetail.Helper.UserControl.FilterRangeTanggal filterRangeTanggal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnCari;
        private OpenRetail.Helper.UserControl.AdvancedTextbox txtNamaCustomer;
    }
}