using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminProductReviewDto;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminApproveReview.Commands
{
    public record UpdateReviewStatusCommand(UpdateReviewStatusDto dto) : IRequest<bool>;

    public class UpdateReviewStatusCommandHandler : IRequestHandler<UpdateReviewStatusCommand, bool>
    {
        private readonly IUnitOfWork uow;

        public UpdateReviewStatusCommandHandler(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<bool> Handle(UpdateReviewStatusCommand request, CancellationToken cancellationToken)
        {
            var repo = uow.Repository<ProductReview>();

            var review = await repo.GetByIdAsync(request.dto.ReviewId, cancellationToken);
            if (review == null) return false;

            review.Status = request.dto.Status;

            await uow.Save(cancellationToken);
            return true;
        }
    }
}
