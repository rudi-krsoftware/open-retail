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
using OpenRetail.Helper.UI.Template;
using OpenRetail.Helper;
using Syncfusion.Windows.Forms.Grid;
using ConceptCave.WaitCursor;
using log4net;

namespace OpenRetail.App.Transaksi
{
    public partial class FrmListPembelianProduk : FrmListEmptyBody, IListener
    {
        private IBeliProdukBll _bll; // deklarasi objek business logic layer 
        private IList<BeliProduk> _listOfBeli = new List<BeliProduk>();
        private ILog _log;
        private Pengguna _pengguna;
        private string _menuId;

        public FrmListPembelianProduk(string header, Pengguna pengguna, string menuId)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.WindowState = FormWindowState.Maximized;

            _log = MainProgram.log;
            _bll = new BeliProdukBll(_log);
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
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfBeli.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tempo", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nota", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Supplier", Width = 300 });
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan", Width = 300 });
            gridListProperties.Add(new GridListControlProperties { Header = "Hutang", Width = 150 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sisa Hutang" });

            GridListControlHelper.InitializeGridListControl<BeliProduk>(this.gridList, _listOfBeli, gridListProperties);

            if (_listOfBeli.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {

                if (_listOfBeli.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfBeli.Count)
                        {
                            double totalNota = 0;

                            var beli = _listOfBeli[rowIndex];
                            if (beli != null)
                                totalNota = beli.grand_total;


                            var isRetur = beli.retur_beli_produk_id != null;

                            if (isRetur)
                                e.Style.BackColor = Color.Red;

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(beli.tanggal);
                                    break;

                                case 3:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(beli.tanggal_tempo);
                                    break;

                                case 4:
                                    e.Style.CellValue = beli.nota;
                                    break;

                                case 5:
                                    if (beli.Supplier != null)
                                        e.Style.CellValue = beli.Supplier.nama_supplier;

                                    break;

                                case 6:
                                    e.Style.CellValue = beli.keterangan;
                                    break;

                                case 7:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(totalNota);
                                    break;

                                case 8:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(totalNota - beli.total_pelunasan);
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
                _listOfBeli = _bll.GetAll();
                GridListControlHelper.Refresh<BeliProduk>(this.gridList, _listOfBeli);
            }

            ResetButton();
        }

        private void LoadData(string supplierName)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfBeli = _bll.GetByName(supplierName);
                GridListControlHelper.Refresh<BeliProduk>(this.gridList, _listOfBeli);
            }

            ResetButton();
        }

        private void LoadData(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfBeli = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);
                GridListControlHelper.Refresh<BeliProduk>(this.gridList, _listOfBeli);
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfBeli.Count > 0);

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfBeli.Count);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryPembelianProduk("Tambah Data " + this.Text, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var beli = _listOfBeli[index];
            beli.tanggal_tempo_old = beli.tanggal_tempo;

            LogicalThreadContext.Properties["OldValue"] = beli.ToJson();

            var frm = new FrmEntryPembelianProduk("Edit Data " + this.Text, beli, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Hapus()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.Text))
                return;

            if (MsgHelper.MsgDelete())
            {
                var beli = _listOfBeli[index];

                var result = _bll.Delete(beli);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<BeliProduk>(this.gridList, _listOfBeli, beli);
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
            var beli = (BeliProduk)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<BeliProduk>(this.gridList, _listOfBeli, beli);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<BeliProduk>(this.gridList, _listOfBeli, beli);
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
