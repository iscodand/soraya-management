using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data.DataContext;

namespace Infrastructure.Data.Repositories
{
    public class MealRepository : GenericRepository<Meal>, IMealRepository
    {
        private readonly DbSet<Meal> _meals;

        public MealRepository(ApplicationDbContext context) : base(context)
        {
            _meals = context.Meals;
        }

        public async Task<ICollection<Meal>> GetMealsByCompanyAsync(int companyId)
        {
            return await _meals.AsNoTracking()
                               .Include(x => x.User).AsNoTracking()
                               .Include(x => x.Company).AsNoTracking()
                               .Where(x => x.CompanyId == companyId).AsNoTracking()
                               .ToListAsync()
                               .ConfigureAwait(false);
        }
    }
}