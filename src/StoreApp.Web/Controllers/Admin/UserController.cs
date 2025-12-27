using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreApp.Application.Features.Admin.AdminDashboard;
using StoreApp.Application.Features.Admin.AdminPermissionFeature.Commands.UpdatePermission;
using StoreApp.Application.Features.Admin.AdminPermissionFeature.Queries.GetPermission;
using StoreApp.Application.Features.Admin.AdminUserFeature.Commands.CreateUser;
using StoreApp.Application.Features.Admin.AdminUserFeature.Commands.DeleteUser;
using StoreApp.Application.Features.Admin.AdminUserFeature.Commands.EditUser;
using StoreApp.Application.Features.Admin.AdminUserFeature.Queries.Get;
using StoreApp.Application.Features.Admin.AdminUserFeature.Queries.GetAll;
using StoreApp.Application.Interfaces;
using StoreApp.Web.Middleware;

namespace StoreApp.Web.Controllers.Admin
{
    public class UserController : AdminApiBaseController
    {
        //[HttpGet]
        ////[HasPermission("user.create")]
        //public async Task<ActionResult> GetUsers([FromQuery] GetAdminUsersQuery query, CancellationToken cancellationToken)
        //{
        //    var result = await Mediator.Send(query, cancellationToken);
        //    return Ok(result);
        //}

        [HttpGet]
        public async Task<ActionResult> GetUsers([FromQuery] string search = "", [FromQuery] string sort = "UserName",
                                         [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var query = new GetAdminUsersQuery
            {
                Search = search,
                Sort = sort,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(string id)
            => Ok(await Mediator.Send(new GetAdminUserByIdQuery { Id = id }));

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(string id, UpdateAdminUserCommand request)
        {
            request.Id = id;
            return Ok(await Mediator.Send(request));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(string id)
            => Ok(await Mediator.Send(new DeleteAdminUserCommand { Id = id }));

        [HttpPut("{userId}/permission")]
        public async Task<IActionResult> UpdateUserPermissions(string userId, [FromBody] List<int> permissionIds)
        {
            await Mediator.Send(new UpdateUserPermissionsCommand
            {
                UserId = userId,
                PermissionIds = permissionIds
            });

            return NoContent();
        }

        [HttpGet("{userId}/permission")]
        public async Task<IActionResult> GetUserPermissions(string userId)
        {
            var perms = await Mediator.Send(new GetUserPermissionsQuery { UserId = userId });
            return Ok(perms);
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult> GetDashboardUsers()
        {
            var result = await Mediator.Send(new GetAdminDashboardStatsQuery());
            return Ok(result.TotalUsers);
        }
    }
}
