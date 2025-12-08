using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminPermissionFeature.Queries.GetPermission
{
    public record GetAllPermissionsQuery() : IRequest<List<Permission>>;

    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, List<Permission>>
    {
        private readonly IPermissionRepository _permissionRepo;

        public GetAllPermissionsQueryHandler(IPermissionRepository permissionRepo)
        {
            _permissionRepo = permissionRepo;
        }

        public async Task<List<Permission>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            var permissions = await _permissionRepo.GetAllAsync();
            return permissions.ToList();
        }
    }
}
