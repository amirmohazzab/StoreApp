using AutoMapper;
using Microsoft.Extensions.Configuration;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Common.Mapping.Resolvers
{
    public class ProductGalleryUrlResolver : IValueResolver<Product, ProductDto, List<string>>
    {
        private readonly IConfiguration configuration;

        public ProductGalleryUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<string> Resolve(Product source, ProductDto destination, List<string> destMember, ResolutionContext context)
        {
            if (source.ProductImages == null || !source.ProductImages.Any())
                return new List<string>();

            string baseUrl = configuration["BackendUrl"] + configuration["LocationImages:ProductsImageLocation"];

            return source.ProductImages
                .Select(img => baseUrl + img.ImageUrl)
                .ToList();
        }
    }
}
