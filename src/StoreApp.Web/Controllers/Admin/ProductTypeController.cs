using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Features.Admin.AdminProductTypeFeature.Commands.CreateProductType;
using StoreApp.Application.Features.Admin.AdminProductTypeFeature.Commands.DeleteProductType;
using StoreApp.Application.Features.Admin.AdminProductTypeFeature.Commands.UpdateProductType;
using StoreApp.Application.Features.Admin.AdminProductTypeFeature.Queries.GetAll;

namespace StoreApp.Web.Controllers.Admin
{
    public class ProductTypeController : AdminApiBaseController
    {
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await Mediator.Send(new GetProductTypeQuery());
            return Ok(res);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductTypeCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(id);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateProductTypeCommand command)
        {
            var success = await Mediator.Send(command);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await Mediator.Send(new DeleteProductTypeCommand { Id = id });
            return success ? Ok() : BadRequest("Cannot delete category with products");
        }
    }
}
