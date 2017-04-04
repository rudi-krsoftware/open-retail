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
using Dapper.Contrib.Extensions;

using OpenRetail.Model;
using OpenRetail.Repository.Api;
 
namespace OpenRetail.Repository.Service
{        
    public class PembayaranKasbonRepository : IPembayaranKasbonRepository
    {
        private IDapperContext _context;
		private ILog _log;
		
        public PembayaranKasbonRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public string GetLastNota()
        {
            return _context.GetLastNota(new PembayaranKasbon().GetTableName());
        }

        public PembayaranKasbon GetByID(string id)
        {
            PembayaranKasbon obj = null;

            try
            {
                obj = _context.db.Get<PembayaranKasbon>(id);
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<PembayaranKasbon> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<PembayaranKasbon> GetByKasbonId(string kasbonId)
        {
            IList<PembayaranKasbon> oList = new List<PembayaranKasbon>();

            try
            {
                oList = _context.db.GetAll<PembayaranKasbon>()
                                .Where(f => f.kasbon_id == kasbonId)
                                .OrderBy(f => f.tanggal)
                                .ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PembayaranKasbon> GetByGajiKaryawan(string gajiKaryawanId)
        {
            IList<PembayaranKasbon> oList = new List<PembayaranKasbon>();

            try
            {
                oList = _context.db.GetAll<PembayaranKasbon>()
                                .Where(f => f.gaji_karyawan_id == gajiKaryawanId)
                                .OrderBy(f => f.tanggal)
                                .ToList();

                foreach (var item in oList)
                {
                    item.entity_state = EntityState.Unchanged;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<PembayaranKasbon> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Save(PembayaranKasbon obj)
        {
            var result = 0;

            try
            {
                obj.pembayaran_kasbon_id = _context.GetGUID();

                _context.db.Insert<PembayaranKasbon>(obj);

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

        public int Update(PembayaranKasbon obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Update<PembayaranKasbon>(obj) ? 1 : 0;

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

        public int Delete(PembayaranKasbon obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<PembayaranKasbon>(obj) ? 1 : 0;

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
