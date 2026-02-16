using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ContactMessage;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Entities.User;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ContactMessageFeature.Query
{
    public class GetContactConversationQuery : IRequest<List<UserContactConversationDto>>
    {
    }

    public class GetContactConversationQueryHandler : IRequestHandler<GetContactConversationQuery, List<UserContactConversationDto>>
    {
        private readonly IUnitOfWork uow;
        private readonly ICurrentUserService currentUserService;

        public GetContactConversationQueryHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
        {
            this.uow = uow;
            this.currentUserService = currentUserService;
        }

        public async Task<List<UserContactConversationDto>> Handle(GetContactConversationQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            var email = currentUserService.Email;

            await uow.Repository<ContactConversation>()
                .GetQueryable()
                .Where(x => x.UserId == null && x.Email == email)
                .ExecuteUpdateAsync(x => x.SetProperty(c => c.UserId, userId), cancellationToken);

            var conversations = await uow.Repository<ContactConversation>()
                .GetQueryable()
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Messages.Any() ? x.Messages.Max(m => m.Created) : x.Created)
                .Select(x => new UserContactConversationDto
                {
                    Id = x.Id,
                    Subject = x.Subject,
                    Created = x.Created,
                    HasUnreadByAdmin = x.Messages.Any(m => m.Sender == MessageSender.User && !m.IsRead),
                    HasReply = x.Messages.Any(m => m.Sender == MessageSender.Admin)
                })
                .ToListAsync(cancellationToken);

            return conversations;
        }
    }
}
