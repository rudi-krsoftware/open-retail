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

namespace OpenRetail.App.Pengeluaran
{
    public partial class FrmEntryPengeluaranBiaya : FrmEntryStandard, IListener
    {
        private IPengeluaranBiayaBll _bll = null;
        private PengeluaranBiaya _pengeluaran = null;
        private IList<ItemPengeluaranBiaya> _listOfItemPengeluaran = new List<ItemPengeluaranBiaya>();
        private IList<ItemPengeluaranBiaya> _listOfItemPengeluaranOld = new List<ItemPengeluaranBiaya>();
        private IList<ItemPengeluaranBiaya> _listOfItemPengeluaranDeleted = new List<ItemPengeluaranBiaya>();
                
        private int _rowIndex = 0;
        private int _colIndex = 0;

        private bool _isNewData = false;
        private ILog _log;
        private Pengguna _pengguna;

        public IListener Listener { private get; set; }

        public FrmEntryPengeluaranBiaya(string header, IPengeluaranBiayaBll bll) 
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

            _listOfItemPengeluaran.Add(new ItemPengeluaranBiaya()); // add dummy objek

            InitGridControl(gridControl);
        }

        public FrmEntryPengeluaranBiaya(string header, PengeluaranBiaya pengeluaran, IPengeluaranBiayaBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._pengeluaran = pengeluaran;
            this._log = MainProgram.log;
            this._pengguna = MainProgram.pengguna;

            txtNota.Text = this._pengeluaran.nota;
            dtpTanggal.Value = (DateTime)this._pengeluaran.tanggal;

            txtKeterangan.Text = this._pengeluaran.keterangan;

            // simpan data lama
            _listOfItemPengeluaranOld.Clear();
            foreach (var item in this._pengeluaran.item_pengeluaran_biaya)
            {
                _listOfItemPengeluaranOld.Add(new ItemPengeluaranBiaya
                {
                    item_pengeluaran_id = item.item_pengeluaran_id,
                    jumlah = item.jumlah,
                    harga = item.harga
                });
            }
            
            _listOfItemPengeluaran = this._pengeluaran.item_pengeluaran_biaya;
            _listOfItemPengeluaran.Add(new ItemPengeluaranBiaya()); // add dummy objek

            InitGridControl(gridControl);

            RefreshTotal();
        }

