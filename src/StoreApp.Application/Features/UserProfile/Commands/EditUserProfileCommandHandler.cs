using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
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

namespace StoreApp.Application.Features.UserProfile.Commands
{
    public class EditUserProfileCommand : IRequest<AddressDto>
    {
        public string? UserId { get; set; } 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Number { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? FullAddress { get; set; }
        public string? PostalCode { get; set; }
        public string? Email { get; set; }
    }

    public class EditUserProfileCommandHandler : IRequestHandler<EditUserProfileCommand, AddressDto>
    {
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public EditUserProfileCommandHandler(
            ICurrentUserService currentUserService,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this.currentUserService = currentUserService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<AddressDto> Handle(EditUserProfileCommand request, CancellationToken cancellationToken)
        {
            var address = await unitOfWork.Repository<Address>()
            .FirstOrDefaultAsync(a => a.UserId == currentUserService.UserId && a.IsMAIN == true, cancellationToken);

            if (address == null)
                throw new NotFoundEntityException("Address not found for current user");

            mapper.Map(request, address);

            unitOfWork.Repository<Address>().Update(address);
            await unitOfWork.Save(cancellationToken);

            var addressDto = mapper.Map<AddressDto>(address);
            return addressDto;
        }
    }



}
