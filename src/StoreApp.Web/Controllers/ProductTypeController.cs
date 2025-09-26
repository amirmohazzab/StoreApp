using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Features.ProductBrandFeature.Queries.GetAll;
using StoreApp.Application.Features.ProductTypeFeature.Queries.GetAll;
using StoreApp.Domain.Entities;

namespace StoreApp.Web.Controllers
{
    public class ProductTypeController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductType>>> Get(CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetAllProductTypeQuery(), cancellationToken));
        }
    }
}
