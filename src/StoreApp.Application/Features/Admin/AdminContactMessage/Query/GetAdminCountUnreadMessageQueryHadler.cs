using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.Contact;
using StoreApp.Domain.Entities.User;
using StoreApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminContactMessage.Query
{
    public class GetAdminCountUnreadMessageQuery : IRequest<int>
    {
    }

    public class GetAdminCountUnreadMessageQueryHadler : IRequestHandler<GetAdminCountUnreadMessageQuery, int>
    {
        private readonly IUnitOfWork uow;
        private readonly ICurrentUserService currentUserService;

        public GetAdminCountUnreadMessageQueryHadler(IUnitOfWork uow, ICurrentUserService currentUserService)
        {
            this.uow = uow;
            this.currentUserService = currentUserService;
        }

        public async Task<int> Handle(GetAdminCountUnreadMessageQuery request, CancellationToken cancellationToken)
        {
            return await uow.Repository<ContactConversation>().GetQueryable()
                    .Where(c => c.Messages.Any(m => m.Sender == MessageSender.User && !m.IsRead))
                    .CountAsync(cancellationToken);
        }
    }
}
