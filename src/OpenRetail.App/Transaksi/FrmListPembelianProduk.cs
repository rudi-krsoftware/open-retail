﻿/**
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
using OpenRetail.App.UI.Template;
using OpenRetail.App.Helper;
using Syncfusion.Windows.Forms.Grid;
using ConceptCave.WaitCursor;

namespace OpenRetail.App.Transaksi
{
    public partial class FrmListPembelianProduk : FrmListEmptyBody, IListener
    {
        private IBeliProdukBll _bll; // deklarasi objek business logic layer 
        private IList<BeliProduk> _listOfBeli = new List<BeliProduk>();
        
        public FrmListPembelianProduk(string header)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            base.WindowState = FormWindowState.Maximized;

            _bll = new BeliProdukBll();

            dtpTanggalMulai.Value = DateTime.Today;
            dtpTanggalSelesai.Value = DateTime.Today;
            LoadData(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);

            InitGridList();
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tempo", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nota", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Supplier", Width = 300 });
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan", Width = 300 });
            gridListProperties.Add(new GridListControlProperties { Header = "Hutang", Width = 150 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sisa Hutang" });

            GridListControlHelper.InitializeGridListControl<BeliProduk>(this.gridList, _listOfBeli, gridListProperties);

            if (_listOfBeli.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {

                if (_listOfBeli.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfBeli.Count)
                        {
                            double totalNota = 0;

                            var obj = _listOfBeli[rowIndex];
                            if (obj != null)
                                totalNota = obj.total_nota_setelah_diskon_dan_ppn;


                            var isRetur = obj.retur_beli_produk_id != null;

                            if (isRetur)
                                e.Style.BackColor = Color.Red;

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(obj.tanggal);
                                    break;

                                case 3:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(obj.tanggal_tempo);
                                    break;

                                case 4:
                                    e.Style.CellValue = obj.nota;
                                    break;

                                case 5:
                                    if (obj.Supplier != null)
                                        e.Style.CellValue = obj.Supplier.nama_supplier;

                                    break;

                                case 6:
                                    e.Style.CellValue = obj.keterangan;
                                    break;

                                case 7:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(totalNota);
                                    break;

                                case 8:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(totalNota - obj.total_pelunasan);
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

        private void LoadData()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfBeli = _bll.GetAll();
                GridListControlHelper.Refresh<BeliProduk>(this.gridList, _listOfBeli);
            }

            ResetButton();
        }

        private void LoadData(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfBeli = _bll.GetByTanggal(tanggalMulai, tanggalSelesai);
                GridListControlHelper.Refresh<BeliProduk>(this.gridList, _listOfBeli);
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfBeli.Count > 0);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryPembelianProduk("Tambah Data " + this.Text, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var beli = _listOfBeli[index];
            beli.tanggal_tempo_old = beli.tanggal_tempo;

            var frm = new FrmEntryPembelianProduk("Edit Data " + this.Text, beli, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Hapus()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.Text))
                return;

            if (MsgHelper.MsgDelete())
            {
                var obj = _listOfBeli[index];

                var result = _bll.Delete(obj);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<BeliProduk>(this.gridList, _listOfBeli, obj);
                    ResetButton();
                }
                else
                    MsgHelper.MsgDeleteError();
            }
        }

        public void Ok(object sender, object data)
        {
            throw new NotImplementedException();
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            var beli = (BeliProduk)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<BeliProduk>(this.gridList, _listOfBeli, beli);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<BeliProduk>(this.gridList, _listOfBeli, beli);
        }

        private void chkTampilkanSemuaData_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            var isEnable = false;

            if (chk.Checked)
                isEnable = false;
            else
                isEnable = true;

            dtpTanggalMulai.Enabled = isEnable;
            dtpTanggalSelesai.Enabled = isEnable;
            btnTampilkan.Enabled = isEnable;

            if (!isEnable)
                LoadData();
            else
                LoadData(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
        }

        private void btnTampilkan_Click(object sender, EventArgs e)
        {
            if (!DateTimeHelper.IsValidRangeTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value))
            {
                MsgHelper.MsgNotValidRangeTanggal();
                return;
            }

            LoadData(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (btnPerbaiki.Enabled)
                Perbaiki();
        }
    }
}
