using Domain.Entities;
using Application.Contracts.Repositories;
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