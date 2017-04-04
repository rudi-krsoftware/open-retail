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
    public partial class FrmListPengeluaranBiaya : FrmListEmptyBody, IListener
    {        
        private IPengeluaranBiayaBll _bll; // deklarasi objek business logic layer 
        private IList<PengeluaranBiaya> _listOfPengeluaran = new List<PengeluaranBiaya>();
        private ILog _log;
        private Pengguna _pengguna;
        private string _menuId;
        
        public FrmListPengeluaranBiaya(string header, Pengguna pengguna, string menuId)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            base.WindowState = FormWindowState.Maximized;

            _log = MainProgram.log;
            _bll = new PengeluaranBiayaBll(_log);
            _pengguna = pengguna;
            _menuId = menuId;

            // set hak akses untuk SELECT
            var role = _pengguna.GetRoleByMenuAndGrant(_menuId, GrantState.SELECT);
            if (role != null)
            {
                if (role.is_grant)
                    LoadData(filterRangeTanggal.TanggalMulai, filterRangeTanggal.TanggalSelesai);

                filterRangeTanggal.Enabled = role.is_grant;
            }            

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfPengeluaran.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nota", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan", Width = 500 });
            gridListProperties.Add(new GridListControlProperties { Header = "Total" });

            GridListControlHelper.InitializeGridListControl<PengeluaranBiaya>(this.gridList, _listOfPengeluaran, gridListProperties);

            if (_listOfPengeluaran.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {

                if (_listOfPengeluaran.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfPengeluaran.Count)
                        {
                            var pengeluaran = _listOfPengeluaran[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(pengeluaran.tanggal);
                                    break;

                                case 3:
                                    e.Style.CellValue = pengeluaran.nota;
                                    break;

                                case 4:
                                    e.Style.CellValue = pengeluaran.keterangan;
                                    break;

                                case 5:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(pengeluaran.total);
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
                _listOfPengeluaran = _bll.GetAll();
                GridListControlHelper.Refresh<PengeluaranBiaya>(this.gridList, _listOfPengeluaran);
            }

            ResetButton();
        }

        private void LoadData(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfPengeluaran = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);
                GridListControlHelper.Refresh<PengeluaranBiaya>(this.gridList, _listOfPengeluaran);
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfPengeluaran.Count > 0);

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfPengeluaran.Count);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryPengeluaranBiaya("Tambah Data " + this.Text, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var pengeluaran = _listOfPengeluaran[index];

            LogicalThreadContext.Properties["OldValue"] = pengeluaran.ToJson();

            var frm = new FrmEntryPengeluaranBiaya("Edit Data " + this.Text, pengeluaran, _bll);
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
                var pengeluaran = _listOfPengeluaran[index];

                var result = _bll.Delete(pengeluaran);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<PengeluaranBiaya>(this.gridList, _listOfPengeluaran, pengeluaran);
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
            var pengeluaran = (PengeluaranBiaya)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<PengeluaranBiaya>(this.gridList, _listOfPengeluaran, pengeluaran);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<PengeluaranBiaya>(this.gridList, _listOfPengeluaran, pengeluaran);
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
    }
}
