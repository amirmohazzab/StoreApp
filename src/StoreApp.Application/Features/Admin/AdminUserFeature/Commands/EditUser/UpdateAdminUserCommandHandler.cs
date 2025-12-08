using MediatR;
using Microsoft.AspNetCore.Identity;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminUserFeature.Commands.EditUser
{
    public class UpdateAdminUserCommand : IRequest<bool>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string Role { get; set; }
    }

    public class UpdateAdminUserCommandHandler : IRequestHandler<UpdateAdminUserCommand, bool>
    {
        private readonly UserManager<User> userManager;
        private readonly IUnitOfWork unitOfWork;

        public UpdateAdminUserCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateAdminUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.Id);

            if (user == null)
                throw new Exception("User not found");

            user.Email = request.Email;
            user.UserName = request.UserName;
            user.IsActive = request.IsActive;
            user.MainRole = request.Role;

            var currentRoles = await userManager.GetRolesAsync(user);
            if (currentRoles.Any())
                await userManager.RemoveFromRolesAsync(user, currentRoles);

            await userManager.AddToRoleAsync(user, request.Role);

            var result = await userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}
