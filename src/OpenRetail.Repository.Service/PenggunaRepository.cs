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
    public class PenggunaRepository : IPenggunaRepository
    {
        private const string SQL_TEMPLATE = @"SELECT m_pengguna.pengguna_id, m_pengguna.nama_pengguna, m_pengguna.pass_pengguna, m_pengguna.is_active, m_pengguna.status_user, 
                                              m_role.role_id, m_role.nama_role
                                              FROM public.m_pengguna LEFT JOIN public.m_role ON m_pengguna.role_id = m_role.role_id
                                              {WHERE}
                                              {ORDER BY}";
        private IDapperContext _context;
        private ILog _log;

        private string _sql;
		
        public PenggunaRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<Pengguna> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<Pengguna> oList = _context.db.Query<Pengguna, Role, Pengguna>(sql, (p, r) =>
            {
                if (r != null)
                    p.role_id = r.role_id; p.Role = r;

                return p;
            }, param, splitOn: "role_id");

            return oList;
        }

        public Pengguna GetByID(string userName)
        {
            Pengguna obj = null;

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE LOWER(m_pengguna.nama_pengguna) = LOWER(@userName)");
                _sql = _sql.Replace("{ORDER BY}", "");

                obj = MappingRecordToObject(_sql, new { userName }).SingleOrDefault();

                if (obj != null)
                {
                    IRolePrivilegeRepository rolePrivilegeRepository = new RolePrivilegeRepository(_context, _log);
                    obj.role_privileges = rolePrivilegeRepository.GetByRole(obj.role_id);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public bool IsValidPengguna(string userName, string password)
        {
            var result = false;

            var pengguna = GetByID(userName);

            if (pengguna != null)
            {
                result = (pengguna.is_active == true) && (password == pengguna.pass_pengguna);
            }

            return result;
        }

        public IList<Pengguna> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<Pengguna> GetAll()
        {
            IList<Pengguna> oList = new List<Pengguna>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY m_pengguna.nama_pengguna");

                oList = MappingRecordToObject(_sql).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(Pengguna obj)
        {
            var result = 0;

            try
            {
                var pengguna = GetByID(obj.nama_pengguna);
                if (pengguna != null)
                    return 0; // nama pengguna sudah terdaftar

                obj.pengguna_id = _context.GetGUID();

                // password sudah dienkripsi dari aplikasi
                _context.db.Insert<Pengguna>(obj);

                result = 1;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Update(Pengguna obj)
        {
            var result = 0;

            try
            {
                // password sudah dienkripsi dari aplikasi
                if (obj.pass_pengguna != null && obj.pass_pengguna.Length > 0)
                {
                    _sql = @"UPDATE m_pengguna SET nama_pengguna = @nama_pengguna, pass_pengguna = @pass_pengguna, role_id = @role_id, is_active = @is_active,
                             status_user = @status_user
                             WHERE pengguna_id = @pengguna_id";
                }
                else
                {
                    _sql = @"UPDATE m_pengguna SET nama_pengguna = @nama_pengguna, role_id = @role_id, is_active = @is_active, status_user = @status_user
                             WHERE pengguna_id = @pengguna_id";
                }

                result = _context.db.Execute(_sql, obj);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Delete(Pengguna obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<Pengguna>(obj) ? 1 : 0;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }        
    }
}     
