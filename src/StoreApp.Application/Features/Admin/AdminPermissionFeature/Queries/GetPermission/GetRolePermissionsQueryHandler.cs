using MediatR;
using StoreApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminPermissionFeature.Queries.GetPermission
{
    public record GetRolePermissionsQuery : IRequest<List<int>>
    {
        public string RoleId { get; set; }

        public GetRolePermissionsQuery(string roleId)
        {
            RoleId = roleId;   
        }
    }

    public class GetRolePermissionsQueryHandler : IRequestHandler<GetRolePermissionsQuery, List<int>>
    {
        private readonly IRoleRepository _roleRepo;

        public GetRolePermissionsQueryHandler(IRoleRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task<List<int>> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleRepo.GetByIdAsync(request.RoleId);
            if (role == null) return new List<int>();

            return role.RolePermissions?.Select(rp => rp.PermissionId).ToList() ?? new List<int>();
        }
    }
}
