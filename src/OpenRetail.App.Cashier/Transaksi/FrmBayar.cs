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

using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Helper.UI.Template;
using OpenRetail.Helper;
using ConceptCave.WaitCursor;
using OpenRetail.Helper.RAWPrinting;
using log4net;

namespace OpenRetail.App.Cashier.Transaksi
{
    public partial class FrmBayar : FrmEntryStandard
    {
        private IJualProdukBll _bll = null; // deklarasi objek business logic layer 
        private JualProduk _jual = null;
        private IList<Kartu> _listOfKartu;

        public IListener Listener { private get; set; }

        public FrmBayar(string header, JualProduk jual, IJualProdukBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._bll = bll;
            this._jual = jual;
            this._listOfKartu = MainProgram.listOfKartu;

            AddHandler();
            LoadKartu();

            txtTotal.Text = this._jual.total_nota.ToString();
            txtGrandTotal.Text = this._jual.grand_total.ToString();

            this.ActiveControl = txtBayarTunai;
        }

        private void AddHandler()
        {
            txtDiskon.TextChanged += HitungGrandTotal;
            txtDiskon.TextChanged += HitungKembalian;

            txtPPN.TextChanged += HitungGrandTotal;
            txtPPN.TextChanged += HitungKembalian;

            txtBayarTunai.TextChanged += HitungKembalian;
            txtBayarKartu.TextChanged += HitungKembalian;
        }

        private void LoadKartu()
        {
            cmbKartu.Items.Clear();
            foreach (var kartu in _listOfKartu)
            {
                cmbKartu.Items.Add(kartu.nama_kartu);
            }

            if (_listOfKartu.Count > 0)
                cmbKartu.SelectedIndex = 0;
        }

        private void HitungGrandTotal(object sender, EventArgs e)
        {
            _jual.diskon = NumberHelper.StringToNumber(txtDiskon.Text);
            _jual.ppn = NumberHelper.StringToFloat(txtPPN.Text);

            txtGrandTotal.Text = NumberHelper.NumberToString(_jual.grand_total);
        }

        private void HitungKembalian(object sender, EventArgs e)
        {            
            var bayarTunai = NumberHelper.StringToNumber(txtBayarTunai.Text);
            var bayarKartu = NumberHelper.StringToNumber(txtBayarKartu.Text);

            var kembalian = (bayarTunai + bayarKartu) - _jual.grand_total;

            txtKembalian.Text = "0";            
            if (kembalian > 0)
            {
                txtKembalian.Text = kembalian.ToString();
            }                
        }

        protected override void Simpan()
        {
            var msg = "'{0}' tidak boleh kosong !";
            var bayarTunai = 0;
            var bayarKartu = 0;

            if (chkBayarViaKartu.Checked) // pembayaran via kartu
            {
                bayarKartu = (int)NumberHelper.StringToNumber(txtBayarKartu.Text);
                if (!(bayarKartu > 0))
                {
                    MsgHelper.MsgWarning(string.Format(msg, "Bayar via Kartu"));
                    txtBayarKartu.Focus();
                    return;
                }
            }

            // pembayaran tunai
            bayarTunai = (int)NumberHelper.StringToNumber(txtBayarTunai.Text);

            if (bayarTunai == 0 && bayarKartu == 0)
            {
                MsgHelper.MsgWarning(string.Format(msg, "Bayar Tunai"));
                txtBayarTunai.Focus();
                return;
            }

            _jual.bayar_tunai = bayarTunai;
            _jual.bayar_kartu = bayarKartu;            

            if ((_jual.jumlah_bayar - _jual.grand_total) < 0)
            {
                MsgHelper.MsgWarning("Maaf jumlah bayar kurang");

                if (bayarTunai > 0)
                {
                    txtBayarTunai.Focus();
                    txtBayarTunai.SelectAll();
                    return;
                }

                if (bayarKartu > 0)
                {
                    txtBayarKartu.Focus();
                    txtBayarKartu.SelectAll();
                    return;
                }
            }

            if (!MsgHelper.MsgKonfirmasi("Apakah proses penyimpanan ingin di lanjutkan ?"))
                return;

            _jual.mesin_id = MainProgram.mesinId;
            _jual.ppn = NumberHelper.StringToDouble(txtPPN.Text);
            _jual.diskon = NumberHelper.StringToDouble(txtDiskon.Text);

            if (_jual.bayar_kartu > 0)
            {
                var kartu = _listOfKartu[cmbKartu.SelectedIndex];

                _jual.kartu_id = kartu.kartu_id;
                _jual.Kartu = kartu;
                _jual.nomor_kartu = txtNoKartu.Text;
            }

            var result = 0;
            var validationError = new ValidationError();

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                result = _bll.Save(_jual, ref validationError);

                if (result > 0)
                {
                    Listener.Ok(this, _jual);
                    this.Close();
                }
                else
                {
                    if (validationError.Message != null && validationError.Message.Length > 0)
                    {
                        MsgHelper.MsgWarning(validationError.Message);
                        base.SetFocusObject(validationError.PropertyName, this);
                    }
                    else
                        MsgHelper.MsgUpdateError();
                }
            }
        }

        private void txtBayarTunai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                KeyPressHelper.NextFocus();
                KeyPressHelper.NextFocus();
            }                
        }

        private void chkBayarViaKartu_CheckedChanged(object sender, EventArgs e)
        {
            var isChecked = ((CheckBox)sender).Checked;

            cmbKartu.Enabled = isChecked;
            txtBayarKartu.Enabled = isChecked;
            txtNoKartu.Enabled = isChecked;

            if (!isChecked)
                txtBayarKartu.Text = "0";
        }        
    }
}
