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
using OpenRetail.App.UI.Template;
using OpenRetail.App.Helper;
using OpenRetail.Bll.Service;
using OpenRetail.App.UserControl;
using OpenRetail.App.Lookup;
using log4net;

namespace OpenRetail.App.Referensi
{
    public partial class FrmEntryPenyesuaianStok : FrmEntryStandard, IListener
    {
        private IPenyesuaianStokBll _bll = null; // deklarasi objek business logic layer 
        private PenyesuaianStok _penyesuaianStok = null;
        private Produk _produk = null;
        private IList<AlasanPenyesuaianStok> _listOfAlasanPenyesuaian;

        private bool _isNewData = false;
        private ILog _log;

        public IListener Listener { private get; set; }

        public FrmEntryPenyesuaianStok(string header, IPenyesuaianStokBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._bll = bll;
            this._isNewData = true;
            this._log = MainProgram.log;

            LoadAlasanPenyesuaianStok();
        }

        public FrmEntryPenyesuaianStok(string header, PenyesuaianStok penyesuaianStok, IPenyesuaianStokBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._penyesuaianStok = penyesuaianStok;
            this._log = MainProgram.log;

            this._produk = this._penyesuaianStok.Produk;
            txtKodeProduk.Text = this._produk.kode_produk;
            txtKodeProduk.Enabled = false;
            txtNamaProduk.Text = this._produk.nama_produk;
            txtStokEtalase.Text = this._produk.stok.ToString();
            txtStokGudang.Text = this._produk.stok_gudang.ToString();

            // info mutasi
            dtpTanggal.Value = (DateTime)this._penyesuaianStok.tanggal;

            txtPenambahanStokEtalase.Text = this._penyesuaianStok.penambahan_stok.ToString();
            txtPenambahanStokGudang.Text = this._penyesuaianStok.penambahan_stok_gudang.ToString();

            txtPenguranganStokEtalase.Text = this._penyesuaianStok.pengurangan_stok.ToString();
            txtPenguranganStokGudang.Text = this._penyesuaianStok.pengurangan_stok_gudang.ToString();

            txtKeterangan.Text = this._penyesuaianStok.keterangan;

            LoadAlasanPenyesuaianStok();
            if (this._penyesuaianStok.AlasanPenyesuaianStok != null)
                cmbAlasanPenyesuaian.SelectedItem = this._penyesuaianStok.AlasanPenyesuaianStok.alasan;
        }

        private void LoadAlasanPenyesuaianStok()
        {
            IAlasanPenyesuaianStokBll bll = new AlasanPenyesuaianStokBll(_log);
            _listOfAlasanPenyesuaian = bll.GetAll();

            cmbAlasanPenyesuaian.Items.Clear();
            foreach (var alasan in _listOfAlasanPenyesuaian)
            {
                cmbAlasanPenyesuaian.Items.Add(alasan.alasan);
            }

            if (_listOfAlasanPenyesuaian.Count > 0)
                cmbAlasanPenyesuaian.SelectedIndex = 0;
        }

        protected override void Simpan()
        {
            if (txtKodeProduk.Text.Length == 0)
            {
                MsgHelper.MsgWarning("'Kode Produk' tidak boleh kosong !");
                txtKodeProduk.Focus();
                return;
            }

            if (this._produk == null)
            {
                MsgHelper.MsgWarning("'Kode Produk' tidak ditemukan !");
                txtKodeProduk.Focus();
                return;
            }

            if (_isNewData)
                _penyesuaianStok = new PenyesuaianStok();

            _penyesuaianStok.produk_id = this._produk.produk_id;
            _penyesuaianStok.Produk = this._produk;

            var alasanPenyesuaian = _listOfAlasanPenyesuaian[cmbAlasanPenyesuaian.SelectedIndex];
            _penyesuaianStok.alasan_penyesuaian_id = alasanPenyesuaian.alasan_penyesuaian_stok_id;
            _penyesuaianStok.AlasanPenyesuaianStok = alasanPenyesuaian;

            _penyesuaianStok.tanggal = dtpTanggal.Value;
            _penyesuaianStok.penambahan_stok = NumberHelper.StringToDouble(txtPenambahanStokEtalase.Text);
            _penyesuaianStok.penambahan_stok_gudang = NumberHelper.StringToDouble(txtPenambahanStokGudang.Text);
            _penyesuaianStok.pengurangan_stok = NumberHelper.StringToDouble(txtPenguranganStokEtalase.Text);
            _penyesuaianStok.pengurangan_stok_gudang = NumberHelper.StringToDouble(txtPenguranganStokGudang.Text);

            _penyesuaianStok.keterangan = txtKeterangan.Text;

            var result = 0;
            var validationError = new ValidationError();

            if (_isNewData)
                result = _bll.Save(_penyesuaianStok, ref validationError);
            else
                result = _bll.Update(_penyesuaianStok, ref validationError);

            if (result > 0) 
            {
                Listener.Ok(this, _isNewData, _penyesuaianStok);

                if (_isNewData)
                {
                    base.ResetForm(this);
                    this._produk = null;
                    txtKodeProduk.Focus();

                }
                else
                    this.Close();

            }
            else
            {
                if (validationError.Message.Length > 0)
                {
                    MsgHelper.MsgWarning(validationError.Message);
                    base.SetFocusObject(validationError.PropertyName, this);
                }
                else
                    MsgHelper.MsgUpdateError();
            }                
        }

        private void txtGolongan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }

        private void txtKeterangan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }

        private void txtKodeProduk_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                var keyword = ((AdvancedTextbox)sender).Text;

                IProdukBll produkBll = new ProdukBll(_log);
                this._produk = produkBll.GetByKode(keyword);

                if (this._produk == null)
                {
                    var listOfProduk = produkBll.GetByName(keyword);

                    if (listOfProduk.Count == 0)
                    {
                        MsgHelper.MsgWarning("Data produk tidak ditemukan");
                        txtKodeProduk.Focus();
                        txtKodeProduk.SelectAll();
                    }
                    else if (listOfProduk.Count == 1)
                    {
                        this._produk = listOfProduk[0];

                        SetDataProduk(this._produk);
                        KeyPressHelper.NextFocus();
                    }
                    else // data lebih dari satu
                    {
                        var frmLookup = new FrmLookupReferensi("Data Produk", listOfProduk);
                        frmLookup.Listener = this;
                        frmLookup.ShowDialog();
                    }
                }
                else
                {
                    SetDataProduk(this._produk);
                    KeyPressHelper.NextFocus();
                }
            }
        }

        private void SetDataProduk(Produk produk)
        {
            txtKodeProduk.Text = produk.kode_produk;
            txtNamaProduk.Text = produk.nama_produk;
            txtStokEtalase.Text = produk.stok.ToString();
            txtStokGudang.Text = produk.stok_gudang.ToString();
        }

        public void Ok(object sender, object data)
        {
            if (data is Produk) // pencarian produk baku
            {
                this._produk = (Produk)data;

                SetDataProduk(this._produk);
                KeyPressHelper.NextFocus();
            }
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            throw new NotImplementedException();
        }

        private void txtKodeProduk_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
