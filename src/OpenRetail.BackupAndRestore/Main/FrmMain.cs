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
using System.Threading;
using System.Threading.Tasks;

using Dapper;
using ConceptCave.WaitCursor;
using OpenRetail.BackupAndRestore.Context;
using OpenRetail.Helper;
using System.IO;
using System.Diagnostics;

namespace OpenRetail.BackupAndRestore.Main
{
    public partial class FrmMain : Form
    {
        private event EventHandler ResultChanged;

        private string _result = "";        
        private string _appConfigFile = string.Format("{0}\\OpenRetail.BackupAndRestore.exe.config", Utils.GetAppPath());

        private string _port = "5435";
        private string _dbName = "DbOpenRetail";        
        private string _pgPassword = "masterkey";

        private string Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                if (ResultChanged != null) ResultChanged(value, null);
            }
        }

        public FrmMain()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);
            this.ShowIcon = true;
            this.ShowInTaskbar = true;

            LoadAppConfig();
            SetInfoBackupAndRestore();
            this.ResultChanged += FrmMain_ResultChanged;
        }

        private void FrmMain_ResultChanged(object sender, EventArgs e)
        {
            txtLog.Text += sender.ToString();
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
            txtLog.Refresh();
        }

        private void LoadAppConfig()
        {
            txtServer.Text = AppConfigHelper.GetValue("server", _appConfigFile);

            _port = AppConfigHelper.GetValue("port", _appConfigFile);
            _dbName = AppConfigHelper.GetValue("dbName", _appConfigFile);
            txtLokasiPenyimpananFileBackup.Text = AppConfigHelper.GetValue("lokasiPenyimpananFileBackup", _appConfigFile);
        }

        private void SaveAppConfig()
        {
            AppConfigHelper.SaveValue("server", txtServer.Text, _appConfigFile);
            AppConfigHelper.SaveValue("lokasiPenyimpananFileBackup", txtLokasiPenyimpananFileBackup.Text, _appConfigFile);
        }

        private void SetInfoBackupAndRestore()
        {
            lblInfoBackup.Text = "Backup database berfungsi untuk membuat duplikasi database dalam bentuk file.\n" +
                                 "Lakukan backup database secara berkala, untuk menghindari apabila terjadi kerusakan\n" +
                                 "database yang disebabkan oleh kerusakan hardisk, virus, dll.\n\n" +
                                 "Saran:\n" +
                                 "1. Backuplah database Anda secara berkala, misal setiap mau tutup toko.\n" +
                                 "2. Simpanlah file backup pada folder khusus, misal di drive D, E atau flashdisk. Jangan di drive sistem.";

            lblInfoRestore.Text = "Restore database berfungsi untuk mengembalikan database ke kondisi sesuai file backup yang dipilih.\n" +
                                  "Lakukan restore database apabila terjadi kerusakan pada pc/server yang digunakan sebagai tempat penyimpanan database.\n\n" +
                                  "Saran:\n" +
                                  "1. Sebelum proses restore, tutup semua program OpenRetail (server dan kasir) baik yang ada di jaringan\n" +
                                  "    atau komputer lokal.\n" +
                                  "2. Jangan melakukan restore database jika tidak terjadi masalah dengan database Anda.\n" +
                                  "3. Pilihlah file backup yang benar.";


            var dt = DateTime.Now;
            txtNamaFileBackup.Text = string.Format("DbOpenRetail_{0}_{1}_{2}_{3}{4}{5}.backup", dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }

        private void btnProsesBackup_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLokasiPenyimpananFileBackup.Text))
            {
                MsgHelper.MsgWarning("Lokasi penyimpanan file backup belum dipilih !");
                txtLokasiPenyimpananFileBackup.Focus();
                return;
            }

            if (!Directory.Exists(txtLokasiPenyimpananFileBackup.Text))
                Directory.CreateDirectory(txtLokasiPenyimpananFileBackup.Text   );

            if (!MsgHelper.MsgKonfirmasi("Apakah proses backup database ingin dilanjutkan ?"))
                return;

            txtLog.Clear();
            SaveAppConfig();

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                Result = "Sedang melakukan koneksi ke database ..." + Environment.NewLine;

                if (!IsOpenConnection())
                {
                    var msg = "Maaf koneksi ke database gagal !!!\n" +
                              "Proses backup database tidak bisa dilanjutkan.";

                    MsgHelper.MsgWarning(msg);
                    return;
                }
                Result = "Koneksi ke database berhasil" + Environment.NewLine + Environment.NewLine;

                Result = "Sedang membuat file backup ..." + Environment.NewLine + Environment.NewLine;
                var fileBackup = string.Format("{0}\\{1}", txtLokasiPenyimpananFileBackup.Text, txtNamaFileBackup.Text);

                var cmd = "-U postgres -h " + txtServer.Text + " -p " + _port + " -i -F c -b -v -f \"" + fileBackup + "\" " + _dbName;
                ExecuteCommand("pg_dump", cmd);
                
                OpenFolder(fileBackup, txtLokasiPenyimpananFileBackup.Text);
            }
        }

        private void btnProsesRestore_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLokasiFileBackup.Text))
            {
                MsgHelper.MsgWarning("File backup belum dipilih !");
                txtLokasiFileBackup.Focus();
                return;
            }

            if (!MsgHelper.MsgKonfirmasi("Apakah proses restore database ingin dilanjutkan ?"))
                return;

            txtLog.Clear();
            SaveAppConfig();

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                Result = "Sedang melakukan koneksi ke database ..." + Environment.NewLine;

                if (!IsOpenConnection())
                {
                    var msg = "Maaf koneksi ke database gagal !!!\n" +
                              "Proses restore database tidak bisa dilanjutkan.";

                    MsgHelper.MsgWarning(msg);
                    return;
                }

                Result = "Koneksi ke database berhasil" + Environment.NewLine + Environment.NewLine;

                if (!ReplaceDatabase(_dbName))
                {
                    var msg = "Gagal menghapus database lama !!!\n" +
                              "Proses restore database gagal.";

                    MsgHelper.MsgError(msg);
                    return;
                }

                Result = "Sedang merestore database ..." + Environment.NewLine + Environment.NewLine;

                var cmd = "-U postgres -h " + txtServer.Text + " -p " + _port + " -i -v -d " + _dbName + " \"" + txtLokasiFileBackup.Text + "\"";
                ExecuteCommand("pg_restore", cmd);                
            }
        }                

        private void OpenFolder(string fileBackup, string lokasiFileBackup)
        {
            var fi = new FileInfo(fileBackup);
            if (fi.Exists)
                Process.Start(lokasiFileBackup);
        }

        private void ExecuteCommand(string cmd, string parameter)
        {
            try
            {
                _result = "";                

                var info = new System.Diagnostics.ProcessStartInfo();
                info.FileName = "pgsql\\" + cmd + ".exe ";
                info.Arguments = parameter;
                info.CreateNoWindow = true;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;
                info.UseShellExecute = false;

                try 
	            {	        
                    info.EnvironmentVariables.Add("PGPASSWORD", _pgPassword); 
	            }
	            catch
	            {
	            }

                var proc = new System.Diagnostics.Process();
                proc.StartInfo = info;
                proc.Start();

                var cTokenSource = new CancellationTokenSource();
                var cToken = cTokenSource.Token;

                Result = Task.Factory.StartNew(() => proc.StandardError.ReadToEnd(), cToken).Result;
                proc.WaitForExit();

                if (proc.ExitCode == 0)
                    Result += Environment.NewLine + ((cmd == "pg_dump") ? "Backup database berhasil." : "Restore database berhasil.");
                else
                    Result += Environment.NewLine + "Error Occured";

            }
            catch (Exception ex)
            {
                MsgHelper.MsgError(ex.Message);
            }
        }                

        private bool IsOpenConnection()
        {
            var result = false;

            using (IDapperContext context = new DapperContext(_pgPassword))
            {
                result = context.IsOpenConnection();
            }

            return result;
        }

        private bool ReplaceDatabase(string dbName)
        {
            var result = false;

            try
            {
                using (IDapperContext context = new DapperContext(_pgPassword))
                {
                    Result = "Menonaktifkan semua koneksi" + Environment.NewLine;

                    var sql = @"SELECT pg_terminate_backend(pid) 
                                FROM pg_stat_activity WHERE datname = @dbName";
                    context.db.Execute(sql, new { dbName });

                    Result = "Menghapus database lama" + Environment.NewLine;

                    sql = "DROP DATABASE \"" + dbName + "\"";
                    context.db.Execute(sql);

                    Result = "Membuat database baru" + Environment.NewLine + Environment.NewLine;

                    sql = "CREATE DATABASE \"" + dbName + "\"";
                    context.db.Execute(sql);

                    result = true;
                }
            }
            catch
            {
            }

            return result;
        }

        private void btnLokasiPenyimpananFileBackup_Click(object sender, EventArgs e)
        {
            using (var dlgOpen = new FolderBrowserDialog())
            {
                var result = dlgOpen.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dlgOpen.SelectedPath))
                {
                    txtLokasiPenyimpananFileBackup.Text = dlgOpen.SelectedPath;
                }
            }
        }

        private void btnLokasiFileBackup_Click(object sender, EventArgs e)
        {
            using (var dlgOpen = new OpenFileDialog())
            {
                dlgOpen.Filter = "OpenRetail Database Backup (*.backup)|*.backup";
                dlgOpen.Title = "OpenRetail Database Backup";

                var result = dlgOpen.ShowDialog();
                if (result == DialogResult.OK)
                {
                    txtLokasiFileBackup.Text = dlgOpen.FileName;
                }
            }
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEsc(e))
                this.Close();
        }        
    }
}
