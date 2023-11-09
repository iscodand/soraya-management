using Infrastructure.Data.Contracts;
using Infrastructure.Data.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly DbSet<IdentityUserRole<string>> _userRoles;

        public UserRoleRepository(ApplicationDbContext context)
        {
            _userRoles = context.Set<IdentityUserRole<string>>();
        }

        public async Task<IdentityUserRole<string>> GetUserRoleAsync(string userId)
        {
            return await _userRoles.AsNoTracking()
                                   .Where(x => x.UserId == userId)
                                   .FirstOrDefaultAsync()
                                   .ConfigureAwait(false);
        }
    }
}