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
using System;
using System.ComponentModel.DataAnnotations;

namespace OpenRetail.WebAPI.Models.DTO
{
    public class PenyesuaianStokDTO
    {
        [Display(Name = "penyesuaian_stok_id")]
        public string penyesuaian_stok_id { get; set; }

        [Display(Name = "produk_id")]
        public string produk_id { get; set; }

        [JsonIgnore]
        public ProdukDTO Produk { get; set; }

        [Display(Name = "alasan_penyesuaian_id")]
        public string alasan_penyesuaian_id { get; set; }

        [JsonIgnore]
        public AlasanPenyesuaianStokDTO AlasanPenyesuaianStok { get; set; }

        [Display(Name = "tanggal")]
        public Nullable<DateTime> tanggal { get; set; }

        [Display(Name = "penambahan_stok")]
        public double penambahan_stok { get; set; }

        [Display(Name = "pengurangan_stok")]
        public double pengurangan_stok { get; set; }

        [Display(Name = "keterangan")]
        public string keterangan { get; set; }

        [Display(Name = "tanggal_sistem")]
        public Nullable<DateTime> tanggal_sistem { get; set; }

        [Display(Name = "penambahan_stok_gudang")]
        public double penambahan_stok_gudang { get; set; }

        [Display(Name = "pengurangan_stok_gudang")]
        public double pengurangan_stok_gudang { get; set; }
    }

    public class PenyesuaianStokDTOValidator : AbstractValidator<PenyesuaianStokDTO>
    {
        public PenyesuaianStokDTOValidator()
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
                RuleFor(c => c.penyesuaian_stok_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
                DefaultRule(msgError1, msgError2);
            });

            RuleSet("delete", () =>
            {
                RuleFor(c => c.penyesuaian_stok_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            });
        }

        private void DefaultRule(string msgError1, string msgError2)
        {
            RuleFor(c => c.produk_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            RuleFor(c => c.alasan_penyesuaian_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            RuleFor(c => c.keterangan).Length(0, 100).WithMessage(msgError2);
        }
    }
}