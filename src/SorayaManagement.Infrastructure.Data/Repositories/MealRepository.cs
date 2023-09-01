using Microsoft.EntityFrameworkCore;
using SorayaManagement.Domain.Entities;
using SorayaManagement.Infrastructure.Data.DataContext;

namespace SorayaManagement.Infrastructure.Data.Repositories
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
                               .Where(x => x.CompanyId == companyId)
                               .ToListAsync();
        }
    }
}