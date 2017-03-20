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
    public partial class FrmLapReturPembelianProduk : FrmSettingReportStandard
    {
        private IList<Supplier> _listOfSupplier = new List<Supplier>();
        private ILog _log;

        public FrmLapReturPembelianProduk(string header)
        {
            InitializeComponent();
            base.SetHeader(header);
            base.SetCheckBoxTitle("Pilih Supplier");
            base.ReSize(10);

            _log = MainProgram.log;

            chkTampilkanNota.Text = "Tampilkan rincian retur";

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

        private IList<string> GetSupplierId(IList<Supplier> listOfSupplier)
        {
            var listOfSupplierId = new List<string>();

            for (var i = 0; i < chkListBox.Items.Count; i++)
            {
                if (chkListBox.GetItemChecked(i))
                {
                    var supplier = listOfSupplier[i];
                    listOfSupplierId.Add(supplier.supplier_id);
                }
            }

            return listOfSupplierId;
        }

        private void PreviewReportHeader()
        {
            var periode = string.Empty;
                                    
            IReportReturBeliProdukBll reportBll = new ReportReturBeliProdukBll(_log);
                        
            IList<ReportReturPembelianProdukHeader> listOfReportHutangPembelian = new List<ReportReturPembelianProdukHeader>();
            IList<string> listOfSupplierId = new List<string>();

            if (chkBoxTitle.Checked)
            {
                listOfSupplierId = GetSupplierId(_listOfSupplier);

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

                listOfReportHutangPembelian = reportBll.GetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
            }
            else
            {
                periode = string.Format("Periode : {0} {1}", cmbBulan.Text, cmbTahun.Text);

                var bulan = cmbBulan.SelectedIndex + 1;
                var tahun = int.Parse(cmbTahun.Text);

                listOfReportHutangPembelian = reportBll.GetByBulan(bulan, tahun);
            }

            if (listOfSupplierId.Count > 0 && listOfReportHutangPembelian.Count > 0)
            {
                listOfReportHutangPembelian = listOfReportHutangPembelian.Where(f => listOfSupplierId.Contains(f.supplier_id))
                                                             .ToList();
            }

            if (listOfReportHutangPembelian.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "ReportReturPembelianProdukHeader",
                    Value = listOfReportHutangPembelian
                };

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("periode", periode));

                base.ShowReport(this.Text, "RvReturPembelianProdukHeader", reportDataSource, parameters);
            }
            else
            {
                MsgHelper.MsgInfo("Maaf laporan data retur pembelian tidak ditemukan");
            }
        }

        private void PreviewReportDetail()
        {
            var periode = string.Empty;

            IReportReturBeliProdukBll reportBll = new ReportReturBeliProdukBll(_log);
            
            IList<ReportReturPembelianProdukDetail> listOfReportHutangPembelian = new List<ReportReturPembelianProdukDetail>();

            IList<string> listOfSupplierId = new List<string>();

            if (chkBoxTitle.Checked)
            {
                listOfSupplierId = GetSupplierId(_listOfSupplier);

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

                listOfReportHutangPembelian = reportBll.DetailGetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
            }
            else
            {
                periode = string.Format("Periode : {0} {1}", cmbBulan.Text, cmbTahun.Text);

                var bulan = cmbBulan.SelectedIndex + 1;
                var tahun = int.Parse(cmbTahun.Text);

                listOfReportHutangPembelian = reportBll.DetailGetByBulan(bulan, tahun);
            }

            if (listOfSupplierId.Count > 0 && listOfReportHutangPembelian.Count > 0)
            {
                listOfReportHutangPembelian = listOfReportHutangPembelian.Where(f => listOfSupplierId.Contains(f.supplier_id))
                                                             .ToList();
            }

            if (listOfReportHutangPembelian.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "ReportReturPembelianProdukDetail",
                    Value = listOfReportHutangPembelian
                };

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("periode", periode));

                base.ShowReport(this.Text, "RvReturPembelianProdukDetail", reportDataSource, parameters);
            }
            else
            {
                MsgHelper.MsgInfo("Maaf laporan data retur pembelian tidak ditemukan");
            }
        }
    }
}
