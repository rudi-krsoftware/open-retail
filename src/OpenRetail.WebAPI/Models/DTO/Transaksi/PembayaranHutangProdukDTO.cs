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

using FluentValidation;
using Newtonsoft.Json;
using OpenRetail.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenRetail.WebAPI.Models.DTO
{
    public class PembayaranHutangProdukDTO
    {
        public PembayaranHutangProdukDTO()
        {
            item_pembayaran_hutang = new List<ItemPembayaranHutangProduk>();
            item_pembayaran_hutang_deleted = new List<ItemPembayaranHutangProduk>();
        }

        [Display(Name = "pembayaran_hutang_produk_id")]
        public string pembayaran_hutang_produk_id { get; set; }

        [Display(Name = "Supplier")]
        public string supplier_id { get; set; }

        //[JsonIgnore]
        public Supplier Supplier { get; set; }

        [Display(Name = "pengguna_id")]
        public string pengguna_id { get; set; }

        [JsonIgnore]
        public Pengguna Pengguna { get; set; }

        [Display(Name = "Tanggal")]
        public Nullable<DateTime> tanggal { get; set; }

        [Display(Name = "Keterangan")]
        public string keterangan { get; set; }

        [JsonIgnore]
        [Display(Name = "tanggal_sistem")]
        public Nullable<DateTime> tanggal_sistem { get; set; }

        [Display(Name = "Nota")]
        public string nota { get; set; }

        [Display(Name = "is_tunai")]
        public bool is_tunai { get; set; }

        public double total_pembayaran { get; set; }

        public IList<ItemPembayaranHutangProduk> item_pembayaran_hutang { get; set; }

        public IList<ItemPembayaranHutangProduk> item_pembayaran_hutang_deleted { get; set; }
    }

    public class PembayaranHutangProdukDTOValidator : AbstractValidator<PembayaranHutangProdukDTO>
    {
        public PembayaranHutangProdukDTOValidator()
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
                RuleFor(c => c.pembayaran_hutang_produk_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
                DefaultRule(msgError1, msgError2);
            });

            RuleSet("delete", () =>
            {
                RuleFor(c => c.pembayaran_hutang_produk_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            });
        }

        private void DefaultRule(string msgError1, string msgError2)
        {
            RuleFor(c => c.supplier_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            RuleFor(c => c.pengguna_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            RuleFor(c => c.nota).NotEmpty().WithMessage(msgError1).Length(1, 20).WithMessage(msgError2);
            RuleFor(c => c.keterangan).Length(0, 100).WithMessage(msgError2);
        }
    }
}