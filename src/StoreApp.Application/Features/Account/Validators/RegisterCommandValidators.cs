using FluentValidation;
using StoreApp.Application.Features.Account.Commands.RegisterUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Account.Validators
{
    public class RegisterCommandValidators : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidators()
        {
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Please Enter your PhoneNumber");
            RuleFor(x => x.DisplayName).NotEmpty().WithMessage("Please Enter your DisplayName");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Please Enter your Password");
        }
    }
}
