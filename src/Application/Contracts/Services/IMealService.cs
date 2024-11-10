using Application.Dtos.Meal;
<<<<<<< HEAD
=======
using Application.Parameters;
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
using Application.Wrappers;

namespace Application.Contracts.Services
{
    public interface IMealService
    {
        public Task<Response<CreateMealDto>> CreateMealAsync(CreateMealDto createMealDto);
        public Task<Response<IEnumerable<GetMealDto>>> GetMealsByCompanyAsync(int companyId);
<<<<<<< HEAD
=======
        public Task<PagedResponse<IEnumerable<GetMealDto>>> GetByCompanyPagedAsync(int companyId, RequestParameter parameter);
>>>>>>> 7c9e06914913873b4bb993389b5b4c0d7fb94650
        public Task<Response<GetMealDto>> GetMealByIdAsync(int mealId, int userCompanyId);
        public Task<Response<IEnumerable<GetMealDto>>> GetMealsByDateRangeAsync(int userCompanyId, DateTime initialDate, DateTime finalDate);
        public Task<Response<DetailMealDto>> DetailMealAsync(int mealId, int userCompanyId);
        public Task<Response<GetMealDto>> UpdateMealAsync(UpdateMealDto updateMealDto);
        public Task<Response<GetMealDto>> DeleteMealAsync(DeleteMealDto deleteMealDto);
    }
}