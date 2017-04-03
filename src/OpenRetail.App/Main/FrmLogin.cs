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
using OpenRetail.App.Helper;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using OpenRetail.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenRetail.App.Main
{
    public partial class FrmLogin : Form
    {
        private ILog _log;

        public FrmLogin()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            _log = MainProgram.log;
            
            LoadAppConfig();
        }

        private void LoadAppConfig()
        {
            var appConfigFile = string.Format("{0}\\OpenRetail.exe.config", Utils.GetAppPath());
            txtServer.Text = AppConfigHelper.GetValue("server", appConfigFile);

            if (Utils.IsRunningUnderIDE()) // mode debug, set user dan password default untuk development
            {
                txtUserName.Text = "admin";
                txtPassword.Text = "admin";
            }
        }

        private void SaveAppConfig()
        {
            var appConfigFile = string.Format("{0}\\OpenRetail.exe.config", Utils.GetAppPath());
            AppConfigHelper.SaveValue("server", txtServer.Text, appConfigFile);
        }

        private void SetProfil()
        {
            IProfilBll profilBll = new ProfilBll(_log);
            MainProgram.profil = profilBll.GetProfil();
        }

        private void SetPengaturanUmum()
        {
            var appConfigFile = string.Format("{0}\\OpenRetail.exe.config", Utils.GetAppPath());

            MainProgram.pengaturanUmum = new PengaturanUmum();
            MainProgram.pengaturanUmum.nama_printer = AppConfigHelper.GetValue("printerName", appConfigFile);
            MainProgram.pengaturanUmum.is_auto_print = AppConfigHelper.GetValue("isAutoPrinter", appConfigFile).ToLower() == "true" ? true : false;
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
                var msg = "Maaf koneksi ke server database gagal !!!\n" +
                          "Silahkan cek koneksi jaringan Anda.";
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
                    log4net.GlobalContext.Properties["UserName"] = txtUserName.Text;
                    MainProgram.pengguna = penggunaBll.GetByID(txtUserName.Text);

                    SetProfil();
                    SetPengaturanUmum();

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

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
                btnLogin_Click(sender, e);
        }        
    }
}
