using StoreApp.Application.Contracts;
using StoreApp.Web.Extensions;

namespace StoreApp.Web.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string UserId { get => _contextAccessor?.HttpContext?.User.GetUserId() ?? string.Empty; }
        public string PhoneNumber { get => _contextAccessor?.HttpContext?.User.GetPhoneNumber() ?? string.Empty; }
    }
}
