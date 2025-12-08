using Microsoft.AspNetCore.Http;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Dtos.Admin.AdminProductDto
{
    public class AdminUpdateProductDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public int? ProductBrandId { get; set; }

        public int? ProductTypeId { get; set; }

        public decimal? Price { get; set; }

        public decimal? OldPrice { get; set; }

        public string? Description { get; set; }

        public string? Summary { get; set; }

        // فقط برای نمایش
        public string? MainImage { get; set; }

        public List<string>? Gallery { get; set; }

        // فایل‌های جدید
        public IFormFile? NewMainImage { get; set; }

        public List<IFormFile>? NewGalleryImages { get; set; }

        public List<string>? RemovedGallery { get; set; }

        public List<string>? Colors { get; set; }

        public List<string>? Sizes { get; set; }

        public int? CategoryId { get; set; }
    }
}
