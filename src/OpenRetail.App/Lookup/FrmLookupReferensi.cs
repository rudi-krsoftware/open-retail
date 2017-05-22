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
using OpenRetail.App.UI.Template;
using OpenRetail.App.Helper;
using Syncfusion.Windows.Forms.Grid;

namespace OpenRetail.App.Lookup
{
    public partial class FrmLookupReferensi : FrmLookupEmptyBody
    {
        private IList<Supplier> _listOfSupplier = null;
        private IList<Customer> _listOfCustomer = null;
        private IList<Produk> _listOfProduk = null;
        private IList<JenisPengeluaran> _listOfJenisPengeluaran = null;
        private IList<KabupatenAsal> _listOfKabupatenAsal = null;
        private IList<KabupatenTujuan> _listOfKabupatenTujuan = null;

        private ReferencesType _referensiType = ReferencesType.Supplier;
        public IListener Listener { private get; set; }

        public FrmLookupReferensi(string header, IList<JenisPengeluaran> listOfJenisPengeluaran)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            this._listOfJenisPengeluaran = listOfJenisPengeluaran;
            this._referensiType = ReferencesType.JenisPengeluaran;

            InitGridList();
            base.SetActiveBtnPilih(listOfJenisPengeluaran.Count > 0);
        }

        public FrmLookupReferensi(string header, IList<Supplier> listOfSupplier)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            this._listOfSupplier = listOfSupplier;
            this._referensiType = ReferencesType.Supplier;

            InitGridList();
            base.SetActiveBtnPilih(listOfSupplier.Count > 0);
        }

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

        public FrmLookupReferensi(string header, IList<KabupatenAsal> listOfKabupatenAsal)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            this._listOfKabupatenAsal = listOfKabupatenAsal;
            this._referensiType = ReferencesType.KabupatenAsal;

            InitGridList();
            base.SetActiveBtnPilih(listOfKabupatenAsal.Count > 0);
        }

        public FrmLookupReferensi(string header, IList<KabupatenTujuan> listOfKabupatenTujuan)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            this._listOfKabupatenTujuan = listOfKabupatenTujuan;
            this._referensiType = ReferencesType.KabupatenTujuan;

            InitGridList();
            base.SetActiveBtnPilih(listOfKabupatenTujuan.Count > 0);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });

            var listCount = 0;

            switch (this._referensiType)
            {
                case ReferencesType.JenisPengeluaran:
                    gridListProperties.Add(new GridListControlProperties { Header = "Jenis Biaya" });

                    GridListControlHelper.InitializeGridListControl<JenisPengeluaran>(this.gridList, _listOfJenisPengeluaran, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridJenisPengeluaran_QueryCellInfo;

                    listCount = _listOfJenisPengeluaran.Count;

                    break;

                case ReferencesType.Customer:
                    gridListProperties.Add(new GridListControlProperties { Header = "Nama Customer", Width = 200 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Alamat" });

                    GridListControlHelper.InitializeGridListControl<Customer>(this.gridList, _listOfCustomer, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridCustomer_QueryCellInfo;

                    listCount = _listOfCustomer.Count;

                    break;

                case ReferencesType.Supplier:
                    gridListProperties.Add(new GridListControlProperties { Header = "Nama Supplier", Width = 200 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Alamat" });

                    GridListControlHelper.InitializeGridListControl<Supplier>(this.gridList, _listOfSupplier, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridSupplier_QueryCellInfo;

                    listCount = _listOfSupplier.Count;
                    break;

                case ReferencesType.Produk:
                    gridListProperties.Add(new GridListControlProperties { Header = "Kode Produk", Width = 120 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Nama Produk", Width = 350 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Golongan" });
                    GridListControlHelper.InitializeGridListControl<Produk>(this.gridList, _listOfProduk, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridProduk_QueryCellInfo;

                    listCount = _listOfProduk.Count;
                    break;

                case ReferencesType.KabupatenAsal:
                    gridListProperties.Add(new GridListControlProperties { Header = "Provinsi", Width = 250 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Kota/Kabupaten", Width = 250 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Kode Pos" });
                    GridListControlHelper.InitializeGridListControl<KabupatenAsal>(this.gridList, _listOfKabupatenAsal, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridKabupatenAsal_QueryCellInfo;

                    listCount = _listOfKabupatenAsal.Count;
                    break;

                case ReferencesType.KabupatenTujuan:
                    gridListProperties.Add(new GridListControlProperties { Header = "Provinsi", Width = 250 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Kota/Kabupaten", Width = 250 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Kode Pos" });
                    GridListControlHelper.InitializeGridListControl<KabupatenTujuan>(this.gridList, _listOfKabupatenTujuan, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridKabupatenTujuan_QueryCellInfo;

                    listCount = _listOfKabupatenTujuan.Count;
                    break;

                default:
                    break;
            }

            if (listCount > 0)
                this.gridList.SetSelected(0, true);
        }

        private void GridKabupatenTujuan_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {            
            if (_listOfKabupatenTujuan.Count > 0)
            {
                if (e.RowIndex > 0)
                {
                    var rowIndex = e.RowIndex - 1;

                    if (rowIndex < _listOfKabupatenTujuan.Count)
                    {
                        var kabupaten = _listOfKabupatenTujuan[rowIndex];

                        switch (e.ColIndex)
                        {
                            case 2:
                                e.Style.CellValue = kabupaten.Provinsi.nama_provinsi;
                                break;

                            case 3:
                                e.Style.CellValue = kabupaten.nama_kabupaten;
                                break;

                            case 4:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                e.Style.CellValue = kabupaten.kode_pos;
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

        private void GridKabupatenAsal_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {            
            if (_listOfKabupatenAsal.Count > 0)
            {
                if (e.RowIndex > 0)
                {
                    var rowIndex = e.RowIndex - 1;

                    if (rowIndex < _listOfKabupatenAsal.Count)
                    {
                        var kabupaten = _listOfKabupatenAsal[rowIndex];

                        switch (e.ColIndex)
                        {
                            case 2:
                                e.Style.CellValue = kabupaten.Provinsi.nama_provinsi;
                                break;

                            case 3:
                                e.Style.CellValue = kabupaten.nama_kabupaten;
                                break;

                            case 4:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                e.Style.CellValue = kabupaten.kode_pos;
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

        private void GridJenisPengeluaran_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
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
        }

        private void GridCustomer_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
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

        private void GridSupplier_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
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
                case ReferencesType.JenisPengeluaran:
                    var jenisPengeluaran = _listOfJenisPengeluaran[rowIndex];
                    this.Listener.Ok(this, jenisPengeluaran);
                    break;

                case ReferencesType.Supplier:
                    var supplier = _listOfSupplier[rowIndex];
                    this.Listener.Ok(this, supplier);
                    break;

                case ReferencesType.Customer:
                    var customer = _listOfCustomer[rowIndex];
                    this.Listener.Ok(this, customer);
                    break;
    
                case ReferencesType.Produk:
                    var produk = _listOfProduk[rowIndex];
                    this.Listener.Ok(this, produk);
                    break;

                case ReferencesType.KabupatenAsal:
                    var kabupatenAsal = _listOfKabupatenAsal[rowIndex];
                    this.Listener.Ok(this, kabupatenAsal);
                    break;

                case ReferencesType.KabupatenTujuan:
                    var kabupatenTujuan = _listOfKabupatenTujuan[rowIndex];
                    this.Listener.Ok(this, kabupatenTujuan);
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
