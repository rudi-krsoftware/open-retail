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
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using log4net;
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.App.Helper;
using OpenRetail.App.UI.Template;
using ConceptCave.WaitCursor;

namespace OpenRetail.App.Referensi
{
    public partial class FrmImportDataSupplier : FrmDialogImport
    {
        private const string ImportTitle = "Supplier";
        private ILog _log;

        public IListener Listener { private get; set; }

        public FrmImportDataSupplier(string header)
            : base()
        {
            InitializeComponent();
            base.SetHeader(header);

            _log = MainProgram.log;
        }

        protected override void OpenFileExcel()
        {
            var msg = "Untuk membuka file master {0} membutuhkan Ms Excel versi 2007 atau yang terbaru.\n\n" +
                      "Apakah proses ingin dilanjutkan ?";

            if (MsgHelper.MsgKonfirmasi(string.Format(msg, ImportTitle)))
            {
                var fileMaster = Utils.GetAppPath() + @"\File Import Excel\Master Data\data_supplier.xlsx";

                if (!File.Exists(fileMaster))
                {
                    msg = "Maaf file master {0} tidak ditemukan.";
                    MsgHelper.MsgWarning(string.Format(msg, ImportTitle));
                    return;
                }

                try
                {
                    using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                    {
                        Process.Start(fileMaster);
                    }
                }
                catch
                {
                    msg = "Gagal membuka file master {0} !!!.\n\n" +
                          "Cek apakah Ms Excel versi 2007 atau yang terbaru sudah terinstall ?";

                    MsgHelper.MsgWarning(string.Format(msg, ImportTitle));
                }
            }
        }

        protected override void BrowseFileExcel()
        {
            cmbWorksheet.Items.Clear();

            using (var dlgOpen = new OpenFileDialog())
            {
                // Excel file only
                dlgOpen.Filter = "Microsoft Excel files (*.xlsx)|*.xlsx";
                dlgOpen.Title = string.Format("Import data master {0}", ImportTitle);

                var result = dlgOpen.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtPath.Text = dlgOpen.FileName;

                    IImportExportDataBll<Supplier> importDataBll = new ImportExportDataSupplierBll(txtPath.Text, _log);

                    if (importDataBll.IsOpened())
                    {
                        var msg = "Maaf file master {0} sedang dibuka, silahkan ditutup terlebih dulu.";
                        MsgHelper.MsgWarning(string.Format(msg, ImportTitle));
                        txtPath.Clear();

                        return;
                    }

                    var listOfWorksheet = importDataBll.GetWorksheets();

                    if (listOfWorksheet.Count > 0)
                    {
                        foreach (var workSheet in listOfWorksheet)
                        {
                            cmbWorksheet.Items.Add(workSheet);
                        }

                        cmbWorksheet.SelectedIndex = 0;
                    }
                }
            }
        }

        protected override void ImportData()
        {
            var msg = string.Empty;

            if (txtPath.Text.Length == 0)
            {
                MsgHelper.MsgWarning("Lokasi dan nama file Excel belum dipilih.");
                return;
            }

            IImportExportDataBll<Supplier> importDataBll = new ImportExportDataSupplierBll(txtPath.Text, _log);

            if (importDataBll.IsOpened())
            {
                msg = "Maaf file master {0} sedang dibuka, silahkan ditutup terlebih dulu.";
                MsgHelper.MsgWarning(string.Format(msg, ImportTitle));

                return;
            }

            if (!importDataBll.IsValidFormat(cmbWorksheet.Text))
            {
                msg = "Maaf format file master {0} tidak valid, proses import tidak bisa dilanjutkan.";
                MsgHelper.MsgWarning(string.Format(msg, ImportTitle));

                return;
            }

            if (MsgHelper.MsgKonfirmasi("Apakah proses ingin dilanjutkan ?"))
            {
                using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
                {
                    var rowCount = 0;
                    var result = importDataBll.Import(cmbWorksheet.Text, ref rowCount);

                    if (result)
                    {
                        msg = "Import data master {0} berhasil.";
                        MsgHelper.MsgInfo(string.Format(msg, ImportTitle));

                        Listener.Ok(this, null);
                        this.Close();
                    }
                    else
                    {
                        if (rowCount == 0)
                        {
                            msg = "Data file master {0} masih kosong.\n" +
                                  "Silahkan diisi terlebih dulu.";
                            MsgHelper.MsgInfo(string.Format(msg, ImportTitle));
                        }
                    }
                }
            }
        }
    }
}
