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

namespace OpenRetail.Model.RajaOngkir
{
    public class query
    {
        public string id { get; set; }

        /// <summary>
        /// id kota asal
        /// </summary>
        public string origin { get; set; }

        /// <summary>
        /// id kota tujuan
        /// </summary>
        public string destination { get; set; }

        /// <summary>
        /// Berat kiriman (gram)
        /// </summary>
        public int weight { get; set; }

        /// <summary>
        /// Kode kurir yang dipakai
        /// </summary>
        public string courier { get; set; }
    }
}
