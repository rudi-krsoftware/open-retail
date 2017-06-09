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

namespace OpenRetail.App.Transaksi
{
    public partial class FrmEntryReturPembelianProduk : FrmEntryStandard, IListener
    {        
        private IReturBeliProdukBll _bll = null;
        private ReturBeliProduk _retur = null;
        private Supplier _supplier = null;
        private BeliProduk _beli = null;
        private IList<ItemReturBeliProduk> _listOfItemRetur = new List<ItemReturBeliProduk>();
        private IList<ItemReturBeliProduk> _listOfItemReturOld = new List<ItemReturBeliProduk>();
        private IList<ItemReturBeliProduk> _listOfItemReturDeleted = new List<ItemReturBeliProduk>();

        private int _rowIndex = 0;
        private int _colIndex = 0;        

        private bool _isNewData = false;
        private bool _isValidJumlahRetur = false;
        private bool _isValidKodeProduk = false;

        private ILog _log;
        private Pengguna _pengguna;

        public IListener Listener { private get; set; }

        public FrmEntryReturPembelianProduk(string header, IReturBeliProdukBll bll) 
            : base()
        {            
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._bll = bll;
            this._isNewData = true;
            this._log = MainProgram.log;
            this._pengguna = MainProgram.pengguna;

            txtNota.Text = bll.GetLastNota();
            dtpTanggal.Value = DateTime.Today;

            _listOfItemRetur.Add(new ItemReturBeliProduk()); // add dummy objek

            InitGridControl(gridControl);
        }

        public FrmEntryReturPembelianProduk(string header, ReturBeliProduk retur, IReturBeliProdukBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._retur = retur;
            this._supplier = retur.Supplier;
            this._beli = retur.BeliProduk;
            this._log = MainProgram.log;
            this._pengguna = MainProgram.pengguna;

            txtNota.Text = this._retur.nota;
            txtNota.Enabled = false;

            dtpTanggal.Value = (DateTime)this._retur.tanggal;
            txtSupplier.Text = this._supplier.nama_supplier;            
            txtKeterangan.Text = this._retur.keterangan;

            if (this._beli != null)
            {
                txtNotaBeli.Text = this._beli.nota;
                txtNotaBeli.Enabled = false;

                LoadItemBeli(this._beli);
            }                

            // simpan data lama
            _listOfItemReturOld.Clear();
            foreach (var item in this._retur.item_retur)
            {
                _listOfItemReturOld.Add(new ItemReturBeliProduk
                {
                    item_retur_beli_produk_id = item.item_retur_beli_produk_id,
                    jumlah_retur = item.jumlah_retur,
                    harga = item.harga
                });
            }
            
            _listOfItemRetur = this._retur.item_retur;
            _listOfItemRetur.Add(new ItemReturBeliProduk()); // add dummy objek

            InitGridControl(gridControl);

            RefreshTotal();
        }

        private void LoadItemBeli(BeliProduk beliProduk)
        {
            IBeliProdukBll bll = new BeliProdukBll(_log);
            _beli.item_beli = bll.GetItemBeli(_beli.beli_produk_id);
        }

