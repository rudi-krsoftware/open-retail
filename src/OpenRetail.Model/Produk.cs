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
	[Table("m_produk")]
    public class Produk
    {
        public Produk()
        {
            list_of_harga_grosir = new List<HargaGrosir>();
        }

		[ExplicitKey]
		[Display(Name = "produk_id")]		
		public string produk_id { get; set; }
		
		[Display(Name = "Nama Produk")]
		public string nama_produk { get; set; }
		
		[Display(Name = "Satuan")]
		public string satuan { get; set; }
		
		[Display(Name = "Stok")]
		public double stok { get; set; }
		
		[Display(Name = "Harga Beli")]
		public double harga_beli { get; set; }
		
		[Display(Name = "Harga Jual")]
		public double harga_jual { get; set; }

        [Display(Name = "Diskon")]
        public double diskon { get; set; }

		[Display(Name = "Kode Produk")]
		public string kode_produk { get; set; }

        [Write(false)]
        public string kode_produk_old { get; set; }

        [Display(Name = "Golongan")]
		public string golongan_id { get; set; }

		[Write(false)]
        public Golongan Golongan { get; set; }

		[Display(Name = "Minimal Stok")]
		public double minimal_stok { get; set; }

		[Display(Name = "Stok Gudang")]
		public double stok_gudang { get; set; }

		[Display(Name = "Minimal Stok Gudang")]
		public double minimal_stok_gudang { get; set; }

        [Computed]
        public double asset
        {
            get { return (stok + stok_gudang) > 0 ? (stok + stok_gudang) * harga_jual : 0; }
        }

        [Write(false)]
        public IList<HargaGrosir> list_of_harga_grosir { get; set; }
	}

    public class ProdukValidator : AbstractValidator<Produk>
    {
        public ProdukValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

            RuleFor(c => c.kode_produk).NotEmpty().WithMessage(msgError1).Length(1, 15).WithMessage(msgError2);
            RuleFor(c => c.nama_produk).NotEmpty().WithMessage(msgError1).Length(1, 300).WithMessage(msgError2);
			RuleFor(c => c.satuan).Length(0, 20).WithMessage(msgError2);			
		}
	}
}
