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
    public partial class FrmListCustomer : FrmListStandard, IListener
    {
        private ICustomerBll _bll; // deklarasi objek business logic layer 
        private IList<Customer> _listOfCustomer = new List<Customer>();
        private ILog _log;

        public FrmListCustomer(string header, Pengguna pengguna, string menuId)
            : base(header)
        {
            InitializeComponent();
            this.btnImport.Visible = true;
            this.toolTip1.SetToolTip(this.btnImport, "Import Data Customer");
            this.mnuBukaFileMaster.Text = "Buka File Master Customer";
            this.mnuImportFileMaster.Text = "Import File Master Customer";

            _log = MainProgram.log;
            _bll = new CustomerBll(_log);

            // set hak akses untuk SELECT
            var role = pengguna.GetRoleByMenuAndGrant(menuId, GrantState.SELECT);
            if (role != null)
                if (role.is_grant)
                    LoadData();

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, pengguna, menuId, _listOfCustomer.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama", Width = 170 });

            gridListProperties.Add(new GridListControlProperties { Header = "Alamat", Width = 250 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kecamatan", Width = 120 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kelurahan", Width = 120 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kota", Width = 120 });
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
                                    e.Style.CellValue = customer.kecamatan;
                                    break;

                                case 5:
                                    e.Style.CellValue = customer.kelurahan;
                                    break;

                                case 6:
                                    e.Style.CellValue = customer.kota;
                                    break;

                                case 7:
                                    e.Style.CellValue = customer.kode_pos;
                                    break;

                                case 8:
                                    e.Style.CellValue = customer.kontak;
                                    break;

                                case 9:
                                    e.Style.CellValue = customer.telepon;
                                    break;

                                case 10:
                                    e.Style.CellValue = customer.diskon;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 11:
                                    e.Style.CellValue = NumberHelper.NumberToString(customer.plafon_piutang);
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    break;

                                case 12:
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
            var msg = string.Empty;
            var fileMaster = Utils.GetAppPath() + @"\File Import Excel\Master Data\data_customer.xlsx";

            IImportExportDataBll _importDataBll = new ImportExportDataCustomerBll(fileMaster, _log);

            if (_importDataBll.IsOpened())
            {
                msg = "Maaf file master Customer sedang dibuka, silahkan ditutup terlebih dulu.";
                MsgHelper.MsgWarning(msg);

                return;
            }

            if (!_importDataBll.IsValidFormat())
            {
                msg = "Maaf format file master Customer tidak valid, proses import tidak bisa dilanjutkan.";
                MsgHelper.MsgWarning(msg);

                return;
            }

            if (MsgHelper.MsgKonfirmasi("Apakah proses ingin dilanjutkan ?"))
            {
                using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                {
                    var rowCount = 0;
                    var result = _importDataBll.Import(ref rowCount);

                    if (result)
                    {
                        msg = "Import data master Customer berhasil.";
                        MsgHelper.MsgInfo(msg);
                        LoadData();
                    }
                    else
                    {
                        if (rowCount == 0)
                        {
                            msg = "Data file master Customer masih kosong.\n" +
                                  "Silahkan diisi terlebih dulu.";
                            MsgHelper.MsgInfo(msg);
                        }
                    }
                }
            }
        }

        public void Ok(object sender, object data)
        {
            throw new NotImplementedException();
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
    }
}
