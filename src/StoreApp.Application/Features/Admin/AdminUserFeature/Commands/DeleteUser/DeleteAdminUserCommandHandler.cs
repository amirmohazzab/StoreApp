using MediatR;
using Microsoft.AspNetCore.Identity;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminUserFeature.Commands.DeleteUser
{
    public class DeleteAdminUserCommand : IRequest<bool>
    {
        public string Id { get; set; }
    }

    public class DeleteAdminUserCommandHandler : IRequestHandler<DeleteAdminUserCommand, bool>
    {
        private readonly UserManager<User> userManager;

        public DeleteAdminUserCommandHandler(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<bool> Handle(DeleteAdminUserCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.Id);

            if (user == null)
                throw new Exception("User not found");

            var result = await userManager.DeleteAsync(user);

            return result.Succeeded;
        }
    }
}
