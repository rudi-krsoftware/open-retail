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
	[Table("t_retur_jual_produk")]
    public class ReturJualProduk
    {
        public ReturJualProduk()
        {
            item_retur = new List<ItemReturJualProduk>();
            item_retur_deleted = new List<ItemReturJualProduk>();
        }

		[ExplicitKey]
		[Display(Name = "retur_jual_id")]		
		public string retur_jual_id { get; set; }
		
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

		[Display(Name = "Customer")]
		public string customer_id { get; set; }

        [JsonIgnore]
		[Write(false)]
        public Customer Customer { get; set; }

		[Display(Name = "Nota")]
		public string nota { get; set; }
		
		[Display(Name = "Tanggal")]
		public Nullable<DateTime> tanggal { get; set; }
		
		[Display(Name = "Keterangan")]
		public string keterangan { get; set; }

        [JsonIgnore]
        [Write(false)]
		[Display(Name = "tanggal_sistem")]
		public Nullable<DateTime> tanggal_sistem { get; set; }

        [JsonIgnore]
        [Computed]
		[Display(Name = "total_nota")]
		public double total_nota { get; set; }

        [Write(false)]
        public IList<ItemReturJualProduk> item_retur { get; set; }

        [Write(false)]
        public IList<ItemReturJualProduk> item_retur_deleted { get; set; }		
	}

    public class ReturJualProdukValidator : AbstractValidator<ReturJualProduk>
    {
        public ReturJualProdukValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

			RuleFor(c => c.jual_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
			RuleFor(c => c.pengguna_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
			RuleFor(c => c.customer_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
			RuleFor(c => c.nota).NotEmpty().WithMessage(msgError1).Length(1, 20).WithMessage(msgError2);
			RuleFor(c => c.keterangan).Length(0, 100).WithMessage(msgError2);
		}
	}
}
