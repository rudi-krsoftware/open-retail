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

namespace OpenRetail.App.Pengeluaran
{
    public partial class FrmListKasbon : FrmListEmptyBody, IListener
    {                
        private IKasbonBll _bll; // deklarasi objek business logic layer 
        private IList<Kasbon> _listOfKasbon = new List<Kasbon>();
        private IList<PembayaranKasbon> _listOfHistoriPembayaranKasbon = new List<PembayaranKasbon>();
        private IList<Karyawan> _listOfKaryawan = new List<Karyawan>();
        private ILog _log;
        private Pengguna _pengguna;
        private string _menuId;
        
        public FrmListKasbon(string header, Pengguna pengguna, string menuId)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            base.WindowState = FormWindowState.Maximized;
            ColorManagerHelper.SetTheme(this, this);            

            _log = MainProgram.log;
            _bll = new KasbonBll(_log);
            _pengguna = pengguna;
            _menuId = menuId;

            // set hak akses untuk SELECT
            var role = _pengguna.GetRoleByMenuAndGrant(_menuId, GrantState.SELECT);
            if (role != null)
            {
                if (role.is_grant)
                {
                    LoadData(filterRangeTanggal.TanggalMulai, filterRangeTanggal.TanggalSelesai);
                    LoadDataKaryawan();
                }                    

                filterRangeTanggal.Enabled = role.is_grant;
            }            

