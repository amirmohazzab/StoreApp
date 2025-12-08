using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreApp.Application.Interfaces;
using StoreApp.Data.Persistence.Context;
using StoreApp.Domain.Entities.User;
using System;

namespace StoreApp.Web.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IPermissionService permissionService, UserManager<User> userManager)
        {
            // گرفتن اکشن فعلی
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            // پیدا کردن Attribute
            var permAttribute = endpoint.Metadata.GetMetadata<HasPermissionAttribute>();
            if (permAttribute == null)
            {
                await _next(context);
                return;
            }

            // اگر کاربر لاگین نیست
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            // گرفتن یوزر
            var user = await userManager.GetUserAsync(context.User);
            if (user == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            // گرفتن پرمیشن‌های یوزر
            var userPermissions = await permissionService.GetUserPermissionsAsync(user);

            // بررسی پرمیشن
            if (!userPermissions.Contains(permAttribute.Permission))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("You do not have permission!");
                return;
            }

            await _next(context);
        }
    }
}
