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

using log4net;
using OpenRetail.Bll.Api;
using OpenRetail.Model;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;
using System;
using System.Collections.Generic;

namespace OpenRetail.Bll.Service
{
    public class RolePrivilegeBll : IRolePrivilegeBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;

        public RolePrivilegeBll(ILog log)
        {
            _log = log;
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
            IList<RolePrivilege> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.RolePrivilegeRepository.GetByRole(roleId);
            }

            return oList;
        }

        public IList<RolePrivilege> GetByRoleAndMenu(string roleId, string menuId)
        {
            IList<RolePrivilege> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.RolePrivilegeRepository.GetByRoleAndMenu(roleId, menuId);
            }

            return oList;
        }

        public IList<RolePrivilege> GetAll()
        {
            IList<RolePrivilege> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.RolePrivilegeRepository.GetAll();
            }

            return oList;
        }

        public int Save(RolePrivilege obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                result = _unitOfWork.RolePrivilegeRepository.Save(obj);
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