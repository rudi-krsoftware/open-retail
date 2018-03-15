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
using OpenRetail.Helper.UI.Template;
using OpenRetail.Helper;
using Syncfusion.Windows.Forms.Grid;
using ConceptCave.WaitCursor;
using log4net;
using Microsoft.Reporting.WinForms;
using OpenRetail.Helper.RAWPrinting;
using OpenRetail.App.Lookup;

namespace OpenRetail.App.Transaksi
{
    public partial class FrmListPenjualanProdukWithNavigation : FrmListEmptyBodyWithNavigation, IListener
    {
        private IJualProdukBll _bll; // deklarasi objek business logic layer 
        private IList<JualProduk> _listOfJual = new List<JualProduk>();
        private IList<Wilayah> _listOfWilayah = new List<Wilayah>();
        private ILog _log;
        private Pengguna _pengguna;
        private PengaturanUmum _pengaturanUmum;

        private int _pageNumber = 1;
        private int _pagesCount = 0;
        private int _pageSize = 0;
        private string _menuId;

        public FrmListPenjualanProdukWithNavigation(string header, Pengguna pengguna, string menuId)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.WindowState = FormWindowState.Maximized;

            _pageSize = MainProgram.pageSize;
            _log = MainProgram.log;
            _listOfWilayah = MainProgram.ListOfWilayah;
            _bll = new JualProdukBll(_log);
            _pengguna = pengguna;
            _pengaturanUmum = MainProgram.pengaturanUmum;
            _menuId = menuId;            

            // set hak akses untuk SELECT
            var role = _pengguna.GetRoleByMenuAndGrant(_menuId, GrantState.SELECT);
            if (role != null)
            {
                if (role.is_grant)
                {
                    this.updLimit.Value = _pageSize;
                    LoadData(filterRangeTanggal.TanggalMulai, filterRangeTanggal.TanggalSelesai);
                }                    

                txtNamaCustomer.Enabled = role.is_grant;
                btnCari.Enabled = role.is_grant;

                filterRangeTanggal.Enabled = role.is_grant;
            }            

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfJual.Count);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tempo", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nota", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Customer", Width = _pengaturanUmum.jenis_printer == JenisPrinter.InkJet ? 180 : 260 });
            gridListProperties.Add(new GridListControlProperties { Header = "Keterangan", Width = 350 });
            gridListProperties.Add(new GridListControlProperties { Header = "Piutang", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sisa Piutang", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Histori Pembayaran", Width = 80 });

            if (_pengaturanUmum.jenis_printer == JenisPrinter.InkJet)
            {
                gridListProperties.Add(new GridListControlProperties { Header = "Cetak Nota/Label", Width = 80 });
                gridListProperties.Add(new GridListControlProperties { Header = "" });
            }
            else
            {
                gridListProperties.Add(new GridListControlProperties { Header = "Cetak Nota", Width = 80 });
            }

            

            GridListControlHelper.InitializeGridListControl<JualProduk>(this.gridList, _listOfJual, gridListProperties, false, rowHeight: 40);

            if (_pengaturanUmum.jenis_printer == JenisPrinter.InkJet)
            {
                // merge header kolom cetak nota/label
                this.gridList.Grid.CoveredRanges.Add(GridRangeInfo.Cells(0, 10, 0, 11));
            }            

