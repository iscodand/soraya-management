using SorayaManagement.Domain.Entities;

namespace SorayaManagement.Infrastructure.Data.Contracts
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<ICollection<Order>> GetOrdersByCompanyAsync(int companyId);
    }
}