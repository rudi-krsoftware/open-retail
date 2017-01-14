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

namespace OpenRetail.App.Referensi
{
    public partial class FrmListCustomer : FrmListStandard, IListener
    {
        private ICustomerBll _bll; // deklarasi objek business logic layer 
        private IList<Customer> _listOfCustomer = new List<Customer>();
        
        public FrmListCustomer(string header)
            : base(header)
        {
            InitializeComponent();

            _bll = new CustomerBll(MainProgram.log);
            LoadData();

            InitGridList();
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama", Width = 250 });
            gridListProperties.Add(new GridListControlProperties { Header = "Alamat", Width = 300 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kontak", Width = 250 });
            gridListProperties.Add(new GridListControlProperties { Header = "Telepon", Width = 130 });
            gridListProperties.Add(new GridListControlProperties { Header = "Plafon Piutang", Width = 130 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sisa Piutang", Width = 100 });

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
                                    e.Style.CellValue = customer.kontak;
                                    break;

                                case 5:
                                    e.Style.CellValue = customer.telepon;
                                    break;

                                case 6:
                                    e.Style.CellValue = NumberHelper.NumberToString(customer.plafon_piutang);
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    break;

                                case 7:
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
