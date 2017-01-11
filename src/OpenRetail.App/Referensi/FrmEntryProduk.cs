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

namespace OpenRetail.App.Referensi
{
    public partial class FrmEntryProduk : FrmEntryStandard
    {        
        private IProdukBll _bll = null; // deklarasi objek business logic layer 
        private Produk _produk = null;
        private IList<Golongan> _listOfGolongan;

        private bool _isNewData = false;
        
        public IListener Listener { private get; set; }

        public FrmEntryProduk(string header, Golongan golongan, IList<Golongan> listOfGolongan, IProdukBll bll)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            this._listOfGolongan = listOfGolongan;
            this._bll = bll;

            this._isNewData = true;
            txtKodeProduk.Text = this._bll.GetLastKodeProduk();

            LoadDataGolongan();

            if (golongan != null)
                cmbGolongan.SelectedItem = golongan.nama_golongan;
        }

        public FrmEntryProduk(string header, Produk produk, IList<Golongan> listOfGolongan, IProdukBll bll)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._listOfGolongan = listOfGolongan;
            this._bll = bll;
            this._produk = produk;

            txtKodeProduk.Text = this._produk.kode_produk;
            txtNamaProduk.Text = this._produk.nama_produk;
            txtSatuan.Text = this._produk.satuan;
            txtHargaBeli.Text = this._produk.harga_beli.ToString();
            txtHargaJual.Text = this._produk.harga_jual.ToString();
            txtStok.Text = this._produk.stok.ToString();
            txtStokGudang.Text = this._produk.stok_gudang.ToString();

            txtMinStokGudang.Text = this._produk.minimal_stok_gudang.ToString();

            LoadDataGolongan();
            if (this._produk.Golongan != null)
                cmbGolongan.SelectedItem = this._produk.Golongan.nama_golongan;
        }

        private void LoadDataGolongan()
        {
            cmbGolongan.Items.Clear();
            foreach (var golongan in _listOfGolongan)
            {
                cmbGolongan.Items.Add(golongan.nama_golongan);
            }

            if (_listOfGolongan.Count > 0)
                cmbGolongan.SelectedIndex = 0;
        }

        protected override void Simpan()
        {
            if (_isNewData)
                _produk = new Produk();

            var golongan = _listOfGolongan[cmbGolongan.SelectedIndex];
            _produk.golongan_id = golongan.golongan_id;
            _produk.Golongan = golongan;

            _produk.kode_produk = txtKodeProduk.Text;
            _produk.nama_produk = txtNamaProduk.Text;
            _produk.satuan = txtSatuan.Text;
            _produk.harga_beli = NumberHelper.StringToDouble(txtHargaBeli.Text);
            _produk.harga_jual = NumberHelper.StringToDouble(txtHargaJual.Text);
            _produk.stok = NumberHelper.StringToDouble(txtStok.Text);
            _produk.stok_gudang = NumberHelper.StringToDouble(txtStokGudang.Text);
            _produk.minimal_stok_gudang = NumberHelper.StringToDouble(txtMinStokGudang.Text);

            var result = 0;
            var validationError = new ValidationError();

            if (_isNewData)
                result = _bll.Save(_produk, ref validationError);
            else
                result = _bll.Update(_produk, ref validationError);

            if (result > 0) 
            {
                Listener.Ok(this, _isNewData, _produk);

                if (_isNewData)
                {
                    base.ResetForm(this);

                    txtKodeProduk.Text = this._bll.GetLastKodeProduk();
                    txtKodeProduk.Focus();
                }
                else
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
                {
                    MsgHelper.MsgDuplicate("kode produk");
                    txtKodeProduk.Focus();
                    txtKodeProduk.SelectAll();
                }                    
            }                
        }

        private void txtMinStokGudang_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }        
    }
}
