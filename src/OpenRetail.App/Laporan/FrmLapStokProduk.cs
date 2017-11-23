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
    public partial class FrmLapStokProduk : FrmSettingReportEmptyBody
    {
        private ILog _log;
        private IList<Supplier> _listOfSupplier = new List<Supplier>();
        private IList<Golongan> _listOfGolongan = new List<Golongan>();

        public FrmLapStokProduk(string header)
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            _log = MainProgram.log;

            base.SetHeader(header);
            cmbStatusStok.SelectedIndex = 0;

            LoadSupplier();
            LoadGolongan();
        }

        private void LoadSupplier()
        {
            ISupplierBll bll = new SupplierBll(_log);
            _listOfSupplier = bll.GetAll();

            FillDataHelper.FillSupplier(cmbSupplier, _listOfSupplier);

            if (_listOfSupplier.Count > 0)
                cmbSupplier.SelectedIndex = 0;
            else
                rdoStokBerdasarkanSupplier.Enabled = false;
        }

        private void LoadGolongan()
        {
            IGolonganBll bll = new GolonganBll(_log);
            _listOfGolongan = bll.GetAll();

            FillDataHelper.FillGolongan(cmbGolongan, _listOfGolongan);

            if (_listOfGolongan.Count > 0)
                cmbGolongan.SelectedIndex = 0;
            else
                rdoStokBerdasarkanGolongan.Enabled = false;
        }

        protected override void Preview()
        {
            var keterangan = string.Empty;

            IReportStokProdukBll reportBll = new ReportStokProdukBll(_log);
            IList<ReportStokProduk> listOfReportStokProduk = new List<ReportStokProduk>();            

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                if (rdoStatusStok.Checked)
                {
                    keterangan = string.Format("Jumlah stok berdasarkan status stok {0}", cmbStatusStok.Text);

                    var statusStok = (StatusStok)cmbStatusStok.SelectedIndex + 1;
                    listOfReportStokProduk = reportBll.GetStokByStatus(statusStok);
                }
                else if (rdoStokKurangDari.Checked)
                {
                    keterangan = string.Format("Jumlah stok kurang dari {0}", txtStok.Text);
                    listOfReportStokProduk = reportBll.GetStokKurangDari(NumberHelper.StringToDouble(txtStok.Text));
                }
                else if (rdoStokBerdasarkanSupplier.Checked)
                {
                    keterangan = string.Format("Jumlah stok berdasarkan supplier {0}", cmbSupplier.Text);

                    var supplierId = _listOfSupplier[cmbSupplier.SelectedIndex].supplier_id;
                    listOfReportStokProduk = reportBll.GetStokBerdasarkanSupplier(supplierId);
                }
                else if (rdoStokBerdasarkanGolongan.Checked)
                {
                    keterangan = string.Format("Jumlah stok berdasarkan golongan {0}", cmbGolongan.Text);

                    var golonganId = _listOfGolongan[cmbGolongan.SelectedIndex].golongan_id;
                    listOfReportStokProduk = reportBll.GetStokBerdasarkanGolongan(golonganId);
                }
                else if (rdoStokBerdasarkanNama.Checked)
                {
                    keterangan = string.Format("Jumlah stok berdasarkan pencarian nama {0}", txtNamaProduk.Text);

                    listOfReportStokProduk = reportBll.GetStokBerdasarkanNama(txtNamaProduk.Text);
                }

                PreviewReport(listOfReportStokProduk, keterangan);                   
            }
        }

        private void PreviewReport(IList<ReportStokProduk> listOfReportStokProduk, string keterangan)
        {
            var periode = string.Empty;

            if (listOfReportStokProduk.Count > 0)
            {
                var reportDataSource = new ReportDataSource
                {
                    Name = "ReportProduk",
                    Value = listOfReportStokProduk
                };

                var parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("keterangan", keterangan));

                base.ShowReport(this.Text, "RvStokProduk", reportDataSource, parameters);
            }
            else
            {
                MsgHelper.MsgInfo("Maaf laporan data stok produk tidak ditemukan");
            }
        }
    }
}
