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
using System.IO;
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

            // baca setting pageSize
            var pageSize = AppConfigHelper.GetValue("pageSize", appConfigFile).Length > 0 ? Convert.ToInt32(AppConfigHelper.GetValue("pageSize", appConfigFile)) : 0;

            if (pageSize > 0)
                MainProgram.pageSize = pageSize;
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

            // set header nota
            IHeaderNotaBll headerNotaBll = new HeaderNotaBll();
            MainProgram.pengaturanUmum.list_of_header_nota = headerNotaBll.GetAll();

            // set label nota
            ILabelNotaBll labelNotaBll = new LabelNotaBll();
            MainProgram.pengaturanUmum.list_of_label_nota = labelNotaBll.GetAll();
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
                    switch (upgradeTo)
                    {
                        case 2: // upgrade database v1 ke v2
                            result = ExecSQL(DatabaseVersionHelper.UpgradeStrukturDatabase_v1_to_v2);
                            break;

                        case 3: // upgrade database v2 ke v3
                            result = ExecSQL(DatabaseVersionHelper.UpgradeStrukturDatabase_v2_to_v3);
                            break;

                        default:
                            break;
                    }

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
                    log4net.GlobalContext.Properties["UserName"] = txtUserName.Text;
                    MainProgram.pengguna = penggunaBll.GetByID(txtUserName.Text);

                    UpgradeDatabase(DatabaseVersionHelper.DatabaseVersion);

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
