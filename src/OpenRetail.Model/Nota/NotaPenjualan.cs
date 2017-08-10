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

namespace OpenRetail.Model.Nota
{
    public class NotaPenjualan
    {
        public string nama_customer { get; set; }
        public string alamat { get; set; }
        public string kecamatan { get; set; }
        public string kelurahan { get; set; }
        public string kota { get; set; }
        public string kode_pos { get; set; }

        public string kontak { get; set; }
        public string telepon { get; set; }

        /// <summary>
        /// Property untuk menyimpan informasi apakah alamat kirim sama dengan alamat customer
        /// </summary>
        public bool is_sdac { get; set; }

        public bool is_dropship { get; set; }

        public string kirim_kepada { get; set; }
        public string kirim_alamat { get; set; }
        public string kirim_desa { get; set; }
        public string kirim_kelurahan { get; set; }
        public string kirim_kecamatan { get; set; }        
        public string kirim_kota { get; set; }
        public string kirim_kabupaten { get; set; }
        public string kirim_kode_pos { get; set; }
        public string kirim_telepon { get; set; }

        public string label_dari1 { get; set; }
        public string label_dari2 { get; set; }
        public string label_dari3 { get; set; }
        public string label_dari4 { get; set; }
        public string label_kepada1 { get; set; }
        public string label_kepada2 { get; set; }
        public string label_kepada3 { get; set; }
        public string label_kepada4 { get; set; }

        public string nota { get; set; }
        public DateTime tanggal { get; set; }
        public DateTime tanggal_tempo { get; set; }
        public double ppn { get; set; }
        public double diskon_nota { get; set; }
        public string kurir { get; set; }
        public double ongkos_kirim { get; set; }
        public string label_ongkos_kirim { get; set; }
        public double total_nota { get; set; }        

        public string kode_produk { get; set; }
        public string nama_produk { get; set; }
        public string satuan { get; set; }
        public double harga { get; set; }
        public double jumlah { get; set; }
        public double jumlah_retur { get; set; }
        public double diskon { get; set; }

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
