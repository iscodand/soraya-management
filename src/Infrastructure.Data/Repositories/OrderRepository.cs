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

        public async Task<Order> GetOrderDetailsAsync(int orderId)
        {
            return await _orders
                   .Include(x => x.User).AsNoTracking()
                   .Include(x => x.Meal).AsNoTracking()
                   .Include(x => x.Customer).AsNoTracking()
                   .Include(x => x.PaymentType).AsNoTracking()
                   .Where(x => x.Id == orderId).AsNoTracking()
                   .FirstOrDefaultAsync()
                   .ConfigureAwait(false);
        }

        public async Task<ICollection<Order>> GetOrdersByCompanyAsync(int companyId)
        {
            return await _orders.AsNoTracking()
                                .Include(x => x.User).AsNoTracking()
                                .Include(x => x.Company).AsNoTracking()
                                .Include(x => x.Meal).AsNoTracking()
                                .Include(x => x.Customer).AsNoTracking()
                                .Include(x => x.PaymentType).AsNoTracking()
                                .Where(x => x.CompanyId == companyId).AsNoTracking()
                                .ToListAsync()
                                .ConfigureAwait(false);
        }

        public async Task<ICollection<Order>> GetOrdersByDateAsync(int companyId, DateTime? date)
        {
            return await _orders.AsNoTracking()
                                .Include(x => x.User).AsNoTracking()
                                .Include(x => x.Company).AsNoTracking()
                                .Include(x => x.Meal).AsNoTracking()
                                .Include(x => x.Customer).AsNoTracking()
                                .Include(x => x.PaymentType).AsNoTracking()
                                .Where(x => x.CreatedAt.Date == date.Value.Date)
                                .Where(x => x.CompanyId == companyId).AsNoTracking()
                                .ToListAsync()
                                .ConfigureAwait(false);
        }
    }
}