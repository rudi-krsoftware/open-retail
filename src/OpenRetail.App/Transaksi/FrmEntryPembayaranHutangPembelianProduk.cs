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
    public partial class FrmEntryPembayaranHutangPembelianProduk : FrmEntryStandard, IListener
    {
        private IPembayaranHutangProdukBll _bll = null;
        private PembayaranHutangProduk _pembayaranHutang = null;
        private Supplier _supplier = null;
        private IList<ItemPembayaranHutangProduk> _listOfItemPembayaranHutang = new List<ItemPembayaranHutangProduk>();
        private IList<ItemPembayaranHutangProduk> _listOfItemPembayaranHutangOld = new List<ItemPembayaranHutangProduk>();
        private IList<ItemPembayaranHutangProduk> _listOfItemPembayaranHutangDeleted = new List<ItemPembayaranHutangProduk>();

        private int _rowIndex = 0;
        private int _colIndex = 0;

        private bool _isNewData = false;
        private ILog _log;
        private Pengguna _pengguna;

        public IListener Listener { private get; set; }

        public FrmEntryPembayaranHutangPembelianProduk(string header, IPembayaranHutangProdukBll bll) 
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

            _listOfItemPembayaranHutang.Add(new ItemPembayaranHutangProduk()); // add dummy objek

            InitGridControl(gridControl);
        }

        public FrmEntryPembayaranHutangPembelianProduk(string header, PembayaranHutangProduk pembayaranHutang, IPembayaranHutangProdukBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._pembayaranHutang = pembayaranHutang;
            this._supplier = pembayaranHutang.Supplier;
            this._log = MainProgram.log;
            this._pengguna = MainProgram.pengguna;

            txtNota.Text = this._pembayaranHutang.nota;
            dtpTanggal.Value = (DateTime)this._pembayaranHutang.tanggal;

            txtSupplier.Text = this._supplier.nama_supplier;
            txtKeterangan.Text = this._pembayaranHutang.keterangan;

            // simpan data lama
            _listOfItemPembayaranHutangOld.Clear();
            foreach (var item in this._pembayaranHutang.item_pembayaran_hutang)
            {
                _listOfItemPembayaranHutangOld.Add(new ItemPembayaranHutangProduk
                {
                    item_pembayaran_hutang_produk_id = item.item_pembayaran_hutang_produk_id,
                    nominal = item.nominal,
                    keterangan = item.keterangan
                });
            }
            
            _listOfItemPembayaranHutang = this._pembayaranHutang.item_pembayaran_hutang;
            _listOfItemPembayaranHutang.Add(new ItemPembayaranHutangProduk()); // add dummy objek

            InitGridControl(gridControl);

            RefreshTotal();
        }

        private void InitGridControl(GridControl grid)
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nota Beli", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Total", Width = 100, IsEditable = false, HorizontalAlignment = GridHorizontalAlignment.Right });
            gridListProperties.Add(new GridListControlProperties { Header = "Kekurangan", Width = 100, IsEditable = false, HorizontalAlignment = GridHorizontalAlignment.Right });
            gridListProperties.Add(new GridListControlProperties { Header = "Pembayaran", Width = 100, HorizontalAlignment = GridHorizontalAlignment.Right });
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan", Width = 200 });
            gridListProperties.Add(new GridListControlProperties { Header = "Aksi" });

            GridListControlHelper.InitializeGridListControl<ItemPembayaranHutangProduk>(grid, _listOfItemPembayaranHutang, gridListProperties);

            grid.PushButtonClick += delegate(object sender, GridCellPushButtonClickEventArgs e)
            {
                if (e.ColIndex == 7)
                {
                    if (grid.RowCount == 1)
                    {
                        MsgHelper.MsgWarning("Minimal 1 nota harus diinputkan !");
                        return;
                    }

                    if (MsgHelper.MsgDelete())
                    {
                        var pembayaranHutang = _listOfItemPembayaranHutang[e.RowIndex - 1];
                        pembayaranHutang.entity_state = EntityState.Deleted;

                        _listOfItemPembayaranHutangDeleted.Add(pembayaranHutang);
                        _listOfItemPembayaranHutang.Remove(pembayaranHutang);

                        grid.RowCount = _listOfItemPembayaranHutang.Count();
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
                    if (!(_listOfItemPembayaranHutang.Count > 0))
                        return;

                    double grand_total = 0;
                    double sisaNota = 0;

                    var itemPembayaran = _listOfItemPembayaranHutang[e.RowIndex - 1];
                    var beli = itemPembayaran.BeliProduk;
                    if (beli != null)
                    {
                        grand_total = beli.grand_total; ;
                        sisaNota = beli.sisa_nota;
                    }

                    switch (e.ColIndex)
                    {
                        case 1: // no urut
                            e.Style.CellValue = e.RowIndex.ToString();
                            break;

                        case 2: // nota beli                            
                            if (beli != null)
                                e.Style.CellValue = beli.nota;

                            if (beli != null)
                            {
                                if (beli.tanggal_tempo.IsNull()) // nota tunai nominalnya tidak bisa diedit
                                {
                                    e.Style.Enabled = false;
                                    e.Style.BackColor = ColorCollection.DEFAULT_FORM_COLOR;
                                    base.SetButtonSimpanToFalse(true);
                                }
                            }

                            break;

                        case 3: // total
                            e.Style.CellValue = NumberHelper.NumberToString(grand_total);

                            break;

                        case 4: // kekurangan
                            e.Style.CellValue = NumberHelper.NumberToString(sisaNota);

                            break;

                        case 5: // pembayaran
                            if (beli != null)
                            {
                                if (beli.tanggal_tempo.IsNull()) // nota tunai nominalnya tidak bisa diedit
                                {
                                    e.Style.Enabled = false;
                                    e.Style.BackColor = ColorCollection.DEFAULT_FORM_COLOR;
                                }
                            }

                            e.Style.CellValue = NumberHelper.NumberToString(itemPembayaran.nominal);

                            break;

                        case 6: // keterangan
                            e.Style.CellValue = itemPembayaran.keterangan;

                            break;

                        case 7: // button hapus
                            e.Style.CellType = GridCellTypeName.PushButton;
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.Description = "Hapus";

                            if (beli != null)
                            {
                                e.Style.Enabled = !beli.tanggal_tempo.IsNull();
                            }

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

        private double SumGrid(IList<ItemPembayaranHutangProduk> listOfItemPembayaranHutang)
        {
            double total = 0;
            foreach (var item in listOfItemPembayaranHutang.Where(f => f.BeliProduk != null))
            {
                total += item.nominal;
            }

            return total;
        }

        private void RefreshTotal()
        {
            lblTotal.Text = NumberHelper.NumberToString(SumGrid(_listOfItemPembayaranHutang));
        }

        protected override void Simpan()
        {
            if (this._supplier == null || txtSupplier.Text.Length == 0)
            {
                MsgHelper.MsgWarning("'Supplier' tidak boleh kosong !");
                txtSupplier.Focus();

                return;
            }

            var total = SumGrid(this._listOfItemPembayaranHutang);
            if (!(total > 0))
            {
                MsgHelper.MsgWarning("Anda belum melengkapi inputan data pembayaran !");
                return;
            }

            if (!MsgHelper.MsgKonfirmasi("Apakah proses ingin dilanjutkan ?"))
                return;

            if (_isNewData)
                _pembayaranHutang = new PembayaranHutangProduk();

            _pembayaranHutang.pengguna_id = this._pengguna.pengguna_id;
            _pembayaranHutang.Pengguna = this._pengguna;
            _pembayaranHutang.supplier_id = this._supplier.supplier_id;
            _pembayaranHutang.Supplier = this._supplier;
            _pembayaranHutang.nota = txtNota.Text;
            _pembayaranHutang.tanggal = dtpTanggal.Value;
            _pembayaranHutang.keterangan = txtKeterangan.Text;

            _pembayaranHutang.item_pembayaran_hutang = this._listOfItemPembayaranHutang.Where(f => f.BeliProduk != null).ToList();

            if (!_isNewData) // update
                _pembayaranHutang.item_pembayaran_hutang_deleted = _listOfItemPembayaranHutangDeleted;

            var result = 0;
            var validationError = new ValidationError();

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                if (_isNewData)
                {
                    result = _bll.Save(_pembayaranHutang, false, ref validationError);
                }
                else
                {
                    result = _bll.Update(_pembayaranHutang, false, ref validationError);
                }

                if (result > 0)
                {
                    Listener.Ok(this, _isNewData, _pembayaranHutang);

                    _supplier = null;
                    _listOfItemPembayaranHutang.Clear();
                    _listOfItemPembayaranHutangDeleted.Clear();                                        

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
                var itemsModified = _pembayaranHutang.item_pembayaran_hutang.Where(f => f.BeliProduk != null && f.entity_state == EntityState.Modified)
                                                     .ToArray();

                foreach (var item in itemsModified)
                {
                    var itemPembayaran = _listOfItemPembayaranHutangOld.Where(f => f.item_pembayaran_hutang_produk_id == item.item_pembayaran_hutang_produk_id)
                                                                       .SingleOrDefault();

                    if (itemPembayaran != null)
                    {
                        item.nominal = itemPembayaran.nominal;
                        item.keterangan = itemPembayaran.keterangan;
                    }
                }

                // restore item yang di delete
                var itemsDeleted = _listOfItemPembayaranHutangDeleted.Where(f => f.BeliProduk != null && f.entity_state == EntityState.Deleted)
                                                                     .ToArray();
                foreach (var item in itemsDeleted)
                {
                    item.entity_state = EntityState.Unchanged;
                    this._pembayaranHutang.item_pembayaran_hutang.Add(item);
                }

                _listOfItemPembayaranHutangDeleted.Clear();
            }

            base.Selesai();
        }

        public void Ok(object sender, object data)
        {
            if (data is BeliProduk) // pencarian nota
            {
                var beli = (BeliProduk)data;

                if (!IsExist(beli.nota))
                {
                    SetItemBayar(this.gridControl, _rowIndex, _colIndex + 1, beli);
                    this.gridControl.Refresh();

                    GridListControlHelper.SetCurrentCell(this.gridControl, _rowIndex, 5); // kolom pembayaran
                }
                else
                {
                    MsgHelper.MsgWarning("Data nota sudah diinputkan");
                    GridListControlHelper.SelectCellText(this.gridControl, _rowIndex, _colIndex);
                }
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

        private bool IsExist(string nota)
        {
            var count = _listOfItemPembayaranHutang.Where(f => f.BeliProduk != null && f.BeliProduk.nota.ToLower() == nota.ToLower())
                                                   .Count();

            return (count > 0);
        }

        private void gridControl_CurrentCellKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var grid = (GridControl)sender;

                var rowIndex = grid.CurrentCell.RowIndex;
                var colIndex = grid.CurrentCell.ColIndex;                

                switch (colIndex)
                {
                    case 2: // kolom nota
                        if (this._supplier == null || txtSupplier.Text.Length == 0)
                        {
                            MsgHelper.MsgWarning("Nama supplier belum diinputkan");
                            txtSupplier.Focus();
                            return;
                        }

                        var cc = grid.CurrentCell;
                        var nota = cc.Renderer.ControlValue.ToString();

                        IList<BeliProduk> listOfBeli = null;
                        IBeliProdukBll bll = new BeliProdukBll(_log);

                        if (nota.Length > 0) // menampilkan nota kredit berdasarkan nota
                        {
                            listOfBeli = bll.GetNotaKreditByNota(this._supplier.supplier_id, nota);
                        }
                        else
                        {
                            listOfBeli = bll.GetNotaKreditBySupplier(this._supplier.supplier_id, false);
                        }

                        if (listOfBeli.Count == 0)
                        {
                            MsgHelper.MsgWarning("Data pembelian kredit tidak ditemukan");
                            GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                        }
                        else if (listOfBeli.Count == 1)
                        {
                            var beli = listOfBeli[0];

                            if (!IsExist(beli.nota))
                            {
                                SetItemBayar(grid, rowIndex, colIndex, beli);
                                grid.Refresh();

                                GridListControlHelper.SetCurrentCell(grid, rowIndex, 5); // kolom pembayaran
                            }
                            else
                            {
                                MsgHelper.MsgWarning("Data pembayaran sudah diinputkan");
                                GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                            }
                        }
                        else // data nota lebih dari satu, tampilkan di form lookup
                        {
                            _rowIndex = rowIndex;
                            _colIndex = colIndex;

                            var frmLookup = new FrmLookupNota("Data Nota Pembelian", listOfBeli);
                            frmLookup.Listener = this;
                            frmLookup.ShowDialog();
                        }

                        break;

                    case 6: // keterangan
                        if (grid.RowCount == rowIndex)
                        {
                            _listOfItemPembayaranHutang.Add(new ItemPembayaranHutangProduk());
                            grid.RowCount = _listOfItemPembayaranHutang.Count;
                        }

                        GridListControlHelper.SetCurrentCell(grid, rowIndex + 1, 2); // fokus ke kolom nota beli
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

            switch (cc.ColIndex)
            {
                case 5: // pembayaran
                    e.Handled = KeyPressHelper.NumericOnly(e);
                    break;

                default:
                    break;
            }
        }

        private void SetItemBayar(GridControl grid, int rowIndex, int colIndex, BeliProduk beli, double nominal = 0, string keterangan = "")
        {
            ItemPembayaranHutangProduk itemBayar;

            if (_isNewData)
            {
                itemBayar = new ItemPembayaranHutangProduk();
            }
            else
            {
                itemBayar = _listOfItemPembayaranHutang[rowIndex - 1];

                if (itemBayar.entity_state == EntityState.Unchanged)
                    itemBayar.entity_state = EntityState.Modified;
            }

            itemBayar.beli_produk_id = beli.beli_produk_id;
            itemBayar.BeliProduk = beli;
            itemBayar.nominal = nominal;
            itemBayar.keterangan = keterangan;

            _listOfItemPembayaranHutang[rowIndex - 1] = itemBayar;
        }

        private void gridControl_CurrentCellValidated(object sender, EventArgs e)
        {
            var grid = (GridControl)sender;

            GridCurrentCell cc = grid.CurrentCell;

            var itemPembayaran = _listOfItemPembayaranHutang[cc.RowIndex - 1];
            var beli = itemPembayaran.BeliProduk;
            var isValidSisaNota = true;

            if (beli != null)
            {
                switch (cc.ColIndex)
                {
                    case 5: // kolom pembayaran
                        itemPembayaran.nominal = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString());
                        beli.total_pelunasan = itemPembayaran.nominal + beli.total_pelunasan_old;
                        isValidSisaNota = beli.sisa_nota >= 0;

                        if (!isValidSisaNota)
                        {
                            beli.total_pelunasan -= itemPembayaran.nominal;

                            MsgHelper.MsgWarning("Maaf, jumlah pembayaran melebihi sisa hutang");

                            GridListControlHelper.SetCurrentCell(grid, cc.RowIndex, cc.ColIndex);
                            GridListControlHelper.SelectCellText(grid, cc.RowIndex, cc.ColIndex);

                            break;
                        }
                        else
                            GridListControlHelper.SetCurrentCell(grid, cc.RowIndex, cc.ColIndex + 1);

                        break;

                    case 6: // kolom keterangan
                        itemPembayaran.keterangan = cc.Renderer.ControlValue.ToString();
                        break;

                    default:
                        break;
                }

                SetItemBayar(grid, cc.RowIndex, cc.ColIndex, beli, itemPembayaran.nominal, itemPembayaran.keterangan);
                grid.Refresh();
                RefreshTotal();
            }             
        }

        private void FrmEntryPembayaranHutangPembelianProduk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // hapus objek dumm
            if (!_isNewData)
            {
                var itemsToRemove = _pembayaranHutang.item_pembayaran_hutang.Where(f => f.BeliProduk == null && f.entity_state == EntityState.Added)
                                                                            .ToArray();   
                foreach (var item in itemsToRemove)
                {
                    _pembayaranHutang.item_pembayaran_hutang.Remove(item);
                }
            }
        }        
    }
}
