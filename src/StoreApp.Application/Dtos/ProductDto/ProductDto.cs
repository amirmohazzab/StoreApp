using AutoMapper;
using StoreApp.Application.Common.Mapping;
using StoreApp.Application.Common.Mapping;
using StoreApp.Application.Common.Mapping.Resolvers;
using StoreApp.Application.Dtos.Common;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.ProductDto
{
    public class ProductDto : CommandDto, IMapFrom<Product>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }

        public string PictureUrl { get; set; }

        public int ProductTypeId { get; set; }

        public int ProductBrandId { get; set; }

        public string ProductType { get; set; }

        public string ProductBrand { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductDto>()
                .ForMember(p => p.PictureUrl, c => c.MapFrom<ProductImageUrlResolver>())
                .ForMember(d => d.Thumbnails, o => o.MapFrom<ProductGalleryUrlResolver>())
                .ForMember(p => p.ProductType, c => c.MapFrom(v => v.ProductType.Title))
                .ForMember(p => p.ProductBrand, c => c.MapFrom(v => v.ProductBrand.Title))
                .ForMember(d => d.Thumbnails, o => o.MapFrom(s => s.ProductImages.Select(i => i.ImageUrl)))
                .ForMember(d => d.Colors, o => o.MapFrom(s => s.Colors.Select(c => c.ColorCode)))
                .ForMember(d => d.Sizes, o => o.MapFrom(s => s.Sizes.Select(sz => sz.Size)))
                .ForMember(d => d.Liked, o => o.Ignore());
        }

        public bool Liked { get; set; }

        public int LikeCount { get; set; }

        public int ViewCount { get; set; }

        public double? AverageRating { get; set; }

        public int? ReviewCount { get; set; }

        public string? Description { get; set; }

        public string? Summary { get; set; }

        public decimal? OldPrice { get; set; }

        public List<string> Thumbnails { get; set; } = new();

        public List<string> Colors { get; set; } = new();

        public List<string> Sizes { get; set; } = new();
    }
}
