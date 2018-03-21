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
using System.Linq.Expressions;
 
namespace OpenRetail.Repository.Service
{        
    public class CustomerRepository : ICustomerRepository
    {
        private const string SQL_TEMPLATE = @"SELECT m_customer.customer_id, m_customer.nama_customer, COALESCE(m_customer.kabupaten, m_customer.kabupaten, m_customer.kota) AS kabupaten_old, m_customer.kecamatan AS kecamatan_old, 
                                              m_customer.alamat, m_customer.kontak, m_customer.telepon, m_customer.plafon_piutang, m_customer.total_piutang, 
                                              m_customer.total_pembayaran_piutang, m_customer.kode_pos, m_customer.diskon, 
                                              m_provinsi2.provinsi_id, m_provinsi2.nama_provinsi, m_kabupaten2.kabupaten_id, m_kabupaten2.nama_kabupaten, 
                                              m_kecamatan.kecamatan_id, m_kecamatan.nama_kecamatan
                                              FROM public.m_customer LEFT JOIN public.m_provinsi2 ON m_customer.provinsi_id = m_provinsi2.provinsi_id
                                              LEFT JOIN public.m_kabupaten2 ON m_customer.kabupaten_id = m_kabupaten2.kabupaten_id
                                              LEFT JOIN public.m_kecamatan ON m_customer.kecamatan_id = m_kecamatan.kecamatan_id
                                              {WHERE}
                                              {ORDER BY}";
        private IDapperContext _context;
        private ILog _log;
        private string _sql;

        public CustomerRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        private IEnumerable<Customer> MappingRecordToObject(string sql, object param = null)
        {
            IEnumerable<Customer> oList = _context.db.Query<Customer, Provinsi, Kabupaten, Kecamatan, Customer>(sql, (cus, prov, kab, kec) =>
            {
                if (prov != null)
                {
                    cus.provinsi_id = prov.provinsi_id; cus.Provinsi = prov;
                }

                if (kab != null)
                {
                    cus.kabupaten_id = kab.kabupaten_id; cus.Kabupaten = kab;
                }

                if (kec != null)
                {
                    cus.kecamatan_id = kec.kecamatan_id; cus.Kecamatan = kec;
                }                

                return cus;
            }, param, splitOn: "provinsi_id, kabupaten_id, kecamatan_id");

            return oList;
        }

        public Customer GetByID(string id)
        {
            Customer obj = null;

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_customer.customer_id = @id");
                _sql = _sql.Replace("{ORDER BY}", "");

                obj = MappingRecordToObject(_sql, new { id }).SingleOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return obj;
        }

        public IList<Customer> GetByName(string name)
        {
            IList<Customer> oList = new List<Customer>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE LOWER(m_customer.nama_customer) LIKE @name");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY m_customer.nama_customer");

                name = "%" + name.ToLower() + "%";

                oList = MappingRecordToObject(_sql, new { name }).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<Customer> GetAll()
        {
            IList<Customer> oList = new List<Customer>();

            try
            {
                _sql = SQL_TEMPLATE.Replace("{WHERE}", "");
                _sql = _sql.Replace("{ORDER BY}", "ORDER BY m_customer.nama_customer");

                oList = MappingRecordToObject(_sql).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public IList<Customer> GetAll(bool isReseller)
        {
            IList<Customer> oList = new List<Customer>();

            try
            {
                /*
                Func<Customer, bool> predicate = p => p.diskon <= 0;

                if (isReseller)
                    predicate = p => p.diskon > 0;

                oList = _context.db.GetAll<Customer>()
                                .Where(predicate)
                                .OrderBy(f => f.nama_customer)
                                .ToList();*/

                if (isReseller)
                {
                    _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_customer.diskon > 0");
                }
                else
                {
                    _sql = SQL_TEMPLATE.Replace("{WHERE}", "WHERE m_customer.diskon <= 0");                                      
                }

                _sql = _sql.Replace("{ORDER BY}", "ORDER BY m_customer.nama_customer");  
                oList = MappingRecordToObject(_sql).ToList();
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return oList;
        }

        public int Save(Customer obj)
        {
            var result = 0;

            try
            {
                if (obj.customer_id == null)
                    obj.customer_id = _context.GetGUID();

                _context.db.Insert<Customer>(obj);
                result = 1;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Update(Customer obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Update<Customer>(obj) ? 1 : 0;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }

        public int Delete(Customer obj)
        {
            var result = 0;

            try
            {
                result = _context.db.Delete<Customer>(obj) ? 1 : 0;
            }
            catch (Exception ex)
            {
                _log.Error("Error:", ex);
            }

            return result;
        }        
    }
}     
