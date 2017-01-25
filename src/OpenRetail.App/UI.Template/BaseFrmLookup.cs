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
using System.Windows.Forms;

namespace OpenRetail.App.UI.Template
{
    public class BaseFrmLookup : Form
    {
        /// <summary>
        /// Status tombol pilihan aktif atau enggak
        /// </summary>
        public virtual bool IsButtonPilihEnabled { get; private set; }

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
                var msg = "Maaf '" + prompt + "' belum dipilih.";
                MsgHelper.MsgWarning(msg);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Method protected untuk mengeset header form lookup
        /// </summary>
        /// <param name="header"></param>
        protected virtual void SetHeader(string header)
        {
        }

        protected virtual void SetTitleBtnPilih(string title)
        {
        }

        /// <summary>
        /// Method protected untuk mengaktifkan/menonaktifkan tombol pilih
        /// </summary>
        /// <param name="status"></param>
        protected virtual void SetActiveBtnPilih(bool status)
        {
        }

        /// <summary>
        /// Method override untuk menghandle proses pilih
        /// </summary>
        protected virtual void Pilih()
        {
        }

        /// <summary>
        /// Method override untuk menghandle event checkbox pilih semua
        /// </summary>
        protected virtual void PilihSemua()
        {
        }

        /// <summary>
        /// Method override untuk menghandle proses selesai
        /// </summary>
        protected virtual void Batal()
        {
            this.Close();
        }
    }
}
