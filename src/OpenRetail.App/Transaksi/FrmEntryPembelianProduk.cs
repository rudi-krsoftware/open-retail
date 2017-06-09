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

using OpenRetail.App.Helper;
using OpenRetail.App.UI.Template;
using OpenRetail.App.Lookup;
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using Syncfusion.Windows.Forms.Grid;
using OpenRetail.App.UserControl;
using OpenRetail.App.Referensi;
using ConceptCave.WaitCursor;
using log4net;
using Microsoft.Reporting.WinForms;

namespace OpenRetail.App.Transaksi
{
    public partial class FrmEntryPembelianProduk : FrmEntryStandard, IListener
    {
        private IBeliProdukBll _bll = null;
        private BeliProduk _beli = null;
        private Supplier _supplier = null;
        private IList<ItemBeliProduk> _listOfItemBeli = new List<ItemBeliProduk>();
        private IList<ItemBeliProduk> _listOfItemBeliOld = new List<ItemBeliProduk>();
        private IList<ItemBeliProduk> _listOfItemBeliDeleted = new List<ItemBeliProduk>();

        private int _rowIndex = 0;
        private int _colIndex = 0;
        private bool _isValidKodeProduk = false;

        private bool _isNewData = false;
        private ILog _log;
        private Pengguna _pengguna;
        private Profil _profil;
        private PengaturanUmum _pengaturanUmum;

        public IListener Listener { private get; set; }

        public FrmEntryPembelianProduk(string header, IBeliProdukBll bll) 
            : base()
        {            
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._bll = bll;
            this._isNewData = true;
            this._log = MainProgram.log;
            this._pengguna = MainProgram.pengguna;
            this._profil = MainProgram.profil;
            this._pengaturanUmum = MainProgram.pengaturanUmum;

            txtNota.Text = bll.GetLastNota();
            dtpTanggal.Value = DateTime.Today;
            dtpTanggalTempo.Value = dtpTanggal.Value;
            chkCetakNotaBeli.Checked = this._pengaturanUmum.is_auto_print;

            _listOfItemBeli.Add(new ItemBeliProduk()); // add dummy objek

            InitGridControl(gridControl);
        }

        public FrmEntryPembelianProduk(string header, BeliProduk beli, IBeliProdukBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._beli = beli;
            this._supplier = beli.Supplier;
            this._log = MainProgram.log;
            this._pengguna = MainProgram.pengguna;
            this._profil = MainProgram.profil;
            this._pengaturanUmum = MainProgram.pengaturanUmum;

            txtNota.Text = this._beli.nota;
            dtpTanggal.Value = (DateTime)this._beli.tanggal;
            dtpTanggalTempo.Value = dtpTanggal.Value;
            chkCetakNotaBeli.Checked = this._pengaturanUmum.is_auto_print;

            if (!this._beli.tanggal_tempo.IsNull())
            {
                rdoKredit.Checked = true;
                dtpTanggalTempo.Value = (DateTime)this._beli.tanggal_tempo;
            }

            txtSupplier.Text = this._supplier.nama_supplier;
            txtKeterangan.Text = this._beli.keterangan;

            txtDiskon.Text = this._beli.diskon.ToString();
            txtPPN.Text = this._beli.ppn.ToString();

            // simpan data lama
            _listOfItemBeliOld.Clear();
            foreach (var item in this._beli.item_beli)
            {
                _listOfItemBeliOld.Add(new ItemBeliProduk
                {
                    item_beli_produk_id = item.item_beli_produk_id,
                    jumlah = item.jumlah,
                    harga = item.harga
                });
            }

            _listOfItemBeli = this._beli.item_beli;
            _listOfItemBeli.Add(new ItemBeliProduk()); // add dummy objek

            InitGridControl(gridControl);

            RefreshTotal();
        }

        private void InitGridControl(GridControl grid)
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kode Produk", Width = 120 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama Produk", Width = 320 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jumlah", Width = 50 });
            gridListProperties.Add(new GridListControlProperties { Header = "Diskon", Width = 50 });
            gridListProperties.Add(new GridListControlProperties { Header = "Harga", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sub Total", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Aksi" });

            GridListControlHelper.InitializeGridListControl<ItemBeliProduk>(grid, _listOfItemBeli, gridListProperties);

