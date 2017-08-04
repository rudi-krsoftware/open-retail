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

using log4net;
using Dapper;
using Dapper.Contrib.Extensions;

using OpenRetail.Model;
using OpenRetail.Repository.Api;
 
namespace OpenRetail.Repository.Service
{        
    public class JualProdukRepository : IJualProdukRepository
    {
        private const string SQL_TEMPLATE = @"SELECT t_jual_produk.jual_id, t_jual_produk.retur_jual_id, t_jual_produk.nota, t_jual_produk.tanggal, t_jual_produk.tanggal_tempo, 
                                              t_jual_produk.ppn, t_jual_produk.kurir, t_jual_produk.ongkos_kirim, t_jual_produk.diskon, t_jual_produk.total_nota, t_jual_produk.total_pelunasan, t_jual_produk.total_pelunasan AS total_pelunasan_old, t_jual_produk.keterangan, t_jual_produk.tanggal_sistem, 
                                              t_jual_produk.is_sdac, t_jual_produk.is_dropship, t_jual_produk.kirim_kepada, t_jual_produk.kirim_alamat, t_jual_produk.kirim_kecamatan, t_jual_produk.kirim_desa, t_jual_produk.kirim_kabupaten, t_jual_produk.kirim_kelurahan, t_jual_produk.kirim_kota, t_jual_produk.kirim_kode_pos, t_jual_produk.kirim_telepon,
                                              t_jual_produk.label_dari1, t_jual_produk.label_dari2, t_jual_produk.label_dari3, t_jual_produk.label_dari4,
                                              t_jual_produk.label_kepada1, t_jual_produk.label_kepada2, t_jual_produk.label_kepada3, t_jual_produk.label_kepada4,
                                              m_customer.customer_id, m_customer.nama_customer, m_customer.alamat, m_customer.kecamatan, m_customer.kelurahan, m_customer.desa, m_customer.kabupaten, m_customer.kota, m_customer.kode_pos, m_customer.telepon, m_customer.diskon, m_customer.plafon_piutang,
                                              m_pengguna.pengguna_id, m_pengguna.nama_pengguna
                                              FROM public.t_jual_produk LEFT JOIN public.m_customer ON t_jual_produk.customer_id = m_customer.customer_id
                                              LEFT JOIN m_pengguna ON m_pengguna.pengguna_id = t_jual_produk.pengguna_id
                                              {WHERE}
                                              {ORDER BY}";
        private IDapperContext _context;
        private ILog _log;
        private string _sql;
		
        public JualProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<JualProduk> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<JualProduk> oList = _context.db.Query<JualProduk, Customer, Pengguna, JualProduk>(sql, (j, c, p) =>
            {
                if (c != null)
                {
                    j.customer_id = c.customer_id; j.Customer = c;
                }

                if (p != null)
                {
                    j.pengguna_id = p.pengguna_id; j.Pengguna = p;
                }

                return j;
            }, param, splitOn: "customer_id, pengguna_id");

            return oList;
        }

        private IList<HargaGrosir> GetListHargaGrosir(string produkId)
        {
            IHargaGrosirRepository repo = new HargaGrosirRepository(_context, _log);

            return repo.GetListHargaGrosir(produkId);
        }

        public IList<ItemJualProduk> GetItemJual(string jualId)
        {
            IList<ItemJualProduk> oList = new List<ItemJualProduk>();

            try
            {
                var sql = @"SELECT t_item_jual_produk.item_jual_id, t_item_jual_produk.jual_id, t_item_jual_produk.pengguna_id, t_item_jual_produk.harga_beli, t_item_jual_produk.harga_jual, 
                            t_item_jual_produk.jumlah, t_item_jual_produk.jumlah_retur, t_item_jual_produk.diskon, t_item_jual_produk.tanggal_sistem, 1 as entity_state,
                            m_produk.produk_id, m_produk.kode_produk, m_produk.nama_produk, m_produk.satuan, m_produk.harga_beli, m_produk.harga_jual, m_produk.diskon
                            FROM public.t_item_jual_produk INNER JOIN public.m_produk ON t_item_jual_produk.produk_id = m_produk.produk_id
                            WHERE t_item_jual_produk.jual_id = @jualId
                            ORDER BY t_item_jual_produk.tanggal_sistem";

                oList = _context.db.Query<ItemJualProduk, Produk, ItemJualProduk>(sql, (ij, p) =>
                {
                    ij.produk_id = p.produk_id; ij.Produk = p;
                    return ij;
                }, new { jualId }, splitOn: "produk_id").ToList();

                foreach (var item in oList)
                {
                    item.Produk.list_of_harga_grosir = GetListHargaGrosir(item.produk_id);
                }
            }
            catch
            {
            }

            return oList;
        }

