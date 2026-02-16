using MediatR;
using Microsoft.AspNetCore.Http;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminContactMessage;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Enums;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminContactMessage.Command
{
    public record AdminReplyContactCommand(int conservationId, AdminReplyDto dto) : IRequest
    {
    }

    public class AdminReplyContactCommandHandler : IRequestHandler<AdminReplyContactCommand>
    {
        private readonly IUnitOfWork uow;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminReplyContactCommandHandler(IUnitOfWork uow, IHttpContextAccessor httpContextAccessor)
        {
            this.uow = uow;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(AdminReplyContactCommand request, CancellationToken cancellationToken)
        {
            var conversation = await uow.Repository<ContactConversation>()
                .GetByIdAsync(request.conservationId, cancellationToken);

            if (conversation == null) throw new NotFoundEntityException("Conversation Not Found");

            var message = new ContactMessage
            {
                ConversationId = request.conservationId,
                Message = request.dto.Message,
                Sender = MessageSender.Admin,
                IsRead = false
            };

            await uow.Repository<ContactMessage>().AddAsync(message, cancellationToken);
            await uow.Save(cancellationToken);

            if (request.dto.Attachments != null && request.dto.Attachments.Any())
            {
                foreach (var file in request.dto.Attachments)
                {
                    var filePath = SaveFile(file);

                    var attachment = new ContactAttachment
                    {
                        ContactMessageId = message.Id,
                        FileName = file.FileName,
                        FilePath = filePath
                    };

                    await uow.Repository<ContactAttachment>().AddAsync(attachment, cancellationToken);
                }
            }

            await uow.Save(cancellationToken);
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
