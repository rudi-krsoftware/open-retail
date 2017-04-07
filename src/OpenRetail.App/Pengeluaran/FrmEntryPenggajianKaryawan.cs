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
using OpenRetail.Bll.Api;
using OpenRetail.App.UI.Template;
using OpenRetail.App.Helper;

namespace OpenRetail.App.Pengeluaran
{
    public partial class FrmEntryPenggajianKaryawan : FrmEntryStandard
    {                    
        private IGajiKaryawanBll _bll = null; // deklarasi objek business logic layer 
        private GajiKaryawan _gaji = null;
        private Karyawan _karyawan;
        private IList<Karyawan> _listOfKaryawan;
        
        private bool _isNewData = false;
        private Pengguna _pengguna;

        public IListener Listener { private get; set; }

        public FrmEntryPenggajianKaryawan(string header, string bulan, string tahun, IList<Karyawan> listOfKaryawan, IGajiKaryawanBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._listOfKaryawan = listOfKaryawan;
            this._bll = bll;
            this._pengguna = MainProgram.pengguna;

            this._isNewData = true;
            txtNota.Text = this._bll.GetLastNota();

            AddHandlerTotal();
            SetBulanTahun(bulan, tahun);
            LoadDataKaryawan();
        }

        public FrmEntryPenggajianKaryawan(string header, string bulan, string tahun, GajiKaryawan gaji, IList<Karyawan> listOfKaryawan, IGajiKaryawanBll bll)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            base.SetButtonSelesaiToBatal();
            this._listOfKaryawan = listOfKaryawan;
            this._bll = bll;
            this._pengguna = MainProgram.pengguna;
            this._gaji = gaji;

            AddHandlerTotal();
            SetBulanTahun(bulan, tahun);
            LoadDataKaryawan();

            cmbKaryawan.SelectedItem = this._gaji.Karyawan.nama_karyawan;
            cmbKaryawan.Enabled = false;

            txtNota.Text = this._gaji.nota;
            dtpTanggal.Value = (DateTime)this._gaji.tanggal;            

            AddHandlerTotal();

            var jenisGajian = this._gaji.Karyawan.jenis_gajian;

            txtKehadiran.Text = this._gaji.kehadiran.ToString();
            txtAbsen.Text = this._gaji.absen.ToString();
            txtJumlahHari.Text = this._gaji.jumlah_hari.ToString();
            txtGaji.Text = this._gaji.gaji_pokok.ToString();
            txtTunjangan.Text = this._gaji.tunjangan.ToString();
            txtJam.Text = this._gaji.jam.ToString();
            txtLembur.Text = this._gaji.lembur.ToString();
            txtBonus.Text = this._gaji.bonus.ToString();
            txtPotongan.Text = this._gaji.potongan.ToString();
        }

        private void AddHandlerTotal()
        {
            txtJumlahHari.TextChanged += RefreshTotal;
            txtGaji.TextChanged += RefreshTotal;
            txtTunjangan.TextChanged += RefreshTotal;
            txtJam.TextChanged += RefreshTotal;
            txtLembur.TextChanged += RefreshTotal;
            txtBonus.TextChanged += RefreshTotal;
            txtPotongan.TextChanged += RefreshTotal;
        }

        private void RefreshTotal(object sender, EventArgs e)
        {
            if (_karyawan == null)
                return;

            JenisGajian jenisGaji = _karyawan.jenis_gajian;

            double tunjangan = NumberHelper.StringToDouble(txtTunjangan.Text);
            double bonus = NumberHelper.StringToDouble(txtBonus.Text);
            double potongan = NumberHelper.StringToDouble(txtPotongan.Text);

            double gaji = jenisGaji == JenisGajian.Bulanan ? NumberHelper.StringToDouble(txtGaji.Text) : NumberHelper.StringToDouble(txtJumlahHari.Text) * NumberHelper.StringToDouble(txtGaji.Text);
            double lembur = jenisGaji == JenisGajian.Bulanan ? NumberHelper.StringToDouble(txtLembur.Text) : NumberHelper.StringToDouble(txtJam.Text) * NumberHelper.StringToDouble(txtLembur.Text);

            txtTotal.Text = NumberHelper.NumberToString(gaji + lembur + tunjangan + bonus - potongan);
        }

