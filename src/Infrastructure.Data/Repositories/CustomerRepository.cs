using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.Contracts;
using Infrastructure.Data.DataContext;

namespace Infrastructure.Data.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly DbSet<Customer> _customers;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _customers = context.Customers;
        }
        public async Task<Customer> GetCustomerByIdAsync(int customerId)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.User).AsNoTracking()
                                   .Where(x => x.Id == customerId)
                                   .FirstOrDefaultAsync()
                                   .ConfigureAwait(false);
        }

        public async Task<Customer> DetailCustomerAsync(int customerId)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.User).AsNoTracking()
                                   .Include(x => x.Company).AsNoTracking()
                                   .Include(x => x.Orders)
                                   .ThenInclude(x => x.Meal).AsNoTracking()
                                   .Include(x => x.Orders)
                                   .ThenInclude(x => x.PaymentType).AsNoTracking()
                                   .Where(x => x.Id == customerId).AsNoTracking()
                                   .FirstOrDefaultAsync()
                                   .ConfigureAwait(false);
        }

        public async Task<ICollection<Customer>> GetCustomersByCompanyAsync(int companyId)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.User).AsNoTracking()
                                   .Include(x => x.Company).AsNoTracking()
                                   .Where(x => x.CompanyId == companyId).AsNoTracking()
                                   .ToListAsync()
                                   .ConfigureAwait(false);
        }

        public async Task<ICollection<Customer>> GetActiveCustomersByCompanyAsync(int companyId)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.User).AsNoTracking()
                                   .Include(x => x.Company).AsNoTracking()
                                   .Where(x => x.CompanyId == companyId).AsNoTracking()
                                   .Where(x => x.IsActive == true).AsNoTracking()
                                   .ToListAsync()
                                   .ConfigureAwait(false);
        }

        public async Task<ICollection<Customer>> GetInactiveCustomersByCompanyAsync(int companyId)
        {
            return await _customers.AsNoTracking()
                                   .Include(x => x.User).AsNoTracking()
                                   .Include(x => x.Company).AsNoTracking()
                                   .Where(x => x.CompanyId == companyId).AsNoTracking()
                                   .Where(x => x.IsActive == false).AsNoTracking()
                                   .ToListAsync()
                                   .ConfigureAwait(false);
        }

        public async Task<bool> CustomerExistsByCompanyAsync(string name, int companyId)
        {
            return await _customers.Where(x => x.CompanyId == companyId)
                                   .AnyAsync(x => x.Name == name)
                                   .ConfigureAwait(false);
        }
    }
}