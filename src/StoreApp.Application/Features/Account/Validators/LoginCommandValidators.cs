using FluentValidation;
using StoreApp.Application.Features.Account.Commands.LoginUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Account.Validators
{
    public class LoginCommandValidators : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidators()
        {
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Please Enter your PhoneNumber");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please Enter your Password");
        }
    }
}
