using AutoMapper;
using StoreApp.Application.Dtos.Admin.AdminProductDto;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Common.Mapping
{
    public class AdminMappingProfile : Profile
    {
        public AdminMappingProfile()
        {
            CreateMap<AdminCreateProductDto, Product>();
            CreateMap<AdminUpdateProductDto, Product>();

            CreateMap<Product, AdminProductListDto>();
            CreateMap<Product, AdminProductDetailsDto>()
                .ForMember(d => d.Gallery, o => o.MapFrom(s => s.ProductImages.Select(i => i.ImageUrl)));
        }
    }
}
