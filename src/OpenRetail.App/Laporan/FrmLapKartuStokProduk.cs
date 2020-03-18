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
using OpenRetail.App.Lookup;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Api.Report;
using OpenRetail.Bll.Service;
using OpenRetail.Bll.Service.Report;
using OpenRetail.Helper;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Helper.UserControl;
using OpenRetail.Model;
using OpenRetail.Model.Report;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenRetail.App.Laporan
{
    public partial class FrmLapKartuStokProduk : FrmSettingReportEmptyBody, IListener
    {
        private ILog _log;
        private Produk _produk = null;
        private IList<Produk> _listOfProduk = new List<Produk>();

        public FrmLapKartuStokProduk(string header)
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

            IReportKartuStokBll reportBll = new ReportKartuStokBll(_log);
            IList<ReportKartuStok> listOfReport = new List<ReportKartuStok>();

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

                if (chkFilterTambahan.Checked)
                {
                    IList<string> listOfKode = GetListKodeProduk(_listOfProduk);

                    if (listOfKode.Count == 0)
                    {
                        MsgHelper.MsgWarning("Minimal satu nama produk harus dipilih !");
                        txtNamaProduk.Focus();
                        return;
                    }

                    listOfReport = reportBll.GetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value, listOfKode);
                }
                else
                    listOfReport = reportBll.GetByTanggal(dtpTanggalMulai.Value, dtpTanggalSelesai.Value);
            }
            else
            {
                periode = string.Format("Periode : {0} {1}", cmbBulan.Text, cmbTahun.Text);

                var bulan = cmbBulan.SelectedIndex + 1;
                var tahun = int.Parse(cmbTahun.Text);

                if (chkFilterTambahan.Checked)
                {
                    IList<string> listOfKode = GetListKodeProduk(_listOfProduk);

                    if (listOfKode.Count == 0)
                    {
                        MsgHelper.MsgWarning("Minimal satu nama produk harus dipilih !");
                        txtNamaProduk.Focus();
                        return;
                    }

                    listOfReport = reportBll.GetByBulan(bulan, tahun, listOfKode);
                }
                else
                    listOfReport = reportBll.GetByBulan(bulan, tahun);
            }

            if (listOfReport.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "DsReportKartuStok",
                    Value = listOfReport
                };

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("periode", periode));

                base.ShowReport(this.Text, "RvKartuStok", reportDataSource, parameters);
            }
            else
            {
                MsgHelper.MsgInfo("Maaf laporan data kartu stok tidak ditemukan");
            }
        }

        private IList<String> GetListKodeProduk(IList<Produk> listOfProduk)
        {
            var result = new List<string>();

            for (int i = 0; i < listOfProduk.Count; i++)
            {
                if (chkListOfProduk.GetItemChecked(i))
                {
                    result.Add(listOfProduk[i].kode_produk.ToLower());
                }
            }

            return result;
        }

        private void txtNamaProduk_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                var keyword = ((AdvancedTextbox)sender).Text;

                IProdukBll produkBll = new ProdukBll(_log);
                this._produk = produkBll.GetByKode(keyword);

                if (this._produk == null)
                {
                    var listOfProduk = produkBll.GetByName(keyword, false);

                    if (listOfProduk.Count == 0)
                    {
                        MsgHelper.MsgWarning("Data produk tidak ditemukan");
                        txtNamaProduk.Focus();
                        txtNamaProduk.SelectAll();
                    }
                    else if (listOfProduk.Count == 1)
                    {
                        this._produk = listOfProduk[0];

                        FillListProduk(this._produk);
                    }
                    else // data lebih dari satu
                    {
                        var frmLookup = new FrmLookupReferensi("Data Produk", listOfProduk);
                        frmLookup.Listener = this;
                        frmLookup.ShowDialog();
                    }
                }
                else
                {
                    FillListProduk(this._produk);
                }
            }
        }

        private void FillListProduk(Produk produk)
        {
            txtNamaProduk.Clear();
            this._listOfProduk.Add(produk);
            chkListOfProduk.Items.Add(produk.nama_produk);
            chkListOfProduk.SetItemChecked(chkListOfProduk.Items.Count - 1, true);
        }

        public void Ok(object sender, object data)
        {
            if (data is Produk) // pencarian produk
            {
                this._produk = (Produk)data;

                FillListProduk(this._produk);
            }
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            throw new NotImplementedException();
        }

        private void chkFilterTambahan_CheckedChanged(object sender, EventArgs e)
        {
            var chkFilter = (CheckBox)sender;

            txtNamaProduk.Enabled = chkFilter.Checked;
            chkListOfProduk.Enabled = chkFilter.Checked;

            if (chkFilter.Checked)
                txtNamaProduk.Focus();
            else
            {
                _produk = null;
                _listOfProduk.Clear();
                txtNamaProduk.Clear();
                chkListOfProduk.Items.Clear();
            }
        }
    }
}