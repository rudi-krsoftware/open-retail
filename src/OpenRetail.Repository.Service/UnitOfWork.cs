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
using OpenRetail.Repository.Api;
using OpenRetail.Repository.Api.Report;
using OpenRetail.Repository.Service.Report;
 
namespace OpenRetail.Repository.Service
{    
    public class UnitOfWork : IUnitOfWork
    {
        private IDapperContext _context;
        private ILog _log;

        private IDatabaseVersionRepository _databaseversionRepository;
        private IAlasanPenyesuaianStokRepository _alasanpenyesuaianstokRepository;
        private IJabatanRepository _jabatanRepository;
        private IJenisPengeluaranRepository _jenispengeluaranRepository;
        private IGolonganRepository _golonganRepository;
        private IProdukRepository _produkRepository;
        private IHargaGrosirRepository _hargaGrosirRepository;
        private ICustomerRepository _customerRepository;
        private ISupplierRepository _supplierRepository;
        private IKaryawanRepository _karyawanRepository;
        private IBeliProdukRepository _beliprodukRepository;
        private IReturBeliProdukRepository _returbeliprodukRepository;
        private IJualProdukRepository _jualprodukRepository;
        private IPembayaranHutangProdukRepository _pembayaranhutangprodukRepository;
        private IPembayaranPiutangProdukRepository _pembayaranpiutangprodukRepository;
        private IReturJualProdukRepository _returjualprodukRepository;
        private ILog4NetRepository _log4NetRepository;
        private IPenyesuaianStokRepository _penyesuaianstokRepository;
        private IPenggunaRepository _penggunaRepository;
        private IRoleRepository _roleRepository;
        private IRolePrivilegeRepository _roleprivilegeRepository;
        private IMenuRepository _menuRepository;
        private IItemMenuRepository _itemmenuRepository;
        private IProfilRepository _profilRepository;
        private IPengeluaranBiayaRepository _pengeluaranbiayaRepository;
        private IKasbonRepository _kasbonRepository;
        private IPembayaranKasbonRepository _pembayarankasbonRepository;
        private IGajiKaryawanRepository _gajikaryawanRepository;
        private ICetakNotaRepository _cetakNotaRepository;
        private ICetakNotaRepository _cetakNotaDummyRepository;

        private IReportBeliProdukRepository _reportBeliProdukRepository;
        private IReportHutangBeliProdukRepository _reportHutangBeliProdukRepository;
        private IReportPembayaranHutangBeliProdukRepository _reportPembayaranHutangBeliProdukRepository;
        private IReportKartuHutangRepository _reportKartuHutangRepository;
        private IReportReturBeliProdukRepository _reportReturBeliProdukRepository;

        private IReportJualProdukRepository _reportJualProdukRepository;
        private IReportPiutangJualProdukRepository _reportPiutangJualProdukRepository;
        private IReportPembayaranPiutangJualProdukRepository _reportPembayaranPiutangJualProdukRepository;
        private IReportKartuPiutangRepository _reportKartuPiutangRepository;
        private IReportReturJualProdukRepository _reportReturJualProdukRepository;

        private IReportStokProdukRepository _reportStokProdukRepository;
        private IReportPengeluaranBiayaRepository _reportPengeluaranBiayaRepository;
        private IReportKasbonRepository _reportKasbonRepository;
        private IReportGajiKaryawanRepository _reportGajiKaryawanRepository;

        private IHeaderNotaRepository _headerNotaRepository;
        private ILabelNotaRepository _labelNotaRepository;
        private IKabupatenRepository _kabupatenRepository;

        private IFooterNotaMiniPosRepository _footernotaminiposRepository;
        private IHeaderNotaMiniPosRepository _headernotaminiposRepository;

