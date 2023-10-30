using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.Contracts;
using Infrastructure.Data.DataContext;

namespace Infrastructure.Data.Repositories
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

        public async Task<ICollection<Order>> GetOrdersByDateRangeAsync(int companyId, DateTime? initialDate, DateTime? finalDate)
        {
            return await _orders.AsNoTracking()
                                .Include(x => x.Customer).AsNoTracking()
                                .Include(x => x.Meal).AsNoTracking()
                                .Include(x => x.PaymentType).AsNoTracking()
                                .Where(x => x.CreatedAt.Date >= initialDate.Value.Date && x.CreatedAt.Date <= finalDate.Value.Date).AsNoTracking()
                                .Where(x => x.CompanyId == companyId).AsNoTracking()
                                .ToListAsync()
                                .ConfigureAwait(false);
        }
    }
}