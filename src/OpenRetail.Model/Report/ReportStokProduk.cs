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
    public enum StatusStok
    {
        Semua = 1,
        Ada = 2,
        Kosong = 3
    }

    public class ReportStokProduk
    {
        public string produk_id { get; set; }
        public string nama_produk { get; set; }
        public string satuan { get; set; }
        public double stok { get; set; }
        public double stok_gudang { get; set; }
        public double harga_beli { get; set; }
        public double harga_jual { get; set; }
        public string golongan_id { get; set; }
        public string nama_golongan { get; set; }        
        public double asset
        {
            get { return (stok + stok_gudang) > 0 ? (stok + stok_gudang) * harga_jual : 0; }
        }
    }
}
