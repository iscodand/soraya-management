using Application.Dtos.Meal;
using Application.Responses;

namespace Application.Contracts.Services
{
    public interface IMealService
    {
        public Task<BaseResponse<CreateMealDto>> CreateMealAsync(CreateMealDto createMealDto);
        public Task<BaseResponse<GetMealDto>> GetMealsByCompanyAsync(int companyId);
        public Task<BaseResponse<GetMealDto>> GetMealByIdAsync(int mealId, int userCompanyId);
        public Task<BaseResponse<GetMealDto>> GetMealsByDateRangeAsync(int userCompanyId, DateTime initialDate, DateTime finalDate);
        public Task<BaseResponse<DetailMealDto>> DetailMealAsync(int mealId, int userCompanyId);
        public Task<BaseResponse<GetMealDto>> UpdateMealAsync(UpdateMealDto updateMealDto);
        public Task<BaseResponse<GetMealDto>> DeleteMealAsync(DeleteMealDto deleteMealDto);
    }
}