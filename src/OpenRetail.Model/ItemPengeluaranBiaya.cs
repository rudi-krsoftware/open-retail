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
	[Table("t_item_pengeluaran_biaya")]
    public class ItemPengeluaranBiaya
    {
        public ItemPengeluaranBiaya()
        {
            entity_state = EntityState.Added;
        }

		[ExplicitKey]
		[Display(Name = "item_pengeluaran_id")]		
		public string item_pengeluaran_id { get; set; }
		
		[Display(Name = "pengeluaran_id")]
		public string pengeluaran_id { get; set; }

        [JsonIgnore]
		[Write(false)]
        public PengeluaranBiaya PengeluaranBiaya { get; set; }

		[Display(Name = "pengguna_id")]
		public string pengguna_id { get; set; }

        [JsonIgnore]
		[Write(false)]
        public Pengguna Pengguna { get; set; }

		[Display(Name = "Jumlah")]
		public double jumlah { get; set; }
		
		[Display(Name = "Harga")]
		public double harga { get; set; }

        [JsonIgnore]
        [Write(false)]
		[Display(Name = "tanggal_sistem")]
		public Nullable<DateTime> tanggal_sistem { get; set; }
		
		[Display(Name = "jenis_pengeluaran_id")]
		public string jenis_pengeluaran_id { get; set; }

        [JsonIgnore]
		[Write(false)]
        public JenisPengeluaran JenisPengeluaran { get; set; }

        [JsonIgnore]
        [Write(false)]
        public EntityState entity_state { get; set; }
	}

    public class ItemPengeluaranBiayaValidator : AbstractValidator<ItemPengeluaranBiaya>
    {
        public ItemPengeluaranBiayaValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

			RuleFor(c => c.pengeluaran_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
			RuleFor(c => c.pengguna_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
			RuleFor(c => c.jenis_pengeluaran_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
		}
	}
}