        public UnitOfWork(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public IDatabaseVersionRepository DatabaseVersionRepository
        {
            get { return _databaseversionRepository ?? (_databaseversionRepository = new DatabaseVersionRepository(_context, _log)); }
        }

        public IAlasanPenyesuaianStokRepository AlasanPenyesuaianStokRepository
        {
            get { return _alasanpenyesuaianstokRepository ?? (_alasanpenyesuaianstokRepository = new AlasanPenyesuaianStokRepository(_context, _log)); }
        }

        public IJabatanRepository JabatanRepository
        {
            get { return _jabatanRepository ?? (_jabatanRepository = new JabatanRepository(_context, _log)); }
        }

        public IJenisPengeluaranRepository JenisPengeluaranRepository
        {
            get { return _jenispengeluaranRepository ?? (_jenispengeluaranRepository = new JenisPengeluaranRepository(_context, _log)); }
        }

        public IGolonganRepository GolonganRepository
        {
            get { return _golonganRepository ?? (_golonganRepository = new GolonganRepository(_context, _log)); }
        }

        public IProdukRepository ProdukRepository
        {
            get { return _produkRepository ?? (_produkRepository = new ProdukRepository(_context, _log)); }
        }

        public IHargaGrosirRepository HargaGrosirRepository
        {
            get { return _hargaGrosirRepository ?? (_hargaGrosirRepository = new HargaGrosirRepository(_context, _log)); }
        }

        public ICustomerRepository CustomerRepository
        {
            get { return _customerRepository ?? (_customerRepository = new CustomerRepository(_context, _log)); }
        }

        public ISupplierRepository SupplierRepository
        {
            get { return _supplierRepository ?? (_supplierRepository = new SupplierRepository(_context, _log)); }
        }

        public IKaryawanRepository KaryawanRepository
        {
            get { return _karyawanRepository ?? (_karyawanRepository = new KaryawanRepository(_context, _log)); }
        }

        public IBeliProdukRepository BeliProdukRepository
        {
            get { return _beliprodukRepository ?? (_beliprodukRepository = new BeliProdukRepository(_context, _log)); }
        }

        public IReturBeliProdukRepository ReturBeliProdukRepository
        {
            get { return _returbeliprodukRepository ?? (_returbeliprodukRepository = new ReturBeliProdukRepository(_context, _log)); }
        }

        public IJualProdukRepository JualProdukRepository
        {
            get { return _jualprodukRepository ?? (_jualprodukRepository = new JualProdukRepository(_context, _log)); }
        }

        public IPembayaranHutangProdukRepository PembayaranHutangProdukRepository
        {
            get { return _pembayaranhutangprodukRepository ?? (_pembayaranhutangprodukRepository = new PembayaranHutangProdukRepository(_context, _log)); }
        }

        public IPembayaranPiutangProdukRepository PembayaranPiutangProdukRepository
        {
            get { return _pembayaranpiutangprodukRepository ?? (_pembayaranpiutangprodukRepository = new PembayaranPiutangProdukRepository(_context, _log)); }
        }

        public IReturJualProdukRepository ReturJualProdukRepository
        {
            get { return _returjualprodukRepository ?? (_returjualprodukRepository = new ReturJualProdukRepository(_context, _log)); }
        }

        public ILog4NetRepository Log4NetRepository
        {
            get { return _log4NetRepository ?? (_log4NetRepository = new Log4NetRepository(_context)); }
        }

        public IPenyesuaianStokRepository PenyesuaianStokRepository
        {
            get { return _penyesuaianstokRepository ?? (_penyesuaianstokRepository = new PenyesuaianStokRepository(_context, _log)); }
        }

        public IPenggunaRepository PenggunaRepository
        {
            get { return _penggunaRepository ?? (_penggunaRepository = new PenggunaRepository(_context, _log)); }
        }

        public IRoleRepository RoleRepository
        {
            get { return _roleRepository ?? (_roleRepository = new RoleRepository(_context, _log)); }
        }

        public IRolePrivilegeRepository RolePrivilegeRepository
        {
            get { return _roleprivilegeRepository ?? (_roleprivilegeRepository = new RolePrivilegeRepository(_context, _log)); }
        }

        public IMenuRepository MenuRepository
        {
            get { return _menuRepository ?? (_menuRepository = new MenuRepository(_context, _log)); }
        }

        public IItemMenuRepository ItemMenuRepository
        {
            get { return _itemmenuRepository ?? (_itemmenuRepository = new ItemMenuRepository(_context, _log)); }
        }

        public IProfilRepository ProfilRepository
        {
            get { return _profilRepository ?? (_profilRepository = new ProfilRepository(_context, _log)); }
        }

        public IReportBeliProdukRepository ReportBeliProdukRepository
        {
            get { return _reportBeliProdukRepository ?? (_reportBeliProdukRepository = new ReportBeliProdukRepository(_context, _log)); }
        }

        public IReportHutangBeliProdukRepository ReportHutangBeliProdukRepository
        {
            get { return _reportHutangBeliProdukRepository ?? (_reportHutangBeliProdukRepository = new ReportHutangBeliProdukRepository(_context, _log)); }
        }

        public IReportPembayaranHutangBeliProdukRepository ReportPembayaranHutangBeliProdukRepository
        {
            get { return _reportPembayaranHutangBeliProdukRepository ?? (_reportPembayaranHutangBeliProdukRepository = new ReportPembayaranHutangBeliProdukRepository(_context, _log)); }
        }

        public IReportKartuHutangRepository ReportKartuHutangRepository
        {
            get { return _reportKartuHutangRepository ?? (_reportKartuHutangRepository = new ReportKartuHutangRepository(_context, _log)); }
        }

        public IReportReturBeliProdukRepository ReportReturBeliProdukRepository
        {
            get { return _reportReturBeliProdukRepository ?? (_reportReturBeliProdukRepository = new ReportReturBeliProdukRepository(_context, _log)); }
        }

        public IReportJualProdukRepository ReportJualProdukRepository
        {
            get { return _reportJualProdukRepository ?? (_reportJualProdukRepository = new ReportJualProdukRepository(_context, _log)); }
        }

        public IReportPiutangJualProdukRepository ReportPiutangJualProdukRepository
        {
            get { return _reportPiutangJualProdukRepository ?? (_reportPiutangJualProdukRepository = new ReportPiutangJualProdukRepository(_context, _log)); }
        }

        public IReportPembayaranPiutangJualProdukRepository ReportPembayaranPiutangJualProdukRepository
        {
            get { return _reportPembayaranPiutangJualProdukRepository ?? (_reportPembayaranPiutangJualProdukRepository = new ReportPembayaranPiutangJualProdukRepository(_context, _log)); }
        }

        public IReportKartuPiutangRepository ReportKartuPiutangRepository
        {
            get { return _reportKartuPiutangRepository ?? (_reportKartuPiutangRepository = new ReportKartuPiutangRepository(_context, _log)); }
        }

        public IReportReturJualProdukRepository ReportReturJualProdukRepository
        {
            get { return _reportReturJualProdukRepository ?? (_reportReturJualProdukRepository = new ReportReturJualProdukRepository(_context, _log)); }
        }

        public IReportStokProdukRepository ReportStokProdukRepository
        {
            get { return _reportStokProdukRepository ?? (_reportStokProdukRepository = new ReportStokProdukRepository(_context, _log)); }
        }

        public IPengeluaranBiayaRepository PengeluaranBiayaRepository
        {
            get { return _pengeluaranbiayaRepository ?? (_pengeluaranbiayaRepository = new PengeluaranBiayaRepository(_context, _log)); }
        }

        public IReportPengeluaranBiayaRepository ReportPengeluaranBiayaRepository
        {
            get { return _reportPengeluaranBiayaRepository ?? (_reportPengeluaranBiayaRepository = new ReportPengeluaranBiayaRepository(_context, _log)); }
        }

        public IReportKasbonRepository ReportKasbonRepository
        {
            get { return _reportKasbonRepository ?? (_reportKasbonRepository = new ReportKasbonRepository(_context, _log)); }
        }

        public IReportGajiKaryawanRepository ReportGajiKaryawanRepository
        {
            get { return _reportGajiKaryawanRepository ?? (_reportGajiKaryawanRepository = new ReportGajiKaryawanRepository(_context, _log)); }
        }

        public IKasbonRepository KasbonRepository
        {
            get { return _kasbonRepository ?? (_kasbonRepository = new KasbonRepository(_context, _log)); }
        }

        public IPembayaranKasbonRepository PembayaranKasbonRepository
        {
            get { return _pembayarankasbonRepository ?? (_pembayarankasbonRepository = new PembayaranKasbonRepository(_context, _log)); }
        }

        public IGajiKaryawanRepository GajiKaryawanRepository
        {
            get { return _gajikaryawanRepository ?? (_gajikaryawanRepository = new GajiKaryawanRepository(_context, _log)); }
        }

        public ICetakNotaRepository CetakNotaDummyRepository
        {
            get { return _cetakNotaDummyRepository ?? (_cetakNotaDummyRepository = new CetakNotaDummyRepository()); }
        }

        public ICetakNotaRepository CetakNotaRepository
        {
            get { return _cetakNotaRepository ?? (_cetakNotaRepository = new CetakNotaRepository(_context, _log)); }
        }

        public IHeaderNotaRepository HeaderNotaRepository
        {
            get { return _headerNotaRepository ?? (_headerNotaRepository = new HeaderNotaRepository(_context, _log)); }
        }

        public ILabelNotaRepository LabelNotaRepository
        {
            get { return _labelNotaRepository ?? (_labelNotaRepository = new LabelNotaRepository(_context, _log)); }
        }

        public IKabupatenRepository KabupatenRepository
        {
            get { return _kabupatenRepository ?? (_kabupatenRepository = new KabupatenRepository(_context, _log)); }
        }

        public IFooterNotaMiniPosRepository FooterNotaMiniPosRepository
        {
            get { return _footernotaminiposRepository ?? (_footernotaminiposRepository = new FooterNotaMiniPosRepository(_context, _log)); }
        }

        public IHeaderNotaMiniPosRepository HeaderNotaMiniPosRepository
        {
            get { return _headernotaminiposRepository ?? (_headernotaminiposRepository = new HeaderNotaMiniPosRepository(_context, _log)); }
        }
    }
}     
