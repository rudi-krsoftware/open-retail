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

using log4net;
using OpenRetail.Model.Nota;
using OpenRetail.Bll.Api;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;

namespace OpenRetail.Bll.Service
{
    public class CetakNotaBll : ICetakNotaBll
    {
        private ILog _log;

        public CetakNotaBll(ILog log)
        {
            _log = log;
        }

        public IList<NotaPembelian> GetNotaPembelian(string beliProdukId)
        {
            IList<NotaPembelian> oList = new List<NotaPembelian>();

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.CetakNotaRepository.GetNotaPembelian(beliProdukId);
            }

            return oList;
        }

        public IList<NotaPenjualan> GetNotaPenjualan(string jualProdukId)
        {
            IList<NotaPenjualan> oList = new List<NotaPenjualan>();

            using (IDapperContext context = new DapperContext())
            {
                IUnitOfWork uow = new UnitOfWork(context, _log);
                oList = uow.CetakNotaRepository.GetNotaPenjualan(jualProdukId);
            }

            foreach (var item in oList)
            {
                item.kecamatan = string.IsNullOrEmpty(item.kecamatan) ? "" : item.kecamatan;
                item.kelurahan = string.IsNullOrEmpty(item.kelurahan) ? "" : item.kelurahan;
                item.kota = string.IsNullOrEmpty(item.kota) ? "" : item.kota;
                item.kode_pos = (string.IsNullOrEmpty(item.kode_pos) || item.kode_pos == "0") ? "" : item.kode_pos;
                item.telepon = string.IsNullOrEmpty(item.telepon) ? "" : item.telepon;

                item.kirim_desa = string.IsNullOrEmpty(item.kirim_desa) ? "" : item.kirim_desa;
                item.kirim_kabupaten = string.IsNullOrEmpty(item.kirim_kabupaten) ? "" : item.kirim_kabupaten;

                item.kirim_kecamatan = string.IsNullOrEmpty(item.kirim_kecamatan) ? "" : item.kirim_kecamatan;
                item.kirim_kelurahan = string.IsNullOrEmpty(item.kirim_kelurahan) ? "" : item.kirim_kelurahan;
                item.kirim_kota = string.IsNullOrEmpty(item.kirim_kota) ? "" : item.kirim_kota;
                item.kirim_kode_pos = string.IsNullOrEmpty(item.kirim_kode_pos) ? "" : item.kirim_kode_pos;
                item.kirim_telepon = string.IsNullOrEmpty(item.kirim_telepon) ? "" : item.kirim_telepon;

                item.label_dari1 = string.IsNullOrEmpty(item.label_dari1) ? "" : item.label_dari1;
                item.label_dari2 = string.IsNullOrEmpty(item.label_dari2) ? "" : item.label_dari2;

                item.label_kepada1 = string.IsNullOrEmpty(item.label_kepada1) ? item.nama_customer : item.label_kepada1;
                item.label_kepada2 = string.IsNullOrEmpty(item.label_kepada2) ? item.alamat : item.label_kepada2;
                item.label_kepada3 = string.IsNullOrEmpty(item.label_kepada3) ? "HP: " + item.telepon : item.label_kepada3;
                item.label_kepada4 = string.IsNullOrEmpty(item.label_kepada4) ? string.Empty : item.label_kepada4;
            }

            return oList;
        }
    }
}
