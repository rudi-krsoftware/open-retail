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

using OpenRetail.App.Helper;

namespace OpenRetail.App.UI.Template
{
    public partial class FrmDialogImport : BaseFrmList
    {
        public FrmDialogImport()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);
        }

        /// <summary>
        /// Method protected untuk mengeset header form entri
        /// </summary>
        /// <param name="header"></param>
        protected void SetHeader(string header)
        {
            this.Text = header;
            this.lblHeader.Text = header;
        }

        /// <summary>
        /// Method override untuk menghandle proses buka file excel
        /// </summary>
        protected virtual void OpenFileExcel()
        {
        }

        /// <summary>
        /// Method override untuk menghandle proses browse file excel
        /// </summary>
        protected virtual void BrowseFileExcel()
        {
        }

        private void btnProses_Click(object sender, EventArgs e)
        {
            ImportData();
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            Selesai();
        }       

        private void btnContohFile_Click(object sender, EventArgs e)
        {
            OpenFileExcel();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            BrowseFileExcel();
        }

        private void FrmDialogImport_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEsc(e))
                Selesai();
        }

        private void FrmDialogImport_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F11:
                    ImportData();
                    break;

                default:
                    break;
            }
        }
    }
}
