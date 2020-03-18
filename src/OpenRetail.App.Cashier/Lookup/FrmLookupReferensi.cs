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

using OpenRetail.Helper;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Model;
using Syncfusion.Windows.Forms.Grid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OpenRetail.App.Cashier.Lookup
{
    public partial class FrmLookupReferensi : FrmLookupEmptyBody
    {
        private IList<Customer> _listOfCustomer = null;
        private IList<Produk> _listOfProduk = null;

        private ReferencesType _referensiType = ReferencesType.Customer;
        public IListener Listener { private get; set; }

        public FrmLookupReferensi(string header, IList<Customer> listOfCustomer)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            this._listOfCustomer = listOfCustomer;
            this._referensiType = ReferencesType.Customer;

            InitGridList();
            base.SetActiveBtnPilih(listOfCustomer.Count > 0);
        }

        public FrmLookupReferensi(string header, IList<Produk> listOfProduk)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            this._listOfProduk = listOfProduk;
            this._referensiType = ReferencesType.Produk;

            InitGridList();
            base.SetActiveBtnPilih(listOfProduk.Count > 0);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 50 });

            var listCount = 0;

            this.gridList.Grid.QueryRowHeight += delegate (object sender, GridRowColSizeEventArgs e)
            {
                e.Size = 27;
                e.Handled = true;
            };

            switch (this._referensiType)
            {
                case ReferencesType.Customer:
                    gridListProperties.Add(new GridListControlProperties { Header = "Nama Customer", Width = 400 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Alamat" });

                    GridListControlHelper.InitializeGridListControl<Customer>(this.gridList, _listOfCustomer, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridCustomer_QueryCellInfo;

                    listCount = _listOfCustomer.Count;

                    break;

                case ReferencesType.Produk:
                    gridListProperties.Add(new GridListControlProperties { Header = "Kode Produk", Width = 150 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Nama Produk", Width = 400 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Harga", Width = 120 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Stok", Width = 70 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Golongan" });
                    GridListControlHelper.InitializeGridListControl<Produk>(this.gridList, _listOfProduk, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridProduk_QueryCellInfo;

                    listCount = _listOfProduk.Count;
                    break;

                default:
                    break;
            }

            if (listCount > 0)
                this.gridList.SetSelected(0, true);
        }

        private void GridCustomer_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            if (_listOfCustomer.Count > 0)
            {
                if (e.RowIndex > 0)
                {
                    e.Style.Font = new GridFontInfo(new Font("Arial", 14f));

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

                            default:
                                break;
                        }

                        // we handled it, let the grid know
                        e.Handled = true;
                    }
                }
            }
        }

        private void GridProduk_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            if (_listOfProduk.Count > 0)
            {
                if (e.RowIndex > 0)
                {
                    e.Style.Font = new GridFontInfo(new Font("Arial", 14f));

                    var rowIndex = e.RowIndex - 1;

                    if (rowIndex < _listOfProduk.Count)
                    {
                        var produk = _listOfProduk[rowIndex];

                        switch (e.ColIndex)
                        {
                            case 2:
                                e.Style.CellValue = produk.kode_produk;
                                break;

                            case 3:
                                e.Style.CellValue = produk.nama_produk;
                                break;

                            case 4:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                e.Style.CellValue = NumberHelper.NumberToString(produk.harga_jual);
                                break;

                            case 5:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                e.Style.CellValue = (produk.stok + produk.stok_gudang);
                                break;

                            case 6:
                                var golongan = produk.Golongan;

                                if (golongan != null)
                                    e.Style.CellValue = golongan.nama_golongan;

                                break;

                            default:
                                break;
                        }

                        // we handled it, let the grid know
                        e.Handled = true;
                    }
                }
            }
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (base.IsButtonPilihEnabled)
                Pilih();
        }

        protected override void Pilih()
        {
            var rowIndex = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(rowIndex, this.Text))
                return;

            switch (this._referensiType)
            {
                case ReferencesType.Customer:
                    var customer = _listOfCustomer[rowIndex];
                    this.Listener.Ok(this, customer);
                    break;

                case ReferencesType.Produk:
                    var produk = _listOfProduk[rowIndex];
                    this.Listener.Ok(this, produk);
                    break;

                default:
                    break;
            }

            this.Close();
        }

        private void gridList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                if (base.IsButtonPilihEnabled)
                    Pilih();
            }
        }
    }
}