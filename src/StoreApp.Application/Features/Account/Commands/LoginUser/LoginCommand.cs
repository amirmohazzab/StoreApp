using MediatR;
using StoreApp.Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Account.Commands.LoginUser
{
    public class LoginCommand : IRequest<UserDto>
    {
        public string PhoneNumber { get; set; }

        public string Password { get; set; }
    }
}
