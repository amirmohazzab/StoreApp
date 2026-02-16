using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Dtos.Account;
using StoreApp.Application.Features.Account.Commands.CreateAddress;
using StoreApp.Application.Features.Account.Queries.GetAddresses;
using StoreApp.Application.Features.Admin.AdminContactMessage.Command;
using StoreApp.Application.Features.Admin.AdminContactMessage.Query;
using StoreApp.Application.Features.ContactMessageFeature.Query;
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

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<IEnumerable<AddressDto>>> GetAddresses(CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetAddressesQuery(), cancellationToken));
        }

        [Authorize]
        [HttpPost("create-address")]
        public async Task<ActionResult<AddressDto>> CreateAddresses([FromBody] CreateAddressCommand request, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [Authorize]
        [HttpPut("conversations/{id}/read-admin-messages")]
        public async Task<IActionResult> ReadAdminMessages(int id)
        {
            await Mediator.Send(new MarkAdminMessageAsReadCommand(id));
            return Ok();
        }

        [HttpGet("conversations/{id}/messages")]
        public async Task<IActionResult> GetConversationMessages(int id)
        {
            var result = await Mediator.Send(new GetConversationMessagesQuery(id));
            return Ok(result);
        }
    }
}
