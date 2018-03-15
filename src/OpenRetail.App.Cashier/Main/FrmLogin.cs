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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using log4net;
using ConceptCave.WaitCursor;

using OpenRetail.Helper;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.Model;

namespace OpenRetail.App.Cashier.Main
{    
    public partial class FrmLogin : Form
    {
        private ILog _log;
        private string _appConfigFile = string.Format("{0}\\OpenRetailCashier.exe.config", Utils.GetAppPath());

        public FrmLogin()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            _log = MainProgram.log;
            
            LoadAppConfig();
        }

        private void LoadAppConfig()
        {            
            txtServer.Text = AppConfigHelper.GetValue("server", _appConfigFile);

            if (Utils.IsRunningUnderIDE()) // mode debug, set user dan password default untuk development
            {
                txtUserName.Text = "admin";
                txtPassword.Text = "admin";
            }
        }

        private void SaveAppConfig()
        {            
            AppConfigHelper.SaveValue("server", txtServer.Text, _appConfigFile);
        }

        private void SetProfil()
        {
            IProfilBll profilBll = new ProfilBll(_log);
            MainProgram.profil = profilBll.GetProfil();
        }

        private void SetPengaturanUmum()
        {
            // set pengaturan lokal (setting di simpan di file app.config)            
            MainProgram.pengaturanUmum = new PengaturanUmum();
            MainProgram.pengaturanUmum.nama_printer = AppConfigHelper.GetValue("printerName", _appConfigFile);
            MainProgram.pengaturanUmum.is_auto_print = AppConfigHelper.GetValue("isAutoPrinter", _appConfigFile).ToLower() == "true" ? true : false;

            // set info printer mini pos
            var jumlahKarakter = AppConfigHelper.GetValue("jumlahKarakter", _appConfigFile).Length > 0 ? Convert.ToInt32(AppConfigHelper.GetValue("jumlahKarakter", _appConfigFile)) : 40;
            var jumlahGulung = AppConfigHelper.GetValue("jumlahGulung", _appConfigFile).Length > 0 ? Convert.ToInt32(AppConfigHelper.GetValue("jumlahGulung", _appConfigFile)) : 5;
            var ukuranFont = AppConfigHelper.GetValue("ukuranFont", _appConfigFile).Length > 0 ? Convert.ToInt32(AppConfigHelper.GetValue("ukuranFont", _appConfigFile)) : 0;

            MainProgram.pengaturanUmum.jenis_printer = AppConfigHelper.GetValue("jenis_printer", _appConfigFile).Length > 0 ? (JenisPrinter)Convert.ToInt32(AppConfigHelper.GetValue("jenis_printer", _appConfigFile)) : JenisPrinter.MiniPOS;
            MainProgram.pengaturanUmum.jumlah_karakter = jumlahKarakter;
            MainProgram.pengaturanUmum.jumlah_gulung = jumlahGulung;
            MainProgram.pengaturanUmum.ukuran_font = ukuranFont;

            // set pengaturan global (setting disimpan di database)
            ISettingAplikasiBll settingAplikasiBll = new SettingAplikasiBll();
            var settingAplikasi = settingAplikasiBll.GetAll().SingleOrDefault();

            if (settingAplikasi != null)
            {
                MainProgram.pengaturanUmum.is_stok_produk_boleh_minus = settingAplikasi.is_stok_produk_boleh_minus;
                MainProgram.pengaturanUmum.is_fokus_input_kolom_jumlah = settingAplikasi.is_fokus_input_kolom_jumlah;
            }

            // set header nota
            IHeaderNotaBll headerNotaBll = new HeaderNotaBll();
            MainProgram.pengaturanUmum.list_of_header_nota = headerNotaBll.GetAll();

            // set header nota minipos
            IHeaderNotaMiniPosBll headerNotaMiniPosBll = new HeaderNotaMiniPosBll();
            MainProgram.pengaturanUmum.list_of_header_nota_mini_pos = headerNotaMiniPosBll.GetAll();

            // set footer nota minipos
            IFooterNotaMiniPosBll footerNotaMiniPosBll = new FooterNotaMiniPosBll();
            MainProgram.pengaturanUmum.list_of_footer_nota_mini_pos = footerNotaMiniPosBll.GetAll();

            // set label nota
            ILabelNotaBll labelNotaBll = new LabelNotaBll();
            MainProgram.pengaturanUmum.list_of_label_nota = labelNotaBll.GetAll();
        }

