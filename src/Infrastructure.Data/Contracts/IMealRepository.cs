using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.Contracts;

namespace SorayaManagement.Infrastructure.Data.Repositories
{
    public interface IMealRepository : IGenericRepository<Meal>
    {
        public Task<ICollection<Meal>> GetMealsByCompanyAsync(int companyId);
    }
}