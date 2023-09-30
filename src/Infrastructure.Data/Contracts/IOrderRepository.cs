using SorayaManagement.Domain.Entities;

namespace SorayaManagement.Infrastructure.Data.Contracts
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        public Task<Order> GetOrderDetailsAsync(int orderId);
        public Task<ICollection<Order>> GetOrdersByCompanyAsync(int companyId);
        public Task<ICollection<Order>> GetOrdersByDateAsync(int companyId, DateTime? date);
    }
}