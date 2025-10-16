using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Dtos.OrderDto;
using StoreApp.Application.Features.OrderFeature.Commands.Create;
using StoreApp.Application.Features.OrderFeature.Commands.Verify;
using StoreApp.Application.Features.OrderFeature.Queries.GetDeliveryMethodById;
using StoreApp.Application.Features.OrderFeature.Queries.GetDeliveryMethods;
using StoreApp.Application.Features.OrderFeature.Queries.GetOrderByIdForUser;
using StoreApp.Application.Features.OrderFeature.Queries.GetOrdersForUser;
using StoreApp.Domain.Entities.Order;

namespace StoreApp.Web.Controllers
{
    public class OrderController : BaseApiController
    {
        [HttpPost("CreateOrder")]
        public async Task<ActionResult<OrderDto>> CeateOrder([FromBody] CreateOrderCommand request, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [HttpGet("GetOrdersForUser")]
        public async Task<ActionResult<List<OrderDto>>> GetOrdersForUser(CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetOrdersForUserQuery(), cancellationToken));
        }

        [HttpGet("GetOrderByIdForUser/{id:int}")]
        public async Task<IActionResult> GetOrderByIdForUser([FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetOrderByIdForUserQuery(id), cancellationToken));
        }

        [HttpGet("GetDeliveryMethods")]
        [AllowAnonymous]
        public async Task<ActionResult<List<DeliveryMethod>>> GetDeliveryMethods(CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetDeliveryMethodsQuery(), cancellationToken));
        }

        [HttpGet("GetDeliveryMethodById/{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethodById([FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetDeliveryMethodByIdQuery(id), cancellationToken));
        }

        [HttpGet("Verify")]
        [AllowAnonymous]
        public async Task<IActionResult> Verify(string authority, string status, CancellationToken cancellationToken)
        {
            return Redirect(await Mediator.Send(new VerifyCommand(authority, status), cancellationToken));
        }
    }
}
