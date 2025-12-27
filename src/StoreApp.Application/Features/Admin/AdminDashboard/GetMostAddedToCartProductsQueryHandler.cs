using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminDashboard;
using StoreApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminDashboard
{
    public record GetMostAddedToCartProductsQuery(int Take) : IRequest<List<MostAddedToBasketDto>>;

    public class GetMostAddedToCartProductsQueryHandler : IRequestHandler<GetMostAddedToCartProductsQuery, List<MostAddedToBasketDto>>
    {
        private readonly IBasketRepository basketRepository;

        public GetMostAddedToCartProductsQueryHandler(IBasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        public async Task<List<MostAddedToBasketDto>> Handle(GetMostAddedToCartProductsQuery request, CancellationToken cancellationToken)
        {
            var baskets = await basketRepository.GetAllBasketAsync(cancellationToken);

            var result = baskets
                .Where(b => !b.IsDelete)
                .SelectMany(b => b.Items)
                .Where(i => !i.IsDelete)
                .GroupBy(i => new { i.ProductId, i.ProductName, i.PictureUrl })
                .Select(g => new MostAddedToBasketDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    PictureUrl = g.Key.PictureUrl
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(request.Take)
                .ToList();

            return result;
        }
    }
}
