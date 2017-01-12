using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenRetail.App.UserControl
{
    [DefaultEvent("BtnTampilkanClicked")]
    public partial class FilterRangeTanggal : System.Windows.Forms.UserControl
    {
        public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler BtnTampilkanClicked;
        public event EventHandler ChkTampilkanSemuaDataClicked;        

        public FilterRangeTanggal()
        {
            InitializeComponent();

            dtpTanggalMulai.Value = DateTime.Today;
            dtpTanggalSelesai.Value = DateTime.Today;
        }

        public DateTime TanggalMulai
        {
            get { return dtpTanggalMulai.Value; }
        }

        public DateTime TanggalSelesai
        {
            get { return dtpTanggalSelesai.Value; }
        }

        private void btnTampilkan_Click(object sender, EventArgs e)
        {
            if (BtnTampilkanClicked != null)
                BtnTampilkanClicked(sender, e);
        }

        private void chkTampilkanSemuaData_CheckedChanged(object sender, EventArgs e)
        {
            var chk = (CheckBox)sender;
            var isEnable = false;

            if (chk.Checked)
                isEnable = false;
            else
                isEnable = true;

            dtpTanggalMulai.Enabled = isEnable;
            dtpTanggalSelesai.Enabled = isEnable;
            btnTampilkan.Enabled = isEnable;

            if (ChkTampilkanSemuaDataClicked != null)
                ChkTampilkanSemuaDataClicked(sender, e);
        }
    }
}
