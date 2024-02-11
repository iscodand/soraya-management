using Newtonsoft.Json;
using Application.DTOs.Authentication;
using Application.Contracts.Services;

namespace Presentation.Services
{
    public class SessionService : ISessionService
    {
        private readonly IHttpContextAccessor _httpContextAcessor;

        public SessionService(IHttpContextAccessor httpContextAcessor)
        {
            _httpContextAcessor = httpContextAcessor;
        }

        public void AddUserSession(GetAuthenticatedUserDto authenticatedUser)
        {
            string value = JsonConvert.SerializeObject(authenticatedUser);
            _httpContextAcessor.HttpContext.Session.SetString("AuthenticatedUserSession", value);
        }

        public GetAuthenticatedUserDto RetrieveUserSession()
        {
            string authenticatedUserSession = _httpContextAcessor.HttpContext.Session.GetString("AuthenticatedUserSession");

            if (string.IsNullOrEmpty(authenticatedUserSession))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<GetAuthenticatedUserDto>(authenticatedUserSession);
        }

        public void RemoveUserSession()
        {
            _httpContextAcessor.HttpContext.Session.Remove("AuthenticatedUserSession");
        }
    }
}