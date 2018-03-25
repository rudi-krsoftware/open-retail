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
        private bool _isUseWebAPI;
        private string _baseUrl = string.Empty;

        private IDatabaseVersionRepository _databaseversionRepository;
        private IKartuRepository _kartuRepository;
        private IAlasanPenyesuaianStokRepository _alasanpenyesuaianstokRepository;
        private IJabatanRepository _jabatanRepository;
        private IJenisPengeluaranRepository _jenispengeluaranRepository;
        private IGolonganRepository _golonganRepository;
        private ISettingAplikasiRepository _settingAplikasiRepository;
        private IProdukRepository _produkRepository;
        private IHargaGrosirRepository _hargaGrosirRepository;
        private ICustomerRepository _customerRepository;
        private ISupplierRepository _supplierRepository;
        private IDropshipperRepository _dropshipperRepository;
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
        private IMesinKasirRepository _mesinRepository;
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
        private IReportMesinKasirRepository _reportMesinKasirRepository;

        private IReportStokProdukRepository _reportStokProdukRepository;
        private IReportKartuStokRepository _reportKartuStokRepository;
        private IReportPengeluaranBiayaRepository _reportPengeluaranBiayaRepository;
        private IReportKasbonRepository _reportKasbonRepository;
        private IReportGajiKaryawanRepository _reportGajiKaryawanRepository;
        private IReportPemasukanPengeluaranRepository _reportPemasukanPengeluaranRepository;

        private IHeaderNotaRepository _headerNotaRepository;
        private ILabelNotaRepository _labelNotaRepository;
        private IKabupatenRajaOngkirRepository _kabupatenRepository;
        private IWilayahRepository _wilayahRepository;

        private IFooterNotaMiniPosRepository _footernotaminiposRepository;
        private IHeaderNotaMiniPosRepository _headernotaminiposRepository;

        public UnitOfWork(IDapperContext context, ILog log)
        {
            this._context = context;
            this._log = log;
        }

        public UnitOfWork(bool isUseWebAPI, string baseUrl, ILog log)
        {
            this._isUseWebAPI = isUseWebAPI;
            this._baseUrl = baseUrl;
            this._log = log;            
        }

        public IDatabaseVersionRepository DatabaseVersionRepository
        {
            get { return _databaseversionRepository ?? (_databaseversionRepository = new DatabaseVersionRepository(_context, _log)); }
        }

        public IKartuRepository KartuRepository
        {
            get 
            {
                return _kartuRepository ?? (_kartuRepository = _isUseWebAPI ? (IKartuRepository)new KartuWebAPIRepository(_baseUrl, _log) : new KartuRepository(_context, _log));
            }
        }

        public IAlasanPenyesuaianStokRepository AlasanPenyesuaianStokRepository
        {
            get { return _alasanpenyesuaianstokRepository ?? (_alasanpenyesuaianstokRepository = new AlasanPenyesuaianStokRepository(_context, _log)); }
        }

        public IJabatanRepository JabatanRepository
        {
            get
            {
                if (_jabatanRepository == null)
                {
                    if (_isUseWebAPI)
                    {
                        _jabatanRepository = new JabatanWebAPIRepository(_baseUrl, _log);
                    }
                    else
                    {
                        _jabatanRepository = new JabatanRepository(_context, _log);
                    }
                }

                return _jabatanRepository;
            }
        }

        public IJenisPengeluaranRepository JenisPengeluaranRepository
        {
            get 
            { 
                if (_jenispengeluaranRepository == null)
                {
                    if (_isUseWebAPI)
                    {
                        _jenispengeluaranRepository = new JenisPengeluaranWebAPIRepository(_baseUrl, _log);
                    }
                    else
                    {
                        _jenispengeluaranRepository = new JenisPengeluaranRepository(_context, _log);
                    }
                }

                return _jenispengeluaranRepository;
            }
        }

        public IGolonganRepository GolonganRepository
        {
            get 
            {
                if (_golonganRepository == null)
                {
                    if (_isUseWebAPI)
                    {
                        _golonganRepository = new GolonganWebAPIRepository(_baseUrl, _log);
                    }
                    else
                    {
                        _golonganRepository = new GolonganRepository(_context, _log);
                    }
                }                

                return _golonganRepository;
            }
        }

        public ISettingAplikasiRepository SettingAplikasiRepository
        {
            get { return _settingAplikasiRepository ?? (_settingAplikasiRepository = new SettingAplikasiRepository(_context, _log)); }
        }

        public IProdukRepository ProdukRepository
        {
            get
            {
                if (_produkRepository == null)
                {
                    if (_isUseWebAPI)
                    {
                        _produkRepository = new ProdukWebAPIRepository(_baseUrl, _log);
                    }
                    else
                    {
                        _produkRepository = new ProdukRepository(_context, _log);
                    }
                }

                return _produkRepository;
            }
        }

        public IHargaGrosirRepository HargaGrosirRepository
        {
            get { return _hargaGrosirRepository ?? (_hargaGrosirRepository = new HargaGrosirRepository(_context, _log)); }
        }

        public ICustomerRepository CustomerRepository
        {
            get
            {
                if (_customerRepository == null)
                {
                    if (_isUseWebAPI)
                    {
                        _customerRepository = new CustomerWebAPIRepository(_baseUrl, _log);
                    }
                    else
                    {
                        _customerRepository = new CustomerRepository(_context, _log);
                    }
                }

                return _customerRepository;
            }
        }

        public ISupplierRepository SupplierRepository
        {
            get
            {
                if (_supplierRepository == null)
                {
                    if (_isUseWebAPI)
                    {
                        _supplierRepository = new SupplierWebAPIRepository(_baseUrl, _log);
                    }
                    else
                    {
                        _supplierRepository = new SupplierRepository(_context, _log);
                    }
                }

                return _supplierRepository;
            }
        }

        public IDropshipperRepository DropshipperRepository
        {
            get { return _dropshipperRepository ?? (_dropshipperRepository = new DropshipperRepository(_context, _log)); }
        }

        public IKaryawanRepository KaryawanRepository
        {
            get
            {
                if (_karyawanRepository == null)
                {
                    if (_isUseWebAPI)
                    {
                        _karyawanRepository = new KaryawanWebAPIRepository(_baseUrl, _log);
                    }
                    else
                    {
                        _karyawanRepository = new KaryawanRepository(_context, _log);
                    }
                }

                return _karyawanRepository;
            }
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

        public IMesinKasirRepository MesinRepository
        {
            get { return _mesinRepository ?? (_mesinRepository = new MesinKasirRepository(_context, _log)); }
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

        public IReportMesinKasirRepository ReportMesinKasirRepository
        {
            get { return _reportMesinKasirRepository ?? (_reportMesinKasirRepository = new ReportMesinKasirRepository(_context, _log)); }
        }

        public IReportStokProdukRepository ReportStokProdukRepository
        {
            get { return _reportStokProdukRepository ?? (_reportStokProdukRepository = new ReportStokProdukRepository(_context, _log)); }
        }

        public IReportKartuStokRepository ReportKartuStokRepository
        {
            get { return _reportKartuStokRepository ?? (_reportKartuStokRepository = new ReportKartuStokRepository(_context, _log)); }
        }

        public IPengeluaranBiayaRepository PengeluaranBiayaRepository
        {
            get
            {
                if (_pengeluaranbiayaRepository == null)
                {
                    if (_isUseWebAPI)
                    {
                        _pengeluaranbiayaRepository = new PengeluaranBiayaWebAPIRepository(_baseUrl, _log);
                    }
                    else
                    {
                        _pengeluaranbiayaRepository = new PengeluaranBiayaRepository(_context, _log);
                    }
                }

                return _pengeluaranbiayaRepository;
            }
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

        public IReportPemasukanPengeluaranRepository ReportPemasukanPengeluaranRepository
        {
            get { return _reportPemasukanPengeluaranRepository ?? (_reportPemasukanPengeluaranRepository = new ReportPemasukanPengeluaranRepository(_context, _log)); }
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

        public IKabupatenRajaOngkirRepository KabupatenRepository
        {
            get { return _kabupatenRepository ?? (_kabupatenRepository = new KabupatenRajaOngkirRepository(_context, _log)); }
        }

        public IWilayahRepository WilayahRepository
        {
            get { return _wilayahRepository ?? (_wilayahRepository = new WilayahRepository(_context, _log)); }
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
