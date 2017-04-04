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
	[Table("t_item_jual_produk")]
    public class ItemJualProduk
    {
        public ItemJualProduk()
        {
            entity_state = EntityState.Added;
        }

		[ExplicitKey]
		[Display(Name = "item_jual_id")]		
		public string item_jual_id { get; set; }
		
		[Display(Name = "jual_id")]
		public string jual_id { get; set; }

        [JsonIgnore]
		[Write(false)]
        public JualProduk JualProduk { get; set; }

		[Display(Name = "pengguna_id")]
		public string pengguna_id { get; set; }

        [JsonIgnore]
		[Write(false)]
        public Pengguna Pengguna { get; set; }

		[Display(Name = "produk_id")]
		public string produk_id { get; set; }

        [JsonIgnore]
		[Write(false)]
        public Produk Produk { get; set; }

		[Display(Name = "Harga Beli")]
		public double harga_beli { get; set; }
		
		[Display(Name = "Harga Jual")]
		public double harga_jual { get; set; }
		
		[Display(Name = "Jumlah")]
		public double jumlah { get; set; }
		
		[Display(Name = "Diskon")]
		public double diskon { get; set; }

        [JsonIgnore]
        [Write(false)]
		[Display(Name = "tanggal_sistem")]
		public Nullable<DateTime> tanggal_sistem { get; set; }
        
        [Write(false)]
		[Display(Name = "jumlah_retur")]
		public double jumlah_retur { get; set; }

        [JsonIgnore]
        [Computed]
        public double diskon_rupiah
        {
            get { return diskon / 100 * harga_jual; }
        }

        [JsonIgnore]
        [Computed]
        public double harga_setelah_diskon
        {
            get { return harga_jual - diskon_rupiah; }
        }

        [JsonIgnore]
        [Computed]
        public double sub_total
        {
            get { return (jumlah - jumlah_retur) * harga_setelah_diskon; }
        }

        [JsonIgnore]
        [Write(false)]
        public EntityState entity_state { get; set; }
	}

    public class ItemJualProdukValidator : AbstractValidator<ItemJualProduk>
    {
        public ItemJualProdukValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

			RuleFor(c => c.jual_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
			RuleFor(c => c.pengguna_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
			RuleFor(c => c.produk_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
		}
	}
}