            InitGridList();
            InitGridListHistoriPembayaran();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfKasbon.Count);
        }        

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nota", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Karyawan", Width = 230 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jumlah", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sisa", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan" });

            GridListControlHelper.InitializeGridListControl<Kasbon>(this.gridList, _listOfKasbon, gridListProperties);

            if (_listOfKasbon.Count > 0)
            {
                this.gridList.SetSelected(0, true);
                GridListHandleSelectionChanged(this.gridList);
            }                

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {

                if (_listOfKasbon.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfKasbon.Count)
                        {
                            var kasbon = _listOfKasbon[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(kasbon.tanggal);
                                    break;

                                case 3:
                                    e.Style.CellValue = kasbon.nota;
                                    break;

                                case 4:
                                    var karyawan = kasbon.Karyawan;
                                    if (karyawan != null)
                                        e.Style.CellValue = kasbon.Karyawan.nama_karyawan;

                                    break;

                                case 5:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(kasbon.nominal);
                                    break;

                                case 6:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(kasbon.sisa);
                                    break;

                                case 7:
                                    e.Style.CellValue = kasbon.keterangan;
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

            this.gridList.SelectedValueChanged += delegate(object sender, EventArgs e)
            {
                GridListHandleSelectionChanged((GridListControl)sender);
            };
        }

        private void GridListHandleSelectionChanged(GridListControl gridList)
        {
            if (gridList.SelectedIndex < 0)
                return;

            if (_listOfKasbon.Count > 0)
            {
                var kasbon = _listOfKasbon[gridList.SelectedIndex];
                if (kasbon != null)
                {
                    _listOfHistoriPembayaranKasbon = kasbon.item_pembayaran_kasbon;
                    GridListControlHelper.Refresh<PembayaranKasbon>(this.gridListHistoriPembayaran, _listOfHistoriPembayaranKasbon);

                    btnTambahPembayaran.Enabled = kasbon.sisa > 0;
                    GridListHistoriPembayaranHandleSelectionChanged(this.gridListHistoriPembayaran);
                }
            }
            else
            {
                _listOfHistoriPembayaranKasbon.Clear();
                GridListControlHelper.Refresh<PembayaranKasbon>(this.gridListHistoriPembayaran, _listOfHistoriPembayaranKasbon);

                ResetButtonHistoriPembayaran(false);
            }
        }

        private void InitGridListHistoriPembayaran()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 70 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nota", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jumlah", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan" });

            GridListControlHelper.InitializeGridListControl<PembayaranKasbon>(this.gridListHistoriPembayaran, _listOfHistoriPembayaranKasbon, gridListProperties);

            if (_listOfHistoriPembayaranKasbon.Count > 0)
                this.gridListHistoriPembayaran.SetSelected(0, true);

            this.gridListHistoriPembayaran.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {

                if (_listOfHistoriPembayaranKasbon.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfHistoriPembayaranKasbon.Count)
                        {
                            var pembayaranKasbon = _listOfHistoriPembayaranKasbon[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(pembayaranKasbon.tanggal);
                                    break;

                                case 3:
                                    e.Style.CellValue = pembayaranKasbon.nota;
                                    break;

                                case 4:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(pembayaranKasbon.nominal);
                                    break;

                                case 5:
                                    e.Style.CellValue = pembayaranKasbon.keterangan;
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

            this.gridListHistoriPembayaran.SelectedValueChanged += delegate(object sender, EventArgs e)
            {
                GridListHistoriPembayaranHandleSelectionChanged((GridListControl)sender);
            };

            this.gridListHistoriPembayaran.DoubleClick += delegate(object sender, EventArgs e)
            {
                if (btnPerbaikiPembayaran.Enabled)
                    btnPerbaikiPembayaran_Click(sender, e);
            };
        }

        private void GridListHistoriPembayaranHandleSelectionChanged(GridListControl gridList)
        {
            if (gridList.SelectedIndex < 0)
                return;

            if (_listOfHistoriPembayaranKasbon.Count > 0)
            {
                ResetButtonHistoriPembayaran(true);

                var pembayaranKasbon = _listOfHistoriPembayaranKasbon[gridList.SelectedIndex];
                if (pembayaranKasbon != null)
                {
                    // nonaktifkan tombol edit dan hapus jika pembayaran kasbon dari gaji
                    ResetButtonHistoriPembayaran(pembayaranKasbon.gaji_karyawan_id == null);
                }
            }            
        }

        private void LoadDataKaryawan()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                IKaryawanBll bll = new KaryawanBll(_log);
                _listOfKaryawan = bll.GetAll();
            }
        }

        private void LoadData()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {                
                _listOfKasbon = _bll.GetAll();
                GridListControlHelper.Refresh<Kasbon>(this.gridList, _listOfKasbon);                
            }

            ResetButton();

            btnTambahPembayaran.Enabled = _listOfKasbon.Count > 0;
            GridListHandleSelectionChanged(this.gridListHistoriPembayaran);
        }

        private void LoadData(bool isLunas)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfKasbon = _bll.GetByStatus(isLunas);
                GridListControlHelper.Refresh<Kasbon>(this.gridList, _listOfKasbon);
            }

            ResetButton();

            btnTambahPembayaran.Enabled = _listOfKasbon.Count > 0;
            GridListHandleSelectionChanged(this.gridListHistoriPembayaran);
        }

        private void LoadData(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfKasbon = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);
                GridListControlHelper.Refresh<Kasbon>(this.gridList, _listOfKasbon);                
            }

            ResetButton();

            btnTambahPembayaran.Enabled = _listOfKasbon.Count > 0;
            GridListHandleSelectionChanged(this.gridListHistoriPembayaran);
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfKasbon.Count > 0);
            
            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfKasbon.Count);
        }

        private void ResetButtonHistoriPembayaran(bool status)
        {
            btnPerbaikiPembayaran.Enabled = status;
            btnHapusPembayaran.Enabled = status;
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryKasbon("Tambah Data " + this.Text, _listOfKaryawan, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var kasbon = _listOfKasbon[index];

            LogicalThreadContext.Properties["OldValue"] = kasbon.ToJson();

            var frm = new FrmEntryKasbon("Edit Data " + this.Text, kasbon, _listOfKaryawan, _bll);
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
                var kasbon = _listOfKasbon[index];

                var result = _bll.Delete(kasbon);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<Kasbon>(this.gridList, _listOfKasbon, kasbon);
                    GridListHandleSelectionChanged(this.gridList);

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
            if (data is Kasbon)
            {
                var kasbon = (Kasbon)data;

                if (isNewData)
                {
                    GridListControlHelper.AddObject<Kasbon>(this.gridList, _listOfKasbon, kasbon);
                    ResetButton();
                    btnTambahPembayaran.Enabled = _listOfKasbon.Count > 0;
                }
                else
                    GridListControlHelper.UpdateObject<Kasbon>(this.gridList, _listOfKasbon, kasbon);
            }            
            else if (data is PembayaranKasbon)
            {
                var index = this.gridList.SelectedIndex;
                var kasbon = _listOfKasbon[index];
                var pembayaranKasbon = (PembayaranKasbon)data;

                if (isNewData)
                {
                    kasbon.total_pelunasan += pembayaranKasbon.nominal;
                    GridListControlHelper.UpdateObject<Kasbon>(this.gridList, _listOfKasbon, kasbon);

                    GridListControlHelper.AddObject<PembayaranKasbon>(this.gridListHistoriPembayaran, kasbon.item_pembayaran_kasbon, pembayaranKasbon);

                    btnTambahPembayaran.Enabled = kasbon.sisa > 0;
                    ResetButtonHistoriPembayaran(_listOfHistoriPembayaranKasbon.Count > 0);
                }
                else
                {
                    kasbon.total_pelunasan -= pembayaranKasbon.old_nominal;
                    kasbon.total_pelunasan += pembayaranKasbon.nominal;

                    btnTambahPembayaran.Enabled = kasbon.sisa > 0;
                    GridListControlHelper.UpdateObject<Kasbon>(this.gridList, _listOfKasbon, kasbon);
                    GridListControlHelper.UpdateObject<PembayaranKasbon>(this.gridListHistoriPembayaran, kasbon.item_pembayaran_kasbon, pembayaranKasbon);
                }                                    
            }
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

            LoadData(tanggalMulai, tanggalSelesai);
        }

        private void filterRangeTanggal_ChkTampilkanSemuaDataClicked(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;

            if (chk.Checked)
                LoadData();
            else
                LoadData(filterRangeTanggal.TanggalMulai, filterRangeTanggal.TanggalSelesai);
        }

        private void btnTambahPembayaran_Click(object sender, EventArgs e)
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var kasbon = _listOfKasbon[index];

            var frm = new FrmEntryPembayaranKasbon("Tambah Data Pembayaran Kasbon", kasbon);
            frm.Listener = this;
            frm.ShowDialog();
        }

        private void btnPerbaikiPembayaran_Click(object sender, EventArgs e)
        {
            var index = this.gridListHistoriPembayaran.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var kasbon = _listOfKasbon[this.gridList.SelectedIndex];           

            var pembayaranKasbon = _listOfHistoriPembayaranKasbon[index];
            pembayaranKasbon.old_nominal = pembayaranKasbon.nominal;

            LogicalThreadContext.Properties["OldValue"] = pembayaranKasbon.ToJson();

            var frm = new FrmEntryPembayaranKasbon("Edit Data Pembayaran Kasbon", kasbon, pembayaranKasbon);
            frm.Listener = this;
            frm.ShowDialog();
        }

        private void btnHapusPembayaran_Click(object sender, EventArgs e)
        {
            var index = this.gridListHistoriPembayaran.SelectedIndex;

            if (!base.IsSelectedItem(index, this.Text))
                return;

            if (MsgHelper.MsgDelete())
            {
                var pembayaranKasbon = _listOfHistoriPembayaranKasbon[index];

                IPembayaranKasbonBll bll = new PembayaranKasbonBll(_log);

                var result = bll.Delete(pembayaranKasbon);
                if (result > 0)
                {
                    var kasbon = _listOfKasbon[this.gridList.SelectedIndex];

                    kasbon.total_pelunasan -= pembayaranKasbon.nominal;
                    kasbon.item_pembayaran_kasbon.Remove(pembayaranKasbon);

                    GridListControlHelper.UpdateObject<Kasbon>(this.gridList, _listOfKasbon, kasbon);
                    GridListControlHelper.RemoveObject<PembayaranKasbon>(this.gridListHistoriPembayaran, _listOfHistoriPembayaranKasbon, pembayaranKasbon);

                    btnTambahPembayaran.Enabled = kasbon.sisa > 0;
                    ResetButtonHistoriPembayaran(_listOfHistoriPembayaranKasbon.Count > 0);
                }
                else
                    MsgHelper.MsgDeleteError();
            }
        }

        private void chkTampilkanYangBelumLunas_CheckedChanged(object sender, EventArgs e)
        {
            filterRangeTanggal.Enabled = !((CheckBox)sender).Checked;

            if (!filterRangeTanggal.Enabled)
            {
                LoadData(false);
            }
            else
            {
                filterRangeTanggal_BtnTampilkanClicked(sender, e);
            }
        }
    }
}
