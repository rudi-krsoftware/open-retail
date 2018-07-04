using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AutoMapper;
using OpenRetail.Model;
using OpenRetail.WebAPI.Models.DTO;

namespace OpenRetail.WebAPI
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize((config) =>
            {
                // referensi
                config.CreateMap<KartuDTO, Kartu>();
                config.CreateMap<AlasanPenyesuaianStokDTO, AlasanPenyesuaianStok>();
                config.CreateMap<PenyesuaianStokDTO, PenyesuaianStok>();
                config.CreateMap<DropshipperDTO, Dropshipper>();
                config.CreateMap<GolonganDTO, Golongan>();
                config.CreateMap<ProdukDTO, Produk>();
                config.CreateMap<HargaGrosirDTO, HargaGrosir>();
                config.CreateMap<JenisPengeluaranDTO, JenisPengeluaran>();
                config.CreateMap<JabatanDTO, Jabatan>();
                config.CreateMap<ProvinsiDTO, Provinsi>();
                config.CreateMap<KabupatenDTO, Kabupaten>();
                config.CreateMap<KecamatanDTO, Kecamatan>();
                config.CreateMap<CustomerDTO, Customer>();
                config.CreateMap<SupplierDTO, Supplier>();                
                config.CreateMap<KaryawanDTO, Karyawan>();

                // pengeluaran
                config.CreateMap<PengeluaranBiayaDTO, PengeluaranBiaya>();
                config.CreateMap<ItemPengeluaranBiayaDTO, ItemPengeluaranBiaya>();
                config.CreateMap<KasbonDTO, Kasbon>();
                config.CreateMap<PembayaranKasbonDTO, PembayaranKasbon>();
                config.CreateMap<GajiKaryawanDTO, GajiKaryawan>();

                // transaksi
                config.CreateMap<JualProdukDTO, JualProduk>();
                config.CreateMap<ItemJualProdukDTO, ItemJualProduk>();
                config.CreateMap<ReturJualProdukDTO, ReturJualProduk>();

                // pengaturan
                config.CreateMap<RoleDTO, Role>();
                config.CreateMap<PenggunaDTO, Pengguna>();
            });
        }
    }
}
