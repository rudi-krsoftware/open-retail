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

using OpenRetail.App.Helper;
using OpenRetail.App.UI.Template;
using OpenRetail.App.Lookup;
using OpenRetail.Model;
using OpenRetail.Model.Nota;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using Syncfusion.Windows.Forms.Grid;
using OpenRetail.App.UserControl;
using OpenRetail.App.Referensi;
using ConceptCave.WaitCursor;
using log4net;
using Microsoft.Reporting.WinForms;
using OpenRetail.Model.RajaOngkir;
using OpenRetail.App.Transaksi.PrinterMiniPOS;

namespace OpenRetail.App.Transaksi
{
    public partial class FrmEntryPenjualanProduk : FrmEntryStandard, IListener
    {
        private IJualProdukBll _bll = null;
        private JualProduk _jual = null;
        private Customer _customer = null;
        private IList<ItemJualProduk> _listOfItemJual = new List<ItemJualProduk>();
        private IList<ItemJualProduk> _listOfItemJualOld = new List<ItemJualProduk>();
        private IList<ItemJualProduk> _listOfItemJualDeleted = new List<ItemJualProduk>();
        
        private int _rowIndex = 0;
        private int _colIndex = 0;
        private bool _isValidKodeProduk = false;

        private bool _isNewData = false;
        private ILog _log;
        private Pengguna _pengguna;
        private Profil _profil;
        private PengaturanUmum _pengaturanUmum;

        public IListener Listener { private get; set; }

        public FrmEntryPenjualanProduk(string header, IJualProdukBll bll) 
            : base()
        {            
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._bll = bll;
            this._isNewData = true;
            this._log = MainProgram.log;
            this._pengguna = MainProgram.pengguna;
            this._profil = MainProgram.profil;
            this._pengaturanUmum = MainProgram.pengaturanUmum;

            txtNota.Text = bll.GetLastNota();
            dtpTanggal.Value = DateTime.Today;
            dtpTanggalTempo.Value = dtpTanggal.Value;

            SetPengaturanPrinter();

            _listOfItemJual.Add(new ItemJualProduk()); // add dummy objek

            InitGridControl(gridControl);
        }        

        public FrmEntryPenjualanProduk(string header, JualProduk jual, IJualProdukBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._jual = jual;
            this._customer = jual.Customer;
            this._log = MainProgram.log;
            this._pengguna = MainProgram.pengguna;
            this._profil = MainProgram.profil;
            this._pengaturanUmum = MainProgram.pengaturanUmum;

            txtNota.Text = this._jual.nota;
            dtpTanggal.Value = (DateTime)this._jual.tanggal;
            dtpTanggalTempo.Value = dtpTanggal.Value;

            SetPengaturanPrinter();            
            chkDropship.Checked = this._jual.is_dropship;

            if (!this._jual.tanggal_tempo.IsNull())
            {
                rdoKredit.Checked = true;
                dtpTanggalTempo.Value = (DateTime)this._jual.tanggal_tempo;
            }

            if (this._customer != null)
                txtCustomer.Text = this._customer.nama_customer;

            txtKeterangan.Text = this._jual.keterangan;
            
            if (!string.IsNullOrEmpty(this._jual.kurir))
                cmbKurir.Text = this._jual.kurir;

            txtOngkosKirim.Text = this._jual.ongkos_kirim.ToString();
            txtDiskon.Text = this._jual.diskon.ToString();
            txtPPN.Text = this._jual.ppn.ToString();

            // simpan data lama
            _listOfItemJualOld.Clear();
            foreach (var item in this._jual.item_jual)
            {
                _listOfItemJualOld.Add(new ItemJualProduk
                {
                    item_jual_id = item.item_jual_id,
                    jumlah = item.jumlah,
                    harga_jual = item.harga_jual
                });
            }
            
            _listOfItemJual = this._jual.item_jual;
            _listOfItemJual.Add(new ItemJualProduk()); // add dummy objek

            InitGridControl(gridControl);

            RefreshTotal();
        }

        private void SetPengaturanPrinter()
        {
            if (this._pengaturanUmum.is_printer_mini_pos)
            {
                btnPreviewNota.Visible = false;
                chkDropship.Visible = false;
            }
            else
            {
                chkCetakNotaJual.Checked = this._pengaturanUmum.is_auto_print;
            }
        }

        private void InitGridControl(GridControl grid)
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kode Produk", Width = 120 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama Produk", Width = 270 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jumlah", Width = 50 });
            gridListProperties.Add(new GridListControlProperties { Header = "Diskon", Width = 50 });
            gridListProperties.Add(new GridListControlProperties { Header = "Harga", Width = 90 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sub Total", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Aksi" });

            GridListControlHelper.InitializeGridListControl<ItemJualProduk>(grid, _listOfItemJual, gridListProperties);

            grid.PushButtonClick += delegate(object sender, GridCellPushButtonClickEventArgs e)
            {
                if (e.ColIndex == 8)
                {
                    if (grid.RowCount == 1)
                    {
                        MsgHelper.MsgWarning("Minimal 1 item produk harus diinputkan !");
                        return;
                    }

                    if (MsgHelper.MsgDelete())
                    {
                        var itemJual = _listOfItemJual[e.RowIndex - 1];
                        itemJual.entity_state = EntityState.Deleted;

                        _listOfItemJualDeleted.Add(itemJual);
                        _listOfItemJual.Remove(itemJual);

                        grid.RowCount = _listOfItemJual.Count();
                        grid.Refresh();

                        RefreshTotal();
                    }                    
                }
            };

            grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                // Make sure the cell falls inside the grid
                if (e.RowIndex > 0)
                {
                    if (!(_listOfItemJual.Count > 0))
                        return;

                    var itemJual = _listOfItemJual[e.RowIndex - 1];
                    var produk = itemJual.Produk;

                    if (e.RowIndex % 2 == 0)
                        e.Style.BackColor = ColorCollection.BACK_COLOR_ALTERNATE;

                    double hargaBeli = 0;
                    double hargaJual = 0;
                    double jumlah = 0;

                    var isRetur = itemJual.jumlah_retur > 0;
                    if (isRetur)
                    {
                        e.Style.BackColor = Color.Red;
                        e.Style.Enabled = false;
                    }

                    switch (e.ColIndex)
                    {
                        case 1: // no urut
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.Enabled = false;
                            e.Style.CellValue = e.RowIndex.ToString();
                            break;

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

                            hargaBeli = itemJual.harga_beli;
                            hargaJual = itemJual.harga_jual;

                            if (produk != null)
                            {
                                if (!(hargaBeli > 0))
                                    hargaBeli = produk.harga_beli;

                                if (!(hargaJual > 0))
                                {
                                    jumlah = itemJual.jumlah - itemJual.jumlah_retur;
                                    hargaJual = GetHargaJualFix(produk, jumlah, produk.harga_jual);
                                }                                    
                            }

                            e.Style.CellValue = NumberHelper.NumberToString(hargaJual);

                            break;

                        case 7: // subtotal
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                            e.Style.Enabled = false;

                            jumlah = itemJual.jumlah - itemJual.jumlah_retur;

                            hargaBeli = itemJual.harga_beli;
                            hargaJual = itemJual.harga_setelah_diskon;                            

                            if (produk != null)
                            {
                                if (!(hargaBeli > 0))
                                    hargaBeli = produk.harga_beli;

                                if (!(hargaJual > 0))
                                {
                                    double diskon = itemJual.diskon;
                                    double diskonRupiah = 0;

                                    if(!(diskon > 0))
                                    {
                                        if (_customer != null)
                                        {
                                            diskon = _customer.diskon;
                                        }

                                        if (!(diskon > 0))
                                        {
                                            var diskonProduk = GetDiskonJualFix(produk, jumlah, produk.diskon);
                                            diskon = diskonProduk > 0 ? diskonProduk : produk.Golongan.diskon;
                                        }                                            
                                    }

                                    hargaJual = GetHargaJualFix(produk, jumlah, produk.harga_jual);

                                    diskonRupiah = diskon / 100 * hargaJual;
                                    hargaJual -= diskonRupiah;
                                }                                    
                            }
                            
                            e.Style.CellValue = NumberHelper.NumberToString(jumlah * hargaJual);
                            break;

                        case 8: // button hapus
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.CellType = GridCellTypeName.PushButton;
                            e.Style.Description = "Hapus";
                            break;

                        default:
                            break;
                    }

                    e.Handled = true; // we handled it, let the grid know
                }
            };

