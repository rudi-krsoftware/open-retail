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

using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace OpenRetail.Model
{
    public class LabelAlamatKirim
    {
        [Display(Name = "Dari #1")]
        public string dari1 { get; set; }

        [Display(Name = "Dari #2")]
        public string dari2 { get; set; }

        [Display(Name = "Dari #3")]
        public string dari3 { get; set; }

        [Display(Name = "Dari #4")]
        public string dari4 { get; set; }

        [Display(Name = "Kepada #1")]
        public string kepada1 { get; set; }

        [Display(Name = "Kepada #2")]
        public string kepada2 { get; set; }

        [Display(Name = "Kepada #3")]
        public string kepada3 { get; set; }

        [Display(Name = "Kepada #4")]
        public string kepada4 { get; set; }
    }

    public class LabelAlamatKirimValidator : AbstractValidator<LabelAlamatKirim>
    {
        public LabelAlamatKirimValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

            var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

            RuleFor(c => c.dari1).NotEmpty().WithMessage(msgError1).Length(1, 100).WithMessage(msgError2);
            RuleFor(c => c.dari2).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.dari3).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.dari4).Length(0, 100).WithMessage(msgError2);

            RuleFor(c => c.kepada1).NotEmpty().WithMessage(msgError1).Length(1, 250).WithMessage(msgError2);
            RuleFor(c => c.kepada2).NotEmpty().WithMessage(msgError1).Length(1, 250).WithMessage(msgError2);
            RuleFor(c => c.kepada3).Length(0, 250).WithMessage(msgError2);
            RuleFor(c => c.kepada4).Length(0, 250).WithMessage(msgError2);
        }
    }
}
