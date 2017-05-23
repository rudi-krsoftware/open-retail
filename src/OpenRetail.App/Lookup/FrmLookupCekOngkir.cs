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

using Syncfusion.Windows.Forms.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenRetail.App.Helper;
using OpenRetail.App.UI.Template;
using OpenRetail.Model;
using OpenRetail.Model.RajaOngkir;
using OpenRetail.Bll.Api;
using OpenRetail.Bll.Service;
using RestSharp;
using Newtonsoft.Json;
using ConceptCave.WaitCursor;
using OpenRetail.App.UserControl;

namespace OpenRetail.App.Lookup
{
    public partial class FrmLookupCekOngkir : FrmLookupEmptyBody, IListener
    {
        private IList<costs> _listOfCost = new List<costs>();
        private IList<Kabupaten> _listOfkabupaten = new List<Kabupaten>();
        private KabupatenAsal _kabupatenAsal = null;
        private KabupatenTujuan _kabupatenTujuan = null;

        public IListener Listener { private get; set; }

        public FrmLookupCekOngkir(string header)
            : base()
        {
            InitializeComponent();
            ColorManagerHelper.SetTheme(this, this);

            base.SetHeader(header);
            this._listOfkabupaten = MainProgram.ListOfKabupaten;

            InitGridList();
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Kurir", Width = 70 });
            gridListProperties.Add(new GridListControlProperties { Header = "Jenis Layanan", Width = 330 });
            gridListProperties.Add(new GridListControlProperties { Header = "Tarif" });

            GridListControlHelper.InitializeGridListControl<costs>(this.gridList, _listOfCost, gridListProperties);

            if (_listOfCost.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                if (_listOfCost.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfCost.Count)
                        {
                            var cost = _listOfCost[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.CellValue = cost.kurir_code;
                                    break;

                                case 3:
                                    e.Style.CellValue = string.Format("{0} ({1})", cost.service, cost.description);
                                    break;

                                case 4:
                                    var costDetail = cost.cost[0];
                                    e.Style.HorizontalAlignment = GridHorizontalAlignment.Right;
                                    e.Style.CellValue = NumberHelper.NumberToString(costDetail.value);
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

        protected override void Pilih()
        {
            var rowIndex = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(rowIndex, this.Text))
                return;

            var ongkir = _listOfCost[rowIndex];
            this.Listener.Ok(this, ongkir);

            this.Close();
        }

        private void gridList_DoubleClick(object sender, EventArgs e)
        {
            if (base.IsButtonPilihEnabled)
                Pilih();
        }

        private void gridList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                if (base.IsButtonPilihEnabled)
                    Pilih();
            }
        }

        private void btnCekOngkir_Click(object sender, EventArgs e)
        {
            if (this._kabupatenAsal == null || txtKabupatenAsal.Text.Length == 0)
            {
                MsgHelper.MsgWarning("'Kota/Kabupaten Asal' tidak boleh kosong !");
                txtKabupatenAsal.Focus();

                return;
            }

            if (this._kabupatenTujuan == null || txtKabupatenTujuan.Text.Length == 0)
            {
                MsgHelper.MsgWarning("'Kota/Kabupaten Tujuan' tidak boleh kosong !");
                txtKabupatenTujuan.Focus();

                return;
            }

            var berat = (int)NumberHelper.StringToDouble(txtBerat.Text);

            if (!(berat > 0))
            {
                MsgHelper.MsgWarning("'Berat kiriman' tidak boleh kosong !");
                txtBerat.Focus();
                return;
            }

            if (MainProgram.rajaOngkirKey.Length == 0)
            {
                MsgHelper.MsgWarning("Maaf API raja ongkir belum diset !!!\nProses cek ongkos kirim tidak bisa dilanjutkan.");
                return;
            }

            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                CekOngkir();
            }            
        }

