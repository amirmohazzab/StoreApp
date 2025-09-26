using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StoreApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private ISender mediatr = null;

        protected ISender Mediator => mediatr ?? HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
