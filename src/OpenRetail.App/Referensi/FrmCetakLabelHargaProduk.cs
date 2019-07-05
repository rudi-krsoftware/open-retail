/**
 * Copyright (C) 2017 Kamarudin (http://coding4ever.net/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 *
 * The latest version of this file can be found at https://github.com/rudi-krsoftware/open-retail
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

using log4net;
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Helper;
using OpenRetail.Bll.Service;
using OpenRetail.Helper.UserControl;
using OpenRetail.App.Lookup;
using System.Drawing.Text;

namespace OpenRetail.App.Referensi
{
    public partial class FrmCetakLabelHargaProduk : Form, IListener
    {
        private Produk _produk = null;
        private PengaturanLabelHarga _pengaturanLabelHarga = null;
        private IList<CheckBox> _listOfCheckboxPosisiLabel = new List<CheckBox>();
        private IList<Panel> _listOfPanelPosisiLabel = new List<Panel>();
        private IList<Panel> _listOfPanelPosisiLabel2 = new List<Panel>();
        private IList<LabelHargaProduk> _listOfLabelHargaProduk = new List<LabelHargaProduk>();

        private ILog _log;

        public FrmCetakLabelHargaProduk(string header)
        {
            InitializeComponent();            
            ColorManagerHelper.SetTheme(this, this);

            this.Text = header;
            this.lblHeader.Text = header;
            this._log = MainProgram.log;
            this._pengaturanLabelHarga = MainProgram.pengaturanLabelHarga;

            InitializeList();
            LoadPengaturanLabelHargaProduk();
        }

        private void LoadPengaturanLabelHargaProduk()
        {
            LoadPrinter(_pengaturanLabelHarga.nama_printer);

            txtBatasAtasBaris1.Text = _pengaturanLabelHarga.batas_atas_baris1.ToString();
            txtBatasAtasBaris2.Text = _pengaturanLabelHarga.batas_atas_baris2.ToString();
            txtBatasAtasBaris3.Text = _pengaturanLabelHarga.batas_atas_baris3.ToString();
            txtBatasAtasBaris4.Text = _pengaturanLabelHarga.batas_atas_baris4.ToString();
            txtBatasAtasBaris5.Text = _pengaturanLabelHarga.batas_atas_baris5.ToString();
            txtBatasAtasBaris6.Text = _pengaturanLabelHarga.batas_atas_baris6.ToString();
            txtBatasAtasBaris7.Text = _pengaturanLabelHarga.batas_atas_baris7.ToString();
            txtBatasAtasBaris8.Text = _pengaturanLabelHarga.batas_atas_baris8.ToString();

            txtBatasKiriKolom1.Text = _pengaturanLabelHarga.batas_kiri_kolom1.ToString();
            txtBatasKiriKolom2.Text = _pengaturanLabelHarga.batas_kiri_kolom2.ToString();
            txtBatasKiriKolom3.Text = _pengaturanLabelHarga.batas_kiri_kolom3.ToString();
            txtBatasKiriKolom4.Text = _pengaturanLabelHarga.batas_kiri_kolom4.ToString();
        }

        private void InitializeList()
        {
            _listOfPanelPosisiLabel.Add(pnlPosisi1);
            _listOfPanelPosisiLabel.Add(pnlPosisi2);
            _listOfPanelPosisiLabel.Add(pnlPosisi3);
            _listOfPanelPosisiLabel.Add(pnlPosisi4);
            _listOfPanelPosisiLabel.Add(pnlPosisi5);
            _listOfPanelPosisiLabel.Add(pnlPosisi6);
            _listOfPanelPosisiLabel.Add(pnlPosisi7);
            _listOfPanelPosisiLabel.Add(pnlPosisi8);
            _listOfPanelPosisiLabel.Add(pnlPosisi9);
            _listOfPanelPosisiLabel.Add(pnlPosisi10);
            _listOfPanelPosisiLabel.Add(pnlPosisi11);
            _listOfPanelPosisiLabel.Add(pnlPosisi12);
            _listOfPanelPosisiLabel.Add(pnlPosisi13);
            _listOfPanelPosisiLabel.Add(pnlPosisi14);
            _listOfPanelPosisiLabel.Add(pnlPosisi15);
            _listOfPanelPosisiLabel.Add(pnlPosisi16);
            _listOfPanelPosisiLabel.Add(pnlPosisi17);
            _listOfPanelPosisiLabel.Add(pnlPosisi18);
            _listOfPanelPosisiLabel.Add(pnlPosisi19);
            _listOfPanelPosisiLabel.Add(pnlPosisi20);
            _listOfPanelPosisiLabel.Add(pnlPosisi21);
            _listOfPanelPosisiLabel.Add(pnlPosisi22);
            _listOfPanelPosisiLabel.Add(pnlPosisi23);
            _listOfPanelPosisiLabel.Add(pnlPosisi24);
            _listOfPanelPosisiLabel.Add(pnlPosisi25);
            _listOfPanelPosisiLabel.Add(pnlPosisi26);
            _listOfPanelPosisiLabel.Add(pnlPosisi27);
            _listOfPanelPosisiLabel.Add(pnlPosisi28);
            _listOfPanelPosisiLabel.Add(pnlPosisi29);
            _listOfPanelPosisiLabel.Add(pnlPosisi30);
            _listOfPanelPosisiLabel.Add(pnlPosisi31);
            _listOfPanelPosisiLabel.Add(pnlPosisi32);

            _listOfPanelPosisiLabel2.Add(panel14);
            _listOfPanelPosisiLabel2.Add(panel18);
            _listOfPanelPosisiLabel2.Add(panel22);
            _listOfPanelPosisiLabel2.Add(panel41);
            _listOfPanelPosisiLabel2.Add(panel15);
            _listOfPanelPosisiLabel2.Add(panel19);
            _listOfPanelPosisiLabel2.Add(panel23);
            _listOfPanelPosisiLabel2.Add(panel43);
            _listOfPanelPosisiLabel2.Add(panel16);
            _listOfPanelPosisiLabel2.Add(panel20);
            _listOfPanelPosisiLabel2.Add(panel24);
            _listOfPanelPosisiLabel2.Add(panel45);
            _listOfPanelPosisiLabel2.Add(panel17);
            _listOfPanelPosisiLabel2.Add(panel21);
            _listOfPanelPosisiLabel2.Add(panel37);
            _listOfPanelPosisiLabel2.Add(panel47);
            _listOfPanelPosisiLabel2.Add(panel28);
            _listOfPanelPosisiLabel2.Add(panel30);
            _listOfPanelPosisiLabel2.Add(panel32);
            _listOfPanelPosisiLabel2.Add(panel49);
            _listOfPanelPosisiLabel2.Add(panel34);
            _listOfPanelPosisiLabel2.Add(panel36);
            _listOfPanelPosisiLabel2.Add(panel39);
            _listOfPanelPosisiLabel2.Add(panel51);
            _listOfPanelPosisiLabel2.Add(panel53);
            _listOfPanelPosisiLabel2.Add(panel55);
            _listOfPanelPosisiLabel2.Add(panel57);
            _listOfPanelPosisiLabel2.Add(panel59);
            _listOfPanelPosisiLabel2.Add(panel61);
            _listOfPanelPosisiLabel2.Add(panel63);
            _listOfPanelPosisiLabel2.Add(panel65);
            _listOfPanelPosisiLabel2.Add(panel67);

            _listOfCheckboxPosisiLabel.Add(chkPosisi1);
            _listOfCheckboxPosisiLabel.Add(chkPosisi2);
            _listOfCheckboxPosisiLabel.Add(chkPosisi3);
            _listOfCheckboxPosisiLabel.Add(chkPosisi4);
            _listOfCheckboxPosisiLabel.Add(chkPosisi5);
            _listOfCheckboxPosisiLabel.Add(chkPosisi6);
            _listOfCheckboxPosisiLabel.Add(chkPosisi7);
            _listOfCheckboxPosisiLabel.Add(chkPosisi8);
            _listOfCheckboxPosisiLabel.Add(chkPosisi9);
            _listOfCheckboxPosisiLabel.Add(chkPosisi10);
            _listOfCheckboxPosisiLabel.Add(chkPosisi11);
            _listOfCheckboxPosisiLabel.Add(chkPosisi12);
            _listOfCheckboxPosisiLabel.Add(chkPosisi13);
            _listOfCheckboxPosisiLabel.Add(chkPosisi14);
            _listOfCheckboxPosisiLabel.Add(chkPosisi15);
            _listOfCheckboxPosisiLabel.Add(chkPosisi16);
            _listOfCheckboxPosisiLabel.Add(chkPosisi17);
            _listOfCheckboxPosisiLabel.Add(chkPosisi18);
            _listOfCheckboxPosisiLabel.Add(chkPosisi19);
            _listOfCheckboxPosisiLabel.Add(chkPosisi20);
            _listOfCheckboxPosisiLabel.Add(chkPosisi21);
            _listOfCheckboxPosisiLabel.Add(chkPosisi22);
            _listOfCheckboxPosisiLabel.Add(chkPosisi23);
            _listOfCheckboxPosisiLabel.Add(chkPosisi24);
            _listOfCheckboxPosisiLabel.Add(chkPosisi25);
            _listOfCheckboxPosisiLabel.Add(chkPosisi26);
            _listOfCheckboxPosisiLabel.Add(chkPosisi27);
            _listOfCheckboxPosisiLabel.Add(chkPosisi28);
            _listOfCheckboxPosisiLabel.Add(chkPosisi29);
            _listOfCheckboxPosisiLabel.Add(chkPosisi30);
            _listOfCheckboxPosisiLabel.Add(chkPosisi31);
            _listOfCheckboxPosisiLabel.Add(chkPosisi32);

            // non aktifkan checkbox pilihan posisi label
            foreach (var checkbox in _listOfCheckboxPosisiLabel)
            {
                checkbox.Enabled = false;
            }
        }

        private void LoadPrinter(string defaultPrinter)
        {
            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                cmbPrinter.Items.Add(printer);
            }

            if (defaultPrinter.Length > 0)
                cmbPrinter.Text = defaultPrinter;
            else
            {
                if (cmbPrinter.Items.Count > 0)
                    cmbPrinter.SelectedIndex = 0;
            }
        }

        private void PreviewLabelHargaProduk(string fontName = "Courier New")
        {
            if (txtKodeProduk.Text.Length > 0 && _produk != null)
            {
                labelHargaProdukPanel.FontName = fontName;
                labelHargaProdukPanel.KodeProduk = _produk.kode_produk;
                labelHargaProdukPanel.NamaProduk = _produk.nama_produk;
                labelHargaProdukPanel.HargaProduk = _produk.harga_jual;
                labelHargaProdukPanel.LastUpdate = _produk.last_update;                

                labelHargaProdukPanel.GenerateLabel();
            }
        }

        private void ResetLabelHarga(bool resetAll = false)
        {            
            labelHargaProdukPanel.BackgroundImage = null;

            if (!resetAll) return;

            var index = 0;
            foreach (var panel in _listOfPanelPosisiLabel)
            {
                panel.BackgroundImage = labelHargaProdukPanel.BackgroundImage;
                _listOfPanelPosisiLabel2[index].BackgroundImage = labelHargaProdukPanel.BackgroundImage;

                index++;
            }

            foreach (var checkbox in _listOfCheckboxPosisiLabel)
            {
                checkbox.Enabled = false;
                checkbox.Checked = false;
            }

            _listOfLabelHargaProduk.Clear();

            txtKodeProduk.Clear();
            txtKodeProduk.Focus();
            txtNamaProduk.Clear();
            txtHargaJual.Text = "0";

            updJumlahCetak.Enabled = false;
            btnCetak.Enabled = false;
            chkPilihSemua.Enabled = false;
            chkPilihSemua.Checked = false;
        }

        private void SetDataProduk(Produk produk)
        {
            txtKodeProduk.Text = produk.kode_produk;
            txtNamaProduk.Text = produk.nama_produk;
            txtHargaJual.Text = produk.harga_jual.ToString();
        }

        private void SaveAppConfig()
        {
            var appConfigFile = string.Format("{0}\\OpenRetail.exe.config", Utils.GetAppPath());

            this._pengaturanLabelHarga.nama_printer = cmbPrinter.Text;
            this._pengaturanLabelHarga.batas_atas_baris1 = Convert.ToSingle(txtBatasAtasBaris1.Text);
            this._pengaturanLabelHarga.batas_atas_baris2 = Convert.ToSingle(txtBatasAtasBaris2.Text);
            this._pengaturanLabelHarga.batas_atas_baris3 = Convert.ToSingle(txtBatasAtasBaris3.Text);
            this._pengaturanLabelHarga.batas_atas_baris4 = Convert.ToSingle(txtBatasAtasBaris4.Text);
            this._pengaturanLabelHarga.batas_atas_baris5 = Convert.ToSingle(txtBatasAtasBaris5.Text);
            this._pengaturanLabelHarga.batas_atas_baris6 = Convert.ToSingle(txtBatasAtasBaris6.Text);
            this._pengaturanLabelHarga.batas_atas_baris7 = Convert.ToSingle(txtBatasAtasBaris7.Text);
            this._pengaturanLabelHarga.batas_atas_baris8 = Convert.ToSingle(txtBatasAtasBaris8.Text);

            this._pengaturanLabelHarga.batas_kiri_kolom1 = Convert.ToSingle(txtBatasKiriKolom1.Text);
            this._pengaturanLabelHarga.batas_kiri_kolom2 = Convert.ToSingle(txtBatasKiriKolom2.Text);
            this._pengaturanLabelHarga.batas_kiri_kolom3 = Convert.ToSingle(txtBatasKiriKolom3.Text);
            this._pengaturanLabelHarga.batas_kiri_kolom4 = Convert.ToSingle(txtBatasKiriKolom4.Text);

            AppConfigHelper.SaveValue("printerLabelHarga", cmbPrinter.Text, appConfigFile);

            AppConfigHelper.SaveValue("batasAtasBaris1LabelHarga", txtBatasAtasBaris1.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasAtasBaris2LabelHarga", txtBatasAtasBaris2.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasAtasBaris3LabelHarga", txtBatasAtasBaris3.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasAtasBaris4LabelHarga", txtBatasAtasBaris4.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasAtasBaris5LabelHarga", txtBatasAtasBaris5.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasAtasBaris6LabelHarga", txtBatasAtasBaris6.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasAtasBaris7LabelHarga", txtBatasAtasBaris7.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasAtasBaris8LabelHarga", txtBatasAtasBaris8.Text, appConfigFile);

            AppConfigHelper.SaveValue("batasKiriKolom1LabelHarga", txtBatasKiriKolom1.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasKiriKolom2LabelHarga", txtBatasKiriKolom2.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasKiriKolom3LabelHarga", txtBatasKiriKolom3.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasKiriKolom4LabelHarga", txtBatasKiriKolom4.Text, appConfigFile);
        }

        /// <summary>
        /// Method untuk mengecek minimal 1 posisi label harga harus dipilih sebelum dicetak
        /// </summary>
        /// <returns></returns>
        private bool IsPilih()
        {
            foreach (var checkbox in _listOfCheckboxPosisiLabel)
            {
                if (checkbox.Checked)
                    return true;
            }

            return false;
        }

        private void txtKodeProduk_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                var keyword = ((AdvancedTextbox)sender).Text;

                IProdukBll produkBll = new ProdukBll(MainProgram.isUseWebAPI, MainProgram.baseUrl, _log);
                this._produk = produkBll.GetByKode(keyword);

                if (this._produk == null)
                {
                    var listOfProduk = produkBll.GetByName(keyword);

                    if (listOfProduk.Count == 0)
                    {
                        MsgHelper.MsgWarning("Data produk tidak ditemukan");
                        txtKodeProduk.Focus();
                        txtKodeProduk.SelectAll();
                        ResetLabelHarga();
                    }
                    else if (listOfProduk.Count == 1)
                    {
                        this._produk = listOfProduk[0];

                        SetDataProduk(this._produk);
                        PreviewLabelHargaProduk();
                    }
                    else // data lebih dari satu, tampilkan form lookup
                    {
                        var frmLookup = new FrmLookupReferensi("Data Produk", listOfProduk);
                        frmLookup.Listener = this;
                        frmLookup.ShowDialog();
                    }
                }
                else
                {
                    SetDataProduk(this._produk);
                    PreviewLabelHargaProduk();
                }
            }
        }        

        public void Ok(object sender, object data)
        {
            if (data is Produk) // pencarian produk baku
            {
                this._produk = (Produk)data;

                SetDataProduk(this._produk);
                PreviewLabelHargaProduk();
            }
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            throw new NotImplementedException();
        }        

        private void btnCetak_Click(object sender, EventArgs e)
        {
            if (!IsPilih())
            {
                MsgHelper.MsgWarning("Minimal satu posisi label harga harus dipilih !");
                return;
            }

            if (MsgHelper.MsgKonfirmasi("Apakah proses pencetakan ingin dilanjutkan ?"))
            {
                SaveAppConfig();

                PrintDocument printLabelHarga = new PrintDocument();
                printLabelHarga.PrinterSettings.PrinterName = cmbPrinter.Text;
                printLabelHarga.PrinterSettings.Copies = (short)updJumlahCetak.Value;
                printLabelHarga.PrintPage += printLabelHarga_PrintPage;
                printLabelHarga.Print();
            }
        }

        private void printLabelHarga_PrintPage(object sender, PrintPageEventArgs e)
        {
            var labelHargaColumns = 4;
            var labelHargaPerPage = 32;

            // Determine printable region for each price label
            var numLines = labelHargaPerPage / labelHargaColumns;

            if ((labelHargaPerPage % labelHargaColumns) != 0)
            {
                ++numLines;
            }

            var labelHargaArea = new SizeF();

            labelHargaArea.Width = (e.MarginBounds.Width / labelHargaColumns);
            labelHargaArea.Height = (e.MarginBounds.Height / numLines);

            var listOfPosition = new Dictionary<int, PointF>();
            
            // baris 1
            listOfPosition.Add(0, new PointF(_pengaturanLabelHarga.batas_kiri_kolom1, _pengaturanLabelHarga.batas_atas_baris1));
            listOfPosition.Add(1, new PointF(_pengaturanLabelHarga.batas_kiri_kolom2, listOfPosition[0].Y));
            listOfPosition.Add(2, new PointF(_pengaturanLabelHarga.batas_kiri_kolom3, listOfPosition[0].Y));
            listOfPosition.Add(3, new PointF(_pengaturanLabelHarga.batas_kiri_kolom4, listOfPosition[0].Y));

            // baris 2
            listOfPosition.Add(4, new PointF(listOfPosition[0].X, _pengaturanLabelHarga.batas_atas_baris2));
            listOfPosition.Add(5, new PointF(listOfPosition[1].X, listOfPosition[4].Y));
            listOfPosition.Add(6, new PointF(listOfPosition[2].X, listOfPosition[4].Y));
            listOfPosition.Add(7, new PointF(listOfPosition[3].X, listOfPosition[4].Y));

            // baris 3
            listOfPosition.Add(8, new PointF(listOfPosition[0].X, _pengaturanLabelHarga.batas_atas_baris3));
            listOfPosition.Add(9, new PointF(listOfPosition[1].X, listOfPosition[8].Y));
            listOfPosition.Add(10, new PointF(listOfPosition[2].X, listOfPosition[8].Y));
            listOfPosition.Add(11, new PointF(listOfPosition[3].X, listOfPosition[8].Y));

            // baris 4
            listOfPosition.Add(12, new PointF(listOfPosition[0].X, _pengaturanLabelHarga.batas_atas_baris4));
            listOfPosition.Add(13, new PointF(listOfPosition[1].X, listOfPosition[12].Y));
            listOfPosition.Add(14, new PointF(listOfPosition[2].X, listOfPosition[12].Y));
            listOfPosition.Add(15, new PointF(listOfPosition[3].X, listOfPosition[12].Y));

            // baris 5
            listOfPosition.Add(16, new PointF(listOfPosition[0].X, _pengaturanLabelHarga.batas_atas_baris5));
            listOfPosition.Add(17, new PointF(listOfPosition[1].X, listOfPosition[16].Y));
            listOfPosition.Add(18, new PointF(listOfPosition[2].X, listOfPosition[16].Y));
            listOfPosition.Add(19, new PointF(listOfPosition[3].X, listOfPosition[16].Y));

            // baris 6
            listOfPosition.Add(20, new PointF(listOfPosition[0].X, _pengaturanLabelHarga.batas_atas_baris6));
            listOfPosition.Add(21, new PointF(listOfPosition[1].X, listOfPosition[20].Y));
            listOfPosition.Add(22, new PointF(listOfPosition[2].X, listOfPosition[20].Y));
            listOfPosition.Add(23, new PointF(listOfPosition[3].X, listOfPosition[20].Y));

            // baris 7
            listOfPosition.Add(24, new PointF(listOfPosition[0].X, _pengaturanLabelHarga.batas_atas_baris7));
            listOfPosition.Add(25, new PointF(listOfPosition[1].X, listOfPosition[24].Y));
            listOfPosition.Add(26, new PointF(listOfPosition[2].X, listOfPosition[24].Y));
            listOfPosition.Add(27, new PointF(listOfPosition[3].X, listOfPosition[24].Y));

            // baris 8
            listOfPosition.Add(28, new PointF(listOfPosition[0].X, _pengaturanLabelHarga.batas_atas_baris8));
            listOfPosition.Add(29, new PointF(listOfPosition[1].X, listOfPosition[28].Y));
            listOfPosition.Add(30, new PointF(listOfPosition[2].X, listOfPosition[28].Y));
            listOfPosition.Add(31, new PointF(listOfPosition[3].X, listOfPosition[28].Y));

            for (var index = 0; index < labelHargaPerPage; index++)
            {
                var isPrint = _listOfCheckboxPosisiLabel[index].Checked;
                
                if (isPrint)
                {
                    var position = listOfPosition[index];

                    /*
                    var drawRectangle = new RectangleF(position, labelHargaArea);
                    var labelHargaImageLocation = new PointF(position.X, position.Y);

                    var labelHarga = _listOfPanelPosisiLabel[index];

                    labelHargaImageLocation.X += (drawRectangle.Width - labelHarga.BackgroundImage.Width) / 2;

                    e.Graphics.DrawImage(labelHarga.BackgroundImage, labelHargaImageLocation);
                     */

                    using (var brush = new SolidBrush(Color.Black))
                    {
                        try
                        {
                            var nLeft = (int)position.X;
                            var nTop = (int)position.Y;

                            var labelHarga = _listOfLabelHargaProduk[index];
                            DrawString(labelHarga, e.Graphics, brush, nLeft, nTop);
                        }
                        catch
                        {
                        } 
                    }

                }
            }
        }

        private void DrawString(LabelHargaProduk label, Graphics g, SolidBrush brush, int nLeft, int nTop)
        {
            var maxLength = 23;
            var font = new Font("Courier New", 9.5f);

            g.DrawString(string.Format("{0}{1}", 
                    StringHelper.CenterAlignment(label.NamaProduk1.Length, maxLength), label.NamaProduk1), font, brush, nLeft, nTop);

            if (label.NamaProduk2.Length > 0)
            {
                nTop += 15;
                g.DrawString(string.Format("{0}{1}", 
                        StringHelper.CenterAlignment(label.NamaProduk2.Length, maxLength), label.NamaProduk2), font, brush, nLeft, nTop);
            }

            nTop += 15;
            g.DrawString(string.Format("{0}{1}", 
                    StringHelper.CenterAlignment(label.Barcode.Length, maxLength), label.Barcode), font, brush, nLeft, nTop);

            nTop += 10;
            g.DrawString(string.Format("{0}{1}", 
                    StringHelper.CenterAlignment(3, maxLength - label.Harga.Length - 5), "Rp."), font, brush, nLeft, nTop + 5);
            g.DrawString(string.Format("{0}{1}", 
                    StringHelper.CenterAlignment(label.Harga.Length, maxLength - 6), label.Harga), new Font("Courier New", 14f, FontStyle.Bold), brush, nLeft, nTop + 2);

            if (label.TanggalUpdate.Length > 0)
            {
                nTop += 20;
                g.DrawString(string.Format("{0}{1}",
                        StringHelper.CenterAlignment(label.TanggalUpdate.Length, maxLength), label.TanggalUpdate), font, brush, nLeft, nTop);
            }            
        }

        private void chkPilihSemua_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var checkbox in _listOfCheckboxPosisiLabel)
            {
                if (checkbox.Enabled) checkbox.Checked = ((CheckBox)sender).Checked;
            }
        }

        private void FrmCetakLabelHargaProduk_KeyPress(object sender, KeyPressEventArgs e)
        {        
            if (KeyPressHelper.IsEsc(e)) this.Close();
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPindahKeDaftarCetak_Click(object sender, EventArgs e)
        {
            var index = 0;

            foreach (var panel in _listOfPanelPosisiLabel)
            {
                if (panel.BackgroundImage == null)
                {
                    panel.BackgroundImage = labelHargaProdukPanel.BackgroundImage;
                    _listOfPanelPosisiLabel2[index].BackgroundImage = labelHargaProdukPanel.BackgroundImage;

                    var arrNamaProduk = StringHelper.SplitByLength(labelHargaProdukPanel.NamaProduk, 23).ToList();

                    var labelHarga = new LabelHargaProduk
                    {
                        NamaProduk1 = arrNamaProduk.Count > 0 ? arrNamaProduk[0] : string.Empty,
                        NamaProduk2 = arrNamaProduk.Count > 1 ? arrNamaProduk[1] : string.Empty,
                        Barcode = labelHargaProdukPanel.KodeProduk,
                        Harga = string.Format("{0:N0}", labelHargaProdukPanel.HargaProduk),
                        TanggalUpdate = labelHargaProdukPanel.LastUpdate != null ? string.Format("{0:dd-MM-yyyy}", labelHargaProdukPanel.LastUpdate) : string.Empty
                    };

                    _listOfLabelHargaProduk.Add(labelHarga);

                    break;
                }          
      
                index++;
            }

            if (index == _listOfCheckboxPosisiLabel.Count) return;

            _listOfCheckboxPosisiLabel[index].Enabled = true;

            chkPilihSemua.Enabled = true;
            updJumlahCetak.Enabled = true;
            btnCetak.Enabled = true;
        }

        private void FrmCetakLabelHargaProduk_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyPressHelper.IsShortcutKey(Keys.F11, e)) btnPindahKeDaftarCetak_Click(sender, new EventArgs());
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetLabelHarga(true);
        }
    }
}
