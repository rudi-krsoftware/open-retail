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

namespace OpenRetail.Model.Report
{
    public class ReportPembelianProdukDetail
    {
        public string supplier_id { get; set; }
        public string nama_supplier { get; set; }
        public string nota { get; set; }
        public DateTime tanggal { get; set; }
        public string produk_id { get; set; }
        public string nama_produk { get; set; }
        public string satuan { get; set; }
        public double jumlah { get; set; }
        public double diskon { get; set; }
        public double jumlah_retur { get; set; }
        public double harga { get; set; }

        public double diskon_rupiah
        {
            get { return diskon / 100 * harga; }
        }

        public double harga_setelah_diskon
        {
            get { return harga - diskon_rupiah; }
        }

        public double sub_total
        {
            get { return (jumlah - jumlah_retur) * harga_setelah_diskon; }
        }
    }
}
