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
    public class ReturJualProdukRepository : IReturJualProdukRepository
    {
        private const string SQL_TEMPLATE = @"SELECT t_retur_jual_produk.retur_jual_id, t_retur_jual_produk.pengguna_id, t_retur_jual_produk.nota, t_retur_jual_produk.tanggal, t_retur_jual_produk.keterangan, t_retur_jual_produk.tanggal_sistem, t_retur_jual_produk.total_nota, 
                                              m_customer.customer_id, m_customer.nama_customer, m_customer.alamat, t_jual_produk.jual_id, t_jual_produk.nota
                                              FROM public.m_customer INNER JOIN public.t_retur_jual_produk ON t_retur_jual_produk.customer_id = m_customer.customer_id
                                              INNER JOIN public.t_jual_produk ON t_retur_jual_produk.jual_id = t_jual_produk.jual_id 
                                              {WHERE}
                                              {ORDER BY}";
        private IDapperContext _context;
        private ILog _log;
        private string _sql;
		
        public ReturJualProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<ReturJualProduk> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<ReturJualProduk> oList = _context.db.Query<ReturJualProduk, Customer, JualProduk, ReturJualProduk>(sql, (rj, c, j) =>
            {
                rj.customer_id = c.customer_id; rj.Customer = c;
                rj.jual_id = j.jual_id; rj.JualProduk = j;

                return rj;
            }, param, splitOn: "customer_id, jual_id");

            return oList;
        }

        private IList<ItemReturJualProduk> GetItemRetur(string returId)
        {
            IList<ItemReturJualProduk> oList = new List<ItemReturJualProduk>();

            try
            {
                var sql = @"SELECT t_item_retur_jual_produk.item_retur_jual_id, t_item_retur_jual_produk.retur_jual_id, t_item_retur_jual_produk.pengguna_id, t_item_retur_jual_produk.item_jual_id, t_item_retur_jual_produk.harga_jual, t_item_retur_jual_produk.jumlah, t_item_retur_jual_produk.jumlah_retur, t_item_retur_jual_produk.tanggal_sistem, 1 as entity_state,
                            m_produk.produk_id, m_produk.kode_produk, m_produk.nama_produk, m_produk.satuan, m_produk.harga_jual
                            FROM public.t_item_retur_jual_produk INNER JOIN public.m_produk ON t_item_retur_jual_produk.produk_id = m_produk.produk_id  
                            WHERE t_item_retur_jual_produk.retur_jual_id = @returId
                            ORDER BY t_item_retur_jual_produk.tanggal_sistem";

                oList = _context.db.Query<ItemReturJualProduk, Produk, ItemReturJualProduk>(sql, (ir, p) =>
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

        public ReturJualProduk GetByID(string id)
        {
            ReturJualProduk obj = null;

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_retur_jual_produk.retur_jual_id = @id");
                _sql = _sql.Replace("{ORDER BY}", "");

                obj = MappingRecordToObject(_sql, new { id }).SingleOrDefault();

                if (obj != null)
                    // load item retur
                    obj.item_retur = GetItemRetur(obj.retur_jual_id);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public string GetLastNota()
        {
            return _context.GetLastNota(new ReturJualProduk().GetTableName());
        }

        public IList<ReturJualProduk> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<ReturJualProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<ReturJualProduk> oList = new List<ReturJualProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_retur_jual_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_retur_jual_produk.tanggal, t_retur_jual_produk.nota");

                oList = MappingRecordToObject(_sql, new { tanggalMulai, tanggalSelesai }).ToList();

                // load item beli
                foreach (var item in oList)
                {
                    item.item_retur = GetItemRetur(item.retur_jual_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<ReturJualProduk> GetAll()
        {
            IList<ReturJualProduk> oList = new List<ReturJualProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_retur_jual_produk.tanggal, t_retur_jual_produk.nota");

                oList = MappingRecordToObject(_sql).ToList();

                // load item beli
                foreach (var item in oList)
                {
                    item.item_retur = GetItemRetur(item.retur_jual_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        private double GetTotalNota(ReturJualProduk obj)
        {
            var total = obj.item_retur.Where(f => f.Produk != null && f.entity_state != EntityState.Deleted)
                                      .Sum(f => f.jumlah_retur * f.harga_jual);
            return total;
        }

        public int Save(ReturJualProduk obj)
        {
            var result = 0;

            try
            {
                _context.BeginTransaction();

                var transaction = _context.transaction;

                obj.retur_jual_id = _context.GetGUID();

                obj.total_nota = GetTotalNota(obj);

                // insert header
                _context.db.Insert<ReturJualProduk>(obj, transaction);

                // insert detil
                foreach (var item in obj.item_retur.Where(f => f.Produk != null))
                {
                    if (item.produk_id.Length > 0)
                    {
                        item.item_retur_jual_id = _context.GetGUID();
                        item.retur_jual_id = obj.retur_jual_id;
                        item.pengguna_id = obj.pengguna_id;

                        _context.db.Insert<ItemReturJualProduk>(item, transaction);

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

        public int Update(ReturJualProduk obj)
        {
            var result = 0;

            try
            {
                _context.BeginTransaction();

                var transaction = _context.transaction;

                obj.total_nota = GetTotalNota(obj);

                // update header
                result = _context.db.Update<ReturJualProduk>(obj, transaction) ? 1 : 0;

                // delete detail
                foreach (var item in obj.item_retur_deleted)
                {
                    result = _context.db.Delete<ItemReturJualProduk>(item, transaction) ? 1 : 0;
                }

                // insert/update detail
                foreach (var item in obj.item_retur.Where(f => f.Produk != null))
                {
                    item.retur_jual_id = obj.retur_jual_id;
                    item.pengguna_id = obj.pengguna_id;

                    if (item.entity_state == EntityState.Added)
                    {
                        item.item_retur_jual_id = _context.GetGUID();
                        _context.db.Insert<ItemReturJualProduk>(item, transaction);

                        result = 1;
                    }
                    else if (item.entity_state == EntityState.Modified)
                    {
                        result = _context.db.Update<ItemReturJualProduk>(item, transaction) ? 1 : 0;
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

        public int Delete(ReturJualProduk obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<ReturJualProduk>(obj) ? 1 : 0;

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
