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
    public class ReportPiutangPenjualanProduk
    {
        public string customer_id { get; set; }
        public string nama_customer { get; set; }
        public string jual_id { get; set; }
        public string nota { get; set; }
        public DateTime tanggal { get; set; }
        public DateTime tanggal_tempo { get; set; }
        public double ppn { get; set; }
        public double diskon_nota { get; set; }
        public double ongkos_kirim { get; set; }
        public double total_nota { get; set; }
        public double total_pelunasan { get; set; }
        public double total_pelunasan_customer  { get; set; }

        public string produk_id { get; set; }
        public string nama_produk { get; set; }
        public string satuan { get; set; }
        public double jumlah { get; set; }
        public double jumlah_retur { get; set; }
        public double diskon { get; set; }
        public double harga_jual { get; set; }

        public double diskon_rupiah_harga_jual
        {
            get { return diskon / 100 * harga_jual; }
        }

        public double harga_jual_setelah_diskon
        {
            get { return harga_jual - diskon_rupiah_harga_jual; }
        }

        public double grand_total
        {
            get { return total_nota - diskon_nota + ongkos_kirim + ppn; }
        }

        public double grand_total_customer { get; set; }

        public double sisa_nota
        {
            get { return grand_total - total_pelunasan; }
        }

        public double sisa_nota_customer
        {
            get { return grand_total_customer - total_pelunasan_customer; }
        }

        public double sub_total
        {
            get { return (jumlah - jumlah_retur) * harga_jual_setelah_diskon; }
        }
    }
}
