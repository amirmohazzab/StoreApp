using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Dtos.Account;
using StoreApp.Application.Dtos.Admin.AdminUserDto;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminUserFeature.Queries.Get
{
    public class GetAdminUserByIdQuery : IRequest<AdminUserDto>
    {
        public string Id { get; set; }
    }

    public class GetAdminUserByIdQueryHandler : IRequestHandler<GetAdminUserByIdQuery, AdminUserDto>
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly IUserRepository userRepo;

        public GetAdminUserByIdQueryHandler(UserManager<User> userManager, IMapper mapper, IUserRepository userRepo)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.userRepo = userRepo;
        }

        public async Task<AdminUserDto> Handle(GetAdminUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepo.GetByIdAsync(request.Id);

            if (user == null)
                throw new Exception("User not found");

            var dto = new AdminUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Role = user.UserRoles.FirstOrDefault()?.Role.Name ?? "User",
                IsActive = user.IsActive,
                CreatedAt = DateTime.UtcNow,
            };

            var directPerms = user.UserPermissions?
       .Select(up => new UserPermissionDto
       {
           Id = up.Permission.Id,
           Name = up.Permission.Name,
           DisplayName = up.Permission.DisplayName
       }) ?? new List<UserPermissionDto>();

            var rolePerms = user.UserRoles?
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => new UserPermissionDto
                {
                    Id = rp.Permission.Id,
                    Name = rp.Permission.Name,
                    DisplayName = rp.Permission.DisplayName
                }) ?? new List<UserPermissionDto>();

            dto.Permissions = directPerms
                .Union(rolePerms, new PermissionComparer()) // PermissionComparer بر اساس Id یا Name
                .ToList();

            return dto;

        }
    }
}
