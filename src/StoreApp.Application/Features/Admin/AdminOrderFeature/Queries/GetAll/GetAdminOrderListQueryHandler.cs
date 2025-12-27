using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Contracts;
using StoreApp.Application.Dtos.Admin.AdminOrderDto;
using StoreApp.Application.Wrappers;
using StoreApp.Domain.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Application.Features.Admin.AdminOrderFeature.Queries.GetAll
{
    public record GetAdminOrderListQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedResult<AdminOrderListDto>>;

    public class GetAdminOrderListQueryHandler : IRequestHandler<GetAdminOrderListQuery, PaginatedResult<AdminOrderListDto>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetAdminOrderListQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<PaginatedResult<AdminOrderListDto>> Handle(GetAdminOrderListQuery request, CancellationToken cancellationToken)
        {
            var query = uow.Repository<Order>()
                .GetQueryable()
                .Include(x => x.DeliveryMethod)
                .AsQueryable();

            var totalCount = await query.CountAsync(cancellationToken);

            var data = await query
                .OrderByDescending(x => x.Created)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var mapped = mapper.Map<List<AdminOrderListDto>>(data);

            return new PaginatedResult<AdminOrderListDto>(mapped, totalCount, request.PageNumber, request.PageSize);

            //var orders = await uow.Repository<Order>()
            //.GetQueryable()
            //.Include(x => x.DeliveryMethod)
            //.OrderByDescending(x => x.Created)
            //.ToListAsync(cancellationToken);

            //return mapper.Map<List<AdminOrderListDto>>(orders);
        }
    }
}
