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

using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.App.Helper;
using OpenRetail.App.UI.Template;
using OpenRetail.Model;

namespace OpenRetail.App.Transaksi
{
    public partial class FrmEntryAlamatKirim : FrmEntryStandard
    {
        //private Customer _customer = null;
        private AlamatKirim _alamatKirim = null;
        private Customer _customer = null;
        private JualProduk _jual = null;

        public IListener Listener { private get; set; }

        public FrmEntryAlamatKirim(string header, Customer customer, JualProduk jual)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);
            base.SetHeader(header);

            this._alamatKirim = new AlamatKirim();
            this._customer = customer;
            this._jual = jual;

            if (this._jual == null)
                chkIsSdac.Checked = true;
            else
                chkIsSdac.Checked = this._jual.is_sdac;

            chkIsSdac_CheckedChanged(chkIsSdac, new EventArgs());
        }

        protected override void Simpan()
        {
            this._alamatKirim.is_sdac = chkIsSdac.Checked;
            this._alamatKirim.kepada = txtKepada.Text;
            this._alamatKirim.alamat = txtAlamat.Text;
            this._alamatKirim.kecamatan = txtKecamatan.Text;
            this._alamatKirim.kelurahan = txtKelurahan.Text;
            this._alamatKirim.kota = txtKota.Text;
            this._alamatKirim.kode_pos = txtKodePos.Text;
            this._alamatKirim.telepon = txtTelepon.Text;

            var validationError = new ValidationError();
            IAlamatKirimBll bll = new AlamatKirimBll();

            if (bll.IsValid(this._alamatKirim, ref validationError))
            {
                Listener.Ok(this, this._alamatKirim);
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

        private void txtTelepon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }

        private void chkIsSdac_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;

            pnlAlamatKirim.Enabled = !chk.Checked;

            if (chk.Checked)
            {
                txtKepada.Text = _customer.nama_customer;
                txtAlamat.Text = _customer.alamat;
                txtKecamatan.Text = _customer.kecamatan;
                txtKelurahan.Text = _customer.kelurahan;
                txtKota.Text = _customer.kota;
                txtKodePos.Text = _customer.kode_pos;
                txtTelepon.Text = _customer.telepon;
            }
            else
            {
                if (_jual != null)
                {
                    txtKepada.Text = _jual.kirim_kepada;
                    txtAlamat.Text = _jual.kirim_alamat;
                    txtKecamatan.Text = _jual.kirim_kecamatan;
                    txtKelurahan.Text = _jual.kirim_kelurahan;
                    txtKota.Text = _jual.kirim_kota;
                    txtKodePos.Text = _jual.kirim_kode_pos;
                    txtTelepon.Text = _jual.kirim_telepon;
                }
            }                
        }
    }
}
