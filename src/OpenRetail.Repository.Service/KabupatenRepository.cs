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
    public class KabupatenRepository : IKabupatenRepository
    {
        private const string SQL_TEMPLATE = @"SELECT m_kabupaten.kabupaten_id, m_kabupaten.tipe, m_kabupaten.nama_kabupaten, m_kabupaten.kode_pos, 
                                              m_provinsi.provinsi_id, m_provinsi.nama_provinsi
                                              FROM public.m_kabupaten INNER JOIN public.m_provinsi ON m_kabupaten.provinsi_id = m_provinsi.provinsi_id                                              
                                              {WHERE}
                                              ORDER BY m_kabupaten.nama_kabupaten";
        private IDapperContext _context;
        private ILog _log;

        private string _sql;
		
        public KabupatenRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<Kabupaten> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<Kabupaten> oList = _context.db.Query<Kabupaten, Provinsi, Kabupaten>(sql, (k, p) =>
            {
                if (p != null)
                {
                    k.provinsi_id = p.provinsi_id; k.Provinsi = p;
                }

                return k;
            }, param, splitOn: "provinsi_id");

            return oList;
        }

        public Kabupaten GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Kabupaten> GetByName(string name)
        {
            IList<Kabupaten> oList = new List<Kabupaten>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE LOWER(m_kabupaten.nama_kabupaten) LIKE @name OR LOWER(m_provinsi.nama_provinsi) LIKE @name");

                name = "%" + name.ToLower() + "%";

                oList = MappingRecordToObject(_sql, new { name }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<Kabupaten> GetAll()
        {
            IList<Kabupaten> oList = new List<Kabupaten>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");

                oList = MappingRecordToObject(_sql).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(Kabupaten obj)
        {
            throw new NotImplementedException();
        }

        public int Update(Kabupaten obj)
        {
            throw new NotImplementedException();
        }

        public int Delete(Kabupaten obj)
        {
            throw new NotImplementedException();
        }
    }
}     
