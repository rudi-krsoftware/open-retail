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
 
using OpenRetail.Repository.Api.Report;

namespace OpenRetail.Repository.Api
{    
    public interface IUnitOfWork
    {
        IDatabaseVersionRepository DatabaseVersionRepository { get; }
        IJenisPengeluaranRepository JenisPengeluaranRepository { get; }

        IGolonganRepository GolonganRepository { get; }
        IProdukRepository ProdukRepository { get; }
        IHargaGrosirRepository HargaGrosirRepository { get; }
        IAlasanPenyesuaianStokRepository AlasanPenyesuaianStokRepository { get; }
        IPenyesuaianStokRepository PenyesuaianStokRepository { get; }

        ICustomerRepository CustomerRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        
        IJabatanRepository JabatanRepository { get; }
        IKaryawanRepository KaryawanRepository { get; }
        
        IBeliProdukRepository BeliProdukRepository { get; }
        IPembayaranHutangProdukRepository PembayaranHutangProdukRepository { get; }
        IReturBeliProdukRepository ReturBeliProdukRepository { get; }              
  
        IJualProdukRepository JualProdukRepository { get; }                
        IPembayaranPiutangProdukRepository PembayaranPiutangProdukRepository { get; }
        IReturJualProdukRepository ReturJualProdukRepository { get; }

        IPengeluaranBiayaRepository PengeluaranBiayaRepository { get; }
        IKasbonRepository KasbonRepository { get; }
        IPembayaranKasbonRepository PembayaranKasbonRepository { get; }
        IGajiKaryawanRepository GajiKaryawanRepository { get; }

        ICetakNotaRepository CetakNotaRepository { get; }
        ICetakNotaRepository CetakNotaDummyRepository { get; }

        ILog4NetRepository Log4NetRepository { get; }
        
        IPenggunaRepository PenggunaRepository { get; }
        IRoleRepository RoleRepository { get; }
        IRolePrivilegeRepository RolePrivilegeRepository { get; }
        IMenuRepository MenuRepository { get; }
        IItemMenuRepository ItemMenuRepository { get; }
        IHeaderNotaRepository HeaderNotaRepository { get; }
        ILabelNotaRepository LabelNotaRepository { get; }

        IProfilRepository ProfilRepository { get; }

        IReportBeliProdukRepository ReportBeliProdukRepository { get; }
        IReportHutangBeliProdukRepository ReportHutangBeliProdukRepository { get; }
        IReportPembayaranHutangBeliProdukRepository ReportPembayaranHutangBeliProdukRepository { get; }
        IReportKartuHutangRepository ReportKartuHutangRepository { get; }
        IReportReturBeliProdukRepository ReportReturBeliProdukRepository { get; }

        IReportJualProdukRepository ReportJualProdukRepository { get; }
        IReportPiutangJualProdukRepository ReportPiutangJualProdukRepository { get; }
        IReportPembayaranPiutangJualProdukRepository ReportPembayaranPiutangJualProdukRepository { get; }
        IReportKartuPiutangRepository ReportKartuPiutangRepository { get; }
        IReportReturJualProdukRepository ReportReturJualProdukRepository { get; }

        IReportStokProdukRepository ReportStokProdukRepository { get; }
        IReportPengeluaranBiayaRepository ReportPengeluaranBiayaRepository { get; }
        IReportKasbonRepository ReportKasbonRepository { get; }
        IReportGajiKaryawanRepository ReportGajiKaryawanRepository { get; }

        IKabupatenRepository KabupatenRepository { get; }
        IFooterNotaMiniPosRepository FooterNotaMiniPosRepository { get; }
        IHeaderNotaMiniPosRepository HeaderNotaMiniPosRepository { get; }        
    }
}     
