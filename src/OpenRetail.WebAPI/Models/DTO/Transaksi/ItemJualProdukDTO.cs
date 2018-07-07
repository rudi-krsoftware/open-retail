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
using OpenRetail.Model;

namespace OpenRetail.WebAPI.Models.DTO
{
    public class ItemJualProdukDTO
    {
		[Display(Name = "item_jual_id")]		
		public string item_jual_id { get; set; }
		
		[Display(Name = "jual_id")]
		public string jual_id { get; set; }

        [JsonIgnore]
        public JualProdukDTO JualProduk { get; set; }

		[Display(Name = "pengguna_id")]
		public string pengguna_id { get; set; }

        [JsonIgnore]
        public PenggunaDTO Pengguna { get; set; }

		[Display(Name = "produk_id")]
		public string produk_id { get; set; }

        public ProdukDTO Produk { get; set; }

        [Display(Name = "Keterangan tambahan")]
        public string keterangan { get; set; }

		[Display(Name = "Harga Beli")]
		public double harga_beli { get; set; }
		
		[Display(Name = "Harga Jual")]
		public double harga_jual { get; set; }

        [Display(Name = "Old Jumlah")]
        public double old_jumlah { get; set; }

		[Display(Name = "Jumlah")]
		public double jumlah { get; set; }
		
		[Display(Name = "Diskon")]
		public double diskon { get; set; }

        [JsonIgnore]
		[Display(Name = "tanggal_sistem")]
		public Nullable<DateTime> tanggal_sistem { get; set; }
        
		[Display(Name = "jumlah_retur")]
		public double jumlah_retur { get; set; }

        public double diskon_rupiah
        {
            get { return diskon / 100 * harga_jual; }
        }

        public double harga_setelah_diskon
        {
            get { return harga_jual - diskon_rupiah; }
        }

        public double sub_total
        {
            get { return (jumlah - jumlah_retur) * harga_setelah_diskon; }
        }

        public EntityState entity_state { get; set; }
    }

    public class ItemJualProdukDTOValidator : AbstractValidator<ItemJualProdukDTO>
    {
        public ItemJualProdukDTOValidator()
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
                RuleFor(c => c.item_jual_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
                DefaultRule(msgError1, msgError2);
            });

            RuleSet("delete", () =>
            {
                RuleFor(c => c.item_jual_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            });
        }

        private void DefaultRule(string msgError1, string msgError2)
        {
            RuleFor(c => c.jual_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            RuleFor(c => c.pengguna_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            RuleFor(c => c.produk_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            RuleFor(c => c.keterangan).Length(0, 100).WithMessage(msgError2);
        }
    }
}