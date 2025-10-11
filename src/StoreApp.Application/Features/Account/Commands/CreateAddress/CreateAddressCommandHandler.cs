using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Common.Mapping;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Account;
using StoreApp.Domain.Entities.User;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Account.Commands.CreateAddress
{
    public class CreateAddressCommand : IRequest<AddressDto>, IMapFrom<Address>
    {
        public bool IsMAIN { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        public string FullName { get; set; }

        public string LastName { get; set; }

        public string FullAddress { get; set; }

        public string Number { get; set; }

        public string PostalCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAddressCommand, Address>();
        }
    }

    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, AddressDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;
        private readonly UserManager<User> userManager;

        public CreateAddressCommandHandler(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ICurrentUserService currentService,
            UserManager<User> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
            this.userManager = userManager;
        }

        public async Task<AddressDto> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var user = await userManager.Users.Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (user == null) throw new NotFoundEntityException();
            if (request.IsMAIN && user.Addresses.Any())
            {
                foreach (var address in user.Addresses)
                {
                    address.IsMAIN = false;
                }
            }
            if (!user.Addresses.Any()) request.IsMAIN = true;

            var entity = mapper.Map<Address>(request);
            entity.UserId = userId;
            user.Addresses.Add(entity);
            var userResult = await userManager.UpdateAsync(user);
            if (!userResult.Succeeded)
                throw new BadRequestEntityException();

            return mapper.Map<AddressDto>(entity);
        }
    }
}
