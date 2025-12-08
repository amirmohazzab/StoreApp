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

namespace StoreApp.Application.Features.Admin.AdminProductFeature.Queries.GetAll
{
    public class AdminGetProductsQuery : IRequest<List<AdminProductListDto>>
    {

    }

    public class AdminGetProductsQueryHandler : IRequestHandler<AdminGetProductsQuery, List<AdminProductListDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AdminGetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<AdminProductListDto>> Handle(AdminGetProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await unitOfWork.Repository<Product>()
                .GetQueryable()
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
           .ToListAsync(cancellationToken);

            return mapper.Map<List<AdminProductListDto>>(products);
        }
    }
}
