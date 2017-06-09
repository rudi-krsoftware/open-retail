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
    public partial class FrmEntryReturPenjualanProduk : FrmEntryStandard, IListener
    {                
        private IReturJualProdukBll _bll = null;
        private ReturJualProduk _retur = null;
        private Customer _customer = null;
        private JualProduk _jual = null;
        private IList<ItemReturJualProduk> _listOfItemRetur = new List<ItemReturJualProduk>();
        private IList<ItemReturJualProduk> _listOfItemReturOld = new List<ItemReturJualProduk>();
        private IList<ItemReturJualProduk> _listOfItemReturDeleted = new List<ItemReturJualProduk>();
        
        private int _rowIndex = 0;
        private int _colIndex = 0;        

        private bool _isNewData = false;
        private bool _isValidJumlahRetur = false;
        private bool _isValidKodeProduk = false;

        private ILog _log;
        private Pengguna _pengguna;

        public IListener Listener { private get; set; }

        public FrmEntryReturPenjualanProduk(string header, IReturJualProdukBll bll) 
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

            _listOfItemRetur.Add(new ItemReturJualProduk()); // add dummy objek

            InitGridControl(gridControl);
        }

        public FrmEntryReturPenjualanProduk(string header, ReturJualProduk retur, IReturJualProdukBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._retur = retur;
            this._customer = retur.Customer;
            this._jual = retur.JualProduk;
            this._log = MainProgram.log;
            this._pengguna = MainProgram.pengguna;

            txtNota.Text = this._retur.nota;
            txtNota.Enabled = false;

            dtpTanggal.Value = (DateTime)this._retur.tanggal;
            txtCustomer.Text = this._customer.nama_customer;            
            txtKeterangan.Text = this._retur.keterangan;

            if (this._jual != null)
            {
                txtNotaJual.Text = this._jual.nota;
                txtNotaJual.Enabled = false;

                LoadItemJual(this._jual);
            }                

            // simpan data lama
            _listOfItemReturOld.Clear();
            foreach (var item in this._retur.item_retur)
            {                
                _listOfItemReturOld.Add(new ItemReturJualProduk
                {
                    item_retur_jual_id = item.item_retur_jual_id,
                    jumlah_retur = item.jumlah_retur,
                    harga_jual = item.harga_jual
                });
            }
            
            _listOfItemRetur = this._retur.item_retur;
            _listOfItemRetur.Add(new ItemReturJualProduk()); // add dummy objek

            InitGridControl(gridControl);

            RefreshTotal();
        }

        private void LoadItemJual(JualProduk jualProduk)
        {
            IJualProdukBll bll = new JualProdukBll(_log);
            _jual.item_jual = bll.GetItemJual(_jual.jual_id);
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

            GridListControlHelper.InitializeGridListControl<ItemReturJualProduk>(grid, _listOfItemRetur, gridListProperties, 30);

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
                            e.Style.CellValue = NumberHelper.NumberToString(itemRetur.harga_jual);

                            break;

                        case 6: // subtotal
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                            e.Style.Enabled = false;

                            e.Style.CellValue = NumberHelper.NumberToString(itemRetur.jumlah_retur * itemRetur.harga_jual);
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

        private double SumGrid(IList<ItemReturJualProduk> listOfItemRetur)
        {
            double total = 0;
            foreach (var item in _listOfItemRetur.Where(f => f.Produk != null))
            {
                total += item.harga_jual * item.jumlah_retur;
            }

            return total;
        }

        private void RefreshTotal()
        {
            lblTotal.Text = NumberHelper.NumberToString(SumGrid(_listOfItemRetur));
        }

        protected override void Simpan()
        {
            if (this._customer == null || txtCustomer.Text.Length == 0)
            {
                MsgHelper.MsgWarning("'Customer' tidak boleh kosong !");
                txtCustomer.Focus();

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
                _retur = new ReturJualProduk();
            
            _retur.jual_id = this._jual.jual_id;
            _retur.JualProduk = this._jual;
            _retur.pengguna_id = this._pengguna.pengguna_id;
            _retur.Pengguna = this._pengguna;
            _retur.customer_id = this._customer.customer_id;
            _retur.Customer = this._customer;
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

                    _customer = null;
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
                    var itemRetur = _listOfItemReturOld.Where(f => f.item_retur_jual_id == item.item_retur_jual_id)
                                                       .SingleOrDefault();

                    if (itemRetur != null)
                    {
                        item.jumlah_retur = itemRetur.jumlah_retur;
                        item.harga_jual = itemRetur.harga_jual;
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
            if (data is ItemJualProduk) // pencarian produk baku
            {
                var itemJual = (ItemJualProduk)data;
                var produk = itemJual.Produk;

                if (!IsExist(produk.produk_id))
                {
                    SetItemProduk(this.gridControl, _rowIndex, _colIndex + 1, itemJual, itemJual.jumlah - itemJual.jumlah_retur, itemJual.harga_jual);
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
            else if (data is Customer) // pencarian customer
            {
                this._customer = (Customer)data;
                txtCustomer.Text = this._customer.nama_customer;
                KeyPressHelper.NextFocus();
            }
            else if (data is JualProduk) // pencarian data jual
            {
                IJualProdukBll bll = new JualProdukBll(_log);

                this._jual = (JualProduk)data;
                this._jual.item_jual = bll.GetItemJual(this._jual.jual_id);
                txtNotaJual.Text = this._jual.nota;

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

        private void txtCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                var name = ((TextBox)sender).Text;

                ICustomerBll bll = new CustomerBll(_log);
                var listOfCustomer = bll.GetByName(name);

                if (listOfCustomer.Count == 0)
                {
                    MsgHelper.MsgWarning("Data Customer tidak ditemukan");
                    txtCustomer.Focus();
                    txtCustomer.SelectAll();

                }
                else if (listOfCustomer.Count == 1)
                {
                    _customer = listOfCustomer[0];
                    txtCustomer.Text = _customer.nama_customer;
                    KeyPressHelper.NextFocus();
                }
                else // data lebih dari satu
                {
                    var frmLookup = new FrmLookupReferensi("Data Customer", listOfCustomer);
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

                if (this._jual == null || txtNotaJual.Text.Length == 0)
                {
                    MsgHelper.MsgWarning("Maaf isian data belum lengkap !");
                    txtNotaJual.Focus();

                    return;
                }

                var grid = (GridControl)sender;

                var rowIndex = grid.CurrentCell.RowIndex;
                var colIndex = grid.CurrentCell.ColIndex;

                GridCurrentCell cc;

                ItemJualProduk itemJual;

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
                            itemJual = this._jual.item_jual.Where(f => f.Produk.kode_produk.ToLower() == kodeProduk.ToLower() && f.jumlah > f.jumlah_retur)
                                                           .SingleOrDefault();

                            if (itemJual == null)
                            {
                                MsgHelper.MsgWarning("Data produk tidak ditemukan");
                                GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                                return;
                            }

                            _isValidKodeProduk = true;

                            if (!IsExist(itemJual.produk_id))
                            {
                                SetItemProduk(grid, rowIndex, colIndex, itemJual, itemJual.jumlah - itemJual.jumlah_retur, itemJual.harga_jual);
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
                            var listOfItemJual = this._jual.item_jual.Where(f => f.Produk.nama_produk.ToLower().Contains(namaProduk.ToLower()) && f.jumlah > f.jumlah_retur)
                                                                     .ToList();

                            if (listOfItemJual.Count == 0)
                            {
                                MsgHelper.MsgWarning("Data produk tidak ditemukan");
                                GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                            }
                            else if (listOfItemJual.Count == 1)
                            {
                                itemJual = listOfItemJual[0];

                                if (!IsExist(itemJual.produk_id))
                                {
                                    SetItemProduk(grid, rowIndex, colIndex, itemJual, itemJual.jumlah - itemJual.jumlah_retur, itemJual.harga_jual);
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

                                var frmLookup = new FrmLookupItemNota("Item Penjualan", listOfItemJual);
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
                            _listOfItemRetur.Add(new ItemReturJualProduk());
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

        private void SetItemProduk(GridControl grid, int rowIndex, int colIndex, ItemJualProduk itemJual, double jumlahRetur = 0, double harga = 0)
        {
            ItemReturJualProduk itemRetur;

            if (_isNewData)
            {
                itemRetur = new ItemReturJualProduk();
            }
            else
            {
                itemRetur = _listOfItemRetur[rowIndex - 1];

                if (itemRetur.entity_state == EntityState.Unchanged)
                    itemRetur.entity_state = EntityState.Modified;
            }

            var produk = itemJual.Produk;

            itemRetur.item_jual_id = itemJual.item_jual_id;
            itemRetur.produk_id = produk.produk_id;
            itemRetur.Produk = produk;

            itemRetur.jumlah = itemJual.jumlah;
            itemRetur.jumlah_retur = jumlahRetur;
            itemRetur.harga_jual = harga;

            _listOfItemRetur[rowIndex - 1] = itemRetur;

            itemJual.jumlah_retur = jumlahRetur;
        }

        private void gridControl_CurrentCellValidated(object sender, EventArgs e)
        {
            var grid = (GridControl)sender;

            GridCurrentCell cc = grid.CurrentCell;

            if (this._jual != null)
            {
                var obj = _listOfItemRetur[cc.RowIndex - 1];
                var itemJual = this._jual.item_jual.Where(f => f.produk_id == obj.produk_id)
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
                            obj.harga_jual = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);
                            break;

                        default:
                            break;
                    }

                    SetItemProduk(grid, cc.RowIndex, cc.ColIndex, itemJual, obj.jumlah_retur, obj.harga_jual);
                    grid.Refresh();

                    RefreshTotal();
                }
            }           
        }

        private void FrmEntryReturPenjualanProduk_FormClosing(object sender, FormClosingEventArgs e)
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

        private void txtNotaJual_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                if (this._customer == null || txtCustomer.Text.Length == 0)
                {
                    MsgHelper.MsgWarning("Maaf isian data belum lengkap !");
                    txtCustomer.Focus();

                    return;
                }

                var nota = ((TextBox)sender).Text;

                IJualProdukBll bll = new JualProdukBll(_log);
                var listOfJual = bll.GetNotaCustomer(this._customer.customer_id, nota);

                if (listOfJual.Count == 0)
                {
                    MsgHelper.MsgWarning("Data nota jual tidak ditemukan");
                    txtNotaJual.Focus();
                    txtNotaJual.SelectAll();
                }
                else if (listOfJual.Count == 1)
                {
                    _jual = listOfJual[0];
                    _jual.item_jual = bll.GetItemJual(_jual.jual_id);

                    txtNotaJual.Text = _jual.nota;
                    KeyPressHelper.NextFocus();
                }
                else // data lebih dari satu
                {
                    var frmLookup = new FrmLookupNota("Data Nota Penjualan", listOfJual);
                    frmLookup.Listener = this;
                    frmLookup.ShowDialog();
                }
            }
        }
    }
}
