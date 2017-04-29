namespace OpenRetail.App.Referensi
{
    partial class FrmListProdukWithNavigation
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
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbGolongan = new System.Windows.Forms.ComboBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCari = new System.Windows.Forms.Button();
            this.txtNamaProduk = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.gridList = new Syncfusion.Windows.Forms.Grid.GridListControl();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnImport
            // 
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.gridList, 0, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(845, 357);
            this.tableLayoutPanel5.TabIndex = 7;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 541F));
            this.tableLayoutPanel6.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.cmbGolongan, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.flowLayoutPanel2, 2, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(845, 28);
            this.tableLayoutPanel6.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "Golongan";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbGolongan
            // 
            this.cmbGolongan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbGolongan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGolongan.FormattingEnabled = true;
            this.cmbGolongan.Location = new System.Drawing.Point(62, 3);
            this.cmbGolongan.Name = "cmbGolongan";
            this.cmbGolongan.Size = new System.Drawing.Size(239, 21);
            this.cmbGolongan.TabIndex = 1;
            this.cmbGolongan.SelectedIndexChanged += new System.EventHandler(this.cmbGolongan_SelectedIndexChanged);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnCari);
            this.flowLayoutPanel2.Controls.Add(this.txtNamaProduk);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(304, 0);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(541, 28);
            this.flowLayoutPanel2.TabIndex = 2;
            // 
            // btnCari
            // 
            this.btnCari.Image = global::OpenRetail.App.Properties.Resources.search16;
            this.btnCari.Location = new System.Drawing.Point(501, 0);
            this.btnCari.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(37, 23);
            this.btnCari.TabIndex = 1;
            this.btnCari.UseVisualStyleBackColor = true;
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // txtNamaProduk
            // 
            this.txtNamaProduk.AutoEnter = false;
            this.txtNamaProduk.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtNamaProduk.EnterFocusColor = System.Drawing.Color.White;
            this.txtNamaProduk.ForeColor = System.Drawing.Color.Black;
            this.txtNamaProduk.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNamaProduk.LetterOnly = false;
            this.txtNamaProduk.Location = new System.Drawing.Point(292, 3);
            this.txtNamaProduk.Name = "txtNamaProduk";
            this.txtNamaProduk.NumericOnly = false;
            this.txtNamaProduk.SelectionText = false;
            this.txtNamaProduk.Size = new System.Drawing.Size(203, 20);
            this.txtNamaProduk.TabIndex = 0;
            this.txtNamaProduk.ThousandSeparator = false;
            this.txtNamaProduk.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNamaProduk_KeyPress);
            // 
            // gridList
            // 
            this.gridList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridList.ItemHeight = 17;
            this.gridList.Location = new System.Drawing.Point(3, 31);
            this.gridList.MultiColumn = false;
            this.gridList.Name = "gridList";
            this.gridList.Properties.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridList.Properties.ForceImmediateRepaint = false;
            this.gridList.Properties.MarkColHeader = false;
            this.gridList.Properties.MarkRowHeader = false;
            this.gridList.SelectedIndex = -1;
            this.gridList.Size = new System.Drawing.Size(839, 323);
            this.gridList.TabIndex = 1;
            this.gridList.TopIndex = 0;
            this.gridList.DoubleClick += new System.EventHandler(this.gridList_DoubleClick);
            // 
            // FrmListProdukWithNavigation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 474);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Name = "FrmListProdukWithNavigation";
            this.Text = "FrmListProdukWithNavigation";
            this.Controls.SetChildIndex(this.tableLayoutPanel5, 0);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbGolongan;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private UserControl.AdvancedTextbox txtNamaProduk;
        private System.Windows.Forms.Button btnCari;
        private Syncfusion.Windows.Forms.Grid.GridListControl gridList;
    }
}