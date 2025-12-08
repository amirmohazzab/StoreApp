using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using StoreApp.Application.Interfaces;
using StoreApp.Data.Persistence.Context;
using StoreApp.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data.Security
{
    using Microsoft.AspNetCore.Authorization;

    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionService _permissionService;

        public PermissionHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return;

            var permissions = await _permissionService.GetEffectiveUserPermissionsAsync(userId);

            if (permissions.Contains(requirement.Permission))
                context.Succeed(requirement);
        }
    }
}
