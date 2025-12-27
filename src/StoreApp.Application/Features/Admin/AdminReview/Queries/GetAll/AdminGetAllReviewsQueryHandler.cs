using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminProductReviewDto;
using StoreApp.Application.Wrappers;
using StoreApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminReview.Queries.GetAll
{
    public record GetAdminReviewsQuery(ReviewFilterDto filter) : IRequest<AdminReviewsResponseDto>;

    public class AdminGetAllReviewsQueryHandler : IRequestHandler<GetAdminReviewsQuery, AdminReviewsResponseDto>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AdminGetAllReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<AdminReviewsResponseDto> Handle(GetAdminReviewsQuery request, CancellationToken cancellationToken)
        {
            var query = unitOfWork.Repository<ProductReview>()
                .GetQueryable()
                .Include(x => x.Product)
                .Include(x => x.User)
                .AsQueryable();

            var f = request.filter;

            if (!string.IsNullOrWhiteSpace(f.ProductName))
                query = query.Where(x => x.Product != null &&
                                         x.Product.Title.Contains(f.ProductName));

            if (!string.IsNullOrWhiteSpace(f.UserName))
                query = query.Where(x => x.User != null && 
                                         x.User.UserName.Contains(f.UserName));

            if (f.Rating.HasValue)
                query = query.Where(x => x.Rating != null && x.Rating == f.Rating.Value);

            if (!string.IsNullOrWhiteSpace(f.Text))
                query = query.Where(x => x.Comment != null && x.Comment.Contains(f.Text));

            if (f.FromDate.HasValue)
                query = query.Where(x => x.Created != null && x.Created >= f.FromDate.Value);

            if (f.ToDate.HasValue)
                query = query.Where(x => x.Created != null && x.Created <= f.ToDate.Value);

            //if (f.IsApproved.HasValue)
            //{
            //    query = query.Where(x => x.IsApproved == f.IsApproved);
            //}

            //if (f.IsApproved == null)
            //{
            //    // Pending → فقط null
            //    query = query.Where(x => x.IsApproved == null);
            //}
            //else if (f.IsApproved == true)
            //{
            //    query = query.Where(x => x.IsApproved == true);
            //}
            //else if (f.IsApproved == false)
            //{
            //    query = query.Where(x => x.IsApproved == false);
            //}
            
                if (f.IsApproved == null)
                {
                    // Pending
                    query = query.Where(x => x.IsApproved == null);
                }
                else
                {
                    // Approved یا Rejected
                    query = query.Where(x => x.IsApproved == f.IsApproved);
                }
           

            var total = await query.CountAsync(cancellationToken);

            query = query
                .OrderBy(x =>
                    x.IsApproved == null ? 0 : x.IsApproved == true ? 1 : 2)
                .ThenByDescending(x => x.Created);

            //query = query.OrderBy(x => x.IsApproved == null ? 0 : 1) 
            //    .ThenByDescending(x => x.Created);

            //query = query.OrderBy(x => x.IsApproved.HasValue)   
            //        .ThenByDescending(x => x.Created);

            var pageNumber = f.PageNumber <= 0 ? 1 : f.PageNumber;
            var pageSize = f.PageSize <= 0 ? 5 : f.PageSize;

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var mapped = mapper.Map<List<AdminProductReviewDto>>(data);

            var pendingCount = await unitOfWork.Repository<ProductReview>()
                    .GetQueryable()
                    .CountAsync(x => x.IsApproved == null, cancellationToken);

            var approvedCount = await unitOfWork.Repository<ProductReview>()
                    .GetQueryable()
                    .CountAsync(x => x.IsApproved == true, cancellationToken);

            var rejectedCount = await unitOfWork.Repository<ProductReview>()
                    .GetQueryable()
                    .CountAsync(x => x.IsApproved == false, cancellationToken);

            var result = new PaginatedResult<AdminProductReviewDto>(mapped, total, pageNumber, pageSize);

            return new AdminReviewsResponseDto
            {
                Reviews = result,
                PendingCount = pendingCount,
                ApprovedCount = approvedCount,
                RejectedCount = rejectedCount
            };
        }
    }
}
