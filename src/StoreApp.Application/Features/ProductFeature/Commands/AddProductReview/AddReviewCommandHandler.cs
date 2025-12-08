using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Application.Features.UserProfile.Commands;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Commands.AddProductReview
{
    public class AddReviewCommand : IRequest<ProductReviewDto>
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
    public class AddReviewCommandHandler : IRequestHandler<AddReviewCommand, ProductReviewDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUserService currentUserService;
        private readonly IMapper mapper;
        private readonly IMediator mediator;

        public AddReviewCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IMapper mapper, IMediator mediator)
        {
            this.unitOfWork = unitOfWork;
            this.currentUserService = currentUserService;
            this.mapper = mapper;
            this.mediator = mediator;
        }

        public async Task<ProductReviewDto> Handle(AddReviewCommand request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId ?? throw new UnauthorizedAccessException();

            var review = new ProductReview
            {
                ProductId = request.ProductId,
                UserId = userId,
                Rating = request.Rating,
                Comment = request.Comment,
                Created = DateTime.UtcNow
            };

            await unitOfWork.Repository<ProductReview>().AddAsync(review, cancellationToken);
            await unitOfWork.Save(cancellationToken);

            //var created = await unitOfWork.Repository<ProductReview>()
            //    .Include(r => r.User)
            //    .FirstOrDefaultAsync(r => r.Id == review.Id);

            await mediator.Send(new UpdateProductReviewStatsCommand { ProductId = review.ProductId }, cancellationToken);

            return mapper.Map<ProductReviewDto>(review);
        }
    }
}
