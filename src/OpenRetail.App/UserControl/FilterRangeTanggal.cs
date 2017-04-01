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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenRetail.App.UserControl
{
    [DefaultEvent("BtnTampilkanClicked")]
    public partial class FilterRangeTanggal : System.Windows.Forms.UserControl
    {
        public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler BtnTampilkanClicked;
        public event EventHandler ChkTampilkanSemuaDataClicked;        

        public FilterRangeTanggal()
        {
            InitializeComponent();

            dtpTanggalMulai.Value = DateTime.Today;
            dtpTanggalSelesai.Value = DateTime.Today;

            this.EnabledChanged += delegate(object sender, EventArgs e)
            {
                chkTampilkanSemuaData.Checked = false;
            };
        }

        public DateTime TanggalMulai
        {
            get { return dtpTanggalMulai.Value; }
        }

        public DateTime TanggalSelesai
        {
            get { return dtpTanggalSelesai.Value; }
        }

        private void btnTampilkan_Click(object sender, EventArgs e)
        {
            if (BtnTampilkanClicked != null)
                BtnTampilkanClicked(sender, e);
        }

        private void chkTampilkanSemuaData_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            var isEnable = false;

            if (chk.Checked)
                isEnable = false;
            else
                isEnable = true;

            dtpTanggalMulai.Enabled = isEnable;
            dtpTanggalSelesai.Enabled = isEnable;
            btnTampilkan.Enabled = isEnable;

            if (ChkTampilkanSemuaDataClicked != null)
                ChkTampilkanSemuaDataClicked(sender, e);
        }
    }
}
