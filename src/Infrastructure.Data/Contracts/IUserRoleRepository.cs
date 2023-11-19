using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.Contracts
{
    public interface IUserRoleRepository
    {
        public Task<IdentityUserRole<string>> GetUserRoleAsync(string userId);
    }
}