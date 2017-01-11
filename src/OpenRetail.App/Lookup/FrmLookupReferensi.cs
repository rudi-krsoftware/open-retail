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
        private IList<Produk> _listOfProduk = null;

        private ReferencesType _referensiType = ReferencesType.Supplier;
        public IListener Listener { private get; set; }

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

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });

            var listCount = 0;

            switch (this._referensiType)
            {
                case ReferencesType.Supplier:
                    gridListProperties.Add(new GridListControlProperties { Header = "Nama Supplier", Width = 200 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Alamat" });

                    GridListControlHelper.InitializeGridListControl<Supplier>(this.gridList, _listOfSupplier, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridSupplier_QueryCellInfo;

                    listCount = _listOfSupplier.Count;
                    break;

                case ReferencesType.Produk:
                    gridListProperties.Add(new GridListControlProperties { Header = "Kode Produk", Width = 120 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Nama Produk" });
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
                case ReferencesType.Supplier:
                    var supplier = _listOfSupplier[rowIndex];
                    this.Listener.Ok(this, supplier);
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