        private void SetBulanTahun(string bulan, string tahun)
        {
            cmbBulan.Items.Add(bulan);
            cmbBulan.SelectedItem = bulan;

            cmbTahun.Items.Add(tahun);
            cmbTahun.SelectedItem = tahun;
        }

        private void LoadDataKaryawan()
        {
            cmbKaryawan.Items.Add("--- Pilih karyawan ---");

            FillDataHelper.FillKaryawan(cmbKaryawan, _listOfKaryawan, false);
            cmbKaryawan.SelectedIndex = 0;
        }

        protected override void Simpan()
        {
            if (_isNewData)
            {
                _gaji = new GajiKaryawan();
            
                if (this._karyawan == null)
                {
                    MsgHelper.MsgWarning("Karyawan belum dipilih");
                    return;
                }

                _gaji.karyawan_id = _karyawan.karyawan_id;
                _gaji.Karyawan = _karyawan;

                _gaji.bulan = DayMonthHelper.GetBulanAngka(cmbBulan.Text);
                _gaji.tahun = int.Parse(cmbTahun.Text);
            }

            _gaji.pengguna_id = this._pengguna.pengguna_id;
            _gaji.Pengguna = this._pengguna;
            _gaji.nota = txtNota.Text;            

            _gaji.tanggal = dtpTanggal.Value;
            _gaji.kehadiran = int.Parse(txtKehadiran.Text);
            _gaji.absen = int.Parse(txtAbsen.Text);

            _gaji.jumlah_hari = int.Parse(txtJumlahHari.Text);
            _gaji.gaji_pokok = NumberHelper.StringToDouble(txtGaji.Text);
            _gaji.tunjangan = NumberHelper.StringToDouble(txtTunjangan.Text);
            _gaji.bonus = NumberHelper.StringToDouble(txtBonus.Text);
            _gaji.jam = int.Parse(txtJam.Text);
            _gaji.lembur = NumberHelper.StringToDouble(txtLembur.Text);
            _gaji.potongan = NumberHelper.StringToDouble(txtPotongan.Text);

            var result = 0;
            var validationError = new ValidationError();

            if (_isNewData)
                result = _bll.Save(_gaji, ref validationError);
            else
                result = _bll.Update(_gaji, ref validationError);

            if (result > 0) 
            {
                Listener.Ok(this, _isNewData, _gaji);

                if (_isNewData)
                {
                    cmbKaryawan.SelectedIndex = 0;
                    cmbKaryawan.Focus();

                    txtNota.Text = _bll.GetLastNota();
                }
                else
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
                {
                    var pesan = string.Format("Maaf, Data yang Anda masukkan gagal disimpan !\nCek apakah data gaji '{0}' sudah diinputkan.", _gaji.Karyawan.nama_karyawan);
                    MsgHelper.MsgWarning(pesan);
                }
            }                
        }

        private void cmbKaryawan_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = cmbKaryawan.SelectedIndex;

            if (index == 0)
            {
                base.ResetForm(this);
                this._karyawan = null;
                return;
            }                

            txtJabatan.Clear();

            this._karyawan = _listOfKaryawan[index - 1];
            var jabatan = this._karyawan.Jabatan;

            if (jabatan != null)
                txtJabatan.Text = jabatan.nama_jabatan;

            if (this._karyawan != null)
            {
                switch (_karyawan.jenis_gajian)
                {
                    case JenisGajian.Mingguan:
                        txtJumlahHari.Visible = true;
                        label9.Visible = true;
                        txtJam.Visible = true;
                        label10.Visible = true;
                        break;

                    case JenisGajian.Bulanan:
                        txtJumlahHari.Visible = false;
                        label9.Visible = false;
                        txtJam.Visible = false;
                        label10.Visible = false;
                        break;

                    default:
                        break;
                }

                txtGaji.Text = this._karyawan.gaji_pokok.ToString();
                txtLembur.Text = this._karyawan.gaji_lembur.ToString();
            }
        }

        private void txtPotongan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                Simpan();
        }
    }
}
