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
    public class ReportGajiKaryawan
    {
        public string karyawan_id { get; set; }
        public string nama_karyawan { get; set; }
        public string nama_jabatan { get; set; }
        public JenisGajian jenis_gajian { get; set; }

        public DateTime tanggal { get; set; }
        public int bulan { get; set; }
        public int tahun { get; set; }
        public int kehadiran { get; set; }
        public int absen { get; set; }
        public double gaji_pokok { get; set; }
        public double lembur { get; set; }
        public double bonus { get; set; }
        public double potongan { get; set; }
        public int jam { get; set; }
        public int jumlah_hari { get; set; }
        public double tunjangan { get; set; }
        
        public double gaji_akhir
        {
            get
            {
                double result = jenis_gajian == JenisGajian.Mingguan ? jumlah_hari * gaji_pokok : gaji_pokok;

                return result;
            }
        }

        public double lembur_akhir
        {
            get
            {
                double result = jenis_gajian == JenisGajian.Mingguan ? jam * lembur : lembur;

                return result;
            }
        }

        public double total_gaji
        {
            get
            {
                return gaji_akhir + tunjangan + lembur_akhir + bonus - potongan;
            }
        }
    }
}
