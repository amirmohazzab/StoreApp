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

namespace StoreApp.Application.Features.ProductFeature.Queries.GetAllProducts
{
    public class GetAllFeaturesQuery : IRequest<List<ProductDto>>
    {
        //public int? CategoryId { get; set; }
        //public int? BrandId { get; set; }
        //public string? Search { get; set; }
    }

    public class GetAllFeaturesQueryHandler : IRequestHandler<GetAllFeaturesQuery, List<ProductDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public GetAllFeaturesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetAllFeaturesQuery request, CancellationToken cancellationToken)
        {
            var products = await unitOfWork.Repository<Product>()
                .GetQueryable()
                .Where(p => p.IsActive)
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            //var query = unitOfWork.Repository<Product>().GetQueryable().Where(p => p.IsActive);

            //if (request.CategoryId.HasValue)
            //    query = query.Where(p => p.ProductTypeId == request.CategoryId);

            //if (request.BrandId.HasValue)
            //    query = query.Where(p => p.ProductBrandId == request.BrandId);

            //if (!string.IsNullOrWhiteSpace(request.Search))
            //    query = query.Where(p => p.Title.Contains(request.Search));

            //var products = await query
            //    .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
            //    .ToListAsync(cancellationToken);

            return products;
        }
    }
}
