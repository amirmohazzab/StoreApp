using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminProductDto;
using StoreApp.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminProductFeature.Queries.GetAll
{
    public record AdminGetMostSoldProductsQuery(int Take = 5) : IRequest<List<AdminMostSoldProductDto>>;

    public class AdminGetMostSoldProductsQueryHandler : IRequestHandler<AdminGetMostSoldProductsQuery, List<AdminMostSoldProductDto>>
    {
        private readonly IUnitOfWork uow;

        public AdminGetMostSoldProductsQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<AdminMostSoldProductDto>> Handle(AdminGetMostSoldProductsQuery request, CancellationToken cancellationToken)
        {
            var query = uow.Repository<OrderItem>()
         .GetQueryable()
         .Where(i => !i.IsDelete)
         .GroupBy(i => new
         {
             i.ItemOrdered.ProductItemId,
             i.ItemOrdered.ProductName,
             i.ItemOrdered.ProductBrandName,
             i.ItemOrdered.ProductTypeName,
             i.ItemOrdered.PictureUrl
         })
         .Select(g => new AdminMostSoldProductDto
         {
             ProductId = g.Key.ProductItemId,
             ProductName = g.Key.ProductName,
             BrandName = g.Key.ProductBrandName,
             TypeName = g.Key.ProductTypeName,
             PictureUrl = g.Key.PictureUrl,

             TotalQuantitySold = g.Sum(x => x.Quantity),
             TotalRevenue = g.Sum(x => x.Price * x.Quantity)
         })
        .OrderByDescending(x => x.TotalQuantitySold)
        .Take(request.Take);

            return await query.ToListAsync(cancellationToken);
        }
    }
}
