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
using OpenRetail.App.UserControl;

namespace OpenRetail.App.Referensi
{
    public partial class FrmListCustomer : FrmListEmptyBody, IListener
    {
        private ICustomerBll _bll; // deklarasi objek business logic layer 
        private IList<Customer> _listOfCustomer = new List<Customer>();
        private ILog _log;

        public FrmListCustomer(string header, Pengguna pengguna, string menuId)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            this.btnImport.Visible = true;
            this.toolTip1.SetToolTip(this.btnImport, "Import/Export Data Customer");
            this.mnuBukaFileMaster.Text = "Buka File Master Customer";
            this.mnuImportFileMaster.Text = "Import File Master Customer";
            this.mnuExportData.Text = "Export Data Customer";

            base.SetHeader(header);
            base.WindowState = FormWindowState.Maximized;

            _log = MainProgram.log;
            _bll = new CustomerBll(_log);

            cmbJenisCustomer.Enabled = false;

            // set hak akses untuk SELECT
            var role = pengguna.GetRoleByMenuAndGrant(menuId, GrantState.SELECT);
            if (role != null)
            {
                if (role.is_grant)
                    cmbJenisCustomer.SelectedIndex = 0;

                cmbJenisCustomer.Enabled = role.is_grant;
                btnImport.Enabled = pengguna.is_administrator;
            }                


            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, pengguna, menuId, _listOfCustomer.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama", Width = 150 });

            gridListProperties.Add(new GridListControlProperties { Header = "Alamat", Width = 230 });
            gridListProperties.Add(new GridListControlProperties { Header = "Desa", Width = 80 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kelurahan", Width = 80 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kecamatan", Width = 80 });            
            gridListProperties.Add(new GridListControlProperties { Header = "Kota", Width = 80 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kabupaten", Width = 80 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kode Pos", Width = 70 });

            gridListProperties.Add(new GridListControlProperties { Header = "Kontak", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Telepon", Width = 100 });

            gridListProperties.Add(new GridListControlProperties { Header = "Diskon", Width = 50 });
            gridListProperties.Add(new GridListControlProperties { Header = "Plafon Piutang", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sisa Piutang" });

            GridListControlHelper.InitializeGridListControl<Customer>(this.gridList, _listOfCustomer, gridListProperties);

            if (_listOfCustomer.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                if (_listOfCustomer.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfCustomer.Count)
                        {
                            var customer = _listOfCustomer[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.CellValue = customer.nama_customer;
                                    break;

                                case 3:
                                    e.Style.CellValue = customer.alamat;
                                    break;


                                case 4:
                                    e.Style.CellValue = customer.desa;
                                    break;

                                case 5:
                                    e.Style.CellValue = customer.kelurahan;
                                    break;

                                case 6:
                                    e.Style.CellValue = customer.kecamatan;
                                    break;

                                case 7:
                                    e.Style.CellValue = customer.kota;
                                    break;

                                case 8:
                                    e.Style.CellValue = customer.kabupaten;
                                    break;

                                case 9:
                                    e.Style.CellValue = customer.kode_pos;
                                    break;

                                case 10:
                                    e.Style.CellValue = customer.kontak;
                                    break;

                                case 11:
                                    e.Style.CellValue = customer.telepon;
                                    break;

                                case 12:
                                    e.Style.CellValue = customer.diskon;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 13:
                                    e.Style.CellValue = NumberHelper.NumberToString(customer.plafon_piutang);
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    break;

                                case 14:
                                    e.Style.CellValue = NumberHelper.NumberToString(customer.total_piutang - customer.total_pembayaran_piutang);
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
                _listOfCustomer = _bll.GetAll();

                GridListControlHelper.Refresh<Customer>(this.gridList, _listOfCustomer);
            }

            ResetButton();
        }

        private void LoadData(bool isReseller)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfCustomer = _bll.GetAll(isReseller);

                GridListControlHelper.Refresh<Customer>(this.gridList, _listOfCustomer);
            }

            ResetButton();
        }

        private void LoadData(string customerName)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfCustomer = _bll.GetByName(customerName);

                GridListControlHelper.Refresh<Customer>(this.gridList, _listOfCustomer);
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfCustomer.Count > 0);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryCustomer("Tambah Data " + this.TabText, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var customer = _listOfCustomer[index];

            var frm = new FrmEntryCustomer("Edit Data " + this.TabText, customer, _bll);
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
                var customer = _listOfCustomer[index];

                var result = _bll.Delete(customer);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<Customer>(this.gridList, _listOfCustomer, customer);
                    ResetButton();
                }
                else
                    MsgHelper.MsgDeleteError();
            }
        }

        protected override void OpenFileMaster()
        {
            var msg = "Untuk membuka file master Customer membutuhkan Ms Excel versi 2007 atau yang terbaru.\n\n" +
                      "Apakah proses ingin dilanjutkan ?";

            if (MsgHelper.MsgKonfirmasi(msg))
            {
                var fileMaster = Utils.GetAppPath() + @"\File Import Excel\Master Data\data_customer.xlsx";

                if (!File.Exists(fileMaster))
                {
                    MsgHelper.MsgWarning("Maaf file master Customer tidak ditemukan.");
                    return;
                }

                try
                {
                    Process.Start(fileMaster);
                }
                catch
                {
                    msg = "Gagal membuka file master Customer !!!.\n\n" +
                          "Cek apakah Ms Excel versi 2007 atau yang terbaru sudah terinstall ?";

                    MsgHelper.MsgError(msg);
                }
            }
        }

        protected override void ImportData()
        {
            var frm = new FrmImportDataCustomer("Import Data Customer dari File Excel");
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void ExportData()
        {
            using (var dlgSave = new SaveFileDialog())
            {
                dlgSave.Filter = "Microsoft Excel files (*.xlsx)|*.xlsx";
                dlgSave.Title = "Export Data Customer";

                var result = dlgSave.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                    {
                        IImportExportDataBll<Customer> _importDataBll = new ImportExportDataCustomerBll(dlgSave.FileName, _log);
                        _importDataBll.Export(_listOfCustomer);
                    }
                }
            }
        }

        public void Ok(object sender, object data)
        {
            if (sender is FrmImportDataCustomer)
            {
                LoadData(); // refresh data setelah import dari file excel
            }
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            var customer = (Customer)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<Customer>(this.gridList, _listOfCustomer, customer);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<Customer>(this.gridList, _listOfCustomer, customer);
        }

        private void cmbJenisCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = ((ComboBox)sender).SelectedIndex;

            switch (index)
            {
                case 0: // semua
                    LoadData();
                    break;

                case 1: // umum
                    LoadData(false);
                    break;

                case 2: // reseller
                    LoadData(true);
                    break;

                default:
                    break;
            }
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (btnPerbaiki.Enabled)
                Perbaiki();
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            if (txtNamaCustomer.Text == "Cari nama customer ...")
                LoadData();
            else
                LoadData(txtNamaCustomer.Text);
        }

        private void txtNamaCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                btnCari_Click(sender, e);
        }

        private void txtNamaCustomer_Enter(object sender, EventArgs e)
        {
            ((AdvancedTextbox)sender).Clear();
        }

        private void txtNamaCustomer_Leave(object sender, EventArgs e)
        {
            var txtCari = (AdvancedTextbox)sender;

            if (txtCari.Text.Length == 0)
                txtCari.Text = "Cari nama customer ...";
        }
    }
}
