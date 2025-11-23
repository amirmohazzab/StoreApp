using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Features.UserLikes.Commands;
using StoreApp.Application.Features.UserLikes.Queries;

namespace StoreApp.Web.Controllers
{
    [Authorize]
    public class UserLikeController : BaseApiController
    {
        [HttpPost("toggle/{productId}")]
        public async Task<IActionResult> ToggleLike(int productId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new ToggleUserLikeCommand(productId), cancellationToken);
            return Ok(new { liked = result});
        }

        [HttpGet("liked-products")]
        public async Task<IActionResult> GetLikedProducts(CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetUserLikedProductsQuery(), cancellationToken));
        }

        

        //    var baseUrl = $"{Request.Scheme}://{Request.Host}/";
        //return Ok(await Mediator.Send(new GetUserLikedProductsQuery { BaseUrl = baseUrl
        //}, cancellationToken));


    }
}
