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
    public class ReportCustomerProduk
    {
        public string produk_id { get; set; }
        public string nama_produk { get; set; }
        public string customer_id { get; set; }
        public string nama_customer { get; set; }
        public string alamat { get; set; }
        public string telepon { get; set; }
        public double jumlah { get; set; }
    }
}