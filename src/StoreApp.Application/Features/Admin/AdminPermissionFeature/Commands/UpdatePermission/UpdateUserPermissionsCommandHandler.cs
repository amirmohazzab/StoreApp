using MediatR;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminPermissionFeature.Commands.UpdatePermission
{
    public class UpdateUserPermissionsCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
        public List<int> PermissionIds { get; set; } = new();
    }

    public class UpdateUserPermissionsCommandHandler : IRequestHandler<UpdateUserPermissionsCommand, Unit>
    {
        private readonly IUserRepository _userRepo;

        public UpdateUserPermissionsCommandHandler(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<Unit> Handle(UpdateUserPermissionsCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.UserId);
            if (user == null) throw new Exception("User not found");

            user.UserPermissions = request.PermissionIds
                .Select(pid => new UserPermission { UserId = user.Id, PermissionId = pid })
                .ToList();

            await _userRepo.UpdateAsync(user);

            return Unit.Value;
        }
    }
}
