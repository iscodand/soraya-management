using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.Contracts;
using Infrastructure.Data.DataContext;

namespace Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _users;

        public UserRepository(ApplicationDbContext context)
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
    }
}