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

using OpenRetail.App.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenRetail.App.UI.Template
{
    public class BaseFrmList : DockContent
    {
        /// <summary>
        /// Method protected untuk mengaktifkan/menonaktifkan tombol Perbaiki dan Hapus
        /// </summary>
        /// <param name="status"></param>
        protected virtual void SetActiveBtnPerbaikiAndHapus(bool status)
        {
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
        /// Method override untuk menghandle proses buka file master
        /// </summary>
        protected virtual void OpenFileMaster()
        {
        }

        /// <summary>
        /// Method override untuk menghandle proses impor data
        /// </summary>
        protected virtual void ImportData()
        {
        }

        /// <summary>
        /// Method override untuk menghandle proses export data
        /// </summary>
        protected virtual void ExportData()
        {
        }

        /// <summary>
        /// Method override untuk pindah ke awal data
        /// </summary>
        protected virtual void MoveFirst()
        {
        }

        /// <summary>
        /// Method override untuk pindah ke data sebelumnya
        /// </summary>
        protected virtual void MovePrevious()
        {
        }

        /// <summary>
        /// Method override untuk pindah ke data berikutnya
        /// </summary>
        protected virtual void MoveNext()
        {
        }

        /// <summary>
        /// Method override untuk pindah ke akhir data
        /// </summary>
        protected virtual void MoveLast()
        {
        }

        /// <summary>
        /// Method override untuk merubah limir row per halaman
        /// </summary>
        protected virtual void LimitRowChanged()
        {
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
    }
}
