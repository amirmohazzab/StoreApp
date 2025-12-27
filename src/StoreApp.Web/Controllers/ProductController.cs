using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Application.Features.ProductBrandFeature.Queries.GetAll;
using StoreApp.Application.Features.ProductCategoryFeature.Query;
using StoreApp.Application.Features.ProductFeature.Commands.AddProductReview;
using StoreApp.Application.Features.ProductFeature.Commands.IncrementViewCount;
using StoreApp.Application.Features.ProductFeature.Commands.ToggleProductLike;
using StoreApp.Application.Features.ProductFeature.Commands.ToggleProductWishlist;
using StoreApp.Application.Features.ProductFeature.Queries.Get;
using StoreApp.Application.Features.ProductFeature.Queries.GetAll;
using StoreApp.Application.Features.ProductFeature.Queries.GetAllMostLiked;
using StoreApp.Application.Features.ProductFeature.Queries.GetAllMostViewed;
using StoreApp.Application.Features.ProductFeature.Queries.GetRelated;
using StoreApp.Application.Features.ProductFeature.Queries.GetReview;
using StoreApp.Application.Features.UserLikes.Commands;
using StoreApp.Application.Features.UserLikes.Queries;
using StoreApp.Application.Features.UserProfile.Commands;
using StoreApp.Domain.Entities;

namespace StoreApp.Web.Controllers
{
    public class ProductController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get([FromQuery] GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> Get([FromRoute] int id, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetProductQuery(id), cancellationToken));
        }

        [HttpPost("{productId}/increament-view")]
        public async Task<IActionResult> IncrementViewCount(int productId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new IncrementProductViewCountCommand { ProductId = productId }, cancellationToken);
            if (!result) return NotFound();

            return Ok(new { success = true });
        }

        [HttpGet("by-category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetProductsByCategoryQuery(categoryId), cancellationToken);
            return Ok(result);
        }

        [HttpPost("toggle-like/{productId}")]
        public async Task<IActionResult> ToggleLike(int productId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new ToggleUserLikeCommand(productId), cancellationToken);
            return Ok(result); // ← خروجی درست شامل liked و likeCount
        }

        [HttpGet("{productId}/related")]
        public async Task<IActionResult> GetRelatedProducts(int productId, [FromQuery] int count = 6, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.Send(new GetRelatedProductsQuery
            {
                ProductId = productId,
                Count = count
            }, cancellationToken);

            return Ok(result);
        }

        //[HttpGet("{productId}/review")]
        //public async Task<IActionResult> GetReviews(int productId, CancellationToken cancellationToken)
        //{
        //    return Ok(await Mediator.Send(new GetProductReviewsQuery { ProductId = productId }, cancellationToken));
        //}

        [Authorize]
        [HttpPost("{productId}/review")]
        public async Task<IActionResult> AddReview(int productId, [FromBody] AddReviewCommand command, CancellationToken cancellationToken)
        {
            command.ProductId = productId;
            return Ok(await Mediator.Send(command, cancellationToken));
        }

        [HttpGet("like-status/{productId}")]
        public async Task<IActionResult> GetLikeStatus(int productId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new GetUserLikeStatusQuery { ProductId = productId }, cancellationToken);
            return Ok(new { liked = result });
        }

        [HttpGet("{id}/review")]
        public async Task<IActionResult> GetProductReviews(int id, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 5, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.Send(new GetProductReviewsQuery { ProductId = id, PageIndex = pageIndex, PageSize = pageSize }, cancellationToken);
            return Ok(result);
        }

        [HttpPut("review/{id}")]
        public async Task<IActionResult> EditReview(int id, [FromBody] EditReviewCommand command, CancellationToken cancellationToken)
        {
            command.Id = id;
            var updated = await Mediator.Send(command, cancellationToken);
            return Ok(updated);
        }

        [HttpDelete("review/{id}")]
        public async Task<IActionResult> DeleteReview(int id, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new DeleteReviewCommand { Id = id }, cancellationToken);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("toggle-wishlist/{productId}")]
        public async Task<IActionResult> ToggleWishlist(int productId)
        {
            var result = await Mediator.Send(new ToggleWishlistCommand(productId));
            return Ok(new { status = result });
        }
    }
}
