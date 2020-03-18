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

using ConceptCave.WaitCursor;
using log4net;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.Helper;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Model;
using Syncfusion.Windows.Forms.Grid;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenRetail.App.Referensi
{
    public partial class FrmListKartu : FrmListStandard, IListener
    {
        private IKartuBll _bll; // deklarasi objek business logic layer
        private IList<Kartu> _listOfKartu = new List<Kartu>();
        private ILog _log;

        public FrmListKartu(string header, Pengguna pengguna, string menuId)
            : base(header)
        {
            InitializeComponent();

            _log = MainProgram.log;
            _bll = new KartuBll(MainProgram.isUseWebAPI, MainProgram.baseUrl, _log);

            // set hak akses untuk SELECT
            var role = pengguna.GetRoleByMenuAndGrant(menuId, GrantState.SELECT);
            if (role != null)
                if (role.is_grant)
                {
                    LoadData();
                }

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, pengguna, menuId, _listOfKartu.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama Kartu", Width = 600 });
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan" });

            GridListControlHelper.InitializeGridListControl<Kartu>(this.gridList, _listOfKartu, gridListProperties);

            if (_listOfKartu.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate (object sender, GridQueryCellInfoEventArgs e)
            {
                if (_listOfKartu.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfKartu.Count)
                        {
                            var kartu = _listOfKartu[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.CellValue = kartu.nama_kartu;
                                    break;

                                case 3:
                                    e.Style.CellValue = kartu.is_debit ? "Kartu Debit" : "Kartu Kredit";
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
                _listOfKartu = _bll.GetAll();

                GridListControlHelper.Refresh<Kartu>(this.gridList, _listOfKartu);
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfKartu.Count > 0);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryKartu("Tambah Data " + this.TabText, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.Text))
                return;

            var kartu = _listOfKartu[index];

            var frm = new FrmEntryKartu("Edit Data " + this.TabText, kartu, _bll);
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
                var kartu = _listOfKartu[index];

                using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                {
                    var result = _bll.Delete(kartu);
                    if (result > 0)
                    {
                        GridListControlHelper.RemoveObject<Kartu>(this.gridList, _listOfKartu, kartu);
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
            var kartu = (Kartu)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<Kartu>(this.gridList, _listOfKartu, kartu);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<Kartu>(this.gridList, _listOfKartu, kartu);
        }
    }
}