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
using System.Threading.Tasks;

using FluentValidation;
using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OpenRetail.Model
{        
	[Table("t_gaji_karyawan")]
    public class GajiKaryawan
    {
        public GajiKaryawan()
        {
            item_pembayaran_kasbon = new List<PembayaranKasbon>();
        }

		[ExplicitKey]
		[Display(Name = "gaji_karyawan_id")]		
		public string gaji_karyawan_id { get; set; }
		
		[Display(Name = "Karyawan")]
		public string karyawan_id { get; set; }

        [JsonIgnore]
		[Write(false)]
        public Karyawan Karyawan { get; set; }

		[Display(Name = "pengguna_id")]
		public string pengguna_id { get; set; }

        [JsonIgnore]
		[Write(false)]
        public Pengguna Pengguna { get; set; }

        [Display(Name = "Nota")]
        public string nota { get; set; }

        [Display(Name = "Tanggal")]
        public Nullable<DateTime> tanggal { get; set; }
      
		[Display(Name = "Bulan")]
		public int bulan { get; set; }
		
		[Display(Name = "Tahun")]
		public int tahun { get; set; }
		
		[Display(Name = "Kehadiran")]
		public int kehadiran { get; set; }
		
		[Display(Name = "Absen")]
		public int absen { get; set; }
		
		[Display(Name = "Gaji Pokok")]
		public double gaji_pokok { get; set; }
		
		[Display(Name = "Lembur")]
		public double lembur { get; set; }
		
		[Display(Name = "Bonus")]
		public double bonus { get; set; }
		
		[Display(Name = "potongan")]
		public double potongan { get; set; }        
		
		[Display(Name = "Jam")]
		public int jam { get; set; }
		
		[Display(Name = "Lainnya")]
		public double lainnya { get; set; }
		
		[Display(Name = "Keterangan")]
		public string keterangan { get; set; }
		
		[Display(Name = "Jumlah Hari")]
		public int jumlah_hari { get; set; }
		
		[Display(Name = "Tunjangan")]
		public double tunjangan { get; set; }
		
        [Computed]
        public double gaji_akhir
        {
            get
            {
                double result = 0;

                if (Karyawan != null)
                {
                    result = Karyawan.jenis_gajian == JenisGajian.Mingguan ? jumlah_hari * gaji_pokok : gaji_pokok;
                }

                return result;
            }
        }

        [Computed]
        public double lembur_akhir
        {
            get
            {
                double result = 0;

                if (Karyawan != null)
                {
                    result = Karyawan.jenis_gajian == JenisGajian.Mingguan ? jam * lembur : lembur;
                }

                return result;
            }
        }

        [Computed]
        public double total_gaji
        {
            get
            {
                return gaji_akhir + tunjangan + lembur_akhir + bonus - potongan;
            }
        }

        [Write(false)]
        public IList<PembayaranKasbon> item_pembayaran_kasbon { get; set; }

        [JsonIgnore]
        [Write(false)]
        [Display(Name = "tanggal_sistem")]
        public Nullable<DateTime> tanggal_sistem { get; set; }
	}

    public class GajiKaryawanValidator : AbstractValidator<GajiKaryawan>
    {
        public GajiKaryawanValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

			RuleFor(c => c.karyawan_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
			RuleFor(c => c.pengguna_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);			
			RuleFor(c => c.nota).NotEmpty().WithMessage(msgError1).Length(1, 20).WithMessage(msgError2);
            RuleFor(c => c.keterangan).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.gaji_pokok).GreaterThan(0).WithMessage(msgError1).When(c => c.Karyawan.jenis_gajian == JenisGajian.Bulanan);
            RuleFor(c => c.jumlah_hari).GreaterThan(0).WithMessage(msgError1).When(c => c.Karyawan.jenis_gajian == JenisGajian.Mingguan);
		}
	}
}
