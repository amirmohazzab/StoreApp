using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.UserLikes.Queries
{
    public class GetUserLikeStatusQuery : IRequest<bool>
    {
        public int ProductId { get; set; }
    }

    public class GetUserLikeStatusQueryHandler : IRequestHandler<GetUserLikeStatusQuery, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public GetUserLikeStatusQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<bool> Handle(GetUserLikeStatusQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            var like = await unitOfWork.Repository<UserLike>()
                .GetQueryable()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == request.ProductId);

            return like?.Liked ?? false;
        }
    }
}
