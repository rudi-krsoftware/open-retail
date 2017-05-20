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
    public partial class FrmEntryLabelNota : FrmEntryStandard
    {
        private LabelAlamatKirim _labelAlamatKirim = null;
        private Customer _customer = null;
        private JualProduk _jual = null;
        private PengaturanUmum _pengaturanUmum = null;

        public IListener Listener { private get; set; }

        public FrmEntryLabelNota(string header, Customer customer, JualProduk jual)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);
            base.SetHeader(header);

            this._pengaturanUmum = MainProgram.pengaturanUmum;
            this._labelAlamatKirim = new LabelAlamatKirim();
            this._customer = customer;
            this._jual = jual;

            SetLabelNota();
        }

        private void SetLabelNota()
        {
            var dari1 = this._pengaturanUmum.list_of_label_nota[0].keterangan;
            var dari2 = this._pengaturanUmum.list_of_label_nota[1].keterangan;

            var kepada1 = this._customer.nama_customer;
            var kepada2 = this._customer.alamat;
            var kepada3 = "HP: " + _customer.telepon;
            var kepada4 = string.Empty;

            if (!(this._jual == null))
            {
                if (!(this._jual.label_dari1 == null || this._jual.label_dari1.Length == 0))
                    dari1 = this._jual.label_dari1;

                if (!(this._jual.label_dari2 == null || this._jual.label_dari2.Length == 0))
                    dari2 = this._jual.label_dari2;

                if (!(this._jual.label_kepada1 == null || this._jual.label_kepada1.Length == 0))
                    kepada1 = this._jual.label_kepada1;

                if (!(this._jual.label_kepada2 == null || this._jual.label_kepada2.Length == 0))
                    kepada2 = this._jual.label_kepada2;

                if (!(this._jual.label_kepada3 == null || this._jual.label_kepada3.Length == 0))
                    kepada3 = this._jual.label_kepada3;

                if (!(this._jual.label_kepada4 == null || this._jual.label_kepada4.Length == 0))
                    kepada4 = this._jual.label_kepada4;
            }

            txtDari1.Text = dari1;
            txtDari2.Text = dari2;

            txtKepada1.Text = kepada1;
            txtKepada2.Text = kepada2;
            txtKepada3.Text = kepada3;
            txtKepada4.Text = kepada4;
        }

        protected override void Simpan()
        {
            this._labelAlamatKirim.dari1 = txtDari1.Text;
            this._labelAlamatKirim.dari2 = txtDari2.Text;
            this._labelAlamatKirim.kepada1 = txtKepada1.Text;
            this._labelAlamatKirim.kepada2 = txtKepada2.Text;
            this._labelAlamatKirim.kepada3 = txtKepada3.Text;
            this._labelAlamatKirim.kepada4 = txtKepada4.Text;

            var validationError = new ValidationError();
            ILabelAlamatKirimBll bll = new LabelAlamatKirimBll();

            if (bll.IsValid(this._labelAlamatKirim, ref validationError))
            {
                Listener.Ok(this, this._labelAlamatKirim);
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

        private void txtKepada4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }
    }
}
