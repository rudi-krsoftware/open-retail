using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace OpenRetail.WebAPI.Models.DTO
{
    public class GolonganDTO
    {
        [Display(Name = "golongan_id")]
        public string golongan_id { get; set; }

        [Display(Name = "Golongan")]
        public string nama_golongan { get; set; }

        [Display(Name = "Diskon")]
        public double diskon { get; set; }
    }

    public class GolonganDTOValidator : AbstractValidator<GolonganDTO>
    {
        public GolonganDTOValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

            var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";            

            RuleSet("save", () =>
            {
                RuleFor(c => c.nama_golongan).NotEmpty().WithMessage(msgError1).Length(1, 50).WithMessage(msgError2);
            });

            RuleSet("update", () =>
            {
                RuleFor(c => c.golongan_id).NotEmpty().WithMessage(msgError1);
                RuleFor(c => c.nama_golongan).NotEmpty().WithMessage(msgError1).Length(1, 50).WithMessage(msgError2);
            });

            RuleSet("delete", () =>
            {
                RuleFor(c => c.golongan_id).NotEmpty().WithMessage(msgError1);
            });
        }
    }
}