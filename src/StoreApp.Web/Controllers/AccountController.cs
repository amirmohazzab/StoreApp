using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Dtos.Account;
using StoreApp.Application.Dtos.ProductDto;
using StoreApp.Application.Features.Account.Commands.ChangePassword;
using StoreApp.Application.Features.Account.Commands.CreateAddress;
using StoreApp.Application.Features.Account.Commands.LoginUser;
using StoreApp.Application.Features.Account.Commands.RegisterUser;
using StoreApp.Application.Features.Account.Queries.GetAddresses;
using StoreApp.Application.Features.UserProfile.Commands;
using StoreApp.Application.Features.UserProfile.Queries;
using StoreApp.Domain.Entities.User;
using System.Security.Claims;

namespace StoreApp.Web.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly Cloudinary cloudinary;
        private readonly UserManager<User> userManager;

        public AccountController(Cloudinary cloudinary, UserManager<User> userManager)
        {
            this.cloudinary = cloudinary;
            this.userManager = userManager;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginCommand request, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterCommand request, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }


        [HttpPost("upload-avatar")]
        public async Task<ActionResult> UploadAvatar(IFormFile file)
        {
            var user = await userManager.GetUserAsync(User);

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                Folder = "avatars"
            };

            // اگر قبلاً عکس داشته حذف کن
            if (!string.IsNullOrEmpty(user.AvatarPublicId))
            {
                var deletion = new DeletionParams(user.AvatarPublicId);
                await cloudinary.DestroyAsync(deletion);
            }

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            // ذخیره در دیتابیس
            user.AvatarUrl = uploadResult.SecureUrl?.ToString();
            user.AvatarPublicId = uploadResult.PublicId;

            await userManager.UpdateAsync(user);

            return Ok(new
            {
                url = user.AvatarUrl
            });
        }

        [HttpGet("review")]
        public async Task<ActionResult<List<ProductReviewDto>>> GetMyReviews(CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetUserReviewsQuery(), cancellationToken));
        }

        [HttpDelete("review/{id}")]
        public async Task<IActionResult> DeleteReview(int id, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new DeleteReviewCommand { Id = id }, cancellationToken);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordCommand command)
        {
            command.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var token = await Mediator.Send(command);

            if (token == null)
                return BadRequest("Current password is incorrect or update failed.");

            return Ok(new { message = "Password changed successfully", token });
        }
    }
}