            grid.PushButtonClick += delegate(object sender, GridCellPushButtonClickEventArgs e)
            {
                if (e.ColIndex == 8)
                {
                    if (grid.RowCount == 1)
                    {
                        MsgHelper.MsgWarning("Minimal 1 item produk harus diinputkan !");
                        return;
                    }

                    if (MsgHelper.MsgDelete())
                    {
                        var itemBeli = _listOfItemBeli[e.RowIndex - 1];
                        itemBeli.entity_state = EntityState.Deleted;

                        _listOfItemBeliDeleted.Add(itemBeli);
                        _listOfItemBeli.Remove(itemBeli);

                        grid.RowCount = _listOfItemBeli.Count();
                        grid.Refresh();

                        RefreshTotal();
                    }                    
                }
            };

            grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                // Make sure the cell falls inside the grid
                if (e.RowIndex > 0)
                {
                    if (!(_listOfItemBeli.Count > 0))
                        return;

                    var itemBeli = _listOfItemBeli[e.RowIndex - 1];
                    var produk = itemBeli.Produk;

                    if (e.RowIndex % 2 == 0)
                        e.Style.BackColor = ColorCollection.BACK_COLOR_ALTERNATE;

                    double harga = 0;
                    double jumlah = 0;

                    var isRetur = itemBeli.jumlah_retur > 0;
                    if (isRetur)
                    {
                        e.Style.BackColor = Color.Red;
                        e.Style.Enabled = false;
                    }

                    switch (e.ColIndex)
                    {
                        case 1: // no urut
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.Enabled = false;
                            e.Style.CellValue = e.RowIndex.ToString();
                            break;

                        case 2:
                            if (produk != null)
                                e.Style.CellValue = produk.kode_produk;

                            break;

                        case 3: // nama produk
                            if (produk != null)
                                e.Style.CellValue = produk.nama_produk;

                            break;

                        case 4: // jumlah
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.CellValue = itemBeli.jumlah - itemBeli.jumlah_retur;

                            break;

                        case 5: // diskon
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.CellValue = itemBeli.diskon;

                            break;

                        case 6: // harga
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;

                            harga = itemBeli.harga;

                            if (!(harga > 0))
                            {
                                if (produk != null)
                                    harga = produk.harga_beli;
                            }

                            e.Style.CellValue = NumberHelper.NumberToString(harga);

                            break;

                        case 7: // subtotal
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                            e.Style.Enabled = false;

                            jumlah = itemBeli.jumlah - itemBeli.jumlah_retur;
                            harga = itemBeli.harga_setelah_diskon;

                            if (!(harga > 0))
                            {
                                if (produk != null)
                                    harga = produk.harga_beli;
                            }

                            e.Style.CellValue = NumberHelper.NumberToString(jumlah * harga);
                            break;

                        case 8: // button hapus
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.CellType = GridCellTypeName.PushButton;
                            e.Style.Description = "Hapus";
                            break;

                        default:
                            break;
                    }

                    e.Handled = true; // we handled it, let the grid know
                }
            };

