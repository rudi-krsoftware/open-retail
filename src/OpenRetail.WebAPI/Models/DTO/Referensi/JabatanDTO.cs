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

namespace OpenRetail.WebAPI.Models.DTO
{        
    public class JabatanDTO
    {
		[Display(Name = "jabatan_id")]		
		public string jabatan_id { get; set; }
		
		[Display(Name = "nama_jabatan")]
		public string nama_jabatan { get; set; }
		
		[Display(Name = "keterangan")]
		public string keterangan { get; set; }
	}

    public class JabatanDTOValidator : AbstractValidator<JabatanDTO>
    {
        public JabatanDTOValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "'{PropertyName}' maksimal {MaxLength} karakter !";

			RuleSet("save", () =>
            {
                RuleFor(c => c.nama_jabatan).NotEmpty().WithMessage(msgError1).Length(1, 50).WithMessage(msgError2);
                RuleFor(c => c.keterangan).Length(0, 100).WithMessage(msgError2);			
            });

            RuleSet("update", () =>
            {
                RuleFor(c => c.jabatan_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);						
                RuleFor(c => c.nama_jabatan).NotEmpty().WithMessage(msgError1).Length(1, 50).WithMessage(msgError2);
                RuleFor(c => c.keterangan).Length(0, 100).WithMessage(msgError2);			
            });

            RuleSet("delete", () =>
            {
                RuleFor(c => c.jabatan_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);						
            });
		}
	}
}
