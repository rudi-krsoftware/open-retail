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
 
namespace OpenRetail.Repository.Api
{    
    public interface IUnitOfWork
    {
        IAlasanPenyesuaianStokRepository AlasanPenyesuaianStokRepository { get; }
        IJabatanRepository JabatanRepository { get; }
        IJenisPengeluaranRepository JenisPengeluaranRepository { get; }
        IGolonganRepository GolonganRepository { get; }
        IProdukRepository ProdukRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        IKaryawanRepository KaryawanRepository { get; }
        IBeliProdukRepository BeliProdukRepository { get; }
        IJualProdukRepository JualProdukRepository { get; }                
        IPembayaranHutangProdukRepository PembayaranHutangProdukRepository { get; }
        IPembayaranPiutangProdukRepository PembayaranPiutangProdukRepository { get; }                
        ILog4NetRepository Log4NetRepository { get; }
        IPenyesuaianStokRepository PenyesuaianStokRepository { get; }
        IPenggunaRepository PenggunaRepository { get; }
        IRoleRepository RoleRepository { get; }
        IRolePrivilegeRepository RolePrivilegeRepository { get; }
        IMenuRepository MenuRepository { get; }
        IItemMenuRepository ItemMenuRepository { get; }                
    }
}     