            var colIndex = 2; // kolom nama produk
            grid.CurrentCell.MoveTo(1, colIndex, GridSetCurrentCellOptions.BeginEndUpdate);
        }

        private double SumGrid(IList<ItemBeliProduk> listOfItemBeli)
        {
            double total = 0;
            foreach (var item in listOfItemBeli.Where(f => f.Produk != null))
            {
                double harga = 0;

                if (item.harga > 0)
                    harga = item.harga_setelah_diskon;
                else
                {
                    if (item.Produk != null)
                        harga = item.Produk.harga_beli;
                }

                total += harga * (item.jumlah - item.jumlah_retur);
            }

            if (total > 0)
            {
                total -= NumberHelper.StringToDouble(txtDiskon.Text);
                total += NumberHelper.StringToDouble(txtPPN.Text);
            }

            return total;
        }

        private void RefreshTotal()
        {
            lblTotal.Text = NumberHelper.NumberToString(SumGrid(_listOfItemBeli));
        }

        protected override void Simpan()
        {
            if (this._supplier == null || txtSupplier.Text.Length == 0)
            {
                MsgHelper.MsgWarning("'Supplier' tidak boleh kosong !");
                txtSupplier.Focus();

                return;
            }

            var total = SumGrid(this._listOfItemBeli);
            if (!(total > 0))
            {
                MsgHelper.MsgWarning("Anda belum melengkapi inputan data produk !");
                return;
            }

            if (rdoKredit.Checked)
            {
                if (!DateTimeHelper.IsValidRangeTanggal(dtpTanggal.Value, dtpTanggalTempo.Value))
                {
                    MsgHelper.MsgNotValidRangeTanggal();
                    return;
                }
            }

            if (!MsgHelper.MsgKonfirmasi("Apakah proses ingin dilanjutkan ?"))
                return;

            if (_isNewData)
                _beli = new BeliProduk();

            _beli.pengguna_id = this._pengguna.pengguna_id;
            _beli.Pengguna = this._pengguna;
            _beli.supplier_id = this._supplier.supplier_id;
            _beli.Supplier = this._supplier;
            _beli.nota = txtNota.Text;
            _beli.tanggal = dtpTanggal.Value;
            _beli.tanggal_tempo = DateTimeHelper.GetNullDateTime();
            _beli.is_tunai = rdoTunai.Checked;

            if (rdoKredit.Checked) // pembelian kredit
            {
                _beli.tanggal_tempo = dtpTanggalTempo.Value;
            }

            _beli.ppn = NumberHelper.StringToDouble(txtPPN.Text);
            _beli.diskon = NumberHelper.StringToDouble(txtDiskon.Text);
            _beli.keterangan = txtKeterangan.Text;

            _beli.item_beli = this._listOfItemBeli.Where(f => f.Produk != null).ToList();
            foreach (var item in _beli.item_beli)
            {
                if (!(item.harga > 0))
                    item.harga = item.Produk.harga_beli;
            }

            if (!_isNewData) // update
                _beli.item_beli_deleted = _listOfItemBeliDeleted;

            var result = 0;
            var validationError = new ValidationError();

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                if (_isNewData)
                {
                    result = _bll.Save(_beli, ref validationError);
                }
                else
                {
                    result = _bll.Update(_beli, ref validationError);
                }

                if (result > 0)
                {
                    try
                    {
                        if (chkCetakNotaBeli.Checked)
                            CetakNota(_beli.beli_produk_id);
                    }
                    catch
                    {                        
                    }

                    Listener.Ok(this, _isNewData, _beli);

                    _supplier = null;
                    _listOfItemBeli.Clear();
                    _listOfItemBeliDeleted.Clear();                                        

                    this.Close();
                }
                else
                {
                    if (validationError.Message.Length > 0)
                    {
                        MsgHelper.MsgWarning(validationError.Message);
                        base.SetFocusObject(validationError.PropertyName, this);
                    }
                    else
                        MsgHelper.MsgUpdateError();
                }
            }            
        }

        private void CetakNota(string beliProdukId)
        {
            ICetakNotaBll cetakBll = new CetakNotaBll(_log);
            var listOfItemNota = cetakBll.GetNotaPembelian(beliProdukId);

            if (listOfItemNota.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "NotaPembelian",
                    Value = listOfItemNota
                };

                // set header nota
                var parameters = new List<ReportParameter>();
                var index = 1;

                foreach (var item in _pengaturanUmum.list_of_header_nota)
                {
                    var paramName = string.Format("header{0}", index);
                    parameters.Add(new ReportParameter(paramName, item.keterangan));

                    index++;
                }

                // set footer nota
                var dt = DateTime.Now;
                var kotaAndTanggal = string.Format("{0}, {1}", _profil.kota, dt.Day + " " + DayMonthHelper.GetBulanIndonesia(dt.Month) + " " + dt.Year);

                parameters.Add(new ReportParameter("kota", kotaAndTanggal));
                parameters.Add(new ReportParameter("footer", _pengguna.nama_pengguna));

                var printReport = new ReportViewerPrintHelper("RvNotaPembelianProduk", reportDataSource, parameters, _pengaturanUmum.nama_printer);
                printReport.Print();
            }
        }

        protected override void Selesai()
        {
            // restore data lama
            if (!_isNewData)
            {
                // restore item yang di edit
                var itemsModified = _beli.item_beli.Where(f => f.Produk != null && f.entity_state == EntityState.Modified)
                                                   .ToArray();

                foreach (var item in itemsModified)
                {
                    var itemBeli = _listOfItemBeliOld.Where(f => f.item_beli_produk_id == item.item_beli_produk_id)
                                                     .SingleOrDefault();

                    if (itemBeli != null)
                    {
                        item.jumlah = itemBeli.jumlah;
                        item.harga = itemBeli.harga;
                    }
                }

                // restore item yang di delete
                var itemsDeleted = _listOfItemBeliDeleted.Where(f => f.Produk != null && f.entity_state == EntityState.Deleted)
                                                         .ToArray();
                foreach (var item in itemsDeleted)
                {
                    item.entity_state = EntityState.Unchanged;
                    this._beli.item_beli.Add(item);
                }

                _listOfItemBeliDeleted.Clear();
            }

            base.Selesai();
        }

        public void Ok(object sender, object data)
        {
            if (data is Produk) // pencarian produk baku
            {
                var produk = (Produk)data;

                SetItemProduk(this.gridControl, _rowIndex, _colIndex + 1, produk);
                this.gridControl.Refresh();
                RefreshTotal();

                GridListControlHelper.SetCurrentCell(this.gridControl, _rowIndex, _colIndex + 1);
            }
            else if (data is Supplier) // pencarian supplier
            {
                this._supplier = (Supplier)data;
                txtSupplier.Text = this._supplier.nama_supplier;
                KeyPressHelper.NextFocus();
            }
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            // do nothing
        }

        private void rdoTunai_CheckedChanged(object sender, EventArgs e)
        {
            dtpTanggalTempo.Enabled = false;
        }

        private void rdoKredit_CheckedChanged(object sender, EventArgs e)
        {
            dtpTanggalTempo.Enabled = true;
        }

        private void txtKeterangan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                gridControl.Focus();
                GridListControlHelper.SetCurrentCell(gridControl, 1, 2); // fokus ke kolom nama produk
            }
        }

        private void txtSupplier_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                var supplierName = ((AdvancedTextbox)sender).Text;

                ISupplierBll bll = new SupplierBll(_log);
                var listOfSupplier = bll.GetByName(supplierName);

                if (listOfSupplier.Count == 0)
                {
                    MsgHelper.MsgWarning("Data supplier tidak ditemukan");
                    txtSupplier.Focus();
                    txtSupplier.SelectAll();

                }
                else if (listOfSupplier.Count == 1)
                {
                    _supplier = listOfSupplier[0];
                    txtSupplier.Text = _supplier.nama_supplier;
                    KeyPressHelper.NextFocus();
                }
                else // data lebih dari satu
                {
                    var frmLookup = new FrmLookupReferensi("Data Supplier", listOfSupplier);
                    frmLookup.Listener = this;
                    frmLookup.ShowDialog();
                }
            }
        }

        private void SetItemProduk(GridControl grid, int rowIndex, int colIndex, Produk produk, double jumlah = 1, double harga = 0, double diskon = 0)
        {
            ItemBeliProduk itemBeli;

            if (_isNewData)
            {
                itemBeli = new ItemBeliProduk();
            }
            else
            {
                itemBeli = _listOfItemBeli[rowIndex - 1];

                if (itemBeli.entity_state == EntityState.Unchanged)
                    itemBeli.entity_state = EntityState.Modified;
            }

            itemBeli.produk_id = produk.produk_id;
            itemBeli.Produk = produk;
            itemBeli.jumlah = jumlah;
            itemBeli.harga = produk.harga_beli;

            if (harga > 0)
                itemBeli.harga = harga;

            itemBeli.diskon = diskon;

            _listOfItemBeli[rowIndex - 1] = itemBeli;
        }

        private void gridControl_CurrentCellKeyDown(object sender, KeyEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                var grid = (GridControl)sender;

                var rowIndex = grid.CurrentCell.RowIndex;
                var colIndex = grid.CurrentCell.ColIndex;

                IProdukBll bll = new ProdukBll(_log);
                Produk produk = null;
                GridCurrentCell cc;

                switch (colIndex)
                {
                    case 2: // kode produk
                        _isValidKodeProduk = false;

                        cc = grid.CurrentCell;
                        var kodeProduk = cc.Renderer.ControlValue.ToString();

                        if (kodeProduk.Length == 0) // kode produk kosong
                        {
                            // fokus ke kolom nama produk
                            GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 1);
                        }
                        else
                        {
                            // pencarian berdasarkan kode produk
                            produk = bll.GetByKode(kodeProduk);

                            if (produk == null)
                            {
                                MsgHelper.MsgWarning("Data produk tidak ditemukan");
                                GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                                return;
                            }

                            _isValidKodeProduk = true;

                            SetItemProduk(grid, rowIndex, colIndex, produk);
                            grid.Refresh();
                            RefreshTotal();

                            GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 2);
                        }

                        break;

                    case 3: // pencarian berdasarkan nama produk

                        cc = grid.CurrentCell;
                        var namaProduk = cc.Renderer.ControlValue.ToString();

                        if (!_isValidKodeProduk)
                        {
                            var listOfProduk = bll.GetByName(namaProduk);

                            if (listOfProduk.Count == 0)
                            {
                                MsgHelper.MsgWarning("Data produk tidak ditemukan");
                                GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                            }
                            else if (listOfProduk.Count == 1)
                            {
                                produk = listOfProduk[0];

                                SetItemProduk(grid, rowIndex, colIndex, produk);
                                grid.Refresh();
                                RefreshTotal();

                                GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 1);
                            }
                            else // data lebih dari satu
                            {
                                _rowIndex = rowIndex;
                                _colIndex = colIndex;

                                var frmLookup = new FrmLookupReferensi("Data Produk", listOfProduk);
                                frmLookup.Listener = this;
                                frmLookup.ShowDialog();
                            }
                        }
                        else
                        {
                            GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 1);
                        }

                        break;

                    case 4: // jumlah
                    case 5: // diskon
                        GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 1);
                        break;

                    case 6:
                        if (grid.RowCount == rowIndex)
                        {
                            _listOfItemBeli.Add(new ItemBeliProduk());
                            grid.RowCount = _listOfItemBeli.Count;
                        }

                        GridListControlHelper.SetCurrentCell(grid, rowIndex + 1, 2); // fokus ke kolom kode produk
                        break;

                    default:
                        break;
                }
            }
        }

        private void gridControl_CurrentCellKeyPress(object sender, KeyPressEventArgs e)
        {
            var grid = (GridControl)sender;
            GridCurrentCell cc = grid.CurrentCell;

            // validasi input angka untuk kolom jumlah dan harga
            switch (cc.ColIndex)
            {
                case 4: // jumlah
                case 5: // diskon
                case 6: // harga
                    e.Handled = KeyPressHelper.NumericOnly(e);
                    break;

                default:
                    break;
            }
        }

        private void gridControl_CurrentCellValidated(object sender, EventArgs e)
        {
            var grid = (GridControl)sender;

            GridCurrentCell cc = grid.CurrentCell;

            var itemBeli = _listOfItemBeli[cc.RowIndex - 1];
            var produk = itemBeli.Produk;

            if (produk != null)
            {
                switch (cc.ColIndex)
                {
                    case 4: // kolom jumlah
                        itemBeli.jumlah = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);
                        break;

                    case 5: // dikson
                        itemBeli.diskon = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);
                        break;

                    case 6: // kolom harga
                        itemBeli.harga = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);
                        break;

                    default:
                        break;
                }

                SetItemProduk(grid, cc.RowIndex, cc.ColIndex, produk, itemBeli.jumlah, itemBeli.harga, itemBeli.diskon);
                grid.Refresh();

                RefreshTotal();
            }           
        }

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // kasus khusus untuk shortcut F2, tidak jalan jika dipanggil melalui event Form KeyDown
            if (KeyPressHelper.IsShortcutKey(Keys.F2, e)) // tambahan data supplier
            {
                ShowEntrySupplier();
            }
        }

        private void ShowEntryProduk()
        {
            var isGrant = RolePrivilegeHelper.IsHaveHakAkses("mnuProduk", _pengguna);
            if (!isGrant)
            {
                MsgHelper.MsgWarning("Maaf Anda tidak mempunyai otoritas untuk mengakses menu ini");
                return;
            }

            IGolonganBll golonganBll = new GolonganBll(_log);
            var listOfGolongan = golonganBll.GetAll();

            Golongan golongan = null;
            if (listOfGolongan.Count > 0)
                golongan = listOfGolongan[0];

            IProdukBll produkBll = new ProdukBll(_log);
            var frmEntryProduk = new FrmEntryProduk("Tambah Data Produk", golongan, listOfGolongan, produkBll);
            frmEntryProduk.Listener = this;
            frmEntryProduk.ShowDialog();
        }

        private void ShowEntrySupplier()
        {
            var isGrant = RolePrivilegeHelper.IsHaveHakAkses("mnuSupplier", _pengguna);
            if (!isGrant)
            {
                MsgHelper.MsgWarning("Maaf Anda tidak mempunyai otoritas untuk mengakses menu ini");
                return;
            }

            ISupplierBll supplierBll = new SupplierBll(_log);
            var frmEntrySupplier = new FrmEntrySupplier("Tambah Data Supplier", supplierBll);
            frmEntrySupplier.Listener = this;
            frmEntrySupplier.ShowDialog();
        }

        private void FrmEntryPembelianProduk_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyPressHelper.IsShortcutKey(Keys.F1, e)) // tambah data produk
            {
                ShowEntryProduk();
            }
            else if (KeyPressHelper.IsShortcutKey(Keys.F2, e)) // tambahan data supplier
            {
                ShowEntrySupplier();
            }
        }

        private void txtDiskon_TextChanged(object sender, EventArgs e)
        {
            RefreshTotal();
        }

        private void txtPPN_TextChanged(object sender, EventArgs e)
        {
            RefreshTotal();
        }

        private void FrmEntryPembelianProduk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // hapus objek dumm
            if (!_isNewData)
            {
                var itemsToRemove = _beli.item_beli.Where(f => f.Produk == null && f.entity_state == EntityState.Added)
                                                   .ToArray();

                foreach (var item in itemsToRemove)
                {
                    _beli.item_beli.Remove(item);
                }
            }
        }

        private void txtPPN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }
    }
}
