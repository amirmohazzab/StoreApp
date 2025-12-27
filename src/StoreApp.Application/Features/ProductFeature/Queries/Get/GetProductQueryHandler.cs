using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Application.Features.ProductFeature.Queries.GetAll;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Queries.Get
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;

        public GetProductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }

        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var product = await unitOfWork.Repository<Product>()
                .GetQueryable()
                .Include(p => p.Reviews)
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .Include(p => p.ProductImages)
                .Include(p => p.Colors)
                .Include(p => p.Sizes)
                .Include(p => p.UserLikes)
                .Include(p => p.UserWishlists)
                .Where(p => p.Id == request.Id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Price = p.Price,
                    PictureUrl = p.PictureUrl,
                    LikeCount = p.LikeCount,
                    ViewCount = p.ViewCount,
                    ProductBrand = p.ProductBrand.Title,
                    ProductType = p.ProductType.Title,

                    Liked = p.UserLikes.Any(x => x.UserId == userId && x.Liked),

                    AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                    ReviewCount = p.Reviews.Count(),
                    IsInWishlist = p.UserWishlists.Any(x => x.UserId == userId && x.ProductId == p.Id),
                })
                .FirstOrDefaultAsync(cancellationToken);

            return product;
            //var spec = new GetProductSpec(request.Id);
            //var result = await unitOfWork.Repository<Product>().GetEntityWithSpec(spec, cancellationToken);
            //if (result == null) throw new NotFoundEntityException();
            //return mapper.Map<ProductDto>(result);

            //var entity = await unitOfWork.Repository<Product>().GetByIdAsync(request.Id, cancellationToken);
            //if (entity == null) throw new Exception(message: "error message");
            //return mapper.Map<ProductDto>(entity);
        }
    }
}
