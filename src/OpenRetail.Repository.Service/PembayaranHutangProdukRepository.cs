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
using System.Data;
 
namespace OpenRetail.Repository.Service
{        
    public class PembayaranHutangProdukRepository : IPembayaranHutangProdukRepository
    {
        private const string SQL_TEMPLATE = @"SELECT t_pembayaran_hutang_produk.pembayaran_hutang_produk_id, t_pembayaran_hutang_produk.pengguna_id, t_pembayaran_hutang_produk.tanggal, 
                                              t_pembayaran_hutang_produk.keterangan, t_pembayaran_hutang_produk.tanggal_sistem, t_pembayaran_hutang_produk.nota, t_pembayaran_hutang_produk.is_tunai,
                                              m_supplier.supplier_id, m_supplier.nama_supplier
                                              FROM public.t_pembayaran_hutang_produk INNER JOIN public.m_supplier ON t_pembayaran_hutang_produk.supplier_id = m_supplier.supplier_id
                                              {WHERE}
                                              {ORDER BY}";
        private IDapperContext _context;
        private ILog _log;
        private string _sql;

        public PembayaranHutangProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<PembayaranHutangProduk> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<PembayaranHutangProduk> oList = _context.db.Query<PembayaranHutangProduk, Supplier, PembayaranHutangProduk>(sql, (p, s) =>
            {
                p.supplier_id = s.supplier_id; p.Supplier = s;
                return p;
            }, param, splitOn: "supplier_id");

            return oList;
        }

