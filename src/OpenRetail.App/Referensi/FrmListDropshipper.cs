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
using System.IO;
using System.Diagnostics;
using OpenRetail.Helper.UserControl;

namespace OpenRetail.App.Referensi
{
    public partial class FrmListDropshipper : FrmListEmptyBody, IListener
    {                
        private IDropshipperBll _bll; // deklarasi objek business logic layer 
        private IList<Dropshipper> _listOfDropshipper = new List<Dropshipper>();
        private ILog _log;
        
        public FrmListDropshipper(string header, Pengguna pengguna, string menuId)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            this.btnImport.Visible = true;
            this.toolTip1.SetToolTip(this.btnImport, "Import/Export Data Dropshipper");
            this.mnuBukaFileMaster.Text = "Buka File Master Dropshipper";
            this.mnuImportFileMaster.Text = "Import File Master Dropshipper";
            this.mnuExportData.Text = "Export Data Dropshipper";

            base.SetHeader(header);
            base.WindowState = FormWindowState.Maximized;

            _log = MainProgram.log;
            _bll = new DropshipperBll(_log);
            
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
            RolePrivilegeHelper.SetHakAkses(this, pengguna, menuId, _listOfDropshipper.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama", Width = 400 });
            gridListProperties.Add(new GridListControlProperties { Header = "Alamat", Width = 700 });
            gridListProperties.Add(new GridListControlProperties { Header = "Telepon" });

            GridListControlHelper.InitializeGridListControl<Dropshipper>(this.gridList, _listOfDropshipper, gridListProperties);

            if (_listOfDropshipper.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                if (_listOfDropshipper.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfDropshipper.Count)
                        {
                            var dropshipper = _listOfDropshipper[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.CellValue = dropshipper.nama_dropshipper;
                                    break;

                                case 3:
                                    e.Style.CellValue = dropshipper.alamat;
                                    break;

                                case 4:
                                    e.Style.CellValue = dropshipper.telepon;
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
                _listOfDropshipper = _bll.GetAll();

                GridListControlHelper.Refresh<Dropshipper>(this.gridList, _listOfDropshipper);
            }

            ResetButton();
        }

        private void LoadData(string dropshipperName)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfDropshipper = _bll.GetByName(dropshipperName);

                GridListControlHelper.Refresh<Dropshipper>(this.gridList, _listOfDropshipper);
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfDropshipper.Count > 0);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryDropshipper("Tambah Data " + this.TabText, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var dropshipper = _listOfDropshipper[index];

            var frm = new FrmEntryDropshipper("Edit Data " + this.TabText, dropshipper, _bll);
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
                var dropshipper = _listOfDropshipper[index];

                var result = _bll.Delete(dropshipper);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<Dropshipper>(this.gridList, _listOfDropshipper, dropshipper);
                    ResetButton();
                }
                else
                    MsgHelper.MsgDeleteError();
            }
        }

        protected override void OpenFileMaster()
        {
            var msg = "Untuk membuka file master Dropshipper membutuhkan Ms Excel versi 2007 atau yang terbaru.\n\n" +
                      "Apakah proses ingin dilanjutkan ?";

            if (MsgHelper.MsgKonfirmasi(msg))
            {
                var fileMaster = Utils.GetAppPath() + @"\File Import Excel\Master Data\data_dropshipper.xlsx";

                if (!File.Exists(fileMaster))
                {
                    MsgHelper.MsgWarning("Maaf file master Dropshipper tidak ditemukan.");
                    return;
                }

                try
                {
                    Process.Start(fileMaster);
                }
                catch
                {
                    msg = "Gagal membuka file master Dropshipper !!!.\n\n" +
                          "Cek apakah Ms Excel versi 2007 atau yang terbaru sudah terinstall ?";

                    MsgHelper.MsgError(msg);
                }
            }
        }

        protected override void ImportData()
        {
            var frm = new FrmImportDataDropshipper("Import Data Dropshipper dari File Excel");
            frm.Listener = this;
            frm.ShowDialog();        
        }

        protected override void ExportData()
        {
            using (var dlgSave = new SaveFileDialog())
            {
                dlgSave.Filter = "Microsoft Excel files (*.xlsx)|*.xlsx";
                dlgSave.Title = "Export Data Dropshipper";

                var result = dlgSave.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                    {
                        IImportExportDataBll<Dropshipper> _importDataBll = new ImportExportDataDropshipperBll(dlgSave.FileName, _log);
                        _importDataBll.Export(_listOfDropshipper);
                    }
                }
            }
        }


        public void Ok(object sender, object data)
        {
            if (sender is FrmImportDataDropshipper)
            {
                LoadData(); // refresh data setelah import dari file excel
            }
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            var dropshipper = (Dropshipper)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<Dropshipper>(this.gridList, _listOfDropshipper, dropshipper);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<Dropshipper>(this.gridList, _listOfDropshipper, dropshipper);
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            if (txtNamaDropshipper.Text == "Cari nama dropshipper ...")
                LoadData();
            else
                LoadData(txtNamaDropshipper.Text);
        }

        private void txtNamaDropshipper_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                btnCari_Click(sender, e);
        }

        private void txtNamaDropshipper_Leave(object sender, EventArgs e)
        {
            var txtCari = (AdvancedTextbox)sender;

            if (txtCari.Text.Length == 0)
                txtCari.Text = "Cari nama dropshipper ...";
        }

        private void txtNamaDropshipper_Enter(object sender, EventArgs e)
        {
            ((AdvancedTextbox)sender).Clear();
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (btnPerbaiki.Enabled)
                Perbaiki();
        }
    }
}
