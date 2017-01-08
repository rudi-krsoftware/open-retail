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

namespace OpenRetail.App.Referensi
{
    public partial class FrmListGolongan : FrmListStandard, IListener
    {
        private IGolonganBll _bll; // deklarsi objek business logic layer 
        private IList<Golongan> _listOfGolongan = new List<Golongan>();

        public FrmListGolongan(string header)
            : base(header)
        {
            InitializeComponent();

            _bll = new GolonganBll();
            LoadData();

            InitGridList();
        }

        private void InitGridList()
        {
            var gridListProperties = new List<GridListControlProperties>();

            gridListProperties.Add(new GridListControlProperties { Header = "No", Width = 30 });
            gridListProperties.Add(new GridListControlProperties { Header = "Golongan" });

            GridListControlHelper.InitializeGridListControl<Golongan>(this.gridList, _listOfGolongan, gridListProperties);

            if (_listOfGolongan.Count > 0)
                this.gridList.SetSelected(0, true);

            this.gridList.Grid.QueryCellInfo += delegate(object sender, GridQueryCellInfoEventArgs e)
            {
                if (_listOfGolongan.Count > 0)
                {
                    if (e.RowIndex > 0)
                    {
                        var rowIndex = e.RowIndex - 1;

                        if (rowIndex < _listOfGolongan.Count)
                        {
                            var obj = _listOfGolongan[rowIndex];

                            switch (e.ColIndex)
                            {
                                case 2:
                                    e.Style.CellValue = obj.nama_golongan;
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

        private void LoadData()
        {
            using (new StCursor(Cursors.WaitCursor, new TimeSpan(0, 0, 0, 0)))
            {
                _listOfGolongan = _bll.GetAll();
            }

            ResetButton();
        }

        private void ResetButton()
        {
            base.SetActiveBtnPerbaikiAndHapus(_listOfGolongan.Count > 0);
        }

        protected override void Tambah()
        {
            var frm = new FrmEntryGolongan("Tambah Data " + this.TabText, _bll);
            frm.Listener = this;
            frm.ShowDialog();
        }

        protected override void Perbaiki()
        {
            var index = this.gridList.SelectedIndex;

            if (!base.IsSelectedItem(index, this.Text))
                return;

            var golongan = _listOfGolongan[index];

            var frm = new FrmEntryGolongan("Edit Data " + this.TabText, golongan, _bll);
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
                var golongan = _listOfGolongan[index];

                var result = _bll.Delete(golongan);
                if (result > 0)
                {
                    GridListControlHelper.RemoveObject<Golongan>(this.gridList, _listOfGolongan, golongan);
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
            var obj = (Golongan)data;

            if (isNewData)
            {
                GridListControlHelper.AddObject<Golongan>(this.gridList, _listOfGolongan, obj);
                ResetButton();
            }
            else
                GridListControlHelper.UpdateObject<Golongan>(this.gridList, _listOfGolongan, obj);
        }
    }
}
