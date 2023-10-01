using Domain.Entities;
using Infrastructure.Data.Contracts;
using Infrastructure.Data.DataContext;

namespace Infrastructure.Data.Repositories
{
    public class PaymentTypeRepository : GenericRepository<PaymentType>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}