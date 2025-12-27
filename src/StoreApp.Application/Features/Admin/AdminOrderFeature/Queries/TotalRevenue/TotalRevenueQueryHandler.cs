using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminOrderFeature.Queries.TotalRevenue
{
    public class TotalRevenueQuery : IRequest<decimal>
    {
    }
    public class TotalRevenueQueryHandler : IRequestHandler<TotalRevenueQuery, decimal>
    {
        private readonly IUnitOfWork unitOfWork;

        public TotalRevenueQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<decimal> Handle(TotalRevenueQuery request, CancellationToken cancellationToken)
        {
            var totalRevenue = await unitOfWork.Repository<Order>()
                .GetQueryable()
                .Where(o => o.IsFinally)
                .Select(o => o.SubTotal + (o.DeliveryMethod != null ? o.DeliveryMethod.Price : 0))
                .SumAsync(cancellationToken);

            return totalRevenue;
        }
    }
}