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
    public enum JenisGajian
    {
        Mingguan, Bulanan
    }

    [Table("m_karyawan")]
    public class Karyawan
    {
        [ExplicitKey]
        [Display(Name = "karyawan_id")]
        public string karyawan_id { get; set; }

        [Display(Name = "Jabatan")]
        public string jabatan_id { get; set; }

        [Write(false)]
        public Jabatan Jabatan { get; set; }

        [Display(Name = "Nama")]
        public string nama_karyawan { get; set; }

        [Display(Name = "Alamat")]
        public string alamat { get; set; }

        [Display(Name = "Telepon")]
        public string telepon { get; set; }

        [Display(Name = "Gaji Pokok")]
        public double gaji_pokok { get; set; }

        [Display(Name = "is_active")]
        public bool is_active { get; set; }

        [Write(false)]
        [Display(Name = "Keterangan")]
        public string keterangan { get; set; }

        [Display(Name = "Jenis Gajian")]
        public JenisGajian jenis_gajian { get; set; }

        [Display(Name = "Gaji Lembur")]
        public double gaji_lembur { get; set; }

        [Computed]
        [Display(Name = "total_kasbon")]
        public double total_kasbon { get; set; }

        [Computed]
        [Display(Name = "total_pembayaran_kasbon")]
        public double total_pembayaran_kasbon { get; set; }

        [Computed]
        [Display(Name = "sisa_kasbon")]
        public double sisa_kasbon
        {
            get { return total_kasbon - total_pembayaran_kasbon; }
        }
    }

    public class KaryawanValidator : AbstractValidator<Karyawan>
    {
        public KaryawanValidator()
        {
            CascadeMode = FluentValidation.CascadeMode.StopOnFirstFailure;

            var msgError1 = "'{PropertyName}' tidak boleh kosong !";
            var msgError2 = "Inputan '{PropertyName}' maksimal {MaxLength} karakter !";

            RuleFor(c => c.jabatan_id).NotEmpty().WithMessage(msgError1).Length(1, 36).WithMessage(msgError2);
            RuleFor(c => c.nama_karyawan).NotEmpty().WithMessage(msgError1).Length(1, 50).WithMessage(msgError2);
            RuleFor(c => c.alamat).Length(0, 100).WithMessage(msgError2);
            RuleFor(c => c.telepon).Length(0, 20).WithMessage(msgError2);
            RuleFor(c => c.keterangan).Length(0, 100).WithMessage(msgError2);
        }
    }
}