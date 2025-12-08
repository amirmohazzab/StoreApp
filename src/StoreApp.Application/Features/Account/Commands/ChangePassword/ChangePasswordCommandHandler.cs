using MediatR;
using Microsoft.AspNetCore.Identity;
using StoreApp.Application.Contracts;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.User;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Account.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<string>
    {
        public string UserId { get; set; }  
        
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, string>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService; 

        public ChangePasswordCommandHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUnitOfWork unitOfWork,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<string> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            // پیدا کردن کاربر
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
                throw new NotFoundEntityException("User Not Found");

            // چک کردن رمز فعلی و تغییر به رمز جدید
            var result = await _userManager.ChangePasswordAsync(
                user,
                request.CurrentPassword,
                request.NewPassword
            );

            if (!result.Succeeded)
                throw new BadRequestEntityException("Operation was failed");

            // آپدیت احراز هویت فعلی
            await _signInManager.RefreshSignInAsync(user);

            // ذخیره تغییرات اگر UnitOfWork داری
            await _unitOfWork.Save(cancellationToken);

            var newToken = await _tokenService.CreateToken(user);
            return newToken; 
        }
    }
}
