using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminApproveReview.Commands
{
    public record ApproveReviewCommand(int ReviewId, bool Approve) : IRequest<bool>;

    public class ApproveReviewCommandHandler : IRequestHandler<ApproveReviewCommand, bool>
    {
        private readonly IUnitOfWork uow;

        public ApproveReviewCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> Handle(ApproveReviewCommand request, CancellationToken cancellationToken)
        {
            var repo = uow.Repository<ProductReview>();

            var review = await repo.GetByIdAsync(request.ReviewId, cancellationToken);
            if (review == null) return false;

            review.IsApproved = request.Approve;

            await uow.Save(cancellationToken);
            return true;
        }
    }
}
