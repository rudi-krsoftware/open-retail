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
    public partial class FrmListKaryawan : FrmListStandard, IListener
    {
        private IKaryawanBll _bll; // deklarasi objek business logic layer 
        private IList<Karyawan> _listOfKaryawan = new List<Karyawan>();
        private IList<Jabatan> _listOfJabatan = new List<Jabatan>();
        private ILog _log;

        public FrmListKaryawan(string header, Pengguna pengguna, string menuId)
            : base(header)
        {
            InitializeComponent();

            _log = MainProgram.log;
            _bll = new KaryawanBll(_log);

            // set hak akses untuk SELECT
            var role = pengguna.GetRoleByMenuAndGrant(menuId, GrantState.SELECT);
            if (role != null)
                if (role.is_grant)
                    LoadData();

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, pengguna, menuId, _listOfKaryawan.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama", Width = 300 });
            gridListProperties.Add(new GridListControlProperties { Header = "Alamat", Width = 300 });
            gridListProperties.Add(new GridListControlProperties { Header = "Telepon", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jenis Gajian", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Status", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jabatan", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sisa Kasbon" });

            GridListControlHelper.InitializeGridListControl<Karyawan>(this.gridList, _listOfKaryawan, gridListProperties);

            if (_listOfKaryawan.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                if (_listOfKaryawan.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfKaryawan.Count)
                        {
                            var karyawan = _listOfKaryawan[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.CellValue = karyawan.nama_karyawan;
                                    break;

                                case 3:
                                    e.Style.CellValue = karyawan.alamat;
                                    break;

                                case 4:
                                    e.Style.CellValue = karyawan.telepon;
                                    break;

                                case 5:
                                    e.Style.CellValue = karyawan.jenis_gajian == JenisGajian.Mingguan ? "Mingguan" : "Bulanan";
                                    break;

                                case 6:
                                    e.Style.CellValue = karyawan.is_active ? "Aktif" : "Non Aktif";
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 7:
                                    var jabatan = karyawan.Jabatan;

                                    if (jabatan != null)
                                        e.Style.CellValue = jabatan.nama_jabatan;

                                    break;

                                case 8:
                                    e.Style.CellValue = NumberHelper.NumberToString(karyawan.sisa_kasbon);
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
                _listOfKaryawan = _bll.GetAll();

                IJabatanBll jabatanBll = new JabatanBll(_log);
                _listOfJabatan = jabatanBll.GetAll();
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfKaryawan.Count > 0);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryKaryawan("Tambah Data " + this.TabText, _listOfJabatan, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.Text))
                return;

            var karyawan = _listOfKaryawan[index];

            var frm = new FrmEntryKaryawan("Edit Data " + this.TabText, karyawan, _listOfJabatan, _bll);
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
                var karyawan = _listOfKaryawan[index];

                var result = _bll.Delete(karyawan);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<Karyawan>(this.gridList, _listOfKaryawan, karyawan);
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
            var karyawan = (Karyawan)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<Karyawan>(this.gridList, _listOfKaryawan, karyawan);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<Karyawan>(this.gridList, _listOfKaryawan, karyawan);
        }
    }
}
