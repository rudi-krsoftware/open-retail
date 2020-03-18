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
using System.ComponentModel.DataAnnotations;

namespace OpenRetail.WebAPI.Models.DTO
{
    public class KecamatanDTO
    {
        [Display(Name = "kecamatan_id")]
        public string kecamatan_id { get; set; }

        [Display(Name = "kabupaten_id")]
        public string kabupaten_id { get; set; }

        [JsonIgnore]
        public KabupatenDTO Kabupaten { get; set; }

        [Display(Name = "nama_kecamatan")]
        public string nama_kecamatan { get; set; }
    }

    public class KecamatanDTOValidator : AbstractValidator<KecamatanDTO>
    {
        public KecamatanDTOValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

            var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "'{PropertyName}' maksimal {MaxLength} karakter !";

            RuleSet("save", () =>
            {
                RuleFor(c => c.kabupaten_id).NotEmpty().WithMessage(msgError1).Length(1, 4).WithMessage(msgError2);
                RuleFor(c => c.nama_kecamatan).NotEmpty().WithMessage(msgError1).Length(1, 250).WithMessage(msgError2);
            });

            RuleSet("update", () =>
            {
                RuleFor(c => c.kecamatan_id).NotEmpty().WithMessage(msgError1).Length(1, 7).WithMessage(msgError2);
                RuleFor(c => c.kabupaten_id).NotEmpty().WithMessage(msgError1).Length(1, 4).WithMessage(msgError2);
                RuleFor(c => c.nama_kecamatan).NotEmpty().WithMessage(msgError1).Length(1, 250).WithMessage(msgError2);
            });

            RuleSet("delete", () =>
            {
                RuleFor(c => c.kabupaten_id).NotEmpty().WithMessage(msgError1).Length(1, 4).WithMessage(msgError2);
            });
        }
    }
}