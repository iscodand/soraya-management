using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;

namespace SorayaManagement.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetAuthenticatedUserName()
        {
            string userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            return userName;
        }
    }
}