using Domain.Entities;

namespace Infrastructure.Data.Contracts
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public Task<ICollection<Customer>> GetCustomersByCompanyAsync(int companyId);
        public Task<ICollection<Customer>> GetActiveCustomersByCompanyAsync(int companyId);
        public Task<ICollection<Customer>> GetInactiveCustomersByCompanyAsync(int companyId);
        public Task<Customer> DetailCustomerAsync(int customerId);
        public Task<Customer> GetCustomerByIdAsync(int customerId);
        public Task<bool> CustomerExistsByNameAsync(string name);
    }
}