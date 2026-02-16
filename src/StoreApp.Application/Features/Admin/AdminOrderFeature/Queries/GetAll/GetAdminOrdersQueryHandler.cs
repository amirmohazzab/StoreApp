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
    public record GetAdminOrdersQuery(AdminOrderFilterDto Filter) : IRequest<PaginatedResult<AdminOrderDto>>;

    public class GetAdminOrdersQueryHandler : IRequestHandler<GetAdminOrdersQuery, PaginatedResult<AdminOrderDto>>
    {
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public GetAdminOrdersQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<PaginatedResult<AdminOrderDto>> Handle(GetAdminOrdersQuery request, CancellationToken cancellationToken)
        {
            var query = uow.Repository<Order>()
                .GetQueryable()
                .Include(x => x.User)
                .Include(x => x.OrderItems).ThenInclude(x => x.ItemOrdered)
                .Include(o => o.ShipToAddress)
                .Include(x => x.DeliveryMethod)
                .Include(x => x.OrderItems)
                .AsQueryable();

            var f = request.Filter;

            if (!string.IsNullOrEmpty(f.BuyerPhoneNumber))
                query = query.Where(o => o.User != null && o.BuyerPhoneNumber.Contains(f.BuyerPhoneNumber));

            if (f.Status.HasValue)
                query = query.Where(o => o.OrderStatus == f.Status.Value);

            if (f.FromDate.HasValue)
                query = query.Where(o => o.Created >= f.FromDate.Value);

            if (f.ToDate.HasValue)
                query = query.Where(o => o.Created <= f.ToDate.Value);

            if (!string.IsNullOrEmpty(f.SortBy))
            {
                query = f.SortBy switch
                {
                    "Id" => f.SortDesc
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id),

                    "BuyerPhoneNumber" => f.SortDesc
                        ? query.OrderByDescending(x => x.BuyerPhoneNumber)
                        : query.OrderBy(x => x.BuyerPhoneNumber),

                    "OrderStatus" => f.SortDesc
                        ? query.OrderByDescending(x => x.OrderStatus)
                        : query.OrderBy(x => x.OrderStatus),

                    "Created" => f.SortDesc
                        ? query.OrderByDescending(x => x.Created)
                        : query.OrderBy(x => x.Created),

                    _ => query.OrderByDescending(x => x.Created)
                };
            }

            query = query.OrderByDescending(o => o.Created);
            
            //if (f.Status.HasValue)
            //{
            //    query = query.Where(o => o.OrderStatus == f.Status.Value);
            //}
            //else if (!string.IsNullOrEmpty(f.SortBy))
            //{
            //    query = f.SortDesc
            //        ? query.OrderByDescending(e => EF.Property<object>(e, f.SortBy))
            //        : query.OrderBy(e => EF.Property<object>(e, f.SortBy));
            //}
            //else
            //{
            //    query = query.OrderByDescending(o => o.Created);
            //}


            var total = await query.CountAsync(cancellationToken);

            query = query.OrderByDescending(x => x.Created);

            var pageNumber = f.PageNumber <= 0 ? 1 : f.PageNumber;
            var pageSize = f.PageSize <= 0 ? 10 : f.PageSize;

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var mapped = mapper.Map<List<AdminOrderDto>>(data);

            return new PaginatedResult<AdminOrderDto>(mapped, total, pageNumber, pageSize);
            // var orders = await uow.Repository<Order>()
            //.GetQueryable()
            //.Include(x => x.User)
            //.Include(x => x.DeliveryMethod)
            //.Include(x => x.OrderItems)
            //.OrderByDescending(x => x.Created)
            //.ToListAsync(cancellationToken);

            // return mapper.Map<List<AdminOrderDto>>(orders);
        }
    }
}
