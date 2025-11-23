using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Commands.ToggleProductLike
{
    public class ToggleProductLikeCommand : IRequest<Object>
    {
        public int ProductId { get; set; }

        public ToggleProductLikeCommand(int productId)
        {
            ProductId = productId;
        }
    }

    public class ToggleProductLikeCommandHandler : IRequestHandler<ToggleProductLikeCommand, Object>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ToggleProductLikeCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<object> Handle(ToggleProductLikeCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            if (userId == null) throw new UnauthorizedAccessException();

            var userLikeRepo = unitOfWork.Repository<UserLike>();
            var like = await userLikeRepo.GetQueryable()
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == request.ProductId, cancellationToken);

            bool isLiked;
            var product = await unitOfWork.Repository<Product>().GetByIdAsync(request.ProductId, cancellationToken);

            if (like == null)
            {
                like = new UserLike
                {
                    UserId = userId,
                    ProductId = request.ProductId,
                    Liked = true
                };
                await userLikeRepo.AddAsync(like, cancellationToken);
                product.LikeCount++;
                isLiked = true;
            }
            else
            {
                like.Liked = !like.Liked;
                product.LikeCount += like.Liked ? 1 : -1;
                userLikeRepo.Update(like);
                isLiked = like.Liked;
            }

            unitOfWork.Repository<Product>().Update(product);
            await unitOfWork.Save(cancellationToken);

            return new
            {
                liked = isLiked,
                likesCount = product.LikeCount
            };
        }
    }
}
