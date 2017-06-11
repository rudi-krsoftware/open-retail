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
                config.CreateMap<GolonganDTO, Golongan>();
                config.CreateMap<JenisPengeluaranDTO, JenisPengeluaran>();
                config.CreateMap<JabatanDTO, Jabatan>();
                config.CreateMap<CustomerDTO, Customer>();
            });
        }
    }
}