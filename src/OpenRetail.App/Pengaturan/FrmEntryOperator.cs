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

namespace OpenRetail.App.Pengaturan
{
    public partial class FrmEntryOperator : FrmEntryStandard
    {        
        private IPenggunaBll _bll = null; // deklarasi objek business logic layer 
        private Pengguna _operator = null;
        private IList<Role> listOfRole;
        private bool _isNewData = false;

        public IListener Listener { private get; set; }

        public FrmEntryOperator(string header, IList<Role> listOfRole, IPenggunaBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._bll = bll;
            this.listOfRole = listOfRole;

            this._isNewData = true;
            LoadRole();
        }

        public FrmEntryOperator(string header, Pengguna userOperator, IList<Role> listOfRole, IPenggunaBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._operator = userOperator;
            this.listOfRole = listOfRole;

            LoadRole();

            txtNama.Text = this._operator.nama_pengguna;

            try
            {
                if (this._operator.Role != null)
                    cmbRole.SelectedItem = this._operator.Role.nama_role;
            }
            catch
            {

                if (listOfRole.Count > 0)
                    cmbRole.SelectedIndex = 0;
            }

            if (this._operator.is_active)
                rdoAktif.Checked = true;
            else
                rdoNonAktif.Checked = true;
        }

        private void LoadRole()
        {
            cmbRole.Items.Clear();
            foreach (var role in listOfRole.Where(f => f.is_active == true))
            {
                cmbRole.Items.Add(role.nama_role);
            }

            if (listOfRole.Count > 0)
                cmbRole.SelectedIndex = 0;
        }

        protected override void Simpan()
        {
            if (txtPassword.Text.Length > 0 && txtKonfirmasiPassword.Text.Length > 0)
            {
                if (txtPassword.Text != txtKonfirmasiPassword.Text)
                {
                    MsgHelper.MsgWarning("Password dan konfirmasi password harus sama");
                    txtPassword.Focus();
                    txtPassword.SelectAll();
                    return;
                }
            }            

            if (_isNewData)
                _operator = new Pengguna();

            _operator.nama_pengguna = txtNama.Text;
            _operator.pass_pengguna = txtPassword.Text;

            if (txtKonfirmasiPassword.Text.Length > 0)
                _operator.konf_pass_pengguna = CryptoHelper.GetMD5Hash(txtKonfirmasiPassword.Text, MainProgram.securityCode);

            var role = listOfRole[cmbRole.SelectedIndex];
            _operator.role_id = role.role_id;
            _operator.Role = role;
            _operator.is_active = rdoAktif.Checked;

            var result = 0;
            var validationError = new ValidationError();

            if (_isNewData)
                result = _bll.Save(_operator, ref validationError);
            else
                result = _bll.Update(_operator);

            if (result > 0) 
            {
                Listener.Ok(this, _isNewData, _operator);

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
    }
}
