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
	[Table("m_kabupaten")]
    public class Kabupaten
    {
		[ExplicitKey]
		[Display(Name = "kabupaten_id")]		
		public int kabupaten_id { get; set; }
		
		[Display(Name = "provinsi_id")]
		public int provinsi_id { get; set; }

		[Write(false)]
        public Provinsi Provinsi { get; set; }

		[Display(Name = "tipe")]
		public string tipe { get; set; }
		
		[Display(Name = "nama_kabupaten")]
		public string nama_kabupaten { get; set; }
		
		[Display(Name = "kode_pos")]
		public string kode_pos { get; set; }
	}

    public class KabupatenValidator : AbstractValidator<Kabupaten>
    {
        public KabupatenValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

			RuleFor(c => c.tipe).NotEmpty().WithMessage(msgError1).Length(1, 15).WithMessage(msgError2);
			RuleFor(c => c.nama_kabupaten).NotEmpty().WithMessage(msgError1).Length(1, 100).WithMessage(msgError2);
			RuleFor(c => c.kode_pos).NotEmpty().WithMessage(msgError1).Length(0, 6).WithMessage(msgError2);
		}
	}
}
