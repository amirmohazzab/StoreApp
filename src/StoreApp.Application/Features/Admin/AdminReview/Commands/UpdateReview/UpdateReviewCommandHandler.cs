using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminProductReviewDto;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminReview.Commands.UpdateReview
{
    public record UpdateReviewCommand(int ReviewId, string Comment) : IRequest<bool>
    {
    }

    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, bool>
    {
        private readonly IUnitOfWork uow;

        public UpdateReviewCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await uow.Repository<ProductReview>()
            .GetByIdAsync(request.ReviewId, cancellationToken);

            if (review == null)
                throw new NotFoundEntityException("Review not found");

            review.Comment = request.Comment;

            await uow.Save(cancellationToken);
            return true;
        }
    }
}