            var colIndex = 2; // kolom nama produk
            grid.CurrentCell.MoveTo(1, colIndex, GridSetCurrentCellOptions.BeginEndUpdate);
        }

        private double SumGrid(IList<ItemJualProduk> listOfItemJual)
        {
            double total = 0;
            foreach (var item in _listOfItemJual.Where(f => f.Produk != null))
            {
                double harga = 0;
                var jumlah = item.jumlah - item.jumlah_retur;

                if (item.harga_jual > 0)
                {
                    harga = item.harga_setelah_diskon;
                }                    
                else
                {
                    if (item.Produk != null)
                    {
                        var hargaJual = GetHargaJualFix(item.Produk, jumlah, item.Produk.harga_jual);
                        var diskon = GetDiskonJualFix(item.Produk, jumlah, item.diskon);

                        double diskonRupiah = diskon / 100 * hargaJual;

                        harga = hargaJual - diskonRupiah;
                    }                        
                }

                total += harga * jumlah;
            }

            if (total > 0)
            {
                total -= NumberHelper.StringToDouble(txtDiskon.Text);
                total += NumberHelper.StringToDouble(txtOngkosKirim.Text);
                total += NumberHelper.StringToDouble(txtPPN.Text);
            }

            return total;
        }

        private void RefreshTotal()
        {
            lblTotal.Text = NumberHelper.NumberToString(SumGrid(_listOfItemJual));
        }

        private HargaGrosir GetHargaGrosir(Produk produk, double jumlah)
        {
            HargaGrosir hargaGrosir = null;

            if (produk.list_of_harga_grosir.Count > 0)
            {
                hargaGrosir = produk.list_of_harga_grosir
                                    .Where(f => f.produk_id == produk.produk_id && f.jumlah_minimal <= jumlah)
                                    .LastOrDefault();
            }

            return hargaGrosir;
        }

        private double GetHargaJualFix(Produk produk, double jumlah, double hargaJualRetail)
        {
            var result = hargaJualRetail;

            if (jumlah > 1)
            {
                var grosir = GetHargaGrosir(produk, jumlah);
                if (grosir != null)
                {
                    if (grosir.harga_grosir > 0)
                        result = grosir.harga_grosir;
                }
            }            

            return result;
        }

        private double GetDiskonJualFix(Produk produk, double jumlah, double diskonJualRetail)
        {
            var result = diskonJualRetail;

            if (jumlah > 1)
            {
                var grosir = GetHargaGrosir(produk, jumlah);
                if (grosir != null)
                {
                    if (grosir.diskon > 0)
                        result = grosir.diskon;
                }
            }            

            return result;
        }

        protected override void Simpan()
        {
            if (_pengaturanUmum.is_customer_required)
            {
                if (this._customer == null || txtCustomer.Text.Length == 0)
                {
                    MsgHelper.MsgWarning("'Customer' tidak boleh kosong !");
                    txtCustomer.Focus();

                    return;
                }
            }            

            var total = SumGrid(this._listOfItemJual);
            if (!(total > 0))
            {
                MsgHelper.MsgWarning("Anda belum melengkapi inputan data produk !");
                return;
            }

            var jumlahBayar = NumberHelper.StringToNumber(txtJumlahBayar.Text);
            if (jumlahBayar > 0 && jumlahBayar < total)
            {
                MsgHelper.MsgWarning("Jumlah bayar kurang !");
                txtJumlahBayar.Focus();
                txtJumlahBayar.SelectAll();
                return;
            }

            if (rdoKredit.Checked)
            {
                if (!DateTimeHelper.IsValidRangeTanggal(dtpTanggal.Value, dtpTanggalTempo.Value))
                {
                    MsgHelper.MsgNotValidRangeTanggal();
                    return;
                }

                total = NumberHelper.StringToDouble(lblTotal.Text);

                if (this._customer != null)
                {
                    if (this._customer.plafon_piutang > 0)
                    {
                        if (!(this._customer.plafon_piutang >= (total + this._customer.sisa_piutang)))
                        {
                            var msg = string.Empty;                            

                            if (this._customer.sisa_piutang > 0)
                            {
                                msg = "Maaf, maksimal plafon piutang customer '{0}' adalah : {1}" +
                                      "\nSaat ini customer '{0}' masih mempunyai piutang sebesar : {2}";

                                msg = string.Format(msg, this._customer.nama_customer, NumberHelper.NumberToString(this._customer.plafon_piutang), NumberHelper.NumberToString(this._customer.sisa_piutang));
                            }   
                            else
                            {
                                msg = "Maaf, maksimal plafon piutang customer '{0}' adalah : {1}";

                                msg = string.Format(msg, this._customer.nama_customer, NumberHelper.NumberToString(this._customer.plafon_piutang));
                            }

                            MsgHelper.MsgWarning(msg);
                            return;
                        }
                    }
                }
            }            

            if (!MsgHelper.MsgKonfirmasi("Apakah proses ingin dilanjutkan ?"))
                return;

            if (_isNewData)
            {
                if (this._jual == null)
                    _jual = new JualProduk();
            }                

            _jual.pengguna_id = this._pengguna.pengguna_id;
            _jual.Pengguna = this._pengguna;

            if (this._customer != null)
            {
                _jual.customer_id = this._customer.customer_id;
                _jual.Customer = this._customer;
            }            

            _jual.nota = txtNota.Text;
            _jual.tanggal = dtpTanggal.Value;
            _jual.tanggal_tempo = DateTimeHelper.GetNullDateTime();
            _jual.is_tunai = rdoTunai.Checked;
            _jual.jumlah_bayar = jumlahBayar;

            if (rdoKredit.Checked) // penjualan kredit
            {
                _jual.tanggal_tempo = dtpTanggalTempo.Value;
            }

            _jual.is_dropship = chkDropship.Checked;
            _jual.kurir = cmbKurir.Text;
            _jual.ongkos_kirim = NumberHelper.StringToDouble(txtOngkosKirim.Text);
            _jual.ppn = NumberHelper.StringToDouble(txtPPN.Text);
            _jual.diskon = NumberHelper.StringToDouble(txtDiskon.Text);
            _jual.keterangan = txtKeterangan.Text;

            _jual.item_jual = this._listOfItemJual.Where(f => f.Produk != null).ToList();
            foreach (var item in _jual.item_jual)
            {
                if (!(item.harga_beli > 0))
                    item.harga_beli = item.Produk.harga_beli;

                if (!(item.harga_jual > 0))
                    item.harga_jual = GetHargaJualFix(item.Produk, item.jumlah - item.jumlah_retur, item.Produk.harga_jual);
            }

            if (!_isNewData) // update
                _jual.item_jual_deleted = _listOfItemJualDeleted;

            var result = 0;
            var validationError = new ValidationError();

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                if (_isNewData)
                {
                    result = _bll.Save(_jual, ref validationError);
                }
                else
                {
                    result = _bll.Update(_jual, ref validationError);
                }

                if (result > 0)
                {
                    try
                    {
                        if (chkCetakNotaJual.Checked)
                        {
                            if (this._pengaturanUmum.is_printer_mini_pos)
                                CetakNotaMiniPOS(_jual);
                            else
                                CetakNota(_jual.jual_id);
                        }
                    }
                    catch
                    {
                    }
                    
                    Listener.Ok(this, _isNewData, _jual);

                    _customer = null;
                    _listOfItemJual.Clear();
                    _listOfItemJualDeleted.Clear();                                        

                    this.Close();
                }
                else
                {
                    if (validationError.Message != null && validationError.Message.Length > 0)
                    {
                        MsgHelper.MsgWarning(validationError.Message);
                        base.SetFocusObject(validationError.PropertyName, this);
                    }
                    else
                        MsgHelper.MsgUpdateError();
                }
            }            
        }

        private void CetakNota(string jualProdukId)
        {
            ICetakNotaBll cetakBll = new CetakNotaBll(_log);
            var listOfItemNota = cetakBll.GetNotaPenjualan(jualProdukId);

            if (listOfItemNota.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "NotaPenjualan",
                    Value = listOfItemNota
                };

                // set header nota
                var parameters = new List<ReportParameter>();                
                var index = 1;

                foreach (var item in _pengaturanUmum.list_of_header_nota)
                {
                    var paramName = string.Format("header{0}", index);
                    parameters.Add(new ReportParameter(paramName, item.keterangan));

                    index++;
                }

                foreach (var item in listOfItemNota)
                {
                    if (item.label_dari1.Length == 0)
                        item.label_dari1 = this._pengaturanUmum.list_of_label_nota[0].keterangan;

                    if (item.label_dari2.Length == 0)
                        item.label_dari2 = this._pengaturanUmum.list_of_label_nota[1].keterangan;
                }

                // set footer nota
                var dt = DateTime.Now;
                var kotaAndTanggal = string.Format("{0}, {1}", _profil.kota, dt.Day + " " + DayMonthHelper.GetBulanIndonesia(dt.Month) + " " + dt.Year);

                parameters.Add(new ReportParameter("kota", kotaAndTanggal));
                parameters.Add(new ReportParameter("footer", _pengguna.nama_pengguna));

                var reportName = chkDropship.Checked ? "RvNotaPenjualanProdukTanpaLabelDropship" : "RvNotaPenjualanProdukTanpaLabel";

                var printReport = new ReportViewerPrintHelper(reportName, reportDataSource, parameters, _pengaturanUmum.nama_printer);
                printReport.Print();
            }
        }

        private void CetakNotaMiniPOS(JualProduk jual)
        {
            IPrinterMiniPOS printerMiniPos = new PrinterMiniPOS.PrinterMiniPOS(_pengaturanUmum.nama_printer);
            printerMiniPos.Cetak(jual, _pengaturanUmum.list_of_header_nota_mini_pos, _pengaturanUmum.list_of_footer_nota_mini_pos, _pengaturanUmum.jumlah_karakter, _pengaturanUmum.jumlah_gulung, _pengaturanUmum.is_cetak_customer);
        }

        protected override void Selesai()
        {
            // restore data lama
            if (!_isNewData)
            {
                // restore item yang di edit
                var itemsModified = _jual.item_jual.Where(f => f.Produk != null && f.entity_state == EntityState.Modified)
                                                   .ToArray();

                foreach (var item in itemsModified)
                {
                    var itemJual = _listOfItemJualOld.Where(f => f.item_jual_id == item.item_jual_id)
                                                     .SingleOrDefault();

                    if (itemJual != null)
                    {
                        item.jumlah = itemJual.jumlah;
                        item.harga_jual = itemJual.harga_jual;
                    }
                }

                // restore item yang di delete
                var itemsDeleted = _listOfItemJualDeleted.Where(f => f.Produk != null && f.entity_state == EntityState.Deleted)
                                                         .ToArray();
                foreach (var item in itemsDeleted)
                {
                    item.entity_state = EntityState.Unchanged;
                    this._jual.item_jual.Add(item);
                }

                _listOfItemJualDeleted.Clear();
            }

            base.Selesai();
        }

        public void Ok(object sender, object data)
        {
            if (data is Produk) // pencarian produk baku
            {
                var produk = (Produk)data;

                double diskon = 0;
                if (_customer != null)
                {
                    diskon = _customer.diskon;
                }

                if (!(diskon > 0))
                {
                    var diskonProduk = GetDiskonJualFix(produk, 1, produk.diskon);
                    diskon = diskonProduk > 0 ? diskonProduk : produk.Golongan.diskon;
                }                    

                SetItemProduk(this.gridControl, _rowIndex, _colIndex + 1, produk, diskon: diskon);
                this.gridControl.Refresh();
                RefreshTotal();

                if (this.gridControl.RowCount == _rowIndex)
                {
                    _listOfItemJual.Add(new ItemJualProduk());
                    this.gridControl.RowCount = _listOfItemJual.Count;
                }

                GridListControlHelper.SetCurrentCell(this.gridControl, _rowIndex + 1, 2); // pindah kebaris berikutnya
            }
            else if (data is Customer) // pencarian customer
            {
                this._customer = (Customer)data;
                txtCustomer.Text = this._customer.nama_customer;
                KeyPressHelper.NextFocus();
            }
            else if (data is AlamatKirim)
            {
                var alamatKirim = (AlamatKirim)data;

                if (this._jual == null)
                    this._jual = new JualProduk();

                this._jual.is_sdac = alamatKirim.is_sdac;
                this._jual.kirim_kepada = alamatKirim.kepada;
                this._jual.kirim_alamat = alamatKirim.alamat;
                this._jual.kirim_kecamatan = alamatKirim.kecamatan;
                this._jual.kirim_kelurahan = alamatKirim.kelurahan;
                this._jual.kirim_desa = alamatKirim.desa;
                this._jual.kirim_kabupaten = alamatKirim.kabupaten;
            }
            else if (data is LabelAlamatKirim)
            {
                var labelAlamatKirim = (LabelAlamatKirim)data;

                if (this._jual == null)
                    this._jual = new JualProduk();

                this._jual.label_dari1 = labelAlamatKirim.dari1;
                this._jual.label_dari2 = labelAlamatKirim.dari2;

                this._jual.label_kepada1 = labelAlamatKirim.kepada1;
                this._jual.label_kepada2 = labelAlamatKirim.kepada2;
                this._jual.label_kepada3 = labelAlamatKirim.kepada3;
                this._jual.label_kepada4 = labelAlamatKirim.kepada4;
            }
            else if (data is costs)
            {
                var ongkir = (costs)data;

                try
                {
                    cmbKurir.Text = string.Format("{0} {1}", ongkir.kurir_code, ongkir.service);
                    txtOngkosKirim.Text = ongkir.cost[0].value.ToString();
                }
                catch
                {
                }
            }
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            // do nothing
        }

        private void rdoTunai_CheckedChanged(object sender, EventArgs e)
        {
            dtpTanggalTempo.Enabled = false;
        }

        private void rdoKredit_CheckedChanged(object sender, EventArgs e)
        {
            dtpTanggalTempo.Enabled = true;
        }

        private void txtKeterangan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                gridControl.Focus();
                GridListControlHelper.SetCurrentCell(gridControl, 1, 2); // fokus ke kolom nama produk
            }
        }

        private void txtCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                var customerName = ((AdvancedTextbox)sender).Text;

                ICustomerBll bll = new CustomerBll(_log);
                var listOfCustomer = bll.GetByName(customerName);

                if (listOfCustomer.Count == 0)
                {
                    MsgHelper.MsgWarning("Data customer tidak ditemukan");
                    txtCustomer.Focus();
                    txtCustomer.SelectAll();

                }
                else if (listOfCustomer.Count == 1)
                {
                    _customer = listOfCustomer[0];
                    txtCustomer.Text = _customer.nama_customer;
                    KeyPressHelper.NextFocus();
                }
                else // data lebih dari satu
                {
                    var frmLookup = new FrmLookupReferensi("Data Customer", listOfCustomer);
                    frmLookup.Listener = this;
                    frmLookup.ShowDialog();
                }
            }
        }

        private void SetItemProduk(GridControl grid, int rowIndex, int colIndex, Produk produk, double jumlah = 1, double harga = 0, double diskon = 0)
        {
            ItemJualProduk itemJual;

            if (_isNewData)
            {
                itemJual = new ItemJualProduk();
            }
            else
            {
                itemJual = _listOfItemJual[rowIndex - 1];

                if (itemJual.entity_state == EntityState.Unchanged)
                    itemJual.entity_state = EntityState.Modified;
            }

            itemJual.produk_id = produk.produk_id;
            itemJual.Produk = produk;
            itemJual.jumlah = jumlah;
            itemJual.harga_beli = produk.harga_beli;
            itemJual.harga_jual = harga > 0 ? harga : produk.harga_jual;
            itemJual.diskon = diskon;

            _listOfItemJual[rowIndex - 1] = itemJual;
        }

        private void gridControl_CurrentCellKeyDown(object sender, KeyEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                var grid = (GridControl)sender;

                var rowIndex = grid.CurrentCell.RowIndex;
                var colIndex = grid.CurrentCell.ColIndex;

                IProdukBll bll = new ProdukBll(_log);
                Produk produk = null;
                GridCurrentCell cc;

                switch (colIndex)
                {
                    case 2: // kode produk
                        _isValidKodeProduk = false;

                        cc = grid.CurrentCell;
                        var kodeProduk = cc.Renderer.ControlValue.ToString();

                        if (kodeProduk.Length == 0) // kode produk kosong
                        {
                            // fokus ke kolom nama produk
                            GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 1);
                        }
                        else
                        {
                            // pencarian berdasarkan kode produk
                            produk = bll.GetByKode(kodeProduk);

                            if (produk == null)
                            {
                                MsgHelper.MsgWarning("Data produk tidak ditemukan");
                                GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                                return;
                            }

                            _isValidKodeProduk = true;

                            double diskon = 0;

                            if (_customer != null)
                            {
                                diskon = _customer.diskon;
                            }

                            if (!(diskon > 0))
                            {
                                var diskonProduk = GetDiskonJualFix(produk, 1, produk.diskon);
                                diskon = diskonProduk > 0 ? diskonProduk : produk.Golongan.diskon;
                            }                                

                            SetItemProduk(grid, rowIndex, colIndex, produk, diskon: diskon);
                            grid.Refresh();
                            RefreshTotal();

                            if (grid.RowCount == rowIndex)
                            {
                                _listOfItemJual.Add(new ItemJualProduk());
                                grid.RowCount = _listOfItemJual.Count;
                            }

                            GridListControlHelper.SetCurrentCell(grid, rowIndex + 1, 2); // pindah kebaris berikutnya
                        }

                        break;

                    case 3: // pencarian berdasarkan nama produk

                        cc = grid.CurrentCell;
                        var namaProduk = cc.Renderer.ControlValue.ToString();

                        if (!_isValidKodeProduk)
                        {
                            var listOfProduk = bll.GetByName(namaProduk);

                            if (listOfProduk.Count == 0)
                            {
                                MsgHelper.MsgWarning("Data produk tidak ditemukan");
                                GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                            }
                            else if (listOfProduk.Count == 1)
                            {
                                produk = listOfProduk[0];

                                double diskon = 0;

                                if (_customer != null)
                                {
                                    diskon = _customer.diskon;
                                }

                                if (!(diskon > 0))
                                {
                                    var diskonProduk = GetDiskonJualFix(produk, 1, produk.diskon);
                                    diskon = diskonProduk > 0 ? diskonProduk : produk.Golongan.diskon;
                                }                                    

                                SetItemProduk(grid, rowIndex, colIndex, produk, diskon: diskon);
                                grid.Refresh();
                                RefreshTotal();

                                if (grid.RowCount == rowIndex)
                                {
                                    _listOfItemJual.Add(new ItemJualProduk());
                                    grid.RowCount = _listOfItemJual.Count;
                                }

                                GridListControlHelper.SetCurrentCell(grid, rowIndex + 1, 2); // pindah kebaris berikutnya
                            }
                            else // data lebih dari satu
                            {
                                _rowIndex = rowIndex;
                                _colIndex = colIndex;

                                var frmLookup = new FrmLookupReferensi("Data Produk", listOfProduk);
                                frmLookup.Listener = this;
                                frmLookup.ShowDialog();
                            }
                        }
                        else
                        {
                            GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 1);
                        }

                        break;

                    case 4: // jumlah
                    case 5: // diskon
                        GridListControlHelper.SetCurrentCell(grid, rowIndex, colIndex + 1);
                        break;

                    case 6:
                        if (grid.RowCount == rowIndex)
                        {
                            _listOfItemJual.Add(new ItemJualProduk());
                            grid.RowCount = _listOfItemJual.Count;
                        }

                        GridListControlHelper.SetCurrentCell(grid, rowIndex + 1, 2); // fokus ke kolom kode produk
                        break;

                    default:
                        break;
                }
            }
        }

        private void gridControl_CurrentCellKeyPress(object sender, KeyPressEventArgs e)
        {
            var grid = (GridControl)sender;
            GridCurrentCell cc = grid.CurrentCell;

            // validasi input angka untuk kolom jumlah dan harga
            switch (cc.ColIndex)
            {
                case 4: // jumlah
                case 5: // diskon
                case 6: // harga
                    e.Handled = KeyPressHelper.NumericOnly(e);
                    break;

                default:
                    break;
            }
        }

        private void gridControl_CurrentCellValidated(object sender, EventArgs e)
        {
            var grid = (GridControl)sender;

            GridCurrentCell cc = grid.CurrentCell;

            var itemJual = _listOfItemJual[cc.RowIndex - 1];
            var produk = itemJual.Produk;

            if (produk != null)
            {                
                switch (cc.ColIndex)
                {
                    case 4: // kolom jumlah
                        itemJual.jumlah = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);

                        itemJual.diskon = GetDiskonJualFix(produk, itemJual.jumlah, itemJual.diskon);
                        itemJual.harga_jual = GetHargaJualFix(produk, itemJual.jumlah, itemJual.harga_jual);
                        break;

                    case 5: // kolom diskon
                        itemJual.diskon = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);
                        break;

                    case 6: // kolom harga
                        itemJual.harga_jual = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);
                        break;

                    default:
                        break;
                }
                
                SetItemProduk(grid, cc.RowIndex, cc.ColIndex, produk, itemJual.jumlah, itemJual.harga_jual, itemJual.diskon);
                grid.Refresh();

                RefreshTotal();
            }           
        }

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            // kasus khusus untuk shortcut F2, tidak jalan jika dipanggil melalui event Form KeyDown
            if (KeyPressHelper.IsShortcutKey(Keys.F2, e)) // tambahan data customer
            {
                ShowEntryCustomer();
            }
        }

        private void ShowEntryProduk()
        {
            var isGrant = RolePrivilegeHelper.IsHaveHakAkses("mnuProduk", _pengguna);
            if (!isGrant)
            {
                MsgHelper.MsgWarning("Maaf Anda tidak mempunyai otoritas untuk mengakses menu ini");
                return;
            }

            IGolonganBll golonganBll = new GolonganBll(_log);
            var listOfGolongan = golonganBll.GetAll();

            Golongan golongan = null;
            if (listOfGolongan.Count > 0)
                golongan = listOfGolongan[0];

            IProdukBll produkBll = new ProdukBll(_log);
            var frmEntryProduk = new FrmEntryProduk("Tambah Data Produk", golongan, listOfGolongan, produkBll);
            frmEntryProduk.Listener = this;
            frmEntryProduk.ShowDialog();
        }

        private void ShowEntryCustomer()
        {
            var isGrant = RolePrivilegeHelper.IsHaveHakAkses("mnuCustomer", _pengguna);
            if (!isGrant)
            {
                MsgHelper.MsgWarning("Maaf Anda tidak mempunyai otoritas untuk mengakses menu ini");
                return;
            }

            ICustomerBll customerBll = new CustomerBll(_log);
            var frmEntryCustomer = new FrmEntryCustomer("Tambah Data Customer", customerBll);
            frmEntryCustomer.Listener = this;
            frmEntryCustomer.ShowDialog();
        }

        private void FrmEntryPenjualanProduk_KeyDown(object sender, KeyEventArgs e)
        {            
            if (KeyPressHelper.IsShortcutKey(Keys.F1, e)) // tambah data produk
            {
                ShowEntryProduk();
            }
            else if (KeyPressHelper.IsShortcutKey(Keys.F2, e)) // tambahan data customer
            {
                ShowEntryCustomer();
            }
            else if (KeyPressHelper.IsShortcutKey(Keys.F5, e) || KeyPressHelper.IsShortcutKey(Keys.F6, e) || KeyPressHelper.IsShortcutKey(Keys.F7, e))
            {                
                var colIndex = 4;
                var rowIndex = this.gridControl.CurrentCell.RowIndex;

                switch (e.KeyCode)
                {
                    case Keys.F5: // edit jumlah
                        colIndex = 4;
                        break;

                    case Keys.F6: // edit diskon
                        colIndex = 5;
                        break;

                    case Keys.F7: // edit harga
                        colIndex = 6;
                        break;

                    default:
                        break;
                }

                if (gridControl.RowCount > 1 && gridControl.RowCount == rowIndex)
                {
                    gridControl.Focus();
                    GridListControlHelper.SetCurrentCell(gridControl, _listOfItemJual.Count - 1, colIndex);
                }
            }
            else if (KeyPressHelper.IsShortcutKey(Keys.F8, e)) // bayar
            {
                txtJumlahBayar.Text = "0";
                txtKembali.Text = "0";
                txtJumlahBayar.Focus();
            }
        }

        private void txtOngkosKirim_TextChanged(object sender, EventArgs e)
        {
            RefreshTotal();
        }

        private void txtDiskon_TextChanged(object sender, EventArgs e)
        {
            RefreshTotal();
        }

        private void txtPPN_TextChanged(object sender, EventArgs e)
        {
            RefreshTotal();
        }

        private void FrmEntryPenjualanProduk_FormClosing(object sender, FormClosingEventArgs e)
        {
            // hapus objek dumm
            if (!_isNewData)
            {
                var itemsToRemove = _jual.item_jual.Where(f => f.Produk == null && f.entity_state == EntityState.Added)
                                                   .ToArray();

                foreach (var item in itemsToRemove)
                {
                    _jual.item_jual.Remove(item);
                }
            }
        }

        private void btnSetAlamatKirim_Click(object sender, EventArgs e)
        {
            if (this._customer == null || txtCustomer.Text.Length == 0)
            {
                MsgHelper.MsgWarning("'Customer' tidak boleh kosong !");
                txtCustomer.Focus();

                return;
            }

            var frmEntryAlamatKirim = new FrmEntryAlamatKirim("Alamat Kirim", this._customer, this._jual);
            frmEntryAlamatKirim.Listener = this;
            frmEntryAlamatKirim.ShowDialog();
        }

        private void btnPreviewNota_Click(object sender, EventArgs e)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                PreviewNota();
            }            
        }

        private void PreviewNota()
        {
            if (this._customer == null || txtCustomer.Text.Length == 0)
            {
                MsgHelper.MsgWarning("'Customer' tidak boleh kosong !");
                txtCustomer.Focus();

                return;
            }

            var total = SumGrid(this._listOfItemJual);
            if (!(total > 0))
            {
                MsgHelper.MsgWarning("Anda belum melengkapi inputan data produk !");
                return;
            }

            if (total > 0)
            {
                total += NumberHelper.StringToDouble(txtDiskon.Text);
                total -= NumberHelper.StringToDouble(txtOngkosKirim.Text);
                total -= NumberHelper.StringToDouble(txtPPN.Text);
            }

            if (this._jual == null)
            {
                this._jual = new JualProduk();
            }

            var listOfItemNota = new List<NotaPenjualan>();

            foreach (var item in this._listOfItemJual.Where(f => f.Produk != null))
            {
                var itemNota = new NotaPenjualan
                {
                    nama_customer = this._customer.nama_customer,
                    alamat = string.IsNullOrEmpty(this._customer.alamat) ? "" : this._customer.alamat,
                    kecamatan = string.IsNullOrEmpty(this._customer.kecamatan) ? "-" : this._customer.kecamatan,
                    kelurahan = string.IsNullOrEmpty(this._customer.kelurahan) ? "-" : this._customer.kelurahan,
                    kota = string.IsNullOrEmpty(this._customer.kota) ? "-" : this._customer.kota,
                    kode_pos = (string.IsNullOrEmpty(this._customer.kode_pos) || this._customer.kode_pos == "0") ? "-" : this._customer.kode_pos,
                    kontak = string.IsNullOrEmpty(this._customer.kontak) ? "" : this._customer.kontak,
                    telepon = string.IsNullOrEmpty(this._customer.telepon) ? "-" : this._customer.telepon,
                    nota = txtNota.Text,
                    tanggal = dtpTanggal.Value,                    
                    ppn = NumberHelper.StringToDouble(txtPPN.Text),
                    diskon_nota = NumberHelper.StringToDouble(txtDiskon.Text),
                    kurir = cmbKurir.Text,
                    ongkos_kirim = NumberHelper.StringToDouble(txtOngkosKirim.Text),
                    total_nota = total,
                    is_sdac = this._jual.is_sdac,
                    kirim_kepada = string.IsNullOrEmpty(this._jual.kirim_kepada) ? "" : this._jual.kirim_kepada,
                    kirim_alamat = string.IsNullOrEmpty(this._jual.kirim_alamat) ? "" : this._jual.kirim_alamat,
                    kirim_desa = string.IsNullOrEmpty(this._jual.kirim_desa) ? "-" : this._jual.kirim_desa,
                    kirim_kabupaten = string.IsNullOrEmpty(this._jual.kirim_kabupaten) ? "-" : this._jual.kirim_kabupaten,
                    kirim_kecamatan = string.IsNullOrEmpty(this._jual.kirim_kecamatan) ? "-" : this._jual.kirim_kecamatan,
                    kirim_kelurahan = string.IsNullOrEmpty(this._jual.kirim_kelurahan) ? "-" : this._jual.kirim_kelurahan,
                    kirim_kota = string.IsNullOrEmpty(this._jual.kirim_kota) ? "-" : this._jual.kirim_kota,
                    kirim_kode_pos = string.IsNullOrEmpty(this._jual.kirim_kode_pos) ? "-" : this._jual.kirim_kode_pos,
                    kirim_telepon = string.IsNullOrEmpty(this._jual.kirim_telepon) ? "-" : this._jual.kirim_telepon,
                    label_dari1 = string.IsNullOrEmpty(this._jual.label_dari1) ? "" : this._jual.label_dari1,
                    label_dari2 = string.IsNullOrEmpty(this._jual.label_dari2) ? "" : this._jual.label_dari1,
                    label_kepada1 = string.IsNullOrEmpty(this._jual.label_kepada1) ? "" : this._jual.label_kepada1,
                    label_kepada2 = string.IsNullOrEmpty(this._jual.label_kepada2) ? "" : this._jual.label_kepada2,
                    label_kepada3 = string.IsNullOrEmpty(this._jual.label_kepada3) ? "" : this._jual.label_kepada3,
                    label_kepada4 = string.IsNullOrEmpty(this._jual.label_kepada4) ? "" : this._jual.label_kepada4,
                    kode_produk = item.Produk.kode_produk,
                    nama_produk = item.Produk.nama_produk,
                    satuan = item.Produk.satuan,
                    harga = item.harga_jual,
                    jumlah = item.jumlah,
                    jumlah_retur = item.jumlah_retur,
                    diskon = item.diskon
                };

                itemNota.tanggal_tempo = DateTimeHelper.GetNullDateTime();
                if (rdoKredit.Checked)
                    itemNota.tanggal_tempo = dtpTanggalTempo.Value;

                if (string.IsNullOrEmpty(itemNota.label_kepada1))
                    itemNota.label_kepada1 = this._customer.nama_customer;

                if (string.IsNullOrEmpty(itemNota.label_kepada2))
                    itemNota.label_kepada2 = this._customer.alamat;

                if (string.IsNullOrEmpty(itemNota.label_kepada3))
                    itemNota.label_kepada3 = "HP: " + this._customer.telepon;

                listOfItemNota.Add(itemNota);
            }

            var reportDataSource = new ReportDataSource
            {
                Name = "NotaPenjualan",
                Value = listOfItemNota
            };

            // set header nota
            var parameters = new List<ReportParameter>();
            var index = 1;

            foreach (var item in _pengaturanUmum.list_of_header_nota)
            {
                var paramName = string.Format("header{0}", index);
                parameters.Add(new ReportParameter(paramName, item.keterangan));

                index++;
            }

            foreach (var item in listOfItemNota)
            {
                if (item.label_dari1.Length == 0)
                    item.label_dari1 = this._pengaturanUmum.list_of_label_nota[0].keterangan;

                if (item.label_dari2.Length == 0)
                    item.label_dari2 = this._pengaturanUmum.list_of_label_nota[1].keterangan;
            }

            // set footer nota
            var dt = DateTime.Now;
            var kotaAndTanggal = string.Format("{0}, {1}", _profil.kota, dt.Day + " " + DayMonthHelper.GetBulanIndonesia(dt.Month) + " " + dt.Year);

            parameters.Add(new ReportParameter("kota", kotaAndTanggal));
            parameters.Add(new ReportParameter("footer", _pengguna.nama_pengguna));

            var reportName = chkDropship.Checked ? "RvNotaPenjualanProdukTanpaLabelDropship" : "RvNotaPenjualanProdukTanpaLabel";

            var frmPreviewReport = new FrmPreviewReport("Preview Nota Penjualan", reportName, reportDataSource, parameters, true);
            frmPreviewReport.ShowDialog();
        }

        private void btnCekOngkir_Click(object sender, EventArgs e)
        {
            var frmCekOngkir = new FrmLookupCekOngkir("Cek Ongkos Kirim");
            frmCekOngkir.Listener = this;
            frmCekOngkir.ShowDialog();
        }

        private void cmbKurir_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                KeyPressHelper.NextFocus();
        }

        private void txtJumlahBayar_TextChanged(object sender, EventArgs e)
        {
            txtKembali.Text = "0";

            var total = NumberHelper.StringToNumber(lblTotal.Text);
            if (total > 0)
            {
                var jumlahBayar = NumberHelper.StringToNumber(((AdvancedTextbox)sender).Text);
                var kembali = jumlahBayar - total;
                
                if (kembali > 0)
                    txtKembali.Text = kembali.ToString();
            }
        }
    }
}
