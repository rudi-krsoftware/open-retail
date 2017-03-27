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
    public partial class FrmEntryKaryawan : FrmEntryStandard
    {
        private IKaryawanBll _bll = null; // deklarasi objek business logic layer 
        private Karyawan _karyawan = null;
        private IList<Jabatan> _listOfJabatan;
        private bool _isNewData = false;
        
        public IListener Listener { private get; set; }

        public FrmEntryKaryawan(string header, IList<Jabatan> listOfJabatan, IKaryawanBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._listOfJabatan = listOfJabatan;
            this._bll = bll;

            this._isNewData = true;

            LoadJabatan();
            cmbJenisGaji.SelectedIndex = 0;
        }

        public FrmEntryKaryawan(string header, Karyawan karyawan, IList<Jabatan> listOfJabatan, IKaryawanBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._listOfJabatan = listOfJabatan;
            this._bll = bll;
            this._karyawan = karyawan;
            LoadJabatan();

            txtNama.Text = this._karyawan.nama_karyawan;
            txtAlamat.Text = this._karyawan.alamat;
            txtTelepon.Text = this._karyawan.telepon;

            if (this._karyawan.is_active)
                rdoAktif.Checked = true;
            else
                rdoNonAktif.Checked = true;

            if (this._karyawan.Jabatan != null)
                cmbJabatan.SelectedItem = this._karyawan.Jabatan.nama_jabatan;

            // pengaturan gaji            
            cmbJenisGaji.SelectedIndex = this._karyawan.jenis_gajian == JenisGajian.Mingguan ? 0 : 1;
            txtGajiPokok.Text = this._karyawan.gaji_pokok.ToString();
            txtLembur.Text = this._karyawan.gaji_lembur.ToString();
        }

        private void LoadJabatan()
        {
            cmbJabatan.Items.Clear();
            foreach (var jabatan in _listOfJabatan)
            {
                cmbJabatan.Items.Add(jabatan.nama_jabatan);
            }

            if (_listOfJabatan.Count > 0)
                cmbJabatan.SelectedIndex = 0;
        }

        protected override void Simpan()
        {
            if (_isNewData)
                _karyawan = new Karyawan();

            _karyawan.nama_karyawan = txtNama.Text;
            _karyawan.alamat = txtAlamat.Text;
            _karyawan.telepon = txtTelepon.Text;
            _karyawan.is_active = rdoAktif.Checked ? true : false;

            var jabatan = _listOfJabatan[cmbJabatan.SelectedIndex];
            _karyawan.jabatan_id = jabatan.jabatan_id;
            _karyawan.Jabatan = jabatan;

            _karyawan.jenis_gajian = (JenisGajian)cmbJenisGaji.SelectedIndex;
            _karyawan.gaji_pokok = NumberHelper.StringToDouble(txtGajiPokok.Text);
            _karyawan.gaji_lembur = NumberHelper.StringToDouble(txtLembur.Text);

            var result = 0;
            var validationError = new ValidationError();

            if (_isNewData)
                result = _bll.Save(_karyawan, ref validationError);
            else
                result = _bll.Update(_karyawan, ref validationError);

            if (result > 0) 
            {
                Listener.Ok(this, _isNewData, _karyawan);

                if (_isNewData)
                {
                    base.ResetForm(this);
                    txtNama.Focus();

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

        private void txtGolongan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }

        private void txtLembur_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }

        private void cmbJenisGaji_SelectedIndexChanged(object sender, EventArgs e)
        {
            label8.Text = ((ComboBox)sender).SelectedIndex == 0 ? "Gaji Per Hari" : "Gaji Bulanan";
        }
    }
}
