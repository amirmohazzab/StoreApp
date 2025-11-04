using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Dtos.Account;
using StoreApp.Application.Features.UserProfile.Commands;
using StoreApp.Application.Features.UserProfile.Queries;

namespace StoreApp.Web.Controllers
{
    public class UserController : BaseApiController
    {
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<AddressDto>> GetUserInfo(CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetUserProfileQuery(), cancellationToken));
        }

        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> EditUserInfo([FromBody] EditUserProfileCommand command, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(command, cancellationToken));
        }
    }
}
