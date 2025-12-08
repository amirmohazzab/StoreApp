using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.UserProfile.Commands
{
    public class UpdateProductReviewStatsCommand : IRequest<int>
    {
        public int ProductId { get; set; }
    }

    public class UpdateProductReviewStatsCommandHandler : IRequestHandler<UpdateProductReviewStatsCommand, int>
    {
        private readonly IUnitOfWork unitOfWork;

        public UpdateProductReviewStatsCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(UpdateProductReviewStatsCommand request, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.Repository<Product>().GetByIdAsync(request.ProductId, cancellationToken);
            if (product == null) return 0;

            var reviews = await unitOfWork.Repository<ProductReview>()
                .GetQueryable()
                .Where(r => r.ProductId == request.ProductId)
                .ToListAsync(cancellationToken);

            product.ReviewCount = reviews.Count;
            product.AverageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

            unitOfWork.Repository<Product>().Update(product);
            await unitOfWork.Save(cancellationToken);

            return product.ReviewCount;
        }
    }
}