        private void InitGridControl(GridControl grid)
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kode Produk", Width = 120 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama Produk", Width = 240 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jumlah Retur", Width = 60 });
            gridListProperties.Add(new GridListControlProperties { Header = "Harga", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sub Total", Width = 110 });
            gridListProperties.Add(new GridListControlProperties { Header = "Aksi" });

            GridListControlHelper.InitializeGridListControl<ItemReturBeliProduk>(grid, _listOfItemRetur, gridListProperties, 30);

            grid.PushButtonClick += delegate(object sender, GridCellPushButtonClickEventArgs e)
            {
                if (e.ColIndex == 7)
                {
                    if (grid.RowCount == 1)
                    {
                        MsgHelper.MsgWarning("Minimal 1 produk harus diinputkan !");
                        return;
                    }

                    if (MsgHelper.MsgDelete())
                    {
                        var itemRetur = _listOfItemRetur[e.RowIndex - 1];
                        itemRetur.entity_state = EntityState.Deleted;

                        _listOfItemReturDeleted.Add(itemRetur);
                        _listOfItemRetur.Remove(itemRetur);

                        grid.RowCount = _listOfItemRetur.Count();
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
                    if (!(_listOfItemRetur.Count > 0))
                        return;

                    var itemRetur = _listOfItemRetur[e.RowIndex - 1];
                    var produk = itemRetur.Produk;

                    if (e.RowIndex % 2 == 0)
                        e.Style.BackColor = ColorCollection.BACK_COLOR_ALTERNATE;

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

                        case 4: // jumlah retur
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.CellValue = itemRetur.jumlah_retur;

                            break;

                        case 5: // harga
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                            e.Style.CellValue = NumberHelper.NumberToString(itemRetur.harga);

                            break;

                        case 6: // subtotal
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                            e.Style.Enabled = false;

                            e.Style.CellValue = NumberHelper.NumberToString(itemRetur.jumlah_retur * itemRetur.harga);
                            break;

                        case 7: // button hapus
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.CellType = GridCellTypeName.PushButton;
                            e.Style.Enabled = true;
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

        private double SumGrid(IList<ItemReturBeliProduk> listOfItemRetur)
        {
            double total = 0;
            foreach (var item in _listOfItemRetur.Where(f => f.Produk != null))
            {
                total += item.harga * item.jumlah_retur;
            }

            return total;
        }

        private void RefreshTotal()
        {
            lblTotal.Text = NumberHelper.NumberToString(SumGrid(_listOfItemRetur));
        }

        protected override void Simpan()
        {
            if (this._supplier == null || txtSupplier.Text.Length == 0)
            {
                MsgHelper.MsgWarning("'Supplier' tidak boleh kosong !");
                txtSupplier.Focus();

                return;
            }

            var total = SumGrid(this._listOfItemRetur);
            if (!(total > 0))
            {
                MsgHelper.MsgWarning("Anda belum melengkapi inputan data produk !");
                return;
            }

            if (!MsgHelper.MsgKonfirmasi("Apakah proses ingin dilanjutkan ?"))
                return;

            if (_isNewData)
                _retur = new ReturBeliProduk();

            _retur.beli_produk_id = this._beli.beli_produk_id;
            _retur.BeliProduk = this._beli;
            _retur.pengguna_id = this._pengguna.pengguna_id;
            _retur.Pengguna = this._pengguna;
            _retur.supplier_id = this._supplier.supplier_id;
            _retur.Supplier = this._supplier;
            _retur.nota = txtNota.Text;
            _retur.tanggal = dtpTanggal.Value;
            _retur.keterangan = txtKeterangan.Text;

            _retur.item_retur = this._listOfItemRetur.Where(f => f.Produk != null).ToList();

            if (!_isNewData) // update
                _retur.item_retur_deleted = _listOfItemReturDeleted;

            var result = 0;
            var validationError = new ValidationError();

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                if (_isNewData)
                {
                    result = _bll.Save(_retur, ref validationError);
                }
                else
                {
                    result = _bll.Update(_retur, ref validationError);
                }

                if (result > 0)
                {
                    Listener.Ok(this, _isNewData, _retur);

                    _supplier = null;
                    _listOfItemRetur.Clear();
                    _listOfItemReturDeleted.Clear();                                        

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

        protected override void Selesai()
        {
            // restore data lama
            if (!_isNewData)
            {
                // restore item yang di edit
                var itemsModified = _retur.item_retur.Where(f => f.Produk != null && f.entity_state == EntityState.Modified)
                                                     .ToArray();

                foreach (var item in itemsModified)
                {
                    var itemRetur = _listOfItemReturOld.Where(f => f.item_retur_beli_produk_id == item.item_retur_beli_produk_id)
                                                       .SingleOrDefault();

                    if (itemRetur != null)
                    {
                        item.jumlah_retur = itemRetur.jumlah_retur;
                        item.harga = itemRetur.harga;
                    }
                }

                // restore item yang di delete
                var itemsDeleted = _listOfItemReturDeleted.Where(f => f.Produk != null && f.entity_state == EntityState.Deleted)
                                                          .ToArray();
                foreach (var item in itemsDeleted)
                {
                    item.entity_state = EntityState.Unchanged;
                    this._retur.item_retur.Add(item);
                }

                _listOfItemReturDeleted.Clear();
            }

            base.Selesai();
        }

        public void Ok(object sender, object data)
        {
            if (data is ItemBeliProduk) // pencarian produk baku
            {
                var itemBeli = (ItemBeliProduk)data;
                var produk = itemBeli.Produk;

                if (!IsExist(produk.produk_id))
                {
                    SetItemProduk(this.gridControl, _rowIndex, _colIndex + 1, itemBeli, itemBeli.jumlah - itemBeli.jumlah_retur, itemBeli.harga);
                    this.gridControl.Refresh();
                    RefreshTotal();

                    GridListControlHelper.SetCurrentCell(this.gridControl, _rowIndex, _colIndex + 1);

                    RefreshTotal();
                }
                else
                {
                    MsgHelper.MsgWarning("Data produk sudah diinputkan");
                    GridListControlHelper.SelectCellText(this.gridControl, _rowIndex, _colIndex);
                }
            }
            else if (data is Supplier) // pencarian Customer
            {
                this._supplier = (Supplier)data;
                txtSupplier.Text = this._supplier.nama_supplier;
                KeyPressHelper.NextFocus();
            }
            else if (data is BeliProduk) // pencarian data jual
            {
                IBeliProdukBll bll = new BeliProdukBll(_log);

                this._beli = (BeliProduk)data;
                this._beli.item_beli = bll.GetItemBeli(this._beli.beli_produk_id);
                txtNotaBeli.Text = this._beli.nota;

                KeyPressHelper.NextFocus();
            }
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            // do nothing
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
                var name = ((TextBox)sender).Text;

                ISupplierBll bll = new SupplierBll(_log);
                var listOfSupplier = bll.GetByName(name);

                if (listOfSupplier.Count == 0)
                {
                    MsgHelper.MsgWarning("Data Supplier tidak ditemukan");
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

        private bool IsExist(string produkId)
        {
            var count = _listOfItemRetur.Where(f => f.produk_id != null && f.produk_id.ToLower() == produkId.ToLower())
                                        .Count();

            return (count > 0);
        }

        private void gridControl_CurrentCellKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (this._beli == null || txtNotaBeli.Text.Length == 0)
                {
                    MsgHelper.MsgWarning("Maaf isian data belum lengkap !");
                    txtNotaBeli.Focus();

                    return;
                }

                var grid = (GridControl)sender;

                var rowIndex = grid.CurrentCell.RowIndex;
                var colIndex = grid.CurrentCell.ColIndex;

                GridCurrentCell cc;

                ItemBeliProduk itemBeli;

                switch (colIndex)
                {
                    case 2: // kode produk
                        _isValidKodeProduk = false;

                        cc = grid.CurrentCell;
                        var kodeProduk = cc.Renderer.ControlValue.ToString();

                        if (kodeProduk.Length == 0)
                        {
                            GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 1);
                        }
                        else
                        {
                            itemBeli = this._beli.item_beli.Where(f => f.Produk.kode_produk.ToLower() == kodeProduk.ToLower() && f.jumlah > f.jumlah_retur)
                                                           .SingleOrDefault();

                            if (itemBeli == null)
                            {
                                MsgHelper.MsgWarning("Data produk tidak ditemukan");
                                GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                                return;
                            }

                            _isValidKodeProduk = true;

                            if (!IsExist(itemBeli.produk_id))
                            {
                                SetItemProduk(grid, rowIndex, colIndex, itemBeli, itemBeli.jumlah - itemBeli.jumlah_retur, itemBeli.harga);
                                grid.Refresh();

                                RefreshTotal();

                                GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 2);
                            }
                            else
                            {
                                GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 2);
                            }
                        }

                        break;

                    case 3: // nama produk

                        cc = grid.CurrentCell;
                        var namaProduk = cc.Renderer.ControlValue.ToString();

                        if (!_isValidKodeProduk)
                        {
                            var listOfItemBeli = this._beli.item_beli.Where(f => f.Produk.nama_produk.ToLower().Contains(namaProduk.ToLower()) && f.jumlah > f.jumlah_retur)
                                                                     .ToList();

                            if (listOfItemBeli.Count == 0)
                            {
                                MsgHelper.MsgWarning("Data produk tidak ditemukan");
                                GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                            }
                            else if (listOfItemBeli.Count == 1)
                            {
                                itemBeli = listOfItemBeli[0];

                                if (!IsExist(itemBeli.produk_id))
                                {
                                    SetItemProduk(grid, rowIndex, colIndex, itemBeli, itemBeli.jumlah - itemBeli.jumlah_retur, itemBeli.harga);
                                    grid.Refresh();

                                    GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 1);
                                }
                                else
                                {
                                    MsgHelper.MsgWarning("Data produk sudah diinputkan");
                                    GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                                }
                            }
                            else // data lebih dari satu
                            {
                                _rowIndex = rowIndex;
                                _colIndex = colIndex;

                                var frmLookup = new FrmLookupItemNota("Item Pembelian", listOfItemBeli);
                                frmLookup.Listener = this;
                                frmLookup.ShowDialog();
                            }
                        }                        

                        break;

                    case 4:
                        _isValidJumlahRetur = false;

                        try
                        {
                            cc = grid.CurrentCell;
                            double jumlahRetur = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);

                            var jumlahJual = _listOfItemRetur[rowIndex - 1].jumlah;
                            if (jumlahRetur <= jumlahJual)
                            {
                                _isValidJumlahRetur = true;
                                GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 1);
                            }
                            else
                                MsgHelper.MsgWarning("Maaf, jumlah retur tidak boleh melebihi jumlah jual");
                        }
                        catch
                        {
                        }

                        break;

                    case 5:
                        if (grid.RowCount == rowIndex)
                        {
                            _listOfItemRetur.Add(new ItemReturBeliProduk());
                            grid.RowCount = _listOfItemRetur.Count;
                        }

                        GridListControlHelper.SetCurrentCell(grid, rowIndex + 1, 2); // fokus ke kolom nama produk
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

            // validasi input angka untuk kolom jumlah retur dan harga
            switch (cc.ColIndex)
            {
                case 4: // jumlah retur
                case 5: // harga
                    e.Handled = KeyPressHelper.NumericOnly(e);
                    break;

                default:
                    break;
            }
        }

        private void SetItemProduk(GridControl grid, int rowIndex, int colIndex, ItemBeliProduk itemBeli, double jumlahRetur = 0, double harga = 0)
        {
            ItemReturBeliProduk itemRetur;

            if (_isNewData)
            {
                itemRetur = new ItemReturBeliProduk();
            }
            else
            {
                itemRetur = _listOfItemRetur[rowIndex - 1];

                if (itemRetur.entity_state == EntityState.Unchanged)
                    itemRetur.entity_state = EntityState.Modified;
            }

            var produk = itemBeli.Produk;

            itemRetur.item_beli_id = itemBeli.item_beli_produk_id;
            itemRetur.produk_id = produk.produk_id;
            itemRetur.Produk = produk;

            itemRetur.jumlah = itemBeli.jumlah;
            itemRetur.jumlah_retur = jumlahRetur;
            itemRetur.harga = harga;

            _listOfItemRetur[rowIndex - 1] = itemRetur;

            itemBeli.jumlah_retur = jumlahRetur;
        }

        private void gridControl_CurrentCellValidated(object sender, EventArgs e)
        {
            var grid = (GridControl)sender;

            GridCurrentCell cc = grid.CurrentCell;

            if (this._beli != null)
            {
                var obj = _listOfItemRetur[cc.RowIndex - 1];
                var itemJual = this._beli.item_beli.Where(f => f.produk_id == obj.produk_id)
                                                  .SingleOrDefault();

                if (itemJual != null)
                {
                    switch (cc.ColIndex)
                    {
                        case 4:
                            if (_isValidJumlahRetur)
                                obj.jumlah_retur = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);

                            break;

                        case 5:
                            obj.harga = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);
                            break;

                        default:
                            break;
                    }

                    SetItemProduk(grid, cc.RowIndex, cc.ColIndex, itemJual, obj.jumlah_retur, obj.harga);
                    grid.Refresh();

                    RefreshTotal();
                }
            }           
        }

        private void FrmEntryReturPembelianProduk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // hapus objek dumm
            if (!_isNewData)
            {
                var itemsToRemove = _retur.item_retur.Where(f => f.Produk == null && f.entity_state == EntityState.Added)
                                                     .ToArray();

                foreach (var item in itemsToRemove)
                {
                    _retur.item_retur.Remove(item);
                }
            }
        }

        private void txtNotaBeli_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                if (this._supplier == null || txtSupplier.Text.Length == 0)
                {
                    MsgHelper.MsgWarning("Maaf isian data belum lengkap !");
                    txtSupplier.Focus();

                    return;
                }

                var nota = ((TextBox)sender).Text;

                IBeliProdukBll bll = new BeliProdukBll(_log);
                var listOfBeli = bll.GetNotaSupplier(this._supplier.supplier_id, nota);

                if (listOfBeli.Count == 0)
                {
                    MsgHelper.MsgWarning("Data nota beli tidak ditemukan");
                    txtNotaBeli.Focus();
                    txtNotaBeli.SelectAll();
                }
                else if (listOfBeli.Count == 1)
                {
                    _beli = listOfBeli[0];
                    _beli.item_beli = bll.GetItemBeli(_beli.beli_produk_id);

                    txtNotaBeli.Text = _beli.nota;
                    KeyPressHelper.NextFocus();
                }
                else // data lebih dari satu
                {
                    var frmLookup = new FrmLookupNota("Data Nota Pembelian", listOfBeli);
                    frmLookup.Listener = this;
                    frmLookup.ShowDialog();
                }
            }
        }
    }
}
