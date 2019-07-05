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

namespace OpenRetail.Model
{
    public class PengaturanLabelHarga
    {
        public string nama_printer { get; set; }
        public string font_name { get; set; }
        public float batas_atas_baris1 { get; set; }
        public float batas_atas_baris2 { get; set; }
        public float batas_atas_baris3 { get; set; }
        public float batas_atas_baris4 { get; set; }
        public float batas_atas_baris5 { get; set; }
        public float batas_atas_baris6 { get; set; }
        public float batas_atas_baris7 { get; set; }
        public float batas_atas_baris8 { get; set; }

        public float batas_kiri_kolom1 { get; set; }
        public float batas_kiri_kolom2 { get; set; }
        public float batas_kiri_kolom3 { get; set; }
        public float batas_kiri_kolom4 { get; set; }
    }
}