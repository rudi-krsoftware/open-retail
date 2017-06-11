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

using Newtonsoft.Json;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace OpenRetail.WebAPI.Models.DTO
{        
    public class CustomerDTO
    {
		[Display(Name = "customer_id")]		
		public string customer_id { get; set; }
		
		[Display(Name = "nama_customer")]
		public string nama_customer { get; set; }
		
		[Display(Name = "alamat")]
		public string alamat { get; set; }
		
		[Display(Name = "kontak")]
		public string kontak { get; set; }
		
		[Display(Name = "telepon")]
		public string telepon { get; set; }
		
		[Display(Name = "plafon_piutang")]
		public double plafon_piutang { get; set; }
		
		[Display(Name = "total_piutang")]
		public double total_piutang { get; set; }
		
		[Display(Name = "total_pembayaran_piutang")]
		public double total_pembayaran_piutang { get; set; }
		
		[Display(Name = "kecamatan")]
		public string kecamatan { get; set; }
		
		[Display(Name = "kelurahan")]
		public string kelurahan { get; set; }
		
		[Display(Name = "kota")]
		public string kota { get; set; }
		
		[Display(Name = "kode_pos")]
		public string kode_pos { get; set; }
		
		[Display(Name = "diskon")]
		public double diskon { get; set; }
	}

    public class CustomerDTOValidator : AbstractValidator<CustomerDTO>
    {
        public CustomerDTOValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "'{PropertyName}' maksimal {MaxLength} karakter !";

			RuleSet("save", () =>
            {
                DefaultRule(msgError1, msgError2);
            });

            RuleSet("update", () =>
            {
                RuleFor(c => c.customer_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
                DefaultRule(msgError1, msgError2);
            });

            RuleSet("delete", () =>
            {
                RuleFor(c => c.customer_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);						
            });
		}

        private void DefaultRule(string msgError1, string msgError2)
        {
            RuleFor(c => c.nama_customer).NotEmpty().WithMessage(msgError1).Length(1, 50).WithMessage(msgError2);
            RuleFor(c => c.alamat).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.kecamatan).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.kelurahan).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.kota).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.kode_pos).Length(0, 6).WithMessage(msgError2);
            RuleFor(c => c.kontak).Length(0, 50).WithMessage(msgError2);
            RuleFor(c => c.telepon).Length(0, 20).WithMessage(msgError2);
        }
	}
}
