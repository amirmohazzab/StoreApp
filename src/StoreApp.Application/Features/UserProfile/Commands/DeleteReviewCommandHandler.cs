using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.UserProfile.Commands
{
    public class DeleteReviewCommand : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IMediator mediator;

        public DeleteReviewCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await unitOfWork.Repository<ProductReview>().GetByIdAsync(request.Id, cancellationToken);
            if (review == null) throw new NotFoundEntityException("Review not found");

            var userId = currentUserService.UserId;
            if (review.UserId != userId) throw new UnauthorizedAccessException("You cannot delete this review");

            unitOfWork.Repository<ProductReview>().Delete(review, cancellationToken);
            await unitOfWork.Save(cancellationToken);

            await mediator.Send(new UpdateProductReviewStatsCommand { ProductId = review.ProductId }, cancellationToken);

            // اختیاری: آپدیت reviewCount/average rating محصول (می‌توان با یک Command جداگانه انجام داد)
            return true;
        }
    }
}
