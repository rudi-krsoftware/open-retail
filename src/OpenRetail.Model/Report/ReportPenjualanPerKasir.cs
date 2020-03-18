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

namespace OpenRetail.Model.Report
{
    public class ReportPenjualanPerKasir
    {
        // header
        public string kasir_id { get; set; }

        public string kasir { get; set; }
        public double saldo_awal { get; set; }
        public double ongkos_kirim { get; set; }
        public double ppn { get; set; }
        public double diskon_nota { get; set; }
        public double total_nota { get; set; }

        public double grand_total
        {
            get { return total_nota - diskon_nota + ongkos_kirim + ppn; }
        }

        // detail
        public string nama_produk { get; set; }

        public string keterangan { get; set; }
        public double harga_jual { get; set; }
        public double jumlah { get; set; }
        public double jumlah_retur { get; set; }
        public double diskon { get; set; }

        public double diskon_rupiah
        {
            get { return diskon / 100 * harga_jual; }
        }

        public double harga_jual_setelah_diskon
        {
            get { return harga_jual - diskon_rupiah; }
        }

        public double sub_total
        {
            get { return (jumlah - jumlah_retur) * harga_jual_setelah_diskon; }
        }
    }
}