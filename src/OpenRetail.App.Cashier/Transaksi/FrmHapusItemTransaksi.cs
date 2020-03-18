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

using OpenRetail.Helper;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Model;
using System.Windows.Forms;

namespace OpenRetail.App.Cashier.Transaksi
{
    public partial class FrmHapusItemTransaksi : FrmEntryStandard
    {
        private JualProduk _jual = null;

        public IListener Listener { private get; set; }

        public FrmHapusItemTransaksi(string header, JualProduk jual)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._jual = jual;
        }

        protected override void Simpan()
        {
            var noTransaksi = NumberHelper.StringToNumber(txtNomorTransaksi.Text);

            if (!(noTransaksi > 0))
            {
                MsgHelper.MsgWarning("'Nomor Transaksi' tidak boleh kosong !");
                txtNomorTransaksi.Focus();
                txtNomorTransaksi.SelectAll();
                return;
            }

            if (noTransaksi > _jual.item_jual.Count)
            {
                MsgHelper.MsgWarning(string.Format("'Nomor Transaksi' tidak valid !\nNomor Transaksi terakhir adalah {0}", _jual.item_jual.Count));
                txtNomorTransaksi.Focus();
                txtNomorTransaksi.SelectAll();
                return;
            }

            if (!MsgHelper.MsgKonfirmasi("Apakah proses penyimpanan ingin di lanjutkan ?"))
                return;

            Listener.Ok(this, new { noTransaksi });
            this.Close();
        }

        private void txtNomorTransaksi_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }
    }
}