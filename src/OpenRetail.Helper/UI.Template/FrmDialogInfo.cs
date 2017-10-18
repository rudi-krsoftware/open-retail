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

using OpenRetail.Helper;
using OpenRetail.Helper.UserControl;

namespace OpenRetail.Helper.UI.Template
{
    /// <summary>
    /// Base class form untuk entry data
    /// </summary>
    public partial class FrmDialogInfo : BaseFrmLookup
    {
        public FrmDialogInfo()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);            
        }
        
        protected override void SetHeader(string header)
        {
            this.Text = header;
            this.lblHeader.Text = header;
        }

        private void btnTutup_Click(object sender, EventArgs e)
        {
            Batal();
        }

        private void FrmDialogInfo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEsc(e))
                Batal();
        }        
    }
}
