using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Queries.GetReview
{
    public class GetProductReviewsQuery : IRequest<List<ProductReviewDto>>
    {
        public int ProductId { get; set; }
    }

    public class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQuery, List<ProductReviewDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public GetProductReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<List<ProductReviewDto>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await unitOfWork.Repository<ProductReview>()
                .GetQueryable()
                .Where(r => r.ProductId == request.ProductId)
                .Include(r => r.User)
                .OrderByDescending(r => r.Created)
                .ProjectTo<ProductReviewDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return reviews;
        }
    }
}
