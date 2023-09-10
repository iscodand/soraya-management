using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Contracts;
using SorayaManagement.Infrastructure.Data.DataContext;

namespace SorayaManagement.Infrastructure.Data.Repositories
{
    public class PaymentTypeRepository : GenericRepository<PaymentType>, IPaymentTypeRepository
    {
        public PaymentTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}