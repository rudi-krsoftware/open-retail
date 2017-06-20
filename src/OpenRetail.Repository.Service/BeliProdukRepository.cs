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
    public class BeliProdukRepository : IBeliProdukRepository
    {
        private const string SQL_TEMPLATE = @"SELECT t_beli_produk.beli_produk_id, t_beli_produk.pengguna_id, t_beli_produk.retur_beli_produk_id, t_beli_produk.nota, t_beli_produk.tanggal, 
                                              t_beli_produk.tanggal_tempo, t_beli_produk.ppn, t_beli_produk.diskon, t_beli_produk.total_nota, t_beli_produk.total_pelunasan, 
                                              t_beli_produk.total_pelunasan AS total_pelunasan_old, t_beli_produk.keterangan, t_beli_produk.tanggal_sistem, 
                                              m_supplier.supplier_id, m_supplier.nama_supplier, m_supplier.alamat
                                              FROM public.t_beli_produk INNER JOIN public.m_supplier ON t_beli_produk.supplier_id = m_supplier.supplier_id
                                              {WHERE}
                                              {ORDER BY}";
        private IDapperContext _context;
        private ILog _log;
        private string _sql;

        public BeliProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<BeliProduk> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<BeliProduk> oList = _context.db.Query<BeliProduk, Supplier, BeliProduk>(sql, (bl, sup) =>
            {
                bl.supplier_id = sup.supplier_id; bl.Supplier = sup;
                return bl;
            }, param, splitOn: "supplier_id");

            return oList;
        }

        public IList<ItemBeliProduk> GetItemBeli(string beliId)
        {
            IList<ItemBeliProduk> oList = new List<ItemBeliProduk>();

            try
            {
                var sql = @"SELECT t_item_beli_produk.item_beli_produk_id, t_item_beli_produk.beli_produk_id, t_item_beli_produk.pengguna_id, t_item_beli_produk.harga, 
                            t_item_beli_produk.jumlah, t_item_beli_produk.jumlah_retur, t_item_beli_produk.diskon, t_item_beli_produk.tanggal_sistem, 1 as entity_state,
                            m_produk.produk_id, m_produk.kode_produk, m_produk.nama_produk, m_produk.satuan, m_produk.harga_beli, m_produk.harga_jual, m_produk.diskon
                            FROM public.t_item_beli_produk INNER JOIN public.m_produk ON t_item_beli_produk.produk_id = m_produk.produk_id
                            WHERE t_item_beli_produk.beli_produk_id = @beliId
                            ORDER BY t_item_beli_produk.tanggal_sistem";

                oList = _context.db.Query<ItemBeliProduk, Produk, ItemBeliProduk>(sql, (ib, p) =>
                {
                    ib.produk_id = p.produk_id; ib.Produk = p;
                    return ib;
                }, new { beliId }, splitOn: "produk_id").ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public BeliProduk GetByID(string id)
        {
            BeliProduk obj = null;

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_beli_produk.beli_produk_id = @id");
                _sql = _sql.Replace("{ORDER BY}", "");

                obj = MappingRecordToObject(_sql, new { id }).SingleOrDefault();

                if (obj != null)
                    // load item beli
                    obj.item_beli = GetItemBeli(obj.beli_produk_id);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<BeliProduk> GetByName(string name)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE LOWER(m_supplier.nama_supplier) LIKE @name");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota");

                name = "%" + name.ToLower() + "%";

                oList = MappingRecordToObject(_sql, new { name }).ToList();

                // load item beli
                foreach (var item in oList)
                {
                    item.item_beli = GetItemBeli(item.beli_produk_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetAll()
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota");

                oList = MappingRecordToObject(_sql).ToList();

                // load item beli
                foreach (var item in oList)
                {
                    item.item_beli = GetItemBeli(item.beli_produk_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetAll(string name)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE LOWER(m_supplier.nama_supplier) LIKE @name");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota");

                name = "%" + name.ToLower() + "%";

                oList = MappingRecordToObject(_sql, new { name }).ToList();

                // load item beli
                foreach (var item in oList)
                {
                    item.item_beli = GetItemBeli(item.beli_produk_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public string GetLastNota()
        {
            return _context.GetLastNota(new BeliProduk().GetTableName());
        }

        public IList<BeliProduk> GetNotaSupplier(string id, string nota)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                object param = null;

                if (nota.Length > 0)
                {
                    nota = nota.ToLower() + "%";
                    param = new { id, nota };

                    _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_supplier.supplier_id = @id AND LOWER(t_beli_produk.nota) LIKE @nota");
                }
                else
                {
                    param = new { id };

                    _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_supplier.supplier_id = @id");                    
                }

                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota");
                oList = MappingRecordToObject(_sql, param).ToList();

                // load item beli
                foreach (var item in oList)
                {
                    item.item_beli = GetItemBeli(item.beli_produk_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetNotaKreditBySupplier(string id, bool isLunas)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                if (isLunas)
                {
                    _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_supplier.supplier_id = @id AND t_beli_produk.tanggal_tempo IS NOT NULL AND (t_beli_produk.total_nota - t_beli_produk.diskon + t_beli_produk.ppn) <= t_beli_produk.total_pelunasan");                    
                }
                else
                {
                    _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_supplier.supplier_id = @id AND t_beli_produk.tanggal_tempo IS NOT NULL AND (t_beli_produk.total_nota - t_beli_produk.diskon + t_beli_produk.ppn) > t_beli_produk.total_pelunasan");
                }

                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota");

                
                oList = MappingRecordToObject(_sql, new { id }).ToList();

                // load item beli, aktifkan perintah berikut
                //foreach (var item in oList)
                //{
                //    item.item_beli = GetItemBeli(item.beli_produk_id);
                //}
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetNotaKreditByNota(string id, string nota)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_supplier.supplier_id = @id AND LOWER(t_beli_produk.nota) LIKE @nota AND t_beli_produk.tanggal_tempo IS NOT NULL AND (t_beli_produk.total_nota - t_beli_produk.diskon + t_beli_produk.ppn) > t_beli_produk.total_pelunasan");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota");

                nota = nota.ToLower() + "%";
                oList = MappingRecordToObject(_sql, new { id, nota }).ToList();

                // load item beli, aktifkan perintah berikut
                //foreach (var item in oList)
                //{
                //    item.item_beli = GetItemBeli(item.beli_produk_id);
                //}
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota");

                oList = MappingRecordToObject(_sql, new { tanggalMulai, tanggalSelesai }).ToList();

                // load item beli
                foreach (var item in oList)
                {
                    item.item_beli = GetItemBeli(item.beli_produk_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<BeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai, string name)
        {
            IList<BeliProduk> oList = new List<BeliProduk>();

            try
            {
                name = "%" + name.ToLower() + "%";

                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai AND LOWER(m_supplier.nama_supplier) LIKE @name");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_beli_produk.tanggal, t_beli_produk.nota");

                oList = MappingRecordToObject(_sql, new { tanggalMulai, tanggalSelesai, name }).ToList();

                // load item beli
                foreach (var item in oList)
                {
                    item.item_beli = GetItemBeli(item.beli_produk_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        private double GetTotalNota(BeliProduk obj)
        {
            var total = obj.item_beli.Where(f => f.Produk != null && f.entity_state != EntityState.Deleted)
                                     .Sum(f => (f.jumlah - f.jumlah_retur) * (f.harga - (f.diskon / 100 * f.harga)));

            total = (int)total;
            return total;
        }

        public int Save(BeliProduk obj)
        {
            var result = 0;

            try
            {
                _context.BeginTransaction();

                var transaction = _context.transaction;

                obj.beli_produk_id = _context.GetGUID();
                obj.total_nota = GetTotalNota(obj);

                // insert header
                _context.db.Insert<BeliProduk>(obj, transaction);

                // insert detail
                foreach (var item in obj.item_beli.Where(f => f.Produk != null))
                {
                    if (item.produk_id.Length > 0)
                    {
                        item.item_beli_produk_id = _context.GetGUID();
                        item.beli_produk_id = obj.beli_produk_id;
                        item.pengguna_id = obj.pengguna_id;

                        _context.db.Insert<ItemBeliProduk>(item, transaction);

                        // update entity state
                        item.entity_state = EntityState.Unchanged;
                    }
                }
                
                // jika pembelian tunai, langsung insert ke pembayaran hutang
                if (obj.tanggal_tempo.IsNull())
                {
                    result = SavePembayaranHutang(obj);
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

        public int Update(BeliProduk obj)
        {
            var result = 0;

            try
            {
                _context.BeginTransaction();

                var transaction = _context.transaction;

                obj.total_nota = GetTotalNota(obj);

                // update header
                result = _context.db.Update<BeliProduk>(obj, transaction) ? 1 : 0;

                // delete detail
                foreach (var item in obj.item_beli_deleted)
                {
                    result = _context.db.Delete<ItemBeliProduk>(item, transaction) ? 1 : 0;
                }

                // insert/update detail
                foreach (var item in obj.item_beli.Where(f => f.Produk != null))
                {
                    item.beli_produk_id = obj.beli_produk_id;
                    item.pengguna_id = obj.pengguna_id;

                    if (item.entity_state == EntityState.Added)
                    {
                        item.item_beli_produk_id = _context.GetGUID();

                        _context.db.Insert<ItemBeliProduk>(item, transaction);

                        result = 1;
                    }
                    else if (item.entity_state == EntityState.Modified)
                    {
                        result = _context.db.Update<ItemBeliProduk>(item, transaction) ? 1 : 0;
                    }

                    // update entity state
                    item.entity_state = EntityState.Unchanged;
                }

                // jika terjadi perubahan status nota dari tunai ke kredit
                if (obj.tanggal_tempo_old.IsNull() && !obj.tanggal_tempo.IsNull())
                {
                    result = HapusPembayaranHutang(obj);
                    if (result > 0)
                        obj.total_pelunasan = 0;
                }
                else if (obj.tanggal_tempo.IsNull()) // jika pembelian tunai, langsung update ke pembayaran hutang
                {
                    result = SavePembayaranHutang(obj);
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
                _log.Error("Error:", ex);
            }

            return result;
        }

        /// <summary>
        /// Method khusus untuk menyimpan pembayaran hutang pembelian tunai
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private int SavePembayaranHutang(BeliProduk obj)
        {
            PembayaranHutangProduk pembayaranHutang;
            ItemPembayaranHutangProduk itemPembayaranHutang;
            IPembayaranHutangProdukRepository pembayaranHutangRepo = new PembayaranHutangProdukRepository(_context, _log);

            var result = 0;

            // set detail
            itemPembayaranHutang = pembayaranHutangRepo.GetByBeliID(obj.beli_produk_id);
            if (itemPembayaranHutang != null) // sudah ada pelunasan
            {
                itemPembayaranHutang.nominal = obj.grand_total;
                itemPembayaranHutang.BeliProduk = new BeliProduk { beli_produk_id = itemPembayaranHutang.beli_produk_id };
                itemPembayaranHutang.entity_state = EntityState.Modified;

                // set header by detail
                pembayaranHutang = itemPembayaranHutang.PembayaranHutangProduk;
                pembayaranHutang.is_tunai = obj.is_tunai;

                // set item pembayaran
                pembayaranHutang.item_pembayaran_hutang.Add(itemPembayaranHutang);

                result = pembayaranHutangRepo.Update(pembayaranHutang, true);
            }
            else // belum ada pelunasan hutang
            {
                pembayaranHutang = new PembayaranHutangProduk();

                // set header
                pembayaranHutang.supplier_id = obj.supplier_id;
                pembayaranHutang.pengguna_id = obj.pengguna_id;
                pembayaranHutang.tanggal = obj.tanggal;
                pembayaranHutang.keterangan = "Pembelian tunai produk";
                pembayaranHutang.is_tunai = obj.is_tunai;

                // set item
                itemPembayaranHutang = new ItemPembayaranHutangProduk();
                itemPembayaranHutang.beli_produk_id = obj.beli_produk_id;
                itemPembayaranHutang.BeliProduk = obj;
                itemPembayaranHutang.nominal = obj.grand_total; // GetTotalNotaSetelahDiskonDanPPN(obj);
                itemPembayaranHutang.keterangan = string.Empty;

                // set item pembayaran
                pembayaranHutang.item_pembayaran_hutang.Add(itemPembayaranHutang);

                // simpan item pembayaran
                result = pembayaranHutangRepo.Save(pembayaranHutang, true);
            }

            return result;
        }        

        /// <summary>
        /// Method untuk menghapus pembayaran hutang jika terjadi perubahan status nota dari tunai ke kredit
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private int HapusPembayaranHutang(BeliProduk obj)
        {
            PembayaranHutangProduk pembayaranHutang;
            ItemPembayaranHutangProduk itemPembayaranHutang;
            IPembayaranHutangProdukRepository pembayaranHutangRepo = new PembayaranHutangProdukRepository(_context, _log);

            var result = 0;

            // set detail
            itemPembayaranHutang = pembayaranHutangRepo.GetByBeliID(obj.beli_produk_id);
            if (itemPembayaranHutang != null)
            {
                pembayaranHutang = itemPembayaranHutang.PembayaranHutangProduk;
                result = pembayaranHutangRepo.Delete(pembayaranHutang);
            }

            return result;
        }

        public int Delete(BeliProduk obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<BeliProduk>(obj) ? 1 : 0;

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
    }
}     
