using Microsoft.EntityFrameworkCore;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Contracts;
using SorayaManagement.Infrastructure.Data.DataContext;

namespace SorayaManagement.Infrastructure.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DbSet<Order> _orders;

        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _orders = context.Orders;
        }

        public async Task<ICollection<Order>> GetOrdersByCompanyAsync(int companyId)
        {
            return await _orders.AsNoTracking()
                               .Include(x => x.User).AsNoTracking()
                               .Include(x => x.Company).AsNoTracking()
                               .Where(x => x.CompanyId == companyId)
                               .ToListAsync();
        }
    }
}