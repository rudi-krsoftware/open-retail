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

namespace OpenRetail.Model
{        
	[Table("t_gaji_karyawan")]
    public class GajiKaryawan
    {
		[ExplicitKey]
		[Display(Name = "gaji_karyawan_id")]		
		public string gaji_karyawan_id { get; set; }
		
		[Display(Name = "karyawan_id")]
		public string karyawan_id { get; set; }

		[Write(false)]
        public Karyawan Karyawan { get; set; }

		[Display(Name = "pengguna_id")]
		public string pengguna_id { get; set; }

		[Write(false)]
        public Pengguna Pengguna { get; set; }

		[Display(Name = "bulan")]
		public int bulan { get; set; }
		
		[Display(Name = "tahun")]
		public int tahun { get; set; }
		
		[Display(Name = "kehadiran")]
		public int kehadiran { get; set; }
		
		[Display(Name = "absen")]
		public int absen { get; set; }
		
		[Display(Name = "gaji_pokok")]
		public double gaji_pokok { get; set; }
		
		[Display(Name = "lembur")]
		public double lembur { get; set; }
		
		[Display(Name = "bonus")]
		public double bonus { get; set; }
		
		[Display(Name = "potongan")]
		public double potongan { get; set; }
		
		[Display(Name = "tanggal_sistem")]
		public Nullable<DateTime> tanggal_sistem { get; set; }
		
		[Display(Name = "minggu")]
		public int minggu { get; set; }
		
		[Display(Name = "jam")]
		public int jam { get; set; }
		
		[Display(Name = "lainnya")]
		public double lainnya { get; set; }
		
		[Display(Name = "keterangan")]
		public string keterangan { get; set; }
		
		[Display(Name = "jumlah_hari")]
		public int jumlah_hari { get; set; }
		
		[Display(Name = "tunjangan")]
		public double tunjangan { get; set; }
		
		[Display(Name = "kasbon")]
		public double kasbon { get; set; }
		
		[Display(Name = "tanggal")]
		public Nullable<DateTime> tanggal { get; set; }
		
		[Display(Name = "nota")]
		public string nota { get; set; }
		
	}

    public class GajiKaryawanValidator : AbstractValidator<GajiKaryawan>
    {
        public GajiKaryawanValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

			// TODO : non aktifkan validasi yang tidak perlu

			RuleFor(c => c.karyawan_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
			RuleFor(c => c.pengguna_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
			RuleFor(c => c.keterangan).NotEmpty().WithMessage(msgError1).Length(1, 100).WithMessage(msgError2);
			RuleFor(c => c.nota).NotEmpty().WithMessage(msgError1).Length(1, 20).WithMessage(msgError2);
		}
	}
}
