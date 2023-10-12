using Application.Dtos.User;
using Domain.Entities;

namespace Infrastructure.Identity.Contracts
{
    public interface IAuthenticatedUserService
    {
        public Task<GetAuthenticatedUserDto> GetAuthenticatedUserAsync();
    }
}