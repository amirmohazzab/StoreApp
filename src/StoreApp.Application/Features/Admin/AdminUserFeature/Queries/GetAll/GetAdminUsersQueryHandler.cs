using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Dtos.Admin.AdminUserDto;
using StoreApp.Application.Wrappers;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminUserFeature.Queries.GetAll
{
    public class GetAdminUsersQuery : IRequest<PaginatedResult<AdminUserDto>>
    {
        public string? Search { get; set; }
        public string Sort { get; set; } = "UserName";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetAdminUsersQueryHandler : IRequestHandler<GetAdminUsersQuery, PaginatedResult<AdminUserDto>>
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public GetAdminUsersQueryHandler(UserManager<User> userManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<PaginatedResult<AdminUserDto>> Handle(GetAdminUsersQuery request, CancellationToken cancellationToken)
        {
            //var users = userManager.Users.AsQueryable();

            //if (!string.IsNullOrEmpty(request.Search))
            //    users = users.Where(x =>
            //        x.UserName.Contains(request.Search) ||
            //        x.Email.Contains(request.Search)
            //    );

            //users = request.Sort switch
            //{
            //    "email" => users.OrderBy(x => x.Email),
            //    "role" => users.OrderBy(x => x.MainRole),
            //    _ => users.OrderBy(x => x.UserName)
            //};

            //var total = await users.CountAsync();

            //var data = await users
            //    .Skip((request.PageNumber - 1) * request.PageSize)
            //    .Take(request.PageSize)
            //   .Select(u => new AdminUserDto
            //   {
            //       Id = u.Id,
            //       UserName = u.UserName,
            //       Email = u.Email,
            //       Role = u.MainRole, 
            //       IsActive = u.IsActive,
            //       //CreatedAt = u.,
            //       DisplayName = u.DisplayName
            //   })
            //    .ToListAsync();

            ////var dtos = mapper.Map<List<AdminUserDto>>(data);

            //return new PaginatedResult<AdminUserDto>(data, total, request.PageNumber, request.PageSize);

            var users = userManager.Users.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                users = users.Where(u =>
                    u.UserName.Contains(request.Search) ||
                    u.Email.Contains(request.Search)
                );
            }

            // Sort
            users = request.Sort?.ToLower() switch
            {
                "email" => users.OrderBy(u => u.Email),
                "role" => users.OrderBy(u => u.MainRole),
                _ => users.OrderBy(u => u.UserName)
            };

            var totalCount = await users.CountAsync(cancellationToken);

            // Pagination
            var usersPage = users
                .Include(u => u.UserPermissions)
                .ThenInclude(up => up.Permission)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Mapping with Role, Permissions, CreatedAt
            var dtos = new List<AdminUserDto>();
            foreach (var u in usersPage)
            {
                var claims = await userManager.GetClaimsAsync(u);
                var role = (await userManager.GetRolesAsync(u)).FirstOrDefault() ?? "";
                var permissions = u.UserPermissions?
                    .Select(up => new UserPermissionDto
                    {
                        Id = up.Permission.Id,
                        Name = up.Permission.Name,
                        DisplayName = up.Permission.DisplayName
                    })
                    .ToList() ?? new List<UserPermissionDto>();

                var createdAtClaim = claims.FirstOrDefault(c => c.Type == "CreatedAt")?.Value;

                dtos.Add(new AdminUserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    DisplayName = u.DisplayName,
                    Role = role,
                    Permissions = permissions,
                    IsActive = u.IsActive,
                    CreatedAt = DateTime.TryParse(createdAtClaim, out var dt) ? dt : DateTime.MinValue
                });
            }

            return new PaginatedResult<AdminUserDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
