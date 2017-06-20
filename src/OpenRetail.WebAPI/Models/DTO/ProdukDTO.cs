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
    public class ProdukDTO
    {
        public ProdukDTO()
        {
            list_of_harga_grosir = new List<HargaGrosirDTO>();
        }

		[Display(Name = "produk_id")]		
		public string produk_id { get; set; }
		
		[Display(Name = "nama_produk")]
		public string nama_produk { get; set; }
		
		[Display(Name = "satuan")]
		public string satuan { get; set; }
		
		[Display(Name = "stok")]
		public double stok { get; set; }
		
		[Display(Name = "harga_beli")]
		public double harga_beli { get; set; }
		
		[Display(Name = "harga_jual")]
		public double harga_jual { get; set; }
		
		[Display(Name = "kode_produk")]
		public string kode_produk { get; set; }

        [Display(Name = "kode_produk_old")]
        public string kode_produk_old { get; set; }

		[Display(Name = "golongan_id")]
		public string golongan_id { get; set; }

        [JsonIgnore]
        public GolonganDTO Golongan { get; set; }

		[Display(Name = "minimal_stok")]
		public double minimal_stok { get; set; }
		
		[Display(Name = "stok_gudang")]
		public double stok_gudang { get; set; }
		
		[Display(Name = "minimal_stok_gudang")]
		public double minimal_stok_gudang { get; set; }
		
		[Display(Name = "diskon")]
		public double diskon { get; set; }

        public IList<HargaGrosirDTO> list_of_harga_grosir { get; set; }
	}

    public class ProdukDTOValidator : AbstractValidator<ProdukDTO>
    {
        public ProdukDTOValidator()
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
                RuleFor(c => c.produk_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
                DefaultRule(msgError1, msgError2);
            });

            RuleSet("delete", () =>
            {
                RuleFor(c => c.produk_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);			
            });
		}

        private void DefaultRule(string msgError1, string msgError2)
        {
            RuleFor(c => c.nama_produk).NotEmpty().WithMessage(msgError1).Length(1, 300).WithMessage(msgError2);
            RuleFor(c => c.satuan).Length(0, 20).WithMessage(msgError2);
            RuleFor(c => c.kode_produk).NotEmpty().WithMessage(msgError1).Length(1, 15).WithMessage(msgError2);
            RuleFor(c => c.golongan_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);			
        }
	}
}
