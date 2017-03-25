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

using OpenRetail.App.Helper;
using OpenRetail.App.UI.Template;
using OpenRetail.Model;
using Syncfusion.Windows.Forms.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenRetail.App.Lookup
{
    public partial class FrmLookupItemNota : FrmLookupEmptyBody
    {
        private IList<ItemJualProduk> _listOfItemJual = null;
        private IList<ItemBeliProduk> _listOfItemBeli = null;

        private ReferencesType _referensiType = ReferencesType.Produk;

        public IListener Listener { private get; set; }

        public FrmLookupItemNota(string header, IList<ItemJualProduk> listOfItemJual)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            this._listOfItemJual = listOfItemJual;
            this._referensiType = ReferencesType.NotaJualProduk;

            InitGridList();
            base.SetActiveBtnPilih(listOfItemJual.Count > 0);
        }

        public FrmLookupItemNota(string header, IList<ItemBeliProduk> listOfItemBeli)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            this._listOfItemBeli = listOfItemBeli;
            this._referensiType = ReferencesType.NotaBeliProduk;

            InitGridList();
            base.SetActiveBtnPilih(listOfItemBeli.Count > 0);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            switch (_referensiType)
            {
                case ReferencesType.NotaJualProduk:
                case ReferencesType.NotaBeliProduk:
                    gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Nama Produk", Width = 300 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Jumlah", Width = 50 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Harga", Width = 90 });
                    gridListProperties.Add(new GridListControlProperties { Header = "Sub Total", Width = 90 });

                    break;

                default:
                    break;
            }

            var listCount = 0;

            switch (_referensiType)
            {
                case ReferencesType.NotaJualProduk:
                    GridListControlHelper.InitializeGridListControl<ItemJualProduk>(this.gridList, _listOfItemJual, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridItemJualProduk_QueryCellInfo;

                    listCount = _listOfItemJual.Count;

                    break;

                case ReferencesType.NotaBeliProduk:
                    GridListControlHelper.InitializeGridListControl<ItemBeliProduk>(this.gridList, _listOfItemBeli, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridItemBeliProduk_QueryCellInfo;

                    listCount = _listOfItemBeli.Count;

                    break;

                default:
                    break;
            }

            if (listCount > 0)
                this.gridList.SetSelected(0, true);
        }

        private void GridItemBeliProduk_QueryCellInfo(object sender, Syncfusion.Windows.Forms.Grid.GridQueryCellInfoEventArgs e)
        {
            if (_listOfItemBeli.Count > 0)
            {
                if (e.RowIndex > 0)
                {
                    var rowIndex = e.RowIndex - 1;

                    if (rowIndex < _listOfItemBeli.Count)
                    {
                        var obj = _listOfItemBeli[rowIndex];
                        var produk = obj.Produk;

                        switch (e.ColIndex)
                        {
                            case 2:
                                if (produk != null)
                                    e.Style.CellValue = produk.nama_produk;
                                break;

                            case 3:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                e.Style.CellValue = obj.jumlah - obj.jumlah_retur;
                                break;

                            case 4:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                e.Style.CellValue = NumberHelper.NumberToString(obj.harga);
                                break;

                            case 5:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                e.Style.CellValue = NumberHelper.NumberToString((obj.jumlah - obj.jumlah_retur) * obj.harga);
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

        private void GridItemJualProduk_QueryCellInfo(object sender, Syncfusion.Windows.Forms.Grid.GridQueryCellInfoEventArgs e)
        {
            if (_listOfItemJual.Count > 0)
            {
                if (e.RowIndex > 0)
                {
                    var rowIndex = e.RowIndex - 1;

                    if (rowIndex < _listOfItemJual.Count)
                    {
                        var obj = _listOfItemJual[rowIndex];
                        var produk = obj.Produk;

                        switch (e.ColIndex)
                        {
                            case 2:
                                if (produk != null)
                                    e.Style.CellValue = produk.nama_produk;
                                break;

                            case 3:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                e.Style.CellValue = obj.jumlah - obj.jumlah_retur;
                                break;

                            case 4:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                e.Style.CellValue = NumberHelper.NumberToString(obj.harga_jual);
                                break;

                            case 5:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                e.Style.CellValue = NumberHelper.NumberToString((obj.jumlah - obj.jumlah_retur) * obj.harga_jual);
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

        protected override void Pilih()
        {
            var rowIndex = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(rowIndex, this.Text))
                return;

            switch (_referensiType)
            {
                case ReferencesType.NotaJualProduk:
                    var itemJual = _listOfItemJual[rowIndex];
                    this.Listener.Ok(this, itemJual);
                    break;

                case ReferencesType.NotaBeliProduk:
                    var itemBeli = _listOfItemBeli[rowIndex];
                    this.Listener.Ok(this, itemBeli);
                    break;

                default:
                    break;
            }

            this.Close();
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (base.IsButtonPilihEnabled)
                Pilih();
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
