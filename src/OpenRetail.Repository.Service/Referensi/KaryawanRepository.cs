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
    public class KaryawanRepository : IKaryawanRepository
    {
        private const string SQL_TEMPLATE = @"SELECT m_karyawan.karyawan_id, m_karyawan.nama_karyawan, m_karyawan.alamat, m_karyawan.telepon, 
                                              m_karyawan.jenis_gajian, m_karyawan.gaji_pokok, m_karyawan.gaji_lembur, m_karyawan.total_kasbon, m_karyawan.total_pembayaran_kasbon, m_karyawan.is_active, m_karyawan.keterangan,
                                              m_jabatan.jabatan_id, m_jabatan.nama_jabatan, m_jabatan.keterangan
                                              FROM public.m_karyawan INNER JOIN public.m_jabatan ON m_karyawan.jabatan_id = m_jabatan.jabatan_id
                                              {WHERE}
                                              {ORDER BY}";
        private IDapperContext _context;
        private ILog _log;
        private string _sql;

        public KaryawanRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<Karyawan> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<Karyawan> oList = _context.db.Query<Karyawan, Jabatan, Karyawan>(sql, (k, j) =>
            {
                k.jabatan_id = j.jabatan_id; k.Jabatan = j;
                return k;
            }, param, splitOn: "jabatan_id");

            return oList;
        }

        public Karyawan GetByID(string id)
        {
            Karyawan obj = null;

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_karyawan.karyawan_id = @id");
                _sql = _sql.Replace("{ORDER BY}", "");

                obj = MappingRecordToObject(_sql, new { id }).SingleOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<Karyawan> GetByName(string name)
        {
            IList<Karyawan> oList = new List<Karyawan>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE LOWER(m_karyawan.nama_karyawan) LIKE @name");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY m_karyawan.nama_karyawan");

                name = "%" + name.ToLower() + "%";

                oList = MappingRecordToObject(_sql, new { name }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<Karyawan> GetAll()
        {
            IList<Karyawan> oList = new List<Karyawan>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY m_karyawan.nama_karyawan");

                oList = MappingRecordToObject(_sql).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(Karyawan obj)
        {
            var result = 0;

            try
            {
                obj.karyawan_id = _context.GetGUID();

                _context.db.Insert<Karyawan>(obj);
                result = 1;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Update(Karyawan obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Update<Karyawan>(obj) ? 1 : 0;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Delete(Karyawan obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<Karyawan>(obj) ? 1 : 0;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }
    }
}     
