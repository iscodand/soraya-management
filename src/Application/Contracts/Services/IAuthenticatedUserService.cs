using Application.DTOs.Authentication;

namespace Application.Contracts.Services
{
    public interface IAuthenticatedUserService
    {
        public Task<GetAuthenticatedUserDto> GetAuthenticatedUserAsync();
    }
}