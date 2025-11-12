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

namespace StoreApp.Application.Features.ProductFeature.Queries.GetAllMostLiked
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
            var products = await unitOfWork.Repository<Product>()
            .GetQueryable()
            .Include(p => p.UserLikes)
            .Where(p => p.IsActive && !p.IsDelete)
            .OrderByDescending(p => p.UserLikes.Count(ul => ul.Liked))
            .Take(request.Count)
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                Price = p.Price,
                PictureUrl = p.PictureUrl,
                IsActive = p.IsActive,
                LikeCount = p.UserLikes.Count(ul => ul.Liked),
                ViewCount = p.ViewCount
            })
            .ToListAsync(cancellationToken);

            return products;
        }
    }
}
