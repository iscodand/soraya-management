using Newtonsoft.Json;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Identity.Contracts;

namespace SorayaManagement.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAcessor;

        public SessionService(IHttpContextAccessor httpContextAcessor)
        {
            _httpContextAcessor = httpContextAcessor;
        }

        public void AddUserSession(User authenticatedUser)
        {
            string value = JsonConvert.SerializeObject(authenticatedUser);
            _httpContextAcessor.HttpContext.Session.SetString("AuthenticatedUserSession", value);
        }

        public User RetrieveUserSession()
        {
            string authenticatedUserSession = _httpContextAcessor.HttpContext.Session.GetString("AuthenticatedUserSession");

            if (string.IsNullOrEmpty(authenticatedUserSession))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<User>(authenticatedUserSession);
        }

        public void RemoveUserSession()
        {
            _httpContextAcessor.HttpContext.Session.Remove("AuthenticatedUserSession");
        }
    }
}