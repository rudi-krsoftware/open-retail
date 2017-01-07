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

using OpenRetail.Repository.Api;
 
namespace OpenRetail.Repository.Service
{    
    public class UnitOfWork : IUnitOfWork
    {
        private IDapperContext _context;
        private IAlasanPenyesuaianStokRepository _alasanpenyesuaianstokRepository;
        private IJabatanRepository _jabatanRepository;
        private IJenisPengeluaranRepository _jenispengeluaranRepository;
        private IGolonganRepository _golonganRepository;
        private IProdukRepository _produkRepository;
        private ICustomerRepository _customerRepository;
        private ISupplierRepository _supplierRepository;

        public UnitOfWork(IDapperContext context)
        {
            this._context = context;
        }

        public IAlasanPenyesuaianStokRepository AlasanPenyesuaianStokRepository
        {
            get { return _alasanpenyesuaianstokRepository ?? (_alasanpenyesuaianstokRepository = new AlasanPenyesuaianStokRepository(_context)); }
        }

        public IJabatanRepository JabatanRepository
        {
            get { return _jabatanRepository ?? (_jabatanRepository = new JabatanRepository(_context)); }
        }

        public IJenisPengeluaranRepository JenisPengeluaranRepository
        {
            get { return _jenispengeluaranRepository ?? (_jenispengeluaranRepository = new JenisPengeluaranRepository(_context)); }
        }

        public IGolonganRepository GolonganRepository
        {
            get { return _golonganRepository ?? (_golonganRepository = new GolonganRepository(_context)); }
        }

        public IProdukRepository ProdukRepository
        {
            get { return _produkRepository ?? (_produkRepository = new ProdukRepository(_context)); }
        }

        public ICustomerRepository CustomerRepository
        {
            get { return _customerRepository ?? (_customerRepository = new CustomerRepository(_context)); }
        }

        public ISupplierRepository SupplierRepository
        {
            get { return _supplierRepository ?? (_supplierRepository = new SupplierRepository(_context)); }
        }
    }
}     
