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

using log4net;
using OpenRetail.App.UI.Template;
using OpenRetail.Model;
using OpenRetail.Model.Report;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.App.Helper;
using OpenRetail.Report;
using OpenRetail.Bll.Api.Report;
using OpenRetail.Bll.Service.Report;
using ConceptCave.WaitCursor;
using Microsoft.Reporting.WinForms;

namespace OpenRetail.App.Laporan
{
    public partial class FrmLapStokProduk : FrmSettingReportEmptyBody
    {
        private ILog _log;

        public FrmLapStokProduk(string header)
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            _log = MainProgram.log;

            base.SetHeader(header);
            cmbStatusStok.SelectedIndex = 0;            
        }

        protected override void Preview()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                PreviewReport();   
            }
        }

        private void PreviewReport()
        {
            var periode = string.Empty;

            IReportStokProdukBll reportBll = new ReportStokProdukBll(_log);

            IList<ReportStokProduk> listOfReportStokProduk = new List<ReportStokProduk>();

            var statusStok = (StatusStok)cmbStatusStok.SelectedIndex + 1;

            listOfReportStokProduk = reportBll.GetStokByStatus(statusStok);

            if (txtNamaProduk.Text.Length > 0)
            {
                listOfReportStokProduk = listOfReportStokProduk.Where(f => f.nama_produk.ToLower().Contains(txtNamaProduk.Text.ToLower()))
                                                               .ToList();
            }

            if (listOfReportStokProduk.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "ReportProduk",
                    Value = listOfReportStokProduk
                };

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("status", string.Format("Status Stok : {0}", cmbStatusStok.Text)));

                base.ShowReport(this.Text, "RvStokProduk", reportDataSource, parameters);
            }
            else
            {
                MsgHelper.MsgInfo("Maaf laporan data stok produk tidak ditemukan");
            }
        }
    }
}
