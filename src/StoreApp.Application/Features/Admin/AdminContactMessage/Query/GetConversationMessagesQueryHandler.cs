using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminContactMessage;
using StoreApp.Domain.Entities.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminContactMessage.Query
{
    public record GetAdminConversationMessagesQuery(int conversationId) : IRequest<List<AdminContactMessageDto>>
    {
    }

    public class GetConversationMessagesQueryHandler : IRequestHandler<GetAdminConversationMessagesQuery, List<AdminContactMessageDto>>
    {
        private readonly IUnitOfWork uow;

        public GetConversationMessagesQueryHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<AdminContactMessageDto>> Handle(GetAdminConversationMessagesQuery request, CancellationToken cancellationToken)
        {
            return await uow.Repository<ContactMessage>()
            .GetQueryable()
            .AsNoTracking()
            .Where(m => m.ConversationId == request.conversationId)
            .OrderBy(m => m.Created)
            .Select(m => new AdminContactMessageDto
            {
                Id = m.Id,
                Message = m.Message,
                Sender = m.Sender,
                IsRead = m.IsRead,
                Created = m.Created,
                Attachments = m.Attachments.Select(a => new AdminContactAttachmentDto
                {
                    FileName = a.FileName,
                    FilePath = a.FilePath
                }).ToList()
            })
            .ToListAsync(cancellationToken);
        }
    }
}
