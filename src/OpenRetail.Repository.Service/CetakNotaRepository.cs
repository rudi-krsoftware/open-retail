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
using Dapper;
using OpenRetail.Model.Nota;
using OpenRetail.Repository.Api;

namespace OpenRetail.Repository.Service
{
    public class CetakNotaRepository : ICetakNotaRepository
    {
        private IDapperContext _context;
        private ILog _log;
        
        public CetakNotaRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<NotaPembelian> GetNotaPembelian(string beliProdukId)
        {
            IList<NotaPembelian> oList = new List<NotaPembelian>();

            try
            {
                var sql = @"SELECT m_supplier.nama_supplier, m_supplier.alamat, m_supplier.kontak, m_supplier.telepon, 
                            t_beli_produk.nota, t_beli_produk.tanggal, t_beli_produk.tanggal_tempo, t_beli_produk.ppn, t_beli_produk.diskon AS diskon_nota, t_beli_produk.total_nota,
                            m_produk.kode_produk, m_produk.nama_produk, m_produk.satuan,
                            t_item_beli_produk.harga, t_item_beli_produk.jumlah, t_item_beli_produk.jumlah_retur, t_item_beli_produk.diskon
                            FROM public.t_beli_produk INNER JOIN public.t_item_beli_produk ON t_item_beli_produk.beli_produk_id = t_beli_produk.beli_produk_id
                            INNER JOIN public.m_produk ON t_item_beli_produk.produk_id = m_produk.produk_id
                            INNER JOIN public.m_supplier ON t_beli_produk.supplier_id = m_supplier.supplier_id
                            WHERE t_beli_produk.beli_produk_id = @beliProdukId
                            ORDER BY t_item_beli_produk.tanggal_sistem";

                oList = _context.db.Query<NotaPembelian>(sql, new { beliProdukId }).ToList();

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<NotaPenjualan> GetNotaPenjualan(string jualProdukId)
        {
            IList<NotaPenjualan> oList = new List<NotaPenjualan>();

            try
            {
                var sql = @"SELECT m_customer.nama_customer, m_customer.alamat, m_customer.kecamatan, m_customer.kelurahan, m_customer.desa, m_customer.kabupaten, m_customer.kota, m_customer.kode_pos, m_customer.kontak, m_customer.telepon, 
                            t_jual_produk.nota, t_jual_produk.tanggal, t_jual_produk.tanggal_tempo, t_jual_produk.ppn, t_jual_produk.kurir, t_jual_produk.ongkos_kirim, t_jual_produk.diskon AS diskon_nota, t_jual_produk.total_nota,
                            t_jual_produk.is_sdac, t_jual_produk.is_dropship, t_jual_produk.kirim_kepada, t_jual_produk.kirim_alamat, t_jual_produk.kirim_kecamatan, t_jual_produk.kirim_desa, t_jual_produk.kirim_kabupaten, t_jual_produk.kirim_kelurahan, t_jual_produk.kirim_kota, t_jual_produk.kirim_kode_pos, t_jual_produk.kirim_telepon,
                            t_jual_produk.label_dari1, t_jual_produk.label_dari2, t_jual_produk.label_dari3, t_jual_produk.label_dari4,
                            t_jual_produk.label_kepada1, t_jual_produk.label_kepada2, t_jual_produk.label_kepada3, t_jual_produk.label_kepada4,
                            m_produk.kode_produk, m_produk.nama_produk, m_produk.satuan,
                            t_item_jual_produk.harga_jual AS harga, t_item_jual_produk.jumlah, t_item_jual_produk.jumlah_retur, t_item_jual_produk.diskon
                            FROM public.t_jual_produk INNER JOIN public.t_item_jual_produk ON t_item_jual_produk.jual_id = t_jual_produk.jual_id
                            INNER JOIN public.m_produk ON t_item_jual_produk.produk_id = m_produk.produk_id
                            INNER JOIN public.m_customer ON t_jual_produk.customer_id = m_customer.customer_id
                            WHERE t_jual_produk.jual_id = @jualProdukId
                            ORDER BY t_item_jual_produk.tanggal_sistem";

                oList = _context.db.Query<NotaPenjualan>(sql, new { jualProdukId }).ToList();

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }
    }
}