        private void CekOngkir()
        {
            var key = MainProgram.rajaOngkirKey; // api key raja ongkir

            var baseUrl = "http://api.rajaongkir.com/starter/";

            var client = new RestClient();
            client.BaseUrl = new Uri(baseUrl);

            // versi gratis raja ongkir, hanya mendukung pengecekan jne, tiki dan pos
            var listOfKurir = new string[] { "jne", "tiki", "pos" };

            _listOfCost.Clear();
            foreach (var kurir in listOfKurir)
            {
                var request = new RestRequest("cost", Method.POST);
                request.AddHeader("key", key);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");

                var query = new query
                {
                    origin = _kabupatenAsal.kabupaten_id.ToString(),
                    destination = _kabupatenTujuan.kabupaten_id.ToString(),
                    weight = (int)NumberHelper.StringToDouble(txtBerat.Text),
                    courier = kurir
                };

                request.RequestFormat = DataFormat.Json;
                request.AddBody(query);

                try
                {
                    var response = client.Execute(request);

                    var ongkir = JsonConvert.DeserializeObject<root>(response.Content);
                    if (ongkir.rajaongkir.results[0].costs.Count > 0)
                    {
                        foreach (var costs in ongkir.rajaongkir.results[0].costs)
                        {
                            costs.kurir_code = kurir.ToUpper();
                            _listOfCost.Add(costs);
                        }
                    }
                }
                catch
                {
                }
            }
            
            GridListControlHelper.Refresh<costs>(this.gridList, _listOfCost);
            base.SetActiveBtnPilih(_listOfCost.Count > 0);
        }

        public void Ok(object sender, object data)
        {
            if (data is KabupatenAsal) // hasil pencarian kabupaten asal
            {
                this._kabupatenAsal = (KabupatenAsal)data;
                txtKabupatenAsal.Text = this._kabupatenAsal.nama_kabupaten;
                KeyPressHelper.NextFocus();
            }
            else if (data is KabupatenTujuan) // hasil pencarian kabupaten tujuan
            {
                this._kabupatenTujuan = (KabupatenTujuan)data;
                txtKabupatenTujuan.Text = this._kabupatenTujuan.nama_kabupaten;
                KeyPressHelper.NextFocus();
            }
        }

        public void Ok(object sender, bool isNewData, object data)
        {
            throw new NotImplementedException();
        }

        private IList<T> GetKabupatenByName<T>(string name)
        {
            var result = _listOfkabupaten.Where(f => f.nama_kabupaten.ToLower().Contains(name.ToLower()) ||
                                                f.Provinsi.nama_provinsi.ToLower().Contains(name.ToLower()))
                                         .OrderBy(f => f.Provinsi.nama_provinsi)
                                         .ThenBy(f => f.nama_kabupaten);

            var serializedParent = JsonConvert.SerializeObject(result);
            IList<T> listOfChild = JsonConvert.DeserializeObject<IList<T>>(serializedParent);

            return listOfChild;
        }

        private void txtKabupatenAsal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                var kabupaten = ((AdvancedTextbox)sender).Text;

                IList<KabupatenAsal> listOfKabupaten = GetKabupatenByName<KabupatenAsal>(kabupaten);

                if (listOfKabupaten.Count == 0)
                {
                    MsgHelper.MsgWarning("Data kota/kabupaten asal tidak ditemukan");
                    txtKabupatenAsal.Focus();
                    txtKabupatenAsal.SelectAll();

                }
                else if (listOfKabupaten.Count == 1)
                {
                    _kabupatenAsal = listOfKabupaten[0];
                    txtKabupatenAsal.Text = _kabupatenAsal.nama_kabupaten;
                    KeyPressHelper.NextFocus();
                }
                else // data lebih dari satu
                {
                    var frmLookup = new FrmLookupReferensi("Data Kota/Kabupaten Asal", listOfKabupaten);
                    frmLookup.Listener = this;
                    frmLookup.ShowDialog();
                }
            }
        }

        private void txtKabupatenTujuan_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressHelper.IsEnter(e))
            {
                var kabupaten = ((AdvancedTextbox)sender).Text;

                IList<KabupatenTujuan> listOfKabupaten = GetKabupatenByName<KabupatenTujuan>(kabupaten);

                if (listOfKabupaten.Count == 0)
                {
                    MsgHelper.MsgWarning("Data kota/kabupaten tujuan tidak ditemukan");
                    txtKabupatenTujuan.Focus();
                    txtKabupatenTujuan.SelectAll();

                }
                else if (listOfKabupaten.Count == 1)
                {
                    _kabupatenTujuan = listOfKabupaten[0];
                    txtKabupatenTujuan.Text = _kabupatenTujuan.nama_kabupaten;
                    KeyPressHelper.NextFocus();
                }
                else // data lebih dari satu
                {
                    var frmLookup = new FrmLookupReferensi("Data Kota/Kabupaten Tujuan", listOfKabupaten);
                    frmLookup.Listener = this;
                    frmLookup.ShowDialog();
                }
            }
        }
    }
}
