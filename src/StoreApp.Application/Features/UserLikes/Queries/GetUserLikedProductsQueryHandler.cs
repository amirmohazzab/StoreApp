using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.UserLikes.Queries
{
    public class GetUserLikedProductsQuery : IRequest<List<ProductDto>>
    {
        //public string BaseUrl { get; set; } = string.Empty;
    }

    public class GetUserLikedProductsQueryHandler : IRequestHandler<GetUserLikedProductsQuery, List<ProductDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;

        public GetUserLikedProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }


        public async Task<List<ProductDto>> Handle(GetUserLikedProductsQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var likedProducts = await unitOfWork.Repository<UserLike>()
                .GetQueryable()
                .Where(ul => ul.UserId == userId && ul.Liked)
                .Include(ul => ul.Product)
                .Select(ul => new ProductDto
                {
                    Id = ul.Product.Id,
                    Title = ul.Product.Title,
                    Description = ul.Product.Description,
                    Price = ul.Product.Price,
                    PictureUrl = ul.Product.PictureUrl,
                    IsActive = ul.Product.IsActive,
                    Liked = true
                })
                .ToListAsync(cancellationToken);

            return likedProducts;
        }
    }
}

        // ul.Product.PictureUrl.StartsWith("http")
        //? ul.Product.PictureUrl
        //: $"{request.BaseUrl}images/products/{ul.Product.PictureUrl}"
