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
using OpenRetail.Bll.Service;
using OpenRetail.App.UI.Template;
using OpenRetail.App.Helper;

namespace OpenRetail.App.Pengaturan
{
    public partial class FrmProfilPerusahaan : FrmEntryStandard
    {
        private Profil _profil = null;
        
        public IListener Listener { private get; set; }

        public FrmProfilPerusahaan(string header, Profil profil)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._profil = profil;

            if (this._profil != null)
            {
                txtNamaPerusahaan.Text = this._profil.nama_profil;
                txtAlamat.Text = this._profil.alamat;
                txtKota.Text = this._profil.kota;
                txtTelepon.Text = this._profil.telepon;
            }            
        }

        protected override void Simpan()
        {
            if (this._profil == null)
                this._profil = new Profil();

            _profil.nama_profil = txtNamaPerusahaan.Text;
            _profil.alamat = txtAlamat.Text;
            _profil.kota = txtKota.Text;
            _profil.telepon = txtTelepon.Text;

            var result = 0;
            var validationError = new ValidationError();

            IProfilBll bll = new ProfilBll(MainProgram.log);
            result = bll.Save(_profil, ref validationError);

            if (result > 0) 
            {
                Listener.Ok(this, _profil);
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
    }
}
