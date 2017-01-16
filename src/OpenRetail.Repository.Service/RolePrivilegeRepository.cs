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
    public class RolePrivilegeRepository : IRolePrivilegeRepository
    {
        private IDapperContext _context;
		private ILog _log;
		
        public RolePrivilegeRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public RolePrivilege GetByID(string id)
        {
            throw new NotImplementedException();
        }

        public IList<RolePrivilege> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<RolePrivilege> GetByRole(string roleId)
        {
            IList<RolePrivilege> oList = new List<RolePrivilege>();

            try
            {
                oList = _context.db.GetAll<RolePrivilege>()
                                .Where(f => f.role_id == roleId)
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<RolePrivilege> GetByRoleAndMenu(string roleId, string menuId)
        {
            IList<RolePrivilege> oList = new List<RolePrivilege>();

            try
            {
                oList = _context.db.GetAll<RolePrivilege>()
                                .Where(f => f.role_id == roleId && f.menu_id == menuId)
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<RolePrivilege> GetAll()
        {
            IList<RolePrivilege> oList = new List<RolePrivilege>();

            try
            {
                oList = _context.db.GetAll<RolePrivilege>()
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(RolePrivilege obj)
        {
            var result = 0;

            try
            {
                var row = _context.db.GetAll<RolePrivilege>()
                                  .Where(f => f.menu_id == obj.menu_id && f.role_id == obj.role_id && f.grant_id == obj.grant_id)
                                  .Count();

                var sql = string.Empty;
                if (row == 0)
                {
                    sql = @"INSERT INTO m_role_privilege (role_id, menu_id, grant_id, is_grant)
                            VALUES (@role_id, @menu_id, @grant_id, @is_grant)";                                        
                }
                else
                {
                    sql = @"UPDATE m_role_privilege SET is_grant = @is_grant
                            WHERE role_id = @role_id AND menu_id = @menu_id AND grant_id = @grant_id";
                }

                _context.db.Execute(sql, obj);
                result = 1;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Update(RolePrivilege obj)
        {
            throw new NotImplementedException();
        }

        public int Delete(RolePrivilege obj)
        {
            throw new NotImplementedException();
        }        
    }
}     
