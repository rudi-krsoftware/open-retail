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
using System.Linq;
using System.Text;

namespace OpenRetail.Bll.Api
{
    public interface IImportExportDataBll<T>
    {
        /// <summary>
        /// Method untuk mengecek file excel sedang dibuka aplikasi lain atau tidak
        /// </summary>
        /// <returns></returns>
        bool IsOpened();

        /// <summary>
        /// Method untuk mengecek format file master data valid atau tidak
        /// </summary>
        /// <param name="workSheetName"></param>
        /// <returns></returns>
        bool IsValidFormat(string workSheetName);

        /// <summary>
        /// Method untuk import data
        /// </summary>
        /// <param name="workSheetName"></param>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        bool Import(string workSheetName, ref int rowCount);

        /// <summary>
        /// Method untuk export data
        /// </summary>
        void Export(IList<T> listOfObject);

        /// <summary>
        /// Method untuk mendapatkan list/daftar worksheet
        /// </summary>
        /// <returns></returns>
        IList<string> GetWorksheets();
    }
}
