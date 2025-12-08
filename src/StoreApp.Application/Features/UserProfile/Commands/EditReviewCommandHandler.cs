using AutoMapper;
using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Domain.Entities;
using StoreApp.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.UserProfile.Commands
{
    public class EditReviewCommand : IRequest<ProductReviewDto>
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }

    public class EditReviewCommandHandler : IRequestHandler<EditReviewCommand, ProductReviewDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;
        private readonly IMediator mediator;

        public EditReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
            this.mediator = mediator;
        }

        public async Task<ProductReviewDto> Handle(EditReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await unitOfWork.Repository<ProductReview>().GetByIdAsync(request.Id, cancellationToken);
            if (review == null) throw new NotFoundEntityException("Review not found");

            var userId = currentUserService.UserId ?? throw new UnauthorizedAccessException();
            if (review.UserId != userId) throw new UnauthorizedAccessException("You cannot edit this review");

            review.Rating = request.Rating;
            review.Comment = request.Comment;
            // optional: review.LastModified = DateTime.UtcNow;

            unitOfWork.Repository<ProductReview>().Update(review);
            await unitOfWork.Save(cancellationToken);

            // update product stats
            await mediator.Send(new UpdateProductReviewStatsCommand { ProductId = review.ProductId }, cancellationToken);

            return mapper.Map<ProductReviewDto>(review);
        }
    }
}
