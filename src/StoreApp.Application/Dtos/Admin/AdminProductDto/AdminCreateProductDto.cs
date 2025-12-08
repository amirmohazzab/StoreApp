using Microsoft.AspNetCore.Http;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminProductDto
{
    public class AdminCreateProductDto
    {
        public string Title { get; set; }

        public int ProductBrandId { get; set; }

        public int ProductTypeId { get; set; }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public string? Description { get; set; }

        public string? Summary { get; set; }

        public IFormFile? MainImage { get; set; }

        public List<IFormFile>? Gallery { get; set; }

        public List<string>? Colors { get; set; }

        public List<string>? Sizes { get; set; }

        public ProductBrand ProductBrand { get; set; }

        public ProductType ProductType { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public int CategoryId { get; set; }
    }
}
