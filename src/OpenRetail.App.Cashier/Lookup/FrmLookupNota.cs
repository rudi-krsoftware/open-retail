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

using log4net;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.Helper;
using OpenRetail.Helper.RAWPrinting;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Model;
using Syncfusion.Windows.Forms.Grid;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OpenRetail.App.Cashier.Lookup
{
    public partial class FrmLookupNota : FrmLookupEmptyBody
    {
        private IList<JualProduk> _listOfJual = null;
        private PengaturanUmum _pengaturanUmum;
        private ILog _log;

        public FrmLookupNota(string header, IList<JualProduk> listOfJual)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);

            this._pengaturanUmum = MainProgram.pengaturanUmum;
            this._log = MainProgram.log;
            this._listOfJual = listOfJual;

            InitGridList();
            base.SetActiveBtnPilih(listOfJual.Count > 0);
            base.SetTitleBtnPilih("F10 Cetak");
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 50 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 120 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nota", Width = 170 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nominal", Width = 150 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kasir" });

            GridListControlHelper.InitializeGridListControl<JualProduk>(this.gridList, _listOfJual, gridListProperties);

            this.gridList.Grid.QueryRowHeight += delegate (object sender, GridRowColSizeEventArgs e)
            {
                e.Size = 27;
                e.Handled = true;
            };

            this.gridList.Grid.QueryCellInfo += GridNota_QueryCellInfo;

            if (_listOfJual.Count > 0)
                this.gridList.SetSelected(0, true);
        }

        private void GridNota_QueryCellInfo(object sender, GridQueryCellInfoEventArgs e)
        {
            if (_listOfJual.Count > 0)
            {
                if (e.RowIndex > 0)
                {
                    e.Style.Font = new GridFontInfo(new Font("Arial", 14f));

                    var rowIndex = e.RowIndex - 1;

                    if (rowIndex < _listOfJual.Count)
                    {
                        var jual = _listOfJual[rowIndex];

                        switch (e.ColIndex)
                        {
                            case 2:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                e.Style.CellValue = DateTimeHelper.DateToString(jual.tanggal);
                                break;

                            case 3:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                e.Style.CellValue = jual.nota;
                                break;

                            case 4:
                                e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                e.Style.CellValue = NumberHelper.NumberToString(jual.grand_total);
                                break;

                            case 5:
                                var kasir = jual.Pengguna;
                                if (kasir != null) e.Style.CellValue = kasir.nama_pengguna;
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

            if (_pengaturanUmum.is_auto_print)
            {
                if (MsgHelper.MsgKonfirmasi("Apakah proses pencetakan ingin dilanjutkan ?"))
                {
                    var jual = _listOfJual[rowIndex];
                    if (jual != null)
                    {
                        IJualProdukBll bll = new JualProdukBll(_log);
                        jual.item_jual = bll.GetItemJual(jual.jual_id).ToList();
                    }

                    switch (_pengaturanUmum.jenis_printer)
                    {
                        case JenisPrinter.DotMatrix:
                            CetakNotaDotMatrix(jual);
                            break;

                        case JenisPrinter.MiniPOS:
                            CetakNotaMiniPOS(jual);
                            break;

                        default:
                            // do nothing
                            break;
                    }
                }
            }
        }

        private void CetakNotaDotMatrix(JualProduk jual)
        {
            IRAWPrinting printerMiniPos = new PrinterDotMatrix(_pengaturanUmum.nama_printer);
            printerMiniPos.Cetak(jual, _pengaturanUmum.list_of_header_nota);
        }

        private void CetakNotaMiniPOS(JualProduk jual)
        {
            var autocutCode = _pengaturanUmum.is_autocut ? _pengaturanUmum.autocut_code : string.Empty;
            var openCashDrawerCode = _pengaturanUmum.is_open_cash_drawer ? _pengaturanUmum.open_cash_drawer_code : string.Empty;

            IRAWPrinting printerMiniPos = new PrinterMiniPOS(_pengaturanUmum.nama_printer);

            printerMiniPos.Cetak(jual, _pengaturanUmum.list_of_header_nota_mini_pos, _pengaturanUmum.list_of_footer_nota_mini_pos,
                _pengaturanUmum.jumlah_karakter, _pengaturanUmum.jumlah_gulung, _pengaturanUmum.is_cetak_customer, ukuranFont: _pengaturanUmum.ukuran_font,
                autocutCode: autocutCode, openCashDrawerCode: openCashDrawerCode);
        }
    }
}