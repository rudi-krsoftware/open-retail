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

using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.App.UI.Template;
using OpenRetail.App.Helper;
using Syncfusion.Windows.Forms.Grid;
using ConceptCave.WaitCursor;
using OpenRetail.App.UserControl;
using log4net;
using System.IO;
using System.Diagnostics;
using Syncfusion.Styles;

namespace OpenRetail.App.Referensi
{
    public partial class FrmListProdukWithNavigation : FrmListEmptyBodyWithNavigation, IListener
    {
        private IProdukBll _bll; // deklarasi objek business logic layer 
        private IList<Produk> _listOfProduk = new List<Produk>();
        private IList<Golongan> _listOfGolongan = new List<Golongan>();
        private ILog _log;
        private int _pageNumber = 1;
        private int _pagesCount = 0;
        private int _pageSize = 0;

        public FrmListProdukWithNavigation(string header, Pengguna pengguna, string menuId)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            this.btnImport.Visible = true;
            this.toolTip1.SetToolTip(this.btnImport, "Import/Export Data Produk");
            this.mnuBukaFileMaster.Text = "Buka File Master Produk";
            this.mnuImportFileMaster.Text = "Import File Master Produk";
            this.mnuExportData.Text = "Export Data Produk";

            base.SetHeader(header);
            base.WindowState = FormWindowState.Maximized;

            _pageSize = MainProgram.pageSize;
            _log = MainProgram.log;
            _bll = new ProdukBll(_log);
            
