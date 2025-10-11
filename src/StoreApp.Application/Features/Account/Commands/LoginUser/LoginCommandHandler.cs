using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Account;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.User;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Account.Commands.LoginUser
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, UserDto>
    {
        private readonly IMapper mapper;
        private readonly SignInManager<User> signInManager;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITokenService tokenService;

        public LoginCommandHandler(
            IMapper mapper, 
            SignInManager<User> signInManager, 
            IUnitOfWork unitOfWork, 
            ITokenService tokenService)
        {
            this.mapper = mapper;
            this.signInManager = signInManager;
            this.unitOfWork = unitOfWork;
            this.tokenService = tokenService;
        }
        public async Task<UserDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.Context.Set<User>()
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

            if (user == null) throw new BadRequestEntityException("User Not Found, please sign in at the website");

            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password,false);

            if (!result.Succeeded) throw new BadRequestEntityException("UserName or Password wrong");

            var mapUser = mapper.Map<UserDto>(user);
            mapUser.Token = await tokenService.CreateToken(user);

            return mapUser;

        }
    }
}
