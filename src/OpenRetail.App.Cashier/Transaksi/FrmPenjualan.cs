using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using log4net;
using OpenRetail.Model;
using OpenRetail.Helper;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using Syncfusion.Windows.Forms.Grid;
using WeifenLuo.WinFormsUI.Docking;
using OpenRetail.App.Cashier.Lookup;
using OpenRetail.Helper.UserControl;
using OpenRetail.Helper.RAWPrinting;

namespace OpenRetail.App.Cashier.Transaksi
{
    public partial class FrmPenjualan : DockContent, IListener
    {
        private IJualProdukBll _bll = null;
        private JualProduk _jual = null;
        private Customer _customer = null;
        private IList<ItemJualProduk> _listOfItemJual = new List<ItemJualProduk>();

        private int _rowIndex = 0;
        private int _colIndex = 0;
        
        private ILog _log;
        private Pengguna _pengguna;
        private Profil _profil;
        private PengaturanUmum _pengaturanUmum;        
        private bool _isCetakStruk = true;
        private string _currentNota;

        public FrmPenjualan(string header, Pengguna pengguna, string menuId)
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            this.Text = header;
            this._log = MainProgram.log;
            this._bll = new JualProdukBll(_log);            
            this._pengguna = MainProgram.pengguna;
            this._profil = MainProgram.profil;
            this._pengaturanUmum = MainProgram.pengaturanUmum;

            _currentNota = this._bll.GetLastNota();

            _listOfItemJual.Add(new ItemJualProduk()); // add dummy objek

            InitGridControl(gridControl);

            SetStatusBar();
            ShowInfoTanggal(_currentNota);
            txtKasir.Text = this._pengguna.nama_pengguna;
        }

        private void InitGridControl(GridControl grid)
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kode Produk", Width = 190 });

            gridListProperties.Add(new GridListControlProperties
                {
                    Header = "Nama Produk",
                    Width = _pengaturanUmum.is_tampilkan_keterangan_tambahan_item_jual ? 520 : 720
                }
            );

            gridListProperties.Add(new GridListControlProperties
                {
                    Header = _pengaturanUmum.keterangan_tambahan_item_jual,
                    Width = _pengaturanUmum.is_tampilkan_keterangan_tambahan_item_jual ? 200 : 0
                }
            );

            gridListProperties.Add(new GridListControlProperties { Header = "Jumlah", Width = 75 });
            gridListProperties.Add(new GridListControlProperties { Header = "Diskon", Width = 75 });
            gridListProperties.Add(new GridListControlProperties { Header = "Harga", Width = 120 });
            gridListProperties.Add(new GridListControlProperties { Header = "Sub Total" });

            GridListControlHelper.InitializeGridListControl<ItemJualProduk>(grid, _listOfItemJual, gridListProperties);
            
            grid.QueryRowHeight += delegate(object sender, GridRowColSizeEventArgs e)
            {
                e.Size = 27;
                e.Handled = true;
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

                    e.Style.Font = new GridFontInfo(new Font("Arial", 15f));
                    
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

                        case 4: // keterangan
                            e.Style.CellValue = itemJual.keterangan;

                            break;

                        case 5: // jumlah
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.CellValue = itemJual.jumlah - itemJual.jumlah_retur;

                            break;

                        case 6: // diskon
                            e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                            e.Style.CellValue = itemJual.diskon;

                            break;

                        case 7: // harga
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

                        case 8: // subtotal
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

                                    if (!(diskon > 0))
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

                        default:
                            break;
                    }

