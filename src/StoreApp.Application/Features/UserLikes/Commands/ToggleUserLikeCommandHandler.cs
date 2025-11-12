using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Features.ProductFeature.Commands.UpdateLikeCount;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.UserLikes.Commands
{
    public class ToggleUserLikeCommand : IRequest<bool>
    {
        public ToggleUserLikeCommand(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; set; }
    }

    public class ToggleUserLikeCommandHandler : IRequestHandler<ToggleUserLikeCommand, bool>
    {
        private readonly ICurrentUserService currentUserService;

        private readonly IUnitOfWork unitOfWork;

        private readonly IMediator Mediator;

        public ToggleUserLikeCommandHandler(ICurrentUserService currentUserService, IUnitOfWork unitOfWork, IMediator Mediator)
        {
            this.currentUserService = currentUserService;
            this.unitOfWork = unitOfWork;
            this.Mediator = Mediator;
        }

        public async Task<bool> Handle(ToggleUserLikeCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;

            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User not authenticated");

            var existingLike = await unitOfWork.Repository<UserLike>()
                .GetQueryable()
                .FirstOrDefaultAsync(u => u.UserId == userId && u.ProductId == request.ProductId, cancellationToken);

            if (existingLike == null)
            {
                var newlike = new UserLike
                {
                    UserId = userId,
                    ProductId = request.ProductId,
                    Liked = true
                };
                await unitOfWork.Repository<UserLike>().AddAsync(newlike, cancellationToken);
                await unitOfWork.Save(cancellationToken);

                await Mediator.Send(new UpdateProductLikeCountCommand
                {
                    ProductId = request.ProductId,
                    IsLiked = true
                }, cancellationToken);

                return true;
            }
            else
            {
                existingLike.Liked = !existingLike.Liked;
                unitOfWork.Repository<UserLike>().Update(existingLike);
                await unitOfWork.Save(cancellationToken);

                await Mediator.Send(new UpdateProductLikeCountCommand
                {
                    ProductId = request.ProductId,
                    IsLiked = existingLike.Liked
                }, cancellationToken);

                return existingLike.Liked;
            }
        }
    }
}
