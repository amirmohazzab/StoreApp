using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminContactMessage;
using StoreApp.Application.Wrappers;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminContactMessage.Query
{
    public record GetContactMessagesQuery(ContactMessageFilterDto filter) : IRequest<PaginatedResult<AdminContactConversationDto>>;

    public class GetContactMessagesQueryHandler : IRequestHandler<GetContactMessagesQuery, PaginatedResult<AdminContactConversationDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetContactMessagesQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PaginatedResult<AdminContactConversationDto>> Handle(GetContactMessagesQuery request, CancellationToken cancellationToken)
        {
            var query = _uow.Repository<ContactConversation>()
                .GetQueryable()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.filter.Search))
            {
                var search = request.filter.Search.Trim();

                query = query.Where(x =>
                    x.Name.Contains(search) ||
                    x.Email.Contains(search) ||
                    x.Subject.Contains(search) ||
                    x.Messages.Any(m => m.Message.Contains(search))
                );
            }

            if (request.filter.IsRead.HasValue)
            {
                if (request.filter.IsRead.Value)
                {
                    query = query.Where(x =>
                        !x.Messages.Any(m =>
                            !m.IsRead && m.Sender == MessageSender.User));
                }
                else
                {
                    query = query.Where(x =>
                        x.Messages.Any(m =>
                            !m.IsRead && m.Sender == MessageSender.User));
                }
            }

            var total = await query.CountAsync(cancellationToken);

            var data = await query
                .OrderByDescending(x => x.Created)
                .Skip((request.filter.PageNumber - 1) * request.filter.PageSize)
                .Take(request.filter.PageSize)
                .Select(x => new AdminContactConversationDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    Subject = x.Subject,
                    Created = x.Created,
                    HasUnRead = x.Messages.Any(m => !m.IsRead && m.Sender == MessageSender.User),
                    HasReply = x.Messages.Any(m => m.Sender == MessageSender.Admin),
                })
                .ToListAsync(cancellationToken);

            return new PaginatedResult<AdminContactConversationDto>(
                data,
                total,
                request.filter.PageNumber,
                request.filter.PageSize
            );
        }
    }
}


