using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductCategoryFeature.Query
{
    public class GetProductsByCategoryQuery : IRequest<List<ProductDto>>
    {
        public int CategoryId { get; set; }

        public GetProductsByCategoryQuery(int categoryId)
        {
            CategoryId = categoryId;
        }
    }

    public class GetProductsByCategoryQueryHandler : IRequestHandler<GetProductsByCategoryQuery, List<ProductDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetProductsByCategoryQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            var products = await unitOfWork.Repository<Product>()
            .GetQueryable()
            .Where(p => p.ProductTypeId == request.CategoryId && p.IsActive)
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider) 
            .ToListAsync(cancellationToken);

            return products;
        }
    }
}
