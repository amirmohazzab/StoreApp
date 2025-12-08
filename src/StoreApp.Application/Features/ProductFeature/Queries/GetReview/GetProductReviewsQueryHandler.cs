using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Application.Wrappers;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.ProductFeature.Queries.GetReview
{
    public class GetProductReviewsQuery : IRequest<PaginatedResult<ProductReviewDto>>
    {
        public int ProductId { get; set; }
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }

    public class GetProductReviewsQueryHandler : IRequestHandler<GetProductReviewsQuery, PaginatedResult<ProductReviewDto>>
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IMapper mapper;

        public GetProductReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<PaginatedResult<ProductReviewDto>> Handle(GetProductReviewsQuery request, CancellationToken cancellationToken)
        {
            //var reviews = await unitOfWork.Repository<ProductReview>()
            //    .GetQueryable()
            //    .Where(r => r.ProductId == request.ProductId)
            //    .Include(r => r.User)
            //    .OrderByDescending(r => r.Created)
            //    .ProjectTo<ProductReviewDto>(mapper.ConfigurationProvider)
            //    .ToListAsync(cancellationToken);

            //return reviews;

            var query = unitOfWork.Repository<ProductReview>()
            .GetQueryable()
            .Where(r => r.ProductId == request.ProductId)
            .Include(r => r.User)
            .OrderByDescending(r => r.Created);

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var dtos = mapper.Map<List<ProductReviewDto>>(items);

            return new PaginatedResult<ProductReviewDto>(dtos, total, request.PageIndex, request.PageSize);
        }
    }
}
