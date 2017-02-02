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
using OpenRetail.Model.DTO;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.App.Helper;
using OpenRetail.Report;
using OpenRetail.Bll.Api.Report;
using OpenRetail.Bll.Service.Report;
using AutoMapper;
using ConceptCave.WaitCursor;

namespace OpenRetail.App.Laporan
{
    public partial class FrmLapPembelianProduk : FrmSettingReportStandard
    {
        private IList<Supplier> _listOfSupplier = new List<Supplier>();
        private ILog _log;

        public FrmLapPembelianProduk(string header)
        {
            InitializeComponent();
            base.SetHeader(header);
            base.SetCheckBoxTitle("Pilih Supplier");
            base.ReSize(120);

            _log = MainProgram.log;
            
            chkTampilkanNota.Visible = false;

            chkTampilkanRincianNota.Visible = true;
            chkTampilkanRincianNota.Enabled = true;
            chkTampilkanRincianNota.Text = "Tampilkan rincian penjualan";

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

            IReportBeliProdukBll reportBll = new ReportBeliProdukBll(_log);

            IList<BeliProduk> listOfBeli = new List<BeliProduk>();
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

                listOfBeli = reportBll.GetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
            }
            else
            {
                periode = string.Format("Periode : {0} {1}", cmbBulan.Text, cmbTahun.Text);

                var bulan = cmbBulan.SelectedIndex + 1;
                var tahun = int.Parse(cmbTahun.Text);

                listOfBeli = reportBll.GetByBulan(bulan, tahun);
            }

            if (listOfSupplierId.Count > 0 && listOfBeli.Count > 0)
            {
                listOfBeli = listOfBeli.Where(f => f.Supplier != null && listOfSupplierId.Contains(f.supplier_id))
                                       .ToList();
            }

            if (listOfBeli.Count > 0)
            {
                var listOfSupplier = listOfBeli.Select(f => f.Supplier).ToList()
                                               .GroupBy(gb => gb.supplier_id).Select(g => g.First()).ToList();

                var listOfPengguna = listOfBeli.Select(f => f.Supplier).ToList()
                                               .GroupBy(gb => gb.supplier_id).Select(g => g.First()).ToList();

                var listOfBeliDto = Mapper.Map<IList<BeliProdukDto>>(listOfBeli);

                var rpt = new CrPembelianProdukHeader();
                rpt.Database.Tables["Supplier"].SetDataSource(listOfSupplier);
                rpt.Database.Tables["Pengguna"].SetDataSource(listOfPengguna);
                rpt.Database.Tables["BeliProduk"].SetDataSource(listOfBeliDto);

                rpt.SetParameterValue("periode", periode);

                var frmPreview = new FrmPreviewReport(this.Text, rpt);
                frmPreview.ShowDialog();
            }
            else
            {
                MsgHelper.MsgInfo("Maaf data pembelian tidak ditemukan");
            }
        }

        private void PreviewReportDetail()
        {
            var periode = string.Empty;

            IReportBeliProdukBll reportBll = new ReportBeliProdukBll(_log);

            IList<BeliProduk> listOfBeli = new List<BeliProduk>();
            IList<ItemBeliProduk> listOfItemBeli = new List<ItemBeliProduk>();

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

                listOfBeli = reportBll.GetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
                listOfItemBeli = reportBll.DetailGetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
            }
            else
            {
                periode = string.Format("Periode : {0} {1}", cmbBulan.Text, cmbTahun.Text);

                var bulan = cmbBulan.SelectedIndex + 1;
                var tahun = int.Parse(cmbTahun.Text);

                listOfBeli = reportBll.GetByBulan(bulan, tahun);
                listOfItemBeli = reportBll.DetailGetByBulan(bulan, tahun);
            }

            if (listOfSupplierId.Count > 0 && listOfBeli.Count > 0)
            {
                listOfBeli = listOfBeli.Where(f => f.Supplier != null && listOfSupplierId.Contains(f.supplier_id))
                                       .ToList();
            }

            if (listOfBeli.Count > 0)
            {
                var listOfSupplier = listOfBeli.Select(f => f.Supplier).ToList()
                                               .GroupBy(gb => gb.supplier_id).Select(g => g.First()).ToList();

                var listOfProduk = listOfItemBeli.Select(f => f.Produk).ToList()
                                                 .GroupBy(gb => gb.produk_id).Select(g => g.First()).ToList();

                var listOfBeliDto = Mapper.Map<IList<BeliProdukDto>>(listOfBeli);
                var listOfItemBeliDto = Mapper.Map<IList<ItemBeliProdukDto>>(listOfItemBeli);

                var rpt = new CrPembelianProdukDetail();
                rpt.Database.Tables["Supplier"].SetDataSource(listOfSupplier);
                rpt.Database.Tables["Produk"].SetDataSource(listOfProduk);
                rpt.Database.Tables["BeliProduk"].SetDataSource(listOfBeliDto);
                rpt.Database.Tables["ItemBeliProduk"].SetDataSource(listOfItemBeliDto);
                rpt.SetParameterValue("periode", periode);

                var frmPreview = new FrmPreviewReport(this.Text, rpt);
                frmPreview.ShowDialog();
            }
            else
            {
                MsgHelper.MsgInfo("Maaf data pembelian tidak ditemukan");
            }
        }
    }
}
