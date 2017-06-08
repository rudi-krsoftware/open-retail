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

using log4net;
using Dapper;
using OpenRetail.Model.Nota;
using OpenRetail.Repository.Api;

namespace OpenRetail.Repository.Service
{
    public class CetakNotaDummyRepository : ICetakNotaRepository
    {
        private IList<NotaPenjualan> _listOfNotaJual;

        public CetakNotaDummyRepository()
        {
            InisialisasiDataDummy();
        }

        private void InisialisasiDataDummy()
        {
            _listOfNotaJual = new List<NotaPenjualan>();

            var itemNota1 = new NotaPenjualan
            {
                nama_customer = "Adhi Jaya", alamat = "Jl. Wonosari Km. 11", kecamatan = "Piyungan", kelurahan = "Sitimulyo", kota = "Bantul", kode_pos = "55792", telepon = "0813 8176 9915",
                nota = "201703210056",
                tanggal = DateTime.Today,
                kurir = "Tiki reguler",
                ongkos_kirim = 25000,
                total_nota = 1542000,
                is_sdac = true,
                label_dari1 = "PIXEL KOMPUTER", label_dari2 = "HP: 0813 81769915",
                label_kepada1 = "Bpk. Sunardi",
                label_kepada2 = "Jl. Ring Road Utara",
                label_kepada3 = "Condong Catur - Sleman - Yogyakarta - 55283",
                label_kepada4 = "HP: 0813 2828282",
                kode_produk = "201704070001", nama_produk = "Flashdisk 2 Gb DEAM", harga = 50000, jumlah = 5
            };

            var itemNota2 = new NotaPenjualan
            {
                nama_customer = "Adhi Jaya", alamat = "Jl. Wonosari Km. 11", kecamatan = "Piyungan", kelurahan = "Sitimulyo", kota = "Bantul", kode_pos = "55792", telepon = "0813 8176 9915",
                nota = "201703210056",
                tanggal = DateTime.Today,
                kurir = "Tiki reguler",
                ongkos_kirim = 25000,
                total_nota = 1542000,
                is_sdac = true,
                label_dari1 = "PIXEL KOMPUTER",
                label_dari2 = "HP: 0813 81769915",
                label_kepada1 = "Bpk. Sunardi",
                label_kepada2 = "Jl. Ring Road Utara",
                label_kepada3 = "Condong Catur - Sleman - Yogyakarta - 55283",
                label_kepada4 = "HP: 0813 2828282",
                kode_produk = "201704070002", nama_produk = "HDD 160 Gb SATA Seagate", harga = 500000, jumlah = 1
            };

            var itemNota3 = new NotaPenjualan
            {
                nama_customer = "Adhi Jaya", alamat = "Jl. Wonosari Km. 11", kecamatan = "Piyungan", kelurahan = "Sitimulyo", kota = "Bantul", kode_pos = "55792", telepon = "0813 8176 9915",
                nota = "201703210056",
                tanggal = DateTime.Today,
                kurir = "Tiki reguler",
                ongkos_kirim = 25000,
                total_nota = 1542000,
                is_sdac = true,
                label_dari1 = "PIXEL KOMPUTER",
                label_dari2 = "HP: 0813 81769915",
                label_kepada1 = "Bpk. Sunardi",
                label_kepada2 = "Jl. Ring Road Utara",
                label_kepada3 = "Condong Catur - Sleman - Yogyakarta - 55283",
                label_kepada4 = "HP: 0813 2828282",
                kode_produk = "201704070003", nama_produk = "LCD 16 in Samsung 633NW", harga = 800000, jumlah = 1, diskon = 1
            };

            _listOfNotaJual.Add(itemNota1);
            _listOfNotaJual.Add(itemNota2);
            _listOfNotaJual.Add(itemNota3);
        }

        public IList<NotaPembelian> GetNotaPembelian(string beliProdukId)
        {
            throw new NotImplementedException();
        }

        public IList<NotaPenjualan> GetNotaPenjualan(string jualProdukId)
        {
            return _listOfNotaJual;
        }
    }
}
