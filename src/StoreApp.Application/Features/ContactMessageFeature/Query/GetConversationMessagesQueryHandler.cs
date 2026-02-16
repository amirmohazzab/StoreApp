using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ContactMessage;
using StoreApp.Domain.Entities.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ContactMessageFeature.Query
{                 
    public record GetConversationMessagesQuery(int ConversationId) : IRequest<List<UserContactMessageDto>>;

    public class GetConversationMessagesQueryHandler : IRequestHandler<GetConversationMessagesQuery, List<UserContactMessageDto>>
    {
        private readonly IUnitOfWork uow;
        private readonly ICurrentUserService currentUserService;

        public GetConversationMessagesQueryHandler(IUnitOfWork uow, ICurrentUserService currentUserService)
        {
            this.uow = uow;
            this.currentUserService = currentUserService;
        }

        public async Task<List<UserContactMessageDto>> Handle(GetConversationMessagesQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var messages = await uow.Repository<ContactMessage>()
                .GetQueryable()
                .AsNoTracking()
                .Where(m =>
                    m.ConversationId == request.ConversationId &&
                    m.Conversation.UserId == userId)
                .OrderBy(m => m.Created)
                .Select(m => new UserContactMessageDto
                {
                    Id = m.Id,
                    Message = m.Message,
                    Sender = m.Sender,
                    IsRead = m.IsRead,
                    Created = m.Created,
                    Attachments = m.Attachments.Select(a => new ContactAttachmentDto
                    {
                        FileName = a.FileName,
                        FilePath = a.FilePath
                    })
                    .ToList()
                })
                .ToListAsync(cancellationToken);

            return messages;
        }      
    }
}



