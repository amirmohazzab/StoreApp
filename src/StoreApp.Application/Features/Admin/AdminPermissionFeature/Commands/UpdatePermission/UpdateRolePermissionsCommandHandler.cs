using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminPermissionFeature.Commands.UpdatePermission
{
    public class UpdateRolePermissionsCommand : IRequest<Unit>
    {
        public string RoleId { get; set; }

        public List<int> PermissionIds { get; set; } = new();
    }

    public class UpdateRolePermissionsCommandHandler : IRequestHandler<UpdateRolePermissionsCommand, Unit>
    {
        private readonly IRoleRepository _roleRepo;

        public UpdateRolePermissionsCommandHandler(IRoleRepository roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task<Unit> Handle(UpdateRolePermissionsCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepo.GetByIdAsync(request.RoleId);

            role.RolePermissions = request.PermissionIds
                .Select(pid => new RolePermission { RoleId = role.Id, PermissionId = pid })
                .ToList();

            await _roleRepo.UpdateAsync(role);

            return Unit.Value;
        }
    }
}
