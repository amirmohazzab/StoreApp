using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Features.BasketFeature.Commands.AddToBasket;
using StoreApp.Application.Features.BasketFeature.Commands.ClearBasket;
using StoreApp.Application.Features.BasketFeature.Commands.DeleteBasket;
using StoreApp.Application.Features.BasketFeature.Commands.DeleteBasketItem;
using StoreApp.Application.Features.BasketFeature.Commands.DeleteItem;
using StoreApp.Application.Features.BasketFeature.Commands.UpdateBasket;
using StoreApp.Application.Features.BasketFeature.Queries.GetBasketById;
using StoreApp.Application.Features.BasketFeature.Queries.GetBasketsForUser;
using StoreApp.Domain.Entities.Basket;

namespace StoreApp.Web.Controllers
{
    public class BasketController : BaseApiController
    {
        [HttpGet("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> GetBasketById([FromRoute] string basketId, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetBasketByIdQuery(basketId), cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket([FromBody] CustomerBasket basket, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new UpdateBasketCommand(basket), cancellationToken));
        }

        [HttpDelete("{basketId}")]
        public async Task<ActionResult<bool>> DeleteBasket([FromRoute] string basketId, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new DeleteBasketCommand(basketId), cancellationToken));
        }


        [HttpDelete("DeleteItem/{basketId}/{productId:int}")]
        public async Task<IActionResult> DeleteItem([FromRoute] string basketId, [FromRoute] int productId, CancellationToken cancellationToken)
        {
            //return Ok(await Mediator.Send(new DeleteItemCommand(basketId, productId), cancellationToken));
            
            Console.WriteLine($"🟡 DELETE called with basketId = {basketId}, productId = {productId}");

            var result = await Mediator.Send(new DeleteItemCommand(basketId, productId), cancellationToken);

            if (result == null)
                return NotFound($"Basket with id '{basketId}' was not found.");

            return Ok(result);
        }

        [HttpGet("getBasketsForUser")]
        public async Task<ActionResult<List<CustomerBasket>>> GetAllBaskets(CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetBasketsForUserQuery(), cancellationToken));
        }

        [HttpPost("add-item")]
        public async Task<ActionResult<CustomerBasket>> AddToBasket([FromBody] CustomerBasket basket, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new AddToBasketCommand(basket), cancellationToken));
        }

        [HttpDelete("removeItem")]
        public async Task<ActionResult<CustomerBasket>> RemoveItem(string basketId, int productId)
        {
            var result = await Mediator.Send(new RemoveBasketItemCommand(basketId, productId));
            return Ok(result);
        }

        [HttpDelete("clear/{basketId}")]
        public async Task<IActionResult> ClearBasket(string basketId)
        {
            var result = await Mediator.Send(new ClearBasketCommand(basketId));
            if (!result) return NotFound();

            return Ok(new { message = "Basket cleared successfully" });
        }
    }
}
