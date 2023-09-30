using Domain.Entities;
using Infrastructure.Data.Contracts;

namespace Infrastructure.Data.Repositories
{
    public interface IMealRepository : IGenericRepository<Meal>
    {
        public Task<ICollection<Meal>> GetMealsByCompanyAsync(int companyId);
    }
}