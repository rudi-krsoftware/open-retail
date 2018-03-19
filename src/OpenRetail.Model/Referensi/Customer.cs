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
	[Table("m_customer")]
    public class Customer
    {
		[ExplicitKey]
		[Display(Name = "customer_id")]		
		public string customer_id { get; set; }
		
		[Display(Name = "Customer")]
		public string nama_customer { get; set; }

        public string provinsi_id { get; set; }

        [Write(false)]
        public Provinsi Provinsi { get; set; }

        public string kabupaten_id { get; set; }

        [Write(false)]
        public string kabupaten_old { get; set; }

        [Write(false)]
        public Kabupaten Kabupaten { get; set; }

        public string kecamatan_id { get; set; }

        [Write(false)]
        public string kecamatan_old { get; set; }

        [Write(false)]
        public Kecamatan Kecamatan { get; set; }

		[Display(Name = "Alamat")]
		public string alamat { get; set; }

        [Display(Name = "Desa")]
        public string desa { get; set; }

        [Display(Name = "Kelurahan")]
        public string kelurahan { get; set; }

        [Display(Name = "Kota")]
        public string kota { get; set; }

        [Display(Name = "Kode Pos")]
        public string kode_pos { get; set; }

		[Display(Name = "Kontak")]
		public string kontak { get; set; }
		
		[Display(Name = "Telepon")]
		public string telepon { get; set; }

        [Display(Name = "Diskon")]
        public double diskon { get; set; }

		[Display(Name = "plafon_piutang")]
		public double plafon_piutang { get; set; }

        [JsonIgnore]
        [Write(false)]
        [Display(Name = "get_wilayah_lengkap")]
        public string get_wilayah_lengkap
        {
            get
            {
                var provinsi = this.Provinsi != null ? this.Provinsi.nama_provinsi : string.Empty;
                var kabupaten = this.Kabupaten != null ? this.Kabupaten.nama_kabupaten : this.kabupaten_old.NullToString();
                var kecamatan = this.Kecamatan != null ? this.Kecamatan.nama_kecamatan : this.kecamatan_old.NullToString();

                var kodePos = (string.IsNullOrEmpty(this.kode_pos) || this.kode_pos == "0") ? string.Empty : this.kode_pos;

                var sb = new StringBuilder();

                if (provinsi.Length > 0)
                    sb.Append(string.Format("{0}", provinsi)).Append(", ");

                if (kabupaten.Length > 0)
                    sb.Append(string.Format("{0}", kabupaten)).Append(", ");

                if (kecamatan.Length > 0)
                    sb.Append(string.Format("{0}", kecamatan)).Append(", ");

                if (kodePos.Length > 0)
                    sb.Append(kodePos);

                var wilayahLengkap = sb.ToString();

                if (wilayahLengkap.Length > 2)
                {
                    if (wilayahLengkap.Right(2) == ", ")
                    {
                        wilayahLengkap = wilayahLengkap.Left(wilayahLengkap.Length - 2);
                    }
                }

                return wilayahLengkap;
            }
        }

        [Computed]
		[Display(Name = "total_piutang")]
		public double total_piutang { get; set; }

        [Computed]
		[Display(Name = "total_pembayaran_piutang")]
		public double total_pembayaran_piutang { get; set; }

        [Computed]
        public double sisa_piutang
        {
            get { return total_piutang - total_pembayaran_piutang; }
        }
	}

    public class CustomerValidator : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

			RuleFor(c => c.nama_customer).NotEmpty().WithMessage(msgError1).Length(1, 50).WithMessage(msgError2);
            RuleFor(c => c.alamat).Length(0, 250).WithMessage(msgError2);
            RuleFor(c => c.desa).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.kelurahan).Length(0, 100).WithMessage(msgError2);
            // RuleFor(c => c.kecamatan).Length(0, 100).WithMessage(msgError2);            
            RuleFor(c => c.kota).Length(0, 100).WithMessage(msgError2);
            // RuleFor(c => c.kabupaten).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.kode_pos).Length(0, 6).WithMessage(msgError2);
			RuleFor(c => c.kontak).Length(0, 50).WithMessage(msgError2);
			RuleFor(c => c.telepon).Length(0, 20).WithMessage(msgError2);
		}
	}
}
