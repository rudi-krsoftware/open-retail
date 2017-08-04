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
    public partial class FrmEntryCustomer : FrmEntryStandard
    {
        private ICustomerBll _bll = null; // deklarasi objek business logic layer 
        private Customer _customer = null;
        private bool _isNewData = false;
        
        public IListener Listener { private get; set; }

        public FrmEntryCustomer(string header, ICustomerBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._bll = bll;

            this._isNewData = true;
        }

        public FrmEntryCustomer(string header, Customer customer, ICustomerBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._customer = customer;

            txtCustomer.Text = this._customer.nama_customer;
            txtAlamat.Text = this._customer.alamat;
            txtDesa.Text = this._customer.desa;
            txtKelurahan.Text = this._customer.kelurahan;
            txtKecamatan.Text = this._customer.kecamatan;            
            txtKota.Text = this._customer.kota;
            txtKabupaten.Text = this._customer.kabupaten;
            txtKodePos.Text = this._customer.kode_pos;
            txtKontak.Text = this._customer.kontak;
            txtTelepon.Text = this._customer.telepon;
            txtDiskon.Text = this._customer.diskon.ToString();
            txtPlafonPiutang.Text = this._customer.plafon_piutang.ToString();
        }

        protected override void Simpan()
        {
            if (_isNewData)
                _customer = new Customer();

            _customer.nama_customer = txtCustomer.Text;
            _customer.alamat = txtAlamat.Text;
            _customer.desa = txtDesa.Text;
            _customer.kelurahan = txtKelurahan.Text;
            _customer.kecamatan = txtKecamatan.Text;            
            _customer.kota = txtKota.Text;
            _customer.kabupaten = txtKabupaten.Text;
            _customer.kode_pos = txtKodePos.Text;
            _customer.kontak = txtKontak.Text;
            _customer.telepon = txtTelepon.Text;
            _customer.diskon = NumberHelper.StringToDouble(txtDiskon.Text, true);
            _customer.plafon_piutang = NumberHelper.StringToDouble(txtPlafonPiutang.Text);

            var result = 0;
            var validationError = new ValidationError();

            if (_isNewData)
                result = _bll.Save(_customer, ref validationError);
            else
                result = _bll.Update(_customer, ref validationError);

            if (result > 0) 
            {
                Listener.Ok(this, _isNewData, _customer);

                if (_isNewData)
                {
                    base.ResetForm(this);
                    txtCustomer.Focus();
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

        private void txtPlafonPiutang_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }        
    }
}
