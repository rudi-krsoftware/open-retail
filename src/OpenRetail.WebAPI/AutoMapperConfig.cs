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
                config.CreateMap<KartuDTO, Kartu>();
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
                config.CreateMap<PengeluaranBiayaDTO, PengeluaranBiaya>();
                config.CreateMap<ItemPengeluaranBiayaDTO, ItemPengeluaranBiaya>();
                config.CreateMap<KaryawanDTO, Karyawan>();
            });
        }
    }
}
