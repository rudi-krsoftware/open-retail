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
using OpenRetail.Helper;
using OpenRetail.Report;
using OpenRetail.Bll.Api.Report;
using OpenRetail.Bll.Service.Report;
using ConceptCave.WaitCursor;
using Microsoft.Reporting.WinForms;
using OpenRetail.Helper.UI.Template;

namespace OpenRetail.App.Laporan
{
    public partial class FrmLapPenjualanPerGolongan : FrmSettingReportStandard
    {
        private IList<Golongan> _listOfGolongan = new List<Golongan>();
        private ILog _log;

        public FrmLapPenjualanPerGolongan(string header)
        {
            InitializeComponent();
            base.SetHeader(header);
            base.SetCheckBoxTitle("Pilih Golongan");
            base.SetToolTip("Cari Golongan ...");
            base.ReSize(120);

            _log = MainProgram.log;
            
            chkTampilkanNota.Visible = false;

            chkTampilkanRincianNota.Visible = true;
            chkTampilkanRincianNota.Enabled = true;
            chkTampilkanRincianNota.Text = "Tampilkan rincian penjualan";

            LoadGolongan();
            LoadBulanDanTahun();            
        }

        private void LoadGolongan()
        {
            IGolonganBll bll = new GolonganBll(_log);
            _listOfGolongan = bll.GetAll();

            FillDataHelper.FillGolongan(chkListBox, _listOfGolongan);
        }

        private void LoadGolongan(string name)
        {
            IGolonganBll bll = new GolonganBll(_log);
            _listOfGolongan = bll.GetByName(name);

            FillDataHelper.FillGolongan(chkListBox, _listOfGolongan);
        }

        private void LoadBulanDanTahun()
        {
            FillDataHelper.FillBulan(cmbBulan, true);
            FillDataHelper.FillTahun(cmbTahun, true);
        }

        protected override void Cari()
        {
            LoadGolongan(txtKeyword.Text);
        }

        protected override void Preview()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                if (chkTampilkanRincianNota.Checked)
                {
                    PreviewReportDetail();
                }
                else
                {
                    PreviewReportHeader();
                }
            }
        }

        private void PreviewReportHeader()
        {
            var periode = string.Empty;
            
            IReportJualProdukBll reportBll = new ReportJualProdukBll(_log);

            IList<ReportPenjualanProdukPerGolongan> listOfReportPenjualan = new List<ReportPenjualanProdukPerGolongan>();
            IList<string> listOfGolonganId = new List<string>();

            if (chkBoxTitle.Checked)
            {
                listOfGolonganId = base.GetGolonganId(_listOfGolongan);

                if (listOfGolonganId.Count == 0)
                {
                    MsgHelper.MsgWarning("Minimal 1 golongan harus dipilih");
                    return;
                }
            }

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

                listOfReportPenjualan = reportBll.PerGolonganGetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
            }
            else
            {
                periode = string.Format("Periode : {0} {1}", cmbBulan.Text, cmbTahun.Text);

                var bulan = cmbBulan.SelectedIndex + 1;
                var tahun = int.Parse(cmbTahun.Text);

                listOfReportPenjualan = reportBll.PerGolonganGetByBulan(bulan, tahun);
            }

            if (listOfGolonganId.Count > 0 && listOfReportPenjualan.Count > 0)
            {
                listOfReportPenjualan = listOfReportPenjualan.Where(f => listOfGolonganId.Contains(f.golongan_id))
                                                             .ToList();
            }

            if (listOfReportPenjualan.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "DsReportPenjualanProdukPerGolongan",
                    Value = listOfReportPenjualan
                };

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("periode", periode));

                base.ShowReport(this.Text, "RvPenjualanProdukPerGolonganHeader", reportDataSource, parameters);
            }
            else
            {
                MsgHelper.MsgInfo("Maaf laporan data penjualan tidak ditemukan");
            }
        }

        private void PreviewReportDetail()
        {
            var periode = string.Empty;

            IReportJualProdukBll reportBll = new ReportJualProdukBll(_log);

            IList<ReportPenjualanProduk> listOfReportPenjualan = new List<ReportPenjualanProduk>();

            IList<string> listOfGolonganId = new List<string>();

            if (chkBoxTitle.Checked)
            {
                listOfGolonganId = base.GetGolonganId(_listOfGolongan);

                if (listOfGolonganId.Count == 0)
                {
                    MsgHelper.MsgWarning("Minimal 1 golongan harus dipilih");
                    return;
                }
            }

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

                listOfReportPenjualan = reportBll.PerGolonganDetailGetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
            }
            else
            {
                periode = string.Format("Periode : {0} {1}", cmbBulan.Text, cmbTahun.Text);

                var bulan = cmbBulan.SelectedIndex + 1;
                var tahun = int.Parse(cmbTahun.Text);

                listOfReportPenjualan = reportBll.PerGolonganDetailGetByBulan(bulan, tahun);
            }

            if (listOfGolonganId.Count > 0 && listOfReportPenjualan.Count > 0)
            {
                listOfReportPenjualan = listOfReportPenjualan.Where(f => listOfGolonganId.Contains(f.golongan_id))
                                                             .ToList();
            }

            if (listOfReportPenjualan.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "DsReportPenjualanProduk",
                    Value = listOfReportPenjualan
                };

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("periode", periode));

                base.ShowReport(this.Text, "RvPenjualanProdukPerGolonganDetail", reportDataSource, parameters);
            }
            else
            {
                MsgHelper.MsgInfo("Maaf laporan data penjualan tidak ditemukan");
            }
        }
    }
}
