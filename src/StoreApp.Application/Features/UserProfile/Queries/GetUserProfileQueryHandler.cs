using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Account;
using StoreApp.Application.Dtos.UserProfile;
using StoreApp.Domain.Entities.User;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.UserProfile.Queries
{
    public class GetUserProfileQuery : IRequest<AddressDto>
    {

    }

    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, AddressDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;

        public GetUserProfileQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }

        public async Task<AddressDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var address = await unitOfWork.Repository<Address>()
                .GetAsync(x => x.UserId == userId && x.IsMAIN, cancellationToken);

            if (address == null) throw new NotFoundEntityException("user not found");

            return mapper.Map<AddressDto>(address);
        }
    }
}
