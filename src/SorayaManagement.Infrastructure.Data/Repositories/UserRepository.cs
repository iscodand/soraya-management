using Microsoft.EntityFrameworkCore;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Contracts;
using SorayaManagement.Infrastructure.Data.DataContext;

namespace SorayaManagement.Infrastructure.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.AsNoTracking()
                                               .Include(x => x.UserCompany).AsNoTracking()
                                               .FirstOrDefaultAsync(x => x.NormalizedUserName == username.Trim().ToUpper());
        }
    }
}