using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Contracts;
using SorayaManagement.Infrastructure.Identity.Contracts;

namespace SorayaManagement.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        public async Task<User> GetAuthenticatedUserAsync()
        {
            string username = _httpContextAccessor.HttpContext.User.Identity.Name;
            User authenticatedUser = await _userRepository.GetUserByUsernameAsync(username);
            return authenticatedUser;
        }
    }
}