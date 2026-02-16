using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminContactMessage.Command
{
    public record DeleteConversationCommand(int conversationId) : IRequest;

    public class DeleteConversationCommandHandler : IRequestHandler<DeleteConversationCommand>
    {
        private readonly IUnitOfWork uow;

        public DeleteConversationCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task Handle(DeleteConversationCommand request, CancellationToken cancellationToken)
        {
            var conversation = await uow.Repository<ContactConversation>()
                .GetQueryable()
                .Include(x => x.Messages)
                .FirstOrDefaultAsync(x => x.Id == request.conversationId, cancellationToken);

            if (conversation == null)
                throw new NotFoundEntityException("Conversation not found");

            uow.Repository<ContactConversation>().Delete(conversation, cancellationToken);
            await uow.Save(cancellationToken);
        }
    }
}
