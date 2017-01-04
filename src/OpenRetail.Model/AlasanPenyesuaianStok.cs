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
	[Table("m_alasan_penyesuaian_stok")]
    public class AlasanPenyesuaianStok
    {
		[Display(Name = "alasan_penyesuaian_stok_id")]
		[ExplicitKey]
		public string alasan_penyesuaian_stok_id { get; set; }
		
		[Display(Name = "Alasan")]
		public string alasan { get; set; }
		
		[Write(false)]
		public string table_name { get { return "m_alasan_penyesuaian_stok"; } }
	}

    public class AlasanPenyesuaianStokValidator : AbstractValidator<AlasanPenyesuaianStok>
    {
        public AlasanPenyesuaianStokValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

			var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

			RuleFor(c => c.alasan).NotEmpty().WithMessage(msgError1).Length(1, 100).WithMessage(msgError2);
		}
	}
}
