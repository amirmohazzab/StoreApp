using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace StoreApp.Web.Middleware
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public string Permission { get; }

        public HasPermissionAttribute(string permission)
        {
            Permission = permission;
        }
    }
}
