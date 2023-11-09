using Domain.Entities;
using Infrastructure.Data.Contracts;
using Infrastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly DbSet<Role> _roles;

        public RoleRepository(ApplicationDbContext context) : base(context)
        {
            _roles = context.Set<Role>();
        }

        public async Task<Role> GetRoleByIdAsync(string roleId)
        {
            return await _roles.AsNoTracking()
                               .Where(x => x.Id == roleId)
                               .FirstOrDefaultAsync()
                               .ConfigureAwait(false);
        }
    }
}