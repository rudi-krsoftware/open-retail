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
using Microsoft.Reporting.WinForms;
using System.Reflection;
using System.IO;

namespace OpenRetail.App.UI.Template
{
    public partial class FrmPreviewReport : Form
    {
        private string _reportNameSpace = @"OpenRetail.Report.{0}.rdlc";
        private Assembly _assemblyReport;

        public FrmPreviewReport()
        {
            InitializeComponent();
            this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            this.reportViewer1.ZoomMode = ZoomMode.Percent;
            this.reportViewer1.ZoomPercent = 100;

            ColorManagerHelper.SetTheme(this, this);
            _assemblyReport = Assembly.LoadFrom("OpenRetail.Report.dll");
        }

        public FrmPreviewReport(string header, string reportName, ReportDataSource reportDataSource, IEnumerable<ReportParameter> parameters = null, bool isPreview = false)
            : this()
        {
            this.Text = header;

            reportName = string.Format(_reportNameSpace, reportName);
            var stream = _assemblyReport.GetManifestResourceStream(reportName);

            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.LocalReport.LoadReportDefinition(stream);

            if (!(parameters == null))
                this.reportViewer1.LocalReport.SetParameters(parameters);

            this.reportViewer1.ShowPrintButton = !isPreview;

            this.reportViewer1.RefreshReport();
        }

        public FrmPreviewReport(string header, string reportName, IList<ReportDataSource> reportDataSources, IEnumerable<ReportParameter> parameters = null, bool isPreview = false)
            : this()
        {
            this.Text = header;

            reportName = string.Format(_reportNameSpace, reportName);
            var stream = _assemblyReport.GetManifestResourceStream(reportName);

            this.reportViewer1.LocalReport.DataSources.Clear();

            foreach (var reportDataSource in reportDataSources)
            {
                this.reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            }

            this.reportViewer1.LocalReport.LoadReportDefinition(stream);

            if (!(parameters == null))
                this.reportViewer1.LocalReport.SetParameters(parameters);

            this.reportViewer1.ShowPrintButton = !isPreview;

            this.reportViewer1.RefreshReport();
        }

        private void FrmPreviewReport_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEsc(e))
                this.Close();
        }
    }
}
