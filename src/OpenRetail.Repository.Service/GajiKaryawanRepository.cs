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
    public class GajiKaryawanRepository : IGajiKaryawanRepository
    {
        private const string SQL_TEMPLATE = @"SELECT t_gaji_karyawan.gaji_karyawan_id, t_gaji_karyawan.pengguna_id, t_gaji_karyawan.bulan, t_gaji_karyawan.tahun, 
                                              t_gaji_karyawan.kehadiran, t_gaji_karyawan.absen, t_gaji_karyawan.gaji_pokok, t_gaji_karyawan.lembur, t_gaji_karyawan.bonus, 
                                              t_gaji_karyawan.potongan, t_gaji_karyawan.jam, t_gaji_karyawan.lainnya, t_gaji_karyawan.keterangan, 
                                              t_gaji_karyawan.jumlah_hari, t_gaji_karyawan.tunjangan, t_gaji_karyawan.kasbon, t_gaji_karyawan.tanggal, t_gaji_karyawan.nota, 
                                              m_karyawan.karyawan_id, m_karyawan.nama_karyawan, m_karyawan.gaji_pokok, m_karyawan.jenis_gajian, m_karyawan.gaji_lembur, m_karyawan.total_kasbon, m_karyawan.total_pembayaran_kasbon, 
                                              m_jabatan.jabatan_id, m_jabatan.nama_jabatan
                                              FROM public.t_gaji_karyawan INNER JOIN public.m_karyawan ON t_gaji_karyawan.karyawan_id = m_karyawan.karyawan_id
                                              INNER JOIN public.m_jabatan ON m_karyawan.jabatan_id = m_jabatan.jabatan_id
                                              {WHERE}
                                              {ORDER BY}";
        private IDapperContext _context;
        private ILog _log;

        private string _sql;

        public GajiKaryawanRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<GajiKaryawan> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<GajiKaryawan> oList = _context.db.Query<GajiKaryawan, Karyawan, Jabatan, GajiKaryawan>(sql, (g, k, j) =>
            {
                g.karyawan_id = k.karyawan_id; g.Karyawan = k;
                k.jabatan_id = j.jabatan_id; k.Jabatan = j;

                return g;
            }, param, splitOn: "karyawan_id, jabatan_id");

            return oList;
        }

        public string GetLastNota()
        {
            return _context.GetLastNota(new GajiKaryawan().GetTableName());
        }

        public GajiKaryawan GetByID(string id)
        {
            GajiKaryawan obj = null;

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_gaji_karyawan.gaji_karyawan_id = @id");
                _sql = _sql.Replace("{ORDER BY}", "");

                obj = MappingRecordToObject(_sql, new { id }).SingleOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<GajiKaryawan> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public IList<GajiKaryawan> GetByBulanAndTahun(int bulan, int tahun)
        {
            IList<GajiKaryawan> oList = new List<GajiKaryawan>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE t_gaji_karyawan.bulan = @bulan AND t_gaji_karyawan.tahun = @tahun");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY m_karyawan.nama_karyawan");

                oList = MappingRecordToObject(_sql, new { bulan, tahun }).ToList();

                IPembayaranKasbonRepository pembayaranKasbonRepo = new PembayaranKasbonRepository(_context, _log);

                foreach (var gaji in oList)
                {
                    gaji.item_pembayaran_kasbon = pembayaranKasbonRepo.GetByGajiKaryawan(gaji.gaji_karyawan_id);
                }

            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<GajiKaryawan> GetAll()
        {
            throw new NotImplementedException();
        }

        private bool IsExist(string karyawanId, int bulan, int tahun)
        {
            var count = _context.db.GetAll<GajiKaryawan>()
                                .Where(f => f.karyawan_id == karyawanId && f.bulan == bulan && f.tahun == tahun)
                                .Count();

            return count > 0;
        }

        public int Save(GajiKaryawan obj)
        {
            var result = 0;

            try
            {
                if (IsExist(obj.karyawan_id, obj.bulan, obj.tahun)) // data gaji karyawan sudah diinputkan
                {
                    return 0;
                }

                _context.BeginTransaction();

                var transaction = _context.transaction;

                obj.gaji_karyawan_id = _context.GetGUID();

                // insert header
                _context.db.Insert<GajiKaryawan>(obj, transaction);

                // insert detail
                foreach (var item in obj.item_pembayaran_kasbon.Where(f => f.Kasbon != null))
                {
                    if (item.kasbon_id.Length > 0)
                    {
                        item.pembayaran_kasbon_id = _context.GetGUID();
                        item.pengguna_id = obj.pengguna_id;
                        item.gaji_karyawan_id = obj.gaji_karyawan_id;                        
                        item.tanggal = obj.tanggal;
                        item.nota = obj.nota;

                        _context.db.Insert<PembayaranKasbon>(item, transaction);

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

        public int Update(GajiKaryawan obj)
        {
            var result = 0;

            try
            {
                _context.BeginTransaction();

                var transaction = _context.transaction;

                // update header
                result = _context.db.Update<GajiKaryawan>(obj, transaction) ? 1 : 0;

                // insert/update detail
                foreach (var item in obj.item_pembayaran_kasbon.Where(f => f.Kasbon != null))
                {
                    item.gaji_karyawan_id = obj.gaji_karyawan_id;
                    item.pengguna_id = obj.pengguna_id;

                    if (item.entity_state == EntityState.Added)
                    {
                        item.pembayaran_kasbon_id = _context.GetGUID();

                        _context.db.Insert<PembayaranKasbon>(item, transaction);

                        result = 1;
                    }
                    else if (item.entity_state == EntityState.Modified)
                    {
                        result = _context.db.Update<PembayaranKasbon>(item, transaction) ? 1 : 0;
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

        public int Delete(GajiKaryawan obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<GajiKaryawan>(obj) ? 1 : 0;

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
