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
            this._alamatKirim.kepada = txtKepada1.Text;
            this._alamatKirim.alamat = txtKepada2.Text;
            this._alamatKirim.kecamatan = txtKepada3.Text;
            this._alamatKirim.kelurahan = txtKepada4.Text;

            Listener.Ok(this, this._alamatKirim);
            this.Close();
        }

        private void chkIsSdac_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;

            pnlAlamatKirim.Enabled = !chk.Checked;

            var kecamatan = string.IsNullOrEmpty(_customer.kecamatan) ? string.Empty : _customer.kecamatan;
            var kelurahan = string.IsNullOrEmpty(_customer.kelurahan) ? string.Empty : _customer.kelurahan;
            var kota = string.IsNullOrEmpty(_customer.kota) ? string.Empty : _customer.kota;
            var kodePos = (string.IsNullOrEmpty(_customer.kode_pos) || _customer.kode_pos == "0") ? string.Empty : _customer.kode_pos;
            var telepon = string.IsNullOrEmpty(_customer.telepon) ? string.Empty : _customer.telepon;

            var kepada1 = _customer.nama_customer;
            var kepada2 = _customer.alamat;
            var kepada3 = string.Format("{0} - {1} - {2} - {3}", kecamatan, kelurahan, kota, kodePos);
            var kepada4 = telepon;

            if (!chk.Checked)
            {
                if (_jual != null)
                {
                    kepada1 = string.IsNullOrEmpty(_jual.kirim_kepada) ? kepada1 : _jual.kirim_kepada;
                    kepada2 = string.IsNullOrEmpty(_jual.kirim_alamat) ? kepada2 : _jual.kirim_alamat;
                    kepada3 = string.IsNullOrEmpty(_jual.kirim_kecamatan) ? kepada3 : _jual.kirim_kecamatan;
                    kepada4 = string.IsNullOrEmpty(_jual.kirim_kelurahan) ? kepada4 : _jual.kirim_kelurahan;
                }
            }

            txtKepada1.Text = kepada1;
            txtKepada2.Text = kepada2;
            txtKepada3.Text = kepada3;
            txtKepada4.Text = kepada4;
        }
    }
}
