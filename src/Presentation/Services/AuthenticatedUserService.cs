using Application.Dtos.User;
using Domain.Entities;
using Infrastructure.Data.Contracts;
using Infrastructure.Identity.Contracts;

namespace Presentation.Services
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

        public async Task<GetAuthenticatedUserDto> GetAuthenticatedUserAsync()
        {
            string username = _httpContextAccessor.HttpContext.User.Identity.Name;
            User authenticatedUser = await _userRepository.GetUserByUsernameAsync(username);

            GetAuthenticatedUserDto getUserDto = new()
            {
                Id = authenticatedUser.Id,
                Name = authenticatedUser.Name,
                Email = authenticatedUser.Email,
                Username = authenticatedUser.Name,
                CompanyName = authenticatedUser.UserCompany.Name,
                CompanyId = authenticatedUser.CompanyId
            };

            return getUserDto;
        }
    }
}