            if (_listOfJual.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.PushButtonClick += delegate(object sender, GridCellPushButtonClickEventArgs e)
            {
                if (e.RowIndex > 0)
                {
                    var index = e.RowIndex - 1;

                    switch (e.ColIndex)
                    {
                        case 9: // histori pembayaran
                            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                            {
                                var jual = _listOfJual[index];

                                IPembayaranPiutangProdukBll bll = new PembayaranPiutangProdukBll(_log);
                                var listOfHistoriPembayaran = bll.GetHistoriPembayaran(jual.jual_id);

                                if (listOfHistoriPembayaran.Count > 0)
                                {
                                    var frmHistoriPembayaran = new FrmLookupHistoriPembayaran("Histori Pembayaran Piutang", jual, listOfHistoriPembayaran);
                                    frmHistoriPembayaran.ShowDialog();
                                }
                                else
                                {
                                    MsgHelper.MsgInfo("Belum ada informasi histori pembayaran");
                                }
                            }

                            break;

                        case 10: // cetak nota jual
                            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                            {
                                var jual = _listOfJual[index];

                                switch (this._pengaturanUmum.jenis_printer)
                                {
                                    case JenisPrinter.DotMatrix:
                                        if (MsgHelper.MsgKonfirmasi("Apakah proses pencetakan ingin dilanjutkan ?"))
                                            CetakNotaDotMatrix(jual);
                                        break;

                                    case JenisPrinter.MiniPOS:
                                        if (MsgHelper.MsgKonfirmasi("Apakah proses pencetakan ingin dilanjutkan ?"))
                                            CetakNotaMiniPOS(jual);
                                        break;

                                    default:
                                        var frmCetakNota = new FrmPreviewNotaPenjualan("Preview Nota Penjualan", jual);
                                        frmCetakNota.ShowDialog();
                                        break;
                                }
                            }

                            break;

                        case 11: // cetak label nota jual
                            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                            {
                                var jual = _listOfJual[index];

                                var frmCetakLabelNota = new FrmPreviewLabelNotaPenjualan("Preview Label Nota Penjualan", jual);
                                frmCetakLabelNota.ShowDialog();
                            }

                            break;

                        default:
                            break;
                    }
                }                
            };

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {

                if (_listOfJual.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfJual.Count)
                        {
                            double totalNota = 0;

                            var jual = _listOfJual[rowIndex];
                            if (jual != null)
                                totalNota = jual.grand_total;


                            var isRetur = jual.retur_jual_id != null;
                            var oldStyleBackColor = e.Style.BackColor;

                            if (isRetur)
                                e.Style.BackColor = Color.Red;

                            switch (e.ColIndex)
                            {
                                case 1:
                                    var noUrut = (_pageNumber - 1) * _pageSize + e.RowIndex;
                                    e.Style.CellValue = noUrut;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    break;

                                case 2:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(jual.tanggal);
                                    break;

                                case 3:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(jual.tanggal_tempo);
                                    break;

                                case 4:
                                    e.Style.CellValue = jual.nota;
                                    break;

                                case 5:
                                    if (jual.Customer != null)
                                    {
                                        SetWilayahCustomer(jual.Customer);
                                        e.Style.CellValue = jual.Customer.nama_customer;
                                    }                                        

                                    break;

                                case 6:
                                    e.Style.CellValue = jual.keterangan;
                                    break;

                                case 7:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(totalNota);
                                    break;

                                case 8:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(totalNota - jual.total_pelunasan);
                                    break;

                                case 9: // button history pembayaran
                                    e.Style.Enabled = jual.tanggal_tempo != null;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellType = GridCellTypeName.PushButton;                                    
                                    e.Style.BackColor = oldStyleBackColor;
                                    e.Style.Description = "Cek Histori";

                                    break;

                                case 10: // button cetak nota
                                    e.Style.Enabled = jual.Customer != null;
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellType = GridCellTypeName.PushButton;                                    
                                    e.Style.BackColor = oldStyleBackColor;
                                    e.Style.Description = "Cetak Nota";
                                    break;

                                case 11: // button cetak label nota
                                    if (_pengaturanUmum.jenis_printer == JenisPrinter.InkJet)
                                    {
                                        e.Style.Enabled = jual.Customer != null;
                                        e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                        e.Style.CellType = GridCellTypeName.PushButton;
                                        e.Style.BackColor = oldStyleBackColor;
                                        e.Style.Description = "Cetak Label Nota";
                                    }                                    

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

        private void SetWilayahCustomer(Customer obj)
        {
            Provinsi provinsi = null;
            Kabupaten kabupaten = null;
            Kecamatan kecamatan = null;

            if (!string.IsNullOrEmpty(obj.provinsi_id))
            {
                provinsi = _listOfWilayah.Where(f => f.provinsi_id == obj.provinsi_id)
                                         .Select(f => new Provinsi { provinsi_id = f.provinsi_id, nama_provinsi = f.nama_provinsi })
                                         .FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(obj.kabupaten_id))
            {
                kabupaten = _listOfWilayah.Where(f => f.kabupaten_id == obj.kabupaten_id)
                                          .Select(f => new Kabupaten { kabupaten_id = f.kabupaten_id, nama_kabupaten = f.nama_kabupaten })
                                          .FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(obj.kecamatan_id))
            {
                kecamatan = _listOfWilayah.Where(f => f.kecamatan_id == obj.kecamatan_id)
                                          .Select(f => new Kecamatan { kecamatan_id = f.kecamatan_id, nama_kecamatan = f.nama_kecamatan })
                                          .FirstOrDefault();
            }

            obj.Provinsi = provinsi;
            obj.Kabupaten = kabupaten;
            obj.Kecamatan = kecamatan;
        }

        private void CetakNotaMiniPOS(JualProduk jual)
        {
            IRAWPrinting printerMiniPos = new PrinterMiniPOS(_pengaturanUmum.nama_printer);
            printerMiniPos.Cetak(jual, _pengaturanUmum.list_of_header_nota_mini_pos, _pengaturanUmum.list_of_footer_nota_mini_pos, 
                _pengaturanUmum.jumlah_karakter, _pengaturanUmum.jumlah_gulung, _pengaturanUmum.is_cetak_customer, ukuranFont: _pengaturanUmum.ukuran_font);
        }

        private void CetakNotaDotMatrix(JualProduk jual)
        {
            IRAWPrinting printerMiniPos = new PrinterDotMatrix(_pengaturanUmum.nama_printer);
            printerMiniPos.Cetak(jual, _pengaturanUmum.list_of_header_nota, _pengaturanUmum.jumlah_gulung, isCetakKeteranganNota: _pengaturanUmum.is_cetak_keterangan_nota);
        }

        private void LoadData()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfJual = _bll.GetAll(_pageNumber, _pageSize, ref _pagesCount);
                GridListControlHelper.Refresh<JualProduk>(this.gridList, _listOfJual);

                base.SetInfoHalaman(_pageNumber, _pagesCount);
                base.SetStateBtnNavigation(_pageNumber, _pagesCount);

                if (!(_listOfJual.Count > 0))
                {
                    base.SetInfoHalaman(0, 0);
                    base.SetStateBtnNavigation(0, 0); // non aktifkan button navigasi
                }
            }

            ResetButton();
        }

        private void LoadData(string customerName)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfJual = _bll.GetByName(customerName, _pageNumber, _pageSize, ref _pagesCount);
                GridListControlHelper.Refresh<JualProduk>(this.gridList, _listOfJual);

                base.SetInfoHalaman(_pageNumber, _pagesCount);
                base.SetStateBtnNavigation(_pageNumber, _pagesCount);

                if (!(_listOfJual.Count > 0))
                {
                    base.SetInfoHalaman(0, 0);
                    base.SetStateBtnNavigation(0, 0); // non aktifkan button navigasi
                }
            }

            ResetButton();
        }

