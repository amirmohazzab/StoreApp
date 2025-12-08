using AutoMapper;
using MediatR;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.UserProfile.Queries
{
    public class GetUserReviewsQuery : IRequest<List<ProductReviewDto>> { }

    public class GetUserReviewsQueryHandler : IRequestHandler<GetUserReviewsQuery, List<ProductReviewDto>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;

        public GetUserReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }

        public async Task<List<ProductReviewDto>> Handle(GetUserReviewsQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUserService.UserId;
            if (string.IsNullOrEmpty(userId)) throw new UnauthorizedAccessException();

            var reviews = await unitOfWork.Repository<ProductReview>()
                .GetQueryable()
                .Where(r => r.UserId == userId)
                .Include(r => r.User)
                .Include(r => r.Product) 
                .OrderByDescending(r => r.Created)
                .ToListAsync(cancellationToken);

            return mapper.Map<List<ProductReviewDto>>(reviews);
        }
    }
}
