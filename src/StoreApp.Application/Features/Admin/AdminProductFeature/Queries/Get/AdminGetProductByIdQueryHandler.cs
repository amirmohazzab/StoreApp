using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminProductDto;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductFeature.Queries.Get
{
    public class AdminGetProductByIdQuery : IRequest<AdminProductDetailsDto>
    {
        public int Id { get; set; }

        public AdminGetProductByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class AdminGetProductByIdQueryHandler : IRequestHandler<AdminGetProductByIdQuery, AdminProductDetailsDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AdminGetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<AdminProductDetailsDto> Handle(AdminGetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Repository<Product>()
                .GetQueryable()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Include(p => p.Category)
                .Include(p => p.Sizes)
                .Include(p => p.Colors)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (product == null)
                throw new Exception("Product not found");

            var dto = mapper.Map<AdminProductDetailsDto>(product);
            return dto;
        }
    }
}
