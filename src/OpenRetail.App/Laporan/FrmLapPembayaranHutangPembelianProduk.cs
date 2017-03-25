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
    public partial class FrmLapPembayaranHutangPembelianProduk : FrmSettingReportStandard
    {
        private IList<Supplier> _listOfSupplier = new List<Supplier>();
        private ILog _log;

        public FrmLapPembayaranHutangPembelianProduk(string header)
        {
            InitializeComponent();
            base.SetHeader(header);
            base.SetCheckBoxTitle("Pilih Supplier");
            base.ReSize(30);

            _log = MainProgram.log;

            chkTampilkanNota.Text = "Tampilkan nota beli";

            LoadSupplier();
            LoadBulanDanTahun();            
        }

        private void LoadSupplier()
        {
            ISupplierBll bll = new SupplierBll(_log);
            _listOfSupplier = bll.GetAll();

            FillDataHelper.FillSupplier(chkListBox, _listOfSupplier);
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
                if (chkTampilkanNota.Checked)
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
                                                
            IReportPembayaranHutangBeliProdukBll reportBll = new ReportPembayaranHutangBeliProdukBll(_log);
                                    
            IList<ReportPembayaranHutangPembelianProdukHeader> listOfReportPembayaranHutangPembelian = new List<ReportPembayaranHutangPembelianProdukHeader>();
            IList<string> listOfSupplierId = new List<string>();

            if (chkBoxTitle.Checked)
            {
                listOfSupplierId = base.GetSupplierId(_listOfSupplier);

                if (listOfSupplierId.Count == 0)
                {
                    MsgHelper.MsgWarning("Minimal 1 supplier harus dipilih");
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

                listOfReportPembayaranHutangPembelian = reportBll.GetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
            }
            else
            {
                periode = string.Format("Periode : {0} {1}", cmbBulan.Text, cmbTahun.Text);

                var bulan = cmbBulan.SelectedIndex + 1;
                var tahun = int.Parse(cmbTahun.Text);

                listOfReportPembayaranHutangPembelian = reportBll.GetByBulan(bulan, tahun);
            }

            if (listOfSupplierId.Count > 0 && listOfReportPembayaranHutangPembelian.Count > 0)
            {
                listOfReportPembayaranHutangPembelian = listOfReportPembayaranHutangPembelian.Where(f => listOfSupplierId.Contains(f.supplier_id))
                                                             .ToList();
            }

            if (listOfReportPembayaranHutangPembelian.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "ReportPembayaranHutangPembelianProdukHeader",
                    Value = listOfReportPembayaranHutangPembelian
                };

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("periode", periode));

                base.ShowReport(this.Text, "RvPembayaranHutangPembelianProdukHeader", reportDataSource, parameters);
            }
            else
            {
                MsgHelper.MsgInfo("Maaf laporan data pembayaran hutang pembelian tidak ditemukan");
            }
        }

        private void PreviewReportDetail()
        {
            var periode = string.Empty;

            IReportPembayaranHutangBeliProdukBll reportBll = new ReportPembayaranHutangBeliProdukBll(_log);
            
            IList<ReportPembayaranHutangPembelianProdukDetail> listOfReportPembayaranHutangPembelian = new List<ReportPembayaranHutangPembelianProdukDetail>();

            IList<string> listOfSupplierId = new List<string>();

            if (chkBoxTitle.Checked)
            {
                listOfSupplierId = base.GetSupplierId(_listOfSupplier);

                if (listOfSupplierId.Count == 0)
                {
                    MsgHelper.MsgWarning("Minimal 1 supplier harus dipilih");
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

                listOfReportPembayaranHutangPembelian = reportBll.DetailGetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
            }
            else
            {
                periode = string.Format("Periode : {0} {1}", cmbBulan.Text, cmbTahun.Text);

                var bulan = cmbBulan.SelectedIndex + 1;
                var tahun = int.Parse(cmbTahun.Text);

                listOfReportPembayaranHutangPembelian = reportBll.DetailGetByBulan(bulan, tahun);
            }

            if (listOfSupplierId.Count > 0 && listOfReportPembayaranHutangPembelian.Count > 0)
            {
                listOfReportPembayaranHutangPembelian = listOfReportPembayaranHutangPembelian.Where(f => listOfSupplierId.Contains(f.supplier_id))
                                                             .ToList();
            }

            if (listOfReportPembayaranHutangPembelian.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "ReportPembayaranHutangPembelianProdukDetail",
                    Value = listOfReportPembayaranHutangPembelian
                };

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("periode", periode));

                base.ShowReport(this.Text, "RvPembayaranHutangPembelianProdukDetail", reportDataSource, parameters);
            }
            else
            {
                MsgHelper.MsgInfo("Maaf laporan data pembayaran hutang pembelian tidak ditemukan");
            }
        }
    }
}
