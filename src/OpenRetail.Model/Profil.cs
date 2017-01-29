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

namespace OpenRetail.Model
{        
	[Table("m_profil")]
    public class Profil
    {
		[ExplicitKey]
		[Display(Name = "profil_id")]		
		public string profil_id { get; set; }
		
		[Display(Name = "Nama Perusahaan")]
		public string nama_profil { get; set; }
		
		[Display(Name = "Alamat")]
		public string alamat { get; set; }
		
		[Display(Name = "Kota")]
		public string kota { get; set; }
		
		[Display(Name = "Telepon")]
		public string telepon { get; set; }
		
	}

    public class ProfilValidator : AbstractValidator<Profil>
    {
        public ProfilValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

			RuleFor(c => c.nama_profil).NotEmpty().WithMessage(msgError1).Length(1, 100).WithMessage(msgError2);
			RuleFor(c => c.alamat).NotEmpty().WithMessage(msgError1).Length(1, 100).WithMessage(msgError2);
			RuleFor(c => c.kota).Length(0, 100).WithMessage(msgError2);
			RuleFor(c => c.telepon).Length(0, 20).WithMessage(msgError2);
		}
	}
}
