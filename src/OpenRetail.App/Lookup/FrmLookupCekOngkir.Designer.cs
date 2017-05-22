namespace OpenRetail.App.Lookup
{
    partial class FrmLookupCekOngkir
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
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKabupatenAsal = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.txtBerat = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.btnCekOngkir = new System.Windows.Forms.Button();
            this.txtKabupatenTujuan = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.tableLayoutPanel4.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.gridList, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(517, 404);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // gridList
            // 
            this.gridList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(235)))), ((int)(((byte)(242)))));
            this.gridList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridList.ItemHeight = 17;
            this.gridList.Location = new System.Drawing.Point(3, 86);
            this.gridList.MultiColumn = false;
            this.gridList.Name = "gridList";
            this.gridList.Properties.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridList.Properties.ForceImmediateRepaint = false;
            this.gridList.Properties.MarkColHeader = false;
            this.gridList.Properties.MarkRowHeader = false;
            this.gridList.SelectedIndex = -1;
            this.gridList.Size = new System.Drawing.Size(511, 290);
            this.gridList.TabIndex = 1;
            this.gridList.TopIndex = 0;
            this.gridList.DoubleClick += new System.EventHandler(this.gridList_DoubleClick);
            this.gridList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.gridList_KeyPress);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtKabupatenAsal, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.flowLayoutPanel1, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.txtKabupatenTujuan, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(511, 77);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Kota/Kabupaten Asal";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "Kota/Kabupaten Tujuan";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 27);
            this.label3.TabIndex = 0;
            this.label3.Text = "Berat kiriman (gram)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtKabupatenAsal
            // 
            this.txtKabupatenAsal.AutoEnter = false;
            this.txtKabupatenAsal.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtKabupatenAsal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKabupatenAsal.EnterFocusColor = System.Drawing.Color.White;
            this.txtKabupatenAsal.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKabupatenAsal.LetterOnly = false;
            this.txtKabupatenAsal.Location = new System.Drawing.Point(131, 3);
            this.txtKabupatenAsal.Name = "txtKabupatenAsal";
            this.txtKabupatenAsal.NumericOnly = false;
            this.txtKabupatenAsal.SelectionText = false;
            this.txtKabupatenAsal.Size = new System.Drawing.Size(377, 20);
            this.txtKabupatenAsal.TabIndex = 0;
            this.txtKabupatenAsal.ThousandSeparator = false;
            this.txtKabupatenAsal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKabupatenAsal_KeyPress);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.txtBerat);
            this.flowLayoutPanel1.Controls.Add(this.btnCekOngkir);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(128, 50);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(383, 27);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // txtBerat
            // 
            this.txtBerat.AutoEnter = true;
            this.txtBerat.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtBerat.EnterFocusColor = System.Drawing.Color.White;
            this.txtBerat.LeaveFocusColor = System.Drawing.Color.White;
            this.txtBerat.LetterOnly = false;
            this.txtBerat.Location = new System.Drawing.Point(3, 3);
            this.txtBerat.MaxLength = 5;
            this.txtBerat.Name = "txtBerat";
            this.txtBerat.NumericOnly = true;
            this.txtBerat.SelectionText = false;
            this.txtBerat.Size = new System.Drawing.Size(58, 20);
            this.txtBerat.TabIndex = 0;
            this.txtBerat.Text = "0";
            this.txtBerat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtBerat.ThousandSeparator = false;
            // 
            // btnCekOngkir
            // 
            this.btnCekOngkir.Location = new System.Drawing.Point(67, 3);
            this.btnCekOngkir.Name = "btnCekOngkir";
            this.btnCekOngkir.Size = new System.Drawing.Size(89, 23);
            this.btnCekOngkir.TabIndex = 1;
            this.btnCekOngkir.Text = "Cek Ongkir";
            this.btnCekOngkir.UseVisualStyleBackColor = true;
            this.btnCekOngkir.Click += new System.EventHandler(this.btnCekOngkir_Click);
            // 
            // txtKabupatenTujuan
            // 
            this.txtKabupatenTujuan.AutoEnter = false;
            this.txtKabupatenTujuan.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtKabupatenTujuan.EnterFocusColor = System.Drawing.Color.White;
            this.txtKabupatenTujuan.LeaveFocusColor = System.Drawing.Color.White;
            this.txtKabupatenTujuan.LetterOnly = false;
            this.txtKabupatenTujuan.Location = new System.Drawing.Point(131, 28);
            this.txtKabupatenTujuan.Name = "txtKabupatenTujuan";
            this.txtKabupatenTujuan.NumericOnly = false;
            this.txtKabupatenTujuan.SelectionText = false;
            this.txtKabupatenTujuan.Size = new System.Drawing.Size(377, 20);
            this.txtKabupatenTujuan.TabIndex = 1;
            this.txtKabupatenTujuan.ThousandSeparator = false;
            this.txtKabupatenTujuan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKabupatenTujuan_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Location = new System.Drawing.Point(3, 379);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(319, 25);
            this.label4.TabIndex = 2;
            this.label4.Text = "Info: Untuk saat ini cek ongkir hanya sampai kota atau kabupaten";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmLookupCekOngkir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 486);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmLookupCekOngkir";
            this.Text = "FrmLookupCekOngkir";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private Syncfusion.Windows.Forms.Grid.GridListControl gridList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private UserControl.AdvancedTextbox txtBerat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnCekOngkir;
        private UserControl.AdvancedTextbox txtKabupatenAsal;
        private UserControl.AdvancedTextbox txtKabupatenTujuan;
    }
}