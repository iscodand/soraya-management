using Domain.Entities;

namespace Infrastructure.Data.Contracts
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public Task<ICollection<Customer>> GetCustomersByCompanyAsync(int companyId);
        public Task<Customer> DetailCustomerAsync(int customerId);
    }
}