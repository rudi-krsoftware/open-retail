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
	[Table("t_beli_produk")]
    public class BeliProduk
    {
        private Nullable<DateTime> _tanggal_tempo;

        public BeliProduk()
        {
            item_beli = new List<ItemBeliProduk>();
            item_beli_deleted = new List<ItemBeliProduk>();
        }

		[ExplicitKey]
		[Display(Name = "beli_produk_id")]		
		public string beli_produk_id { get; set; }
		
		[Display(Name = "pengguna_id")]
		public string pengguna_id { get; set; }

        [JsonIgnore]
		[Write(false)]
        public Pengguna Pengguna { get; set; }

		[Display(Name = "Supplier")]
		public string supplier_id { get; set; }

        [JsonIgnore]
		[Write(false)]
        public Supplier Supplier { get; set; }

		[Display(Name = "retur_beli_produk_id")]
		public string retur_beli_produk_id { get; set; }

        [JsonIgnore]
		[Write(false)]
        public ReturBeliProduk ReturBeliProduk { get; set; }

		[Display(Name = "Nota")]
		public string nota { get; set; }
		
		[Display(Name = "Tanggal")]
		public Nullable<DateTime> tanggal { get; set; }
		
		[Display(Name = "Tanggal Tempo")]
        public Nullable<DateTime> tanggal_tempo
        {
            get { return _tanggal_tempo.IsNull() ? null : _tanggal_tempo; }
            set { _tanggal_tempo = value; }
        }

        [JsonIgnore]
        [Write(false)]
        public Nullable<DateTime> tanggal_tempo_old { get; set; }

		[Display(Name = "PPN")]
		public double ppn { get; set; }
		
		[Display(Name = "diskon")]
		public double diskon { get; set; }
		
        [Computed]
		[Display(Name = "total_nota")]
		public double total_nota { get; set; }

        [Computed]
		[Display(Name = "total_pelunasan")]
		public double total_pelunasan { get; set; }
		
		[Display(Name = "Keterangan")]
		public string keterangan { get; set; }

        [JsonIgnore]
        [Write(false)]
		[Display(Name = "tanggal_sistem")]
		public Nullable<DateTime> tanggal_sistem { get; set; }

        [JsonIgnore]
        [Computed]
        public double total_pelunasan_old { get; set; }

        [JsonIgnore]
        [Computed]
        public double grand_total
        {
            get { return total_nota - diskon + ppn; }
        }

        [JsonIgnore]
        [Computed]
        public double sisa_nota
        {
            get { return grand_total - total_pelunasan; }
        }

        [JsonIgnore]
        [Write(false)]
        public bool is_tunai { get; set; }

        [Write(false)]
        public IList<ItemBeliProduk> item_beli { get; set; }
        
        [Write(false)]
        public IList<ItemBeliProduk> item_beli_deleted { get; set; }
	}

    public class BeliProdukValidator : AbstractValidator<BeliProduk>
    {
        public BeliProdukValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

			RuleFor(c => c.pengguna_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
			RuleFor(c => c.nota).NotEmpty().WithMessage(msgError1).Length(1, 20).WithMessage(msgError2);
			RuleFor(c => c.keterangan).Length(0, 100).WithMessage(msgError2);
		}
	}
}
