using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.Contracts;
using Infrastructure.Data.DataContext;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly DbSet<User> _users;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _users = context.Set<User>();
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _users.AsNoTracking()
                               .Include(x => x.UserCompany).AsNoTracking()
                               .FirstOrDefaultAsync(x => x.NormalizedUserName == username.Trim().ToUpper())
                               .ConfigureAwait(false);
        }

        public async Task<ICollection<User>> GetUsersByCompanyAsync(int companyId)
        {
            return await _users.AsNoTracking()
                               .Where(x => x.CompanyId == companyId).AsNoTracking()
                               .ToListAsync()
                               .ConfigureAwait(false);
        }

        public async Task<bool> UserExistsByEmailAsync(string email)
        {
            return await _users.AsNoTracking()
                               .AnyAsync(x => x.NormalizedEmail == email.Trim().ToUpper())
                               .ConfigureAwait(false);
        }

        public async Task<bool> UserExistsByUsernameAsync(string username)
        {
            return await _users.AsNoTracking()
                               .AnyAsync(x => x.NormalizedUserName == username.Trim().ToUpper())
                               .ConfigureAwait(false);
        }
    }
}