using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminReview.Queries.GetAtLeastOneReview
{
    public class GetAtLeastOneReviewQuery : IRequest<int>
    {
    }
    public class GetAtLeastOneReviewQueryHandler : IRequestHandler<GetAtLeastOneReviewQuery, int>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetAtLeastOneReviewQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetAtLeastOneReviewQuery request, CancellationToken cancellationToken)
        {
            return await unitOfWork.Repository<ProductReview>()
                .GetQueryable()
                .Select(r => r.ProductId)
                .Distinct()
                .CountAsync();
        }
    }
}
