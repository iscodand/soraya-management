using Microsoft.EntityFrameworkCore;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Contracts;
using SorayaManagement.Infrastructure.Data.DataContext;

namespace SorayaManagement.Infrastructure.Data.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DbSet<Customer> _customers;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _customers = context.Customers;
        }

        public async Task<ICollection<Customer>> GetCustomersByCompany(int companyId)
        {
            return await _customers.AsNoTracking()
                                                   .Include(x => x.User)
                                                   .ThenInclude(x => x.UserCompany).AsNoTracking()
                                                   .Where(x => x.User.CompanyId == companyId)
                                                   .ToListAsync();
        }
    }
}