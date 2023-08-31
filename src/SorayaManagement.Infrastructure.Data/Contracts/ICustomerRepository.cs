using SorayaManagement.Domain.Entities;

namespace SorayaManagement.Infrastructure.Data.Contracts
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public Task<ICollection<Customer>> GetCustomersByCompany(int companyId);
    }
}