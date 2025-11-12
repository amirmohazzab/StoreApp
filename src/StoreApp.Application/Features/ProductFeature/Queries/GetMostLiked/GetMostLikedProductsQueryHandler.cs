using AutoMapper;
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

namespace StoreApp.Application.Features.ProductFeature.Queries.GetMostLiked
{
    public class GetMostLikedProductsQuery: IRequest<List<ProductDto>>
    {
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
            var products = await unitOfWork.Repository<Product>()
            .GetQueryable()
            .OrderByDescending(p => p.LikeCount)
            .Take(10)
            .ToListAsync(cancellationToken);

            return mapper.Map<List<ProductDto>>(products);
        }
    }
}
