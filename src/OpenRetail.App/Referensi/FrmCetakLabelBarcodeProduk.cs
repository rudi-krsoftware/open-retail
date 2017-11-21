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
using Zen.Barcode;
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Helper;
using OpenRetail.Bll.Service;
using OpenRetail.Helper.UserControl;
using OpenRetail.App.Lookup;

namespace OpenRetail.App.Referensi
{
    public partial class FrmCetakLabelBarcodeProduk : Form, IListener
    {
        private Produk _produk = null;
        private PengaturanBarcode _pengaturanBarcode = null;
        private IList<CheckBox> _listOfCheckboxPosisiLabel = new List<CheckBox>();
        private IList<Panel> _listOfPanelPosisiLabel = new List<Panel>();
        private ILog _log;

        public FrmCetakLabelBarcodeProduk(string header)
        {
            InitializeComponent();            
            ColorManagerHelper.SetTheme(this, this);

            this.Text = header;
            this.lblHeader.Text = header;
            this._log = MainProgram.log;
            this._pengaturanBarcode = MainProgram.pengaturanBarcode;

            InitializeList();
            LoadPengaturanBarcode();
        }

        private void LoadPengaturanBarcode()
        {
            txtHeaderBarcode.Text = _pengaturanBarcode.header_label;
            LoadPrinter(_pengaturanBarcode.nama_printer);

            txtBatasAtasBaris1.Text = _pengaturanBarcode.batas_atas_baris1.ToString();
            txtBatasAtasBaris2.Text = _pengaturanBarcode.batas_atas_baris2.ToString();
            txtBatasAtasBaris3.Text = _pengaturanBarcode.batas_atas_baris3.ToString();
            txtBatasAtasBaris4.Text = _pengaturanBarcode.batas_atas_baris4.ToString();

            txtBatasKiriKolom1.Text = _pengaturanBarcode.batas_kiri_kolom1.ToString();
            txtBatasKiriKolom2.Text = _pengaturanBarcode.batas_kiri_kolom2.ToString();
            txtBatasKiriKolom3.Text = _pengaturanBarcode.batas_kiri_kolom3.ToString();
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

        private void PreviewBarcode()
        {
            if (txtKodeProduk.Text.Length > 0 && _produk != null)
            {
                barcodePanel.Symbology = BarcodeSymbology.Code128;
                barcodePanel.MaxBarHeight = 50;

                barcodePanel.HeaderLabel = txtHeaderBarcode.Text;
                barcodePanel.Text = txtKodeProduk.Text;
                barcodePanel.PriceLabel = NumberHelper.StringToNumber(txtHargaJual.Text);
                barcodePanel.IsDisplayPriceLabel = chkCetakHargaJual.Checked;

                foreach (var panel in _listOfPanelPosisiLabel)
                {
                    panel.BackgroundImage = barcodePanel.BackgroundImage;
                }

                foreach (var checkbox in _listOfCheckboxPosisiLabel)
                {
                    checkbox.Enabled = true;
                }

                chkPilihSemua.Enabled = true;
                updJumlahCetak.Enabled = true;
                btnCetak.Enabled = true;
            }
        }

        private void ResetBarcode()
        {
            barcodePanel.Text = "";
            barcodePanel.BackgroundImage = null;

            foreach (var panel in _listOfPanelPosisiLabel)
            {
                panel.BackgroundImage = barcodePanel.BackgroundImage;
            }

            foreach (var checkbox in _listOfCheckboxPosisiLabel)
            {
                checkbox.Enabled = false;
            }

            txtNamaProduk.Clear();
            txtHargaJual.Text = "0";

            updJumlahCetak.Enabled = false;
            btnCetak.Enabled = false;
            chkPilihSemua.Enabled = false;
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

            this._pengaturanBarcode.header_label = txtHeaderBarcode.Text;
            this._pengaturanBarcode.nama_printer = cmbPrinter.Text;
            this._pengaturanBarcode.batas_atas_baris1 = Convert.ToSingle(txtBatasAtasBaris1.Text);
            this._pengaturanBarcode.batas_atas_baris2 = Convert.ToSingle(txtBatasAtasBaris2.Text);
            this._pengaturanBarcode.batas_atas_baris3 = Convert.ToSingle(txtBatasAtasBaris3.Text);
            this._pengaturanBarcode.batas_atas_baris4 = Convert.ToSingle(txtBatasAtasBaris4.Text);

            this._pengaturanBarcode.batas_kiri_kolom1 = Convert.ToSingle(txtBatasKiriKolom1.Text);
            this._pengaturanBarcode.batas_kiri_kolom2 = Convert.ToSingle(txtBatasKiriKolom2.Text);
            this._pengaturanBarcode.batas_kiri_kolom3 = Convert.ToSingle(txtBatasKiriKolom3.Text);

            AppConfigHelper.SaveValue("headerLabel", txtHeaderBarcode.Text, appConfigFile);
            AppConfigHelper.SaveValue("printerBarcode", cmbPrinter.Text, appConfigFile);

            AppConfigHelper.SaveValue("batasAtasBaris1", txtBatasAtasBaris1.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasAtasBaris2", txtBatasAtasBaris2.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasAtasBaris3", txtBatasAtasBaris3.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasAtasBaris4", txtBatasAtasBaris4.Text, appConfigFile);

            AppConfigHelper.SaveValue("batasKiriKolom1", txtBatasKiriKolom1.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasKiriKolom2", txtBatasKiriKolom2.Text, appConfigFile);
            AppConfigHelper.SaveValue("batasKiriKolom3", txtBatasKiriKolom3.Text, appConfigFile);
        }

        /// <summary>
        /// Method untuk mengecek minimal 1 posisi label barcode harus dipilih sebelum dicetak
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

                IProdukBll produkBll = new ProdukBll(_log);
                this._produk = produkBll.GetByKode(keyword);

                if (this._produk == null)
                {
                    var listOfProduk = produkBll.GetByName(keyword);

                    if (listOfProduk.Count == 0)
                    {
                        MsgHelper.MsgWarning("Data produk tidak ditemukan");
                        txtKodeProduk.Focus();
                        txtKodeProduk.SelectAll();

                        ResetBarcode();
                    }
                    else if (listOfProduk.Count == 1)
                    {
                        this._produk = listOfProduk[0];

                        SetDataProduk(this._produk);
                        PreviewBarcode();
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
                    PreviewBarcode();
                }
            }
        }        

        private void chkCetakHargaJual_CheckedChanged(object sender, EventArgs e)
        {
            PreviewBarcode();
        }

        private void txtHeaderBarcode_TextChanged(object sender, EventArgs e)
        {
            PreviewBarcode();
        }        

        public void Ok(object sender, object data)
        {
            if (data is Produk) // pencarian produk baku
            {
                this._produk = (Produk)data;

                SetDataProduk(this._produk);
                PreviewBarcode();
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
                MsgHelper.MsgWarning("Minimal satu posisi label barcode harus dipilih !");
                return;
            }

            if (MsgHelper.MsgKonfirmasi("Apakah proses pencetakan ingin dilanjutkan ?"))
            {
                SaveAppConfig();

                PrintDocument printBarcode = new PrintDocument();
                printBarcode.PrinterSettings.PrinterName = cmbPrinter.Text;
                printBarcode.PrinterSettings.Copies = (short)updJumlahCetak.Value;
                printBarcode.PrintPage += printBarcode_PrintPage;
                printBarcode.Print();
            }
        }

        private void printBarcode_PrintPage(object sender, PrintPageEventArgs e)
        {
            var barcodeColumns = 3;
            var barcodesPerPage = 12;

            // Determine printable region for each barcode and label
            var numLines = barcodesPerPage / barcodeColumns;

            if ((barcodesPerPage % barcodeColumns) != 0)
            {
                ++numLines;
            }

            var barcodeArea = new SizeF();

            barcodeArea.Width = (e.MarginBounds.Width / barcodeColumns);
            barcodeArea.Height = (e.MarginBounds.Height / numLines);

            var listOfPosition = new Dictionary<int, PointF>();
            
            // baris 1
            listOfPosition.Add(0, new PointF(_pengaturanBarcode.batas_kiri_kolom1, _pengaturanBarcode.batas_atas_baris1));
            listOfPosition.Add(1, new PointF(_pengaturanBarcode.batas_kiri_kolom2, listOfPosition[0].Y));
            listOfPosition.Add(2, new PointF(_pengaturanBarcode.batas_kiri_kolom3, listOfPosition[0].Y));

            // baris 2
            listOfPosition.Add(3, new PointF(listOfPosition[0].X, _pengaturanBarcode.batas_atas_baris2));
            listOfPosition.Add(4, new PointF(listOfPosition[1].X, listOfPosition[3].Y));
            listOfPosition.Add(5, new PointF(listOfPosition[2].X, listOfPosition[3].Y));

            // baris 3
            listOfPosition.Add(6, new PointF(listOfPosition[0].X, _pengaturanBarcode.batas_atas_baris3));
            listOfPosition.Add(7, new PointF(listOfPosition[1].X, listOfPosition[6].Y));
            listOfPosition.Add(8, new PointF(listOfPosition[2].X, listOfPosition[6].Y));

            // baris 4
            listOfPosition.Add(9, new PointF(listOfPosition[0].X, _pengaturanBarcode.batas_atas_baris4));
            listOfPosition.Add(10, new PointF(listOfPosition[1].X, listOfPosition[9].Y));
            listOfPosition.Add(11, new PointF(listOfPosition[2].X, listOfPosition[9].Y));

            for (var index = 0; index < barcodesPerPage; index++)
            {
                var isPrint = _listOfCheckboxPosisiLabel[index].Checked;
                
                if (isPrint)
                {
                    var position = listOfPosition[index];

                    var drawRectangle = new RectangleF(position, barcodeArea);
                    var barcodeImageLocation = new PointF(position.X, position.Y);
                    
                    barcodeImageLocation.X += (drawRectangle.Width - barcodePanel.BackgroundImage.Width) / 2;

                    e.Graphics.DrawImage(barcodePanel.BackgroundImage, barcodeImageLocation);
                }
            }
        }

        private void chkPilihSemua_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var checkbox in _listOfCheckboxPosisiLabel)
            {
                checkbox.Checked = ((CheckBox)sender).Checked;
            }
        }

        private void FrmCetakLabelBarcodeProduk_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEsc(e))
                this.Close();
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }        
    }
}
