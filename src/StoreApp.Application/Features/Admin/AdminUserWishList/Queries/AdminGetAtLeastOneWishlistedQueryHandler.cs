using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminUserWishList.Queries
{
    public class AdminGetAtLeastOneWishlistedQuery() : IRequest<int>
    {

    }

    public class AdminGetAtLeastOneWishlistedQueryHandler : IRequestHandler<AdminGetAtLeastOneWishlistedQuery, int>
    {
        private readonly IUnitOfWork uow;

        public AdminGetAtLeastOneWishlistedQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<int> Handle(AdminGetAtLeastOneWishlistedQuery request, CancellationToken cancellationToken)
        {
            var wishlistProductCount = await uow
                .Repository<UserWishlist>()
                .GetQueryable()
                .Where(x => !x.IsDelete)
                .Select(x => x.ProductId)
                .Distinct()
                .CountAsync(cancellationToken);

            return wishlistProductCount;
        }
    }
}
