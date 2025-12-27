using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminProductReviewDto;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminReview.Queries.GetMostReviewProduct
{
    public record GetMostReviewedProductsQuery(int Take = 5) : IRequest<List<MostReviewedProductDto>>;

    public class GetMostReviewedProductsQueryHandler : IRequestHandler<GetMostReviewedProductsQuery, List<MostReviewedProductDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetMostReviewedProductsQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<List<MostReviewedProductDto>> Handle(GetMostReviewedProductsQuery request, CancellationToken cancellationToken)
        {
            var query = _uow.Repository<ProductReview>()
            .GetQueryable()
            .Where(r => r.ProductId != null && r.IsApproved == true)
            .GroupBy(r => new
            {
                r.ProductId,
                r.Product!.Title,
                r.Product.PictureUrl
            })
            .Select(g => new MostReviewedProductDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Title,
                ReviewsCount = g.Count(),
                PictureUrl = g.Key.PictureUrl
            })
            .OrderByDescending(x => x.ReviewsCount)
            .Take(request.Take);

            return await query.ToListAsync(cancellationToken);
        }
    }
}

//&& r.Created >= DateTime.UtcNow.AddDays(-30)
