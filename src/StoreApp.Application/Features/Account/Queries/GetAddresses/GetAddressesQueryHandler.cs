using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Account;
using StoreApp.Domain.Entities.User;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Account.Queries.GetAddresses
{
    public class GetAddressesQuery : IRequest<IEnumerable<AddressDto>>
    {

    }

    public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, IEnumerable<AddressDto>>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        public GetAddressesQueryHandler(ICurrentUserService currentUserService, IMapper mapper, UserManager<User> userManager)
        {
            this.currentUserService = currentUserService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<AddressDto>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
        {
            var user = await userManager.Users.Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == currentUserService.UserId, cancellationToken);

            if (user == null) throw new NotFoundEntityException();

            return mapper.Map<IEnumerable<AddressDto>>(user.Addresses);


        }
    }
}
