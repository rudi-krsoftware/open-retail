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
using System.Linq.Expressions;
 
namespace OpenRetail.Repository.Service
{        
    public class CustomerRepository : ICustomerRepository
    {
        private IDapperContext _context;
        private ILog _log;

        public CustomerRepository(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public Customer GetByID(string id)
        {
            Customer obj = null;

            try
            {
                obj = _context.db.Get<Customer>(id);
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
                oList = _context.db.GetAll<Customer>()
                                .Where(f => f.nama_customer.ToLower().Contains(name.ToLower()))
                                .OrderBy(f => f.nama_customer)
                                .ToList();
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
                oList = _context.db.GetAll<Customer>()
                                .OrderBy(f => f.nama_customer)
                                .ToList();
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
                Func<Customer, bool> predicate = p => p.diskon <= 0;

                if (isReseller)
                    predicate = p => p.diskon > 0;

                oList = _context.db.GetAll<Customer>()
                                .Where(predicate)
                                .OrderBy(f => f.nama_customer)
                                .ToList();
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
