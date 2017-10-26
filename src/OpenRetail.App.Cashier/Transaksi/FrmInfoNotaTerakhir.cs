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
using OpenRetail.Helper.UI.Template;
using OpenRetail.Helper;
using ConceptCave.WaitCursor;
using OpenRetail.Helper.RAWPrinting;
using log4net;
using Syncfusion.Windows.Forms.Grid;

namespace OpenRetail.App.Cashier.Transaksi
{
    public partial class FrmInfoNotaTerakhir : FrmDialogInfo
    {
        private JualProduk _jual;

        public FrmInfoNotaTerakhir(string header, JualProduk jual)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._jual = jual;

            ShowInfoNota();
            InitGridList();
        }

        private void ShowInfoNota()
        {
            // info header nota
            if (this._jual.Customer != null)
                txtPelanggan.Text = this._jual.Customer.nama_customer;

            var dt = (DateTime)this._jual.tanggal_sistem;

            var tanggal = string.Format("{0}, {1}", DayMonthHelper.GetHariIndonesia(dt), dt.Day + " " + DayMonthHelper.GetBulanIndonesia(dt.Month) + " " + dt.Year);
            var jam = string.Format("{0:HH:mm:ss}", dt);

            txtNotaTanggal.Text = string.Format("{0} / {1} {2}", this._jual.nota, tanggal, jam);

            // info footer nota
            txtDiskon.Text = this._jual.diskon.ToString();
            txtPPN.Text = this._jual.ppn.ToString();
            txtGrandTotal.Text = this._jual.grand_total.ToString();
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();


            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kode Produk", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama Produk", Width = 270 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jumlah", Width = 50 });
            gridListProperties.Add(new GridListControlProperties { Header = "Diskon", Width = 50 });
            gridListProperties.Add(new GridListControlProperties { Header = "Harga", Width = 75 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sub Total", Width = 100 });

            var listOfItemJual = _jual.item_jual;

            GridListControlHelper.InitializeGridListControl<ItemJualProduk>(this.gridList, listOfItemJual, gridListProperties);

            if (listOfItemJual.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                if (listOfItemJual.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < listOfItemJual.Count)
                        {
                            var itemJual = listOfItemJual[rowIndex];
                            var produk = itemJual.Produk;

                            switch (e.ColIndex)
                            {
                                case 2:
                                    if (produk != null)
                                        e.Style.CellValue = produk.kode_produk;

                                    break;

                                case 3: // nama produk
                                    if (produk != null)
                                        e.Style.CellValue = produk.nama_produk;

                                    break;

                                case 4: // jumlah
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = itemJual.jumlah - itemJual.jumlah_retur;

                                    break;

                                case 5: // diskon
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = itemJual.diskon;

                                    break;

                                case 6: // harga
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(itemJual.harga_jual);

                                    break;

                                case 7: // subtotal
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;

                                    var jumlah = itemJual.jumlah - itemJual.jumlah_retur;
                                    e.Style.CellValue = NumberHelper.NumberToString(jumlah * itemJual.harga_setelah_diskon);
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
    }
}