        public JualProduk GetByID(string id)
        {
            JualProduk obj = null;

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_jual_produk.jual_id = @id");
                _sql = _sql.Replace("{ORDER BY}", "");

                obj = MappingRecordToObject(_sql, new { id }).SingleOrDefault();

                if (obj != null)
                    // load item jual
                    obj.item_jual = GetItemJual(obj.jual_id);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<JualProduk> GetByName(string name)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE LOWER(m_customer.nama_customer) LIKE @name");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_jual_produk.tanggal, t_jual_produk.nota");

                name = "%" + name.ToLower() + "%";

                oList = MappingRecordToObject(_sql, new { name }).ToList();

                // load item jual
                foreach (var item in oList)
                {
                    item.item_jual = GetItemJual(item.jual_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<JualProduk> GetAll()
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_jual_produk.tanggal, t_jual_produk.nota");

                oList = MappingRecordToObject(_sql).ToList();

                // load item jual
                foreach (var item in oList)
                {
                    item.item_jual = GetItemJual(item.jual_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        private double GetTotalNota(JualProduk obj)
        {
            var total = obj.item_jual.Where(f => f.Produk != null && f.entity_state != EntityState.Deleted)
                                     .Sum(f => (f.jumlah - f.jumlah_retur) * (f.harga_jual - (f.diskon / 100 * f.harga_jual)));

            total = (int)total;
            return total;
        }

        public int Save(JualProduk obj)
        {
            var result = 0;

            try
            {
                _context.BeginTransaction();

                var transaction = _context.transaction;

                obj.jual_id = _context.GetGUID();
                obj.total_nota = GetTotalNota(obj);

                // insert header
                _context.db.Insert<JualProduk>(obj, transaction);

                // insert detail
                foreach (var item in obj.item_jual.Where(f => f.Produk != null))
                {
                    if (item.produk_id.Length > 0)
                    {
                        item.item_jual_id = _context.GetGUID();
                        item.jual_id = obj.jual_id;
                        item.pengguna_id = obj.pengguna_id;

                        _context.db.Insert<ItemJualProduk>(item, transaction);

                        // update entity state
                        item.entity_state = EntityState.Unchanged;
                    }
                }

                // jika pembelian tunai, langsung insert ke pembayaran hutang
                if (obj.tanggal_tempo.IsNull())
                {
                    result = SavePembayaranPiutang(obj);
                    if (result > 0)
                        obj.total_pelunasan = obj.grand_total;

                }

                _context.Commit();

                LogicalThreadContext.Properties["NewValue"] = obj.ToJson();
                _log.Info("Tambah data");

                result = 1;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        /// <summary>
        /// Method khusus untuk menyimpan pembayaran penjualan tunai
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private int SavePembayaranPiutang(JualProduk obj)
        {
            PembayaranPiutangProduk pembayaranPiutang;
            ItemPembayaranPiutangProduk itemPembayaranPiutang;
            IPembayaranPiutangProdukRepository pembayaranPiutangRepo = new PembayaranPiutangProdukRepository(_context, _log);

            var result = 0;

            // set detail            
            itemPembayaranPiutang = pembayaranPiutangRepo.GetByJualID(obj.jual_id);
            if (itemPembayaranPiutang != null) // sudah ada pelunasan
            {
                itemPembayaranPiutang.nominal = obj.grand_total; // GetTotalNotaSetelahDiskonDanPPN(obj);
                itemPembayaranPiutang.JualProduk = new JualProduk { jual_id = itemPembayaranPiutang.jual_id };
                itemPembayaranPiutang.entity_state = EntityState.Modified;

                // set header by detail
                pembayaranPiutang = itemPembayaranPiutang.PembayaranPiutangProduk;
                pembayaranPiutang.is_tunai = obj.is_tunai;

                // set item pembayaran
                pembayaranPiutang.item_pembayaran_piutang.Add(itemPembayaranPiutang);

                result = pembayaranPiutangRepo.Update(pembayaranPiutang, true);
            }
            else // belum ada pelunasan hutang
            {
                pembayaranPiutang = new PembayaranPiutangProduk();

                // set header
                pembayaranPiutang.customer_id = obj.customer_id;
                pembayaranPiutang.pengguna_id = obj.pengguna_id;
                pembayaranPiutang.tanggal = obj.tanggal;
                pembayaranPiutang.keterangan = "Penjualan tunai produk";
                pembayaranPiutang.is_tunai = obj.is_tunai;

                // set item
                itemPembayaranPiutang = new ItemPembayaranPiutangProduk();
                itemPembayaranPiutang.jual_id = obj.jual_id;
                itemPembayaranPiutang.JualProduk = obj;
                itemPembayaranPiutang.nominal = obj.grand_total;
                itemPembayaranPiutang.keterangan = string.Empty;

                // set item pembayaran
                pembayaranPiutang.item_pembayaran_piutang.Add(itemPembayaranPiutang);

                // simpan item pembayaran
                result = pembayaranPiutangRepo.Save(pembayaranPiutang, true);
            }

            return result;
        }

        /// <summary>
        /// Method untuk menghapus pembayaran piutang jika terjadi perubahan status nota dari tunai ke kredit
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private int HapusPembayaranPiutang(JualProduk obj)
        {
            PembayaranPiutangProduk pembayaranHutang;
            ItemPembayaranPiutangProduk itemPembayaranHutang;
            IPembayaranPiutangProdukRepository pembayaranHutangRepo = new PembayaranPiutangProdukRepository(_context, _log);

            var result = 0;

            // set detail
            itemPembayaranHutang = pembayaranHutangRepo.GetByJualID(obj.jual_id);
            if (itemPembayaranHutang != null)
            {
                pembayaranHutang = itemPembayaranHutang.PembayaranPiutangProduk;
                result = pembayaranHutangRepo.Delete(pembayaranHutang);
            }

            return result;
        }

        public int Update(JualProduk obj)
        {
            var result = 0;

            try
            {
                _context.BeginTransaction();

                var transaction = _context.transaction;

                obj.total_nota = GetTotalNota(obj);

                // update header
                result = _context.db.Update<JualProduk>(obj, transaction) ? 1 : 0;

                // delete detail
                foreach (var item in obj.item_jual_deleted)
                {
                    result = _context.db.Delete<ItemJualProduk>(item, transaction) ? 1 : 0;
                }

                // insert/update detail
                foreach (var item in obj.item_jual.Where(f => f.Produk != null))
                {
                    item.jual_id = obj.jual_id;
                    item.pengguna_id = obj.pengguna_id;

                    if (item.entity_state == EntityState.Added)
                    {
                        item.item_jual_id = _context.GetGUID();

                        _context.db.Insert<ItemJualProduk>(item, transaction);

                        result = 1;
                    }
                    else if (item.entity_state == EntityState.Modified)
                    {
                        result = _context.db.Update<ItemJualProduk>(item, transaction) ? 1 : 0;
                    }

                    // update entity state
                    item.entity_state = EntityState.Unchanged;
                }

                // jika terjadi perubahan status nota dari tunai ke kredit
                if (obj.tanggal_tempo_old.IsNull() && !obj.tanggal_tempo.IsNull())
                {
                    result = HapusPembayaranPiutang(obj);
                    if (result > 0)
                        obj.total_pelunasan = 0;
                }
                else if (obj.tanggal_tempo.IsNull()) // jika penjualan tunai, langsung update ke pembayaran piutang
                {
                    result = SavePembayaranPiutang(obj);
                    if (result > 0)
                        obj.total_pelunasan = obj.grand_total;
                }

                _context.Commit();

                LogicalThreadContext.Properties["NewValue"] = obj.ToJson();
                _log.Info("Update data");

                result = 1;
            }
            catch (Exception ex)
            {
                result = 0;
                _log.Error("Error:", ex);
            }

            return result;
        }        

        public int Delete(JualProduk obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<JualProduk>(obj) ? 1 : 0;

                if (result > 0)
                {
                    LogicalThreadContext.Properties["OldValue"] = obj.ToJson();
                    _log.Info("Hapus data");
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public string GetLastNota()
        {
            return _context.GetLastNota(new JualProduk().GetTableName());
        }

        public IList<JualProduk> GetAll(string name)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE LOWER(m_customer.nama_customer) LIKE @name");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_jual_produk.tanggal, t_jual_produk.nota");

                name = "%" + name.ToLower() + "%";

                oList = MappingRecordToObject(_sql, new { name }).ToList();

                // load item beli
                foreach (var item in oList)
                {
                    item.item_jual = GetItemJual(item.jual_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }
            
            return oList;
        }

        public IList<JualProduk> GetNotaCustomer(string id, string nota)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                object param = null;

                if (nota.Length > 0)
                {
                    nota = nota.ToLower() + "%";
                    param = new { id, nota };

                    _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_customer.customer_id = @id AND LOWER(t_jual_produk.nota) LIKE @nota");
                }
                else
                {
                    param = new { id };

                    _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_customer.customer_id = @id");
                }

                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_jual_produk.tanggal, t_jual_produk.nota");
                oList = MappingRecordToObject(_sql, param).ToList();

                // load item jual
                foreach (var item in oList)
                {
                    item.item_jual = GetItemJual(item.jual_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<JualProduk> GetNotaKreditByCustomer(string id, bool isLunas)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                if (isLunas)
                {
                    _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_customer.customer_id = @id AND t_jual_produk.tanggal_tempo IS NOT NULL AND (t_jual_produk.total_nota - t_jual_produk.diskon + t_jual_produk.ongkos_kirim + t_jual_produk.ppn) <= t_jual_produk.total_pelunasan");
                }
                else
                {
                    _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_customer.customer_id = @id AND t_jual_produk.tanggal_tempo IS NOT NULL AND (t_jual_produk.total_nota - t_jual_produk.diskon + t_jual_produk.ongkos_kirim + t_jual_produk.ppn) > t_jual_produk.total_pelunasan");
                }

                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_jual_produk.tanggal, t_jual_produk.nota");


                oList = MappingRecordToObject(_sql, new { id }).ToList();

                // load item jual, aktifkan perintah berikut
                //foreach (var item in oList)
                //{
                //    item.item_jual = GetItemJual(item.jual_id);
                //}
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<JualProduk> GetNotaKreditByNota(string id, string nota)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_customer.customer_id = @id AND LOWER(t_jual_produk.nota) LIKE @nota AND t_jual_produk.tanggal_tempo IS NOT NULL AND (t_jual_produk.total_nota - t_jual_produk.diskon + t_jual_produk.ongkos_kirim + t_jual_produk.ppn) > t_jual_produk.total_pelunasan");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_jual_produk.tanggal, t_jual_produk.nota");

                nota = nota.ToLower() + "%";
                oList = MappingRecordToObject(_sql, new { id, nota }).ToList();

                // load item jual, aktifkan perintah berikut
                //foreach (var item in oList)
                //{
                //    item.item_jual = GetItemJual(item.jual_id);
                //}
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<JualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_jual_produk.tanggal, t_jual_produk.nota");

                oList = MappingRecordToObject(_sql, new { tanggalMulai, tanggalSelesai }).ToList();

                // load item jual
                foreach (var item in oList)
                {
                    item.item_jual = GetItemJual(item.jual_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<JualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, string name)
        {
            IList<JualProduk> oList = new List<JualProduk>();

            try
            {
                name = "%" + name.ToLower() + "%";

                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai AND LOWER(m_customer.nama_customer) LIKE @name");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_jual_produk.tanggal, t_jual_produk.nota");

                oList = MappingRecordToObject(_sql, new { tanggalMulai, tanggalSelesai, name }).ToList();

                // load item jual
                foreach (var item in oList)
                {
                    item.item_jual = GetItemJual(item.jual_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }        
    }
}     
