using Application.DTOs.Authentication;

namespace Application.Contracts
{
    public interface IAuthenticatedUserService
    {
        public Task<GetAuthenticatedUserDto> GetAuthenticatedUserAsync();
    }
}