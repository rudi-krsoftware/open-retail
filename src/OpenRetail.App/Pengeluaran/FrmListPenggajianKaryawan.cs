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
using OpenRetail.Bll.Service;
using OpenRetail.App.UI.Template;
using OpenRetail.App.Helper;
using Syncfusion.Windows.Forms.Grid;
using ConceptCave.WaitCursor;
using log4net;

namespace OpenRetail.App.Pengeluaran
{
    public partial class FrmListPenggajianKaryawan : FrmListEmptyBody, IListener
    {                
        private IGajiKaryawanBll _bll; // deklarasi objek business logic layer 
        private IList<GajiKaryawan> _listOfGaji = new List<GajiKaryawan>();
        private IList<Karyawan> _listOfKaryawan = new List<Karyawan>();
        private ILog _log;
        private Pengguna _pengguna;
        private string _menuId;
        
        public FrmListPenggajianKaryawan(string header, Pengguna pengguna, string menuId)
            : base()
        {
            InitializeComponent();

            base.SetHeader(header);
            base.WindowState = FormWindowState.Maximized;

            _log = MainProgram.log;
            _bll = new GajiKaryawanBll(_log);
            _pengguna = pengguna;
            _menuId = menuId;

            // set hak akses untuk SELECT
            var role = _pengguna.GetRoleByMenuAndGrant(_menuId, GrantState.SELECT);
            if (role != null)
            {
                if (role.is_grant)
                {
                    LoadBulanDanTahun();

                    var bulan = cmbBulan.SelectedIndex + 1;
                    var tahun = int.Parse(cmbTahun.Text);

                    LoadData(bulan, tahun);
                    LoadDataKaryawan();
                }
            }            

            InitGridList();

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfGaji.Count);
        }

        private void LoadDataKaryawan()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                IKaryawanBll bll = new KaryawanBll(_log);
                _listOfKaryawan = bll.GetAll();
            }
        }

        private void LoadBulanDanTahun()
        {
            FillDataHelper.FillBulan(cmbBulan, true);
            FillDataHelper.FillTahun(cmbTahun, true);
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tanggal", Width = 80 });
            gridListProperties.Add(new GridListControlProperties { Header = "Nama", Width = 200 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jabatan", Width = 200 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kehadiran", Width = 70 });
            gridListProperties.Add(new GridListControlProperties { Header = "Absen", Width = 70 });
            gridListProperties.Add(new GridListControlProperties { Header = "Gaji", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tunjangan", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Bonus", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Lembur", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Potongan", Width = 100 });
            gridListProperties.Add(new GridListControlProperties { Header = "Total" });

            GridListControlHelper.InitializeGridListControl<GajiKaryawan>(this.gridList, _listOfGaji, gridListProperties);

            if (_listOfGaji.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {

                if (_listOfGaji.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfGaji.Count)
                        {
                            var gaji = _listOfGaji[rowIndex];                            

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = DateTimeHelper.DateToString(gaji.tanggal);
                                    break;

                                case 3:
                                    var karyawan = gaji.Karyawan;
                                    if (karyawan != null)
                                        e.Style.CellValue = karyawan.nama_karyawan;

                                    break;

                                case 4:
                                    var jabatan = gaji.Karyawan.Jabatan;

                                    if (jabatan != null)
                                        e.Style.CellValue = jabatan.nama_jabatan;

                                    break;

                                case 5:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = gaji.kehadiran;
                                    break;

                                case 6:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Center;
                                    e.Style.CellValue = gaji.absen;
                                    break;

                                case 7:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(gaji.gaji_akhir);

                                    break;

                                case 8:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(gaji.tunjangan);

                                    break;

                                case 9:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(gaji.bonus);

                                    break;

                                case 10:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(gaji.lembur_akhir);

                                    break;

                                case 11:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(gaji.potongan);

                                    break;

                                case 12:
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(gaji.total_gaji);

                                    break;

                                default:
                                    break;
                            }

                            // we handled it, let the grid know
                            e.Handled = true;
                        }
                    }
                }
            };
        }

        private void LoadData(int bulan, int tahun)
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfGaji = _bll.GetByBulanAndTahun(bulan, tahun);
                GridListControlHelper.Refresh<GajiKaryawan>(this.gridList, _listOfGaji);
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfGaji.Count > 0);

            // set hak akses selain SELECT (TAMBAH, PERBAIKI dan HAPUS)
            RolePrivilegeHelper.SetHakAkses(this, _pengguna, _menuId, _listOfGaji.Count);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryPenggajianKaryawan("Tambah Data " + this.Text, cmbBulan.Text, cmbTahun.Text, _listOfKaryawan, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.TabText))
                return;

            var gaji = _listOfGaji[index];

            LogicalThreadContext.Properties["OldValue"] = gaji.ToJson();

            var frm = new FrmEntryPenggajianKaryawan("Edit Data " + this.Text, cmbBulan.Text, cmbTahun.Text, gaji, _listOfKaryawan, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Hapus()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.Text))
                return;

            if (MsgHelper.MsgDelete())
            {
                var pengeluaran = _listOfGaji[index];

                var result = _bll.Delete(pengeluaran);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<GajiKaryawan>(this.gridList, _listOfGaji, pengeluaran);
                    ResetButton();
                }
                else
                    MsgHelper.MsgDeleteError();
            }
        }

        public void Ok(object sender, object data)
        {
            throw new NotImplementedException();
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            var gaji = (GajiKaryawan)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<GajiKaryawan>(this.gridList, _listOfGaji, gaji);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<GajiKaryawan>(this.gridList, _listOfGaji, gaji);
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (btnPerbaiki.Enabled)
                Perbaiki();
        }

        private void btnTampilkan_Click(object sender, EventArgs e)
        {
            var bulan = cmbBulan.SelectedIndex + 1;
            var tahun = int.Parse(cmbTahun.Text);

            LoadData(bulan, tahun);
        }
    }
}
