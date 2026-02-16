using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Dtos.ContactUs;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Application.Features.ContactMessageFeature.Command;
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

            // [FromQuery] string? sort
            //new GetAllProductsQuery { SortBy = sort }
            return Ok(await Mediator.Send(new GetAllFeaturesQuery(), cancellationToken));
        }

        [HttpGet("most-liked")]
        public async Task<IActionResult> GetMostLikedProducts([FromQuery] int count = 6, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.Send(new GetMostLikedProductsQuery { Count = count }, cancellationToken);
            return Ok(result);
        }

        [HttpGet("most-viewed")]
        public async Task<IActionResult> GetMostViewedProducts([FromQuery] int count = 6, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.Send(new GetMostViewedProductsQuery { Count = count }, cancellationToken);
            return Ok(result);
        }

        [HttpPost("contact-us")]
        public async Task<IActionResult> SendMessage(ContactMessageDto dto, CancellationToken cancellationToken = default)
        {
            var result = await Mediator.Send(new CreateContactMessageCommand(dto), cancellationToken);
            return Ok(result);
        }

        [HttpGet("about-us")]
        public async Task<IActionResult> AboutUs(ContactMessageDto dto, CancellationToken cancellationToken = default)
        {
            return Ok(new 
            {
                title = "About StoreApp",
                description = "StoreApp is a sample e-commerce project built with ASP.NET Core and Angular for portfolio purposes.",
                technologies = new[]
                    {
                        "ASP.NET Core",
                        "Angular",
                        "Entity Framework Core",
                        "CQRS",
                        "Repository Pattern"
                    }
            });
        }
    }
}