        private void InitGridControl(GridControl grid)
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jenis Pengeluaran", Width = 350 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jumlah", Width = 50 });
            gridListProperties.Add(new GridListControlProperties { Header = "Harga", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sub Total", Width = 110 });
            gridListProperties.Add(new GridListControlProperties { Header = "Aksi" });

            GridListControlHelper.InitializeGridListControl<ItemPengeluaranBiaya>(grid, _listOfItemPengeluaran, gridListProperties);

            grid.PushButtonClick += delegate(object sender, GridCellPushButtonClickEventArgs e)
            {
                if (e.ColIndex == 6)
                {
                    if (grid.RowCount == 1)
                    {
                        MsgHelper.MsgWarning("Minimal 1 item pengeluaran harus diinputkan !");
                        return;
                    }

                    if (MsgHelper.MsgDelete())
                    {
                        var itemPengeluaran = _listOfItemPengeluaran[e.RowIndex - 1];
                        itemPengeluaran.entity_state = EntityState.Deleted;

                        _listOfItemPengeluaranDeleted.Add(itemPengeluaran);
                        _listOfItemPengeluaran.Remove(itemPengeluaran);

                        grid.RowCount = _listOfItemPengeluaran.Count();
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
                    if (!(_listOfItemPengeluaran.Count > 0))
                        return;

                    var itemPengeluaran = _listOfItemPengeluaran[e.RowIndex - 1];
                    var jenisPengeluaran = itemPengeluaran.JenisPengeluaran;

                    if (e.RowIndex % 2 == 0)
                        e.Style.BackColor = ColorCollection.BACK_COLOR_ALTERNATE;

                    switch (e.ColIndex)
                    {
                        case 1: // no urut
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.Enabled = false;
                            e.Style.CellValue = e.RowIndex.ToString();
                            break;

                        case 2: // nama jenis pengeluaran
                            if (jenisPengeluaran != null)
                                e.Style.CellValue = jenisPengeluaran.nama_jenis_pengeluaran;

                            break;

                        case 3: // jumlah
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.CellValue = itemPengeluaran.jumlah;

                            break;

                        case 4: // harga
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                            e.Style.CellValue = NumberHelper.NumberToString(itemPengeluaran.harga);

                            break;

                        case 5: // subtotal
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                            e.Style.Enabled = false;
                            e.Style.CellValue = NumberHelper.NumberToString(itemPengeluaran.jumlah * itemPengeluaran.harga);
                            break;

                        case 6: // button hapus
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

        private double SumGrid(IList<ItemPengeluaranBiaya> listOfItemPengeluaran)
        {
            double total = 0;
            foreach (var item in _listOfItemPengeluaran.Where(f => f.JenisPengeluaran != null))
            {
                total += item.harga * item.jumlah;
            }

            return total;
        }

        private void RefreshTotal()
        {
            lblTotal.Text = NumberHelper.NumberToString(SumGrid(_listOfItemPengeluaran));
        }

        protected override void Simpan()
        {
            var total = SumGrid(this._listOfItemPengeluaran);
            if (!(total > 0))
            {
                MsgHelper.MsgWarning("Anda belum melengkapi inputan data produk !");
                return;
            }

            if (!MsgHelper.MsgKonfirmasi("Apakah proses ingin dilanjutkan ?"))
                return;

            if (_isNewData)
                _pengeluaran = new PengeluaranBiaya();

            _pengeluaran.pengguna_id = this._pengguna.pengguna_id;
            _pengeluaran.Pengguna = this._pengguna;
            _pengeluaran.nota = txtNota.Text;
            _pengeluaran.tanggal = dtpTanggal.Value;
            _pengeluaran.keterangan = txtKeterangan.Text;

            _pengeluaran.item_pengeluaran_biaya = this._listOfItemPengeluaran.Where(f => f.JenisPengeluaran != null).ToList();

            if (!_isNewData) // update
                _pengeluaran.item_pengeluaran_biaya_deleted = _listOfItemPengeluaranDeleted;

            var result = 0;
            var validationError = new ValidationError();

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                if (_isNewData)
                {
                    result = _bll.Save(_pengeluaran, ref validationError);
                }
                else
                {
                    result = _bll.Update(_pengeluaran, ref validationError);
                }

                if (result > 0)
                {
                    Listener.Ok(this, _isNewData, _pengeluaran);

                    _listOfItemPengeluaran.Clear();
                    _listOfItemPengeluaranDeleted.Clear();                                        

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
                var itemsModified = this._pengeluaran.item_pengeluaran_biaya.Where(f => f.JenisPengeluaran != null && f.entity_state == EntityState.Modified)
                                                     .ToArray();

                foreach (var item in itemsModified)
                {
                    var itemPengeluaran = _listOfItemPengeluaranOld.Where(f => f.item_pengeluaran_id == item.item_pengeluaran_id)
                                                                   .SingleOrDefault();

                    if (itemPengeluaran != null)
                    {
                        item.jumlah = itemPengeluaran.jumlah;
                        item.harga = itemPengeluaran.harga;
                    }
                }

                // restore item yang di delete
                var itemsDeleted = _listOfItemPengeluaranDeleted.Where(f => f.JenisPengeluaran != null && f.entity_state == EntityState.Deleted)
                                                                .ToArray();
                foreach (var item in itemsDeleted)
                {
                    item.entity_state = EntityState.Unchanged;
                    this._pengeluaran.item_pengeluaran_biaya.Add(item);
                }

                _listOfItemPengeluaranDeleted.Clear();
            }

            base.Selesai();
        }

        public void Ok(object sender, object data)
        {
            if (data is JenisPengeluaran) // pencarian jenis pengeluaran
            {
                var jenisPengeluaran = (JenisPengeluaran)data;

                if (!IsExist(jenisPengeluaran.jenis_pengeluaran_id))
                {
                    SetItemJenisPengeluaran(this.gridControl, _rowIndex, _colIndex + 1, jenisPengeluaran);
                    this.gridControl.Refresh();
                    RefreshTotal();

                    GridListControlHelper.SetCurrentCell(this.gridControl, _rowIndex, _colIndex + 1);
                }
                else
                {
                    MsgHelper.MsgWarning("Data jenis pengeluaran sudah diinputkan");
                    GridListControlHelper.SelectCellText(this.gridControl, _rowIndex, _colIndex);
                }
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

        private bool IsExist(string jenisPengeluaranId)
        {
            var count = _listOfItemPengeluaran.Where(f => f.jenis_pengeluaran_id != null && f.jenis_pengeluaran_id.ToLower() == jenisPengeluaranId.ToLower())
                                              .Count();

            return (count > 0);
        }

        private void SetItemJenisPengeluaran(GridControl grid, int rowIndex, int colIndex, JenisPengeluaran jenisPengeluaran, double jumlah = 1, double harga = 0)
        {
            ItemPengeluaranBiaya itemPengeluaranBiaya;

            if (_isNewData)
            {
                itemPengeluaranBiaya = new ItemPengeluaranBiaya();
            }
            else
            {
                itemPengeluaranBiaya = _listOfItemPengeluaran[rowIndex - 1];

                if (itemPengeluaranBiaya.entity_state == EntityState.Unchanged)
                    itemPengeluaranBiaya.entity_state = EntityState.Modified;
            }

            itemPengeluaranBiaya.jenis_pengeluaran_id = jenisPengeluaran.jenis_pengeluaran_id;
            itemPengeluaranBiaya.JenisPengeluaran = jenisPengeluaran;
            itemPengeluaranBiaya.jumlah = jumlah;
            itemPengeluaranBiaya.harga = harga;

            _listOfItemPengeluaran[rowIndex - 1] = itemPengeluaranBiaya;
        }

        private void gridControl_CurrentCellKeyDown(object sender, KeyEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                var grid = (GridControl)sender;

                var rowIndex = grid.CurrentCell.RowIndex;
                var colIndex = grid.CurrentCell.ColIndex;
                
                JenisPengeluaran jenisPengeluaran = null;

                switch (colIndex)
                {
                    case 2: // pencarian berdasarkan nama jenis pengeluaran

                        GridCurrentCell cc = grid.CurrentCell;
                        var namaProduk = cc.Renderer.ControlValue.ToString();

                        IJenisPengeluaranBll bll = new JenisPengeluaranBll(_log);
                        var listOfJenisPengeluaran = bll.GetByName(namaProduk);

                        if (listOfJenisPengeluaran.Count == 0)
                        {
                            MsgHelper.MsgWarning("Data jenis pengeluaran tidak ditemukan");
                            GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                        }
                        else if (listOfJenisPengeluaran.Count == 1)
                        {
                            jenisPengeluaran = listOfJenisPengeluaran[0];

                            if (!IsExist(jenisPengeluaran.jenis_pengeluaran_id))
                            {
                                SetItemJenisPengeluaran(grid, rowIndex, colIndex, jenisPengeluaran);
                                grid.Refresh();
                                RefreshTotal();

                                GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 1);
                            }
                            else
                            {
                                MsgHelper.MsgWarning("Data jenis pengeluaran sudah diinputkan");
                                GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex);
                            }
                        }
                        else // data lebih dari satu, tampilkan form lookup
                        {
                            _rowIndex = rowIndex;
                            _colIndex = colIndex;

                            var frmLookup = new FrmLookupReferensi("Data Jenis Pengeluaran", listOfJenisPengeluaran);
                            frmLookup.Listener = this;
                            frmLookup.ShowDialog();
                        }

                        break;

                    case 3:
                        GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 1);
                        break;

                    case 4:
                        if (grid.RowCount == rowIndex)
                        {
                            _listOfItemPengeluaran.Add(new ItemPengeluaranBiaya());
                            grid.RowCount = _listOfItemPengeluaran.Count;
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
                case 5: // harga
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

            var itemPengeluaran = _listOfItemPengeluaran[cc.RowIndex - 1];
            var jenisPengeluaran = itemPengeluaran.JenisPengeluaran;

            if (jenisPengeluaran != null)
            {
                switch (cc.ColIndex)
                {
                    case 3: // kolom jumlah
                        itemPengeluaran.jumlah = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);
                        break;

                    case 4: // kolom harga
                        itemPengeluaran.harga = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString());
                        break;

                    default:
                        break;
                }

                SetItemJenisPengeluaran(grid, cc.RowIndex, cc.ColIndex, jenisPengeluaran, itemPengeluaran.jumlah, itemPengeluaran.harga);
                grid.Refresh();

                RefreshTotal();
            }           
        }

        private void ShowEntryJenisPengeluaran()
        {
            var isGrant = RolePrivilegeHelper.IsHaveHakAkses("mnuJenisPengeluaran", _pengguna);
            if (!isGrant)
            {
                MsgHelper.MsgWarning("Maaf Anda tidak mempunyai otoritas untuk mengakses menu ini");
                return;
            }

            IJenisPengeluaranBll jenisPengeluaranBll = new JenisPengeluaranBll(_log);
            var frmEntryJenisPengeluaran = new FrmEntryJenisPengeluaran("Tambah Data Jenis Biaya", jenisPengeluaranBll);
            frmEntryJenisPengeluaran.Listener = this;
            frmEntryJenisPengeluaran.ShowDialog();
        }

        private void FrmEntryPengeluaranBiaya_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyPressHelper.IsShortcutKey(Keys.F1, e)) // tambah data jenis pengeluaran
            {
                ShowEntryJenisPengeluaran();
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

        private void FrmEntryPengeluaranBiaya_FormClosing(object sender, FormClosingEventArgs e)
        {
            // hapus objek dumm
            if (!_isNewData)
            {
                var itemsToRemove = _pengeluaran.item_pengeluaran_biaya.Where(f => f.JenisPengeluaran == null && f.entity_state == EntityState.Added)
                                                   .ToArray();

                foreach (var item in itemsToRemove)
                {
                    _pengeluaran.item_pengeluaran_biaya.Remove(item);
                }
            }
        }

        private void txtPPN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
