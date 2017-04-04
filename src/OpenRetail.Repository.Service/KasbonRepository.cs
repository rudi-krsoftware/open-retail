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
    public class KasbonRepository : IKasbonRepository
    {
        private const string SQL_TEMPLATE = @"SELECT t_kasbon.kasbon_id, t_kasbon.nota, t_kasbon.tanggal, t_kasbon.nominal, t_kasbon.total_pelunasan, t_kasbon.keterangan, t_kasbon.pengguna_id,
                                              m_karyawan.karyawan_id, m_karyawan.nama_karyawan
                                              FROM public.m_karyawan INNER JOIN public.t_kasbon ON m_karyawan.karyawan_id = t_kasbon.karyawan_id
                                              {WHERE}
                                              {ORDER BY}";
        private IDapperContext _context;
        private ILog _log;

        private string _sql;
		
        public KasbonRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<Kasbon> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<Kasbon> oList = _context.db.Query<Kasbon, Karyawan, Kasbon>(sql, (kas, kar) =>
            {
                kas.karyawan_id = kar.karyawan_id; kas.Karyawan = kar;
                return kas;
            }, param, splitOn: "karyawan_id");

            return oList;
        }

        public Kasbon GetByID(string id)
        {
            Kasbon obj = null;

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_kasbon.kasbon_id = @id");
                _sql = _sql.Replace("{ORDER BY}", "");

                obj = MappingRecordToObject(_sql, new { id }).SingleOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<Kasbon> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<Kasbon> GetByKaryawanId(string karyawanId)
        {
            IList<Kasbon> oList = new List<Kasbon>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_kasbon.karyawan_id = @karyawanId");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_kasbon.tanggal");

                oList = MappingRecordToObject(_sql, new { karyawanId }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<Kasbon> GetByStatus(bool isLunas)
        {
            IList<Kasbon> oList = new List<Kasbon>();

            try
            {
                if (isLunas)
                {
                    _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE (t_kasbon.nominal  - t_kasbon.total_pelunasan) <= 0");
                }
                else
                {
                    _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE (t_kasbon.nominal  - t_kasbon.total_pelunasan) > 0");
                }
                
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_kasbon.tanggal");

                oList = MappingRecordToObject(_sql).ToList();

                if (oList.Count > 0)
                {
                    IPembayaranKasbonRepository pembayaranKasbonRepo = new PembayaranKasbonRepository(_context, _log);

                    foreach (var kasbon in oList)
                    {
                        kasbon.item_pembayaran_kasbon = pembayaranKasbonRepo.GetByKasbonId(kasbon.kasbon_id);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<Kasbon> GetByTanggal(DateTime tanggalMulai, DateTime tanggalSelesai)
        {
            IList<Kasbon> oList = new List<Kasbon>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_kasbon.tanggal BETWEEN @tanggalMulai AND @tanggalSelesai");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_kasbon.tanggal");

                oList = MappingRecordToObject(_sql, new { tanggalMulai, tanggalSelesai }).ToList();

                if (oList.Count > 0)
                {
                    IPembayaranKasbonRepository pembayaranKasbonRepo = new PembayaranKasbonRepository(_context, _log);

                    foreach (var kasbon in oList)
                    {
                        kasbon.item_pembayaran_kasbon = pembayaranKasbonRepo.GetByKasbonId(kasbon.kasbon_id);
                    }
                }                
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<Kasbon> GetAll()
        {
            IList<Kasbon> oList = new List<Kasbon>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY t_kasbon.tanggal");

                oList = MappingRecordToObject(_sql).ToList();

                if (oList.Count > 0)
                {
                    IPembayaranKasbonRepository pembayaranKasbonRepo = new PembayaranKasbonRepository(_context, _log);

                    foreach (var kasbon in oList)
                    {
                        kasbon.item_pembayaran_kasbon = pembayaranKasbonRepo.GetByKasbonId(kasbon.kasbon_id);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(Kasbon obj)
        {
            var result = 0;

            try
            {
                obj.kasbon_id = _context.GetGUID();

                _context.db.Insert<Kasbon>(obj);

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

        public int Update(Kasbon obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Update<Kasbon>(obj) ? 1 : 0;

                if (result > 0)
                {
                    LogicalThreadContext.Properties["NewValue"] = obj.ToJson();
                    _log.Info("Update data");
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Delete(Kasbon obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<Kasbon>(obj) ? 1 : 0;

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
            return _context.GetLastNota(new Kasbon().GetTableName());
        }        
    }
}     
