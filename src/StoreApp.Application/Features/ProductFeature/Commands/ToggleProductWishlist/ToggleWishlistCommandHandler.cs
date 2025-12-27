using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Commands.ToggleProductWishlist
{
    public record ToggleWishlistCommand : IRequest<bool>
    {
        public int ProductId { get; set; }

        public ToggleWishlistCommand(int productId)
        {
            ProductId = productId;
        }
    }

    public class ToggleWishlistCommandHandler : IRequestHandler<ToggleWishlistCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;

        public ToggleWishlistCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
        }

        public async Task<bool> Handle(ToggleWishlistCommand request, CancellationToken cancellationToken)
        {
            var repo = unitOfWork.Repository<UserWishlist>();

            var userId = currentUserService.UserId; 

            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not authenticated.");

            var existed = await repo.GetQueryable().FirstOrDefaultAsync(
                    x => x.UserId == userId && x.ProductId == request.ProductId, cancellationToken);

            if (existed != null)
            {
                repo.Delete(existed, cancellationToken);
            }
            else
            {
                var newItem = new UserWishlist
                {
                    UserId = userId,
                    ProductId = request.ProductId
                };

                await repo.AddAsync(newItem, cancellationToken);
            }

            await unitOfWork.Save(cancellationToken);
            return true;
        }
    }
}
