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
    public class PengaturanUmum
    {
        public string nama_printer { get; set; }
        public bool is_auto_print { get; set; }
        public bool is_auto_print_label_nota { get; set; }
        public bool is_printer_mini_pos { get; set; }
        public bool is_cetak_customer { get; set; }
        public bool is_show_minimal_stok { get; set; }
        public bool is_customer_required { get; set; }
        public bool is_singkat_penulisan_ongkir { get; set; }
        public int jumlah_karakter { get; set; }
        public int jumlah_gulung { get; set; }
        public IList<HeaderNota> list_of_header_nota { get; set; }
        public IList<HeaderNotaMiniPos> list_of_header_nota_mini_pos { get; set; }
        public IList<FooterNotaMiniPos> list_of_footer_nota_mini_pos { get; set; }
        public IList<LabelNota> list_of_label_nota { get; set; }
    }
}
