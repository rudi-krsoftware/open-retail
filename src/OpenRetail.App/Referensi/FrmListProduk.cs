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

namespace OpenRetail.App.Referensi
{
    public partial class FrmListProduk : FrmListEmptyBody, IListener
    {
        private IProdukBll _bll; // deklarasi objek business logic layer 
        private IList<Produk> _listOfProduk = new List<Produk>();
        private IList<Golongan> _listOfGolongan = new List<Golongan>();
        private ILog _log;

        public FrmListProduk(string header, Pengguna pengguna, string menuId)
            : base()
        {
            InitializeComponent();
            this.btnImport.Visible = true;
            this.toolTip1.SetToolTip(this.btnImport, "Import Data Produk");
            this.mnuBukaFileMaster.Text = "Buka File Master Produk";
            this.mnuImportFileMaster.Text = "Import File Master Produk";

            base.SetHeader(header);
            base.WindowState = FormWindowState.Maximized;

            _log = MainProgram.log;
            _bll = new ProdukBll(_log);
            
            // set hak akses untuk SELECT
            var role = pengguna.GetRoleByMenuAndGrant(menuId, GrantState.SELECT);
            if (role != null)
            {
                if (role.is_grant)
                    LoadDataGolongan();                    

                txtNamaProduk.Enabled = role.is_grant;
                btnCari.Enabled = role.is_grant;
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

        private void LoadDataProduk(string golonganId = "")
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                if (golonganId.Length > 0)
                    _listOfProduk = _bll.GetByGolongan(golonganId);
                else
                    _listOfProduk = _bll.GetAll();

                GridListControlHelper.Refresh<Produk>(this.gridList, _listOfProduk);
            }

            ResetButton();
        }

        private void LoadDataProdukByName(string name)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfProduk = _bll.GetByName(name);
                GridListControlHelper.Refresh<Produk>(this.gridList, _listOfProduk);
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
            gridListProperties.Add(new GridListControlProperties { Header = "Nama Produk", Width = 400 });
            gridListProperties.Add(new GridListControlProperties { Header = "Satuan", Width = 130 });
            gridListProperties.Add(new GridListControlProperties { Header = "Harga Beli", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Harga Jual", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Diskon", Width = 50 });
            gridListProperties.Add(new GridListControlProperties { Header = "Stok Etalase", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Stok Gudang", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Min. Stok Gudang" });

            GridListControlHelper.InitializeGridListControl<Produk>(this.gridList, _listOfProduk, gridListProperties);

            if (_listOfProduk.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {

                if (_listOfProduk.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {

                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfProduk.Count)
                        {
                            var produk = _listOfProduk[rowIndex];

                            switch (e.ColIndex)
                            {
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

                                case 7:
                                    e.Style.CellValue = NumberHelper.NumberToString(produk.harga_jual);
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    break;

                                case 8:
                                    e.Style.CellValue = produk.diskon;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 9:
                                    e.Style.CellValue = produk.stok;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 10:
                                    e.Style.CellValue = produk.stok_gudang;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 11:
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

        private void cmbGolongan_SelectedIndexChanged(object sender, EventArgs e)
        {
            var golonganId = string.Empty;

            var index = ((ComboBox)sender).SelectedIndex;

            if (index > 0)
            {
                var golongan = _listOfGolongan[index - 1];
                golonganId = golongan.golongan_id;
            }

            LoadDataProduk(golonganId);
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
            var index = this.gridList.SelectedIndex;

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
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            if (MsgHelper.MsgDelete())
            {
                var produk = _listOfProduk[index];

                var result = _bll.Delete(produk);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<Produk>(this.gridList, _listOfProduk, produk);
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
            var msg = string.Empty;
            var fileMaster = Utils.GetAppPath() + @"\File Import Excel\Master Data\data_produk.xlsx";

            IImportExportDataBll _importDataBll = new ImportExportDataProdukBll(fileMaster, _log);

            if (_importDataBll.IsOpened())
            {
                msg = "Maaf file master Produk sedang dibuka, silahkan ditutup terlebih dulu.";
                MsgHelper.MsgWarning(msg);

                return;
            }

            if (!_importDataBll.IsValidFormat())
            {
                msg = "Maaf format file master Produk tidak valid, proses import tidak bisa dilanjutkan.";
                MsgHelper.MsgWarning(msg);

                return;
            }

            if (MsgHelper.MsgKonfirmasi("Apakah proses ingin dilanjutkan ?"))
            {
                using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                {
                    var rowCount = 0;
                    var result = _importDataBll.Import(ref rowCount);

                    if (result)
                    {
                        msg = "Import data master Produk berhasil.";
                        MsgHelper.MsgInfo(msg);

                        if (cmbGolongan.SelectedIndex == 0)
                            LoadDataProduk();
                        else
                            cmbGolongan.SelectedIndex = 0;
                    }
                    else
                    {
                        if (rowCount == 0)
                        {
                            msg = "Data file master Produk masih kosong.\n" +
                                  "Silahkan diisi terlebih dulu.";
                            MsgHelper.MsgInfo(msg);
                        }
                    }
                }
            } 
        }

        public void Ok(object sender, object data)
        {
            throw new NotImplementedException();
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            var produk = (Produk)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<Produk>(this.gridList, _listOfProduk, produk);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<Produk>(this.gridList, _listOfProduk, produk);
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (btnPerbaiki.Enabled)
                Perbaiki();
        }

        private void txtNamaProduk_Enter(object sender, EventArgs e)
        {
            var text = (AdvancedTextbox)sender;

            text.Clear();
            text.ForeColor = Color.Black;
        }

        private void txtNamaProduk_Leave(object sender, EventArgs e)
        {
            var text = (AdvancedTextbox)sender;

            if (string.IsNullOrEmpty(text.Text))
            {
                text.Text = "Ketik nama produk ...";
                text.ForeColor = Color.Gray;
            }        
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            LoadDataProdukByName(txtNamaProduk.Text);
        }

        private void txtNamaProduk_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                btnCari_Click(sender, e);
        }
    }
}
