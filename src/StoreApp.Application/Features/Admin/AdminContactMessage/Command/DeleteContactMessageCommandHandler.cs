using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Entities.Order;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminContactMessage.Command
{
    public record DeleteContactMessageCommand(int id) : IRequest<bool>
    {
    }
    public class DeleteContactMessageCommandHandler : IRequestHandler<DeleteContactMessageCommand, bool>
    {
        private readonly IUnitOfWork uow;

        public DeleteContactMessageCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> Handle(DeleteContactMessageCommand request, CancellationToken cancellationToken)
        {
            var msg = await uow.Repository<ContactMessage>().GetByIdAsync(request.id, cancellationToken);

            if (msg == null) throw new NotFoundEntityException("message not found");

            msg.IsDelete = true;
            await uow.Save(cancellationToken);

            var hasAnyMessage = await uow.Repository<ContactMessage>()
                .GetQueryable().AnyAsync(c => c.ConversationId == msg.ConversationId, cancellationToken);

            if (!hasAnyMessage)
            {
                var conv = await uow.Repository<ContactConversation>().GetByIdAsync(msg.ConversationId, cancellationToken);
                conv.IsDelete = true;
                await uow.Save(cancellationToken);
            }

            return true;
        }
    }
}
