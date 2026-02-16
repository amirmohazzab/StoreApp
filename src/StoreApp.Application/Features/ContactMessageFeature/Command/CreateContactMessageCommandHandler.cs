using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ContactUs;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ContactMessageFeature.Command
{
    public record CreateContactMessageCommand(ContactMessageDto dto) : IRequest<int>
    {
    }

    public class CreateContactMessageCommandHandler : IRequestHandler<CreateContactMessageCommand, int>
    {
        private readonly IUnitOfWork uow;
        private readonly ICurrentUserService currentUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateContactMessageCommandHandler(
            IUnitOfWork uow,
            ICurrentUserService currentUserService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            this.currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> Handle(CreateContactMessageCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var conversation = await uow.Repository<ContactConversation>()
                    .GetQueryable()
                    .FirstOrDefaultAsync(x =>
                        x.Email == request.dto.Email.Trim().ToLower() &&
                        x.Subject.Trim() == request.dto.Subject.Trim(), cancellationToken);

            if (conversation != null && string.IsNullOrEmpty(conversation.UserId) && !string.IsNullOrEmpty(userId))
            {
                conversation.UserId = userId;
                await uow.Save(cancellationToken);
            }

            if (conversation == null)
            {
                conversation = new ContactConversation
                {
                    Name = request.dto.Name,
                    Email = request.dto.Email.Trim().ToLower(),
                    Subject = request.dto.Subject.Trim(),
                    UserId = string.IsNullOrEmpty(userId) ? null : userId
                };

                await uow.Repository<ContactConversation>().AddAsync(conversation, cancellationToken);
                await uow.Save(cancellationToken);
            }

            var message = new ContactMessage
            {
                ConversationId = conversation.Id,
                Message = request.dto.Message,
                Sender = MessageSender.User,
                IsRead = false,
                Created = DateTime.UtcNow
            };

            await uow.Repository<ContactMessage>().AddAsync(message, cancellationToken);
            await uow.Save(cancellationToken);

            if (request.dto.Attachment != null)
            {
                var filePath = SaveFile(request.dto.Attachment);
                var attachment = new ContactAttachment
                {
                    ContactMessageId = message.Id,
                    FileName = request.dto.Attachment.FileName,
                    FilePath = filePath
                };

                await uow.Repository<ContactAttachment>().AddAsync(attachment, cancellationToken);
            }

            await uow.Save(cancellationToken);
            return conversation.Id;
        }

        private string SaveFile(IFormFile file)
        {
            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "contact-attachments");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(stream);

            var request = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";

            return $"{baseUrl}/contact-attachments/{fileName}";
        }
    }
}
