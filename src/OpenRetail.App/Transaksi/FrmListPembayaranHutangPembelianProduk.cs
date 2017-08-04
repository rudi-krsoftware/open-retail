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
using log4net;

namespace OpenRetail.App.Transaksi
{
    public partial class FrmListPembayaranHutangPembelianProduk : FrmListEmptyBody, IListener
    {
        private IPembayaranHutangProdukBll _bll; // deklarasi objek business logic layer 
        private IList<PembayaranHutangProduk> _listOfPembayaranHutang = new List<PembayaranHutangProduk>();
        private ILog _log;
        private Pengguna _pengguna;
        private string _menuId;

        public FrmListPembayaranHutangPembelianProduk(string header, Pengguna pengguna, string menuId)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.WindowState = FormWindowState.Maximized;

            _log = MainProgram.log;
            _bll = new PembayaranHutangProdukBll(_log);
            _pengguna = pengguna;
            _menuId = menuId;

            // set hak akses untuk SELECT
            var role = _pengguna.GetRoleByMenuAndGrant(_menuId, GrantState.SELECT);
            if (role != null)
            {
                if (role.is_grant)
                    LoadData(filterRangeTanggal.TanggalMulai, filterRangeTanggal.TanggalSelesai);

                txtNamaSupplier.Enabled = role.is_grant;
                btnCari.Enabled = role.is_grant;

                filterRangeTanggal.Enabled = role.is_grant;
            }

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfPembayaranHutang.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nota Pembayaran", Width = 120 });
            gridListProperties.Add(new GridListControlProperties { Header = "Supplier", Width = 300 });
            gridListProperties.Add(new GridListControlProperties { Header = "Pembayaran", Width = 150 });
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan" });            

            GridListControlHelper.InitializeGridListControl<PembayaranHutangProduk>(this.gridList, _listOfPembayaranHutang, gridListProperties);

            if (_listOfPembayaranHutang.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {

                if (_listOfPembayaranHutang.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfPembayaranHutang.Count)
                        {
                            var pembayaran = _listOfPembayaranHutang[rowIndex];
                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(pembayaran.tanggal);
                                    break;

                                case 3:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = pembayaran.nota;
                                    break;

                                case 4:
                                    var supplier = pembayaran.Supplier;

                                    if (supplier != null)
                                        e.Style.CellValue = supplier.nama_supplier;

                                    break;

                                case 5:
                                    var total = pembayaran.item_pembayaran_hutang.Sum(f => f.nominal);

                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(total);

                                    break;

                                case 6:
                                    e.Style.CellValue = pembayaran.keterangan;
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

        private void LoadData()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfPembayaranHutang = _bll.GetAll();
                GridListControlHelper.Refresh<PembayaranHutangProduk>(this.gridList, _listOfPembayaranHutang);
            }

            ResetButton();
        }

        private void LoadData(string supplierName)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfPembayaranHutang = _bll.GetByName(supplierName);
                GridListControlHelper.Refresh<PembayaranHutangProduk>(this.gridList, _listOfPembayaranHutang);
            }

            ResetButton();
        }

        private void LoadData(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfPembayaranHutang = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);
                GridListControlHelper.Refresh<PembayaranHutangProduk>(this.gridList, _listOfPembayaranHutang);
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfPembayaranHutang.Count > 0);

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfPembayaranHutang.Count);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryPembayaranHutangPembelianProduk("Tambah Data " + this.Text, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var pembayaran = _listOfPembayaranHutang[index];

            LogicalThreadContext.Properties["OldValue"] = pembayaran.ToJson();

            var frm = new FrmEntryPembayaranHutangPembelianProduk("Edit Data " + this.Text, pembayaran, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Hapus()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.Text))
                return;

            var pembayaran = _listOfPembayaranHutang[index];
            if (pembayaran.is_tunai)
            {
                MsgHelper.MsgWarning("Maaf pembayaran hutang pembelian tunai tidak bisa dihapus");
                return;
            }

            if (MsgHelper.MsgDelete())
            {                
                var result = _bll.Delete(pembayaran);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<PembayaranHutangProduk>(this.gridList, _listOfPembayaranHutang, pembayaran);
                    ResetButton();
                }
                else
                    MsgHelper.MsgDeleteError();
            }
        }

        public void Ok(object sender, object data)
        {
            throw new NotImplementedException();
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            var pembayaran = (PembayaranHutangProduk)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<PembayaranHutangProduk>(this.gridList, _listOfPembayaranHutang, pembayaran);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<PembayaranHutangProduk>(this.gridList, _listOfPembayaranHutang, pembayaran);
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (btnPerbaiki.Enabled)
                Perbaiki();
        }

        private void filterRangeTanggal_BtnTampilkanClicked(object sender, EventArgs e)
        {
            var tanggalMulai = filterRangeTanggal.TanggalMulai;
            var tanggalSelesai = filterRangeTanggal.TanggalSelesai;

            if (!DateTimeHelper.IsValidRangeTanggal(tanggalMulai, tanggalSelesai))
            {
                MsgHelper.MsgNotValidRangeTanggal();
                return;
            }

            txtNamaSupplier.Clear();
            LoadData(tanggalMulai, tanggalSelesai);
        }

        private void filterRangeTanggal_ChkTampilkanSemuaDataClicked(object sender, EventArgs e)
        {
            txtNamaSupplier.Clear();

            var chk = (CheckBox)sender;

            if (chk.Checked)
            {
                LoadData();
                txtNamaSupplier.Enabled = false;
                btnCari.Enabled = false;
            }                
            else
            {
                LoadData(filterRangeTanggal.TanggalMulai, filterRangeTanggal.TanggalSelesai);
                txtNamaSupplier.Enabled = true;
                btnCari.Enabled = true;
            }                
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            if (txtNamaSupplier.Text.Length > 0)
                LoadData(txtNamaSupplier.Text);
        }

        private void txtNamaSupplier_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                btnCari_Click(sender, e);
        }
    }
}
