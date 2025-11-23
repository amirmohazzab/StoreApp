using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Queries.GetRelated
{
    public class GetRelatedProductsQuery : IRequest<List<ProductDto>>
    {
        public int ProductId { get; set; }

        public int Count { get; set; } = 6;
    }

    public class GetRelatedProductsQueryHandler : IRequestHandler<GetRelatedProductsQuery, List<ProductDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public GetRelatedProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetRelatedProductsQuery request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Repository<Product>()
            .GetQueryable()
            .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);

            if (product == null)
                return new List<ProductDto>();

            var query = unitOfWork.Repository<Product>()
                .GetQueryable()
                .Where(p =>
                    p.Id != product.Id &&
                    p.IsActive &&
                    (p.ProductTypeId == product.ProductTypeId || p.ProductBrandId == product.ProductBrandId))
                .OrderByDescending(p => p.LikeCount)
                .ThenByDescending(p => p.ViewCount);

            var relatedProducts = await query
            .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            .Take(request.Count)
            .ToListAsync(cancellationToken);

            return relatedProducts;
        }
    }
}
