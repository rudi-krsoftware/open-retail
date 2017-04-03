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
using System.Drawing.Printing;

namespace OpenRetail.App.Pengaturan
{
    public partial class FrmPengaturanUmum : FrmEntryStandard
    {
        private PengaturanUmum _pengaturanUmum = null;
        
        public FrmPengaturanUmum(string header, PengaturanUmum pengaturanUmum)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._pengaturanUmum = pengaturanUmum;

            LoadPrinter(this._pengaturanUmum.nama_printer);
            chkCetakOtomatis.Checked = this._pengaturanUmum.is_auto_print;
        }

        private void LoadPrinter(string defaultPrinter)
        {
            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                cmbPrinter.Items.Add(printer);
            }

            if (defaultPrinter.Length > 0)
                cmbPrinter.Text = defaultPrinter;
            else
            {
                if (cmbPrinter.Items.Count > 0)
                    cmbPrinter.SelectedIndex = 0;
            }
        }

        protected override void Simpan()
        {
            _pengaturanUmum.nama_printer = cmbPrinter.Text;
            _pengaturanUmum.is_auto_print = chkCetakOtomatis.Checked;

            var appConfigFile = string.Format("{0}\\OpenRetail.exe.config", Utils.GetAppPath());
            AppConfigHelper.SaveValue("printerName", cmbPrinter.Text, appConfigFile);
            AppConfigHelper.SaveValue("isAutoPrinter", chkCetakOtomatis.Checked.ToString(), appConfigFile);

            this.Close();    
        }
    }
}
