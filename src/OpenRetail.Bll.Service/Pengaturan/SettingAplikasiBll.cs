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
using OpenRetail.Model;
using OpenRetail.Bll.Api;
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Service;

namespace OpenRetail.Bll.Service
{
    public class SettingAplikasiBll : ISettingAplikasiBll
    {
        private ILog _log;
        private IUnitOfWork _unitOfWork;

        public SettingAplikasiBll()
        {
        }

        public SettingAplikasi GetByID(string id)
        {
            SettingAplikasi obj = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                obj = _unitOfWork.SettingAplikasiRepository.GetByID(id);
            }

            return obj;
        }

        public int Save(SettingAplikasi obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                result = _unitOfWork.SettingAplikasiRepository.Save(obj);
            }

            return result;
        }

        public int Update(SettingAplikasi obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                result = _unitOfWork.SettingAplikasiRepository.Update(obj);
            }

            return result;
        }

        public int Delete(SettingAplikasi obj)
        {
            var result = 0;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                result = _unitOfWork.SettingAplikasiRepository.Delete(obj);
            }

            return result;
        }

        public IList<SettingAplikasi> GetAll()
        {
            IList<SettingAplikasi> oList = null;

            using (IDapperContext context = new DapperContext())
            {
                _unitOfWork = new UnitOfWork(context, _log);
                oList = _unitOfWork.SettingAplikasiRepository.GetAll();
            }

            return oList;
        }
    }
}
