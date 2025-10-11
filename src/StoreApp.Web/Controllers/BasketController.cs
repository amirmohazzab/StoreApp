using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Features.BasketFeature.Commands.DeleteBasket;
using StoreApp.Application.Features.BasketFeature.Commands.DeleteItem;
using StoreApp.Application.Features.BasketFeature.Commands.UpdateBasket;
using StoreApp.Application.Features.BasketFeature.Queries.GetBasketById;
using StoreApp.Domain.Entities.Basket;

namespace StoreApp.Web.Controllers
{
    public class BasketController : BaseApiController
    {
        [HttpGet("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> GetBasketById([FromRoute] int basketId, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetBasketByIdQuery(basketId), cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket([FromBody] CustomerBasket basket, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new UpdateBasketCommand(basket), cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{basketId}")]
        public async Task<ActionResult<bool>> DeleteBasket([FromRoute] int basketId, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new DeleteBasketCommand(basketId), cancellationToken));
        }


        [HttpDelete("{basketId:int}/{id:int}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int basketId, [FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new DeleteItemCommand(basketId, id), cancellationToken));
        }
    }
}
