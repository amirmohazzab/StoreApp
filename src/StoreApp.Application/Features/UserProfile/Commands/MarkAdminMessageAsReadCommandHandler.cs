using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.UserProfile.Commands
{
    public record MarkAdminMessageAsReadCommand(int ConversationId) : IRequest
    {
    }

    public class MarkAdminMessageAsReadCommandHandler : IRequestHandler<MarkAdminMessageAsReadCommand>
    {
        private readonly ICurrentUserService currentUserService;

        private readonly IUnitOfWork uow;

        public MarkAdminMessageAsReadCommandHandler(ICurrentUserService currentUserService, IUnitOfWork uow)
        {
            this.currentUserService = currentUserService;
            this.uow = uow;
        }

        public async Task Handle(MarkAdminMessageAsReadCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var messages = await uow.Repository<ContactMessage>()
                .GetQueryable()
                .Where(m =>
                    m.ConversationId == request.ConversationId &&
                    m.Sender == MessageSender.Admin &&
                    !m.IsRead &&
                    m.Conversation.UserId == userId)
                .ToListAsync(cancellationToken);

            foreach (var msg in messages)
                msg.IsRead = true;

            await uow.Save(cancellationToken);
        }
    }
}
