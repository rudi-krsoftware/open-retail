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
    public partial class FrmLookupNota : FrmLookupEmptyBody
    {
        private IList<BeliProduk> _listOfNotaBeliProduk = null;
        private IList<JualProduk> _listOfNotaJualProduk = null;

        private ReferencesType _referensiType = ReferencesType.NotaBeliProduk;

        public IListener Listener { private get; set; }

        public FrmLookupNota(string header, IList<BeliProduk> listOfNotaBeliProduk)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            this._listOfNotaBeliProduk = listOfNotaBeliProduk;
            this._referensiType = ReferencesType.NotaBeliProduk;

            InitGridList();
            base.SetActiveBtnPilih(listOfNotaBeliProduk.Count > 0);
        }

        public FrmLookupNota(string header, IList<JualProduk> listOfNotaJualProduk)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            this._listOfNotaJualProduk = listOfNotaJualProduk;
            this._referensiType = ReferencesType.NotaJualProduk;

            InitGridList();
            base.SetActiveBtnPilih(listOfNotaJualProduk.Count > 0);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 80, HorizontalAlignment = GridHorizontalAlignment.Center });
            gridListProperties.Add(new GridListControlProperties { Header = "Tempo", Width = 80, HorizontalAlignment = GridHorizontalAlignment.Center });

            if (this._referensiType == ReferencesType.NotaJualProduk)
            {
                gridListProperties.Add(new GridListControlProperties { Header = "Nota Jual", Width = 90, HorizontalAlignment = GridHorizontalAlignment.Center });
                gridListProperties.Add(new GridListControlProperties { Header = "Customer", Width = 170 });
                gridListProperties.Add(new GridListControlProperties { Header = "Sisa Piutang", HorizontalAlignment = GridHorizontalAlignment.Right });
            }
            else
            {
                gridListProperties.Add(new GridListControlProperties { Header = "Nota Beli", Width = 90, HorizontalAlignment = GridHorizontalAlignment.Center });
                gridListProperties.Add(new GridListControlProperties { Header = "Supplier", Width = 170 });
                gridListProperties.Add(new GridListControlProperties { Header = "Sisa Hutang", HorizontalAlignment = GridHorizontalAlignment.Right });
            }

            var listCount = 0;

            switch (this._referensiType)
            {
                case ReferencesType.NotaBeliProduk:
                    GridListControlHelper.InitializeGridListControl<BeliProduk>(this.gridList, _listOfNotaBeliProduk, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridNotaBeliProduk_QueryCellInfo;

                    listCount = _listOfNotaBeliProduk.Count;

                    break;

                case ReferencesType.NotaJualProduk:
                    GridListControlHelper.InitializeGridListControl<JualProduk>(this.gridList, _listOfNotaJualProduk, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridNotaJualProduk_QueryCellInfo;

                    listCount = _listOfNotaJualProduk.Count;

                    break;

                default:
                    break;
            }

            if (listCount > 0)
                this.gridList.SetSelected(0, true);
        }

        private void GridNotaJualProduk_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {            
            if (_listOfNotaJualProduk.Count > 0)
            {
                if (e.RowIndex > 0)
                {
                    var rowIndex = e.RowIndex - 1;

                    if (rowIndex < _listOfNotaJualProduk.Count)
                    {
                        var notaJual = _listOfNotaJualProduk[rowIndex];

                        switch (e.ColIndex)
                        {
                            case 2: // tanggal
                                e.Style.CellValue = DateTimeHelper.DateToString(notaJual.tanggal);
                                break;

                            case 3: // tempo
                                e.Style.CellValue = DateTimeHelper.DateToString(notaJual.tanggal_tempo);
                                break;

                            case 4: // nota
                                e.Style.CellValue = notaJual.nota;
                                break;

                            case 5: // customer
                                var customer = notaJual.Customer;

                                if (customer != null)
                                    e.Style.CellValue = customer.nama_customer;

                                break;

                            case 6: // sisa
                                e.Style.CellValue = NumberHelper.NumberToString(notaJual.sisa_nota);
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

        private void GridNotaBeliProduk_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {            
            if (_listOfNotaBeliProduk.Count > 0)
            {
                if (e.RowIndex > 0)
                {
                    var rowIndex = e.RowIndex - 1;

                    if (rowIndex < _listOfNotaBeliProduk.Count)
                    {
                        var notaBeli = _listOfNotaBeliProduk[rowIndex];

                        switch (e.ColIndex)
                        {
                            case 2: // tanggal
                                e.Style.CellValue = DateTimeHelper.DateToString(notaBeli.tanggal);
                                break;

                            case 3: // tempo
                                e.Style.CellValue = DateTimeHelper.DateToString(notaBeli.tanggal_tempo);
                                break;

                            case 4: // nota
                                e.Style.CellValue = notaBeli.nota;
                                break;

                            case 5: // supplier
                                var supplier = notaBeli.Supplier;

                                if (supplier != null)
                                    e.Style.CellValue = supplier.nama_supplier;

                                break;

                            case 6: // sisa
                                e.Style.CellValue = NumberHelper.NumberToString(notaBeli.sisa_nota);
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

        private void gridList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                if (base.IsButtonPilihEnabled)
                    Pilih();
            }
        }

        protected override void Pilih()
        {
            var rowIndex = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(rowIndex, this.Text))
                return;

            switch (this._referensiType)
            {
                case ReferencesType.NotaBeliProduk:
                    var notaBeliProduk = _listOfNotaBeliProduk[rowIndex];
                    this.Listener.Ok(this, notaBeliProduk);

                    break;

                case ReferencesType.NotaJualProduk:
                    var notaJualProduk = _listOfNotaJualProduk[rowIndex];
                    this.Listener.Ok(this, notaJualProduk);
                    break;

                default:
                    break;
            }

            this.Close();
        }        
    }
}
