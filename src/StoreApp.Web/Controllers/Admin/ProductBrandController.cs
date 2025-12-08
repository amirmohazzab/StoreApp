using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Features.Admin.AdminProductBrandFeature.Commands.CreateProductBrand;
using StoreApp.Application.Features.Admin.AdminProductBrandFeature.Commands.DeleteProductBrand;
using StoreApp.Application.Features.Admin.AdminProductBrandFeature.Commands.UpdatePrpductBrand;
using StoreApp.Application.Features.Admin.AdminProductBrandFeature.Queries.GetAll;
using StoreApp.Application.Features.Admin.AdminProductTypeFeature.Commands.CreateProductType;
using StoreApp.Application.Features.Admin.AdminProductTypeFeature.Commands.DeleteProductType;
using StoreApp.Application.Features.Admin.AdminProductTypeFeature.Commands.UpdateProductType;
using StoreApp.Application.Features.Admin.AdminProductTypeFeature.Queries.GetAll;

namespace StoreApp.Web.Controllers.Admin
{
    public class ProductBrandController : AdminApiBaseController
    {
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var res = await Mediator.Send(new GetProductBrandQuery());
            return Ok(res);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateProductBrandCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(id);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateProductBrandCommand command)
        {
            var success = await Mediator.Send(command);
            return success ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await Mediator.Send(new DeleteProductBrandCommand { Id = id });
            return success ? Ok() : BadRequest("Cannot delete Product Brand with products");
        }
    }
}
