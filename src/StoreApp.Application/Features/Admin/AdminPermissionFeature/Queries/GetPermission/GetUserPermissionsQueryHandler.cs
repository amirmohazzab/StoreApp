using MediatR;
using StoreApp.Application.Dtos.Admin.AdminUserDto;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminPermissionFeature.Queries.GetPermission
{
    public class GetUserPermissionsQuery : IRequest<List<UserPermissionDto>>
    {
        public string UserId { get; set; }
    }

    public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, List<UserPermissionDto>>
    {
        private readonly IUserRepository _userRepo;

        public GetUserPermissionsQueryHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<List<UserPermissionDto>> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.UserId);

            if (user == null)
                return new List<UserPermissionDto>();

            return user.UserPermissions?
                .Where(up => up.Permission != null)
                .Select(up => new UserPermissionDto
                {
                    Id = up.Permission.Id,
                    Name = up.Permission.Name,
                    DisplayName = up.Permission.DisplayName
                })
                .ToList() ?? new List<UserPermissionDto>();
        }

        private class PermissionComparer : IEqualityComparer<UserPermissionDto>
        {
            public bool Equals(UserPermissionDto x, UserPermissionDto y) => x.Id == y.Id;
            public int GetHashCode(UserPermissionDto obj) => obj.Id.GetHashCode();
        }
    }
}