        private void LoadData(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfJual = _bll.GetByTanggal(tanggalMulai, tanggalSelesai, _pageNumber, _pageSize, ref _pagesCount);
                GridListControlHelper.Refresh<JualProduk>(this.gridList, _listOfJual);

                base.SetInfoHalaman(_pageNumber, _pagesCount);
                base.SetStateBtnNavigation(_pageNumber, _pagesCount);

                if (!(_listOfJual.Count > 0))
                {
                    base.SetInfoHalaman(0, 0);
                    base.SetStateBtnNavigation(0, 0); // non aktifkan button navigasi
                }
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfJual.Count > 0);

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfJual.Count);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryPenjualanProduk("Tambah Data " + this.Text, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var jual = _listOfJual[index];
            jual.tanggal_tempo_old = jual.tanggal_tempo;

            LogicalThreadContext.Properties["OldValue"] = jual.ToJson();

            var frm = new FrmEntryPenjualanProduk("Edit Data " + this.Text, jual, _bll);
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
                var jual = _listOfJual[index];

                var result = _bll.Delete(jual);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<JualProduk>(this.gridList, _listOfJual, jual);
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
            var jual = (JualProduk)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<JualProduk>(this.gridList, _listOfJual, jual);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<JualProduk>(this.gridList, _listOfJual, jual);
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (btnPerbaiki.Enabled)
                Perbaiki();
        }

        private void filterRangeTanggal_BtnTampilkanClicked(object sender, EventArgs e)
        {
            var tanggalMulai = filterRangeTanggal.TanggalMulai;
            var tanggalSelesai = filterRangeTanggal.TanggalSelesai;

            if (!DateTimeHelper.IsValidRangeTanggal(tanggalMulai, tanggalSelesai))
            {
                MsgHelper.MsgNotValidRangeTanggal();
                return;
            }

            _pageNumber = 1;
            txtNamaCustomer.Clear();
            LoadData(tanggalMulai, tanggalSelesai);
        }

        private void filterRangeTanggal_ChkTampilkanSemuaDataClicked(object sender, EventArgs e)
        {
            _pageNumber = 1;
            txtNamaCustomer.Clear();

            var chk = (CheckBox)sender;

            if (chk.Checked)
            {
                LoadData();
                txtNamaCustomer.Enabled = false;
                btnCari.Enabled = false;
            }                
            else
            {
                LoadData(filterRangeTanggal.TanggalMulai, filterRangeTanggal.TanggalSelesai);
                txtNamaCustomer.Enabled = true;
                btnCari.Enabled = true;
            }                
        }

        private void txtNamaCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                btnCari_Click(sender, e);
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            if (txtNamaCustomer.Text.Length > 0)
            {
                _pageNumber = 1;
                LoadData(txtNamaCustomer.Text);
            }                
        }

        protected override void MoveFirst()
        {
            _pageNumber = 1;

            RefreshData();
        }

        protected override void MovePrevious()
        {
            _pageNumber--;

            RefreshData();
        }

        protected override void MoveNext()
        {
            _pageNumber++;

            RefreshData();
        }

        protected override void MoveLast()
        {
            _pageNumber = _pagesCount;

            RefreshData();
        }

        protected override void LimitRowChanged()
        {
            MainProgram.pageSize = (int)this.updLimit.Value;
            _pageSize = MainProgram.pageSize;
            _pageNumber = 1;

            RefreshData();
        }

        private void RefreshData()
        {
            // set hak akses untuk SELECT
            var role = _pengguna.GetRoleByMenuAndGrant(_menuId, GrantState.SELECT);
            if (role != null)
            {
                if (role.is_grant)
                {
                    if (filterRangeTanggal.IsCheckedTampilkanSemuaData)
                    {
                        LoadData();
                    } 
                    else if (txtNamaCustomer.Text.Length > 0)
                    {
                        LoadData(txtNamaCustomer.Text);
                    }                        
                    else
                    {
                        LoadData(filterRangeTanggal.TanggalMulai, filterRangeTanggal.TanggalSelesai);
                    }
                }
            }

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfJual.Count);
        }
    }
}
