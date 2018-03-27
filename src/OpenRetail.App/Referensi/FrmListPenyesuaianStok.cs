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
using Syncfusion.Styles;

namespace OpenRetail.App.Referensi
{
    public partial class FrmListPenyesuaianStok : FrmListStandard, IListener
    {
        private IPenyesuaianStokBll _bll; // deklarasi objek business logic layer 
        private IList<PenyesuaianStok> _listOfPenyesuaianStok = new List<PenyesuaianStok>();
        private ILog _log;

        public FrmListPenyesuaianStok(string header, Pengguna pengguna, string menuId)
            : base(header)
        {
            InitializeComponent();

            _log = MainProgram.log;
            _bll = new PenyesuaianStokBll(MainProgram.isUseWebAPI, MainProgram.baseUrl, _log);

            // set hak akses untuk SELECT
            var role = pengguna.GetRoleByMenuAndGrant(menuId, GrantState.SELECT);
            if (role != null)
                if (role.is_grant)
                    LoadData();

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, pengguna, menuId, _listOfPenyesuaianStok.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 80 });
            gridListProperties.Add(new GridListControlProperties { Header = "Produk", Width = 250 });
            gridListProperties.Add(new GridListControlProperties { Header = "Penambahan", Width = 80 });
            gridListProperties.Add(new GridListControlProperties { Header = "Penambahan", Width = 80 });
            gridListProperties.Add(new GridListControlProperties { Header = "Pengurangan", Width = 80 });
            gridListProperties.Add(new GridListControlProperties { Header = "Pengurangan", Width = 80 });
            gridListProperties.Add(new GridListControlProperties { Header = "Alasan Penyesuaian", Width = 350 });
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan" });

            GridListControlHelper.InitializeGridListControl<PenyesuaianStok>(this.gridList, _listOfPenyesuaianStok, gridListProperties, false, additionalRowCount: 1);
            this.gridList.Grid.Model.RowHeights[1] = 25;
            this.gridList.Grid.Model.Rows.FrozenCount = 1;

            this.gridList.Grid.PrepareViewStyleInfo += delegate(object sender, GridPrepareViewStyleInfoEventArgs e)
            {
                var subHeaderHargaJual = new string[] { "Stok Etalase", "Stok Gudang", "Stok Etalase", "Stok Gudang" };
                if (e.ColIndex > 3 && e.RowIndex == 1)
                {
                    var colIndex = 4;

                    foreach (var header in subHeaderHargaJual)
                    {
                        if (colIndex == e.ColIndex)
                            e.Style.Text = header;

                        colIndex++;
                    }
                }
            };

            if (_listOfPenyesuaianStok.Count > 0)
                this.gridList.SetSelected(1, true);

            // merge cell
            var column = 1; // kolom no
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 2; // tanggal
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 3; // produk
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 4; // kolom penambahan stok etalase dan gudang
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 0, column + 1));

            column = 6; // kolom pengurangan stok etalase dan gudang
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 0, column + 1));

            column = 8; // alasan penyesuaian
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            column = 9; // keterangan
            this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, column, 1, column));

            var headerStyle = this.gridList.Grid.BaseStylesMap["Column Header"].StyleInfo;
            headerStyle.CellType = GridCellTypeName.Header;

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                if (e.RowIndex == 1)
                {
                    if (e.ColIndex > 3)
                    {
                        e.Style.ModifyStyle(headerStyle, StyleModifyType.ApplyNew);
                    }

                    // we handled it, let the grid know
                    e.Handled = true;
                }

                if (_listOfPenyesuaianStok.Count > 0)
                {
                    if (e.RowIndex > 1)
                    {
                        var rowIndex = e.RowIndex - 2;

                        if (rowIndex < _listOfPenyesuaianStok.Count)
                        {
                            var penyesuaianStok = _listOfPenyesuaianStok[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 1:
                                    e.Style.CellValue = e.RowIndex - 1;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 2:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(penyesuaianStok.tanggal);
                                    break;

                                case 3:
                                    e.Style.CellValue = penyesuaianStok.Produk.nama_produk;
                                    break;

                                case 4:
                                    e.Style.CellValue = penyesuaianStok.penambahan_stok;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 5:
                                    e.Style.CellValue = penyesuaianStok.penambahan_stok_gudang;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 6:
                                    e.Style.CellValue = penyesuaianStok.pengurangan_stok;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 7:
                                    e.Style.CellValue = penyesuaianStok.pengurangan_stok_gudang;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 8:
                                    e.Style.CellValue = penyesuaianStok.AlasanPenyesuaianStok.alasan;
                                    break;

                                case 9:
                                    e.Style.CellValue = penyesuaianStok.keterangan;
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
                _listOfPenyesuaianStok = _bll.GetAll();
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfPenyesuaianStok.Count > 0);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryPenyesuaianStok("Tambah Data " + this.TabText, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex - 1;

            if (!base.IsSelectedItem(index, this.Text))
                return;

            var penyesuaianStok = _listOfPenyesuaianStok[index];

            var frm = new FrmEntryPenyesuaianStok("Edit Data " + this.TabText, penyesuaianStok, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Hapus()
        {
            var index = this.gridList.SelectedIndex - 1;

            if (!base.IsSelectedItem(index, this.Text))
                return;

            if (MsgHelper.MsgDelete())
            {
                var penyesuaianStok = _listOfPenyesuaianStok[index];

                using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                {
                    var result = _bll.Delete(penyesuaianStok);
                    if (result > 0)
                    {
                        GridListControlHelper.RemoveObject<PenyesuaianStok>(this.gridList, _listOfPenyesuaianStok, penyesuaianStok, additionalRowCount: 1);
                        ResetButton();
                    }
                    else
                        MsgHelper.MsgDeleteError();
                }                
            }
        }

        public void Ok(object sender, object data)
        {
            throw new NotImplementedException();
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            var penyesuaianStok = (PenyesuaianStok)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<PenyesuaianStok>(this.gridList, _listOfPenyesuaianStok, penyesuaianStok, additionalRowCount: 1);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<PenyesuaianStok>(this.gridList, _listOfPenyesuaianStok, penyesuaianStok, additionalRowCount: 1);
        }
    }
}
