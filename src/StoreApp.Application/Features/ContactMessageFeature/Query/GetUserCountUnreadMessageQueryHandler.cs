using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ContactMessageFeature.Query
{
    public record GetUserCountUnreadMessageQuery : IRequest<int>
    {
    }

    public class GetUserCountUnreadMessageQueryHandler : IRequestHandler<GetUserCountUnreadMessageQuery, int>
    {
        private readonly IUnitOfWork uow;
        private readonly ICurrentUserService currentUserService;

        public GetUserCountUnreadMessageQueryHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
        {
            this.uow = uow;
            this.currentUserService = currentUserService;
        }

        public async Task<int> Handle(GetUserCountUnreadMessageQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            return await uow.Repository<ContactMessage>().GetQueryable()
                .CountAsync(m => m.Sender == MessageSender.Admin && !m.IsRead && m.Conversation.UserId == userId); ;
        }
    }
}
