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

using log4net;
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.App.UI.Template;
using OpenRetail.App.Helper;

namespace OpenRetail.App.Pengeluaran
{
    public partial class FrmEntryPembayaranKasbon : FrmEntryStandard
    {                    
        private IPembayaranKasbonBll _bll = null; // deklarasi objek business logic layer 
        private Kasbon _kasbon = null;
        private PembayaranKasbon _pembayaranKasbon = null;
        
        private bool _isNewData = false;
        private Pengguna _pengguna;
        private ILog _log;

        public IListener Listener { private get; set; }

        public FrmEntryPembayaranKasbon(string header, Kasbon kasbon)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);

            this._log = MainProgram.log;
            this._bll = new PembayaranKasbonBll(this._log);
            this._kasbon = kasbon;
            this._pengguna = MainProgram.pengguna;

            this._isNewData = true;
            txtNota.Text = this._bll.GetLastNota();
            txtNamaKaryawan.Text = this._kasbon.Karyawan.nama_karyawan;
            txtSisaKasbon.Text = this._kasbon.sisa.ToString();
        }

        public FrmEntryPembayaranKasbon(string header, Kasbon kasbon, PembayaranKasbon pembayaranKasbon)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);
            
            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();

            this._log = MainProgram.log;
            this._bll = new PembayaranKasbonBll(this._log);
            this._kasbon = kasbon;
            this._pembayaranKasbon = pembayaranKasbon;
            this._pengguna = MainProgram.pengguna;

            txtNota.Text = this._pembayaranKasbon.nota;
            dtpTanggal.Value = (DateTime)this._pembayaranKasbon.tanggal;
            txtNamaKaryawan.Text = this._kasbon.Karyawan.nama_karyawan;
            txtSisaKasbon.Text = this._kasbon.sisa.ToString();
            txtKeterangan.Text = this._pembayaranKasbon.keterangan;
            txtJumlah.Text = this._pembayaranKasbon.nominal.ToString();
        }

        protected override void Simpan()
        {
            if (_isNewData)
                _pembayaranKasbon = new PembayaranKasbon();

            _pembayaranKasbon.kasbon_id = this._kasbon.kasbon_id;
            _pembayaranKasbon.Kasbon = this._kasbon;

            _pembayaranKasbon.pengguna_id = this._pengguna.pengguna_id;
            _pembayaranKasbon.Pengguna = this._pengguna;

            _pembayaranKasbon.nota = txtNota.Text;
            _pembayaranKasbon.tanggal = dtpTanggal.Value;
            _pembayaranKasbon.nominal = NumberHelper.StringToDouble(txtJumlah.Text);
            _pembayaranKasbon.sisa_kasbon = NumberHelper.StringToDouble(txtSisaKasbon.Text);
            _pembayaranKasbon.keterangan = txtKeterangan.Text;            

            var result = 0;
            var validationError = new ValidationError();

            if (_isNewData)
                result = _bll.Save(_pembayaranKasbon, ref validationError);
            else
                result = _bll.Update(_pembayaranKasbon, ref validationError);

            if (result > 0) 
            {
                Listener.Ok(this, _isNewData, _pembayaranKasbon);
                this.Close();
            }
            else
            {
                if (validationError.Message != null && validationError.Message.Length > 0)
                {
                    MsgHelper.MsgWarning(validationError.Message);
                    base.SetFocusObject(validationError.PropertyName, this);
                }
            }                
        }

        private void txtKeterangan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }
    }
}
