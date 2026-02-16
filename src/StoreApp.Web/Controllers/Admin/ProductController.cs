using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Dtos.Admin.AdminOrderDto;
using StoreApp.Application.Dtos.Admin.AdminProductReviewDto;
using StoreApp.Application.Features.Admin.AdminApproveReview.Commands;
using StoreApp.Application.Features.Admin.AdminDashboard;
using StoreApp.Application.Features.Admin.AdminOrderDetailFeature.Queries.GetAll;
using StoreApp.Application.Features.Admin.AdminOrderFeature.Queries.GetAll;
using StoreApp.Application.Features.Admin.AdminProductFeature.Commands.CreateProduct;
using StoreApp.Application.Features.Admin.AdminProductFeature.Commands.DeleteProduct;
using StoreApp.Application.Features.Admin.AdminProductFeature.Commands.EditProduct;
using StoreApp.Application.Features.Admin.AdminProductFeature.Queries.Get;
using StoreApp.Application.Features.Admin.AdminProductFeature.Queries.GetAll;
using StoreApp.Application.Features.Admin.AdminReview.Commands.DeleteReview;
using StoreApp.Application.Features.Admin.AdminReview.Commands.UpdateReview;
using StoreApp.Application.Features.Admin.AdminReview.Queries.GetAll;
using StoreApp.Application.Features.Admin.AdminReview.Queries.GetAtLeastOneReview;
using StoreApp.Application.Features.Admin.AdminReview.Queries.GetMostReviewProduct;
using StoreApp.Application.Features.Admin.AdminUserWishList.Queries;

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

        [HttpGet("at-least-one-wishlisted")]
        public async Task<IActionResult> GetAdminWishlist()
        {
            var result = await Mediator.Send(new AdminGetAtLeastOneWishlistedQuery());
            return Ok(result);
        }

        [HttpGet("most-wishlisted-products")]
        public async Task<IActionResult> GetAdminWishlist([FromQuery] int take = 5)
        {
            var result = await Mediator.Send(new AdminGetMostWishlistProductsQuery(take));
            return Ok(result);
        }

        [HttpGet("review")]
        public async Task<IActionResult> GetReviews([FromQuery] ReviewFilterDto filter)
        {
            var result = await Mediator.Send(new GetAdminReviewsQuery(filter));
            return Ok(result);
        }

        [HttpPut("review/update")]
        public async Task<IActionResult> ApproveReview([FromBody] UpdateReviewStatusCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("review/{reviewId}")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] UpdateReviewDto dto)
        {
            dto.ReviewId = reviewId;

            var result = await Mediator.Send(new UpdateReviewCommand(reviewId, dto.Comment));
            return Ok(result);
        }

        [HttpDelete("review/{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            return Ok(await Mediator.Send(new DeleteReviewCommand(reviewId)));
        }

        [HttpGet("most-reviewed-products")]
        public async Task<IActionResult> GetMostReviewedProducts([FromQuery] int take = 5)
        {
            var result = await Mediator.Send(new GetMostReviewedProductsQuery(take));
            return Ok(result);
        }

        [HttpGet("most-added-to-basket")]
        public async Task<IActionResult> GetMostAddedToCartProducts([FromQuery] int take = 5)
        {
            var result = await Mediator.Send(new GetMostAddedToCartProductsQuery(take));
            return Ok(result);
        }

        [HttpGet("most-sold-products")]
        public async Task<IActionResult> GetMostSoldProducts([FromQuery] int take = 5)
        {
            var result = await Mediator.Send(new AdminGetMostSoldProductsQuery(take));
            return Ok(result);
        }

        [HttpGet("review/at-least-one-review")]
        public async Task<IActionResult> GetAtLeastOneReview()
        {
            var result = await Mediator.Send(new GetAtLeastOneReviewQuery());
            return Ok(result);
        }
    }
}
