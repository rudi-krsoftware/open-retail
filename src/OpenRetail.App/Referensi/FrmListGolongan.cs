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
using System.IO;
using System.Diagnostics;

namespace OpenRetail.App.Referensi
{
    public partial class FrmListGolongan : FrmListStandard, IListener
    {
        private IGolonganBll _bll; // deklarasi objek business logic layer 
        private IList<Golongan> _listOfGolongan = new List<Golongan>();
        private ILog _log;

        public FrmListGolongan(string header, Pengguna pengguna, string menuId)
            : base(header)
        {
            InitializeComponent();
            this.btnImport.Visible = true;
            this.toolTip1.SetToolTip(this.btnImport, "Import/Export Data Golongan");
            this.mnuBukaFileMaster.Text = "Buka File Master Golongan";
            this.mnuImportFileMaster.Text = "Import File Master Golongan";
            this.mnuExportData.Text = "Export Data Golongan";

            _log = MainProgram.log;
            _bll = new GolonganBll(_log);

            // set hak akses untuk SELECT
            var role = pengguna.GetRoleByMenuAndGrant(menuId, GrantState.SELECT);
            if (role != null)
                if (role.is_grant)
                {
                    LoadData();

                    btnImport.Enabled = pengguna.is_administrator;
                }                    

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, pengguna, menuId, _listOfGolongan.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Golongan", Width = 700 });
            gridListProperties.Add(new GridListControlProperties { Header = "Diskon" });

            GridListControlHelper.InitializeGridListControl<Golongan>(this.gridList, _listOfGolongan, gridListProperties);

            if (_listOfGolongan.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                if (_listOfGolongan.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfGolongan.Count)
                        {
                            var golongan = _listOfGolongan[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.CellValue = golongan.nama_golongan;
                                    break;

                                case 3:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = golongan.diskon;
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
                _listOfGolongan = _bll.GetAll();

                GridListControlHelper.Refresh<Golongan>(this.gridList, _listOfGolongan);
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfGolongan.Count > 0);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryGolongan("Tambah Data " + this.TabText, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.Text))
                return;

            var golongan = _listOfGolongan[index];

            var frm = new FrmEntryGolongan("Edit Data " + this.TabText, golongan, _bll);
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
                var golongan = _listOfGolongan[index];

                var result = _bll.Delete(golongan);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<Golongan>(this.gridList, _listOfGolongan, golongan);
                    ResetButton();
                }
                else
                    MsgHelper.MsgDeleteError();
            }
        }

        protected override void OpenFileMaster()
        {
            var msg = "Untuk membuka file master Golongan membutuhkan Ms Excel versi 2007 atau yang terbaru.\n\n" +
                      "Apakah proses ingin dilanjutkan ?";

            if (MsgHelper.MsgKonfirmasi(msg))
            {
                var fileMaster = Utils.GetAppPath() + @"\File Import Excel\Master Data\data_golongan.xlsx";

                if (!File.Exists(fileMaster))
                {
                    MsgHelper.MsgWarning("Maaf file master Golongan tidak ditemukan.");
                    return;
                }

                try
                {
                    Process.Start(fileMaster);
                }
                catch
                {
                    msg = "Gagal membuka file master Golongan !!!.\n\n" +
                          "Cek apakah Ms Excel versi 2007 atau yang terbaru sudah terinstall ?";

                    MsgHelper.MsgError(msg);
                }
            }
        }

        protected override void ImportData()
        {
            var frm = new FrmImportDataGolongan("Import Data Golongan dari File Excel");
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void ExportData()
        {
            using (var dlgSave = new SaveFileDialog())
            {
                dlgSave.Filter = "Microsoft Excel files (*.xlsx)|*.xlsx";
                dlgSave.Title = "Export Data Golongan";

                var result = dlgSave.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                    {
                        IImportExportDataBll<Golongan> _importDataBll = new ImportExportDataGolonganBll(dlgSave.FileName, _log);
                        _importDataBll.Export(_listOfGolongan);
                    }
                }
            }
        }

        public void Ok(object sender, object data)
        {
            if (sender is FrmImportDataGolongan)
            {
                LoadData(); // refresh data setelah import dari file excel
            }
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            var golongan = (Golongan)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<Golongan>(this.gridList, _listOfGolongan, golongan);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<Golongan>(this.gridList, _listOfGolongan, golongan);
        }
    }
}
