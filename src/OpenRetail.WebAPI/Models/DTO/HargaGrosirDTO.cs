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
    public class HargaGrosirDTO
    {
		[Display(Name = "harga_grosir_id")]		
		public string harga_grosir_id { get; set; }
		
		[Display(Name = "produk_id")]
		public string produk_id { get; set; }

		[JsonIgnore]
        public ProdukDTO Produk { get; set; }

		[Display(Name = "harga_ke")]
		public int harga_ke { get; set; }
		
		[Display(Name = "harga_grosir")]
		public double harga_grosir { get; set; }
		
		[Display(Name = "jumlah_minimal")]
		public double jumlah_minimal { get; set; }
		
		[Display(Name = "diskon")]
		public double diskon { get; set; }
		
	}

    public class HargaGrosirDTOValidator : AbstractValidator<HargaGrosirDTO>
    {
        public HargaGrosirDTOValidator()
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
                RuleFor(c => c.harga_grosir_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
                DefaultRule(msgError1, msgError2);
            });

            RuleSet("delete", () =>
            {
                RuleFor(c => c.harga_grosir_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);			
            });
		}

        private void DefaultRule(string msgError1, string msgError2)
        {
            RuleFor(c => c.produk_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            RuleFor(c => c.harga_ke).NotEmpty().WithMessage(msgError1);
            RuleFor(c => c.harga_grosir).NotEmpty().WithMessage(msgError1);
            RuleFor(c => c.jumlah_minimal).NotEmpty().WithMessage(msgError1);
        }
	}
}
