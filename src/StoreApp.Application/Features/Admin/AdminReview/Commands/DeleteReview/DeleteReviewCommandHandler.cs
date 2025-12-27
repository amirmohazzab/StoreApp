using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminReview.Commands.DeleteReview
{
    public class DeleteReviewCommand : IRequest<bool> 
    {
        public int ReviewId { get; set; }

        public DeleteReviewCommand(int reviewId)
        {
            ReviewId = reviewId;
        }
    };

    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IUnitOfWork uow;

        public DeleteReviewCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await uow.Repository<ProductReview>()
            .GetByIdAsync(request.ReviewId, cancellationToken);

            if (review == null)
                throw new NotFoundEntityException("Review not found");

            uow.Repository<ProductReview>().Delete(review, cancellationToken);
            var result = await uow.Save(cancellationToken);

            await uow.Save(cancellationToken);
            return true;
        }
    }
}
