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
    public partial class FrmEntryJenisPengeluaran : FrmEntryStandard
    {
        private IJenisPengeluaranBll _bll = null; // deklarasi objek business logic layer 
        private JenisPengeluaran _jenisPengeluaran = null;
        private bool _isNewData = false;

        public IListener Listener { private get; set; }

        public FrmEntryJenisPengeluaran(string header, IJenisPengeluaranBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._bll = bll;

            this._isNewData = true;
        }

        public FrmEntryJenisPengeluaran(string header, JenisPengeluaran jenisPengeluaran, IJenisPengeluaranBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._jenisPengeluaran = jenisPengeluaran;

            txtJenisPengeluaran.Text = this._jenisPengeluaran.nama_jenis_pengeluaran;
        }

        protected override void Simpan()
        {
            if (_isNewData)
                _jenisPengeluaran = new JenisPengeluaran();

            _jenisPengeluaran.nama_jenis_pengeluaran = txtJenisPengeluaran.Text;

            var result = 0;
            var validationError = new ValidationError();

            if (_isNewData)
                result = _bll.Save(_jenisPengeluaran, ref validationError);
            else
                result = _bll.Update(_jenisPengeluaran, ref validationError);

            if (result > 0) 
            {
                Listener.Ok(this, _isNewData, _jenisPengeluaran);

                if (_isNewData)
                {
                    base.ResetForm(this);
                    txtJenisPengeluaran.Focus();

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

        private void txtJenisPengeluaran_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }
    }
}
