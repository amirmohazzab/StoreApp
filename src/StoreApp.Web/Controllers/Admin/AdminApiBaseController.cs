using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StoreApp.Web.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class AdminApiBaseController : ControllerBase
    {
        private ISender mediatr = null;

        protected ISender Mediator => mediatr ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
