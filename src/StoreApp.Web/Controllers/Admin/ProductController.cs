using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Features.Admin.AdminProductFeature.Commands.CreateProduct;
using StoreApp.Application.Features.Admin.AdminProductFeature.Commands.DeleteProduct;
using StoreApp.Application.Features.Admin.AdminProductFeature.Commands.EditProduct;
using StoreApp.Application.Features.Admin.AdminProductFeature.Queries.Get;
using StoreApp.Application.Features.Admin.AdminProductFeature.Queries.GetAll;

namespace StoreApp.Web.Controllers.Admin
{
    public class ProductController : AdminApiBaseController
    {
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] AdminCreateProductCommand request)
        {
            var result = await Mediator.Send(request);

            return Ok(new
            {
                id = result,
                message = "Product created successfully"
            });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] AdminUpdateProductCommand request)
        {
            var result = await Mediator.Send(request);

            return Ok(new
            {
                success = result,
                message = "Product updated successfully"
            });
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await Mediator.Send(new AdminGetProductByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await Mediator.Send(new AdminGetProductsQuery());
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await Mediator.Send(new AdminDeleteProductCommand(id));

            return Ok(new
            {
                success = result,
                message = "Product deleted successfully"
            });
        }
    }
}
