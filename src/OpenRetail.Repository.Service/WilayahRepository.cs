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
    public class WilayahRepository : IWilayahRepository
    {        
        private IDapperContext _context;
        private ILog _log;

        public WilayahRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public int Save(Wilayah obj)
        {
            throw new NotImplementedException();
        }

        public int Update(Wilayah obj)
        {
            throw new NotImplementedException();
        }

        public int Delete(Wilayah obj)
        {
            throw new NotImplementedException();
        }

        public IList<Wilayah> GetAll()
        {
            IList<Wilayah> oList = new List<Wilayah>();

            try
            {
                var sql = @"SELECT m_provinsi2.provinsi_id, m_provinsi2.nama_provinsi, 
                            m_kabupaten2.kabupaten_id, m_kabupaten2.nama_kabupaten, 
                            m_kecamatan.kecamatan_id, m_kecamatan.nama_kecamatan
                            FROM public.m_provinsi2 INNER JOIN public.m_kabupaten2 ON m_kabupaten2.provinsi_id = m_provinsi2.provinsi_id
                            INNER JOIN public.m_kecamatan ON m_kecamatan.kabupaten_id = m_kabupaten2.kabupaten_id
                            ORDER BY m_provinsi2.nama_provinsi, m_kabupaten2.nama_kabupaten, m_kecamatan.nama_kecamatan";

                oList = _context.db.Query<Wilayah>(sql)
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public Wilayah GetProvinsi(string name)
        {            
            Wilayah obj = null;

            try
            {
                var sql = @"SELECT * FROM m_provinsi2 
                            WHERE LOWER(nama_provinsi) = @name";

                name = name.ToLower();
                obj = _context.db.QuerySingleOrDefault<Wilayah>(sql, new { name });
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public Wilayah GetKabupaten(string name)
        {
            Wilayah obj = null;

            try
            {
                var sql = @"SELECT * FROM m_kabupaten2
                            WHERE LOWER(nama_kabupaten) = @name";

                name = name.ToLower();
                obj = _context.db.QuerySingleOrDefault<Wilayah>(sql, new { name });
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public Wilayah GetKecamatan(string name)
        {
            Wilayah obj = null;

            try
            {
                var sql = @"SELECT * FROM m_kecamatan
                            WHERE LOWER(nama_kecamatan) = @name";

                name = name.ToLower();
                obj = _context.db.QuerySingleOrDefault<Wilayah>(sql, new { name });
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }
    }
}
