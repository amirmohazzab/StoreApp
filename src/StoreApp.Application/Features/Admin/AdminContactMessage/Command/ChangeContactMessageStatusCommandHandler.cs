using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminContactMessage.Command
{
    public record ChangeContactMessageStatusCommand(int messageId, bool IsRead) : IRequest<bool>
    {

    }
    public class ChangeContactMessageStatusCommandHandler : IRequestHandler<ChangeContactMessageStatusCommand, bool>
    {
        private readonly IUnitOfWork uow;

        public ChangeContactMessageStatusCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> Handle(ChangeContactMessageStatusCommand request, CancellationToken cancellationToken)
        {
            var msg = await uow.Repository<ContactMessage>().GetByIdAsync(request.messageId, cancellationToken);

            if (msg == null)
                throw new NotFoundEntityException("Message not found");

            msg.IsRead = request.IsRead;
            await uow.Save(cancellationToken);

            return true;
        }
    }
}
