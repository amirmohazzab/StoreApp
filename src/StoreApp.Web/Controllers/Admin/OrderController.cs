using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Dtos.Admin.AdminOrderDto;
using StoreApp.Application.Features.Admin.AdminOrderDetailFeature.Queries.GetAll;
using StoreApp.Application.Features.Admin.AdminOrderFeature.Commands;
using StoreApp.Application.Features.Admin.AdminOrderFeature.Queries.GetAll;
using StoreApp.Application.Features.Admin.AdminOrderFeature.Queries.OrderPaymentStatus;
using StoreApp.Application.Features.Admin.AdminOrderFeature.Queries.TotalRevenue;
using StoreApp.Application.Features.Admin.AdminProductFeature.Queries.GetAll;
using StoreApp.Domain.Enums;

namespace StoreApp.Web.Controllers.Admin
{
    public class OrderController : AdminApiBaseController
    {
        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] AdminOrderFilterDto filter)
        {
            var result = await Mediator.Send(new GetAdminOrdersQuery(filter));
            return Ok(result);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetOrders([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        //{
        //    var result = await Mediator.Send(new GetAdminOrderListQuery(pageNumber, pageSize));

        //    return Ok(result);
        //}

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderDetail(int orderId)
        {
            var result = await Mediator.Send(new GetOrderDetailQuery(orderId));
            return Ok(result);
        }

        [HttpPut("change-status")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusDto dto)
        {
            var result = await Mediator.Send(new UpdateOrderStatusCommand(dto));
            return Ok(result);
        }

        [HttpGet("Payment-status")]
        public async Task<IActionResult> GetPaymentStatus()
        {
            var result = await Mediator.Send(new OrderPaymentStatusQuery());
            return Ok(result);
        }

        [HttpGet("stats/total-revenue")]
        public async Task<IActionResult> GetTotalRevenue()
        {
            var result = await Mediator.Send(new TotalRevenueQuery());
            return Ok(result);
        }
    }
}