            // set hak akses untuk SELECT
            var role = pengguna.GetRoleByMenuAndGrant(menuId, GrantState.SELECT);
            if (role != null)
            {
                if (role.is_grant)
                {
                    cmbSortBy.SelectedIndex = 1;
                    this.updLimit.Value = _pageSize;

                    LoadDataGolongan();                    
                }

                cmbSortBy.Enabled = role.is_grant;
                txtNamaProduk.Enabled = role.is_grant;
                btnCari.Enabled = role.is_grant;

                btnImport.Enabled = pengguna.is_administrator;
            }

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, pengguna, menuId, _listOfGolongan.Count);
        }

        private void LoadDataGolongan()
        {
            IGolonganBll golonganBll = new GolonganBll(_log);

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfGolongan = golonganBll.GetAll();

                cmbGolongan.Items.Clear();
                cmbGolongan.Items.Add("-- Semua --");
                foreach (var golongan in _listOfGolongan)
                {
                    cmbGolongan.Items.Add(golongan.nama_golongan);
                }

                cmbGolongan.SelectedIndex = 0;
            }
        }

        private void LoadDataProduk(string golonganId = "", int sortIndex = 1)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                if (golonganId.Length > 0)
                    _listOfProduk = _bll.GetByGolongan(golonganId, sortIndex, _pageNumber, _pageSize, ref _pagesCount);
                else
                    _listOfProduk = _bll.GetAll(sortIndex, _pageNumber, _pageSize, ref _pagesCount);

                GridListControlHelper.Refresh<Produk>(this.gridList, _listOfProduk, additionalRowCount: 1);

                base.SetInfoHalaman(_pageNumber, _pagesCount);
                base.SetStateBtnNavigation(_pageNumber, _pagesCount);

                if (!(_listOfProduk.Count > 0))
                {
                    base.SetInfoHalaman(0, 0);
                    base.SetStateBtnNavigation(0, 0); // non aktifkan button navigasi
                }
            }

            ResetButton();
        }

        private void LoadDataProdukByName(string name, int sortIndex = 1)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfProduk = _bll.GetByName(name, sortIndex, _pageNumber, _pageSize, ref _pagesCount);
                GridListControlHelper.Refresh<Produk>(this.gridList, _listOfProduk, additionalRowCount: 1);
                
                base.SetInfoHalaman(_pageNumber, _pagesCount);
                base.SetStateBtnNavigation(_pageNumber, _pagesCount);

                if (!(_listOfProduk.Count > 0))
                {
                    base.SetInfoHalaman(0, 0);
                    base.SetStateBtnNavigation(0, 0); // non aktifkan button navigasi
                }
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfProduk.Count > 0);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Golongan", Width = 130 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kode Produk", Width = 130 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama Produk", Width = 350 });
            gridListProperties.Add(new GridListControlProperties { Header = "Satuan", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Harga Beli", Width = 70 });
            
            gridListProperties.Add(new GridListControlProperties { Header = "Harga Jual", Width = 70 });
            gridListProperties.Add(new GridListControlProperties { Header = "Harga Jual", Width = 70 });
            gridListProperties.Add(new GridListControlProperties { Header = "Harga Jual", Width = 70 });
            gridListProperties.Add(new GridListControlProperties { Header = "Harga Jual", Width = 70 });

            gridListProperties.Add(new GridListControlProperties { Header = "Diskon", Width = 50 });
            gridListProperties.Add(new GridListControlProperties { Header = "Stok Etalase", Width = 60 });
            gridListProperties.Add(new GridListControlProperties { Header = "Stok Gudang", Width = 60 });
            gridListProperties.Add(new GridListControlProperties { Header = "Min. Stok Gudang" });

            GridListControlHelper.InitializeGridListControl<Produk>(this.gridList, _listOfProduk, gridListProperties, false, additionalRowCount: 1);
            this.gridList.Grid.Model.RowHeights[1] = 25;
            this.gridList.Grid.Model.Rows.FrozenCount = 1;

            this.gridList.Grid.PrepareViewStyleInfo += delegate(object sender, GridPrepareViewStyleInfoEventArgs e)
            {
                var subHeaderHargaJual = new string[] { "Retail", "Grosir 1", "Grosir 2", "Grosir 3" };
                if (e.ColIndex > 6 && e.RowIndex == 1)
                {
                    var colIndex = 7;

                    foreach (var header in subHeaderHargaJual)
                    {
                        if (colIndex == e.ColIndex)
                            e.Style.Text = header;

                        colIndex++;
                    }
                }
            };

            if (_listOfProduk.Count > 0)
                this.gridList.SetSelected(1, true);

            // merge cell
            var column = 1; // kolom no
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 2; // kolom golongan
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 3; // kolom kode
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 4; // kolom nama produk
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 5; // kolom satuan
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 6; // kolom harga beli
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 7; // kolom harga jual
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 0, column + 3));

            column = 11; // kolom diskon
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 12; // kolom stok etalase
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 13; // kolom stok gudang
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 14; // kolom minimal stok
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));
            
            var headerStyle = this.gridList.Grid.BaseStylesMap["Column Header"].StyleInfo;
            headerStyle.CellType = GridCellTypeName.Header;

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                if (e.RowIndex == 1)
                {
                    if (e.ColIndex > 6)
                    {
                        e.Style.ModifyStyle(headerStyle, StyleModifyType.ApplyNew);
                    }

                    // we handled it, let the grid know
                    e.Handled = true;
                }

                if (_listOfProduk.Count > 0)
                {                    
                    if (e.RowIndex > 1)
                    {

                        var rowIndex = e.RowIndex - 2;

                        if (rowIndex < _listOfProduk.Count)
                        {
                            var produk = _listOfProduk[rowIndex];
                            var listOfHargaGrosir = produk.list_of_harga_grosir;
                            var hargaGrosir = 0d;

                            switch (e.ColIndex)
                            {
                                case 1:
                                    var noUrut = (_pageNumber - 1) * _pageSize + e.RowIndex - 1;
                                    e.Style.CellValue = noUrut;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 2:
                                    if (produk.Golongan != null)
                                        e.Style.CellValue = produk.Golongan.nama_golongan;

                                    break;

                                case 3:
                                    e.Style.CellValue = produk.kode_produk;
                                    break;

                                case 4:
                                    e.Style.CellValue = produk.nama_produk;
                                    break;

                                case 5:
                                    var satuan = string.Empty;

                                    if (produk.satuan.Length > 0)
                                        satuan = produk.satuan;

                                    e.Style.CellValue = satuan;
                                    break;

                                case 6:
                                    e.Style.CellValue = NumberHelper.NumberToString(produk.harga_beli);
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    break;

                                case 7: // harga jual ritel
                                    e.Style.CellValue = NumberHelper.NumberToString(produk.harga_jual);
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    break;

                                case 8: // harga grosir 1
                                    hargaGrosir = listOfHargaGrosir.Count > 0 ? listOfHargaGrosir[0].harga_grosir : 0;

                                    e.Style.CellValue = NumberHelper.NumberToString(hargaGrosir);
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    break;

                                case 9: // harga grosir 2
                                    hargaGrosir = listOfHargaGrosir.Count > 1 ? listOfHargaGrosir[1].harga_grosir : 0;

                                    e.Style.CellValue = NumberHelper.NumberToString(hargaGrosir);
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    break;

                                case 10: // harga grosir 3
                                    hargaGrosir = listOfHargaGrosir.Count > 2 ? listOfHargaGrosir[2].harga_grosir : 0;

                                    e.Style.CellValue = NumberHelper.NumberToString(hargaGrosir);
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    break;

                                case 11:
                                    e.Style.CellValue = produk.diskon;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 12:
                                    e.Style.CellValue = produk.stok;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 13:
                                    e.Style.CellValue = produk.stok_gudang;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 14:
                                    e.Style.CellValue = produk.minimal_stok_gudang;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                default:
                                    break;
                            }

                            // we handled it, let the grid know
                            e.Handled = true;
                        }
                    }
                }
            };
        }

        public void Ok(object sender, object data)
        {
            if (sender is FrmImportDataProduk)
            {
                // refresh data setelah import dari file excel
                if (cmbGolongan.SelectedIndex == 0)
                    LoadDataProduk();
                else
                    cmbGolongan.SelectedIndex = 0;
            }
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            var produk = (Produk)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<Produk>(this.gridList, _listOfProduk, produk, additionalRowCount: 1);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<Produk>(this.gridList, _listOfProduk, produk, additionalRowCount: 1);
        }

        private void txtNamaProduk_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                btnCari_Click(sender, e);
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            _pageNumber = 1;
            LoadDataProdukByName(txtNamaProduk.Text, cmbSortBy.SelectedIndex);
        }

        private void cmbGolongan_SelectedIndexChanged(object sender, EventArgs e)
        {
            var golonganId = string.Empty;

            var index = ((ComboBox)sender).SelectedIndex;

            if (index > 0)
            {
                var golongan = _listOfGolongan[index - 1];
                golonganId = golongan.golongan_id;
            }

            _pageNumber = 1;
            LoadDataProduk(golonganId, cmbSortBy.SelectedIndex);
        }

        protected override void Tambah()
        {
            if (cmbGolongan.SelectedIndex == 0)
            {
                var msg = "Maaf data 'Golongan' belum dipilih.";
                MsgHelper.MsgWarning(msg);

                return;
            }

            var golongan = _listOfGolongan[cmbGolongan.SelectedIndex - 1];

            var frm = new FrmEntryProduk("Tambah Data " + this.Text, golongan, _listOfGolongan, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex - 1;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var produk = _listOfProduk[index];
            produk.kode_produk_old = produk.kode_produk;

            var frm = new FrmEntryProduk("Edit Data " + this.Text, produk, _listOfGolongan, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Hapus()
        {
            var index = this.gridList.SelectedIndex - 1;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            if (MsgHelper.MsgDelete())
            {
                var produk = _listOfProduk[index];

                var result = _bll.Delete(produk);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<Produk>(this.gridList, _listOfProduk, produk, additionalRowCount: 1);
                    ResetButton();
                }
                else
                    MsgHelper.MsgDeleteError();
            }
        }

        protected override void OpenFileMaster()
        {
            var msg = "Untuk membuka file master Produk membutuhkan Ms Excel versi 2007 atau yang terbaru.\n\n" +
                      "Apakah proses ingin dilanjutkan ?";

            if (MsgHelper.MsgKonfirmasi(msg))
            {
                var fileMaster = Utils.GetAppPath() + @"\File Import Excel\Master Data\data_produk.xlsx";

                if (!File.Exists(fileMaster))
                {
                    MsgHelper.MsgWarning("Maaf file master Produk tidak ditemukan.");
                    return;
                }

                try
                {
                    Process.Start(fileMaster);
                }
                catch
                {
                    msg = "Gagal membuka file master Produk !!!.\n\n" +
                          "Cek apakah Ms Excel versi 2007 atau yang terbaru sudah terinstall ?";

                    MsgHelper.MsgError(msg);
                }
            }
        }

        protected override void ImportData()
        {
            var frm = new FrmImportDataProduk("Import Data Produk dari File Excel");
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void ExportData()
        {            
            using (var dlgSave = new SaveFileDialog())
            {
                dlgSave.Filter = "Microsoft Excel files (*.xlsx)|*.xlsx";
                dlgSave.Title = "Export Data Produk";

                var result = dlgSave.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                    {
                        var listOfProduk = _bll.GetAll(cmbSortBy.SelectedIndex);
                        
                        IImportExportDataBll<Produk> _importDataBll = new ImportExportDataProdukBll(dlgSave.FileName, _log);
                        _importDataBll.Export(listOfProduk);
                    }                    
                }
            }                   
        }

        private void RefreshData()
        {
            if (txtNamaProduk.Text.Length > 0)
                LoadDataProdukByName(txtNamaProduk.Text);
            else
            {
                var golonganId = string.Empty;

                var index = cmbGolongan.SelectedIndex;

                if (index > 0)
                {
                    var golongan = _listOfGolongan[index - 1];
                    golonganId = golongan.golongan_id;
                }

                LoadDataProduk(golonganId);
            }
        }

        protected override void MoveFirst()
        {
            _pageNumber = 1;

            RefreshData();
        }        

        protected override void MovePrevious()
        {
            _pageNumber--;

            RefreshData();
        }

        protected override void MoveNext()
        {
            _pageNumber++;

            RefreshData();
        }

        protected override void MoveLast()
        {
            _pageNumber = _pagesCount;

            RefreshData();
        }

        protected override void LimitRowChanged()
        {
            MainProgram.pageSize = (int)this.updLimit.Value;
            _pageSize = MainProgram.pageSize;

            cmbGolongan_SelectedIndexChanged(cmbGolongan, new EventArgs());
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (btnPerbaiki.Enabled)
                Perbaiki();
        }

        private void cmbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            var golonganId = string.Empty;

            var index = cmbGolongan.SelectedIndex;

            if (index > 0)
            {
                var golongan = _listOfGolongan[index - 1];
                golonganId = golongan.golongan_id;
            }

            _pageNumber = 1;
            LoadDataProduk(golonganId, cmbSortBy.SelectedIndex);
        }
        
    }
}
