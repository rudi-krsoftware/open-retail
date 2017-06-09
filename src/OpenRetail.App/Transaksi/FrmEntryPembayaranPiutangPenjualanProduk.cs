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
    public partial class FrmEntryPembayaranPiutangPenjualanProduk : FrmEntryStandard, IListener
    {        
        private IPembayaranPiutangProdukBll _bll = null;
        private PembayaranPiutangProduk _pembayaranPiutang = null;
        private Customer _customer = null;
        private IList<ItemPembayaranPiutangProduk> _listOfItemPembayaranPiutang = new List<ItemPembayaranPiutangProduk>();
        private IList<ItemPembayaranPiutangProduk> _listOfItemPembayaranPiutangOld = new List<ItemPembayaranPiutangProduk>();
        private IList<ItemPembayaranPiutangProduk> _listOfItemPembayaranPiutangDeleted = new List<ItemPembayaranPiutangProduk>();
        
        private int _rowIndex = 0;
        private int _colIndex = 0;

        private bool _isNewData = false;
        private ILog _log;
        private Pengguna _pengguna;

        public IListener Listener { private get; set; }

        public FrmEntryPembayaranPiutangPenjualanProduk(string header, IPembayaranPiutangProdukBll bll) 
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

            _listOfItemPembayaranPiutang.Add(new ItemPembayaranPiutangProduk()); // add dummy objek
            
            InitGridControl(gridControl);
        }

        public FrmEntryPembayaranPiutangPenjualanProduk(string header, PembayaranPiutangProduk pembayaranPiutang, IPembayaranPiutangProdukBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._pembayaranPiutang = pembayaranPiutang;
            this._customer = pembayaranPiutang.Customer;
            this._log = MainProgram.log;
            this._pengguna = MainProgram.pengguna;

            txtNota.Text = this._pembayaranPiutang.nota;
            dtpTanggal.Value = (DateTime)this._pembayaranPiutang.tanggal;
            
            txtCustomer.Text = this._customer.nama_customer;
            txtKeterangan.Text = this._pembayaranPiutang.keterangan;
            
            // simpan data lama
            _listOfItemPembayaranPiutangOld.Clear();
            foreach (var item in this._pembayaranPiutang.item_pembayaran_piutang)
            {
                _listOfItemPembayaranPiutangOld.Add(new ItemPembayaranPiutangProduk
                {
                    item_pembayaran_piutang_id = item.item_pembayaran_piutang_id,
                    nominal = item.nominal,
                    keterangan = item.keterangan
                });
            }
            
            _listOfItemPembayaranPiutang = this._pembayaranPiutang.item_pembayaran_piutang;
            _listOfItemPembayaranPiutang.Add(new ItemPembayaranPiutangProduk()); // add dummy objek

            InitGridControl(gridControl);

            RefreshTotal();
        }

        private void InitGridControl(GridControl grid)
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nota Jual", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Total", Width = 100, IsEditable = false, HorizontalAlignment = GridHorizontalAlignment.Right });
            gridListProperties.Add(new GridListControlProperties { Header = "Kekurangan", Width = 100, IsEditable = false, HorizontalAlignment = GridHorizontalAlignment.Right });
            gridListProperties.Add(new GridListControlProperties { Header = "Pembayaran", Width = 100, HorizontalAlignment = GridHorizontalAlignment.Right });
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan", Width = 200 });
            gridListProperties.Add(new GridListControlProperties { Header = "Aksi" });

            GridListControlHelper.InitializeGridListControl<ItemPembayaranPiutangProduk>(grid, _listOfItemPembayaranPiutang, gridListProperties);

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
                        var pembayaranPiutang = _listOfItemPembayaranPiutang[e.RowIndex - 1];
                        pembayaranPiutang.entity_state = EntityState.Deleted;

                        _listOfItemPembayaranPiutangDeleted.Add(pembayaranPiutang);
                        _listOfItemPembayaranPiutang.Remove(pembayaranPiutang);

                        grid.RowCount = _listOfItemPembayaranPiutang.Count();
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
                    if (!(_listOfItemPembayaranPiutang.Count > 0))
                        return;

                    double grand_total = 0;
                    double sisaNota = 0;

                    var itemPembayaran = _listOfItemPembayaranPiutang[e.RowIndex - 1];
                    var jual = itemPembayaran.JualProduk;
                    if (jual != null)
                    {
                        grand_total = jual.grand_total; ;
                        sisaNota = jual.sisa_nota;
                    }

                    switch (e.ColIndex)
                    {
                        case 1: // no urut
                            e.Style.CellValue = e.RowIndex.ToString();
                            break;

                        case 2: // nota jual
                            if (jual != null)
                                e.Style.CellValue = jual.nota;

                            if (jual != null)
                            {
                                if (jual.tanggal_tempo.IsNull()) // nota tunai nominalnya tidak bisa diedit
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
                            if (jual != null)
                            {
                                if (jual.tanggal_tempo.IsNull()) // nota tunai nominalnya tidak bisa diedit
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

                            if (jual != null)
                            {
                                e.Style.Enabled = !jual.tanggal_tempo.IsNull();
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

        private double SumGrid(IList<ItemPembayaranPiutangProduk> listOfItemPembayaranPiutang)
        {
            double total = 0;
            foreach (var item in listOfItemPembayaranPiutang.Where(f => f.JualProduk != null))
            {
                total += item.nominal;
            }

            return total;
        }

        private void RefreshTotal()
        {
            lblTotal.Text = NumberHelper.NumberToString(SumGrid(_listOfItemPembayaranPiutang));
        }

        protected override void Simpan()
        {
            if (this._customer == null || txtCustomer.Text.Length == 0)
            {
                MsgHelper.MsgWarning("'Customer' tidak boleh kosong !");
                txtCustomer.Focus();

                return;
            }

            var total = SumGrid(this._listOfItemPembayaranPiutang);
            if (!(total > 0))
            {
                MsgHelper.MsgWarning("Anda belum melengkapi inputan data pembayaran !");
                return;
            }

            if (!MsgHelper.MsgKonfirmasi("Apakah proses ingin dilanjutkan ?"))
                return;

            if (_isNewData)
                _pembayaranPiutang = new PembayaranPiutangProduk();

            _pembayaranPiutang.pengguna_id = this._pengguna.pengguna_id;
            _pembayaranPiutang.Pengguna = this._pengguna;
            _pembayaranPiutang.customer_id = this._customer.customer_id;
            _pembayaranPiutang.Customer = this._customer;
            _pembayaranPiutang.nota = txtNota.Text;
            _pembayaranPiutang.tanggal = dtpTanggal.Value;
            _pembayaranPiutang.keterangan = txtKeterangan.Text;

            _pembayaranPiutang.item_pembayaran_piutang = this._listOfItemPembayaranPiutang.Where(f => f.JualProduk != null).ToList();

            if (!_isNewData) // update
                _pembayaranPiutang.item_pembayaran_piutang_deleted = _listOfItemPembayaranPiutangDeleted;

            var result = 0;
            var validationError = new ValidationError();

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                if (_isNewData)
                {
                    result = _bll.Save(_pembayaranPiutang, false, ref validationError);
                }
                else
                {
                    result = _bll.Update(_pembayaranPiutang, false, ref validationError);
                }

                if (result > 0)
                {
                    Listener.Ok(this, _isNewData, _pembayaranPiutang);

                    _customer = null;
                    _listOfItemPembayaranPiutang.Clear();
                    _listOfItemPembayaranPiutangDeleted.Clear();                                        

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
                var itemsModified = _pembayaranPiutang.item_pembayaran_piutang.Where(f => f.JualProduk != null && f.entity_state == EntityState.Modified)
                                                     .ToArray();

                foreach (var item in itemsModified)
                {
                    var itemPembayaran = _listOfItemPembayaranPiutangOld.Where(f => f.item_pembayaran_piutang_id == item.item_pembayaran_piutang_id)
                                                                       .SingleOrDefault();                    
                    if (itemPembayaran != null)
                    {
                        item.nominal = itemPembayaran.nominal;
                        item.keterangan = itemPembayaran.keterangan;
                    }
                }

                // restore item yang di delete
                var itemsDeleted = _listOfItemPembayaranPiutangDeleted.Where(f => f.JualProduk != null && f.entity_state == EntityState.Deleted)
                                                                     .ToArray();
                foreach (var item in itemsDeleted)
                {
                    item.entity_state = EntityState.Unchanged;
                    this._pembayaranPiutang.item_pembayaran_piutang.Add(item);
                }

                _listOfItemPembayaranPiutangDeleted.Clear();
            }

            base.Selesai();
        }

        public void Ok(object sender, object data)
        {
            if (data is JualProduk) // pencarian nota
            {
                var jual = (JualProduk)data;

                if (!IsExist(jual.nota))
                {
                    SetItemBayar(this.gridControl, _rowIndex, _colIndex + 1, jual);
                    this.gridControl.Refresh();

                    GridListControlHelper.SetCurrentCell(this.gridControl, _rowIndex, 5); // kolom pembayaran
                }
                else
                {
                    MsgHelper.MsgWarning("Data nota sudah diinputkan");
                    GridListControlHelper.SelectCellText(this.gridControl, _rowIndex, _colIndex);
                }
            }
            else if (data is Customer) // pencarian customer
            {
                this._customer = (Customer)data;
                txtCustomer.Text = this._customer.nama_customer;
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
                    MsgHelper.MsgWarning("Data customer tidak ditemukan");
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

        private bool IsExist(string nota)
        {
            var count = _listOfItemPembayaranPiutang.Where(f => f.JualProduk != null && f.JualProduk.nota.ToLower() == nota.ToLower())
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
                        if (this._customer == null || txtCustomer.Text.Length == 0)
                        {
                            MsgHelper.MsgWarning("Nama Customer belum diinputkan");
                            txtCustomer.Focus();
                            return;
                        }

                        var cc = grid.CurrentCell;
                        var nota = cc.Renderer.ControlValue.ToString();

                        IList<JualProduk> listOfJual = null;
                        IJualProdukBll bll = new JualProdukBll(_log);

                        if (nota.Length > 0) // menampilkan nota kredit berdasarkan nota
                        {
                            listOfJual = bll.GetNotaKreditByNota(this._customer.customer_id, nota);
                        }
                        else
                        {
                            listOfJual = bll.GetNotaKreditByCustomer(this._customer.customer_id, false);
                        }

                        if (listOfJual.Count == 0)
                        {
                            MsgHelper.MsgWarning("Data penjualan kredit tidak ditemukan");
                            GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                        }
                        else if (listOfJual.Count == 1)
                        {
                            var jual = listOfJual[0];

                            if (!IsExist(jual.nota))
                            {
                                SetItemBayar(grid, rowIndex, colIndex, jual);
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

                            var frmLookup = new FrmLookupNota("Data Nota Penjualan", listOfJual);
                            frmLookup.Listener = this;
                            frmLookup.ShowDialog();
                        }

                        break;

                    case 6: // keterangan
                        if (grid.RowCount == rowIndex)
                        {
                            _listOfItemPembayaranPiutang.Add(new ItemPembayaranPiutangProduk());
                            grid.RowCount = _listOfItemPembayaranPiutang.Count;
                        }

                        GridListControlHelper.SetCurrentCell(grid, rowIndex + 1, 2); // fokus ke kolom nota jual
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

        private void SetItemBayar(GridControl grid, int rowIndex, int colIndex, JualProduk jual, double nominal = 0, string keterangan = "")
        {
            ItemPembayaranPiutangProduk itemBayar;

            if (_isNewData)
            {
                itemBayar = new ItemPembayaranPiutangProduk();
            }
            else
            {
                itemBayar = _listOfItemPembayaranPiutang[rowIndex - 1];

                if (itemBayar.entity_state == EntityState.Unchanged)
                    itemBayar.entity_state = EntityState.Modified;
            }

            itemBayar.jual_id = jual.jual_id;
            itemBayar.JualProduk = jual;
            itemBayar.nominal = nominal;
            itemBayar.keterangan = keterangan;

            _listOfItemPembayaranPiutang[rowIndex - 1] = itemBayar;
        }

        private void gridControl_CurrentCellValidated(object sender, EventArgs e)
        {
            var grid = (GridControl)sender;

            GridCurrentCell cc = grid.CurrentCell;

            var itemPembayaran = _listOfItemPembayaranPiutang[cc.RowIndex - 1];
            var jual = itemPembayaran.JualProduk;
            var isValidSisaNota = true;

            if (jual != null)
            {
                switch (cc.ColIndex)
                {
                    case 5: // kolom pembayaran
                        itemPembayaran.nominal = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString());
                        jual.total_pelunasan = itemPembayaran.nominal + jual.total_pelunasan_old;
                        isValidSisaNota = jual.sisa_nota >= 0;

                        if (!isValidSisaNota)
                        {
                            jual.total_pelunasan -= itemPembayaran.nominal;

                            MsgHelper.MsgWarning("Maaf, jumlah pembayaran melebihi sisa piutang");

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

                SetItemBayar(grid, cc.RowIndex, cc.ColIndex, jual, itemPembayaran.nominal, itemPembayaran.keterangan);
                grid.Refresh();
                RefreshTotal();
            }             
        }

        private void FrmEntryPembayaranPiutangPenjualanProduk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // hapus objek dumm
            if (!_isNewData)
            {
                var itemsToRemove = _pembayaranPiutang.item_pembayaran_piutang.Where(f => f.JualProduk == null && f.entity_state == EntityState.Added)
                                                                            .ToArray();   
                foreach (var item in itemsToRemove)
                {
                    _pembayaranPiutang.item_pembayaran_piutang.Remove(item);
                }
            }
        }        
    }
}
