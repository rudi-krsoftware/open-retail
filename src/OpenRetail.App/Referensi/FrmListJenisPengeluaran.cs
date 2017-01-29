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
    public partial class FrmListJenisPengeluaran : FrmListStandard, IListener
    {        
        private IJenisPengeluaranBll _bll; // deklarasi objek business logic layer 
        private IList<JenisPengeluaran> _listOfJenisPengeluaran = new List<JenisPengeluaran>();
        private ILog _log;

        public FrmListJenisPengeluaran(string header, Pengguna pengguna, string menuId)
            : base(header)
        {
            InitializeComponent();

            _log = MainProgram.log;
            _bll = new JenisPengeluaranBll(_log);

            // set hak akses untuk SELECT
            var role = pengguna.GetRoleByMenuAndGrant(menuId, GrantState.SELECT);
            if (role != null)
                if (role.is_grant)
                    LoadData();

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, pengguna, menuId, _listOfJenisPengeluaran.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jenis Pengeluaran" });

            GridListControlHelper.InitializeGridListControl<JenisPengeluaran>(this.gridList, _listOfJenisPengeluaran, gridListProperties);

            if (_listOfJenisPengeluaran.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                if (_listOfJenisPengeluaran.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfJenisPengeluaran.Count)
                        {
                            var jenisPengeluaran = _listOfJenisPengeluaran[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.CellValue = jenisPengeluaran.nama_jenis_pengeluaran;
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
                _listOfJenisPengeluaran = _bll.GetAll();
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfJenisPengeluaran.Count > 0);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryJenisPengeluaran("Tambah Data " + this.TabText, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.Text))
                return;

            var jenisPengeluaran = _listOfJenisPengeluaran[index];

            var frm = new FrmEntryJenisPengeluaran("Edit Data " + this.TabText, jenisPengeluaran, _bll);
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
                var jenisPengeluaran = _listOfJenisPengeluaran[index];

                var result = _bll.Delete(jenisPengeluaran);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<JenisPengeluaran>(this.gridList, _listOfJenisPengeluaran, jenisPengeluaran);
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
            var jenisPengeluaran = (JenisPengeluaran)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<JenisPengeluaran>(this.gridList, _listOfJenisPengeluaran, jenisPengeluaran);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<JenisPengeluaran>(this.gridList, _listOfJenisPengeluaran, jenisPengeluaran);
        }
    }
}
