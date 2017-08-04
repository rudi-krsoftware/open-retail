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
	[Table("m_customer")]
    public class Customer
    {
		[ExplicitKey]
		[Display(Name = "customer_id")]		
		public string customer_id { get; set; }
		
		[Display(Name = "Customer")]
		public string nama_customer { get; set; }
		
		[Display(Name = "Alamat")]
		public string alamat { get; set; }

        [Display(Name = "Desa")]
        public string desa { get; set; }

        [Display(Name = "Kelurahan")]
        public string kelurahan { get; set; }

        [Display(Name = "Kecamatan")]
        public string kecamatan { get; set; }        

        [Display(Name = "Kota")]
        public string kota { get; set; }

        [Display(Name = "Kabupaten")]
        public string kabupaten { get; set; }

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
            RuleFor(c => c.kecamatan).Length(0, 100).WithMessage(msgError2);            
            RuleFor(c => c.kota).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.kabupaten).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.kode_pos).Length(0, 6).WithMessage(msgError2);
			RuleFor(c => c.kontak).Length(0, 50).WithMessage(msgError2);
			RuleFor(c => c.telepon).Length(0, 20).WithMessage(msgError2);
		}
	}
}
