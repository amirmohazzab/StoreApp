using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Contracts.Specification;
using StoreApp.Application.Dtos.Admin.AdminProductDto;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductFeature.Commands.EditProduct
{
    public class AdminUpdateProductCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ProductBrandId { get; set; }

        public int ProductTypeId { get; set; }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public string? Description { get; set; }

        public string? Summary { get; set; }

        public string? MainImage { get; set; }

        public List<string>? Gallery { get; set; }

        public IFormFile? newMainImage { get; set; }

        public List<IFormFile>? NewGalleryImages { get; set; }

        public List<string>? RemovedGallery { get; set; }

        public List<string>? Colors { get; set; }

        public List<string>? Sizes { get; set; }

        public int CategoryId { get; set; }
    }

    public class AdminUpdateProductCommandHandler : IRequestHandler<AdminUpdateProductCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IUploadService uploadService;

        public AdminUpdateProductCommandHandler(
            IUnitOfWork unitOfWork,
            IUploadService uploadService, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.uploadService = uploadService;
        }

        public async Task<bool> Handle(AdminUpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Repository<Product>()
                    .GetQueryable()
                    .Include(p => p.ProductImages)
                    .Include(p => p.Colors)
                    .Include(p => p.Sizes)
                    .Include(p => p.ProductBrand)
                    .Include(p => p.ProductType)
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (product == null)
                throw new Exception("Product not found");

            product.Title = request.Title;
            product.Price = request.Price;
            product.OldPrice = request.OldPrice;
            product.Description = request.Description;
            product.Summary = request.Summary;
            product.ProductBrandId = request.ProductBrandId;
            product.ProductTypeId = request.ProductTypeId;
            product.CategoryId = request.CategoryId;

            if (request.RemovedGallery != null && request.RemovedGallery.Any())
            {
                foreach (var galleryUrl in request.RemovedGallery)
                {
                    if (string.IsNullOrWhiteSpace(galleryUrl))
                        continue;

                    var fileName = Path.GetFileName(galleryUrl);

                    var gallery = product.ProductImages
                        .FirstOrDefault(g => g.ImageUrl.EndsWith(fileName));

                    if (gallery == null)
                        continue;

                    gallery.IsDelete = true;
                    unitOfWork.Repository<ProductImage>().Update(gallery);

                    await uploadService.DeleteImageAsync(gallery.ImageUrl);
                    
                    // await DeleteFileAsync(gallery.ImageUrl);
                }
            }

            if (request.NewGalleryImages != null && request.NewGalleryImages.Any())
            {
                foreach (var newImg in request.NewGalleryImages)
                {
                    var url = await uploadService.UploadImageAsync(newImg);
                    product.ProductImages.Add(new ProductImage
                    {
                        ImageUrl = url,
                        ProductId = product.Id
                    });
                }
            }

            if (request.newMainImage != null)
            {
                if (!string.IsNullOrEmpty(product.PictureUrl))
                    await uploadService.DeleteFileAsync(product.PictureUrl);

                var newUrl = await uploadService.UploadImageAsync(request.newMainImage);
                product.PictureUrl = newUrl;
            }

            if (request.Colors != null && request.Colors.Any()) {

                var productColors = await unitOfWork.Repository<ProductColor>()
                    .Where(pc => pc.ProductId == request.Id)
                    .ToListAsync(cancellationToken);

                foreach (var color in productColors)
                {
                    await unitOfWork.Repository<ProductColor>()
                        .HardDelete(color, cancellationToken);
                }

                var productColorss = await unitOfWork.Repository<ProductColor>()
                    .Where(pc => pc.ProductId == request.Id)
                    .ToListAsync(cancellationToken);

                if (request.Colors is not null)
                {
                    foreach (var color in request.Colors)
                    {
                        var newColor = new ProductColor
                        {
                            ProductId = request.Id,
                            ColorCode = color
                        };

                        await unitOfWork.Repository<ProductColor>()
                            .AddAsync(newColor, cancellationToken);
                    }
                }

            }
            if (request.Sizes != null && request.Sizes.Any())
            {
                var productSizes = await unitOfWork.Repository<ProductSize>()
                        .Where(ps => ps.ProductId == request.Id)
                        .ToListAsync(cancellationToken);

                foreach (var size in productSizes)
                {
                    await unitOfWork.Repository<ProductSize>()
                        .HardDelete(size, cancellationToken);
                }

                if (request.Sizes is not null)
                {
                    foreach (var size in request.Sizes)
                    {
                        var newSize = new ProductSize
                        {
                            ProductId = request.Id,
                            Size = size
                        };

                        await unitOfWork.Repository<ProductSize>()
                            .AddAsync(newSize, cancellationToken);
                    }
                }
            }

            await unitOfWork.Save(cancellationToken);
            return true;
        }
    }
}

