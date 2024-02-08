using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.Repositories
{
    public interface IUserRoleRepository
    {
        public Task<IdentityUserRole<string>> GetUserRoleAsync(string userId);
    }
}