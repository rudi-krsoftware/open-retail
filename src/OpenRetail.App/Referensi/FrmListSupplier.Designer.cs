namespace OpenRetail.App.Referensi
{
    partial class FrmListSupplier
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCari = new System.Windows.Forms.Button();
            this.txtNamaSupplier = new OpenRetail.App.UserControl.AdvancedTextbox();
            this.gridList = new Syncfusion.Windows.Forms.Grid.GridListControl();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).BeginInit();
            this.SuspendLayout();
            
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.gridList, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(820, 359);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnCari);
            this.flowLayoutPanel1.Controls.Add(this.txtNamaSupplier);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(820, 28);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // btnCari
            // 
            this.btnCari.Image = global::OpenRetail.App.Properties.Resources.search16;
            this.btnCari.Location = new System.Drawing.Point(780, 0);
            this.btnCari.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.btnCari.Name = "btnCari";
            this.btnCari.Size = new System.Drawing.Size(37, 23);
            this.btnCari.TabIndex = 1;
            this.btnCari.UseVisualStyleBackColor = true;
            this.btnCari.Click += new System.EventHandler(this.btnCari_Click);
            // 
            // txtNamaSupplier
            // 
            this.txtNamaSupplier.AutoEnter = false;
            this.txtNamaSupplier.Conversion = OpenRetail.App.UserControl.EConversion.Normal;
            this.txtNamaSupplier.EnterFocusColor = System.Drawing.Color.White;
            this.txtNamaSupplier.LeaveFocusColor = System.Drawing.Color.White;
            this.txtNamaSupplier.LetterOnly = false;
            this.txtNamaSupplier.Location = new System.Drawing.Point(552, 3);
            this.txtNamaSupplier.Name = "txtNamaSupplier";
            this.txtNamaSupplier.NumericOnly = false;
            this.txtNamaSupplier.SelectionText = false;
            this.txtNamaSupplier.Size = new System.Drawing.Size(222, 20);
            this.txtNamaSupplier.TabIndex = 0;
            this.txtNamaSupplier.Text = "Cari nama supplier ...";
            this.txtNamaSupplier.ThousandSeparator = false;
            this.txtNamaSupplier.Enter += new System.EventHandler(this.txtNamaSupplier_Enter);
            this.txtNamaSupplier.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNamaSupplier_KeyPress);
            this.txtNamaSupplier.Leave += new System.EventHandler(this.txtNamaSupplier_Leave);
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
            this.gridList.Size = new System.Drawing.Size(814, 325);
            this.gridList.TabIndex = 0;
            this.gridList.TopIndex = 0;
            this.gridList.DoubleClick += new System.EventHandler(this.gridList_DoubleClick);
            // 
            // FrmListSupplier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 441);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Name = "FrmListSupplier";
            this.Text = "FrmListSupplier";
            this.Controls.SetChildIndex(this.tableLayoutPanel3, 0);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnCari;
        private UserControl.AdvancedTextbox txtNamaSupplier;
        private Syncfusion.Windows.Forms.Grid.GridListControl gridList;
    }
}