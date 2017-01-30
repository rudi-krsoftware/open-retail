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
using CrystalDecisions.CrystalReports.Engine;

namespace OpenRetail.App.UI.Template
{
    public partial class FrmPreviewReport : Form
    {
        public FrmPreviewReport()
        {
            InitializeComponent();
        }

        public FrmPreviewReport(string header, ReportClass reportSource)
            : this()
        {
            this.Text = header;
            crViewer.ReportSource = reportSource;
            crViewer.RemoveMainTab();
        }

        private void FrmPreviewReport_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEsc(e))
                this.Close();
        }
    }

    internal static class CrViewerExtensions
    {
        public static void RemoveMainTab(this CrystalDecisions.Windows.Forms.CrystalReportViewer crv)
        {
            foreach (System.Windows.Forms.Control ct in crv.Controls)
            {
                if (ct is CrystalDecisions.Windows.Forms.PageView)
                {
                    foreach (var c in ct.Controls)
                    {
                        if (c is System.Windows.Forms.TabControl)
                        {
                            var tab = (ct as CrystalDecisions.Windows.Forms.PageView).Controls[0] as System.Windows.Forms.TabControl;
                            tab.ItemSize = new System.Drawing.Size(0, 1);
                            tab.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
                            tab.Appearance = System.Windows.Forms.TabAppearance.Buttons;

                            break;
                        }
                    }
                }
            }
        }
    }
}
