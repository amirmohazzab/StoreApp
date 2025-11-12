using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Queries.GetAllMostViewed
{
    public class GetMostViewedProductsQuery : IRequest<List<ProductDto>>
    {
        public int Count { get; set; } = 6;
    }

    public class GetMostViewedProductsQueryHandler : IRequestHandler<GetMostViewedProductsQuery, List<ProductDto>>
    {
        public IUnitOfWork unitOfWork { get; set; }

        public IMapper mapper { get; set; }

        public GetMostViewedProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetMostViewedProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await unitOfWork.Repository<Product>()
                .GetQueryable()
                .Where(p => p.IsActive && !p.IsDelete)
                .OrderByDescending(p => p.ViewCount) 
                .Take(request.Count)
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return products;
        }
    }
}