        private IList<ItemPembayaranHutangProduk> GetItemPembayaran(string id)
        {
            IList<ItemPembayaranHutangProduk> oList = new List<ItemPembayaranHutangProduk>();

            try
            {
                var sql = @"SELECT t_item_pembayaran_hutang_produk.item_pembayaran_hutang_produk_id, t_item_pembayaran_hutang_produk.pembayaran_hutang_produk_id, t_item_pembayaran_hutang_produk.nominal, 
                            t_item_pembayaran_hutang_produk.keterangan, t_item_pembayaran_hutang_produk.tanggal_sistem, 1 as entity_state, (SELECT COUNT(*) FROM t_item_pembayaran_hutang_produk WHERE beli_produk_id = t_beli_produk.beli_produk_id) AS jumlah_angsuran,
                            t_beli_produk.beli_produk_id, t_beli_produk.nota, t_beli_produk.tanggal, t_beli_produk.tanggal_tempo, t_beli_produk.ppn, t_beli_produk.diskon, t_beli_produk.total_nota, t_beli_produk.total_pelunasan
                            FROM public.t_item_pembayaran_hutang_produk INNER JOIN public.t_beli_produk ON t_item_pembayaran_hutang_produk.beli_produk_id = t_beli_produk.beli_produk_id
                            WHERE t_item_pembayaran_hutang_produk.pembayaran_hutang_produk_id = @id
                            ORDER BY t_item_pembayaran_hutang_produk.tanggal_sistem";

                oList = _context.db.Query<ItemPembayaranHutangProduk, BeliProduk, ItemPembayaranHutangProduk>(sql, (ip, b) =>
                {
                    ip.beli_produk_id = b.beli_produk_id; ip.BeliProduk = b;

                    return ip;
                }, new { id }, splitOn: "beli_produk_id").ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public PembayaranHutangProduk GetByID(string id)
        {
            PembayaranHutangProduk obj = null;

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_pembayaran_hutang_produk.pembayaran_hutang_produk_id = @id");
                _sql = _sql.Replace("{ORDER BY}", "");

                obj = MappingRecordToObject(_sql, new { id }).SingleOrDefault();
                if (obj != null)
                    obj.item_pembayaran_hutang = GetItemPembayaran(obj.pembayaran_hutang_produk_id);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<PembayaranHutangProduk> GetByName(string name)
        {
            IList<PembayaranHutangProduk> oList = new List<PembayaranHutangProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE LOWER(m_supplier.nama_supplier) LIKE @name");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_pembayaran_hutang_produk.tanggal, t_pembayaran_hutang_produk.nota");

                name = "%" + name.ToLower() + "%";

                oList = MappingRecordToObject(_sql, new { name }).ToList();

                foreach (var item in oList)
                {
                    item.item_pembayaran_hutang = GetItemPembayaran(item.pembayaran_hutang_produk_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PembayaranHutangProduk> GetAll()
        {
            IList<PembayaranHutangProduk> oList = new List<PembayaranHutangProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_pembayaran_hutang_produk.tanggal, t_pembayaran_hutang_produk.nota");

                oList = MappingRecordToObject(_sql).ToList();

                foreach (var item in oList)
                {
                    item.item_pembayaran_hutang = GetItemPembayaran(item.pembayaran_hutang_produk_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PembayaranHutangProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PembayaranHutangProduk> oList = new List<PembayaranHutangProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_pembayaran_hutang_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_pembayaran_hutang_produk.tanggal, t_pembayaran_hutang_produk.nota");

                oList = MappingRecordToObject(_sql, new { tanggalMulai, tanggalSelesai }).ToList();

                foreach (var item in oList)
                {
                    item.item_pembayaran_hutang = GetItemPembayaran(item.pembayaran_hutang_produk_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(PembayaranHutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Save(PembayaranHutangProduk obj, bool isSaveFromPembelian)
        {
            var result = 0;

            try
            {
                IDbTransaction transaction = null;

                if (!isSaveFromPembelian)
                {
                    _context.BeginTransaction();

                    transaction = _context.transaction;
                }                    

                obj.pembayaran_hutang_produk_id = _context.GetGUID();

                if (obj.nota == null || obj.nota.Length == 0)
                {
                    obj.nota = this.GetLastNota();
                }

                // insert header
                _context.db.Insert<PembayaranHutangProduk>(obj, transaction);

                // insert detil
                foreach (var item in obj.item_pembayaran_hutang.Where(f => f.BeliProduk != null))
                {
                    if (item.beli_produk_id.Length > 0)
                    {
                        item.item_pembayaran_hutang_produk_id = _context.GetGUID();
                        item.pembayaran_hutang_produk_id = obj.pembayaran_hutang_produk_id;

                        _context.db.Insert<ItemPembayaranHutangProduk>(item, transaction);

                        // update entity state
                        item.entity_state = EntityState.Unchanged;
                    }
                }

                if (!isSaveFromPembelian)
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

        public int Update(PembayaranHutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Update(PembayaranHutangProduk obj, bool isUpdateFromPembelian)
        {
            var result = 0;

            try
            {
                IDbTransaction transaction = null;

                if (!isUpdateFromPembelian)
                {
                    _context.BeginTransaction();

                    transaction = _context.transaction;
                }

                // update header
                result = _context.db.Update<PembayaranHutangProduk>(obj, transaction) ? 1 : 0;

                // delete detail
                foreach (var item in obj.item_pembayaran_hutang_deleted)
                {
                    result = _context.db.Delete<ItemPembayaranHutangProduk>(item, transaction) ? 1 : 0;
                }

                // insert/update detail
                foreach (var item in obj.item_pembayaran_hutang.Where(f => f.BeliProduk != null))
                {
                    item.pembayaran_hutang_produk_id = obj.pembayaran_hutang_produk_id;

                    if (item.entity_state == EntityState.Added)
                    {
                        item.item_pembayaran_hutang_produk_id = _context.GetGUID();

                        _context.db.Insert<ItemPembayaranHutangProduk>(item, transaction);

                        result = 1;
                    }
                    else if (item.entity_state == EntityState.Modified)
                    {
                        result = _context.db.Update<ItemPembayaranHutangProduk>(item, transaction) ? 1 : 0;
                    }

                    // update entity state
                    item.entity_state = EntityState.Unchanged;
                }

                if (!isUpdateFromPembelian)
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

        public int Delete(PembayaranHutangProduk obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<PembayaranHutangProduk>(obj) ? 1 : 0;

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
            return _context.GetLastNota(new PembayaranHutangProduk().GetTableName());
        }

        public ItemPembayaranHutangProduk GetByBeliID(string id)
        {
            ItemPembayaranHutangProduk obj = null;

            try
            {
                var sql = @"SELECT t_item_pembayaran_hutang_produk.item_pembayaran_hutang_produk_id, t_item_pembayaran_hutang_produk.beli_produk_id, t_item_pembayaran_hutang_produk.nominal, t_item_pembayaran_hutang_produk.keterangan, 1 as entity_state, 
                            t_pembayaran_hutang_produk.pembayaran_hutang_produk_id, t_pembayaran_hutang_produk.supplier_id, t_pembayaran_hutang_produk.pengguna_id, t_pembayaran_hutang_produk.tanggal, t_pembayaran_hutang_produk.keterangan, t_pembayaran_hutang_produk.nota, t_pembayaran_hutang_produk.is_tunai
                            FROM public.t_item_pembayaran_hutang_produk INNER JOIN public.t_pembayaran_hutang_produk ON t_item_pembayaran_hutang_produk.pembayaran_hutang_produk_id = t_pembayaran_hutang_produk.pembayaran_hutang_produk_id
                            WHERE t_item_pembayaran_hutang_produk.beli_produk_id = @id";

                obj = _context.db.Query<ItemPembayaranHutangProduk, PembayaranHutangProduk, ItemPembayaranHutangProduk>(sql, (iph, ph) =>
                {
                    iph.pembayaran_hutang_produk_id = ph.pembayaran_hutang_produk_id;
                    iph.PembayaranHutangProduk = ph;

                    return iph;
                }, new { id }, splitOn: "pembayaran_hutang_produk_id").SingleOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }        
    }
}     
