using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Features.ProductFeature.Commands.UpdateLikeCount;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.UserLikes.Commands
{
    public class ToggleUserLikeCommand : IRequest<ToggleLikeResult>
    {
        public ToggleUserLikeCommand(int productId)
        {
            ProductId = productId;
        }

        public int ProductId { get; set; }
    }

    public class ToggleUserLikeCommandHandler : IRequestHandler<ToggleUserLikeCommand, ToggleLikeResult>
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

        public async Task<ToggleLikeResult> Handle(ToggleUserLikeCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                throw new UnauthorizedAccessException("User not authenticated");

            var product = await unitOfWork.Repository<Product>()
                .GetByIdAsync(request.ProductId, cancellationToken);

            if (product == null)
                throw new Exception("Product not found");

            var existingLike = await unitOfWork.Repository<UserLike>()
                .GetQueryable()
                .FirstOrDefaultAsync(u => u.UserId == userId && u.ProductId == request.ProductId, cancellationToken);

            bool isLiked;

            if (existingLike == null)
            {
                var newLike = new UserLike
                {
                    UserId = userId,
                    ProductId = request.ProductId,
                    Liked = true
                };

                await unitOfWork.Repository<UserLike>().AddAsync(newLike, cancellationToken);

                product.LikeCount++;
                isLiked = true;
            }
            else
            {
                existingLike.Liked = !existingLike.Liked;
                isLiked = existingLike.Liked; // ← حتما مقداردهی کن

                if (isLiked)
                    product.LikeCount++;
                else
                    product.LikeCount--;

                if (product.LikeCount < 0)
                    product.LikeCount = 0;

                unitOfWork.Repository<UserLike>().Update(existingLike);
            }

            unitOfWork.Repository<Product>().Update(product);

            await unitOfWork.Save(cancellationToken);

            return new ToggleLikeResult
            {
                Liked = isLiked,
                LikeCount = product.LikeCount
            };
        }
    }
}
