using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminDashboard;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminDashboard
{
    public class GetAdminDashboardStatsQuery : IRequest<AdminDashboardStatsDto>
    {
    }

    public class GetAdminDashboardStatsQueryHandler : IRequestHandler<GetAdminDashboardStatsQuery, AdminDashboardStatsDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<User> userManager;

        public GetAdminDashboardStatsQueryHandler(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
        }

        public async Task<AdminDashboardStatsDto> Handle(GetAdminDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            var totalProducts = await unitOfWork.Repository<Product>()
                .GetQueryable().CountAsync(cancellationToken);

            var totalUsers = await userManager.Users.CountAsync(cancellationToken);

            var totalWishlistItems = await unitOfWork.Repository<UserWishlist>()
                .GetQueryable().CountAsync(cancellationToken);

            var totalCategories = await unitOfWork.Repository<ProductCategory>()
                .GetQueryable().CountAsync(cancellationToken);

            return new AdminDashboardStatsDto
            {
                TotalProducts = totalProducts,
                TotalUsers = totalUsers,
                TotalWishlistItems = totalWishlistItems,
                TotalCategories = totalCategories
            };
        }
    }
}
