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

using OpenRetail.App.Helper;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenRetail.App.UI.Template
{
    public partial class FrmListEmptyBody : DockContent
    {
        public FrmListEmptyBody()
        {
            InitializeComponent();
        }

        #region protected method

        protected void SetHeader(string header)
        {
            this.TabText = header;
            this.Text = header;
            this.lblHeader.Text = header;
        }

        /// <summary>
        /// Method protected untuk mengaktifkan/menonaktifkan tombol Perbaiki dan Hapus
        /// </summary>
        /// <param name="status"></param>
        protected void SetActiveBtnPerbaikiAndHapus(bool status)
        {
            btnPerbaiki.Enabled = status;
            btnHapus.Enabled = status;
        }

        /// <summary>
        /// Method override untuk menghandle proses tambah
        /// </summary>
        protected virtual void Tambah()
        {
        }

        /// <summary>
        /// Method override untuk menghandle proses perbaiki
        /// </summary>
        protected virtual void Perbaiki()
        {
        }

        /// <summary>
        /// Method override untuk menghandle proses Hapus
        /// </summary>
        protected virtual void Hapus()
        {
        }

        /// <summary>
        /// Method override untuk menghandle proses selesai
        /// </summary>
        protected virtual void Selesai()
        {
            this.Close();
        }

        /// <summary>
        /// Method override untuk menghandle item yang dipilih
        /// </summary>
        /// <param name="index">Diisi dengan index grid list</param>
        /// <param name="prompt">Informasi data yang dipilih</param>
        /// <returns></returns>
        protected bool IsSelectedItem(int index, string prompt)
        {
            if (index < 0)
            {
                var msg = "Maaf data '" + prompt + "' belum dipilih.";
                MsgHelper.MsgWarning(msg);

                return false;
            }

            return true;
        }

        #endregion

        private void btnTambah_Click(object sender, EventArgs e)
        {
            Tambah();
        }

        private void btnPerbaiki_Click(object sender, EventArgs e)
        {
            Perbaiki();
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            Hapus();
        }

        private void btnSelesai_Click(object sender, EventArgs e)
        {
            Selesai();
        }
    }
}
