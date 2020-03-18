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

using ConceptCave.WaitCursor;
using log4net;
using OpenRetail.Bll.Api.Report;
using OpenRetail.Bll.Service.Report;
using OpenRetail.Helper;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Model.Report;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenRetail.App.Laporan
{
    public partial class FrmLapPenjualanProdukFavorit : FrmSettingReportEmptyBody
    {
        private ILog _log;

        public FrmLapPenjualanProdukFavorit(string header)
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            _log = MainProgram.log;

            base.SetHeader(header);

            dtpTanggalMulai.Value = DateTime.Today;
            dtpTanggalSelesai.Value = DateTime.Today;

            LoadBulanDanTahun();
        }

        private void LoadBulanDanTahun()
        {
            FillDataHelper.FillBulan(cmbBulan, true);
            FillDataHelper.FillTahun(cmbTahun, true);
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

            IReportJualProdukBll reportBll = new ReportJualProdukBll(_log);
            IList<ReportProdukFavorit> listOfReport;

            var limit = (int)updLimit.Value;

            if (rdoTanggal.Checked)
            {
                if (!DateTimeHelper.IsValidRangeTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value))
                {
                    MsgHelper.MsgNotValidRangeTanggal();
                    return;
                }

                var tanggalMulai = DateTimeHelper.DateToString(dtpTanggalMulai.Value);
                var tanggalSelesai = DateTimeHelper.DateToString(dtpTanggalSelesai.Value);

                periode = dtpTanggalMulai.Value == dtpTanggalSelesai.Value ? string.Format("Periode : {0}", tanggalMulai) : string.Format("Periode : {0} s.d {1}", tanggalMulai, tanggalSelesai);

                listOfReport = reportBll.ProdukFavoritGetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value, limit);
            }
            else
            {
                periode = string.Format("Periode : {0} {1}", cmbBulan.Text, cmbTahun.Text);

                var bulan = cmbBulan.SelectedIndex + 1;
                var tahun = int.Parse(cmbTahun.Text);

                listOfReport = reportBll.ProdukFavoritGetByBulan(bulan, tahun, limit);
            }

            if (listOfReport.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "DsProdukFavorit",
                    Value = listOfReport
                };

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("periode", periode));

                base.ShowReport(this.Text, "RvProdukFavorit", reportDataSource, parameters);
            }
            else
            {
                MsgHelper.MsgInfo("Maaf laporan data penjualan produk favorit tidak ditemukan");
            }
        }
    }
}