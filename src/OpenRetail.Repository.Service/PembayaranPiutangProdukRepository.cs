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
    public class PembayaranPiutangProdukRepository : IPembayaranPiutangProdukRepository
    {
        private const string SQL_TEMPLATE = @"SELECT t_pembayaran_piutang_produk.pembayaran_piutang_id, t_pembayaran_piutang_produk.pengguna_id, t_pembayaran_piutang_produk.tanggal, t_pembayaran_piutang_produk.keterangan, t_pembayaran_piutang_produk.tanggal_sistem, t_pembayaran_piutang_produk.nota, t_pembayaran_piutang_produk.is_tunai,
                                              m_customer.customer_id, m_customer.nama_customer
                                              FROM public.m_customer RIGHT JOIN public.t_pembayaran_piutang_produk ON t_pembayaran_piutang_produk.customer_id = m_customer.customer_id
                                              {WHERE}
                                              {ORDER BY}";
        private IDapperContext _context;
        private ILog _log;
        private string _sql;
		
        public PembayaranPiutangProdukRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<PembayaranPiutangProduk> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<PembayaranPiutangProduk> oList = _context.db.Query<PembayaranPiutangProduk, Customer, PembayaranPiutangProduk>(sql, (pp, c) =>
            {
                if (c != null)
                {
                    pp.customer_id = c.customer_id; pp.Customer = c;
                }

                return pp;
            }, param, splitOn: "customer_id");

            return oList;
        }

        private IList<ItemPembayaranPiutangProduk> GetItemPembayaran(string id)
        {
            IList<ItemPembayaranPiutangProduk> oList = new List<ItemPembayaranPiutangProduk>();

            try
            {
                var sql = @"SELECT t_item_pembayaran_piutang_produk.item_pembayaran_piutang_id, t_item_pembayaran_piutang_produk.pembayaran_piutang_id, t_item_pembayaran_piutang_produk.nominal, t_item_pembayaran_piutang_produk.keterangan, 
                            t_item_pembayaran_piutang_produk.tanggal_sistem, 1 as entity_state, (SELECT COUNT(*) FROM t_item_pembayaran_piutang_produk WHERE jual_id = t_jual_produk.jual_id) AS jumlah_angsuran,
                            t_jual_produk.jual_id, t_jual_produk.nota, t_jual_produk.tanggal, t_jual_produk.tanggal_tempo, t_jual_produk.ppn, t_jual_produk.ongkos_kirim, t_jual_produk.diskon, t_jual_produk.total_nota, t_jual_produk.total_pelunasan
                            FROM public.t_item_pembayaran_piutang_produk INNER JOIN public.t_jual_produk ON t_item_pembayaran_piutang_produk.jual_id = t_jual_produk.jual_id
                            WHERE t_item_pembayaran_piutang_produk.pembayaran_piutang_id = @id
                            ORDER BY t_item_pembayaran_piutang_produk.tanggal_sistem";

                oList = _context.db.Query<ItemPembayaranPiutangProduk, JualProduk, ItemPembayaranPiutangProduk>(sql, (ip, j) =>
                {
                    ip.jual_id = j.jual_id; ip.JualProduk = j;

                    return ip;
                }, new { id }, splitOn: "jual_id").ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public PembayaranPiutangProduk GetByID(string id)
        {
            PembayaranPiutangProduk obj = null;

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_pembayaran_piutang_produk.pembayaran_piutang_id = @id");
                _sql = _sql.Replace("{ORDER BY}", "");

                obj = MappingRecordToObject(_sql, new { id }).SingleOrDefault();
                if (obj != null)
                    obj.item_pembayaran_piutang = GetItemPembayaran(obj.pembayaran_piutang_id);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public string GetLastNota()
        {
            return _context.GetLastNota(new PembayaranPiutangProduk().GetTableName());
        }

        public ItemPembayaranPiutangProduk GetByJualID(string id)
        {
            ItemPembayaranPiutangProduk obj = null;

            try
            {
                var sql = @"SELECT t_item_pembayaran_piutang_produk.item_pembayaran_piutang_id, t_item_pembayaran_piutang_produk.jual_id, t_item_pembayaran_piutang_produk.nominal, t_item_pembayaran_piutang_produk.keterangan, 1 as entity_state, 
                            t_pembayaran_piutang_produk.pembayaran_piutang_id, t_pembayaran_piutang_produk.customer_id, t_pembayaran_piutang_produk.pengguna_id, t_pembayaran_piutang_produk.tanggal, t_pembayaran_piutang_produk.keterangan, t_pembayaran_piutang_produk.nota, t_pembayaran_piutang_produk.is_tunai
                            FROM public.t_item_pembayaran_piutang_produk INNER JOIN public.t_pembayaran_piutang_produk ON t_item_pembayaran_piutang_produk.pembayaran_piutang_id = t_pembayaran_piutang_produk.pembayaran_piutang_id
                            WHERE t_item_pembayaran_piutang_produk.jual_id = @id";

                obj = _context.db.Query<ItemPembayaranPiutangProduk, PembayaranPiutangProduk, ItemPembayaranPiutangProduk>(sql, (ipp, pp) =>
                {
                    ipp.pembayaran_piutang_id = pp.pembayaran_piutang_id;
                    ipp.PembayaranPiutangProduk = pp;

                    return ipp;
                }, new { id }, splitOn: "pembayaran_piutang_id").SingleOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<PembayaranPiutangProduk> GetByName(string name)
        {
            IList<PembayaranPiutangProduk> oList = new List<PembayaranPiutangProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE LOWER(m_customer.nama_customer) LIKE @name");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_pembayaran_piutang_produk.tanggal, t_pembayaran_piutang_produk.nota");

                name = "%" + name.ToLower() + "%";

                oList = MappingRecordToObject(_sql, new { name }).ToList();

                foreach (var item in oList)
                {
                    item.item_pembayaran_piutang = GetItemPembayaran(item.pembayaran_piutang_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }        

        public IList<PembayaranPiutangProduk> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PembayaranPiutangProduk> oList = new List<PembayaranPiutangProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_pembayaran_piutang_produk.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_pembayaran_piutang_produk.tanggal, t_pembayaran_piutang_produk.nota");

                oList = MappingRecordToObject(_sql, new { tanggalMulai, tanggalSelesai }).ToList();

                foreach (var item in oList)
                {
                    item.item_pembayaran_piutang = GetItemPembayaran(item.pembayaran_piutang_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PembayaranPiutangProduk> GetAll()
        {
            IList<PembayaranPiutangProduk> oList = new List<PembayaranPiutangProduk>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_pembayaran_piutang_produk.tanggal, t_pembayaran_piutang_produk.nota");

                oList = MappingRecordToObject(_sql).ToList();

                foreach (var item in oList)
                {
                    item.item_pembayaran_piutang = GetItemPembayaran(item.pembayaran_piutang_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(PembayaranPiutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Save(PembayaranPiutangProduk obj, bool isSaveFromPenjualan)
        {
            var result = 0;

            try
            {
                IDbTransaction transaction = null;

                if (!isSaveFromPenjualan)
                {
                    _context.BeginTransaction();

                    transaction = _context.transaction;
                }

                obj.pembayaran_piutang_id = _context.GetGUID();

                if (obj.nota == null || obj.nota.Length == 0)
                {
                    obj.nota = this.GetLastNota();
                }
                
                // insert header
                _context.db.Insert<PembayaranPiutangProduk>(obj, transaction);

                // insert detil
                foreach (var item in obj.item_pembayaran_piutang.Where(f => f.JualProduk != null))
                {
                    if (item.jual_id.Length > 0)
                    {
                        item.item_pembayaran_piutang_id = _context.GetGUID();
                        item.pembayaran_piutang_id = obj.pembayaran_piutang_id;

                        _context.db.Insert<ItemPembayaranPiutangProduk>(item, transaction);

                        // update entity state
                        item.entity_state = EntityState.Unchanged;
                    }
                }

                if (!isSaveFromPenjualan)
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

        public int Update(PembayaranPiutangProduk obj)
        {
            throw new NotImplementedException();
        }

        public int Update(PembayaranPiutangProduk obj, bool isUpdateFromPenjualan)
        {
            var result = 0;

            try
            {
                IDbTransaction transaction = null;

                if (!isUpdateFromPenjualan)
                {
                    _context.BeginTransaction();

                    transaction = _context.transaction;
                }

                // update header
                result = _context.db.Update<PembayaranPiutangProduk>(obj, transaction) ? 1 : 0;

                // delete detail
                foreach (var item in obj.item_pembayaran_piutang_deleted)
                {
                    result = _context.db.Delete<ItemPembayaranPiutangProduk>(item, transaction) ? 1 : 0;
                }

                // insert/update detail
                foreach (var item in obj.item_pembayaran_piutang.Where(f => f.JualProduk != null))
                {
                    item.pembayaran_piutang_id = obj.pembayaran_piutang_id;

                    if (item.entity_state == EntityState.Added)
                    {
                        item.item_pembayaran_piutang_id = _context.GetGUID();

                        _context.db.Insert<ItemPembayaranPiutangProduk>(item, transaction);

                        result = 1;
                    }
                    else if (item.entity_state == EntityState.Modified)
                    {
                        result = _context.db.Update<ItemPembayaranPiutangProduk>(item, transaction) ? 1 : 0;
                    }

                    // update entity state
                    item.entity_state = EntityState.Unchanged;
                }

                if (!isUpdateFromPenjualan)
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

        public int Delete(PembayaranPiutangProduk obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<PembayaranPiutangProduk>(obj) ? 1 : 0;

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
