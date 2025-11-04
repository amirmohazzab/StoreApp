using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Common.Mapping;
using StoreApp.Application.Dtos.Account;
using StoreApp.Application.Interfaces;
using StoreApp.Domain.Entities.User;
using StoreApp.Domain.Enums;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Account.Commands.RegisterUser
{
    public class RegisterCommand : IRequest<UserDto>, IMapFrom<User>
    {
        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public string DisplayName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterCommand, User>()
                .ForMember(x => x.UserName, c => c.MapFrom(v => v.PhoneNumber));
        }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserDto>
    {
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        private readonly ITokenService tokenService;

        public RegisterCommandHandler(IMapper mapper, UserManager<User> userManager, ITokenService tokenService)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        public async Task<UserDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var checkUser = await userManager.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber);
            if (checkUser) throw new BadImageFormatException("PhoneNumger is Repeated");

            var user = mapper.Map<User>(request);
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) throw new BadRequestEntityException(result.Errors.FirstOrDefault().Description);

            var roleResult = await userManager.AddToRoleAsync(user, RoleType.User.ToString());
            if (!roleResult.Succeeded) throw new BadRequestEntityException(roleResult.Errors.FirstOrDefault().Description);

            var mapUser = mapper.Map<UserDto>(user);
            mapUser.Token = await tokenService.CreateToken(user);

            return mapUser;
        }
    }
}
