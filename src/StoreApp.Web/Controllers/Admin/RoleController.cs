using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Features.Admin.AdminPermissionFeature.Commands.UpdatePermission;
using StoreApp.Application.Features.Admin.AdminPermissionFeature.Queries.GetPermission;
using StoreApp.Domain.Entities.User;

namespace StoreApp.Web.Controllers.Admin
{
    public class RoleController : AdminApiBaseController
    {
        [HttpGet("permission")]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await Mediator.Send(new GetAllPermissionsQuery());
            return Ok(permissions);
        }

        [HttpGet("{roleId}/permission")]
        public async Task<IActionResult> GetRolePermissions(string roleId)
        {
            var perms = await Mediator.Send(new GetRolePermissionsQuery(roleId));
            return Ok(perms);
        }

        [HttpPut("{userId}/permission")]
        public async Task<IActionResult> UpdateUserPermissions(
     string userId,
     [FromBody] UpdateUserPermissionsCommand command)
        {
            command.UserId = userId;
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