        /// <summary>
        /// Load data kartu untuk pembayaran via kartu
        /// </summary>
        private void LoadKartu()
        {
            IKartuBll bll = new KartuBll(_log);
            MainProgram.listOfKartu = bll.GetAll();
        }

        private void SimpanSaldoAwal(string penggunaId, int saldoAwal)
        {
            IMesinKasirBll mesinBll = new MesinKasirBll(_log);

            var obj = new MesinKasir
            {
                pengguna_id = penggunaId,
                saldo_awal = saldoAwal
            };

            var result = mesinBll.Save(obj);
            if (result > 0)
                MainProgram.mesinId = obj.mesin_id;
        }

        private bool ExecSQL(string fileName)
        {
            var result = false;
            var fileSql = string.Format(@"{0}\sql\{1}", Utils.GetAppPath(), fileName);

            if (File.Exists(fileSql))
            {
                IDbConnectionHelper dbHelper = new DbConnectionHelper();

                using (var reader = new StreamReader(fileSql))
                {
                    var sql = reader.ReadToEnd();
                    result = dbHelper.ExecSQL(sql);
                }
            }

            return result;
        }

        private void UpgradeDatabase(int newDatabaseVersion)
        {
            IDatabaseVersionBll bll = new DatabaseVersionBll(_log);
            
            var dbVersion = bll.Get();
            if (dbVersion != null)
            {
                var result = true;
                var upgradeTo = dbVersion.version_number + 1;
                
                while (upgradeTo <= newDatabaseVersion)
                {
                    var scriptUpgrade = DatabaseVersionHelper.ListOfUpgradeDatabaseScript[upgradeTo];
                    result = ExecSQL(scriptUpgrade);

                    if (!result)
                        break;

                    upgradeTo++;
                    if (!(bll.UpdateVersion() > 0))
                        break;
                }
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var isConnected = false;

            SaveAppConfig();

            // tes koneksi ke server
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                IDbConnectionHelper dbConn = new DbConnectionHelper();
                isConnected = dbConn.IsOpenConnection();
            }

            if (!isConnected)
            {
                var msg = "Maaf koneksi ke database gagal !!!\n\n" +
                          "Disarankan untuk menginstall OpenRetail di 'Drive D'.\n" +
                          "Silahkan uninstall dulu OpenRetailnya, kemudian install lagi di 'Drive D'.";

                MsgHelper.MsgError(msg);
                return;
            }

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                IPenggunaBll penggunaBll = new PenggunaBll(_log);

                var pass = CryptoHelper.GetMD5Hash(txtPassword.Text, MainProgram.securityCode);
                var isLogin = penggunaBll.IsValidPengguna(txtUserName.Text, pass);

                if (isLogin)
                {
                    UpgradeDatabase(DatabaseVersionHelper.DatabaseVersion);

                    log4net.GlobalContext.Properties["UserName"] = txtUserName.Text;
                    MainProgram.pengguna = penggunaBll.GetByID(txtUserName.Text);

                    SetProfil();
                    SetPengaturanUmum();
                    LoadKartu();

                    var saldoAwal = NumberHelper.StringToDouble(txtSaldoAwal.Text);
                    SimpanSaldoAwal(MainProgram.pengguna.pengguna_id, (int)saldoAwal);
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MsgHelper.MsgWarning("User name atau password salah !!!");
                    txtUserName.Focus();
                }
            }
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabControl_Click(object sender, EventArgs e)
        {
            switch (((TabControl)sender).SelectedIndex)
            {
                case 0:
                    txtUserName.Focus();
                    break;

                case 1:
                    txtServer.Focus();
                    break;

                default:
                    break;
            }
        }

        private void txtSaldoAwal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                btnLogin_Click(sender, e);
        }        
    }
}
