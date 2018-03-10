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
        private IList<ItemPembayaranPiutangProduk> _listOfHistoriPembayaranPiutang = null;
        private IList<ItemPembayaranHutangProduk> _listOfHistoriPembayaranHutang = null;
        private PaymentHistoryType _paymentType = PaymentHistoryType.PembayaranHutang;
        private ILog _log;

        public FrmLookupHistoriPembayaran(string header)
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);
            this._log = MainProgram.log;

            this.Text = header;
            this.lblHeader.Text = header;
        }

        public FrmLookupHistoriPembayaran(string header, BeliProduk beli, IList<ItemPembayaranHutangProduk> listOfHistoriPembayaran)
            : this(header)
        {
            this.groupBox1.Text = " [ Informasi Pembelian ] ";
            this.label3.Text = "Supplier";
            this._listOfHistoriPembayaranHutang = listOfHistoriPembayaran;
            this._paymentType = PaymentHistoryType.PembayaranHutang;

            txtTanggal.Text = DateTimeHelper.DateToString(beli.tanggal);
            txtNota.Text = beli.nota;
            txtCustomerOrSupplier.Text = beli.Supplier.nama_supplier;
            txtTotal.Text = NumberHelper.NumberToString(SumGrid(listOfHistoriPembayaran));

            InitGridList();
        }

        public FrmLookupHistoriPembayaran(string header, JualProduk jual, IList<ItemPembayaranPiutangProduk> listOfHistoriPembayaran)
            : this(header)
        {
            this.groupBox1.Text = " [ Informasi Penjualan ] ";
            this.label3.Text = "Customer";
            this._listOfHistoriPembayaranPiutang = listOfHistoriPembayaran;
            this._paymentType = PaymentHistoryType.PembayaranPiutang;

            txtTanggal.Text = DateTimeHelper.DateToString(jual.tanggal);
            txtNota.Text = jual.nota;
            txtCustomerOrSupplier.Text = jual.Customer.nama_customer;
            txtTotal.Text = NumberHelper.NumberToString(SumGrid(listOfHistoriPembayaran));
            InitGridList();
        }

        private double SumGrid(IList<ItemPembayaranHutangProduk> listOfHistoriPembayaran)
        {
            return listOfHistoriPembayaran.Sum(f => f.nominal);
        }

        private double SumGrid(IList<ItemPembayaranPiutangProduk> listOfHistoriPembayaran)
        {
            return listOfHistoriPembayaran.Sum(f => f.nominal);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nota", Width = 90 });            
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan", Width = 250 });
            gridListProperties.Add(new GridListControlProperties { Header = "Operator", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nominal" });

            var listCount = 0;

            switch (this._paymentType)
            {
                case PaymentHistoryType.PembayaranHutang:
                    GridListControlHelper.InitializeGridListControl<ItemPembayaranHutangProduk>(this.gridList, _listOfHistoriPembayaranHutang, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridPembayaranHutang_QueryCellInfo;
                    listCount = _listOfHistoriPembayaranHutang.Count;

                    break;

                case PaymentHistoryType.PembayaranPiutang:
                    GridListControlHelper.InitializeGridListControl<ItemPembayaranPiutangProduk>(this.gridList, _listOfHistoriPembayaranPiutang, gridListProperties);
                    this.gridList.Grid.QueryCellInfo += GridPembayaranPiutang_QueryCellInfo;
                    listCount = _listOfHistoriPembayaranPiutang.Count;

                    break;

                default:
                    break;
            }

            if (listCount > 0)
                this.gridList.SetSelected(0, true);
        }

        private void GridPembayaranHutang_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            if (_listOfHistoriPembayaranHutang.Count > 0)
            {
                if (e.RowIndex > 0)
                {
                    var rowIndex = e.RowIndex - 1;

                    if (rowIndex < _listOfHistoriPembayaranHutang.Count)
                    {
                        var historiPembayaran = _listOfHistoriPembayaranHutang[rowIndex];

                        switch (e.ColIndex)
                        {
                            case 2:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                e.Style.CellValue = DateTimeHelper.DateToString(historiPembayaran.PembayaranHutangProduk.tanggal);
                                break;

                            case 3:
                                e.Style.CellValue = historiPembayaran.PembayaranHutangProduk.nota;
                                break;                            

                            case 4:
                                e.Style.CellValue = historiPembayaran.keterangan;
                                break;

                            case 5:
                                e.Style.CellValue = historiPembayaran.PembayaranHutangProduk.Pengguna.nama_pengguna;
                                break;

                            case 6:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                e.Style.CellValue = NumberHelper.NumberToString(historiPembayaran.nominal);
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

        void GridPembayaranPiutang_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {            
            if (_listOfHistoriPembayaranPiutang.Count > 0)
            {
                if (e.RowIndex > 0)
                {
                    var rowIndex = e.RowIndex - 1;

                    if (rowIndex < _listOfHistoriPembayaranPiutang.Count)
                    {
                        var historiPembayaran = _listOfHistoriPembayaranPiutang[rowIndex];

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
                                e.Style.CellValue = historiPembayaran.keterangan;
                                break;

                            case 5:
                                e.Style.CellValue = historiPembayaran.PembayaranPiutangProduk.Pengguna.nama_pengguna;
                                break;

                            case 6:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                e.Style.CellValue = NumberHelper.NumberToString(historiPembayaran.nominal);
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
