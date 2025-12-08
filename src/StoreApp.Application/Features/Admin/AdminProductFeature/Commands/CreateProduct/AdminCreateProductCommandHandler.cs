using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminProductDto;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductFeature.Commands.CreateProduct
{
    public class AdminCreateProductCommand : IRequest<int>
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }

        public int ProductTypeId { get; set; }
        public int ProductBrandId { get; set; }
        public int CategoryId { get; set; }

        public IFormFile MainImage { get; set; }
        public List<IFormFile>? Gallery { get; set; }

        public List<string>? Colors { get; set; }
        public List<string>? Sizes { get; set; }
    }

    public class AdminCreateProductCommandHandler : IRequestHandler<AdminCreateProductCommand, int>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment env;
        private readonly IUploadService uploadService;

        public AdminCreateProductCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment env,
            IUploadService uploadService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.env = env;
            this.uploadService = uploadService;
        }

        public async Task<int> Handle(AdminCreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = mapper.Map<Product>(request);

            if (request.MainImage != null)
            {
                product.PictureUrl = await uploadService.UploadImageAsync(request.MainImage);
            }

            if (request.Gallery != null && request.Gallery.Any())
            {
                product.ProductImages = new List<ProductImage>();

                foreach (var img in request.Gallery)
                {
                    var url = await uploadService.UploadImageAsync(img);
                    product.ProductImages.Add(new ProductImage { ImageUrl = url });
                }
            }

            if (request.Colors != null)
            {
                product.Colors = request.Colors
                    .Select(c => new ProductColor { ColorCode = c })
                    .ToList();
            }

            if (request.Sizes != null)
            {
                product.Sizes = request.Sizes
                    .Select(s => new ProductSize { Size = s })
                    .ToList();
            }

            await unitOfWork.Repository<Product>().AddAsync(product, cancellationToken);
            await unitOfWork.Save(cancellationToken);

            return product.Id;
        }
    }
}
