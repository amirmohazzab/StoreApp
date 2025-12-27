using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminProductDto;
using StoreApp.Application.Dtos.Admin.AdminWishlist;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminUserWishList.Queries
{
    public record AdminGetMostWishlistProductsQuery(int Take = 5) : IRequest<List<AdminMostWishlistedProductDto>> {}

    public class AdminGetAllWishlistQueryHandler : IRequestHandler<AdminGetMostWishlistProductsQuery, List<AdminMostWishlistedProductDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;

        public AdminGetAllWishlistQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
        }

        public async Task<List<AdminMostWishlistedProductDto>> Handle(AdminGetMostWishlistProductsQuery request, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.Repository<UserWishlist>()
                .GetQueryable()
                .Where(x => !x.IsDelete)
                .Include(x => x.Product)
                    .ThenInclude(p => p.ProductBrand)
                .GroupBy(x => new
                {
                    x.ProductId,
                    x.Product.Title,
                    BrandName = x.Product.ProductBrand.Title,
                    x.Product.PictureUrl
                })
                .Select(g => new AdminMostWishlistedProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductTitle = g.Key.Title,
                    ProductBrand = g.Key.BrandName,
                    PictureUrl = g.Key.PictureUrl,
                    Wishcount = g.Count()
                })
                .OrderByDescending(x => x.Wishcount)
                .Take(request.Take)
                .ToListAsync(cancellationToken);

            return result;

            //var repo = unitOfWork.Repository<UserWishlist>().GetQueryable();

            //return await repo
            //    .ProjectTo<AdminWishlistDto>(mapper.ConfigurationProvider)
            //    .ToListAsync(cancellationToken);
        }
    }
}
