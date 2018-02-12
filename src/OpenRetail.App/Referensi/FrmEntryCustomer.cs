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
using OpenRetail.Helper.UI.Template;
using OpenRetail.Helper;

namespace OpenRetail.App.Referensi
{
    public partial class FrmEntryCustomer : FrmEntryStandard
    {
        private ICustomerBll _bll = null; // deklarasi objek business logic layer 
        private Customer _customer = null;
        private bool _isNewData = false;

        private IList<Provinsi> _listOfProvinsi;
        private IList<Kabupaten> _listOfKabupaten;
        private IList<Kecamatan> _listOfKecamatan;
        private IList<Wilayah> _listOfWilayah;

        public IListener Listener { private get; set; }

        public FrmEntryCustomer(string header, ICustomerBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._bll = bll;
            this._listOfWilayah = MainProgram.ListOfWilayah;

            this._isNewData = true;
            LoadProvinsi();
        }

        public FrmEntryCustomer(string header, Customer customer, ICustomerBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._listOfWilayah = MainProgram.ListOfWilayah;
            this._customer = customer;

            LoadProvinsi();

            txtCustomer.Text = this._customer.nama_customer;

            if (this._customer.Provinsi != null)
                cmbProvinsi.SelectedItem = this._customer.Provinsi.nama_provinsi;

            if (this._customer.Kabupaten != null)
                cmbKabupaten.SelectedItem = this._customer.Kabupaten.nama_kabupaten;

            if (this._customer.Kecamatan != null)
                cmbKecamatan.SelectedItem = this._customer.Kecamatan.nama_kecamatan;

            txtAlamat.Text = this._customer.alamat;
            txtKodePos.Text = this._customer.kode_pos;
            txtKontak.Text = this._customer.kontak;
            txtTelepon.Text = this._customer.telepon;
            txtDiskon.Text = this._customer.diskon.ToString();
            txtPlafonPiutang.Text = this._customer.plafon_piutang.ToString();
        }

        private void LoadProvinsi()
        {
            _listOfProvinsi = _listOfWilayah.GroupBy(g => new { g.provinsi_id, g.nama_provinsi })
                                            .Select(f => new Provinsi { provinsi_id = f.FirstOrDefault().provinsi_id, nama_provinsi = f.FirstOrDefault().nama_provinsi })
                                            .OrderBy(f => f.nama_provinsi)
                                            .ToList();

            cmbProvinsi.Items.Clear();
            cmbProvinsi.Items.Add("Pilih");

            foreach (var provinsi in _listOfProvinsi)
            {
                cmbProvinsi.Items.Add(provinsi.nama_provinsi);
            }

            cmbProvinsi.SelectedIndex = 0;
        }

        private void LoadKabupaten(string provinsiId)
        {
            _listOfKabupaten = _listOfWilayah.Where(f => f.provinsi_id == provinsiId)
                                             .GroupBy(g => new { g.kabupaten_id, g.nama_kabupaten })
                                             .Select(f => new Kabupaten { kabupaten_id = f.FirstOrDefault().kabupaten_id, nama_kabupaten = f.FirstOrDefault().nama_kabupaten })
                                             .OrderBy(f => f.nama_kabupaten)
                                             .ToList();

            cmbKabupaten.Items.Clear();
            cmbKabupaten.Items.Add("Pilih");

            foreach (var kabupaten in _listOfKabupaten)
            {
                cmbKabupaten.Items.Add(kabupaten.nama_kabupaten);
            }

            cmbKabupaten.SelectedIndex = 0;
        }

        private void LoadKecamatan(string kabupatenId)
        {            
            _listOfKecamatan = _listOfWilayah.Where(f => f.kabupaten_id == kabupatenId)
                                             .GroupBy(g => new { g.kecamatan_id, g.nama_kecamatan })
                                             .Select(f => new Kecamatan { kecamatan_id = f.FirstOrDefault().kecamatan_id, nama_kecamatan = f.FirstOrDefault().nama_kecamatan })
                                             .OrderBy(f => f.nama_kecamatan)
                                             .ToList();            
            
            cmbKecamatan.Items.Clear();
            cmbKecamatan.Items.Add("Pilih");

            foreach (var kabupaten in _listOfKecamatan)
            {
                cmbKecamatan.Items.Add(kabupaten.nama_kecamatan);
            }

            cmbKecamatan.SelectedIndex = 0;
        }

        protected override void Simpan()
        {
            if (_isNewData)
                _customer = new Customer();

            _customer.nama_customer = txtCustomer.Text;

            _customer.provinsi_id = null;
            _customer.Provinsi = null;

            if (cmbProvinsi.SelectedIndex > 0)
            {
                var provinsi = _listOfProvinsi[cmbProvinsi.SelectedIndex - 1];

                _customer.provinsi_id = provinsi.provinsi_id;
                _customer.Provinsi = provinsi;
            }

            _customer.kabupaten_id = null;
            _customer.Kabupaten = null;

            if (cmbKabupaten.SelectedIndex > 0)
            {
                var kabupaten = _listOfKabupaten[cmbKabupaten.SelectedIndex - 1];

                _customer.kabupaten_id = kabupaten.kabupaten_id;
                _customer.Kabupaten = kabupaten;
            }

            _customer.kecamatan_id = null;
            _customer.Kecamatan = null;

            if (cmbKecamatan.SelectedIndex > 0)
            {
                var kecamatan = _listOfKecamatan[cmbKecamatan.SelectedIndex - 1];

                _customer.kecamatan_id = kecamatan.kecamatan_id;
                _customer.Kecamatan = kecamatan;
            }

            _customer.alamat = txtAlamat.Text;
            _customer.desa = string.Empty;
            _customer.kelurahan = string.Empty;
            _customer.kota = string.Empty;
            
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

        private void cmbProvinsi_SelectedIndexChanged(object sender, EventArgs e)
        {
            var provinsiId = ((ComboBox)sender).SelectedIndex == 0 ? "0" : _listOfProvinsi[((ComboBox)sender).SelectedIndex - 1].provinsi_id;
            LoadKabupaten(provinsiId);
        }

        private void cmbKabupaten_SelectedIndexChanged(object sender, EventArgs e)
        {
            var kabupatenId = ((ComboBox)sender).SelectedIndex == 0 ? "0" : _listOfKabupaten[((ComboBox)sender).SelectedIndex - 1].kabupaten_id;
            LoadKecamatan(kabupatenId);
        }        
    }
}
