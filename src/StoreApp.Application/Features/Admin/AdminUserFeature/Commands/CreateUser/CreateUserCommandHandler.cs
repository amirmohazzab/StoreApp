using MediatR;
using StoreApp.Application.Dtos.Admin.AdminUserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminUserFeature.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<AdminUserDto>
    {
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, AdminUserDto>
    {
        public Task<AdminUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
