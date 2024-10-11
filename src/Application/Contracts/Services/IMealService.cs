using Application.Dtos.Meal;
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface IMealService
    {
        public Task<Response<CreateMealDto>> CreateMealAsync(CreateMealDto createMealDto);
        public Task<Response<IEnumerable<GetMealDto>>> GetMealsByCompanyAsync(int companyId);
        public Task<Response<GetMealDto>> GetMealByIdAsync(int mealId, int userCompanyId);
        public Task<Response<IEnumerable<GetMealDto>>> GetMealsByDateRangeAsync(int userCompanyId, DateTime initialDate, DateTime finalDate);
        public Task<Response<DetailMealDto>> DetailMealAsync(int mealId, int userCompanyId);
        public Task<Response<GetMealDto>> UpdateMealAsync(UpdateMealDto updateMealDto);
        public Task<Response<GetMealDto>> DeleteMealAsync(DeleteMealDto deleteMealDto);
    }
}