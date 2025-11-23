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

namespace StoreApp.Application.Features.ProductFeature.Queries.GetMostLiked
{
    public class GetMostLikedProductsQuery: IRequest<List<ProductDto>>
    {
        public int Count { get; set; } = 6;
    }

    public class GetMostLikedProductsQueryHandler : IRequestHandler<GetMostLikedProductsQuery, List<ProductDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public GetMostLikedProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<ProductDto>> Handle(GetMostLikedProductsQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Repository<Product>()
            .GetQueryable()
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.LikeCount);

            return await query
                .ProjectTo<ProductDto>(mapper.ConfigurationProvider)
                .Take(request.Count)
                .ToListAsync(cancellationToken);
        }
    }
}
