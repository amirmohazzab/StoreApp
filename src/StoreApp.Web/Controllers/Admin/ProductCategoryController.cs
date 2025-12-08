using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Features.Admin.AdminProductCategoryFeature.Commands.CreateProductCategory;
using StoreApp.Application.Features.Admin.AdminProductCategoryFeature.Commands.DeleteProductCategory;
using StoreApp.Application.Features.Admin.AdminProductCategoryFeature.Commands.UpdareProductCategory;
using StoreApp.Application.Features.Admin.AdminProductCategoryFeature.Queries.GetAll;

namespace StoreApp.Web.Controllers.Admin
{
    public class ProductCategoryController : AdminApiBaseController
    {
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await Mediator.Send(new GetProductCategoriesQuery());
            return Ok(res);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductCategoryCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(id);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateProductCategoryCommand command)
        {
            var success = await Mediator.Send(command);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await Mediator.Send(new DeleteProductCategoryCommand { Id = id });
            return success ? Ok() : BadRequest("Cannot delete category with products");
        }
    }
}
