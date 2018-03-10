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
using OpenRetail.Helper;
using Syncfusion.Windows.Forms.Grid;
using ConceptCave.WaitCursor;
using log4net;

namespace OpenRetail.App.Lookup
{
    public partial class FrmLookupHistoriPembayaran : Form
    {
        private IList<ItemPembayaranPiutangProduk> _listOfHistoriPembayaran = null;
        private ILog _log;

        public FrmLookupHistoriPembayaran(string header, JualProduk jual, IList<ItemPembayaranPiutangProduk> listOfHistoriPembayaran)
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            this.Text = header;
            this.lblHeader.Text = header;
            this.groupBox1.Text = " [ Informasi Penjualan ] ";
            this.label3.Text = "Customer";
            this._listOfHistoriPembayaran = listOfHistoriPembayaran;
            this._log = MainProgram.log;

            txtTanggal.Text = DateTimeHelper.DateToString(jual.tanggal);
            txtNota.Text = jual.nota;
            txtCustomerOrSupplier.Text = jual.Customer.nama_customer;

            InitGridList();
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nota", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nominal", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan", Width = 250 });
            gridListProperties.Add(new GridListControlProperties { Header = "Operator" });

            GridListControlHelper.InitializeGridListControl<ItemPembayaranPiutangProduk>(this.gridList, _listOfHistoriPembayaran, gridListProperties);

            if (_listOfHistoriPembayaran.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                if (_listOfHistoriPembayaran.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfHistoriPembayaran.Count)
                        {
                            var historiPembayaran = _listOfHistoriPembayaran[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(historiPembayaran.PembayaranPiutangProduk.tanggal);
                                    break;

                                case 3:
                                    e.Style.CellValue = historiPembayaran.PembayaranPiutangProduk.nota;
                                    break;

                                case 4:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(historiPembayaran.nominal);
                                    break;

                                case 5:
                                    e.Style.CellValue = historiPembayaran.keterangan;
                                    break;

                                case 6:
                                    e.Style.CellValue = historiPembayaran.PembayaranPiutangProduk.Pengguna.nama_pengguna;
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

        private void FrmLookupHistoriPembayaran_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEsc(e))
                this.Close();
        }
    }
}
