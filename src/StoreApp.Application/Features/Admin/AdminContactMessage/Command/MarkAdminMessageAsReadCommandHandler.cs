using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Features.UserProfile.Commands;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminContactMessage.Command
{
    public record MarkAdminMessagesAsReadCommand(int ConversationId) : IRequest;
    public class MarkAdminMessagesAsReadCommandHandler
    : IRequestHandler<MarkAdminMessagesAsReadCommand>
    {
        private readonly IUnitOfWork uow;
        private readonly ICurrentUserService currentUserService;

        public MarkAdminMessagesAsReadCommandHandler(
            IUnitOfWork uow,
            ICurrentUserService currentUserService)
        {
            this.uow = uow;
            this.currentUserService = currentUserService;
        }

        public async Task Handle(
            MarkAdminMessagesAsReadCommand request,
            CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var messages = await uow.Repository<ContactMessage>()
                .GetQueryable()
                .Where(m =>
                    m.ConversationId == request.ConversationId &&
                    m.Conversation.UserId == userId &&
                    m.Sender == MessageSender.Admin &&
                    !m.IsRead)
                .ToListAsync(cancellationToken);

            foreach (var msg in messages)
            {
                msg.IsRead = true;
            }

            await uow.Save(cancellationToken);
        }
    }
}