                    e.Handled = true; // we handled it, let the grid know
                }
            };

            var colIndex = 2; // kolom nama produk
            grid.CurrentCell.MoveTo(1, colIndex, GridSetCurrentCellOptions.BeginEndUpdate);
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

        private void SetStatusBar()
        {
            var infoStatus = "F3 : Input Produk | F4 : Cari Pelanggan | F5 : Edit Jumlah | F6 : Edit Diskon | F7 : Edit Harga | F8 : Cek Nota Terakhir | F10 : Bayar" +
                             "\r\nCTRL + B : Pembatalan Transaksi | CTRL + D: Hapus Item Transaksi | CTRL + L : Laporan Penjualan | CTRL + N : Tanpa Nota/Struk | CTRL + P : Setting Printer | CTRL + X : Tutup Form Transaksi";

            lblStatusBar.Text = infoStatus;
        }

        private void ShowInfoTanggal(string nota)
        {
            var dt = DateTime.Now;

            var tanggal = string.Format("{0}, {1}", DayMonthHelper.GetHariIndonesia(dt), dt.Day + " " + DayMonthHelper.GetBulanIndonesia(dt.Month) + " " + dt.Year);
            var jam = string.Format("{0:HH:mm:ss}", dt);

            txtNotaDanTanggal.Text = string.Format("{0} / {1} {2}", nota, tanggal, jam);
        }

        private void tmrTanggalJam_Tick(object sender, EventArgs e)
        {
            ShowInfoTanggal(_currentNota);
        }

        private void tmrResetPesan_Tick(object sender, EventArgs e)
        {
            lblPesan.Text = "";
            ((Timer)sender).Enabled = false;
        }

        private void SetItemProduk(GridControl grid, int rowIndex, Produk produk,
            double jumlah = 1, double harga = 0, double diskon = 0, string keterangan = "")
        {
            var itemJual = new ItemJualProduk();
            itemJual.produk_id = produk.produk_id;
            itemJual.Produk = produk;
            itemJual.keterangan = keterangan;
            itemJual.jumlah = jumlah;
            itemJual.harga_beli = produk.harga_beli;
            itemJual.harga_jual = harga > 0 ? harga : produk.harga_jual;
            itemJual.diskon = diskon;

            _listOfItemJual[rowIndex - 1] = itemJual;
        }

        private void UpdateItemProduk(GridControl grid, int rowIndex)
        {
            var itemJual = _listOfItemJual[rowIndex];

            if (itemJual.entity_state == EntityState.Unchanged)
                itemJual.entity_state = EntityState.Modified;

            itemJual.jumlah += 1;

            _listOfItemJual[rowIndex] = itemJual;
        }

        private ItemJualProduk GetExistItemProduk(string produkId)
        {
            var obj = _listOfItemJual.Where(f => f.produk_id == produkId)
                                     .FirstOrDefault();
            return obj;
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

            return Math.Round(total, MidpointRounding.AwayFromZero);
        }

        private void RefreshTotal()
        {
            lblTotal.Text = NumberHelper.NumberToString(SumGrid(_listOfItemJual));
        }

        private void gridControl_CurrentCellKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F4)
            {
                Shortcut(sender, e);
                return;
            }

            if (KeyPressHelper.IsEnter(e))
            {
                if (lblKembalian.Text.Length > 0)
                    lblKembalian.Text = "";

                var grid = (GridControl)sender;

                var rowIndex = grid.CurrentCell.RowIndex;
                var colIndex = grid.CurrentCell.ColIndex;

                IProdukBll bll = new ProdukBll(_log);
                Produk produk = null;
                GridCurrentCell cc;

                switch (colIndex)
                {
                    case 2: // kode produk
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
                                ShowMessage("Data produk tidak ditemukan", true);
                                GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                                return;
                            }

                            if (!_pengaturanUmum.is_stok_produk_boleh_minus)
                            {
                                if (produk.is_stok_minus)
                                {
                                    ShowMessage("Maaf stok produk tidak boleh minus", true);
                                    GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                                    return;
                                }
                            }

                            ShowMessage("");

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

                            // cek item produk sudah diinputkan atau belum ?
                            var itemProduk = GetExistItemProduk(produk.produk_id);

                            if (itemProduk != null) // sudah ada, tinggal update jumlah
                            {
                                var index = _listOfItemJual.IndexOf(itemProduk);

                                UpdateItemProduk(grid, index);
                                cc.Renderer.ControlText = string.Empty;
                            }
                            else
                            {
                                SetItemProduk(grid, rowIndex, produk, diskon: diskon);

                                if (grid.RowCount == rowIndex)
                                {
                                    _listOfItemJual.Add(new ItemJualProduk());
                                    grid.RowCount = _listOfItemJual.Count;
                                }
                            }
                            
                            grid.Refresh();
                            RefreshTotal();

                            if (_pengaturanUmum.is_tampilkan_keterangan_tambahan_item_jual)
                            {
                                // fokus ke kolom keterangan
                                GridListControlHelper.SetCurrentCell(grid, rowIndex, 4);
                            }
                            else
                            {
                                if (_pengaturanUmum.is_fokus_input_kolom_jumlah)
                                {
                                    GridListControlHelper.SetCurrentCell(grid, rowIndex, 5); // fokus ke kolom jumlah
                                }
                                else
                                {
                                    GridListControlHelper.SetCurrentCell(grid, _listOfItemJual.Count, 2); // pindah kebaris berikutnya
                                }
                            }                                                        
                        }

                        break;

                    case 3: // pencarian berdasarkan nama produk

                        cc = grid.CurrentCell;
                        var namaProduk = cc.Renderer.ControlValue.ToString();

                        var listOfProduk = bll.GetByName(namaProduk);

                        if (listOfProduk.Count == 0)
                        {
                            ShowMessage("Data produk tidak ditemukan", true);
                            GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                        }
                        else if (listOfProduk.Count == 1)
                        {
                            ShowMessage("");
                            produk = listOfProduk[0];

                            if (!_pengaturanUmum.is_stok_produk_boleh_minus)
                            {
                                if (produk.is_stok_minus)
                                {
                                    ShowMessage("Maaf stok produk tidak boleh minus", true);
                                    GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);
                                    return;
                                }
                            }

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

                            // cek item produk sudah diinputkan atau belum ?
                            var itemProduk = GetExistItemProduk(produk.produk_id);

                            if (itemProduk != null) // sudah ada, tinggal update jumlah
                            {
                                var index = _listOfItemJual.IndexOf(itemProduk);

                                UpdateItemProduk(grid, index);
                                cc.Renderer.ControlText = string.Empty;
                            }
                            else
                            {
                                SetItemProduk(grid, rowIndex, produk, diskon: diskon);

                                if (grid.RowCount == rowIndex)
                                {
                                    _listOfItemJual.Add(new ItemJualProduk());
                                    grid.RowCount = _listOfItemJual.Count;
                                }
                            }
                            
                            grid.Refresh();
                            RefreshTotal();

                            if (_pengaturanUmum.is_tampilkan_keterangan_tambahan_item_jual)
                            {
                                // fokus ke kolom keterangan
                                GridListControlHelper.SetCurrentCell(grid, rowIndex, 4);
                            }
                            else
                            {
                                if (_pengaturanUmum.is_fokus_input_kolom_jumlah)
                                {
                                    GridListControlHelper.SetCurrentCell(grid, rowIndex, 5); // fokus ke kolom jumlah
                                }
                                else
                                {
                                    GridListControlHelper.SetCurrentCell(grid, _listOfItemJual.Count, 2); // pindah kebaris berikutnya
                                }
                            }                            
                        }
                        else // data lebih dari satu
                        {
                            ShowMessage("");
                            _rowIndex = rowIndex;
                            _colIndex = colIndex;

                            var frmLookup = new FrmLookupReferensi("Data Produk", listOfProduk);
                            frmLookup.Listener = this;
                            frmLookup.ShowDialog();
                        }

                        break;

                    case 4: // keterangan
                        if (grid.RowCount == rowIndex)
                        {
                            _listOfItemJual.Add(new ItemJualProduk());
                            grid.RowCount = _listOfItemJual.Count;
                        }

                        if (_pengaturanUmum.is_fokus_input_kolom_jumlah)
                        {
                            GridListControlHelper.SetCurrentCell(grid, rowIndex, 5); // fokus ke kolom jumlah
                        }
                        else
                        {
                            GridListControlHelper.SetCurrentCell(grid, _listOfItemJual.Count, 2); // pindah kebaris berikutnya
                        }

                        break;

                    case 5: // jumlah
                        if (!_pengaturanUmum.is_stok_produk_boleh_minus)
                        {
                            gridControl_CurrentCellValidated(sender, new EventArgs());

                            var itemJual = _listOfItemJual[rowIndex - 1];
                            produk = itemJual.Produk;

                            var isValidStok = (produk.sisa_stok - itemJual.jumlah) >= 0;

                            if (!isValidStok)
                            {
                                ShowMessage("Maaf stok produk tidak boleh minus", true);
                                GridListControlHelper.SelectCellText(grid, rowIndex, colIndex);

                                return;
                            }
                        }

                        if (grid.RowCount == rowIndex)
                        {
                            _listOfItemJual.Add(new ItemJualProduk());
                            grid.RowCount = _listOfItemJual.Count;
                        }

                        GridListControlHelper.SetCurrentCell(grid, _listOfItemJual.Count, 2); // pindah kebaris berikutnya
                        break;

                    case 6: // diskon
                        if (grid.RowCount == rowIndex)
                        {
                            _listOfItemJual.Add(new ItemJualProduk());
                            grid.RowCount = _listOfItemJual.Count;
                        }

                        GridListControlHelper.SetCurrentCell(grid, _listOfItemJual.Count, 2);
                        break;

                    case 7:
                        if (grid.RowCount == rowIndex)
                        {
                            _listOfItemJual.Add(new ItemJualProduk());
                            grid.RowCount = _listOfItemJual.Count;
                        }

                        GridListControlHelper.SetCurrentCell(grid, _listOfItemJual.Count, 2); // fokus ke kolom kode produk
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

            // validasi input angka untuk kolom jumlah, diskon dan harga
            switch (cc.ColIndex)
            {
                case 5: // jumlah
                case 6: // diskon
                case 7: // harga
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

            Produk produk = null;
            ItemJualProduk itemJual = null;

            var rowIndex = cc.RowIndex - 1;

            if (_listOfItemJual.Count > rowIndex)
            {
                itemJual = _listOfItemJual[rowIndex];
                produk = itemJual.Produk;
            }            

            if (produk != null)
            {
                switch (cc.ColIndex)
                {
                    case 4: // kolom keterangan
                        itemJual.keterangan = cc.Renderer.ControlValue.ToString();
                        break;

                    case 5: // kolom jumlah
                        itemJual.jumlah = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);

                        itemJual.diskon = GetDiskonJualFix(produk, itemJual.jumlah, itemJual.diskon);
                        itemJual.harga_jual = GetHargaJualFix(produk, itemJual.jumlah, itemJual.harga_jual);
                        break;

                    case 6: // kolom diskon
                        itemJual.diskon = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);
                        break;

                    case 7: // kolom harga
                        itemJual.harga_jual = NumberHelper.StringToDouble(cc.Renderer.ControlValue.ToString(), true);
                        break;

                    default:
                        break;
                }

                SetItemProduk(grid, cc.RowIndex, produk, itemJual.jumlah, itemJual.harga_jual, itemJual.diskon, itemJual.keterangan);
                grid.Refresh();

                RefreshTotal();
            }
        }

        private void gridControl_KeyDown(object sender, KeyEventArgs e)
        {
            Shortcut(sender, e);
        }

        private void FrmPenjualan_KeyDown(object sender, KeyEventArgs e)
        {
            Shortcut(sender, e);
        }

        private void ShowMessage(string msg, bool isAutoReset = false)
        {
            lblPesan.Text = msg;
            tmrResetPesan.Enabled = isAutoReset;
        }

        private void ResetTransaksi(bool isShowConfirm = true)
        {
            if (isShowConfirm)
            {
                var msg = "Apakah Anda ingin membatalkan transaksi saat ini ?";

                if (!MsgHelper.MsgKonfirmasi(msg))
                    return;
            }

            _listOfItemJual.Clear();
            _listOfItemJual.Add(new ItemJualProduk()); // add dummy objek
            _jual = null;

            gridControl.RowCount = _listOfItemJual.Count();
            gridControl.Refresh();

            RefreshTotal();

            _isCetakStruk = true;
            ShowMessage("");
            lblStatusBar.Text = lblStatusBar.Text.Replace("Reset Pelanggan", "Cari Pelanggan");

            txtCustomer.Clear();
            _customer = null;

            _currentNota = this._bll.GetLastNota();
            ShowInfoTanggal(_currentNota);

            gridControl.Focus();
            GridListControlHelper.SetCurrentCell(gridControl, _listOfItemJual.Count, 2); // fokus ke kolom kode produk
        }

        private void Shortcut(object sender, KeyEventArgs e)
        {
            double total = 0;

            try
            {
                
                if (e.Modifiers == Keys.Control && e.KeyCode == Keys.B) // pembatalan transaksi
                {                    
                    total = SumGrid(_listOfItemJual);

                    if (total > 0)
                    {
                        ResetTransaksi(); // reset transaksi dengan menampilkan pesan konfirmasi
                    }
                }
                else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.D) // hapus item transaksi
                {
                    total = SumGrid(_listOfItemJual);

                    if (total > 0)
                    {
                        HapusItemTransaksi(); // hapus item transaksi
                    }
                }
                else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.N) // tanpa nota/struk
                {
                    if (_isCetakStruk)
                    {
                        _isCetakStruk = false;
                        ShowMessage("Tanpa nota/struk transaksi", true);
                    }

                }
                else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.X) // tutup form aktif
                {
                    this.Close();
                }
                else
                {                    
                    if (KeyPressHelper.IsShortcutKey(Keys.F5, e) || KeyPressHelper.IsShortcutKey(Keys.F6, e) || 
                        KeyPressHelper.IsShortcutKey(Keys.F7, e))
                    {
                        var colIndex = 5;
                        var rowIndex = this.gridControl.CurrentCell.RowIndex;

                        switch (e.KeyCode)
                        {
                            case Keys.F5: // edit jumlah
                                colIndex = 5;
                                break;

                            case Keys.F6: // edit diskon
                                colIndex = 6;
                                break;

                            case Keys.F7: // edit harga
                                colIndex = 7;
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
                    else
                    {
                        switch (e.KeyCode)
                        {
                            case Keys.F3: // input produk
                                gridControl.Focus();
                                GridListControlHelper.SetCurrentCell(gridControl, _listOfItemJual.Count, 2); // fokus ke kolom kode produk

                                break;

                            case Keys.F4: // cari/reset pelanggan

                                if (_customer == null) // cari pelanggan
                                {
                                    txtCustomer.Enabled = true;
                                    txtCustomer.Focus();
                                }
                                else // reset pelanggan
                                {
                                    _customer = null;
                                    txtCustomer.Clear();

                                    lblStatusBar.Text = lblStatusBar.Text.Replace("Reset Pelanggan", "Cari Pelanggan");
                                }                                

                                break;

                            case Keys.F8: // cek nota terakhir
                                var jual = _bll.GetListItemNotaTerakhir(_pengguna.pengguna_id, MainProgram.mesinId);

                                if (jual == null)
                                {
                                    ShowMessage("Belum ada info nota terakhir", true);
                                    return;
                                }                                    

                                var frmInfoNota = new FrmInfoNotaTerakhir("Info Nota Terakhir", jual);
                                frmInfoNota.ShowDialog();
                                
                                break;

                            case Keys.F10: // bayar

                                e.SuppressKeyPress = true;

                                if (this._jual == null)
                                    _jual = new JualProduk();

                                _jual.total_nota = SumGrid(_listOfItemJual);

                                if (!(_jual.total_nota > 0))
                                {
                                    ShowMessage("Anda belum melengkapi inputan data produk !", true);
                                    return;
                                }                                

                                _jual.pengguna_id = this._pengguna.pengguna_id;
                                _jual.Pengguna = this._pengguna;

                                if (this._customer != null)
                                {
                                    _jual.customer_id = this._customer.customer_id;
                                    _jual.Customer = this._customer;
                                }

                                _jual.nota = _currentNota;
                                _jual.tanggal = DateTime.Today;
                                _jual.tanggal_tempo = DateTimeHelper.GetNullDateTime();
                                _jual.is_tunai = true;
                                
                                _jual.item_jual = this._listOfItemJual.Where(f => f.Produk != null).ToList();
                                foreach (var item in _jual.item_jual)
                                {
                                    if (!(item.harga_beli > 0))
                                        item.harga_beli = item.Produk.harga_beli;

                                    if (!(item.harga_jual > 0))
                                        item.harga_jual = GetHargaJualFix(item.Produk, item.jumlah - item.jumlah_retur, item.Produk.harga_jual);
                                }

                                var frmBayar = new FrmBayar("Pembayaran", _jual, _bll);
                                frmBayar.Listener = this;
                                frmBayar.ShowDialog();

                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }
        }

        private void HapusItemTransaksi()
        {
            var jual = new JualProduk();
            jual.item_jual = this._listOfItemJual.Where(f => f.Produk != null).ToList();

            var frmHapusTransaksi = new FrmHapusItemTransaksi("Hapus Item Transaksi", jual);
            frmHapusTransaksi.Listener = this;
            frmHapusTransaksi.ShowDialog();
        }

        public void Ok(object sender, object data)
        {
            // filter berdasarkan data
            if (data is Produk) // pencarian produk baku
            {
                var produk = (Produk)data;

                if (!_pengaturanUmum.is_stok_produk_boleh_minus)
                {
                    if (produk.is_stok_minus)
                    {
                        ShowMessage("Maaf stok produk tidak boleh minus", true);
                        GridListControlHelper.SelectCellText(this.gridControl, _rowIndex, 3);
                        return;
                    }
                }

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

                // cek item produk sudah diinputkan atau belum ?
                var itemProduk = GetExistItemProduk(produk.produk_id);

                if (itemProduk != null) // sudah ada, tinggal update jumlah
                {
                    var index = _listOfItemJual.IndexOf(itemProduk);

                    UpdateItemProduk(this.gridControl, index);
                    this.gridControl.GetCellRenderer(_rowIndex, _colIndex).ControlText = string.Empty;
                }
                else
                {
                    SetItemProduk(this.gridControl, _rowIndex, produk, diskon: diskon);

                    if (this.gridControl.RowCount == _rowIndex)
                    {
                        _listOfItemJual.Add(new ItemJualProduk());
                        this.gridControl.RowCount = _listOfItemJual.Count;
                    }
                }
                
                this.gridControl.Refresh();
                RefreshTotal();

                if (_pengaturanUmum.is_tampilkan_keterangan_tambahan_item_jual)
                {
                    // fokus ke kolom keterangan
                    GridListControlHelper.SetCurrentCell(this.gridControl, _rowIndex, 4);
                }
                else
                {
                    if (_pengaturanUmum.is_fokus_input_kolom_jumlah)
                    {
                        GridListControlHelper.SetCurrentCell(this.gridControl, _rowIndex, 5); // fokus ke kolom jumlah
                    }
                    else
                    {
                        if (itemProduk != null)
                            GridListControlHelper.SetCurrentCell(this.gridControl, _rowIndex, 2); // fokus ke kolom kode
                        else
                            GridListControlHelper.SetCurrentCell(this.gridControl, _rowIndex + 1, 2); // fokus kebaris berikutnya
                    }  
                }                              
            }
            else if (data is Customer) // pencarian customer
            {
                this._customer = (Customer)data;
                txtCustomer.Text = this._customer.nama_customer;

                ShowMessage("");
                lblStatusBar.Text = lblStatusBar.Text.Replace("Cari Pelanggan", "Reset Pelanggan");

                KeyPressHelper.NextFocus();
            }
            else if (data is JualProduk) // pembayaran
            {
                var jual = (JualProduk)data;

                if (_pengaturanUmum.is_auto_print)
                {
                    if (_isCetakStruk)
                    {
                        switch (_pengaturanUmum.jenis_printer)
                        {
                            case JenisPrinter.DotMatrix:
                                CetakNotaDotMatrix(_jual);
                                break;

                            case JenisPrinter.MiniPOS:
                                CetakNotaMiniPOS(_jual);
                                break;

                            default:
                                // do nothing
                                break;
                        }
                    }                    
                }

                var kembalian = Math.Abs(jual.jumlah_bayar - jual.grand_total);
                lblKembalian.Text = string.Format("Kembalian: {0}", NumberHelper.NumberToString(kembalian));

                ResetTransaksi(false);                
            }
            else // filter bardasarkan nama form
            {
                var frmName = sender.GetType().Name;

                switch (frmName)
                {
                    case "FrmHapusItemTransaksi":
                        var noTransaksi = (int)((dynamic)data).noTransaksi;

                        var itemJual = _listOfItemJual[noTransaksi - 1];
                        itemJual.entity_state = EntityState.Deleted;

                        _listOfItemJual.Remove(itemJual);

                        gridControl.RowCount = _listOfItemJual.Count();
                        gridControl.Refresh();

                        RefreshTotal();

                        GridListControlHelper.SetCurrentCell(gridControl, _listOfItemJual.Count, 2);
                        break;

                    default:
                        break;
                }
            }
        }
        
        public void Ok(object sender, bool isNewData, object data)
        {
            throw new NotImplementedException();
        }

        private void txtCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                var customerName = ((AdvancedTextbox)sender).Text;

                if (customerName.Length == 0)
                {
                    ShowMessage("Nama pelanggan tidak boleh kosong", true);
                    return;
                }

                ICustomerBll bll = new CustomerBll(_log);
                var listOfCustomer = bll.GetByName(customerName);

                if (listOfCustomer.Count == 0)
                {
                    ShowMessage("Data pelanggan tidak ditemukan", true);

                    txtCustomer.Focus();
                    txtCustomer.SelectAll();

                }
                else if (listOfCustomer.Count == 1)
                {
                    _customer = listOfCustomer[0];
                    txtCustomer.Text = _customer.nama_customer;

                    ShowMessage("");
                    lblStatusBar.Text = lblStatusBar.Text.Replace("Cari Pelanggan", "Reset Pelanggan");

                    KeyPressHelper.NextFocus();
                }
                else // data lebih dari satu
                {
                    var frmLookup = new FrmLookupReferensi("Data Pelanggan", listOfCustomer);
                    frmLookup.Listener = this;
                    frmLookup.ShowDialog();
                }
            }
        }

        private void txtCustomer_Leave(object sender, EventArgs e)
        {
            var obj = (AdvancedTextbox)sender;

            obj.Enabled = false;
            obj.BackColor = Color.FromArgb(232, 235, 242);

            if (_customer == null)
                obj.Clear();
        }

        private void FrmPenjualan_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Utils.IsRunningUnderIDE())
            {
                e.Cancel = !CloseCancel();
            }            
        }

        private bool CloseCancel()
        {
            var total = SumGrid(_listOfItemJual);

            if (total > 0)
            {
                ShowMessage("Sudah ada transaksi, form tidak bisa ditutup", true);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CetakNotaDotMatrix(JualProduk jual)
        {
            IRAWPrinting printerMiniPos = new PrinterDotMatrix(_pengaturanUmum.nama_printer);
            printerMiniPos.Cetak(jual, _pengaturanUmum.list_of_header_nota);
        }

        private void CetakNotaMiniPOS(JualProduk jual)
        {
            IRAWPrinting printerMiniPos = new PrinterMiniPOS(_pengaturanUmum.nama_printer);
            printerMiniPos.Cetak(jual, _pengaturanUmum.list_of_header_nota_mini_pos, _pengaturanUmum.list_of_footer_nota_mini_pos, 
                _pengaturanUmum.jumlah_karakter, _pengaturanUmum.jumlah_gulung, _customer != null, ukuranFont: _pengaturanUmum.ukuran_font);
        }
    }
}
