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
using OpenRetail.App.UI.Template;
using OpenRetail.App.Helper;

namespace OpenRetail.App.Pengeluaran
{
    public partial class FrmEntryKasbon : FrmEntryStandard
    {            
        private IKasbonBll _bll = null; // deklarasi objek business logic layer 
        private Kasbon _kasbon = null;
        private IList<Karyawan> _listOfKaryawan;
        
        private bool _isNewData = false;
        private Pengguna _pengguna;

        public IListener Listener { private get; set; }

        public FrmEntryKasbon(string header, IList<Karyawan> listOfKaryawan, IKasbonBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._listOfKaryawan = listOfKaryawan;
            this._bll = bll;
            this._pengguna = MainProgram.pengguna;

            this._isNewData = true;
            txtNota.Text = this._bll.GetLastNota();

            LoadDataKaryawan();
        }

        public FrmEntryKasbon(string header, Kasbon kasbon, IList<Karyawan> listOfKaryawan, IKasbonBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._listOfKaryawan = listOfKaryawan;
            this._bll = bll;
            this._pengguna = MainProgram.pengguna;
            this._kasbon = kasbon;

            txtNota.Text = this._kasbon.nota;
            dtpTanggal.Value = (DateTime)this._kasbon.tanggal;
            txtKeterangan.Text = this._kasbon.keterangan;
            txtJumlah.Text = this._kasbon.nominal.ToString();

            LoadDataKaryawan();
            if (this._kasbon.Karyawan != null)
                cmbKaryawan.SelectedItem = this._kasbon.Karyawan.nama_karyawan;
        }

        private void LoadDataKaryawan()
        {
            cmbKaryawan.Items.Add("--- Pilih karyawan ---");

            FillDataHelper.FillKaryawan(cmbKaryawan, _listOfKaryawan, false);
            cmbKaryawan.SelectedIndex = 0;
        }

        protected override void Simpan()
        {
            if (_isNewData)
                _kasbon = new Kasbon();                

            if (cmbKaryawan.SelectedIndex == 0)
            {
                MsgHelper.MsgWarning("Karyawan belum dipilih");
                return;
            }

            _kasbon.nota = txtNota.Text;
            _kasbon.tanggal = dtpTanggal.Value;
            _kasbon.nominal = NumberHelper.StringToDouble(txtJumlah.Text);
            _kasbon.keterangan = txtKeterangan.Text;

            var karyawan = _listOfKaryawan[cmbKaryawan.SelectedIndex - 1];
            _kasbon.karyawan_id = karyawan.karyawan_id;
            _kasbon.Karyawan = karyawan;

            _kasbon.pengguna_id = this._pengguna.pengguna_id;
            _kasbon.Pengguna = this._pengguna;

            var result = 0;
            var validationError = new ValidationError();

            if (_isNewData)
                result = _bll.Save(_kasbon, ref validationError);
            else
                result = _bll.Update(_kasbon, ref validationError);

            if (result > 0) 
            {
                Listener.Ok(this, _isNewData, _kasbon);
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
