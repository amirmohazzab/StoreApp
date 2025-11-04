using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Application.Features.ProductBrandFeature.Queries.GetAll;
using StoreApp.Application.Features.ProductFeature.Queries.Get;
using StoreApp.Application.Features.ProductFeature.Queries.GetAll;
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

        [HttpPost("add-like")]
        public async Task<IActionResult> AddLike([FromQuery] string targetUserName)
        {

        }
    }
}
