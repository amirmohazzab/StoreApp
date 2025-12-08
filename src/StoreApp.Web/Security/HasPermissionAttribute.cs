using Microsoft.AspNetCore.Authorization;

namespace StoreApp.Web.Security
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission)
        {
            Policy = permission;
        }
    }
}
