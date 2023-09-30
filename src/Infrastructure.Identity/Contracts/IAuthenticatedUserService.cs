using Domain.Entities;

namespace Infrastructure.Identity.Contracts
{
    public interface IAuthenticatedUserService
    {
        public Task<User> GetAuthenticatedUserAsync();
    }
}