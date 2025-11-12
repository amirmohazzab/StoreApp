using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Application.Features.ProductFeature.Commands.IncrementViewCount;
using StoreApp.Application.Features.ProductFeature.Queries.GetAll;
using StoreApp.Application.Features.ProductFeature.Queries.GetAllMostLiked;
using StoreApp.Application.Features.ProductFeature.Queries.GetAllMostViewed;
using StoreApp.Application.Features.ProductFeature.Queries.GetAllProducts;

namespace StoreApp.Web.Controllers
{
    public class HomeController : BaseApiController
    {
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts(CancellationToken cancellationToken)
        {
            //[FromQuery] int? categoryId, [FromQuery] int? brandId, [FromQuery] string? search
            //var query = new GetAllProductsQuery
            //{
            //    CategoryId = categoryId,
            //    BrandId = brandId,
            //    Search = search
            //};
            // query instead of new GetAllFeaturesQuery()
            return Ok(await Mediator.Send(new GetAllFeaturesQuery(), cancellationToken));
        }

        [HttpGet("most-liked")]
        public async Task<IActionResult> GetMostLikedProducts([FromQuery] int count = 10, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.Send(new GetMostLikedProductsQuery { Count = count }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("most-viewed")]
        public async Task<IActionResult> GetMostViewedProducts([FromQuery] int count = 10, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.Send(new GetMostViewedProductsQuery { Count = count }, cancellationToken);
            return Ok(result);
        }

        [HttpPost("increament-view/{productId}")]
        public async Task<IActionResult> IncrementViewCount(int productId, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new IncrementProductViewCountCommand { ProductId = productId }, cancellationToken);
            if (!result) return NotFound();

            return Ok(new { success = true });
        }
    }
}
