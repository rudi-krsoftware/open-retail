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
    public class ReturBeliProdukRepository : IReturBeliProdukRepository
    {
        private const string SQL_TEMPLATE = @"SELECT t_retur_beli_produk.retur_beli_produk_id, t_retur_beli_produk.pengguna_id, t_retur_beli_produk.nota, t_retur_beli_produk.tanggal, t_retur_beli_produk.keterangan, t_retur_beli_produk.tanggal_sistem, t_retur_beli_produk.total_nota, 
                                              m_supplier.supplier_id, m_supplier.nama_supplier, m_supplier.alamat, t_beli_produk.beli_produk_id, t_beli_produk.nota
                                              FROM public.m_supplier INNER JOIN public.t_retur_beli_produk ON t_retur_beli_produk.supplier_id = m_supplier.supplier_id
                                              INNER JOIN public.t_beli_produk ON t_retur_beli_produk.beli_produk_id = t_beli_produk.beli_produk_id
                                              {WHERE}
                                              {ORDER BY}";
        private IDapperContext _context;
        private ILog _log;
        private string _sql;
		
        public ReturBeliProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<ReturBeliProduk> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<ReturBeliProduk> oList = _context.db.Query<ReturBeliProduk, Supplier, BeliProduk, ReturBeliProduk>(sql, (rb, s, b) =>
            {
                rb.supplier_id = s.supplier_id; rb.Supplier = s;
                rb.beli_produk_id = b.beli_produk_id; rb.BeliProduk = b;

                return rb;
            }, param, splitOn: "supplier_id, beli_produk_id");

            return oList;
        }

        private IList<ItemReturBeliProduk> GetItemRetur(string returId)
        {
            IList<ItemReturBeliProduk> oList = new List<ItemReturBeliProduk>();

            try
            {
                var sql = @"SELECT t_item_retur_beli_produk.item_retur_beli_produk_id, t_item_retur_beli_produk.retur_beli_produk_id, t_item_retur_beli_produk.pengguna_id, t_item_retur_beli_produk.item_beli_id, t_item_retur_beli_produk.harga, t_item_retur_beli_produk.jumlah, t_item_retur_beli_produk.jumlah_retur, t_item_retur_beli_produk.tanggal_sistem, 1 as entity_state,
                            m_produk.produk_id, m_produk.kode_produk, m_produk.nama_produk, m_produk.satuan, m_produk.harga_jual
                            FROM public.t_item_retur_beli_produk INNER JOIN public.m_produk ON t_item_retur_beli_produk.produk_id = m_produk.produk_id  
                            WHERE t_item_retur_beli_produk.retur_beli_produk_id = @returId
                            ORDER BY t_item_retur_beli_produk.tanggal_sistem";

                oList = _context.db.Query<ItemReturBeliProduk, Produk, ItemReturBeliProduk>(sql, (ir, p) =>
                {
                    ir.produk_id = p.produk_id; ir.Produk = p;
                    return ir;

                }, new { returId }, splitOn: "produk_id").ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public ReturBeliProduk GetByID(string id)
        {
            ReturBeliProduk obj = null;

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_retur_beli_produk.retur_beli_produk_id = @id");
                _sql = _sql.Replace("{ORDER BY}", "");

                obj = MappingRecordToObject(_sql, new { id }).SingleOrDefault();

                if (obj != null)
                    // load item retur
                    obj.item_retur = GetItemRetur(obj.retur_beli_produk_id);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public string GetLastNota()
        {
            return _context.GetLastNota(new ReturBeliProduk().GetTableName());
        }

        public IList<ReturBeliProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReturBeliProduk> oList = new List<ReturBeliProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_retur_beli_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_retur_beli_produk.tanggal, t_retur_beli_produk.nota");

                oList = MappingRecordToObject(_sql, new { tanggalMulai, tanggalSelesai }).ToList();

                // load item beli
                foreach (var item in oList)
                {
                    item.item_retur = GetItemRetur(item.retur_beli_produk_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReturBeliProduk> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<ReturBeliProduk> GetAll()
        {
            IList<ReturBeliProduk> oList = new List<ReturBeliProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_retur_beli_produk.tanggal, t_retur_beli_produk.nota");

                oList = MappingRecordToObject(_sql).ToList();

                // load item beli
                foreach (var item in oList)
                {
                    item.item_retur = GetItemRetur(item.retur_beli_produk_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        private double GetTotalNota(ReturBeliProduk obj)
        {
            var total = obj.item_retur.Where(f => f.Produk != null && f.entity_state != EntityState.Deleted)
                                      .Sum(f => f.jumlah_retur * f.harga);
            return total;
        }

        public int Save(ReturBeliProduk obj)
        {
            var result = 0;

            try
            {
                _context.BeginTransaction();

                var transaction = _context.transaction;

                obj.retur_beli_produk_id = _context.GetGUID();

                obj.total_nota = GetTotalNota(obj);

                // insert header
                _context.db.Insert<ReturBeliProduk>(obj, transaction);

                // insert detil
                foreach (var item in obj.item_retur.Where(f => f.Produk != null))
                {
                    if (item.produk_id.Length > 0)
                    {
                        item.item_retur_beli_produk_id = _context.GetGUID();
                        item.retur_beli_produk_id = obj.retur_beli_produk_id;
                        item.pengguna_id = obj.pengguna_id;

                        _context.db.Insert<ItemReturBeliProduk>(item, transaction);

                        // update entity state
                        item.entity_state = EntityState.Unchanged;
                    }
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

        public int Update(ReturBeliProduk obj)
        {
            var result = 0;

            try
            {
                _context.BeginTransaction();

                var transaction = _context.transaction;

                obj.total_nota = GetTotalNota(obj);

                // update header
                result = _context.db.Update<ReturBeliProduk>(obj, transaction) ? 1 : 0;

                // delete detail
                foreach (var item in obj.item_retur_deleted)
                {
                    result = _context.db.Delete<ItemReturBeliProduk>(item, transaction) ? 1 : 0;
                }

                // insert/update detail
                foreach (var item in obj.item_retur.Where(f => f.Produk != null))
                {
                    item.retur_beli_produk_id = obj.retur_beli_produk_id;
                    item.pengguna_id = obj.pengguna_id;

                    if (item.entity_state == EntityState.Added)
                    {
                        item.item_retur_beli_produk_id = _context.GetGUID();
                        _context.db.Insert<ItemReturBeliProduk>(item, transaction);

                        result = 1;
                    }
                    else if (item.entity_state == EntityState.Modified)
                    {
                        result = _context.db.Update<ItemReturBeliProduk>(item, transaction) ? 1 : 0;
                    }

                    // update entity state
                    item.entity_state = EntityState.Unchanged;
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

        public int Delete(ReturBeliProduk obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<ReturBeliProduk>(obj) ? 1 : 0;

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
