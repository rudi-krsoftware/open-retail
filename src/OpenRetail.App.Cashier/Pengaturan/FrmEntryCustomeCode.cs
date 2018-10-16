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
using ConceptCave.WaitCursor;

namespace OpenRetail.App.Cashier.Pengaturan
{
    public partial class FrmEntryCustomeCode : FrmEntryStandard
    {
        private PengaturanUmum _pengaturanUmum = null;
        private bool _isAutocutCode;

        public FrmEntryCustomeCode(string header, PengaturanUmum pengaturanUmum, bool isAutocutCode)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._pengaturanUmum = pengaturanUmum;
            this._isAutocutCode = isAutocutCode;

            txtCustomeCode.Text = isAutocutCode ? this._pengaturanUmum.autocut_code : this._pengaturanUmum.open_cash_drawer_code;
        }

        protected override void Simpan()
        {
            if (_isAutocutCode)
                _pengaturanUmum.autocut_code = txtCustomeCode.Text;
            else
                _pengaturanUmum.open_cash_drawer_code = txtCustomeCode.Text;

            this.Close();
        }
    }
}
