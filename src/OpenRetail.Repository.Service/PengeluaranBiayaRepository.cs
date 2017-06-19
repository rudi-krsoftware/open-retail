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
    public class PengeluaranBiayaRepository : IPengeluaranBiayaRepository
    {
        private IDapperContext _context;
		private ILog _log;
		
        public PengeluaranBiayaRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IList<ItemPengeluaranBiaya> GetItemPengeluaranBiaya(string pengeluaranBiayaId)
        {
            IList<ItemPengeluaranBiaya> oList = new List<ItemPengeluaranBiaya>();

            try
            {
                var sql = @"SELECT t_item_pengeluaran_biaya.item_pengeluaran_id, t_item_pengeluaran_biaya.pengeluaran_id, t_item_pengeluaran_biaya.pengguna_id, 
                            t_item_pengeluaran_biaya.jumlah, t_item_pengeluaran_biaya.harga, 1 as entity_state,
                            m_jenis_pengeluaran.jenis_pengeluaran_id, m_jenis_pengeluaran.nama_jenis_pengeluaran
                            FROM public.t_item_pengeluaran_biaya INNER JOIN public.m_jenis_pengeluaran ON t_item_pengeluaran_biaya.jenis_pengeluaran_id = m_jenis_pengeluaran.jenis_pengeluaran_id
                            WHERE t_item_pengeluaran_biaya.pengeluaran_id = @pengeluaranBiayaId
                            ORDER BY t_item_pengeluaran_biaya.tanggal_sistem";

                oList = _context.db.Query<ItemPengeluaranBiaya, JenisPengeluaran, ItemPengeluaranBiaya>(sql, (ip, jp) =>
                {
                    ip.jenis_pengeluaran_id = jp.jenis_pengeluaran_id; ip.JenisPengeluaran = jp;
                    return ip;
                }, new { pengeluaranBiayaId }, splitOn: "jenis_pengeluaran_id").ToList();
            }
            catch
            {
            }

            return oList;
        }

        public PengeluaranBiaya GetByID(string id)
        {
            PengeluaranBiaya obj = null;

            try
            {
                obj = _context.db.Get<PengeluaranBiaya>(id);

                // load item pengeluaran
                if (obj != null)
                    obj.item_pengeluaran_biaya = GetItemPengeluaranBiaya(obj.pengeluaran_id);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<PengeluaranBiaya> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<PengeluaranBiaya> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<PengeluaranBiaya> oList = new List<PengeluaranBiaya>();

            try
            {
                oList = _context.db.GetAll<PengeluaranBiaya>()
                                .Where(f => f.tanggal >= tanggalMulai && f.tanggal <= tanggalSelesai)
                                .OrderBy(f => f.tanggal)
                                .ToList();

                // load item pengeluaran
                foreach (var item in oList)
                {
                    item.item_pengeluaran_biaya = GetItemPengeluaranBiaya(item.pengeluaran_id);
                }

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PengeluaranBiaya> GetAll()
        {
            IList<PengeluaranBiaya> oList = new List<PengeluaranBiaya>();

            try
            {
                oList = _context.db.GetAll<PengeluaranBiaya>()
                                .OrderBy(f => f.tanggal)
                                .ToList();

                // load item pengeluaran
                foreach (var item in oList)
                {
                    item.item_pengeluaran_biaya = GetItemPengeluaranBiaya(item.pengeluaran_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        private double GetTotalNota(PengeluaranBiaya obj)
        {
            var total = obj.item_pengeluaran_biaya.Where(f => f.JenisPengeluaran != null && f.entity_state != EntityState.Deleted)
                                                  .Sum(f => f.jumlah * f.harga);

            total = (int)total;
            return total;
        }

        public int Save(PengeluaranBiaya obj)
        {
            var result = 0;

            try
            {
                _context.BeginTransaction();

                var transaction = _context.transaction;

                obj.pengeluaran_id = _context.GetGUID();
                obj.total = GetTotalNota(obj);

                // insert header
                _context.db.Insert<PengeluaranBiaya>(obj, transaction);

                // insert detail
                foreach (var item in obj.item_pengeluaran_biaya.Where(f => f.JenisPengeluaran != null))
                {
                    if (item.jenis_pengeluaran_id.Length > 0)
                    {
                        item.item_pengeluaran_id = _context.GetGUID();
                        item.pengeluaran_id = obj.pengeluaran_id;
                        item.pengguna_id = obj.pengguna_id;
                        
                        _context.db.Insert<ItemPengeluaranBiaya>(item, transaction);

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

        public int Update(PengeluaranBiaya obj)
        {
            var result = 0;

            try
            {
                _context.BeginTransaction();

                var transaction = _context.transaction;

                obj.total = GetTotalNota(obj);
                
                // update header
                result = _context.db.Update<PengeluaranBiaya>(obj, transaction) ? 1 : 0;

                // delete detail
                foreach (var item in obj.item_pengeluaran_biaya_deleted)
                {
                    result = _context.db.Delete<ItemPengeluaranBiaya>(item, transaction) ? 1 : 0;
                }

                // insert/update detail
                foreach (var item in obj.item_pengeluaran_biaya.Where(f => f.JenisPengeluaran != null))
                {
                    item.pengeluaran_id = obj.pengeluaran_id;
                    item.pengguna_id = obj.pengguna_id;

                    if (item.entity_state == EntityState.Added)
                    {
                        item.item_pengeluaran_id = _context.GetGUID();

                        _context.db.Insert<ItemPengeluaranBiaya>(item, transaction);

                        result = 1;
                    }
                    else if (item.entity_state == EntityState.Modified)
                    {
                        result = _context.db.Update<ItemPengeluaranBiaya>(item, transaction) ? 1 : 0;
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

        public int Delete(PengeluaranBiaya obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<PengeluaranBiaya>(obj) ? 1 : 0;

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
            return _context.GetLastNota(new PengeluaranBiaya().GetTableName());
        }
    }
}     
