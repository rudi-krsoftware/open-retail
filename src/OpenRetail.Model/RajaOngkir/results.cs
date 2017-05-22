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
    public class results
    {
        public string province_id { get; set; }
        public string province { get; set; }

        /// <summary>
        /// id kota/kabupaten
        /// </summary>
        public string city_id { get; set; }

        /// <summary>
        /// Jenis Daerah Tingkat II. Berisi "Kota" atau "Kabupaten"
        /// </summary>
        public string type { get; set; }
        public string city_name { get; set; }
        public string postal_code { get; set; }

        public string code { get; set; }
        public string name { get; set; }
        public List<costs> costs { get; set; }
    }
}
