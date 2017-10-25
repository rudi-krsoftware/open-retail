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
using OpenRetail.Model;
using OpenRetail.Model.Report;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.Report;
using OpenRetail.Bll.Api.Report;
using OpenRetail.Bll.Service.Report;
using ConceptCave.WaitCursor;
using Microsoft.Reporting.WinForms;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Helper;

namespace OpenRetail.App.Laporan
{
    public partial class FrmLapPemasukanPengeluaran : FrmSettingReportEmptyBody
    {
        private ILog _log;

        public FrmLapPemasukanPengeluaran(string header)
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

            IReportPemasukanPengeluaranBll reportBll = new ReportPemasukanPengeluaranBll(_log);
            ReportPemasukanPengeluaran report;

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

                report = reportBll.GetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
            }
            else
            {
                periode = string.Format("Periode : {0} {1}", cmbBulan.Text, cmbTahun.Text);

                var bulan = cmbBulan.SelectedIndex + 1;
                var tahun = int.Parse(cmbTahun.Text);

                report = reportBll.GetByBulan(bulan, tahun);
            }

            var beban = report.list_of_beban.Sum(f => f.jumlah);
            var totalPengeluaran = report.persediaan_awal + report.pembelian + beban;

            if (report.penjualan > 0 || totalPengeluaran > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "DsBebanUsaha",
                    Value = report.list_of_beban
                };

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("periode", periode));
                parameters.Add(new ReportParameter("penjualan", NumberHelper.NumberToString(report.penjualan)));
                parameters.Add(new ReportParameter("pembelian", NumberHelper.NumberToString(report.pembelian)));
                parameters.Add(new ReportParameter("total_pengeluaran", NumberHelper.NumberToString(totalPengeluaran)));

                base.ShowReport(this.Text, "RvPemasukanPengeluaran", reportDataSource, parameters);
            }            
            else
            {
                MsgHelper.MsgInfo("Maaf laporan data pemasukan dan pengeluaran tidak ditemukan");
            }
        }
    }
}
