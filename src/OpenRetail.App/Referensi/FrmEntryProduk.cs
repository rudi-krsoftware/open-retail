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
using OpenRetail.App.UserControl;

namespace OpenRetail.App.Referensi
{
    public partial class FrmEntryProduk : FrmEntryStandard
    {        
        private IProdukBll _bll = null; // deklarasi objek business logic layer 
        private Produk _produk = null;
        private IList<Golongan> _listOfGolongan;

        private IList<AdvancedTextbox> _listOfTxtHargaGrosir = new List<AdvancedTextbox>();
        private IList<AdvancedTextbox> _listOfTxtJumlahGrosir = new List<AdvancedTextbox>();
        private IList<AdvancedTextbox> _listOfTxtDiskonGrosir = new List<AdvancedTextbox>();

        private bool _isNewData = false;
        
        public IListener Listener { private get; set; }

        public FrmEntryProduk(string header, Golongan golongan, IList<Golongan> listOfGolongan, IProdukBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._listOfGolongan = listOfGolongan;
            this._bll = bll;

            this._isNewData = true;
            txtKodeProduk.Text = this._bll.GetLastKodeProduk();

            LoadDataGolongan();
            LoadInputGrosir();

            if (golongan != null)
                cmbGolongan.SelectedItem = golongan.nama_golongan;
        }

        public FrmEntryProduk(string header, Produk produk, IList<Golongan> listOfGolongan, IProdukBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

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
            txtDiskon.Text = this._produk.diskon.ToString();
            txtStok.Text = this._produk.stok.ToString();
            txtStokGudang.Text = this._produk.stok_gudang.ToString();
            txtMinStokGudang.Text = this._produk.minimal_stok_gudang.ToString();

            LoadInputGrosir();

            LoadDataGolongan();
            if (this._produk.Golongan != null)
                cmbGolongan.SelectedItem = this._produk.Golongan.nama_golongan;
        }

        private void LoadInputGrosir()
        {
            _listOfTxtHargaGrosir.Add(txtHargaGrosir1);
            _listOfTxtHargaGrosir.Add(txtHargaGrosir2);
            _listOfTxtHargaGrosir.Add(txtHargaGrosir3);

            _listOfTxtJumlahGrosir.Add(txtJumlahMinimalGrosir1);
            _listOfTxtJumlahGrosir.Add(txtJumlahMinimalGrosir2);
            _listOfTxtJumlahGrosir.Add(txtJumlahMinimalGrosir3);

            _listOfTxtDiskonGrosir.Add(txtDiskonGrosir1);
            _listOfTxtDiskonGrosir.Add(txtDiskonGrosir2);
            _listOfTxtDiskonGrosir.Add(txtDiskonGrosir3);

            if (this._produk != null)
            {
                var listOfHargaGrosir = this._produk.list_of_harga_grosir;
                if (listOfHargaGrosir.Count > 0)
                {
                    var index = 0;
                    foreach (var grosir in listOfHargaGrosir)
                    {
                        var txtHargaGrosir = _listOfTxtHargaGrosir[index];
                        txtHargaGrosir.Text = grosir.harga_grosir.ToString();

                        var txtJumlahMinGrosir = _listOfTxtJumlahGrosir[index];
                        txtJumlahMinGrosir.Text = grosir.jumlah_minimal.ToString();

                        var txtDiskonGrosir = _listOfTxtDiskonGrosir[index];
                        txtDiskonGrosir.Text = grosir.diskon.ToString();

                        index++;
                    }
                }
            }            
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
            
            if (_produk.list_of_harga_grosir.Count == 0)
            {
                var index = 0;
                foreach (var item in _listOfTxtHargaGrosir)
                {                    
                    var txtHargaGrosir = _listOfTxtHargaGrosir[index];
                    var txtJumlahMinGrosir = _listOfTxtJumlahGrosir[index];
                    var txtDiskonGrosir = _listOfTxtDiskonGrosir[index];

                    var hargaGrosir = new HargaGrosir
                    {
                        harga_ke = index + 1,
                        harga_grosir = NumberHelper.StringToDouble(txtHargaGrosir.Text),
                        jumlah_minimal = NumberHelper.StringToDouble(txtJumlahMinGrosir.Text, true),
                        diskon = NumberHelper.StringToDouble(txtDiskonGrosir.Text, true)
                    };

                    _produk.list_of_harga_grosir.Add(hargaGrosir);

                    index++;
                }
            }
            else
            {
                var index = 0;
                foreach (var item in _produk.list_of_harga_grosir)
	            {
                    var txtHargaGrosir = _listOfTxtHargaGrosir[index];
                    var txtJumlahMinGrosir = _listOfTxtJumlahGrosir[index];
                    var txtDiskonGrosir = _listOfTxtDiskonGrosir[index];
                    
                    item.harga_grosir = NumberHelper.StringToDouble(txtHargaGrosir.Text);
                    item.jumlah_minimal = NumberHelper.StringToDouble(txtJumlahMinGrosir.Text, true);
                    item.diskon = NumberHelper.StringToDouble(txtDiskonGrosir.Text, true);

                    index++;
	            }
            }

            var golongan = _listOfGolongan[cmbGolongan.SelectedIndex];
            _produk.golongan_id = golongan.golongan_id;
            _produk.Golongan = golongan;

            _produk.kode_produk = txtKodeProduk.Text;
            _produk.nama_produk = txtNamaProduk.Text;
            _produk.satuan = txtSatuan.Text;
            _produk.harga_beli = NumberHelper.StringToDouble(txtHargaBeli.Text);
            _produk.harga_jual = NumberHelper.StringToDouble(txtHargaJual.Text);
            _produk.diskon = NumberHelper.StringToDouble(txtDiskon.Text, true);
            _produk.stok = NumberHelper.StringToDouble(txtStok.Text, true);
            _produk.stok_gudang = NumberHelper.StringToDouble(txtStokGudang.Text, true);
            _produk.minimal_stok_gudang = NumberHelper.StringToDouble(txtMinStokGudang.Text, true);

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
