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

namespace OpenRetail.App.Referensi
{
    public partial class FrmListSupplier : FrmListStandard, IListener
    {        
        private ISupplierBll _bll; // deklarasi objek business logic layer 
        private IList<Supplier> _listOfSupplier = new List<Supplier>();
        private ILog _log;

        public FrmListSupplier(string header, Pengguna pengguna, string menuId)
            : base(header)
        {
            InitializeComponent();

            _log = MainProgram.log;
            _bll = new SupplierBll(_log);

            // set hak akses untuk SELECT
            var role = pengguna.GetRoleByMenuAndGrant(menuId, GrantState.SELECT);
            if (role != null)
                if (role.is_grant)
                    LoadData();

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, pengguna, menuId, _listOfSupplier.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama", Width = 250 });
            gridListProperties.Add(new GridListControlProperties { Header = "Alamat", Width = 300 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kontak", Width = 250 });
            gridListProperties.Add(new GridListControlProperties { Header = "Telepon", Width = 130 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sisa Piutang", Width = 100 });

            GridListControlHelper.InitializeGridListControl<Supplier>(this.gridList, _listOfSupplier, gridListProperties);

            if (_listOfSupplier.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                if (_listOfSupplier.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfSupplier.Count)
                        {
                            var supplier = _listOfSupplier[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.CellValue = supplier.nama_supplier;
                                    break;

                                case 3:
                                    e.Style.CellValue = supplier.alamat;
                                    break;

                                case 4:
                                    e.Style.CellValue = supplier.kontak;
                                    break;

                                case 5:
                                    e.Style.CellValue = supplier.telepon;
                                    break;

                                case 6:
                                    e.Style.CellValue = NumberHelper.NumberToString(supplier.total_hutang - supplier.total_pembayaran_hutang);
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
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
                _listOfSupplier = _bll.GetAll();
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfSupplier.Count > 0);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntrySupplier("Tambah Data " + this.TabText, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var supplier = _listOfSupplier[index];

            var frm = new FrmEntrySupplier("Edit Data " + this.TabText, supplier, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Hapus()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            if (MsgHelper.MsgDelete())
            {
                var supplier = _listOfSupplier[index];

                var result = _bll.Delete(supplier);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<Supplier>(this.gridList, _listOfSupplier, supplier);
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
            var supplier = (Supplier)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<Supplier>(this.gridList, _listOfSupplier, supplier);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<Supplier>(this.gridList, _listOfSupplier, supplier);
        }
    }
}
