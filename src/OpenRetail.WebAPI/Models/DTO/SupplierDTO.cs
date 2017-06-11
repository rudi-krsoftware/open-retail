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
    public class SupplierDTO
    {
		[Display(Name = "supplier_id")]		
		public string supplier_id { get; set; }
		
		[Display(Name = "nama_supplier")]
		public string nama_supplier { get; set; }
		
		[Display(Name = "alamat")]
		public string alamat { get; set; }
		
		[Display(Name = "kontak")]
		public string kontak { get; set; }
		
		[Display(Name = "telepon")]
		public string telepon { get; set; }
		
		[Display(Name = "total_hutang")]
		public double total_hutang { get; set; }
		
		[Display(Name = "total_pembayaran_hutang")]
		public double total_pembayaran_hutang { get; set; }
		
	}

    public class SupplierDTOValidator : AbstractValidator<SupplierDTO>
    {
        public SupplierDTOValidator()
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
                RuleFor(c => c.supplier_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
                DefaultRule(msgError1, msgError2);
            });

            RuleSet("delete", () =>
            {
                RuleFor(c => c.supplier_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);			
            });
		}

        private void DefaultRule(string msgError1, string msgError2)
        {
            RuleFor(c => c.nama_supplier).NotEmpty().WithMessage(msgError1).Length(1, 50).WithMessage(msgError2);
            RuleFor(c => c.alamat).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.kontak).Length(0, 50).WithMessage(msgError2);
            RuleFor(c => c.telepon).Length(0, 20).WithMessage(msgError2);			
        }
	}
}
