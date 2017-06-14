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
using OpenRetail.App.Helper;
using Syncfusion.Windows.Forms.Grid;
using ConceptCave.WaitCursor;
using log4net;

namespace OpenRetail.App.Lookup
{
    public partial class FrmLookupMinimalStok : Form
    {
        private IList<Produk> _listOfProduk = null;
        private ILog _log;

        public FrmLookupMinimalStok(string header, IList<Produk> listOfProduk)
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            this.Text = header;
            this.lblHeader.Text = header;
            this._listOfProduk = listOfProduk;
            this._log = MainProgram.log;

            InitGridList();
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Golongan", Width = 120 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kode Produk", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama Produk", Width = 250 });
            gridListProperties.Add(new GridListControlProperties { Header = "Stok Etalase", Width = 60 });
            gridListProperties.Add(new GridListControlProperties { Header = "Stok Gudang", Width = 60 });
            gridListProperties.Add(new GridListControlProperties { Header = "Min. Stok Gudang" });
            
            GridListControlHelper.InitializeGridListControl<Produk>(this.gridList, _listOfProduk, gridListProperties, rowHeight:40);

            if (_listOfProduk.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
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
                                    e.Style.CellValue = produk.Golongan.nama_golongan;
                                    break;

                                case 3:
                                    e.Style.CellValue = produk.kode_produk;
                                    break;

                                case 4:
                                    e.Style.CellValue = produk.nama_produk;
                                    break;

                                case 5:
                                    e.Style.CellValue = produk.stok;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 6:
                                    e.Style.CellValue = produk.stok_gudang;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 7:
                                    e.Style.CellValue = produk.minimal_stok_gudang;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
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

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmLookupMinimalStok_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEsc(e))
                this.Close();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (var dlgSave = new SaveFileDialog())
            {
                dlgSave.Filter = "Microsoft Excel files (*.xlsx)|*.xlsx";
                dlgSave.Title = "Export Data Produk";

                var result = dlgSave.ShowDialog();
                if (result == DialogResult.OK)
                {
                    using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                    {
                        IImportExportDataBll<Produk> _importDataBll = new ImportExportDataProdukBll(dlgSave.FileName, _log);
                        _importDataBll.Export(_listOfProduk);
                    }
                }
            }
        }
    }
}
