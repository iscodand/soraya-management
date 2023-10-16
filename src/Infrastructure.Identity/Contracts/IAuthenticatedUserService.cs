using Application.Dtos.User;

namespace Infrastructure.Identity.Contracts
{
    public interface IAuthenticatedUserService
    {
        public Task<GetAuthenticatedUserDto> GetAuthenticatedUserAsync();
    }
}