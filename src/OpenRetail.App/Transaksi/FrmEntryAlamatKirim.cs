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

using OpenRetail.Helper;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Model;
using System;
using System.Windows.Forms;

namespace OpenRetail.App.Transaksi
{
    public partial class FrmEntryAlamatKirim : FrmEntryStandard
    {
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

            var kepada1 = _customer.nama_customer;
            var kepada2 = _customer.alamat;
            var kepada3 = _customer.get_wilayah_lengkap;
            var kepada4 = string.Format("HP: {0}", _customer.telepon.NullToString());

            if (!chk.Checked) // alamat kirim tidak sama dengan alamat customer
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