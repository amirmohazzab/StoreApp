using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ContactMessageFeature.Command
{
    public record MarkContactMessageAsReadCommand(int messageId) : IRequest
    {
    }
    public class MarkContactMessageAsReadCommandHandler : IRequestHandler<MarkContactMessageAsReadCommand>
    {
        private readonly IUnitOfWork uow;

        public MarkContactMessageAsReadCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task Handle(MarkContactMessageAsReadCommand request, CancellationToken cancellationToken)
        {
            var message = await uow.Repository<ContactMessage>()
                   .GetQueryable()
                   .FirstOrDefaultAsync(x => x.Id == request.messageId, cancellationToken);

            if (message == null)
                throw new NotFoundEntityException("Message not found");

            message.IsRead = true;

            await uow.Save(cancellationToken);

        }
    }
}
